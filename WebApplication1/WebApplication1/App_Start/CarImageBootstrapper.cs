using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using WebApplication1.Models;

namespace WebApplication1.App_Start
{
    public static class CarImageBootstrapper
    {
        private const string CarImageKind = "car-image";
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        public static void EnsureSeeded(HttpServerUtility server)
        {
            // Отключено по запросу пользователя для ручного управления библиотекой
            // ImportLegacyImages(storage, server);
            // AssignMissingCarImages(storage, server);
            
            // Оставляем только очистку записей, для которых нет физических файлов
            CleanupOrphanRecords(server);
        }

        public static string BuildImageUrl(Page page, string plate, string brand, string model, string color = "", IDictionary<string, int?> cache = null, int? explicitFileId = null)
        {
            CarImageData imageData = new CarImageData
            {
                Plate = plate,
                Brand = brand,
                Model = model,
                Color = color,
                CachedMainId = TryGetFromCache(cache, brand, model),
                ExplicitFileId = explicitFileId
            };

            int? fileId = imageData.ExplicitFileId
                          ?? imageData.CachedMainId
                          ?? LookupMainImageIdByPlate(imageData.Plate)
                          ?? LookupMainImageIdByBrandModel(imageData.Brand, imageData.Model)
                          ?? FindBestFileId(imageData.Brand, imageData.Model, imageData.Color);

            if (fileId.HasValue)
            {
                return page.ResolveUrl("~/ImageHandler.ashx?id=" + fileId.Value);
            }

            return page.ResolveUrl("~/assets/images/default-car.png.png");
        }

        public static List<string> GetLatestImageUrls(Page page, int take)
        {
            var storage = new FileStorageService(server: page.Server);
            return storage.GetFiles(CarImageKind)
                          .Take(take)
                          .Select(f => page.ResolveUrl("~/ImageHandler.ashx?id=" + f.FileId))
                          .ToList();
        }

