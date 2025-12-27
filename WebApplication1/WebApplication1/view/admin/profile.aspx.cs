using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1
{
    public partial class profile : System.Web.UI.Page
    {
        private FileStorageService _storage => new FileStorageService(server: Server);
        protected global::System.Web.UI.WebControls.Panel pnlLicensePreview;
        protected global::System.Web.UI.WebControls.Panel pnlUploadLicense;
        protected global::System.Web.UI.WebControls.Image imgLicensePreview;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserEmail"] == null || Session["UserId"] == null)
                {
                    Response.Redirect("~/view/admin/login.aspx");
                    return;
                }

                EnsureProfileSchema();

                // Проверяем, только ли что пользователь зарегистрировался
                bool justRegistered = Session["JustRegistered"] != null && (bool)Session["JustRegistered"];

                if (!justRegistered)
                {
                     // Загружаем текущие данные пользователя только если это не первый визит после регистрации
                    LoadUserData();
                    LoadLicenseData();
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
                 Response.Redirect("~/view/admin/login.aspx"); // Перенаправляем на логин, если UserId нет в сессии
                 return;
            }

            string connectionString = WebApplication1.Models.Functions.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand("SELECT CustName, CustPhone, CustAdd, CustUserName FROM CustomerTbl WHERE CustId = @CustId", connection);
                cmd.Parameters.AddWithValue("@CustId", (int)Session["UserId"]); // Явно приводим к int

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtUsername.Text = reader["CustUserName"] == DBNull.Value ? string.Empty : reader["CustUserName"].ToString();
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
                 Response.Redirect("~/view/admin/login.aspx"); // Перенаправляем на логин, если UserId нет в сессии
                 return;
            }

            // Сбрасываем сообщение об ошибке/успехе
            lblPasswordChangeMsg.Visible = false;
            lblPasswordChangeMsg.CssClass = "d-block text-center";

            // Валидация основных полей профиля (ник, ФИО, телефон, город)
            if (!ValidateInput())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Пожалуйста, введите корректную информацию: ник, ФИО, телефон, город.');", true);
                return;
            }

            string userName = txtUsername.Text.Trim();
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
                    SaveProfileData(null, null, userName); // Передаем null, так как email и пароль не меняются
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
                 SaveProfileData(emailChanged ? newEmail : null, passwordChanged ? newPassword : null, userName);
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

            string connectionString = WebApplication1.Models.Functions.GetConnectionString();

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

            string connectionString = WebApplication1.Models.Functions.GetConnectionString();

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
            return ValidateUsername(txtUsername.Text) &&
                   ValidateName(txtName.Text) &&
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

        private bool ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                // Ник может быть пустым, но подсказка на главной попросит заполнить
                return true;
            }

            if (username.Length < 3 || username.Length > 30)
                return false;

            // Разрешаем буквы, цифры, подчеркивание и дефис
            return Regex.IsMatch(username, @"^[A-Za-zА-Яа-яЁё0-9_\-]+$");
        }

        protected void btnUploadLicense_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Пожалуйста, войдите в систему.');", true);
                return;
            }

            try
            {
                if (!fuLicense.HasFile)
                {
                    lblLicenseStatus.Text = "Добавьте скан/фото водительского удостоверения.";
                    lblLicenseStatus.CssClass = "text-danger";
                    return;
                }

                DateTime? expiryDate = string.IsNullOrWhiteSpace(txtLicenseExpiryDate.Text) ? (DateTime?)null : DateTime.Parse(txtLicenseExpiryDate.Text);

                int? uploader = Session["UserId"] != null ? (int?)Convert.ToInt32(Session["UserId"]) : null;
                int fileId = _storage.Save(new HttpPostedFileWrapper(fuLicense.PostedFile), "driver-license", uploader);

                using (var conn = new SqlConnection(Functions.GetConnectionString()))
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            var insert = new SqlCommand(@"
INSERT INTO DrivingLicense (CustomerId, LicenseNumber, IssuedDate, ExpiryDate, Status, FileId, VerifiedByAI, Comment)
OUTPUT INSERTED.LicenseId
VALUES (@custId, NULL, NULL, @expiry, 'Pending', @fileId, 0, @comment)", conn, tran);
                            insert.Parameters.AddWithValue("@custId", (int)Session["UserId"]);
                            insert.Parameters.AddWithValue("@expiry", (object)expiryDate ?? DBNull.Value);
                            insert.Parameters.AddWithValue("@fileId", fileId);
                            insert.Parameters.AddWithValue("@comment", "Отправлено пользователем, ожидает проверки.");
                            insert.ExecuteScalar();

                            // Note: Experience calculation based on document date removed as per user request.
                            // The admin will now manually decide categories.
                            
                            tran.Commit();
                        }
                        catch
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                }

                lblLicenseStatus.Text = "Лицензия загружена. Статус: ожидает проверки админом или ИИ.";
                lblLicenseStatus.CssClass = "text-success";
                LoadLicenseData();
            }
            catch (Exception ex)
            {
                lblLicenseStatus.Text = "Ошибка загрузки удостоверения: " + ex.Message;
                lblLicenseStatus.CssClass = "text-danger";
            }
        }

        private void LoadLicenseData()
        {
            if (Session["UserId"] == null) return;

            try
            {
                using (var conn = new SqlConnection(Functions.GetConnectionString()))
                {
                    conn.Open();
                    var cmd = new SqlCommand(@"
SELECT TOP 1 LicenseNumber, IssuedDate, ExpiryDate, Status, Comment, FileId
FROM DrivingLicense
WHERE CustomerId = @custId
ORDER BY CreatedAt DESC", conn);
                    cmd.Parameters.AddWithValue("@custId", (int)Session["UserId"]);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtLicenseExpiryDate.Text = reader["ExpiryDate"] == DBNull.Value ? string.Empty : Convert.ToDateTime(reader["ExpiryDate"]).ToString("yyyy-MM-dd");
                            string status = reader["Status"].ToString();
                            string comment = reader["Comment"] == DBNull.Value ? "" : reader["Comment"].ToString();
                            
                            string statusText = "Статус: ";
                            if (status == "Pending") 
                            {
                                statusText += "🕒 Ожидает проверки";
                                lblLicenseStatus.Text = $"{statusText}. {comment}";
                            }
                            else if (status == "Approved") 
                            {
                                statusText += "✅ Одобрено";
                                
                                // Fetch allowed classes
                                var allowedClasses = new System.Collections.Generic.List<string>();
                                using (var conn_inner = new SqlConnection(Functions.GetConnectionString()))
                                {
                                    conn_inner.Open();
                                    var cmd_inner = new SqlCommand(@"
                                        SELECT cc.Name 
                                        FROM CustomerAllowedCategory cac
                                        JOIN CarCategory cc ON cac.CategoryId = cc.CategoryId
                                        WHERE cac.CustomerId = @custId", conn_inner);
                                    cmd_inner.Parameters.AddWithValue("@custId", (int)Session["UserId"]);
                                    using (var reader_inner = cmd_inner.ExecuteReader())
                                    {
                                        while (reader_inner.Read()) allowedClasses.Add(reader_inner["Name"].ToString());
                                    }
                                }

                                if (allowedClasses.Count > 0)
                                {
                                    string classesText = string.Join(", ", allowedClasses);
                                    lblLicenseStatus.Text = $"{statusText}. Вам доступны автомобили следующих классов: {classesText}.";
                                }
                                else
                                {
                                    lblLicenseStatus.Text = $"{statusText}. Доступные классы пока не назначены администратором.";
                                }
                            }
                            else if (status == "Rejected") 
                            {
                                statusText += "❌ Отклонено";
                                lblLicenseStatus.Text = $"{statusText}. {comment}";
                            }
                            else 
                            {
                                statusText += status;
                                lblLicenseStatus.Text = $"{statusText}. {comment}";
                            }

                            lblLicenseStatus.CssClass = status.Equals("Approved", StringComparison.OrdinalIgnoreCase) ? "text-success" : "text-primary";

                            if (reader["FileId"] != DBNull.Value)
                            {
                                int fileId = Convert.ToInt32(reader["FileId"]);
                                imgLicensePreview.ImageUrl = "/ImageHandler.ashx?id=" + fileId;
                                pnlLicensePreview.Visible = true;
                                pnlUploadLicense.Visible = false;
                            }
                            else
                            {
                                pnlLicensePreview.Visible = false;
                                pnlUploadLicense.Visible = true;
                            }
                        }
                        else
                        {
                            lblLicenseStatus.Text = "Документы ещё не загружены.";
                            lblLicenseStatus.CssClass = "text-muted";
                            pnlLicensePreview.Visible = false;
                            pnlUploadLicense.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblLicenseStatus.Text = "Ошибка загрузки данных лицензии: " + ex.Message;
                lblLicenseStatus.CssClass = "text-danger";
            }
        }

        protected void btnDeleteLicense_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] == null) return;

            try
            {
                using (var conn = new SqlConnection(Functions.GetConnectionString()))
                {
                    conn.Open();
                    // Get FileId to delete physical file too
                    var getFile = new SqlCommand("SELECT TOP 1 FileId FROM DrivingLicense WHERE CustomerId = @custId ORDER BY CreatedAt DESC", conn);
                    getFile.Parameters.AddWithValue("@custId", (int)Session["UserId"]);
                    object fileIdObj = getFile.ExecuteScalar();

                    // Delete record
                    var del = new SqlCommand("DELETE FROM DrivingLicense WHERE CustomerId = @custId", conn);
                    del.Parameters.AddWithValue("@custId", (int)Session["UserId"]);
                    del.ExecuteNonQuery();

                    // Reset experience
                    var reset = new SqlCommand("UPDATE CustomerTbl SET DrivingExperienceYears=0, LicenseIssueDate=NULL WHERE CustId=@c", conn);
                    reset.Parameters.AddWithValue("@c", (int)Session["UserId"]);
                    reset.ExecuteNonQuery();
                    
                    // Delete from CustomerAllowedCategory as well since license is gone
                    var delCat = new SqlCommand("DELETE FROM CustomerAllowedCategory WHERE CustomerId=@c", conn);
                    delCat.Parameters.AddWithValue("@c", (int)Session["UserId"]);
                    delCat.ExecuteNonQuery();

                    if (fileIdObj != null && fileIdObj != DBNull.Value)
                    {
                        // Optional: physical deletion if needed, but safe storage might keep it.
                        // For simplicity, we just remove the link in this project's logic.
                    }
                }
                
                LoadLicenseData();
                lblLicenseStatus.Text = "Документ удален. Вы можете загрузить новое фото.";
                lblLicenseStatus.CssClass = "text-info";
            }
            catch (Exception ex)
            {
                lblLicenseStatus.Text = "Ошибка при удалении: " + ex.Message;
                lblLicenseStatus.CssClass = "text-danger";
            }
        }

        private void SaveProfileData(string newEmail, string newPassword, string userName)
        {
             if (Session["UserId"] == null)
            {
                 throw new Exception("User ID not found in session."); // Бросаем исключение, если UserId отсутствует
            }

            string connectionString = WebApplication1.Models.Functions.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(); // Начинаем транзакцию

                try
                {
                    // Обновление основных данных профиля в CustomerTbl
                    var updateCustomerCmd = new SqlCommand(
                        "UPDATE CustomerTbl SET CustUserName = @UserName, CustName = @Name, CustPhone = @Phone, CustAdd = @City WHERE CustId = @CustId", connection, transaction);

                    updateCustomerCmd.Parameters.AddWithValue("@UserName", string.IsNullOrWhiteSpace(userName) ? (object)DBNull.Value : userName);
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

        private void EnsureProfileSchema()
        {
            string connectionString = WebApplication1.Models.Functions.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var addUserNameColumn = new SqlCommand(@"
                    IF COL_LENGTH('CustomerTbl', 'CustUserName') IS NULL
                    BEGIN
                        ALTER TABLE CustomerTbl ADD CustUserName NVARCHAR(100) NULL;
                    END;", connection);

                addUserNameColumn.ExecuteNonQuery();
            }
        }
    }
}
