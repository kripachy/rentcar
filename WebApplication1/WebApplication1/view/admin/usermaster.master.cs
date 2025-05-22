using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace WebApplication1.view.admin
{
    public partial class usermaster : System.Web.UI.MasterPage
    {
        protected string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True";

        public int? GetCurrentUserId()
        {
            if (Session["UserEmail"] == null)
                return null;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT CustId FROM CustomerAuthTbl WHERE CustEmail = @Email";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", Session["UserEmail"]);
                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                    {
                        return null;
                    }
                    return Convert.ToInt32(result);
                }
            }
        }

        protected string GetActiveClass(string pageName)
        {
            string currentPage = System.IO.Path.GetFileName(Request.Path);
            return currentPage.Equals(pageName, StringComparison.OrdinalIgnoreCase)
                ? "nav-link active text-danger"
                : "nav-link text-danger";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Store user ID in session if not already there
            if (Session["UserEmail"] != null && Session["UserId"] == null)
            {
                int? userId = GetCurrentUserId();
                if (userId.HasValue)
                {
                    Session["UserId"] = userId.Value;
                }
            }
        }
    }
}
