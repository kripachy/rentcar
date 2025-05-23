using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using WebApplication1.Models;
using System.Net.Mail;
using System.IO;
using System.Net;

namespace WebApplication1.view.admin
{
    public partial class AstonMartinVanquish : System.Web.UI.Page
    {
        // Добавляем строку подключения к базе данных
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "text/html; charset=utf-8";
            Response.Charset = "utf-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.HeaderEncoding = Encoding.UTF8;

            if (!IsPostBack)
            {
                LoadCarDetails();
                CheckUserRentals();
            }
        }

        private void LoadCarDetails()
        {
            Functions functions = new Functions();
            string query = "SELECT Price FROM CarTbl WHERE Brand = 'Aston Martin' AND Model = 'Vanquish' AND Color = 'White'";
            DataTable dt = functions.GetData(query);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                decimal price = Convert.ToDecimal(row["Price"]);
                lblPrice.Text = price.ToString("0.00");
            }
        }

        protected void btnRent_Click(object sender, EventArgs e)
        {
            // Валидация выбранных дат и времени
            DateTime startDate;
            DateTime endDate;

            if (string.IsNullOrWhiteSpace(txtStartDate.Text) || string.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                ShowRentalMessage("Пожалуйста, выберите даты и время начала и конца аренды.", false);
                return;
            }

            if (!DateTime.TryParse(txtStartDate.Text, out startDate) || !DateTime.TryParse(txtEndDate.Text, out endDate))
            {
                ShowRentalMessage("Неверный формат даты или времени.", false);
                return;
            }

            // Получаем текущее время с точностью до минут
            DateTime currentTime = DateTime.Now;
            currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 
                                     currentTime.Hour, currentTime.Minute, 0);

            // Проверка, что дата и время начала не в прошлом
            if (startDate < currentTime)
            {
                ShowRentalMessage("Нельзя выбрать прошедшие дату или время. Пожалуйста, выберите время не раньше текущего момента.", false);
                return;
            }

            // Проверка, что дата конца после даты начала
            if (endDate <= startDate)
            {
                ShowRentalMessage("Дата и время конца аренды должны быть позже даты и времени начала.", false);
                return;
            }

            // Проверка минимальной длительности аренды (минимум 24 часа)
            TimeSpan rentalDuration = endDate - startDate;
            if (rentalDuration.TotalHours < 24)
            {
                ShowRentalMessage("Минимальная длительность аренды - 24 часа.", false);
                return;
            }

            // === Логика сохранения аренды ===
            try
            {
                // 1. Получаем CustId текущего пользователя
                int? custId = GetCurrentUserId();
                if (!custId.HasValue)
                {
                    ShowRentalMessage("Ошибка: Пользователь не авторизован. Пожалуйста, войдите в систему.", false); // Улучшенное сообщение
                    return;
                }

                // 2. Находим доступный автомобиль "Aston Martin Vanquish" белого цвета на выбранные даты
                string carPlateNum = GetAvailableCarPlate("Aston Martin", "Vanquish", "White", startDate, endDate);
                if (string.IsNullOrEmpty(carPlateNum))
                {
                    ShowRentalMessage("Извините, нет доступных Aston Martin Vanquish на выбранные даты.", false);
                    return;
                }

                // 3. Рассчитываем Fees
                decimal dailyPrice = 0;
                 if (!decimal.TryParse(lblPrice.Text.Replace(",", "."), out dailyPrice))
                 {
                      // Получаем цену из БД еще раз для надежности, если не удалось спарсить из лейбла
                      dailyPrice = GetCarPrice("Aston Martin", "Vanquish", "White"); // Новый метод для получения цены из БД
                      if (dailyPrice <= 0)
                      {
                           ShowRentalMessage("Не удалось получить цену автомобиля из базы данных.", false);
                           return;
                      }
                 }

                 double numberOfDays = rentalDuration.TotalDays;
                 // Округляем дни до большего целого, если есть дробная часть, для расчета стоимости
                 int rentalDays = (int)Math.Ceiling(numberOfDays);
                 if (rentalDays < 1 && rentalDuration.TotalHours > 0) rentalDays = 1; // Аренда меньше дня, но больше 0 часов, считаем как за 1 день
                 else if (rentalDays < 0) rentalDays = 0; // Не должно происходить из-за валидации дат

                 int fees = (int)(dailyPrice * rentalDays);

                 // 4. Определяем следующий RentId
                 int nextRentId = GetNextRentId();

                 // 5. Вставляем запись в RentTbl
                 string insertRentQuery = "INSERT INTO RentTbl (RentId, Car, Customer, RentDate, ReturnDate, Fees) " +
                                          "VALUES (@RentId, @Car, @Customer, @RentDate, @ReturnDate, @Fees)";

                 using (SqlConnection conn = new SqlConnection(connectionString))
                 {
                     conn.Open();
                     // Используем транзакцию для надежности
                     SqlTransaction transaction = conn.BeginTransaction();

                     try
                     {
                         using (SqlCommand cmd = new SqlCommand(insertRentQuery, conn, transaction))
                         {
                             cmd.Parameters.AddWithValue("@RentId", nextRentId);
                             cmd.Parameters.AddWithValue("@Car", carPlateNum);
                             cmd.Parameters.AddWithValue("@Customer", custId.Value);
                             cmd.Parameters.AddWithValue("@RentDate", startDate.Date); // Вставляем только дату
                             cmd.Parameters.AddWithValue("@ReturnDate", endDate.Date);   // Вставляем только дату
                             cmd.Parameters.AddWithValue("@Fees", fees);
                             cmd.ExecuteNonQuery();
                         }

                         // 6. Обновляем статус доступности автомобиля в CarTbl
                         string updateCarAvailabilityQuery = "UPDATE CarTbl SET Status = 'Unavailable' WHERE CPlateNum = @CPlateNum";
                         using (SqlCommand cmd = new SqlCommand(updateCarAvailabilityQuery, conn, transaction))
                         {
                             cmd.Parameters.AddWithValue("@CPlateNum", carPlateNum);
                             cmd.ExecuteNonQuery();
                         }

                         transaction.Commit(); // Подтверждаем транзакцию
                         ShowRentalMessage("Автомобиль успешно арендован!", true);

                         // Добавляем текст инструкции после успешной аренды
                         string instructionsMessage = "<br/>Вам на почту придет примерный договор аренды. Вы сможете подъехать к нам, подписать его и забрать машину.";
                         lblRentalMessage.Text += instructionsMessage;

                         // Get user email and send the agreement
                         int currentCustId = custId.Value; // Assuming custId has a value here
                         string userEmail = GetUserEmail(currentCustId);

                         if (!string.IsNullOrEmpty(userEmail))
                         {
                             string agreementFilePath = @"C:\Users\kiril\OneDrive\Документы\rentcar\WebApplication1\WebApplication1\colorcars\Договор_аренды_WheelDeal.docx";
                             SendRentalAgreementEmail(userEmail, agreementFilePath);
                         }

                     }
                     catch (Exception exT)
                     {
                         transaction.Rollback(); // Откатываем транзакцию в случае ошибки
                         throw exT; // Повторно бросаем исключение для обработки во внешнем catch блоке
                     }
                 }
            }
            catch (Exception ex)
            {
                ShowRentalMessage("Ошибка при оформлении аренды: " + ex.Message, false);
            }
        }

        private void ShowRentalMessage(string message, bool isSuccess)
        {
            lblRentalMessage.Text = message;
            lblRentalMessage.CssClass = isSuccess ? "d-block text-center mt-2 text-success" : "d-block text-center mt-2 text-danger";
            lblRentalMessage.Visible = true;
        }

         // Метод для получения CustId текущего пользователя из сессии
        private int? GetCurrentUserId()
        {
            // Получаем UserId из сессии, который должен быть установлен при входе обычного пользователя
            if (Session["UserId"] != null)
            {
                return (int)Session["UserId"];
            }
            // Если UserId нет в сессии, возможно, пользователь не авторизован как обычный пользователь.
            return null;
        }

        // Метод для получения email пользователя по CustId
        private string GetUserEmail(int custId)
        {
            string email = null;
            // Use the same connection string as the rest of the page
            // string authConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True;Connect Timeout=30;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT CustEmail FROM CustomerAuthTbl WHERE CustId = @CustId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustId", custId);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        email = result.ToString();
                    }
                }
            }
            return email;
        }

        // Метод для отправки договора аренды по email
        private void SendRentalAgreementEmail(string recipientEmail, string attachmentPath)
        {
            try
            {
                // Basic email validation
                if (string.IsNullOrWhiteSpace(recipientEmail) || !recipientEmail.Contains("@"))
                {
                    // Log or handle invalid email
                    System.Diagnostics.Debug.WriteLine("Invalid recipient email address.");
                    return;
                }

                // Check if the attachment file exists
                if (!File.Exists(attachmentPath))
                {
                     System.Diagnostics.Debug.WriteLine($"Rental agreement file not found at: {attachmentPath}");
                     // ShowRentalMessage("Автомобиль успешно арендован! Но не удалось отправить договор аренды (файл не найден).", false); // Inform user about email failure
                    return; // Don't throw an error, just don't send the email
                }

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("your_email@example.com", "WheelDeal Rentals"); // Replace with your sender email and name
                    mail.To.Add(recipientEmail);
                    mail.Subject = "Ваш договор аренды автомобиля WheelDeal";
                    mail.Body = "Здравствуйте,\n\nБлагодарим вас за аренду автомобиля в WheelDeal. В приложении к этому письму вы найдете копию вашего договора аренды.\n\nС уважением,\nКоманда WheelDeal";
                    mail.IsBodyHtml = false; // Set to true if using HTML body

                    // Attach the document
                    Attachment attachment = new Attachment(attachmentPath);
                    mail.Attachments.Add(attachment);

                    // Configure SMTP client - REPLACE WITH YOUR SMTP SERVER DETAILS
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)) // Replace with your SMTP server and port
                    {
                        smtp.Credentials = new NetworkCredential("wheeldeal989@gmail.com", "xqwj lscl uvgw gusf"); // Replace with your email credentials
                        smtp.EnableSsl = true; // Set to true if your SMTP server requires SSL/TLS
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // Ensure sending over network

                        smtp.Send(mail);
                    }

                     System.Diagnostics.Debug.WriteLine($"Rental agreement email sent to {recipientEmail}");
                     // You might want to show a message indicating email sent successfully if needed, but maybe not on the rental page itself.
                     // ShowRentalMessage("Автомобиль успешно арендован и договор отправлен на ваш email!", true);
                }
            }
            catch (Exception ex)
            {
                // Log the email sending error
                System.Diagnostics.Debug.WriteLine($"Error sending rental agreement email: {ex.Message}");
                // Optionally inform the user that the email sending failed, but avoid showing internal errors
                 // ShowRentalMessage("Автомобиль успешно арендован! Но не удалось отправить договор аренды по email.", false);
            }
        }

        // Метод для получения цены автомобиля из CarTbl
        private decimal GetCarPrice(string brand, string model, string color)
        {
             decimal price = 0;
             using (SqlConnection conn = new SqlConnection(connectionString))
             {
                 conn.Open();
                 string query = "SELECT Price FROM CarTbl WHERE Brand = @Brand AND Model = @Model AND Color = @Color";
                 using (SqlCommand cmd = new SqlCommand(query, conn))
                 {
                     cmd.Parameters.AddWithValue("@Brand", brand);
                     cmd.Parameters.AddWithValue("@Model", model);
                     cmd.Parameters.AddWithValue("@Color", color);
                     object result = cmd.ExecuteScalar();
                     if (result != null && result != DBNull.Value)
                     {
                         price = Convert.ToDecimal(result);
                     }
                 }
             }
             return price;
        }

        // Метод для поиска доступного автомобиля по марке, модели, цвету и периоду аренды
        private string GetAvailableCarPlate(string brand, string model, string color, DateTime startDate, DateTime endDate)
        {
            string carPlate = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Находим автомобиль по марке, модели, цвету и статусу доступности
                string findCarQuery = "SELECT TOP 1 CPlateNum FROM CarTbl WHERE Brand = @Brand AND Model = @Model AND Color = @Color AND Status = 'Available'";
                using (SqlCommand cmd = new SqlCommand(findCarQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Brand", brand);
                    cmd.Parameters.AddWithValue("@Model", model);
                    cmd.Parameters.AddWithValue("@Color", color);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        carPlate = result.ToString();
                    }
                }

                // Проверяем, не забронирован ли этот автомобиль на выбранные даты
                if (carPlate != null)
                {
                    // Проверяем наличие пересекающихся аренд в RentTbl
                    string checkRentQuery = "SELECT COUNT(*) FROM RentTbl WHERE Car = @CPlateNum " +
                                            "AND ( (RentDate <= @EndDate AND ReturnDate >= @StartDate) OR (RentDate <= @EndDate AND ReturnDate IS NULL) )"; // Проверяем на пересечение дат или активную аренду без даты возврата
                    using (SqlCommand cmd = new SqlCommand(checkRentQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CPlateNum", carPlate);
                        cmd.Parameters.AddWithValue("@StartDate", startDate.Date);
                        cmd.Parameters.AddWithValue("@EndDate", endDate.Date);
                        int activeRentals = (int)cmd.ExecuteScalar();

                        if (activeRentals > 0)
                        {
                            // Автомобиль уже забронирован на эти даты
                            carPlate = null; // Сбрасываем, так как он не доступен в этот период
                        }
                    }
                }
            }
             return carPlate; // Вернет номер машины, если доступна и не забронирована, иначе null
        }

        // Метод для получения следующего RentId
        private int GetNextRentId()
        {
            int maxId = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ISNULL(MAX(RentId), 0) FROM RentTbl";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        maxId = Convert.ToInt32(result);
                    }
                }
            }
            return maxId + 1;
        }

        private void CheckUserRentals()
        {
            int? custId = GetCurrentUserId();
            if (!custId.HasValue)
            {
                btnRent.Visible = false;
                lblRentalMessage.Text = "Пожалуйста, войдите в систему для аренды автомобиля.";
                lblRentalMessage.CssClass = "d-block text-center mt-2 text-danger";
                lblRentalMessage.Visible = true;
                return;
            }

            // Если пользователь авторизован, показываем кнопку аренды
            btnRent.Visible = true;
            lblRentalMessage.Visible = false;
        }
    }
} 