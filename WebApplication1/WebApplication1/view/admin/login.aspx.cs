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
                string запрос = "SELECT COUNT(*) FROM CustomerAuthTbl WHERE CustEmail = @Email AND CustPassword = @Password";
                SqlCommand cmd = new SqlCommand(запрос, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                int найдено = (int)cmd.ExecuteScalar();
                if (найдено > 0)
                {
                    Session["UserEmail"] = email;
                    Response.Redirect("home.aspx");
                }
                else
                {
                    lblMsg.Text = "Неверный email или пароль!";
                    lblMsg.Visible = true;
                }
            }
        }

        protected void btnSendCode_Click(object sender, EventArgs e)
        {
            string email = txtRecoveryEmail.Text.Trim();
            string временныйПароль = GenerateRandomPassword(8);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string проверка = "SELECT COUNT(*) FROM CustomerAuthTbl WHERE CustEmail = @Email";
                SqlCommand checkCmd = new SqlCommand(проверка, conn);
                checkCmd.Parameters.AddWithValue("@Email", email);

                int существует = (int)checkCmd.ExecuteScalar();
                if (существует == 0)
                {
                    lblMsg.Text = "Email не найден!";
                    lblMsg.Visible = true;
                    return;
                }

                string обновитьAuth = "UPDATE CustomerAuthTbl SET CustPassword = @Password WHERE CustEmail = @Email";
                SqlCommand updateCmd = new SqlCommand(обновитьAuth, conn);
                updateCmd.Parameters.AddWithValue("@Password", временныйПароль);
                updateCmd.Parameters.AddWithValue("@Email", email);
                updateCmd.ExecuteNonQuery();

                string обновитьMain = @"
                    UPDATE CustomerTbl 
                    SET CustPassword = @Password 
                    WHERE CustId = (
                        SELECT CustId FROM CustomerAuthTbl WHERE CustEmail = @Email
                    )";
                SqlCommand updateMainCmd = new SqlCommand(обновитьMain, conn);
                updateMainCmd.Parameters.AddWithValue("@Password", временныйПароль);
                updateMainCmd.Parameters.AddWithValue("@Email", email);
                updateMainCmd.ExecuteNonQuery();
            }

            try
            {
                MailMessage письмо = new MailMessage();
                письмо.From = new MailAddress("wheeldeal989@gmail.com");
                письмо.To.Add(email);
                письмо.Subject = "Временный пароль для WheelDeal";
                письмо.Body = $"Ваш временный пароль: {временныйПароль}\n\n" +
                              "Используйте его для входа в систему. Рекомендуем сразу сменить пароль после входа.";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 25);
                smtp.Credentials = new NetworkCredential("wheeldeal989@gmail.com", "xqwj lscl uvgw gusf");
                smtp.EnableSsl = true;
                smtp.Send(письмо);

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

        private string GenerateRandomPassword(int длина)
        {
            const string допустимыеСимволы = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            Random random = new Random();
            char[] символы = new char[длина];
            for (int i = 0; i < длина; i++)
            {
                символы[i] = допустимыеСимволы[random.Next(допустимыеСимволы.Length)];
            }
            return new string(символы);
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            string email = txtRecoveryEmail.Text.Trim();
            string код = txtResetCode.Text;
            string новыйПароль = txtNewPassword.Text;
            string подтверждениеПароля = txtConfirmPassword.Text;

            if (новыйПароль != подтверждениеПароля)
            {
                lblResetMsg.Text = "Пароли не совпадают!";
                lblResetMsg.Visible = true;
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string проверкаКода = @"SELECT COUNT(*) FROM PasswordResetCodes 
                                        WHERE Email = @Email AND Code = @Code 
                                        AND Expiration > GETDATE()";
                SqlCommand cmd = new SqlCommand(проверкаКода, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Code", код);

                int действителен = (int)cmd.ExecuteScalar();
                if (действителен == 0)
                {
                    lblResetMsg.Text = "Неверный или просроченный код!";
                    lblResetMsg.Visible = true;
                    return;
                }

                string обновитьПароль = "UPDATE CustomerAuthTbl SET CustPassword = @Password WHERE CustEmail = @Email";
                cmd = new SqlCommand(обновитьПароль, conn);
                cmd.Parameters.AddWithValue("@Password", новыйПароль);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.ExecuteNonQuery();

                string удалитьКод = "DELETE FROM PasswordResetCodes WHERE Email = @Email";
                cmd = new SqlCommand(удалитьКод, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.ExecuteNonQuery();

                lblResetMsg.Text = "Пароль успешно обновлён!";
                lblResetMsg.Visible = true;
                lblResetMsg.CssClass = "text-success d-block text-center mt-2";
            }
        }
    }
}
