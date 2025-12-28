using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.App_Start;
using WebApplication1.Models;

namespace WebApplication1.view.admin
{
    public partial class slider : Page
    {
        private const string HeroSlideFileKind = "hero-slide";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsAdmin())
            {
                Response.Redirect("~/view/admin/login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                DatabaseInitializer.EnsureSchema();
                BindCurrentSlidesPreview();
                LoadSlides();
                ResetForm();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DatabaseInitializer.EnsureSchema();

                string title = (txtTitle.Text ?? string.Empty).Trim();
                string subtitle = (txtSubtitle.Text ?? string.Empty).Trim();

                bool isEdit = int.TryParse(hfSlideId.Value, out int slideId);
                int sortOrder = 0;
                if (isEdit)
                {
                    int.TryParse((txtSortOrder.Text ?? "0").Trim(), out sortOrder);
                }

                int? imageFileId = null;
                if (fuImage != null && fuImage.HasFile)
                {
                    var storage = new FileStorageService(server: Server);
                    int? uploader = null;
                    if (Session["UserId"] != null)
                    {
                        uploader = Convert.ToInt32(Session["UserId"]);
                    }

                    imageFileId = storage.Save(new HttpPostedFileWrapper(fuImage.PostedFile), HeroSlideFileKind, uploader);
                }
                else if (!isEdit)
                {
                    ShowStatus("Выберите изображение.", isError: true);
                    return;
                }

                string cs = Functions.GetConnectionString();
                using (var conn = new SqlConnection(cs))
                {
                    conn.Open();
                    if (!isEdit)
                    {
                        using (var maxCmd = new SqlCommand("SELECT ISNULL(MAX(SortOrder), -1) FROM HeroSlider", conn))
                        {
                            sortOrder = Convert.ToInt32(maxCmd.ExecuteScalar()) + 1;
                        }

                        var cmd = new SqlCommand(@"
INSERT INTO HeroSlider (Title, Subtitle, ImageFileId, SortOrder, IsActive)
VALUES (@title, @subtitle, @imageFileId, @sortOrder, @isActive);", conn);

                        cmd.Parameters.AddWithValue("@title", (object)title ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@subtitle", (object)subtitle ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@imageFileId", (object)imageFileId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@sortOrder", sortOrder);
                        cmd.Parameters.AddWithValue("@isActive", chkActive.Checked);
                        cmd.ExecuteNonQuery();

                        ShowStatus("Слайд добавлен.", isError: false);
                    }
                    else
                    {
                        var cmd = new SqlCommand(@"
UPDATE HeroSlider
SET Title=@title,
    Subtitle=@subtitle,
    SortOrder=@sortOrder,
    IsActive=@isActive,
    ImageFileId = COALESCE(@imageFileId, ImageFileId)
WHERE SlideId=@id;", conn);

                        cmd.Parameters.AddWithValue("@id", slideId);
                        cmd.Parameters.AddWithValue("@title", (object)title ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@subtitle", (object)subtitle ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@sortOrder", sortOrder);
                        cmd.Parameters.AddWithValue("@isActive", chkActive.Checked);
                        cmd.Parameters.AddWithValue("@imageFileId", (object)imageFileId ?? DBNull.Value);
                        cmd.ExecuteNonQuery();

                        ShowStatus("Слайд обновлён.", isError: false);
                    }
                }

                LoadSlides();
                BindCurrentSlidesPreview();
                ResetForm();
            }
            catch (Exception ex)
            {
                ShowStatus("Ошибка сохранения: " + ex.Message, isError: true);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        protected void gvSlides_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "delete" && e.CommandName != "edit" && e.CommandName != "editSlide")
                return;

            if (!int.TryParse(e.CommandArgument.ToString(), out int rowIndex))
                return;

            int slideId = Convert.ToInt32(gvSlides.DataKeys[rowIndex].Value);

            if (e.CommandName == "delete")
            {
                DeleteSlide(slideId);
                LoadSlides();
                BindCurrentSlidesPreview();
                ResetForm();
                return;
            }

            if (e.CommandName == "edit" || e.CommandName == "editSlide")
            {
                LoadSlideIntoForm(slideId);
            }
        }

        protected void gvSlides_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            e.Cancel = true;
        }

        protected void gvSlides_RowEditing(object sender, GridViewEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void LoadSlides()
        {
            string cs = Functions.GetConnectionString();
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
SELECT SlideId, Title, Subtitle, ImageFileId, SortOrder, IsActive
FROM HeroSlider
ORDER BY SortOrder, SlideId;", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(reader);
                    table.Columns.Add("ImageUrl", typeof(string));

                    foreach (DataRow row in table.Rows)
                    {
                        object fileIdObj = row["ImageFileId"]; 
                        if (fileIdObj != DBNull.Value)
                        {
                            int fileId = Convert.ToInt32(fileIdObj);
                            row["ImageUrl"] = ResolveUrl("~/ImageHandler.ashx?id=" + fileId);
                        }
                        else
                        {
                            row["ImageUrl"] = ResolveUrl("~/assets/images/Слой 1.png");
                        }
                    }

                    gvSlides.DataSource = table;
                    gvSlides.DataBind();

                    if (repOrder != null)
                    {
                        repOrder.DataSource = table;
                        repOrder.DataBind();
                    }
                }
            }
        }

        private void BindCurrentSlidesPreview()
        {
            DatabaseInitializer.EnsureSchema();

            var slides = LoadActiveHeroSlides();

            if (lblCurrentSlidesHint != null)
            {
                lblCurrentSlidesHint.Visible = false;
                lblCurrentSlidesHint.Text = string.Empty;
            }

            repCurrentSlides.DataSource = slides;
            repCurrentSlides.DataBind();
        }

        private List<HeroSlidePreview> LoadActiveHeroSlides()
        {
            var result = new List<HeroSlidePreview>();
            string cs = Functions.GetConnectionString();

            using (var conn = new SqlConnection(cs))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
SELECT Title, Subtitle, ImageFileId
FROM HeroSlider
WHERE IsActive = 1
ORDER BY SortOrder, SlideId;", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int? fileId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2);
                        string imgUrl = fileId.HasValue
                            ? ResolveUrl("~/ImageHandler.ashx?id=" + fileId.Value)
                            : ResolveUrl("~/assets/images/Слой 1.png");

                        result.Add(new HeroSlidePreview
                        {
                            Title = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                            Subtitle = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            ImageUrl = imgUrl
                        });
                    }
                }
            }

            return result;
        }

        private class HeroSlidePreview
        {
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string ImageUrl { get; set; }
        }

        private void DeleteSlide(int slideId)
        {
            try
            {
                string cs = Functions.GetConnectionString();
                using (var conn = new SqlConnection(cs))
                {
                    conn.Open();
                    var cmd = new SqlCommand("DELETE FROM HeroSlider WHERE SlideId=@id", conn);
                    cmd.Parameters.AddWithValue("@id", slideId);
                    cmd.ExecuteNonQuery();
                }

                ShowStatus("Слайд удалён.", isError: false);
            }
            catch (Exception ex)
            {
                ShowStatus("Ошибка удаления: " + ex.Message, isError: true);
            }
        }

        private void LoadSlideIntoForm(int slideId)
        {
            string cs = Functions.GetConnectionString();
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
SELECT SlideId, Title, Subtitle, SortOrder, IsActive
FROM HeroSlider
WHERE SlideId=@id;", conn);
                cmd.Parameters.AddWithValue("@id", slideId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return;

                    hfSlideId.Value = reader.GetInt32(0).ToString();
                    txtTitle.Text = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                    txtSubtitle.Text = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                    txtSortOrder.Text = reader.GetInt32(3).ToString();
                    chkActive.Checked = !reader.IsDBNull(4) && reader.GetBoolean(4);
                }
            }

            btnSave.Text = "Обновить";
        }

        private void ResetForm()
        {
            hfSlideId.Value = string.Empty;
            txtTitle.Text = string.Empty;
            txtSubtitle.Text = string.Empty;
            txtSortOrder.Text = "0";
            chkActive.Checked = true;
            btnSave.Text = "Сохранить";
        }

        private bool IsAdmin()
        {
            return Session["UserEmail"] != null &&
                   Session["UserEmail"].ToString().Equals("admin@gmail.com", StringComparison.OrdinalIgnoreCase);
        }

        private void ShowStatus(string message, bool isError)
        {
            lblStatus.Visible = true;
            lblStatus.Text = message;
            lblStatus.CssClass = isError ? "text-danger fw-semibold" : "text-success fw-semibold";
        }

        protected void btnSaveOrder_Click(object sender, EventArgs e)
        {
            try
            {
                DatabaseInitializer.EnsureSchema();

                string value = (hfOrder?.Value ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(value))
                {
                    ShowStatus("Не удалось определить порядок.", isError: true);
                    return;
                }

                var ids = new List<int>();
                foreach (var part in value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (int.TryParse(part.Trim(), out int id))
                    {
                        ids.Add(id);
                    }
                }

                if (ids.Count == 0)
                {
                    ShowStatus("Не удалось определить порядок.", isError: true);
                    return;
                }

                string cs = Functions.GetConnectionString();
                using (var conn = new SqlConnection(cs))
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction())
                    {
                        for (int i = 0; i < ids.Count; i++)
                        {
                            using (var cmd = new SqlCommand("UPDATE HeroSlider SET SortOrder=@o WHERE SlideId=@id", conn, tx))
                            {
                                cmd.Parameters.AddWithValue("@o", i);
                                cmd.Parameters.AddWithValue("@id", ids[i]);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();
                    }
                }

                LoadSlides();
                BindCurrentSlidesPreview();
                ShowStatus("Порядок сохранён.", isError: false);
            }
            catch (Exception ex)
            {
                ShowStatus("Ошибка сохранения порядка: " + ex.Message, isError: true);
            }
        }
    }
}
