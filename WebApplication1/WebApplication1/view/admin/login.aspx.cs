using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace WebApplication1
{
    public partial class login : System.Web.UI.Page
    {
        string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True";

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

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

        protected void btnSendCode_Click(object sender, EventArgs e)
        {
            string email = txtRecoveryEmail.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string checkEmail = "SELECT COUNT(*) FROM CustomerAuthTbl WHERE CustEmail = @Email";
                SqlCommand cmd = new SqlCommand(checkEmail, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                int exists = (int)cmd.ExecuteScalar();
                if (exists == 0)
                {
                    lblMsg.Text = "Email не найден!";
                    lblMsg.Visible = true;
                    return;
                }
            }

            string tempPassword = GenerateRandomPassword(8);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string updatePassword = "UPDATE CustomerAuthTbl SET CustPassword = @Password WHERE CustEmail = @Email";
                SqlCommand cmd = new SqlCommand(updatePassword, conn);
                cmd.Parameters.AddWithValue("@Password", tempPassword);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.ExecuteNonQuery();
            }

            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("wheeldeal989@gmail.com"); 
                mail.To.Add(email);
                mail.Subject = "Временный пароль для WheelDeal";
                mail.Body = $"Ваш временный пароль: {tempPassword}\n\n" +
                           "Используйте его для входа в систему. Рекомендуем сразу сменить пароль после входа.";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 25); 
                smtp.Credentials = new NetworkCredential("wheeldeal989@gmail.com", "xqwj lscl uvgw gusf");
                smtp.EnableSsl = true;
                smtp.Send(mail);

                lblMsg.Text = "Временный пароль отправлен на вашу почту!";
                lblMsg.Visible = true;
                lblMsg.CssClass = "text-success d-block text-center";
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Ошибка при отправке email: " + ex.Message;
                lblMsg.Visible = true;
            }
        }

        private string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            Random random = new Random();
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(validChars.Length)];
            }
            return new string(chars);
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            string email = txtRecoveryEmail.Text.Trim();
            string code = txtResetCode.Text;
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (newPassword != confirmPassword)
            {
                lblResetMsg.Text = "Passwords do not match!";
                lblResetMsg.Visible = true;
                return;
            }

            // Verify code
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string checkCode = @"SELECT COUNT(*) FROM PasswordResetCodes 
                                   WHERE Email = @Email AND Code = @Code 
                                   AND Expiration > GETDATE()";
                SqlCommand cmd = new SqlCommand(checkCode, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Code", code);

                int valid = (int)cmd.ExecuteScalar();
                if (valid == 0)
                {
                    lblResetMsg.Text = "Invalid or expired code!";
                    lblResetMsg.Visible = true;
                    return;
                }

                // Update password
                string updatePassword = "UPDATE CustomerAuthTbl SET CustPassword = @Password WHERE CustEmail = @Email";
                cmd = new SqlCommand(updatePassword, conn);
                cmd.Parameters.AddWithValue("@Password", newPassword);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.ExecuteNonQuery();

                // Delete used code
                string deleteCode = "DELETE FROM PasswordResetCodes WHERE Email = @Email";
                cmd = new SqlCommand(deleteCode, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.ExecuteNonQuery();

                lblResetMsg.Text = "Password updated successfully!";
                lblResetMsg.Visible = true;
                lblResetMsg.CssClass = "text-success d-block text-center mt-2";
            }
        }
    }
}