using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.IO;
using System.Net;
using WebApplication1.Models;

namespace WebApplication1.view.admin
{
    public partial class AudiTT : System.Web.UI.Page
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False";

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "text/html; charset=utf-8";
            Response.Charset = "utf-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.HeaderEncoding = Encoding.UTF8;

            if (!IsPostBack)
            {
                // Check if user is logged in
                if (Session["UserEmail"] == null)
                {
                    // Store the current URL in session to redirect back after login
                    Session["ReturnUrl"] = Request.RawUrl;
                    Response.Redirect("~/view/login.aspx");
                }

                LoadCarDetails();
            }
            else
            {
                // Reload price when color changes
                LoadCarDetails();
            }
        }

        private void LoadCarDetails()
        {
            try
            {
                string selectedColor = hdnSelectedColor?.Value ?? "Black";
                string query = "SELECT Price FROM CarTbl WHERE Brand = 'Audi' AND Model = 'TT' AND Color = @Color";
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Color", selectedColor);
                        object result = cmd.ExecuteScalar();
                        
                        if (result != null && result != DBNull.Value)
                        {
                            lblPrice.Text = Convert.ToDecimal(result).ToString("0.00");
                        }
                        else
                        {
                            lblPrice.Text = "500.00"; // Default price if not found
                        }
                    }
                }
            }
            catch
            {
                lblPrice.Text = "500.00"; // Default price on error
            }
        }

        protected void btnRent_Click(object sender, EventArgs e)
        {
            // Get the selected color from the hidden field
            string selectedColor = hdnSelectedColor.Value;
            
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
                    ShowRentalMessage("Ошибка: Пользователь не авторизован. Пожалуйста, войдите в систему.", false);
                    return;
                }

                // 2. Находим доступный автомобиль выбранного цвета
                string carPlateNum = GetAvailableCarPlate("Audi", "TT", selectedColor, startDate, endDate);
                if (string.IsNullOrEmpty(carPlateNum))
                {
                    ShowRentalMessage($"Извините, нет доступных Audi TT {selectedColor} цвета на выбранные даты.", false);
                    return;
                }

                // 3. Рассчитываем Fees
                decimal dailyPrice = GetCarPrice("Audi", "TT", selectedColor);
                if (dailyPrice <= 0)
                {
                    ShowRentalMessage("Не удалось получить цену автомобиля из базы данных.", false);
                    return;
                }

                double numberOfDays = rentalDuration.TotalDays;
                // Округляем дни до большего целого, если есть дробная часть
                int rentalDays = (int)Math.Ceiling(numberOfDays);
                if (rentalDays < 1 && rentalDuration.TotalHours > 0) rentalDays = 1;
                else if (rentalDays < 0) rentalDays = 0;

                int fees = (int)(dailyPrice * rentalDays);

                // 4. Определяем следующий RentId
                int nextRentId = GetNextRentId();

                // 5. Вставляем запись в RentTbl
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        string insertRentQuery = "INSERT INTO RentTbl (RentId, Car, Customer, RentDate, ReturnDate, Fees) " +
                                               "VALUES (@RentId, @Car, @Customer, @RentDate, @ReturnDate, @Fees)";

                        using (SqlCommand cmd = new SqlCommand(insertRentQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@RentId", nextRentId);
                            cmd.Parameters.AddWithValue("@Car", carPlateNum);
                            cmd.Parameters.AddWithValue("@Customer", custId.Value);
                            cmd.Parameters.AddWithValue("@RentDate", startDate.Date);
                            cmd.Parameters.AddWithValue("@ReturnDate", endDate.Date);
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

                        transaction.Commit();
                        ShowRentalMessage("Автомобиль успешно арендован!", true);

                        // Добавляем текст инструкции после успешной аренды
                        string instructionsMessage = "<br/>Вам на почту придет примерный договор аренды. Вы сможете подъехать к нам, подписать его и забрать машину.";
                        lblRentalMessage.Text += instructionsMessage;

                        // Get user email and send the agreement
                        string userEmail = GetUserEmail(custId.Value);
                        if (!string.IsNullOrEmpty(userEmail))
                        {
                            string agreementFilePath = @"C:\Users\kiril\OneDrive\Документы\rentcar\WebApplication1\WebApplication1\colorcars\Договор_аренды_WheelDeal.docx";
                            SendRentalAgreementEmail(userEmail, agreementFilePath);
                        }
                    }
                    catch (Exception exT)
                    {
                        transaction.Rollback();
                        throw exT;
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

        private int? GetCurrentUserId()
        {
            if (Session["UserEmail"] == null) return null;

            string email = Session["UserEmail"].ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT CustId FROM CustomerAuthTbl WHERE CustEmail = @Email";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            return null;
        }

        private string GetUserEmail(int custId)
        {
            string email = null;
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

        private void SendRentalAgreementEmail(string recipientEmail, string attachmentPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(recipientEmail) || !recipientEmail.Contains("@"))
                {
                    System.Diagnostics.Debug.WriteLine("Invalid recipient email address.");
                    return;
                }

                if (!File.Exists(attachmentPath))
                {
                    System.Diagnostics.Debug.WriteLine($"Rental agreement file not found at: {attachmentPath}");
                    return;
                }

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("wheeldeal989@gmail.com", "WheelDeal Rentals");
                    mail.To.Add(recipientEmail);
                    mail.Subject = "Ваш договор аренды автомобиля WheelDeal";
                    mail.Body = "Здравствуйте,\n\nБлагодарим вас за аренду автомобиля в WheelDeal. В приложении к этому письму вы найдете копию вашего договора аренды.\n\nС уважением,\nКоманда WheelDeal";
                    mail.IsBodyHtml = false;

                    Attachment attachment = new Attachment(attachmentPath);
                    mail.Attachments.Add(attachment);

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("wheeldeal989@gmail.com", "xqwj lscl uvgw gusf");
                        smtp.EnableSsl = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Send(mail);
                    }

                    System.Diagnostics.Debug.WriteLine($"Rental agreement email sent to {recipientEmail}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sending rental agreement email: {ex.Message}");
            }
        }

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

        private string GetAvailableCarPlate(string brand, string model, string color, DateTime startDate, DateTime endDate)
        {
            string carPlate = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Find car by brand, model, color and availability status
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

                // Check if the car is not booked for the selected dates
                if (carPlate != null)
                {
                    string checkBookingQuery = @"SELECT COUNT(*) FROM RentTbl 
                                               WHERE Car = @CarPlate 
                                               AND ((RentDate <= @EndDate AND ReturnDate >= @StartDate))";
                    using (SqlCommand cmd = new SqlCommand(checkBookingQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CarPlate", carPlate);
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);
                        int bookingCount = Convert.ToInt32(cmd.ExecuteScalar());
                        if (bookingCount > 0)
                        {
                            carPlate = null; // Car is booked for these dates
                        }
                    }
                }
            }
            return carPlate;
        }

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
    }
} 