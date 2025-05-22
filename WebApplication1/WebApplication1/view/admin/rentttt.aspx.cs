using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;

namespace WebApplication1.view.admin
{
    public partial class rentttt : System.Web.UI.Page
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True";
        private Dictionary<string, string> specificCarImages = new Dictionary<string, string>
        {
            {"Aston Martin Vanquish", "~/colorcars/Aston Martin Vanquish/white/1.jpg"},
            {"Audi TT", "~/colorcars/Audi TT/blue/1.jpg"},
            {"Chevrolet Camaro", "~/colorcars/Chevrolet Camaro/yellow/1.jpg"},
            {"Ford Mustang S550", "~/colorcars/Ford Mustang S550/orange/1.jpg"},
            {"Jaguar XJ", "~/colorcars/Jaguar XJ/black/1.jpg"},
            {"Lamborghini Huracan", "~/colorcars/Lamborghini Huracan/purple/1.jpg"},
            {"Maserati GranTurismo", "~/colorcars/Maserati GranTurismo/yellow/2.jpg"},
            {"Porsche 911", "~/colorcars/Porsche 911/green/1.jpeg"}
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRentals();
            }
        }

        protected void GridViewRents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CancelRent")
            {
                int rentId = Convert.ToInt32(e.CommandArgument);
                CancelRental(rentId);
            }
        }

        private void CancelRental(int rentId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string getRentInfoQuery = @"SELECT r.Car, r.Customer, c.CustEmail 
                                              FROM RentTbl r 
                                              INNER JOIN CustomerAuthTbl c ON r.Customer = c.CustId 
                                              WHERE r.RentId = @RentId";
                    
                    string carPlate = null;
                    string userEmail = null;

                    using (SqlCommand cmd = new SqlCommand(getRentInfoQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@RentId", rentId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                carPlate = reader["Car"].ToString();
                                userEmail = reader["CustEmail"].ToString();
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(carPlate))
                    {
                        throw new Exception("Аренда не найдена");
                    }

                    string deleteRentQuery = "DELETE FROM RentTbl WHERE RentId = @RentId";
                    using (SqlCommand cmd = new SqlCommand(deleteRentQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@RentId", rentId);
                        cmd.ExecuteNonQuery();
                    }

                    string updateCarStatusQuery = "UPDATE CarTbl SET Status = 'Available' WHERE CPlateNum = @CarPlate";
                    using (SqlCommand cmd = new SqlCommand(updateCarStatusQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@CarPlate", carPlate);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    if (!string.IsNullOrEmpty(userEmail))
                    {
                        SendCancellationEmail(userEmail, carPlate);
                    }

                    ShowMessage("Аренда успешно отменена", true);
                    LoadRentals();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ShowMessage("Ошибка при отмене аренды: " + ex.Message, false);
                }
            }
        }

        private void SendCancellationEmail(string userEmail, string carPlate)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("wheeldeal989@gmail.com", "WheelDeal Rentals");
                    mail.To.Add(userEmail);
                    mail.Subject = "Отмена аренды автомобиля";
                    mail.Body = $"Здравствуйте,\n\nВаша аренда автомобиля {carPlate} была отменена.\n\nС уважением,\nКоманда WheelDeal";
                    mail.IsBodyHtml = false;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("wheeldeal989@gmail.com", "xqwj lscl uvgw gusf");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sending cancellation email: {ex.Message}");
            }
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isSuccess ? "alert alert-success" : "alert alert-danger";
            lblMessage.Visible = true;
        }

        protected void LoadRentals()
        {
            int? custId = GetCurrentUserId();
            if (!custId.HasValue)
            {
                ShowMessage("Пожалуйста, войдите в систему", false);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT r.RentId, r.Car, r.RentDate, r.ReturnDate, r.Fees 
                               FROM RentTbl r 
                               WHERE r.Customer = @CustId 
                               AND r.ReturnDate >= GETDATE() 
                               ORDER BY r.RentDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustId", custId.Value);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        GridViewRents.DataSource = dt;
                        GridViewRents.DataBind();
                    }
                }
            }
        }

        private int? GetCurrentUserId()
        {
            if (Session["UserId"] != null)
            {
                return (int)Session["UserId"];
            }
            return null;
        }

        protected string GetCarImageUrl(object carPlate)
        {
            if (carPlate == null) return "";
            string plate = carPlate.ToString();
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Brand, Model, Color FROM CarTbl WHERE CPlateNum = @CarPlate";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CarPlate", plate);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string brand = reader["Brand"].ToString();
                            string model = reader["Model"].ToString();
                            string color = reader["Color"].ToString();
                            return $"../../colorcars/{brand} {model}/{color}/1.jpg";
                        }
                    }
                }
            }
            return "";
        }

        protected string GetCarDetails(object carPlate)
        {
            if (carPlate == null) return "";
            string plate = carPlate.ToString();
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Brand, Model, Color FROM CarTbl WHERE CPlateNum = @CarPlate";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CarPlate", plate);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string brand = reader["Brand"].ToString();
                            string model = reader["Model"].ToString();
                            string color = reader["Color"].ToString();
                            return $"{brand} {model} ({color})";
                        }
                    }
                }
            }
            return "";
        }

        protected void GridViewRents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblRemainingTime = (Label)e.Row.FindControl("lblRemainingTime");
                if (lblRemainingTime != null)
                {
                    DateTime returnDate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "ReturnDate"));
                    TimeSpan remaining = returnDate - DateTime.Now;
                    
                    if (remaining.TotalDays > 0)
                    {
                        lblRemainingTime.Text = $"{(int)remaining.TotalDays} дн. {remaining.Hours} ч.";
                    }
                    else if (remaining.TotalHours > 0)
                    {
                        lblRemainingTime.Text = $"{(int)remaining.TotalHours} ч. {remaining.Minutes} мин.";
                    }
                    else
                    {
                        lblRemainingTime.Text = $"{(int)remaining.TotalMinutes} мин.";
                    }
                }
            }
        }
    }
}