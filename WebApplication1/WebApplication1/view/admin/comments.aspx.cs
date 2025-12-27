using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.view.admin
{
    public partial class comments : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsAdmin())
            {
                Response.Redirect("~/view/admin/login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                EnsureSchema();
                LoadComments();
            }
        }

        protected void gvComments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "delete" && e.CommandName != "togglehide")
                return;

            int rowIndex;
            if (!int.TryParse(e.CommandArgument.ToString(), out rowIndex))
                return;

            int id = Convert.ToInt32(gvComments.DataKeys[rowIndex].Value);
            string connectionString = WebApplication1.Models.Functions.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                if (e.CommandName == "delete")
                {
                    var deleteCmd = new SqlCommand("DELETE FROM CustomerComments WHERE Id = @Id", connection);
                    deleteCmd.Parameters.AddWithValue("@Id", id);
                    deleteCmd.ExecuteNonQuery();
                    ShowStatus("Отзыв удалён.", false);
                }
                else if (e.CommandName == "togglehide")
                {
                    var toggleCmd = new SqlCommand(@"
                        UPDATE CustomerComments
                        SET IsHidden = CASE WHEN IsHidden = 1 THEN 0 ELSE 1 END
                        WHERE Id = @Id;", connection);
                    toggleCmd.Parameters.AddWithValue("@Id", id);
                    toggleCmd.ExecuteNonQuery();
                    ShowStatus("Статус изменён.", false);
                }
            }

            LoadComments();
        }

        protected void gvComments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvComments.PageIndex = e.NewPageIndex;
            LoadComments();
        }

        protected void gvComments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Prevent default delete handling because we process deletes in RowCommand.
            e.Cancel = true;
        }

        private void LoadComments()
        {
            string connectionString = WebApplication1.Models.Functions.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var loadCmd = new SqlCommand(@"
                    SELECT Id, CustId, UserName, Rating, CommentText, CreatedAt, IsHidden
                    FROM CustomerComments
                    ORDER BY CreatedAt DESC;", connection);

                using (var reader = loadCmd.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(reader);
                    gvComments.DataSource = table;
                    gvComments.DataBind();
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

        private void ShowStatus(string message, bool isError)
        {
            lblStatus.Visible = true;
            lblStatus.Text = message;
            lblStatus.CssClass = isError ? "text-danger fw-semibold" : "text-success fw-semibold";
        }
    }
}
