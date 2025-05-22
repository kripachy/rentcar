using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Models;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;
using System.Text;

namespace WebApplication1.view.admin
{
    public partial class login : Page
    {
        private WebApplication1.Models.Functions functions;
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            functions = new WebApplication1.Models.Functions();
            
            // Проверяем, не пытаемся ли мы выйти из системы
            if (Request.QueryString["logout"] != null)
            {
                Session.Clear();
                Session.Abandon();
                return;
            }

            // Если это не постбэк и пользователь уже вошел, перенаправляем только если это не страница входа
            if (!IsPostBack && Session["UserEmail"] != null && !Request.Path.EndsWith("login.aspx", StringComparison.OrdinalIgnoreCase))
            {
                string userEmail = Session["UserEmail"].ToString();
                RedirectBasedOnRole(userEmail);
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ShowError("Пожалуйста, заполните все поля");
                return;
            }

            
            if (email.Equals("admin@gmail.com", StringComparison.OrdinalIgnoreCase))
            {
               
                string adminPassword = "11111111"; 

                if (password == adminPassword)
                {
                    Session["UserEmail"] = email;
                    Response.Redirect("~/view/admin/home.aspx");
                    return;
                }
                else
                {
                    ShowError("Неверный email или пароль");
                    return;
                }
            }

            // Проверяем учетные данные в таблице авторизации для обычных пользователей
            string query = $"SELECT * FROM CustomerAuthTbl WHERE CustEmail = '{email}' AND CustPassword = '{password}'";
            DataTable dt = functions.GetData(query);

            if (dt.Rows.Count > 0)
            {
                // Сохраняем email пользователя в сессии
                Session["UserEmail"] = email;

                // Перенаправляем в зависимости от роли (для обычных пользователей)
                RedirectBasedOnRole(email);
            }
            else
            {
                ShowError("Неверный email или пароль");
            }
        }

        private void RedirectBasedOnRole(string email)
        {
            if (email.Equals("admin@gmail.com", StringComparison.OrdinalIgnoreCase))
            {
                // Перенаправляем на страницу администратора
                Response.Redirect("~/view/admin/home.aspx");
            }
            else
            {
                // Перенаправляем на страницу пользователя
                Response.Redirect("~/view/admin/userdashboard.aspx");
            }
        }

        private void ShowError(string message)
        {
            lblMsg.Text = message;
            lblMsg.Visible = true;
        }

        protected void btnSendCode_Click(object sender, EventArgs e)
        {
            // Логика для кнопки "Получить временный пароль"
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            // Логика для кнопки "Сбросить пароль"
        }

        protected void btnSendPassword_Click(object sender, EventArgs e)
        {
            string email = txtRecoveryEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                ShowRecoveryMessage("Пожалуйста, введите email", true);
                return;
            }

            // Проверяем существование email в базе данных
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM CustomerAuthTbl WHERE CustEmail = @Email";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    int count = (int)cmd.ExecuteScalar();
                    if (count == 0)
                    {
                        ShowRecoveryMessage("Пользователь с таким email не найден", true);
                        return;
                    }
                }
            }

            // Генерируем временный пароль
            string tempPassword = GenerateTemporaryPassword();

            try
            {
                // Отправляем email
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("wheeldeal989@gmail.com");
                    mail.To.Add(email);
                    mail.Subject = "Временный пароль для WheelDeal";
                    mail.Body = $"Ваш временный пароль: {tempPassword}\n\n" +
                              "Используйте его для входа в систему. Рекомендуем сразу сменить пароль после входа.";

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("wheeldeal989@gmail.com", "xqwj lscl uvgw gusf");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }

                // Обновляем пароль в базе данных CustomerAuthTbl
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Обновляем пароль в CustomerAuthTbl
                    string updateAuthQuery = "UPDATE CustomerAuthTbl SET CustPassword = @Password WHERE CustEmail = @Email";
                    using (SqlCommand cmd = new SqlCommand(updateAuthQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Password", tempPassword);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.ExecuteNonQuery();
                    }

                    // Находим CustId по Email из CustomerAuthTbl
                    string getCustIdQuery = "SELECT CustId FROM CustomerAuthTbl WHERE CustEmail = @Email";
                    int custId = -1; // Default value if not found
                    using (SqlCommand cmd = new SqlCommand(getCustIdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            custId = Convert.ToInt32(result);
                        }
                    }

                    // Обновляем пароль в CustomerTbl, если CustId найден
                    if (custId != -1)
                    {
                         string updateCustomerQuery = "UPDATE CustomerTbl SET CustPassword = @Password WHERE CustId = @CustId";
                         using (SqlCommand cmd = new SqlCommand(updateCustomerQuery, conn))
                         {
                             cmd.Parameters.AddWithValue("@Password", tempPassword);
                             cmd.Parameters.AddWithValue("@CustId", custId);
                             int rowsAffected = cmd.ExecuteNonQuery();
                             if (rowsAffected > 0)
                             {
                                 // Добавляем сообщение об успешном обновлении CustomerTbl (для отладки)
                                 ShowRecoveryMessage("Пароль обновлен в CustomerTbl", false);
                             }
                             else
                             {
                                 // Добавляем сообщение о том, что CustId не найден в CustomerTbl (для отладки)
                                 ShowRecoveryMessage("Ошибка: CustId не найден в CustomerTbl для обновления", true);
                             }
                         }
                    }
                     else
                    {
                         // Добавляем сообщение о том, что CustId не найден в CustomerAuthTbl (для отладки)
                         ShowRecoveryMessage("Ошибка: Email найден в Auth, но не удалось получить CustId", true);
                    }
                }

                //showrecoverymessage("Временный пароль отправлен на ваш email", false);
            }
            catch (Exception ex)
            {
                ShowRecoveryMessage("Ошибка при отправке email: " + ex.Message, true);
            }
        }

        private string GenerateTemporaryPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var password = new StringBuilder(8);

            for (int i = 0; i < 8; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }

            return password.ToString();
        }

        private void ShowRecoveryMessage(string message, bool isError)
        {
            lblRecoveryMessage.Text = message;
            lblRecoveryMessage.CssClass = isError ? "d-block text-center mt-2 text-danger" : "d-block text-center mt-2 text-success";
            lblRecoveryMessage.Visible = true;
        }
    }
} 