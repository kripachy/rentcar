using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.view.admin
{
    public partial class rentauto : Page
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True;Connect Timeout=30";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBrands();
                carImage.ImageUrl = "~/assets/images/default-car.png";
                hfCurrentImageIndex.Value = "0";
                hfImageFiles.Value = "";
            }

            if (!string.IsNullOrEmpty(hfSelectedColor.Value))
            {
                CheckColorAvailability();
            }
        }

        private void LoadBrands()
        {
            ddlBrand.Items.Clear();
            ddlBrand.Items.Add("Выберите марку");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT Brand FROM CarTbl", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ddlBrand.Items.Add(reader.GetString(0));
                }
            }
        }

        protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            string brand = ddlBrand.SelectedValue;

            if (brand == "Выберите марку")
            {
                lblModel.Text = "";
                lblPrice.Text = "";
                carImage.ImageUrl = "~/assets/images/default-car.png";
                hfCurrentImageIndex.Value = "0";
                hfImageFiles.Value = "";
                dotsContainer.Controls.Clear();
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 Model, Price FROM CarTbl WHERE Brand = @brand", conn);
                cmd.Parameters.AddWithValue("@brand", brand);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblModel.Text = reader["Model"].ToString();
                    lblPrice.Text = "$" + reader["Price"].ToString();
                }
                else
                {
                    lblModel.Text = "Модель не найдена";
                    lblPrice.Text = "";
                }
            }

            // Сброс картинок и индексов
            carImage.ImageUrl = "~/assets/images/default-car.png";
            hfCurrentImageIndex.Value = "0";
            hfImageFiles.Value = "";
            dotsContainer.Controls.Clear();
        }

        private void CheckColorAvailability()
        {
            string color = hfSelectedColor.Value.Trim().ToLower();
            string brand = ddlBrand.SelectedValue.Trim();
            string model = lblModel.Text.Trim();

            if (string.IsNullOrEmpty(model) || brand == "Выберите марку" || string.IsNullOrEmpty(color))
                return;

            bool available = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM CarTbl WHERE Brand = @brand AND Model = @model AND Color = @color AND Status = 'Available'", conn);
                cmd.Parameters.AddWithValue("@brand", brand);
                cmd.Parameters.AddWithValue("@model", model);
                cmd.Parameters.AddWithValue("@color", color);
                available = (int)cmd.ExecuteScalar() > 0;
            }

            string folderPath = Server.MapPath($"~/colorcars/{brand} {model}/{color}");
            if (!Directory.Exists(folderPath))
            {
                carImage.ImageUrl = "~/assets/images/default-car.png";
                hfImageFiles.Value = "";
                dotsContainer.Controls.Clear();
                return;
            }

            string[] imageFiles = Directory.GetFiles(folderPath, "*.jpg");
            if (available && imageFiles.Length > 0)
            {
                List<string> relativePaths = new List<string>();
                string root = Server.MapPath("~/");
                foreach (var file in imageFiles)
                {
                    string relative = "~/" + file.Substring(root.Length).Replace("\\", "/");
                    relativePaths.Add(relative);
                }

                hfImageFiles.Value = string.Join(";", relativePaths);
                hfCurrentImageIndex.Value = "0";
                LoadCurrentImage();
            }
            else
            {
                carImage.ImageUrl = "~/assets/images/default-car.png";
                hfImageFiles.Value = "";
                dotsContainer.Controls.Clear();
            }
        }

        private void LoadCurrentImage()
        {
            string[] images = hfImageFiles.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (images.Length == 0)
            {
                carImage.ImageUrl = "~/assets/images/default-car.png";
                dotsContainer.Controls.Clear();
                return;
            }

            int index;
            if (!int.TryParse(hfCurrentImageIndex.Value, out index))
                index = 0;

            if (index < 0)
                index = images.Length - 1;
            else if (index >= images.Length)
                index = 0;

            hfCurrentImageIndex.Value = index.ToString();
            carImage.ImageUrl = images[index];
            RenderDots(images.Length, index);
        }

        private void RenderDots(int count, int currentIndex)
        {
            dotsContainer.Controls.Clear();
            for (int i = 0; i < count; i++)
            {
                Button dot = new Button
                {
                    CssClass = "dot" + (i == currentIndex ? " active" : ""),
                    CommandArgument = i.ToString(),
                    ToolTip = $"Изображение {i + 1}",
                    Width = Unit.Pixel(16),
                    Height = Unit.Pixel(16),
                    BorderStyle = BorderStyle.None,
                    BackColor = System.Drawing.Color.Transparent
                };
                dot.Click += Dot_Click;
                dotsContainer.Controls.Add(dot);
            }
        }

        protected void Dot_Click(object sender, EventArgs e)
        {
            Button clickedDot = (Button)sender;
            int index = int.Parse(clickedDot.CommandArgument);
            hfCurrentImageIndex.Value = index.ToString();
            LoadCurrentImage();
        }

        protected void btnPrevImage_Click(object sender, EventArgs e)
        {
            string[] images = hfImageFiles.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (images.Length == 0) return;

            int index = int.Parse(hfCurrentImageIndex.Value);
            index = (index - 1 + images.Length) % images.Length;
            hfCurrentImageIndex.Value = index.ToString();
            LoadCurrentImage();
        }

        protected void btnNextImage_Click(object sender, EventArgs e)
        {
            string[] images = hfImageFiles.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (images.Length == 0) return;

            int index = int.Parse(hfCurrentImageIndex.Value);
            index = (index + 1) % images.Length;
            hfCurrentImageIndex.Value = index.ToString();
            LoadCurrentImage();
        }

        protected void btnRent_Click(object sender, EventArgs e)
        {
            if (Session["CustId"] == null)
            {
                carDetails.InnerText = "Вы должны войти в систему, чтобы арендовать автомобиль.";
                return;
            }

            string brand = ddlBrand.SelectedValue;
            string model = lblModel.Text;
            string color = hfSelectedColor.Value;
            string startDateTime = txtStartDate.Text + " " + txtStartTime.Text;
            string endDateTime = txtEndDate.Text + " " + txtEndTime.Text;

            if (brand == "Выберите марку" || string.IsNullOrEmpty(model) || string.IsNullOrEmpty(color) ||
                string.IsNullOrEmpty(txtStartDate.Text) || string.IsNullOrEmpty(txtStartTime.Text) ||
                string.IsNullOrEmpty(txtEndDate.Text) || string.IsNullOrEmpty(txtEndTime.Text))
            {
                carDetails.InnerText = "Пожалуйста, заполните все поля.";
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand getCarCmd = new SqlCommand("SELECT TOP 1 CPlateNum, Price FROM CarTbl WHERE Brand=@brand AND Model=@model AND Color=@color AND Status='Available'", conn);
                getCarCmd.Parameters.AddWithValue("@brand", brand);
                getCarCmd.Parameters.AddWithValue("@model", model);
                getCarCmd.Parameters.AddWithValue("@color", color);
                SqlDataReader reader = getCarCmd.ExecuteReader();

                if (!reader.Read())
                {
                    carDetails.InnerText = "Автомобиль не найден или недоступен.";
                    return;
                }

                string carPlate = reader["CPlateNum"].ToString();
                int price = Convert.ToInt32(reader["Price"]);
                reader.Close();

                int customerId = Convert.ToInt32(Session["CustId"]);

                SqlCommand getMaxId = new SqlCommand("SELECT ISNULL(MAX(RentId), 0) + 1 FROM RentTbl", conn);
                int rentId = (int)getMaxId.ExecuteScalar();

                SqlCommand insertCmd = new SqlCommand(
                    "INSERT INTO RentTbl (RentId, Car, Customer, RentDate, ReturnDate, Fees) " +
                    "VALUES (@id, @car, @cust, @start, @end, @fee)", conn);

                insertCmd.Parameters.AddWithValue("@id", rentId);
                insertCmd.Parameters.AddWithValue("@car", carPlate);
                insertCmd.Parameters.AddWithValue("@cust", customerId);
                insertCmd.Parameters.AddWithValue("@start", DateTime.Parse(startDateTime));
                insertCmd.Parameters.AddWithValue("@end", DateTime.Parse(endDateTime));
                insertCmd.Parameters.AddWithValue("@fee", price);
                insertCmd.ExecuteNonQuery();

                carDetails.InnerText = $"Аренда успешна: {brand} {model} ({color}) с {startDateTime} до {endDateTime}";
            }
        }
    }
}
