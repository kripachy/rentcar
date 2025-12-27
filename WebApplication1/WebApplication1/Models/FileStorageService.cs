using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Web;

namespace WebApplication1.Models
{
    /// <summary>
    /// File storage that keeps binaries in ~/uploads/ folder inside the project (accessible via HTTP)
    /// and stores metadata in the FileStorage table. Stores relative paths in database for portability.
    /// </summary>
    public class FileStorageService
    {
        private readonly string _rootPath;
        private readonly string _connectionString;
        private readonly HttpServerUtility _server;

        public FileStorageService(string rootPath = null, HttpServerUtility server = null)
        {
            _server = server ?? HttpContext.Current?.Server;
            if (_server == null)
            {
                throw new InvalidOperationException("HttpServerUtility is required. Use HttpContext.Current.Server.");
            }

            if (string.IsNullOrWhiteSpace(rootPath))
            {
                // Store files in ~/uploads/ folder inside the project
                _rootPath = _server.MapPath("~/uploads");
            }
            else
            {
                _rootPath = rootPath;
            }

            Directory.CreateDirectory(_rootPath);
            _connectionString = Functions.GetConnectionString();
        }

        public string RootPath => _rootPath;

        /// <summary>
        /// Gets the relative path from site root (e.g., "uploads/car-image/filename.jpg")
        /// </summary>
        private string GetRelativePath(string physicalPath)
        {
            string siteRoot = _server.MapPath("~/");
            if (physicalPath.StartsWith(siteRoot, StringComparison.OrdinalIgnoreCase))
            {
                return physicalPath.Substring(siteRoot.Length).Replace('\\', '/');
            }
            return physicalPath;
        }

        /// <summary>
        /// Gets the physical path from relative path or absolute path (for backward compatibility)
        /// </summary>
        public string GetPhysicalPath(string storedPath)
        {
            if (string.IsNullOrEmpty(storedPath))
                return null;

            // If it's a relative path, map it to a physical path. This is the standard.
            if (!Path.IsPathRooted(storedPath))
            {
                return _server.MapPath("~/" + storedPath.Replace('\\', '/'));
            }

            // --- Handle absolute paths (for backward compatibility and recovery) ---

            // 1. Check if the absolute path is valid as-is.
            if (File.Exists(storedPath))
            {
                return storedPath;
            }

            // 2. If not valid, it's likely from a different environment. Try to recover the correct path.
            // We assume the structure is '.../uploads/file-kind/filename.ext'.
            // We'll find the 'uploads' folder in the path and use the part that comes after it.
            string uploadsMarker = "\\uploads\\";
            int uploadsIndex = storedPath.LastIndexOf(uploadsMarker, StringComparison.OrdinalIgnoreCase);
            if (uploadsIndex == -1)
            {
                uploadsMarker = "/uploads/";
                uploadsIndex = storedPath.LastIndexOf(uploadsMarker, StringComparison.OrdinalIgnoreCase);
            }

            if (uploadsIndex != -1)
            {
                // Get the part of the path *after* the ".../uploads/" marker.
                int relativePathStartIndex = uploadsIndex + uploadsMarker.Length;
                if (relativePathStartIndex < storedPath.Length)
                {
                    string pathWithinUploads = storedPath.Substring(relativePathStartIndex);
                    // Combine it with the current environment's physical uploads folder path.
                    string reconstructedPath = Path.Combine(_rootPath, pathWithinUploads.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar));
                    if (File.Exists(reconstructedPath))
                    {
                        return reconstructedPath;
                    }
                }
            }

            // 3. Fallback: if 'uploads' marker is not found, try to guess based on the last two parts of the path.
            try
            {
                string filename = Path.GetFileName(storedPath);
                string directory = Path.GetFileName(Path.GetDirectoryName(storedPath)); // Assumes 'file-kind'
                if (!string.IsNullOrEmpty(filename) && !string.IsNullOrEmpty(directory))
                {
                    // Combine with the root path for uploads, e.g., '.../uploads' + 'car-image' + 'file.jpg'
                    string reconstructedPath = Path.Combine(_rootPath, directory, filename);
                    if (File.Exists(reconstructedPath))
                    {
                        return reconstructedPath;
                    }
                }
            }
            catch (ArgumentException)
            {
                // Path.Get... methods can throw if the path contains invalid characters. Ignore.
            }

