using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using WebApplication1.App_Start;

namespace WebApplication1.view.admin
{
    public partial class userdashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "text/html; charset=utf-8";
            Response.Charset = "utf-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.HeaderEncoding = Encoding.UTF8;

            if (!IsPostBack)
            {
                CarImageBootstrapper.EnsureSeeded(Server);
                ConfigureSliderImages();
                EnsureSchema();
                ConfigureCommentUi();
                LoadComments();
            }
        }

        private void ConfigureCommentUi()
        {
            bool isLoggedIn = Session["UserId"] != null;

            pnlLoginRequired.Visible = !isLoggedIn;
            pnlLoginRequired.CssClass = pnlLoginRequired.Visible ? "alert alert-dark" : "alert alert-dark d-none";

            if (!isLoggedIn)
            {
                pnlCommentForm.Visible = false;
                pnlUsernameRequired.Visible = false;
                return;
            }

            string userName = GetCurrentUserName();
            lblCurrentUserName.Text = string.IsNullOrWhiteSpace(userName) ? "Заполните ник в профиле" : userName;

            bool hasUserName = !string.IsNullOrWhiteSpace(userName);
            pnlUsernameRequired.Visible = !hasUserName;
            pnlUsernameRequired.CssClass = pnlUsernameRequired.Visible ? "alert alert-info" : "alert alert-info d-none";

            pnlCommentForm.Visible = hasUserName;
        }

        private void ConfigureSliderImages()
        {
            var urls = GetStaticHeroImages();
            if (urls.Count == 0)
            {
                urls = CarImageBootstrapper.GetLatestImageUrls(this, 3);
            }
            SetSlideBackground(slide1, urls, 0);
            SetSlideBackground(slide2, urls, 1);
            SetSlideBackground(slide3, urls, 2);
        }

        private static void SetSlideBackground(HtmlGenericControl slide, System.Collections.Generic.List<string> urls, int index)
        {
            if (slide == null) return;
            string url = (urls.Count > index ? urls[index] : null) ?? "~/assets/images/Слой 1.png";
            slide.Style["background-image"] = $"url('{url}')";
            slide.Style["background-size"] = "cover";
            slide.Style["background-position"] = "center";
        }

        private List<string> GetStaticHeroImages()
        {
            return new List<string>
            {
                ResolveUrl("~/colorcars/Aston Martin Vanquish/white/3.jpg"),
                ResolveUrl("~/colorcars/Lamborghini Huracan/purple/3.jpg"),
                ResolveUrl("~/colorcars/Maserati GranTurismo/red/5.jpg")
            };
        }

        protected void btnSubmitComment_Click(object sender, EventArgs e)
        {
            EnsureSchema();

            if (Session["UserId"] == null)
            {
                pnlLoginRequired.Visible = true;
                pnlCommentForm.Visible = false;
                lblCommentStatus.Visible = false;
                return;
            }

            string userName = GetCurrentUserName();
            if (string.IsNullOrWhiteSpace(userName))
            {
                pnlUsernameRequired.Visible = true;
                pnlCommentForm.Visible = false;
                lblCommentStatus.Visible = false;
                return;
            }
            else
            {
                ConfigureCommentUi();
            }

            if (!int.TryParse(hfRating.Value, out int rating) || rating < 1 || rating > 5)
            {
                ShowStatus("Выберите оценку от 1 до 5.", isError: true);
                return;
            }

            string commentText = (txtComment.Text ?? string.Empty).Trim();
            if (commentText.Length < 3)
            {
                ShowStatus("Комментарий слишком короткий. Добавьте пару слов.", isError: true);
                return;
            }
            if (commentText.Length > 1000)
            {
                ShowStatus("Комментарий должен быть не длиннее 1000 символов.", isError: true);
                return;
            }

            string connectionString = WebApplication1.Models.Functions.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var insertCmd = new SqlCommand(@"
                    INSERT INTO CustomerComments (CustId, UserName, Rating, CommentText, CreatedAt)
                    VALUES (@CustId, @UserName, @Rating, @CommentText, GETDATE());", connection);

                insertCmd.Parameters.AddWithValue("@CustId", (int)Session["UserId"]);
                insertCmd.Parameters.AddWithValue("@UserName", userName);
                insertCmd.Parameters.AddWithValue("@Rating", rating);
                insertCmd.Parameters.AddWithValue("@CommentText", commentText);

                insertCmd.ExecuteNonQuery();
            }

            txtComment.Text = string.Empty;
            hfRating.Value = "5";
            ShowStatus("Спасибо! Ваш отзыв сохранен.", isError: false);
            LoadComments();
        }

        protected void rptComments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string command = e.CommandName.ToLowerInvariant();
            if (command != "delete")
                return;

            bool isAdmin = IsAdmin();

            if (!isAdmin && Session["UserId"] == null)
            {
                ShowStatus("Авторизуйтесь, чтобы управлять отзывами.", isError: true);
                return;
            }

            if (!int.TryParse(e.CommandArgument.ToString(), out int commentId))
            {
                ShowStatus("Не удалось определить отзыв.", isError: true);
                return;
            }

            string connectionString = WebApplication1.Models.Functions.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var deleteCmd = new SqlCommand(@"
                    DELETE FROM CustomerComments
                    WHERE Id = @Id AND (@IsAdmin = 1 OR CustId = @CustId);", connection);
                deleteCmd.Parameters.AddWithValue("@Id", commentId);
                int custIdParam = Session["UserId"] == null ? 0 : (int)Session["UserId"];
                deleteCmd.Parameters.AddWithValue("@CustId", custIdParam);
                deleteCmd.Parameters.AddWithValue("@IsAdmin", isAdmin ? 1 : 0);

                int rows = deleteCmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    ShowStatus("Отзыв удалён.", isError: false);
                    LoadComments();
                }
                else
                {
                    ShowStatus("Можно удалять только свои отзывы.", isError: true);
                }
            }
        }

        protected void rptComments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var dataRowView = e.Item.DataItem as DataRowView;
            if (dataRowView == null)
                return;

            int commentCustId = dataRowView["CustId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRowView["CustId"]);
            int currentUserId = Session["UserId"] == null ? -1 : (int)Session["UserId"];
            bool isAdmin = IsAdmin();
            bool isOwn = currentUserId != -1 && currentUserId == commentCustId;
            bool isHidden = dataRowView["IsHidden"] != DBNull.Value && Convert.ToBoolean(dataRowView["IsHidden"]);

            var deleteButton = e.Item.FindControl("btnDelete") as LinkButton;
            if (deleteButton != null)
            {
                deleteButton.Visible = isAdmin || isOwn;
            }

            var hiddenLabel = e.Item.FindControl("lblHidden") as Label;
            if (hiddenLabel != null)
            {
                hiddenLabel.Visible = isHidden;
            }
        }

        private void LoadComments()
        {
            string connectionString = WebApplication1.Models.Functions.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var loadCmd = new SqlCommand(@"
                    SELECT TOP 30 Id, CustId, UserName, Rating, CommentText, CreatedAt, IsHidden
                    FROM CustomerComments
                    WHERE IsHidden = 0
                    ORDER BY CreatedAt DESC;", connection);

                using (var reader = loadCmd.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(reader);
                    rptComments.DataSource = table;
                    rptComments.DataBind();
                }
            }
        }

        private void EnsureSchema()
        {
            string connectionString = WebApplication1.Models.Functions.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var ensureUserNameColumn = new SqlCommand(@"
                    IF COL_LENGTH('CustomerTbl', 'CustUserName') IS NULL
                    BEGIN
                        ALTER TABLE CustomerTbl ADD CustUserName NVARCHAR(100) NULL;
                    END;", connection);

                ensureUserNameColumn.ExecuteNonQuery();

                var ensureCommentsTable = new SqlCommand(@"
                    IF OBJECT_ID('dbo.CustomerComments', 'U') IS NULL
                    BEGIN
                        CREATE TABLE dbo.CustomerComments
                        (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            CustId INT NOT NULL,
                            UserName NVARCHAR(100) NOT NULL,
                            Rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
                            CommentText NVARCHAR(1000) NOT NULL,
                            CreatedAt DATETIME NOT NULL DEFAULT(GETDATE()),
                            IsHidden BIT NOT NULL DEFAULT(0)
                        );
                        CREATE INDEX IX_CustomerComments_CreatedAt ON dbo.CustomerComments (CreatedAt DESC);
                    END;", connection);

                ensureCommentsTable.ExecuteNonQuery();

                var ensureIsHiddenColumn = new SqlCommand(@"
                    IF COL_LENGTH('CustomerComments', 'IsHidden') IS NULL
                    BEGIN
                        ALTER TABLE CustomerComments ADD IsHidden BIT NOT NULL DEFAULT(0);
                    END;", connection);
                ensureIsHiddenColumn.ExecuteNonQuery();
            }
        }

        private bool IsAdmin()
        {
            return Session["UserEmail"] != null &&
                   Session["UserEmail"].ToString().Equals("admin@gmail.com", StringComparison.OrdinalIgnoreCase);
        }

        private string GetCurrentUserName()
        {
            if (Session["UserId"] == null)
                return string.Empty;

            string connectionString = WebApplication1.Models.Functions.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand("SELECT CustUserName FROM CustomerTbl WHERE CustId = @CustId", connection);
                cmd.Parameters.AddWithValue("@CustId", (int)Session["UserId"]);
                var result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? string.Empty : result.ToString();
            }
        }

        private void ShowStatus(string message, bool isError)
        {
            lblCommentStatus.Visible = true;
            lblCommentStatus.Text = HttpUtility.HtmlEncode(message);
            lblCommentStatus.CssClass = isError ? "text-danger fw-semibold" : "text-success fw-semibold";
        }

        protected string RenderStars(object ratingObj)
        {
            int rating = 0;
            if (ratingObj != null)
            {
                int.TryParse(ratingObj.ToString(), out rating);
            }

            var sb = new StringBuilder();
            for (int i = 1; i <= 5; i++)
            {
                sb.Append(i <= rating
                    ? "<i class='fas fa-star text-warning'></i>"
                    : "<i class='far fa-star text-muted'></i>");
            }
            return sb.ToString();
        }
    }
}
