using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.view.admin
{
    public partial class home : System.Web.UI.Page
    {
        private Functions functions;

        protected global::System.Web.UI.WebControls.Literal litCarsCount;
        protected global::System.Web.UI.WebControls.Literal litCustomersCount;
        protected global::System.Web.UI.WebControls.Literal litRentsCount;
        protected global::System.Web.UI.WebControls.Literal litCommentsCount;
        protected global::System.Web.UI.WebControls.Literal litPendingVerifications;

        protected void Page_Load(object sender, EventArgs e)
        {
            functions = new Functions();

            if (!IsPostBack)
            {
                // Проверяем авторизацию
                if (Session["UserEmail"] == null)
                {
                    Response.Redirect("~/view/admin/login.aspx");
                    return;
                }

                string userEmail = Session["UserEmail"].ToString();
                if (!userEmail.Equals("admin@gmail.com", StringComparison.OrdinalIgnoreCase))
                {
                    Response.Redirect("~/view/admin/userdashboard.aspx");
                    return;
                }

                LoadDashboardStats();
            }
        }

        private void LoadDashboardStats()
        {
            string cs = Functions.GetConnectionString();
            using (var conn = new SqlConnection(cs))
            {
                conn.Open();

                litCarsCount.Text = ExecuteCount(conn, "SELECT COUNT(*) FROM CarTbl").ToString();
                litCustomersCount.Text = ExecuteCount(conn, "SELECT COUNT(*) FROM CustomerTbl").ToString();
                litRentsCount.Text = ExecuteCount(conn, "SELECT COUNT(*) FROM RentTbl").ToString();
                litCommentsCount.Text = ExecuteCount(conn, "SELECT CASE WHEN OBJECT_ID('dbo.CustomerComments','U') IS NULL THEN 0 ELSE (SELECT COUNT(*) FROM dbo.CustomerComments) END").ToString();
                litPendingVerifications.Text = ExecuteCount(conn, "SELECT CASE WHEN OBJECT_ID('dbo.DrivingLicense','U') IS NULL THEN 0 ELSE (SELECT COUNT(*) FROM dbo.DrivingLicense WHERE Status = 'Pending') END").ToString();
            }
        }

        private static int ExecuteCount(SqlConnection conn, string sql)
        {
            using (var cmd = new SqlCommand(sql, conn))
            {
                object value = cmd.ExecuteScalar();
                if (value == null || value == DBNull.Value) return 0;
                return Convert.ToInt32(value);
            }
        }
    }
}