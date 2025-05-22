using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;

namespace WebApplication1
{
    public partial class profile : System.Web.UI.Page
    {
        // Добавляем объявления для новых элементов управления
        protected global::System.Web.UI.WebControls.TextBox txtNewEmail;
        protected global::System.Web.UI.WebControls.TextBox txtCurrentPassword;
        protected global::System.Web.UI.WebControls.TextBox txtNewPassword;
        protected global::System.Web.UI.WebControls.TextBox txtConfirmNewPassword;
        protected global::System.Web.UI.WebControls.Label lblPasswordChangeMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserEmail"] == null || Session["UserId"] == null)
                {
                    Response.Redirect("~/view/login.aspx");
                    return;
                }

                // Проверяем, только ли что пользователь зарегистрировался
                bool justRegistered = Session["JustRegistered"] != null && (bool)Session["JustRegistered"];

                if (!justRegistered)
                {
                     // Загружаем текущие данные пользователя только если это не первый визит после регистрации
                    LoadUserData();
                } else {
                    // Если только что зарегистрировались, очищаем флаг после первого посещения профиля
                    Session["JustRegistered"] = false;
                }
            }
        }

        private void LoadUserData()
        {
            // Убеждаемся, что UserId доступен перед запросом к базе данных
            if (Session["UserId"] == null)
            {
                 Response.Redirect("~/view/login.aspx"); // Перенаправляем на логин, если UserId нет в сессии
                 return;
            }

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True;Connect Timeout=30;";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand("SELECT CustName, CustPhone, CustAdd FROM CustomerTbl WHERE CustId = @CustId", connection);
                cmd.Parameters.AddWithValue("@CustId", (int)Session["UserId"]); // Явно приводим к int

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtName.Text = reader["CustName"].ToString();
                        txtPhone.Text = reader["CustPhone"].ToString();
                        string city = reader["CustAdd"].ToString();
                        
                        // Устанавливаем выбранный город в выпадающем списке
                        if (!string.IsNullOrEmpty(city))
                        {
                            var item = ddlCity.Items.FindByValue(city);
                            if (item != null)
                            {
                                item.Selected = true;
                            }
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
             if (Session["UserId"] == null)
            {
                 Response.Redirect("~/view/login.aspx"); // Перенаправляем на логин, если UserId нет в сессии
                 return;
            }

            // Сбрасываем сообщение об ошибке/успехе
            lblPasswordChangeMsg.Visible = false;
            lblPasswordChangeMsg.CssClass = "d-block text-center";

            // Валидация основных полей профиля (ФИО, телефон, город)
            if (!ValidateInput())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Пожалуйста, введите корректную информацию в основные поля профиля (ФИО, Телефон, Город)');", true);
                return;
            }

            string newEmail = txtNewEmail.Text.Trim();
            string currentPassword = txtCurrentPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmNewPassword = txtConfirmNewPassword.Text;

            bool emailChanged = !string.IsNullOrEmpty(newEmail);
            bool passwordChanged = !string.IsNullOrEmpty(newPassword);

            // Если ничего не меняется в полях email/пароль, просто сохраняем основные данные профиля
            if (!emailChanged && !passwordChanged)
            {
                try
                {
                    SaveProfileData(null, null); // Передаем null, так как email и пароль не меняются
                    Session["JustRegistered"] = false;
                     ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Основные данные профиля успешно сохранены');", true);
                    // Response.Redirect("userdashboard.aspx"); // Можно перенаправить, но, возможно, лучше остаться на странице с сообщением
                }
                 catch (Exception ex)
                {
                     ClientScript.RegisterStartupScript(this.GetType(), "error", $"alert('Ошибка при сохранении основных данных: {ex.Message}');", true);
                }
                return;
            }

            // Если меняется email или пароль, требуется ввод текущего пароля
            if (string.IsNullOrEmpty(currentPassword))
            {
                lblPasswordChangeMsg.Text = "Для изменения почты или пароля введите текущий пароль.";
                lblPasswordChangeMsg.CssClass = "text-danger";
                lblPasswordChangeMsg.Visible = true;
                return;
            }

            // Проверяем текущий пароль
            if (!CheckCurrentPassword(currentPassword))
            {
                lblPasswordChangeMsg.Text = "Неверный текущий пароль.";
                lblPasswordChangeMsg.CssClass = "text-danger";
                lblPasswordChangeMsg.Visible = true;
                txtCurrentPassword.Text = ""; // Очищаем поле текущего пароля
                return;
            }

            // Валидация нового email
            if (emailChanged && !ValidateEmail(newEmail))
            {
                lblPasswordChangeMsg.Text = "Пожалуйста, введите корректный адрес электронной почты для нового Email.";
                lblPasswordChangeMsg.CssClass = "text-danger";
                lblPasswordChangeMsg.Visible = true;
                return;
            }

             // Проверка на уникальность нового email, если он меняется
            if (emailChanged && !IsEmailUnique(newEmail))
            {
                 lblPasswordChangeMsg.Text = "Этот новый email уже зарегистрирован.";
                 lblPasswordChangeMsg.CssClass = "text-danger";
                 lblPasswordChangeMsg.Visible = true;
                 return;
            }

            // Валидация нового пароля
            if (passwordChanged)
            {
                if (newPassword.Length < 8)
                {
                    lblPasswordChangeMsg.Text = "Новый пароль должен содержать не менее 8 символов.";
                    lblPasswordChangeMsg.CssClass = "text-danger";
                    lblPasswordChangeMsg.Visible = true;
                    return;
                }
                if (newPassword != confirmNewPassword)
                {
                    lblPasswordChangeMsg.Text = "Новые пароли не совпадают.";
                    lblPasswordChangeMsg.CssClass = "text-danger";
                    lblPasswordChangeMsg.Visible = true;
                    txtNewPassword.Text = "";
                    txtConfirmNewPassword.Text = "";
                    return;
                }
            }

            // Если все валидации пройдены, сохраняем все изменения
            try
            {
                 SaveProfileData(emailChanged ? newEmail : null, passwordChanged ? newPassword : null);
                 Session["JustRegistered"] = false;
                
                lblPasswordChangeMsg.Text = "Изменения успешно сохранены!";
                lblPasswordChangeMsg.CssClass = "text-success";
                lblPasswordChangeMsg.Visible = true;

                // Если email был изменен, обновляем его в сессии
                if (emailChanged)
                {
                     Session["UserEmail"] = newEmail;
                }

                // Очищаем поля паролей после успешного сохранения
                txtCurrentPassword.Text = "";
                txtNewPassword.Text = "";
                txtConfirmNewPassword.Text = "";
                
                // Можно перенаправить пользователя, например, на главную страницу, или оставить на странице профиля с сообщением об успехе.
                // Response.Redirect("userdashboard.aspx");
            }
            catch (Exception ex)
            {
                 lblPasswordChangeMsg.Text = "Ошибка при сохранении изменений: " + ex.Message;
                 lblPasswordChangeMsg.CssClass = "text-danger";
                 lblPasswordChangeMsg.Visible = true;
            }
        }

         // Проверка текущего пароля пользователя
        private bool CheckCurrentPassword(string currentPassword)
        {
            if (Session["UserId"] == null)
                 return false;

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True;Connect Timeout=30;";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand("SELECT COUNT(*) FROM CustomerAuthTbl WHERE CustId = @CustId AND CustPassword = @Password", connection);
                cmd.Parameters.AddWithValue("@CustId", (int)Session["UserId"]);
                cmd.Parameters.AddWithValue("@Password", currentPassword);

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

         // Проверка уникальности нового email
        private bool IsEmailUnique(string email)
        {
             if (Session["UserId"] == null)
                 return false; // Или true, зависит от желаемой логики при отсутствии пользователя в сессии

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True;Connect Timeout=30;";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Проверяем, существует ли email у другого пользователя
                var cmd = new SqlCommand("SELECT COUNT(*) FROM CustomerAuthTbl WHERE CustEmail = @Email AND CustId <> @CustId", connection);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@CustId", (int)Session["UserId"]);

                int count = (int)cmd.ExecuteScalar();
                return count == 0;
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

        private bool ValidateInput()
        {
            return ValidateName(txtName.Text) &&
                   ValidatePhone(txtPhone.Text) &&
                   ValidateCity(ddlCity.SelectedValue);
        }

        private bool ValidateName(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            var parts = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
                return false;

            foreach (var part in parts)
            {
                if (!Regex.IsMatch(part, @"^[A-ZА-ЯЁ][a-zа-яё-]*$"))
                    return false;
            }

            return true;
        }

        private bool ValidatePhone(string text)
        {
            return Regex.IsMatch(text, @"^80\d{9}$");
        }

        private bool ValidateCity(string city)
        {
            return !string.IsNullOrEmpty(city) && city != "Выберите город";
        }

        private void SaveProfileData(string newEmail, string newPassword)
        {
             if (Session["UserId"] == null)
            {
                 throw new Exception("User ID not found in session."); // Бросаем исключение, если UserId отсутствует
            }

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True;Connect Timeout=30;";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(); // Начинаем транзакцию

                try
                {
                    // Обновление основных данных профиля в CustomerTbl
                    var updateCustomerCmd = new SqlCommand(
                        "UPDATE CustomerTbl SET CustName = @Name, CustPhone = @Phone, CustAdd = @City WHERE CustId = @CustId", connection, transaction);

                    updateCustomerCmd.Parameters.AddWithValue("@Name", txtName.Text);
                    updateCustomerCmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    updateCustomerCmd.Parameters.AddWithValue("@City", ddlCity.SelectedValue);
                    updateCustomerCmd.Parameters.AddWithValue("@CustId", (int)Session["UserId"]);

                    updateCustomerCmd.ExecuteNonQuery();

                    // Обновление данных авторизации в CustomerAuthTbl (если менялись email или пароль)
                    if (!string.IsNullOrEmpty(newEmail) || !string.IsNullOrEmpty(newPassword))
                    {
                        string updateAuthQuery = "UPDATE CustomerAuthTbl SET ";
                        bool firstField = true;

                        if (!string.IsNullOrEmpty(newEmail))
                        {
                            updateAuthQuery += "CustEmail = @NewEmail";
                            firstField = false;
                        }
                        if (!string.IsNullOrEmpty(newPassword))
                        {
                            if (!firstField) updateAuthQuery += ", ";
                            updateAuthQuery += "CustPassword = @NewPassword";
                        }

                        updateAuthQuery += " WHERE CustId = @CustId";

                        var updateAuthCmd = new SqlCommand(updateAuthQuery, connection, transaction);

                        if (!string.IsNullOrEmpty(newEmail))
                        {
                            updateAuthCmd.Parameters.AddWithValue("@NewEmail", newEmail);
                        }
                        if (!string.IsNullOrEmpty(newPassword))
                        {
                            updateAuthCmd.Parameters.AddWithValue("@NewPassword", newPassword);
                        }
                        updateAuthCmd.Parameters.AddWithValue("@CustId", (int)Session["UserId"]);

                        updateAuthCmd.ExecuteNonQuery();
                    }

                    transaction.Commit(); // Подтверждаем транзакцию
                }
                catch (Exception)
                {
                    transaction.Rollback(); // Откатываем транзакцию в случае ошибки
                    throw; // Повторно бросаем исключение для обработки в вызывающем коде
                }
            }
        }
    }
}
