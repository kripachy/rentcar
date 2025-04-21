using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace WebApplication1
{
    public partial class register : System.Web.UI.Page
    {
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Validate email format
            if (!IsValidEmail(email))
            {
                lblMsg.Text = "Please enter a valid email address";
                lblMsg.Visible = true;
                return;
            }

            // Validate password match
            if (password != confirmPassword)
            {
                lblMsg.Text = "Passwords do not match";
                lblMsg.Visible = true;
                return;
            }

            // Validate password strength
            if (password.Length < 8)
            {
                lblMsg.Text = "Password must be at least 8 characters long";
                lblMsg.Visible = true;
                return;
            }

            string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Check if email already exists
                string checkEmail = "SELECT COUNT(*) FROM CustomerAuthTbl WHERE CustEmail = @Email";
                SqlCommand checkCmd = new SqlCommand(checkEmail, conn);
                checkCmd.Parameters.AddWithValue("@Email", email);

                int emailExists = (int)checkCmd.ExecuteScalar();
                if (emailExists > 0)
                {
                    lblMsg.Text = "This email is already registered";
                    lblMsg.Visible = true;
                    return;
                }

                // Insert new user
                string insertUser = "INSERT INTO CustomerAuthTbl (CustEmail, CustPassword) VALUES (@Email, @Password)";
                SqlCommand insertCmd = new SqlCommand(insertUser, conn);
                insertCmd.Parameters.AddWithValue("@Email", email);
                insertCmd.Parameters.AddWithValue("@Password", password);

                insertCmd.ExecuteNonQuery();

                // Save email in session to use it when user completes profile
                Session["UserEmail"] = email;

                // Redirect to profile page to fill in the details
                Response.Redirect("profile.aspx");
            }
        }


        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Use simple regex for basic validation
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}