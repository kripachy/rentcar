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
            string confirmation = txtConfirmPassword.Text;

            if (!ValidateEmail(email))
            {
                lblMsg.Text = "Пожалуйста, введите корректный адрес электронной почты";
                lblMsg.Visible = true;
                return;
            }

            if (password != confirmation)
            {
                lblMsg.Text = "Пароли не совпадают";
                lblMsg.Visible = true;
                return;
            }

            if (password.Length < 8)
            {
                lblMsg.Text = "Пароль должен содержать не менее 8 символов";
                lblMsg.Visible = true;
                return;
            }

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string checkEmailQuery = "SELECT COUNT(*) FROM CustomerAuthTbl WHERE CustEmail = @Email";
                using (SqlCommand checkCmd = new SqlCommand(checkEmailQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Email", email);
                    int emailExists = (int)checkCmd.ExecuteScalar();
                    if (emailExists > 0)
                    {
                        lblMsg.Text = "Этот email уже зарегистрирован";
                        lblMsg.Visible = true;
                        return;
                    }
                }

                int newId;
                string idQuery = "SELECT ISNULL(MAX(CustId), 0) + 1 FROM CustomerTbl";
                using (SqlCommand idCmd = new SqlCommand(idQuery, conn))
                {
                    newId = (int)idCmd.ExecuteScalar();
                }

                string defaultName = "Новый пользователь";
                string defaultAddress = "Не указан";
                string defaultPhone = "0000000000";

                string insertCustomer = "INSERT INTO CustomerTbl (CustId, CustName, CustAdd, CustPhone, CustPassword) " +
                                    "VALUES (@CustId, @Name, @Address, @Phone, @Password)";
                using (SqlCommand customerCmd = new SqlCommand(insertCustomer, conn))
                {
                    customerCmd.Parameters.AddWithValue("@CustId", newId);
                    customerCmd.Parameters.AddWithValue("@Name", defaultName);
                    customerCmd.Parameters.AddWithValue("@Address", defaultAddress);
                    customerCmd.Parameters.AddWithValue("@Phone", defaultPhone);
                    customerCmd.Parameters.AddWithValue("@Password", password);
                    customerCmd.ExecuteNonQuery();
                }

                string insertAuth = "INSERT INTO CustomerAuthTbl (CustId, CustEmail, CustPassword) VALUES (@CustId, @Email, @Password)";
                using (SqlCommand authCmd = new SqlCommand(insertAuth, conn))
                {
                    authCmd.Parameters.AddWithValue("@CustId", newId);
                    authCmd.Parameters.AddWithValue("@Email", email);
                    authCmd.Parameters.AddWithValue("@Password", password);
                    authCmd.ExecuteNonQuery();
                }

                Session["UserEmail"] = email;
                Response.Redirect("profile.aspx");
            }
        }

        private bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
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