        public static Dictionary<string, int?> GetMainImageLookup()
        {
            var map = new Dictionary<string, int?>(StringComparer.OrdinalIgnoreCase);
            using (var conn = new SqlConnection(Functions.GetConnectionString()))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT Brand, Model, MainImageFileId FROM CarTbl", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string brand = reader["Brand"]?.ToString() ?? string.Empty;
                        string model = reader["Model"]?.ToString() ?? string.Empty;
                        int? fileId = reader["MainImageFileId"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["MainImageFileId"]);
                        string key = $"{brand}|{model}";
                        if (!map.ContainsKey(key))
                        {
                            map[key] = fileId;
                        }
                    }
                }
            }

            return map;
        }

        public static List<string> GetGalleryUrls(Page page, string brand, string model, int take = 5)
        {
            var urls = new List<string>();
            using (var conn = new SqlConnection(Functions.GetConnectionString()))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
SELECT TOP (@take) ci.FileId
FROM CarTbl c
JOIN CarImages ci ON ci.CarPlate = c.CPlateNum
WHERE c.Brand = @brand AND c.Model = @model
ORDER BY ci.IsPrimary DESC, ci.CarImageId DESC", conn);
                cmd.Parameters.AddWithValue("@brand", brand ?? string.Empty);
                cmd.Parameters.AddWithValue("@model", model ?? string.Empty);
                cmd.Parameters.AddWithValue("@take", take);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int fileId = reader.GetInt32(0);
                        urls.Add(page.ResolveUrl("~/ImageHandler.ashx?id=" + fileId));
                    }
                }
            }

            if (!urls.Any())
            {
                urls = GetLatestImageUrls(page, take);
            }

            if (!urls.Any())
            {
                urls.Add(page.ResolveUrl("~/assets/images/default-car.png.png"));
            }

            return urls;
        }

        private static void ImportLegacyImages(FileStorageService storage, HttpServerUtility server)
        {
            var roots = new[]
            {
                server.MapPath("~/colorcars"),
                server.MapPath("~/assets/images")
            };

            CleanupOrphanRecords(server);

            var existingFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            using (var conn = new SqlConnection(Functions.GetConnectionString()))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT OriginalName FROM FileStorage", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader["OriginalName"]?.ToString();
                        if (!string.IsNullOrEmpty(name)) existingFiles.Add(name);
                    }
                }
            }

            foreach (var root in roots)
            {
                if (!Directory.Exists(root)) continue;

                foreach (var file in Directory.GetFiles(root, "*.*", SearchOption.AllDirectories))
                {
                    string originalName = Path.GetFileName(file);
                    if (existingFiles.Contains(originalName)) continue;

                    string ext = Path.GetExtension(file)?.ToLowerInvariant();
                    if (!AllowedExtensions.Contains(ext)) continue;

                    try
                    {
                        using (var fs = File.OpenRead(file))
                        {
                            var posted = new LegacyPostedFile(file, fs, MimeMapping.GetMimeMapping(file));
                            storage.Save(posted, CarImageKind, null);
                        }
                    }
                    catch
                    {
                    }
                }
            }

            RegisterUploads(storage, server);
        }

        private static void RegisterUploads(FileStorageService storage, HttpServerUtility server)
        {
            string uploadsPath = server.MapPath("~/uploads/car-image");
            if (!Directory.Exists(uploadsPath)) return;

            var existingPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            using (var conn = new SqlConnection(Functions.GetConnectionString()))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT FilePath FROM FileStorage", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) existingPaths.Add(reader.GetString(0).Replace('\\', '/'));
                }

                foreach (var file in Directory.GetFiles(uploadsPath, "*.*", SearchOption.AllDirectories))
                {
                    string ext = Path.GetExtension(file).ToLowerInvariant();
                    if (!AllowedExtensions.Contains(ext)) continue;

                    string siteRoot = server.MapPath("~/");
                    string relativePath = file.Substring(siteRoot.Length).Replace('\\', '/');

                    if (existingPaths.Contains(relativePath)) continue;

                    var ins = new SqlCommand(@"
INSERT INTO FileStorage (FilePath, OriginalName, ContentType, FileKind, CreatedAt)
VALUES (@path, @name, @type, @kind, @date)", conn);
                    ins.Parameters.AddWithValue("@path", relativePath);
                    ins.Parameters.AddWithValue("@name", Path.GetFileName(file));
                    ins.Parameters.AddWithValue("@type", MimeMapping.GetMimeMapping(file));
                    ins.Parameters.AddWithValue("@kind", CarImageKind);
                    ins.Parameters.AddWithValue("@date", DateTime.Now);
                    ins.ExecuteNonQuery();
                }
            }
        }

        private static void CleanupOrphanRecords(HttpServerUtility server)
        {
            var storage = new FileStorageService(server: server);
            using (var conn = new SqlConnection(Functions.GetConnectionString()))
            {
                conn.Open();
                var orphans = new List<int>();
                var cmd = new SqlCommand("SELECT FileId, FilePath FROM FileStorage", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string path = storage.GetPhysicalPath(reader.GetString(1));
                        if (!File.Exists(path))
                        {
                            orphans.Add(id);
                        }
                    }
                }

                if (orphans.Any())
                {
                    foreach (int id in orphans)
                    {
                        var delRefs = new SqlCommand("DELETE FROM CarImages WHERE FileId=@id; DELETE FROM FileStorage WHERE FileId=@id;", conn);
                        delRefs.Parameters.AddWithValue("@id", id);
                        delRefs.ExecuteNonQuery();
                    }
                }
            }
        }

        private static void AssignMissingCarImages(FileStorageService storage, HttpServerUtility server)
        {
            var files = storage.GetFiles(CarImageKind).ToList();
            if (!files.Any()) return;

            using (var conn = new SqlConnection(Functions.GetConnectionString()))
            {
                conn.Open();
                var cars = new List<(string plate, string brand, string model, string color, int? mainId)>();
                using (var cmd = new SqlCommand("SELECT CPlateNum, Brand, Model, Color, MainImageFileId FROM CarTbl", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cars.Add((
                            reader["CPlateNum"].ToString(),
                            reader["Brand"].ToString(),
                            reader["Model"].ToString(),
                            reader["Color"]?.ToString() ?? "",
                            reader["MainImageFileId"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["MainImageFileId"])
                        ));
                    }
                }

                foreach (var car in cars)
                {
                    var match = FindBestFile(files, car.brand, car.model, car.color);
                    if (match == null) continue;
                    
                    if (car.mainId == match.FileId) continue;

                    var update = new SqlCommand("UPDATE CarTbl SET MainImageFileId=@file WHERE CPlateNum=@plate", conn);
                    update.Parameters.AddWithValue("@file", match.FileId);
                    update.Parameters.AddWithValue("@plate", car.plate);
                    update.ExecuteNonQuery();

                    var resetPrimary = new SqlCommand("UPDATE CarImages SET IsPrimary=0 WHERE CarPlate=@plate", conn);
                    resetPrimary.Parameters.AddWithValue("@plate", car.plate);
                    resetPrimary.ExecuteNonQuery();

                    var insert = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM CarImages WHERE CarPlate=@plate AND FileId=@file)
    UPDATE CarImages SET IsPrimary=1 WHERE CarPlate=@plate AND FileId=@file
ELSE
    INSERT INTO CarImages (CarPlate, FileId, IsPrimary) VALUES (@plate, @file, 1)", conn);
                    insert.Parameters.AddWithValue("@plate", car.plate);
                    insert.Parameters.AddWithValue("@file", match.FileId);
                    insert.ExecuteNonQuery();
                }
            }
        }

        private static StoredFile FindBestFile(IEnumerable<StoredFile> files, string brand, string model, string color = "")
        {
            if (string.IsNullOrWhiteSpace(brand)) return null;

            var candidates = files.Where(f => 
                !(f.OriginalName?.IndexOf("logo", StringComparison.OrdinalIgnoreCase) >= 0) &&
                !(f.FilePath?.IndexOf("logo", StringComparison.OrdinalIgnoreCase) >= 0)
            ).ToList();

            if (!string.IsNullOrWhiteSpace(color))
            {
                var matchByColor = candidates.FirstOrDefault(f =>
                    f.OriginalName?.IndexOf(brand, StringComparison.OrdinalIgnoreCase) >= 0 &&
                    f.OriginalName?.IndexOf(color, StringComparison.OrdinalIgnoreCase) >= 0);
                if (matchByColor != null) return matchByColor;
            }

            var match = candidates.FirstOrDefault(f =>
                f.FilePath?.IndexOf("colorcars", StringComparison.OrdinalIgnoreCase) >= 0 &&
                f.FilePath?.IndexOf(brand, StringComparison.OrdinalIgnoreCase) >= 0 &&
                (!string.IsNullOrWhiteSpace(model) && f.FilePath?.IndexOf(model, StringComparison.OrdinalIgnoreCase) >= 0));
            if (match != null) return match;

            match = candidates.FirstOrDefault(f =>
                (f.OriginalName?.IndexOf(brand, StringComparison.OrdinalIgnoreCase) >= 0 || f.FilePath?.IndexOf(brand, StringComparison.OrdinalIgnoreCase) >= 0) &&
                (!string.IsNullOrWhiteSpace(model) && (f.OriginalName?.IndexOf(model, StringComparison.OrdinalIgnoreCase) >= 0 || f.FilePath?.IndexOf(model, StringComparison.OrdinalIgnoreCase) >= 0)));
            if (match != null) return match;

            match = candidates.FirstOrDefault(f =>
                f.FilePath?.IndexOf("colorcars", StringComparison.OrdinalIgnoreCase) >= 0 &&
                f.FilePath?.IndexOf(brand, StringComparison.OrdinalIgnoreCase) >= 0);
            if (match != null) return match;

            return candidates.FirstOrDefault(f =>
                f.OriginalName?.IndexOf(brand, StringComparison.OrdinalIgnoreCase) >= 0 || 
                f.FilePath?.IndexOf(brand, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private static int? FindBestFileId(string brand, string model, string color = "")
        {
            if (HttpContext.Current == null) return null;
            var storage = new FileStorageService(server: HttpContext.Current.Server);
            var file = FindBestFile(storage.GetFiles(CarImageKind), brand, model, color);
            return file?.FileId;
        }

        private static int? LookupMainImageIdByPlate(string plate)
        {
            if (string.IsNullOrWhiteSpace(plate)) return null;

            using (var conn = new SqlConnection(Functions.GetConnectionString()))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT MainImageFileId FROM CarTbl WHERE CPlateNum=@plate", conn);
                cmd.Parameters.AddWithValue("@plate", plate);
                var result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? (int?)null : Convert.ToInt32(result);
            }
        }

        private static int? LookupMainImageIdByBrandModel(string brand, string model)
        {
            if (string.IsNullOrWhiteSpace(brand) || string.IsNullOrWhiteSpace(model)) return null;

            using (var conn = new SqlConnection(Functions.GetConnectionString()))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT TOP 1 MainImageFileId FROM CarTbl WHERE Brand=@b AND Model=@m AND MainImageFileId IS NOT NULL", conn);
                cmd.Parameters.AddWithValue("@b", brand);
                cmd.Parameters.AddWithValue("@m", model);
                var result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? (int?)null : Convert.ToInt32(result);
            }
        }

        private static int? TryGetFromCache(IDictionary<string, int?> cache, string brand, string model)
        {
            if (cache == null) return null;
            string key = $"{brand}|{model}";
            return cache.TryGetValue(key, out var value) ? value : null;
        }

        private class CarImageData
        {
            public string Plate { get; set; }
            public string Brand { get; set; }
            public string Model { get; set; }
            public string Color { get; set; }
            public int? CachedMainId { get; set; }
            public int? ExplicitFileId { get; set; }
        }

        private class LegacyPostedFile : HttpPostedFileBase
        {
            private readonly string _fileName;
            private readonly Stream _stream;
            private readonly string _contentType;

            public LegacyPostedFile(string fileName, Stream stream, string contentType)
            {
                _fileName = fileName;
                _stream = stream;
                _contentType = contentType;
            }

            public override int ContentLength => (int)_stream.Length;
            public override string FileName => Path.GetFileName(_fileName);
            public override Stream InputStream => _stream;
            public override string ContentType => _contentType;

            public override void SaveAs(string filename)
            {
                _stream.Position = 0;
                using (var file = File.Create(filename))
                {
                    _stream.CopyTo(file);
                }
            }
        }
    }
}

