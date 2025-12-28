using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using ClosedXML.Excel;
using WebApplication1.Models;
using WebApplication1.App_Start;

namespace WebApplication1.view.admin
{
    public partial class cars : System.Web.UI.Page
    {
        Models.Functions Conn;
        private FileStorageService storage;
        private const string CarImageKind = "car-image";

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "text/html; charset=utf-8";
            Response.Charset = "utf-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.HeaderEncoding = Encoding.UTF8;

            Conn = new Models.Functions();
            storage = new FileStorageService(server: Server);

            if (!IsPostBack)
            {
                DatabaseInitializer.EnsureSchema();
                CarImageBootstrapper.EnsureSeeded(Server);
                
                ddlColor.Items.Clear();
                ddlColor.Items.Add(new ListItem("Выберите цвет", ""));
                ddlColor.Items.Add(new ListItem("Красный", "Red"));
                ddlColor.Items.Add(new ListItem("Синий", "Blue"));
                ddlColor.Items.Add(new ListItem("Зелёный", "Green"));
                ddlColor.Items.Add(new ListItem("Чёрный", "Black"));
                ddlColor.Items.Add(new ListItem("Белый", "White"));
                ddlColor.Items.Add(new ListItem("Жёлтый", "Yellow"));
                ddlColor.Items.Add(new ListItem("Серый", "Gray"));
                ddlColor.Items.Add(new ListItem("Оранжевый", "Orange"));
                ddlColor.Items.Add(new ListItem("Фиолетовый", "Purple"));
                
                LoadCategories();
                BindImageLibrary(null);
                LoadCars();
            }
        }

        private void LoadCars()
        {
            try
            {
                string query = @"SELECT c.CarId, c.CPlateNum, c.Brand, c.Model, c.Price, c.Color, c.Status, c.CategoryId,
                                        cc.Name AS CategoryName
                                 FROM CarTbl c
                                 LEFT JOIN CarCategory cc ON c.CategoryId = cc.CategoryId";
                carlist.DataSource = Conn.GetData(query);
                carlist.DataBind();
            }
            catch (Exception ex)
            {
                Notify("Ошибка загрузки автомобилей: " + ex.Message);
            }
        }

