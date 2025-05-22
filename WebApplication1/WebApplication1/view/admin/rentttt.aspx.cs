using System;
using System.Data;
using System.Data.SqlClient;
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
                LoadUserRents();
            }
        }

        private void LoadUserRents()
        {
            // Получаем CustId текущего пользователя из сессии
            int? custId = GetCurrentUserId();
            if (!custId.HasValue)
            {
                // Если пользователь не авторизован, можно перенаправить на страницу входа
                Response.Redirect("~/view/admin/login.aspx");
                return;
            }

            // Запрос для получения активных аренд пользователя с информацией об автомобиле
            string query = @"SELECT r.Car, c.Brand, c.Model, c.Color, r.RentDate, r.ReturnDate, r.Fees 
                           FROM RentTbl r 
                           JOIN CarTbl c ON r.Car = c.CPlateNum 
                           WHERE r.Customer = @CustId AND (r.ReturnDate >= GETDATE() OR r.ReturnDate IS NULL)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustId", custId.Value);
                    conn.Open();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    GridViewRents.DataSource = dt;
                    GridViewRents.DataBind();

                    // После привязки данных, рассчитываем и отображаем оставшееся время
                    CalculateAndDisplayRemainingTime();
                }
            }
        }

        protected string GetCarImageUrl(object carPlate)
        {
            if (carPlate == null) return ResolveUrl("~/images/default_car.png");

            string carPlateStr = carPlate.ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Brand, Model FROM CarTbl WHERE CPlateNum = @CarPlate";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CarPlate", carPlateStr);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string brand = reader["Brand"].ToString();
                            string model = reader["Model"].ToString();
                            string carName = $"{brand} {model}";

                            if (specificCarImages.ContainsKey(carName))
                            {
                                return ResolveUrl(specificCarImages[carName]);
                            }
                        }
                    }
                }
            }
            return ResolveUrl("~/images/default_car.png");
        }

        protected string GetCarDetails(object carPlate)
        {
            if (carPlate == null) return string.Empty;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Brand, Model, Color FROM CarTbl WHERE CPlateNum = @CarPlate";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CarPlate", carPlate.ToString());
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string brand = reader["Brand"].ToString();
                            string model = reader["Model"].ToString();
                            string color = reader["Color"].ToString();
                            return $"{brand} {model} • {color}";
                        }
                    }
                }
            }
            return string.Empty;
        }

        // Метод для получения CustId текущего пользователя из сессии
        private int? GetCurrentUserId()
        {
            // Получаем UserId из сессии, который должен быть установлен при входе обычного пользователя
            if (Session["UserId"] != null)
            {
                return (int)Session["UserId"];
            }
            return null;
        }

         // Метод для расчета и отображения оставшегося времени аренды
        private void CalculateAndDisplayRemainingTime()
        {
            foreach (GridViewRow row in GridViewRents.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    // Получаем дату окончания аренды из BoundField
                    DateTime returnDate;
                    if (DateTime.TryParse(row.Cells[2].Text, out returnDate))
                    {
                        Label lblRemainingTime = (Label)row.FindControl("lblRemainingTime");
                        if (lblRemainingTime != null)
                        {
                            TimeSpan remaining = returnDate - DateTime.Now;

                            if (remaining.TotalSeconds > 0)
                            {
                                if (remaining.TotalDays >= 1)
                                {
                                    lblRemainingTime.Text = $"{remaining.Days} д. {remaining.Hours} ч.";
                                } else if (remaining.TotalHours >= 1)
                                {
                                     lblRemainingTime.Text = $"{remaining.Hours} ч. {remaining.Minutes} мин.";
                                } else if (remaining.TotalMinutes >= 1)
                                {
                                    lblRemainingTime.Text = $"{remaining.Minutes} мин. {remaining.Seconds} сек.";
                                } else
                                {
                                    lblRemainingTime.Text = $"{remaining.Seconds} сек.";
                                }

                            } else
                            {
                                lblRemainingTime.Text = "Срок истек";
                                lblRemainingTime.CssClass = "text-danger";
                            }
                        }
                    }
                    else
                    {
                        // Если ReturnDate NULL или не парсится, возможно, это активная аренда без указанной даты конца
                        Label lblRemainingTime = (Label)row.FindControl("lblRemainingTime");
                         if (lblRemainingTime != null)
                         {
                            lblRemainingTime.Text = "Активная аренда";
                         }
                    }
                }
            }
        }

    }
}