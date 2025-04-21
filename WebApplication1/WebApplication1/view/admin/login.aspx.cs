using System;
using System.Data.SqlClient;

namespace WebApplication1
{
    public partial class login : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string checkUser = "SELECT COUNT(*) FROM CustomerAuthTbl WHERE CustEmail = @Email AND CustPassword = @Password";
                SqlCommand cmd = new SqlCommand(checkUser, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                int exists = (int)cmd.ExecuteScalar();
                if (exists > 0)
                {
                    Session["UserEmail"] = email;
                    Response.Redirect("home.aspx");
                }
                else
                {
                    lblMsg.Text = "Invalid email or password!";
                    lblMsg.Visible = true;
                }
            }
        }
    }
}