        private void LoadCategories()
        {
            string query = "SELECT CategoryId, Name FROM CarCategory ORDER BY MinExperienceYears";
            var dt = Conn.GetData(query);
            ddlCategory.DataSource = dt;
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("Выберите категорию", ""));
        }

        private void BindImageLibrary(List<int> selectedIds, int? mainId = null)
        {
            var files = storage.GetFiles(CarImageKind);
            var data = files.Select(f => new
            {
                f.FileId,
                f.OriginalName,
                PreviewUrl = ResolveUrl("~/ImageHandler.ashx?id=" + f.FileId)
            }).ToList();

            repImages.DataSource = data;
            repImages.DataBind();

            panelNoImages.Visible = !data.Any();

            foreach (RepeaterItem item in repImages.Items)
            {
                var hf = item.FindControl("hfFileId") as HiddenField;
                var rb = item.FindControl("rbMain") as RadioButton;
                var chk = item.FindControl("chkAttach") as CheckBox;
                if (hf == null) continue;

                if (int.TryParse(hf.Value, out int fileId))
                {
                    if (selectedIds != null && selectedIds.Contains(fileId))
                    {
                        if (chk != null) chk.Checked = true;
                    }
                    if (mainId.HasValue && mainId.Value == fileId)
                    {
                        if (rb != null) rb.Checked = true;
                        if (chk != null) chk.Checked = true;
                    }
                }
            }
        }

        private void GetSelectedImages(out int? mainImageId, out List<int> galleryImageIds)
        {
            mainImageId = null;
            galleryImageIds = new List<int>();

            foreach (RepeaterItem item in repImages.Items)
            {
                var hf = item.FindControl("hfFileId") as HiddenField;
                var rb = item.FindControl("rbMain") as RadioButton;
                var chk = item.FindControl("chkAttach") as CheckBox;

                if (hf == null) continue;

                if (int.TryParse(hf.Value, out int fileId))
                {
                    if (chk != null && chk.Checked)
                    {
                        galleryImageIds.Add(fileId);
                    }
                    if (rb != null && rb.Checked)
                    {
                        mainImageId = fileId;
                    }
                }
            }

            if (mainImageId.HasValue && !galleryImageIds.Contains(mainImageId.Value))
            {
                galleryImageIds.Insert(0, mainImageId.Value);
            }

            // Sync with UI
            if (mainImageId.HasValue)
            {
                imgSummary.InnerText = $"Выбрано: Главное + {galleryImageIds.Count - 1} галерея";
                imgSummary.Style["display"] = "inline-block";
            }
            else
            {
                imgSummary.InnerText = galleryImageIds.Count > 0 
                    ? $"Выбрано {galleryImageIds.Count} фото" 
                    : "";
                imgSummary.Style["display"] = galleryImageIds.Count > 0 ? "inline-block" : "none";
            }
        }

        protected void repImages_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteImage")
            {
                // Preserve selection if possible
                GetSelectedImages(out int? currentMain, out List<int> currentGallery);

                int fileId = Convert.ToInt32(e.CommandArgument);
                DeleteImage(fileId);

                // Remove deleted ID from preserved selection
                if (currentMain == fileId) currentMain = null;
                currentGallery.Remove(fileId);

                BindImageLibrary(currentGallery, currentMain);
            }
        }

        private void DeleteImage(int fileId)
        {
            string cs = Models.Functions.GetConnectionString();
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();
                // Get physical path using FileStorageService
                var storage = new FileStorageService(server: Server);
                var file = storage.GetFile(fileId);
                string path = file?.FilePath;

                var delRefs = new SqlCommand("DELETE FROM CarImages WHERE FileId=@id", conn);
                delRefs.Parameters.AddWithValue("@id", fileId);
                delRefs.ExecuteNonQuery();

                var del = new SqlCommand("DELETE FROM FileStorage WHERE FileId=@id", conn);
                del.Parameters.AddWithValue("@id", fileId);
                del.ExecuteNonQuery();

                // Delete physical file
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch
                    {
                        // Ignore deletion errors - file may already be deleted
                    }
                }
            }
        }

        private void UpsertCarImages(string plate, int? mainImageId, List<int> galleryImageIds)
        {
            string cs = Models.Functions.GetConnectionString();
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        var delCmd = new SqlCommand("DELETE FROM CarImages WHERE CarPlate = @plate", conn, tran);
                        delCmd.Parameters.AddWithValue("@plate", plate);
                        delCmd.ExecuteNonQuery();

                        if (galleryImageIds != null && galleryImageIds.Count > 0)
                        {
                            foreach (var fileId in galleryImageIds.Distinct())
                            {
                                var ins = new SqlCommand(@"INSERT INTO CarImages (CarPlate, FileId, IsPrimary) 
VALUES (@plate, @fileId, @isPrimary)", conn, tran);
                                ins.Parameters.AddWithValue("@plate", plate);
                                ins.Parameters.AddWithValue("@fileId", fileId);
                                ins.Parameters.AddWithValue("@isPrimary", mainImageId.HasValue && mainImageId.Value == fileId);
                                ins.ExecuteNonQuery();
                            }
                        }

                        var updateMain = new SqlCommand("UPDATE CarTbl SET MainImageFileId = @main WHERE CPlateNum = @plate", conn, tran);
                        updateMain.Parameters.AddWithValue("@main", (object)mainImageId ?? DBNull.Value);
                        updateMain.Parameters.AddWithValue("@plate", plate);
                        updateMain.ExecuteNonQuery();

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        private (List<int> galleryIds, int? mainId) LoadImageSelection(string plate)
        {
            var ids = new List<int>();
            int? mainId = null;
            string cs = Models.Functions.GetConnectionString();
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT FileId, IsPrimary FROM CarImages WHERE CarPlate = @plate", conn);
                cmd.Parameters.AddWithValue("@plate", plate);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int fileId = reader.GetInt32(0);
                        bool isPrimary = reader.GetBoolean(1);
                        ids.Add(fileId);
                        if (isPrimary)
                        {
                            mainId = fileId;
                        }
                    }
                }
            }

            return (ids, mainId);
        }

        private int GetOrCreateBrandId(string brand)
        {
            string cs = Models.Functions.GetConnectionString();
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();
                var get = new SqlCommand("SELECT BrandId FROM Brand WHERE Name = @name", conn);
                get.Parameters.AddWithValue("@name", brand);
                var existing = get.ExecuteScalar();
                if (existing != null) return Convert.ToInt32(existing);

                var ins = new SqlCommand("INSERT INTO Brand (Name) OUTPUT INSERTED.BrandId VALUES (@name)", conn);
                ins.Parameters.AddWithValue("@name", brand);
                return (int)ins.ExecuteScalar();
            }
        }

        private int GetOrCreateModelId(int brandId, string model)
        {
            string cs = Models.Functions.GetConnectionString();
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();
                var get = new SqlCommand("SELECT ModelId FROM CarModel WHERE BrandId=@brandId AND Name=@name", conn);
                get.Parameters.AddWithValue("@brandId", brandId);
                get.Parameters.AddWithValue("@name", model);
                var existing = get.ExecuteScalar();
                if (existing != null) return Convert.ToInt32(existing);

                var ins = new SqlCommand("INSERT INTO CarModel (BrandId, Name) OUTPUT INSERTED.ModelId VALUES (@brandId, @name)", conn);
                ins.Parameters.AddWithValue("@brandId", brandId);
                ins.Parameters.AddWithValue("@name", model);
                return (int)ins.ExecuteScalar();
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string constr = Models.Functions.GetConnectionString();
                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection(constr))
                {
                    // Выбираем только понятные пользователю данные
                    string query = @"SELECT c.CPlateNum as [Госномер], 
                                            c.Brand as [Марка], 
                                            c.Model as [Модель], 
                                            c.Price as [Цена в день ($)], 
                                            c.Color as [Цвет], 
                                            CASE WHEN c.Status = 'Available' THEN N'Свободен' ELSE N'Занят' END as [Статус]
                                     FROM CarTbl c";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                if (dt.Rows.Count == 0)
                {
                    ShowError("Нет данных для экспорта. Сначала добавьте хотя бы один автомобиль.");
                    return;
                }

                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("Автопарк");

                    // Загружаем данные
                    ws.Cell(1, 1).InsertTable(dt);

                    // Стилизация заголовков
                    var header = ws.Row(1);
                    header.Style.Font.Bold = true;
                    header.Style.Fill.BackgroundColor = XLColor.FromHtml("#1a202c");
                    header.Style.Font.FontColor = XLColor.White;

                    // Форматирование колонки цен
                    ws.Column(4).Style.NumberFormat.Format = "#,##0";

                    ws.Columns().AdjustToContents();

                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        byte[] bytes = stream.ToArray();

                        Response.Clear();
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", $"attachment;filename=Cars_Export_{DateTime.Now:yyyyMMdd}.xlsx");
                        Response.BinaryWrite(bytes);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Ошибка при экспорте: " + ex.Message);
            }
        }

        private void Notify(string message, bool isError = true)
        {
            ErrorMsg.InnerText = message;
            statusAlert.Attributes["class"] = isError ? "alert-toast show alert-danger-custom" : "alert-toast show alert-success-custom";
            statusIcon.Attributes["class"] = isError ? "fas fa-exclamation-circle" : "fas fa-check-circle";
        }

        private void ShowError(string message) => Notify(message, true);
        private void ShowSuccess(string message) => Notify(message, false);

        protected void btnUploadImage_Click(object sender, EventArgs e)
        {
            try
            {
                if (!fileUpload.HasFiles)
                {
                    ShowError("Выберите файлы для загрузки.");
                    return;
                }

                int? uploader = null;
                if (Session["UserId"] != null)
                {
                    uploader = Convert.ToInt32(Session["UserId"]);
                }

                int uploadCount = 0;
                foreach (HttpPostedFile postedFile in fileUpload.PostedFiles)
                {
                    if (postedFile.ContentLength > 0)
                    {
                        storage.Save(new HttpPostedFileWrapper(postedFile), CarImageKind, uploader);
                        uploadCount++;
                    }
                }

                List<int> selected = null;
                int? mainId = null;
                if (ViewState["SelectedCarKey"] != null)
                {
                    var res = LoadImageSelection(ViewState["SelectedCarKey"].ToString());
                    selected = res.galleryIds;
                    mainId = res.mainId;
                }

                BindImageLibrary(selected, mainId);
                ShowSuccess($"Загружено {uploadCount} фото на сервер.");
            }
            catch (Exception ex)
            {
                ShowError("Ошибка загрузки: " + ex.Message);
            }
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtLicence.Text) ||
                    string.IsNullOrWhiteSpace(txtBrand.Text) ||
                    string.IsNullOrWhiteSpace(txtModel.Text) ||
                    string.IsNullOrWhiteSpace(txtPrice.Text) ||
                    ddlColor.SelectedIndex <= 0)
                {
                    ShowError("Заполните все обязательные поля");
                    return;
                }

                string CPlateNum = txtLicence.Text.Trim().Replace("'", "''");
                string checkQuery = $"SELECT COUNT(*) AS Count FROM CarTbl WHERE CPlateNum = N'{CPlateNum}'";
                var result = Conn.GetData(checkQuery);

                if (Convert.ToInt32(result.Rows[0]["Count"]) > 0)
                {
                    ShowError("Автомобиль с таким номером уже существует");
                    return;
                }

                string Brand = txtBrand.Text.Trim();
                string Model = txtModel.Text.Trim();
                int? categoryId = string.IsNullOrEmpty(ddlCategory.SelectedValue) ? (int?)null : int.Parse(ddlCategory.SelectedValue);

                int brandId = GetOrCreateBrandId(Brand);
                int modelId = GetOrCreateModelId(brandId, Model);

                string cleanPrice = Regex.Replace(txtPrice.Text, @"[^\d]", "");
                if (!int.TryParse(cleanPrice, out int Price))
                {
                    ShowError("Некорректная цена");
                    return;
                }

                string Color = ddlColor.SelectedValue;
                string Status = ddlAvailable.SelectedValue == "1" ? "Available" : "Booked";

                GetSelectedImages(out int? mainImageId, out List<int> galleryImageIds);

                string cs = Models.Functions.GetConnectionString();
                using (var conn = new SqlConnection(cs))
                {
                    conn.Open();
                    var cmd = new SqlCommand(@"
INSERT INTO CarTbl (CPlateNum, Brand, Model, Price, Color, Status, BrandId, ModelId, CategoryId, MainImageFileId)
VALUES (@plate, @brand, @model, @price, @color, @status, @brandId, @modelId, @categoryId, @mainImageId)", conn);
                    cmd.Parameters.AddWithValue("@plate", CPlateNum);
                    cmd.Parameters.AddWithValue("@brand", Brand);
                    cmd.Parameters.AddWithValue("@model", Model);
                    cmd.Parameters.AddWithValue("@price", Price);
                    cmd.Parameters.AddWithValue("@color", Color);
                    cmd.Parameters.AddWithValue("@status", Status);
                    cmd.Parameters.AddWithValue("@brandId", brandId);
                    cmd.Parameters.AddWithValue("@modelId", modelId);
                    cmd.Parameters.AddWithValue("@categoryId", (object)categoryId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@mainImageId", (object)mainImageId ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }

                if (galleryImageIds.Any())
                {
                    UpsertCarImages(CPlateNum, mainImageId, galleryImageIds);
                }

                LoadCars();
                ClearFields();
                ShowSuccess("Автомобиль успешно добавлен");
            }
            catch (Exception ex)
            {
                ShowError("Ошибка при сохранении: " + ex.Message);
            }
        }

        protected void carlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (carlist.SelectedRow == null || carlist.SelectedDataKey == null) return;

                string plate = carlist.SelectedDataKey.Value.ToString();
                ViewState["SelectedCarKey"] = plate;

                string cs = Models.Functions.GetConnectionString();
                using (var conn = new SqlConnection(cs))
                {
                    conn.Open();
                    var cmd = new SqlCommand(@"SELECT Brand, Model, Price, Color, CategoryId, MainImageFileId 
FROM CarTbl WHERE CPlateNum = @plate", conn);
                    cmd.Parameters.AddWithValue("@plate", plate);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtLicence.Text = plate;
                            txtBrand.Text = reader["Brand"].ToString();
                            txtModel.Text = reader["Model"].ToString();
                            txtPrice.Text = reader["Price"].ToString();

                            string color = reader["Color"].ToString();
                            ListItem item = ddlColor.Items.Cast<ListItem>()
                                .FirstOrDefault(i => i.Value.Equals(color, StringComparison.OrdinalIgnoreCase));
                            if (item != null)
                            {
                                ddlColor.ClearSelection();
                                item.Selected = true;
                            }

                            string categoryId = reader["CategoryId"] == DBNull.Value ? "" : reader["CategoryId"].ToString();
                            if (!string.IsNullOrEmpty(categoryId) && ddlCategory.Items.FindByValue(categoryId) != null)
                            {
                                ddlCategory.SelectedValue = categoryId;
                            }

                            int? mainImageId = reader["MainImageFileId"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["MainImageFileId"]);
                            var (galleryIds, selectedMain) = LoadImageSelection(plate);
                            if (mainImageId == null) mainImageId = selectedMain;
                            BindImageLibrary(galleryIds, mainImageId);

                            if (mainImageId.HasValue)
                            {
                                imgSummary.InnerText = "Изображения загружены";
                            }
                            else
                            {
                                imgSummary.InnerText = "Нет фото";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Ошибка при выборе автомобиля: " + ex.Message);
            }
        }


        protected void Edit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["SelectedCarKey"] == null)
                {
                    ShowError("Выберите автомобиль для редактирования");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtLicence.Text) ||
                    string.IsNullOrWhiteSpace(txtBrand.Text) ||
                    string.IsNullOrWhiteSpace(txtPrice.Text) ||
                    ddlColor.SelectedIndex <= 0)
                {
                    ShowError("Заполните все обязательные поля");
                    return;
                }

                string originalPlate = ViewState["SelectedCarKey"].ToString().Replace("'", "''");
                string newPlate = txtLicence.Text.Trim().Replace("'", "''");
                string Brand = txtBrand.Text.Trim();
                string Model = txtModel.Text.Trim();

                int brandId = GetOrCreateBrandId(Brand);
                int modelId = GetOrCreateModelId(brandId, Model);
                int? categoryId = string.IsNullOrEmpty(ddlCategory.SelectedValue) ? (int?)null : int.Parse(ddlCategory.SelectedValue);

                string cleanPrice = Regex.Replace(txtPrice.Text, @"[^\d]", "");
                if (!int.TryParse(cleanPrice, out int Price))
                {
                    ShowError("Некорректная цена");
                    return;
                }

                string Color = ddlColor.SelectedValue;
                string Status = ddlAvailable.SelectedValue == "1" ? "Available" : "Booked";

                if (originalPlate != newPlate)
                {
                    string checkQuery = $"SELECT COUNT(*) AS Count FROM CarTbl WHERE CPlateNum = '{newPlate}'";
                    var result = Conn.GetData(checkQuery);
                    if (Convert.ToInt32(result.Rows[0]["Count"]) > 0)
                    {
                        ShowError("Автомобиль с таким номером уже существует");
                        return;
                    }
                }

                GetSelectedImages(out int? mainImageId, out List<int> galleryImageIds);

                string cs = Models.Functions.GetConnectionString();
                using (var conn = new SqlConnection(cs))
                {
                    conn.Open();
                    var cmd = new SqlCommand(@"
UPDATE CarTbl 
SET CPlateNum=@plate, Brand=@brand, Model=@model, Price=@price, Color=@color, Status=@status,
    BrandId=@brandId, ModelId=@modelId, CategoryId=@categoryId, MainImageFileId=@mainImageId
WHERE CPlateNum=@originalPlate", conn);
                    cmd.Parameters.AddWithValue("@plate", newPlate);
                    cmd.Parameters.AddWithValue("@brand", Brand);
                    cmd.Parameters.AddWithValue("@model", Model);
                    cmd.Parameters.AddWithValue("@price", Price);
                    cmd.Parameters.AddWithValue("@color", Color);
                    cmd.Parameters.AddWithValue("@status", Status);
                    cmd.Parameters.AddWithValue("@brandId", brandId);
                    cmd.Parameters.AddWithValue("@modelId", modelId);
                    cmd.Parameters.AddWithValue("@categoryId", (object)categoryId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@mainImageId", (object)mainImageId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@originalPlate", originalPlate);
                    cmd.ExecuteNonQuery();
                }

                UpsertCarImages(newPlate, mainImageId, galleryImageIds);
                LoadCars();
                ClearFields();
                ShowSuccess("Автомобиль успешно обновлён");
            }
            catch (Exception ex)
            {
                ShowError("Ошибка при редактировании: " + ex.Message);
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["SelectedCarKey"] == null)
                {
                    ShowError("Выберите автомобиль для удаления");
                    return;
                }

                string CPlateNum = ViewState["SelectedCarKey"].ToString().Replace("'", "''");
                string cs = Models.Functions.GetConnectionString();
                using (var conn = new SqlConnection(cs))
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            var delImgs = new SqlCommand("DELETE FROM CarImages WHERE CarPlate=@plate", conn, tran);
                            delImgs.Parameters.AddWithValue("@plate", CPlateNum);
                            delImgs.ExecuteNonQuery();

                            var delCar = new SqlCommand("DELETE FROM CarTbl WHERE CPlateNum=@plate", conn, tran);
                            delCar.Parameters.AddWithValue("@plate", CPlateNum);
                            delCar.ExecuteNonQuery();

                            tran.Commit();
                        }
                        catch
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                }
                LoadCars();
                ClearFields();
                ShowSuccess("Автомобиль успешно удалён");
            }
            catch (Exception ex)
            {
                ShowError("Ошибка при удалении: " + ex.Message);
            }
        }

        private void ClearFields()
        {
            txtLicence.Text = "";
            txtBrand.Text = "";
            txtModel.Text = "";
            txtPrice.Text = "";
            ddlColor.SelectedIndex = 0;
            ddlAvailable.SelectedIndex = 0;
            ddlCategory.SelectedIndex = 0;
            ViewState["SelectedCarKey"] = null;
            carlist.SelectedIndex = -1;
            imgSummary.InnerText = "Управление изображениями";
            statusAlert.Attributes["class"] = "alert-toast";
            BindImageLibrary(null, null);
        }
    }
}