            // 4. If all recovery attempts fail, return the original (broken) path.
            // The calling code (e.g., ImageHandler) will then handle the "file not found" case.
            return storedPath;
        }

        public int Save(HttpPostedFileBase file, string fileKind, int? userId)
        {
            if (file == null || file.ContentLength == 0)
            {
                throw new InvalidOperationException("Файл не загружен или пустой.");
            }

            string originalFileName = Path.GetFileName(file.FileName);
            string extension = Path.GetExtension(originalFileName);
            string baseName = Path.GetFileNameWithoutExtension(originalFileName);
            
            // Clean filename from invalid characters
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                baseName = baseName.Replace(c, '_');
            }

            string relativeFolder = fileKind ?? "misc";
            string folderPath = Path.Combine(_rootPath, relativeFolder);
            Directory.CreateDirectory(folderPath);

            string fileName = baseName + extension;
            string fullPath = Path.Combine(folderPath, fileName);

            // Handle collisions by adding (1), (2), etc.
            int counter = 1;
            while (File.Exists(fullPath))
            {
                fileName = $"{baseName}({counter}){extension}";
                fullPath = Path.Combine(folderPath, fileName);
                counter++;
            }

            file.SaveAs(fullPath);

            // Store relative path in database for portability
            string relativePath = GetRelativePath(fullPath);

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
INSERT INTO FileStorage (FilePath, OriginalName, ContentType, FileKind, UploadedBy)
OUTPUT INSERTED.FileId
VALUES (@path, @name, @type, @kind, @userId);", conn);

                cmd.Parameters.AddWithValue("@path", relativePath);
                cmd.Parameters.AddWithValue("@name", Path.GetFileName(file.FileName));
                cmd.Parameters.AddWithValue("@type", file.ContentType ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@kind", fileKind ?? "misc");
                cmd.Parameters.AddWithValue("@userId", (object)userId ?? DBNull.Value);

                return (int)cmd.ExecuteScalar();
            }
        }

        public IEnumerable<StoredFile> GetFiles(string fileKind)
        {
            var result = new List<StoredFile>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
SELECT FileId, FilePath, OriginalName, ContentType, FileKind, UploadedBy, CreatedAt
FROM FileStorage
WHERE FileKind = @kind
ORDER BY CreatedAt DESC", conn);
                cmd.Parameters.AddWithValue("@kind", fileKind ?? "misc");

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string storedPath = reader.GetString(1);
                        // Convert to physical path for compatibility
                        string physicalPath = GetPhysicalPath(storedPath);

                        result.Add(new StoredFile
                        {
                            FileId = reader.GetInt32(0),
                            FilePath = physicalPath, // Return physical path for existing code
                            OriginalName = reader.GetString(2),
                            ContentType = reader.IsDBNull(3) ? null : reader.GetString(3),
                            FileKind = reader.GetString(4),
                            UploadedBy = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                            CreatedAt = reader.GetDateTime(6)
                        });
                    }
                }
            }

            return result;
        }

        public StoredFile GetFile(int fileId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
SELECT FileId, FilePath, OriginalName, ContentType, FileKind, UploadedBy, CreatedAt
FROM FileStorage
WHERE FileId = @id", conn);
                cmd.Parameters.AddWithValue("@id", fileId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return null;

                    string storedPath = reader.GetString(1);
                    // GetFile returns the physical path for compatibility
                    string physicalPath = GetPhysicalPath(storedPath);

                    return new StoredFile
                    {
                        FileId = reader.GetInt32(0),
                        FilePath = physicalPath, // Return physical path for existing code compatibility
                        OriginalName = reader.GetString(2),
                        ContentType = reader.IsDBNull(3) ? null : reader.GetString(3),
                        FileKind = reader.GetString(4),
                        UploadedBy = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                        CreatedAt = reader.GetDateTime(6)
                    };
                }
            }
        }
    }

    public class StoredFile
    {
        public int FileId { get; set; }
        public string FilePath { get; set; }
        public string OriginalName { get; set; }
        public string ContentType { get; set; }
        public string FileKind { get; set; }
        public int? UploadedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

