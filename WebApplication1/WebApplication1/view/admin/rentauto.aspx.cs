using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using ClosedXML.Excel;
using Newtonsoft.Json;

namespace WebApplication1.view.admin
{
    public partial class rentauto : Page
    {
        protected global::System.Web.UI.WebControls.DropDownList ddlColor;
        private Dictionary<string, Dictionary<string, List<string>>> carImages = new Dictionary<string, Dictionary<string, List<string>>>();
        private int currentImageIndex = 0;
        private Dictionary<string, CarInfo> carData = new Dictionary<string, CarInfo>();

        private class CarInfo
        {
            public string Model { get; set; }
            public int Price { get; set; }
            public string Color { get; set; }
            public string Status { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    LoadCarData();
                    InitializeCarImages();
                    LoadBrands();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Ошибка при загрузке страницы: {ex.Message}');", true);
                }
            }
        }

        private void LoadCarData()
        {
            try
            {
                string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False";
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    string query = @"
                        SELECT DISTINCT Brand, Model, Price, Color, Status 
                        FROM CarTbl 
                        WHERE Status = 'Available'
                        ORDER BY Brand, Model";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string brand = reader["Brand"].ToString().Trim();
                                string model = reader["Model"].ToString().Trim();
                                int price = Convert.ToInt32(reader["Price"]);
                                string color = reader["Color"].ToString().Trim();
                                string status = reader["Status"].ToString().Trim();

                                carData[brand] = new CarInfo
                                {
                                    Model = model,
                                    Price = price,
                                    Color = color,
                                    Status = status
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Ошибка загрузки данных: {ex.Message}');", true);
            }
        }

        private void InitializeCarImages()
        {
            string basePath = Server.MapPath("~/colorcars/");
            System.Diagnostics.Debug.WriteLine($"Base path: {basePath}");

            // Aston Martin - только белый
            string astonPath = basePath + "Aston Martin Vanquish/white/";
            System.Diagnostics.Debug.WriteLine($"Aston Martin path: {astonPath}");
            System.Diagnostics.Debug.WriteLine($"Directory exists: {Directory.Exists(astonPath)}");

            var astonImages = GetImagePaths(astonPath, new[] { "1.jpg", "2.jpg", "3.jpg" });
            System.Diagnostics.Debug.WriteLine($"Found {astonImages.Count} images for Aston Martin");
            foreach (var img in astonImages)
            {
                System.Diagnostics.Debug.WriteLine($"Aston Martin image: {img}");
            }

            carImages["Aston Martin"] = new Dictionary<string, List<string>>
            {
                ["white"] = astonImages
            };

            // Audi - черный, синий, оранжевый
            carImages["Audi"] = new Dictionary<string, List<string>>
            {
                ["black"] = GetImagePaths(basePath + "Audi TT/black/", new[] { "1.jpg", "2.jpeg" }),
                ["blue"] = GetImagePaths(basePath + "Audi TT/blue/", new[] { "1.jpg", "2.jpg", "3.jpg" }),
                ["orange"] = GetImagePaths(basePath + "Audi TT/orange/", new[] { "1.jpg", "2.jpg", "3.jpg" }),
                ["white"] = GetImagePaths(basePath + "Audi TT/white/", new[] { "1.jpg", "2.jpg", "3.jpg" })
            };

            // Chevrolet - только желтый
            carImages["Chevrolet"] = new Dictionary<string, List<string>>
            {
                ["yellow"] = GetImagePaths(basePath + "Chevrolet Camaro/yellow/", new[] { "1.jpg", "2.jpg", "3.jpg" })
            };

            // Ford - только оранжевый
            carImages["Ford"] = new Dictionary<string, List<string>>
            {
                ["orange"] = GetImagePaths(basePath + "Ford Mustang S550/orange/", new[] { "1.jpg", "2.jpg", "3.jpg" })
            };

            // Jaguar - только черный
            carImages["Jaguar"] = new Dictionary<string, List<string>>
            {
                ["black"] = GetImagePaths(basePath + "Jaguar XJ/black/", new[] { "1.jpg", "2.jpg", "3.jpg" })
            };

            // Lamborghini - только фиолетовый
            carImages["Lamborghini"] = new Dictionary<string, List<string>>
            {
                ["purple"] = GetImagePaths(basePath + "Lamborghini Huracan/purple/", new[] { "1.jpg", "2.jpg", "3.jpg" })
            };

            // Maserati - серый, красный
            carImages["Maserati"] = new Dictionary<string, List<string>>
            {
                ["gray"] = GetImagePaths(basePath + "Maserati GranTurismo/gray/", new[] { "1.jpg", "2.jpg", "3.jpg", "4.jpg" }),
                ["red"] = GetImagePaths(basePath + "Maserati GranTurismo/red/", new[] { "1.jpg", "2.jpg", "3.jpg", "4.jpg", "5.jpg" })
            };

            // Porsche - только зеленый
            carImages["Porsche"] = new Dictionary<string, List<string>>
            {
                ["green"] = GetImagePaths(basePath + "Porsche 911/green/", new[] { "1.jpeg", "2.jpg", "3.jpeg" })
            };
        }

        private List<string> GetImagePaths(string directory, string[] fileNames)
        {
            var images = new List<string>();
            try
            {
                if (Directory.Exists(directory))
                {
                    string appRoot = Server.MapPath("~");
                    string relativePath = directory.Replace(appRoot, "").Replace("\\", "/").TrimStart('/');
                    
                    foreach (string fileName in fileNames)
                    {
                        string fullPath = Path.Combine(directory, fileName);
                        if (File.Exists(fullPath))
                        {
                            // Используем ResolveUrl для получения правильного URL
                            string imageUrl = $"~/{relativePath}/{fileName}";
                            string resolvedUrl = ResolveUrl(imageUrl);
                            System.Diagnostics.Debug.WriteLine($"Image URL: {resolvedUrl}");
                            images.Add(resolvedUrl);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetImagePaths: {ex.Message}");
            }
            return images;
        }

        private void LoadBrands()
        {
            ddlBrand.Items.Clear();
            ddlBrand.Items.Add(new ListItem("Выберите марку", ""));
            foreach (var brand in carData.Keys)
            {
                ddlBrand.Items.Add(brand);
            }
        }

        protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedBrand = ddlBrand.SelectedValue.Trim();
                System.Diagnostics.Debug.WriteLine($"Выбрана марка: {selectedBrand}");

                // Очищаем и сбрасываем значения
                lblModel.Text = "";
                lblPrice.Text = "";
                hfSelectedColor.Value = "";
                ddlColor.Items.Clear();
                ddlColor.Items.Add(new ListItem("Выберите цвет", ""));

                if (!string.IsNullOrEmpty(selectedBrand))
                {
                    string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False";
                    using (SqlConnection conn = new SqlConnection(constr))
                    {
                        conn.Open();

                        // Получаем модель и цену
                        string query = "SELECT DISTINCT Model, Price FROM CarTbl WHERE Brand = @Brand AND Status = 'Available'";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Brand", selectedBrand);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    lblModel.Text = reader["Model"].ToString().Trim();
                                    lblPrice.Text = Convert.ToInt32(reader["Price"]).ToString("N0") + " $/день";
                                }
                            }
                        }

                        // Получаем доступные цвета для выбранной марки
                        query = "SELECT DISTINCT Color FROM CarTbl WHERE Brand = @Brand AND Status = 'Available' ORDER BY Color";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Brand", selectedBrand);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string color = reader["Color"].ToString().Trim();
                                    ddlColor.Items.Add(new ListItem(color, color));
                                }
                            }
                        }
                    }

                    // Если есть доступные цвета, выбираем первый и показываем картинки
                    if (ddlColor.Items.Count > 1)
                    {
                        ddlColor.SelectedIndex = 1; // Выбираем первый цвет (пропускаем "Выберите цвет")
                        string firstColor = ddlColor.SelectedValue;
                        hfSelectedColor.Value = firstColor;

                        // Показываем картинки для первого цвета
                        string colorKey = firstColor.ToLower();
                        if (carImages.ContainsKey(selectedBrand) && carImages[selectedBrand].ContainsKey(colorKey))
                        {
                            var images = carImages[selectedBrand][colorKey];
                            if (images.Count > 0)
                            {
                                string script = $@"
                                    console.log('Картинки для карусели:', {JsonConvert.SerializeObject(images)});
                                    initializeCarousel({JsonConvert.SerializeObject(images)});
                                    var prevButton = document.getElementById('{btnPrevImage.ClientID}');
                                    var nextButton = document.getElementById('{btnNextImage.ClientID}');
                                    if (prevButton) prevButton.style.display = 'flex';
                                    if (nextButton) nextButton.style.display = 'flex';";
                                ScriptManager.RegisterStartupScript(this, GetType(), "initializeCarousel", script, true);
                            }
                        }
                    }
                }
                else
                {
                    // Сбрасываем отображение картинок если марка не выбрана
                    string script = @"
                        initializeCarousel([]);
                        var prevButton = document.getElementById('" + btnPrevImage.ClientID + @"');
                        var nextButton = document.getElementById('" + btnNextImage.ClientID + @"');
                        if (prevButton) prevButton.style.display = 'none';
                        if (nextButton) nextButton.style.display = 'none';";

                    ScriptManager.RegisterStartupScript(this, GetType(), "resetAll", script, true);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при выборе марки: {ex.Message}");
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Ошибка при выборе марки: {ex.Message}');", true);
            }
        }

        protected void btnPrevImage_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "prevSlide", "prevSlide();", true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при переключении изображения: {ex.Message}");
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Ошибка при переключении изображения: {ex.Message}');", true);
            }
        }

        protected void btnNextImage_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "nextSlide", "nextSlide();", true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при переключении изображения: {ex.Message}");
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Ошибка при переключении изображения: {ex.Message}');", true);
            }
        }

        protected void btnRent_Click(object sender, EventArgs e)
        {
            string brand = ddlBrand.SelectedValue;
            string model = lblModel.Text;
            string color = hfSelectedColor.Value;

            if (string.IsNullOrEmpty(brand) || string.IsNullOrEmpty(model) || string.IsNullOrEmpty(color))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Пожалуйста, выберите бренд, модель и цвет.');", true);
                return;
            }

            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False";
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string query = "SELECT COUNT(*), Price FROM CarTbl WHERE Brand = @Brand AND Model = @Model AND Color = @Color AND Status = 'Available' GROUP BY Price";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Brand", brand);
                    cmd.Parameters.AddWithValue("@Model", model);
                    cmd.Parameters.AddWithValue("@Color", color);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int availableCount = reader.GetInt32(0);
                            int price = reader.GetInt32(1);
                            if (availableCount > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Вы арендовали {brand} {model} ({color}) за {price:N0} $/день');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('К сожалению, выбранная машина сейчас недоступна.');", true);
                            }
                        }
                    }
                }
            }
        }

        protected void ddlColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string brand = ddlBrand.SelectedValue;
                string color = ddlColor.SelectedValue;
                hfSelectedColor.Value = color;
                if (string.IsNullOrEmpty(brand) || string.IsNullOrEmpty(color))
                    return;
                string colorKey = color.ToLower();
                if (carImages.ContainsKey(brand) && carImages[brand].ContainsKey(colorKey))
                {
                    var images = carImages[brand][colorKey];
                    if (images.Count > 0)
                    {
                        string script = $@"
                            console.log('Картинки для карусели:', {JsonConvert.SerializeObject(images)});
                            initializeCarousel({JsonConvert.SerializeObject(images)});
                            var prevButton = document.getElementById('{btnPrevImage.ClientID}');
                            var nextButton = document.getElementById('{btnNextImage.ClientID}');
                            if (prevButton) prevButton.style.display = 'flex';
                            if (nextButton) nextButton.style.display = 'flex';";
                        ScriptManager.RegisterStartupScript(this, GetType(), "initializeCarousel", script, true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Изображения для выбранного цвета не найдены');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Изображения для выбранного цвета не найдены');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Ошибка при выборе цвета: {ex.Message}');", true);
            }
        }
    }
}