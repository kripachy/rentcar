using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebApplication1.view.admin
{
    public partial class carlistt : Page
    {
        
        public class CarInfo
        {
            public string Brand { get; set; }
            public string Model { get; set; }
            public int Price { get; set; }
            public string Status { get; set; }
            public string ImageUrl { get; set; }
        }

        private Dictionary<string, string> specificCarImages = new Dictionary<string, string>
        {
            {"Aston Martin Vanquish", @"C:\Users\kiril\OneDrive\Документы\rentcar\WebApplication1\WebApplication1\colorcars\Aston Martin Vanquish\white\1.jpg"},
            {"Audi TT", @"C:\Users\kiril\OneDrive\Документы\rentcar\WebApplication1\WebApplication1\colorcars\Audi TT\orange\3.jpg"},
            {"Chevrolet Camaro", @"C:\Users\kiril\OneDrive\Документы\rentcar\WebApplication1\WebApplication1\colorcars\Chevrolet Camaro\yellow\1.jpg"},
            {"Ford Mustang S550", @"C:\Users\kiril\OneDrive\Документы\rentcar\WebApplication1\WebApplication1\colorcars\Ford Mustang S550\orange\1.jpg"},
            {"Jaguar XJ", @"C:\Users\kiril\OneDrive\Документы\rentcar\WebApplication1\WebApplication1\colorcars\Jaguar XJ\black\1.jpg"},
            {"Lamborghini Huracan", @"C:\Users\kiril\OneDrive\Документы\rentcar\WebApplication1\WebApplication1\colorcars\Lamborghini Huracan\purple\1.jpg"},
            {"Maserati GranTurismo", @"C:\Users\kiril\OneDrive\Документы\rentcar\WebApplication1\WebApplication1\colorcars\Maserati GranTurismo\yellow\2.jpg"},
            {"Porsche 911", @"C:\Users\kiril\OneDrive\Документы\rentcar\WebApplication1\WebApplication1\colorcars\Porsche 911\green\1.jpeg"}
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if user is logged in
                if (Session["UserEmail"] == null)
                {
                    // Store the current URL in session to redirect back after login
                    Session["ReturnUrl"] = Request.RawUrl;
                    Response.Redirect("~/view/login.aspx");
                }

                ddlSort.SelectedValue = "Price ASC";

                LoadCars(ddlSort.SelectedValue);
            } else {
                
                 LoadCars(ddlSort.SelectedValue);
            }
        }

        private void LoadCars(string sortBy)
        {
            List<CarInfo> cars = new List<CarInfo>();
            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False";
            string defaultImageUrl = ResolveUrl("~/images/default_car.png");
            
            string workspaceRoot = Server.MapPath("~");

            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
             
                string query = "SELECT Brand, Model, Price, Status FROM CarTbl GROUP BY Brand, Model, Price, Status";

                 if (!string.IsNullOrEmpty(sortBy))
                 {

                    if (sortBy.Contains(";") || sortBy.Contains("--") || sortBy.Contains("/") ||
   !(sortBy == "Price ASC" || sortBy == "Price DESC"))
                    {
                        sortBy = "Price ASC";
                    }

                    query += " ORDER BY " + sortBy;
                 }
                else
                {
                    query += " ORDER BY Price ASC";
                }


                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                       
                        HashSet<string> addedCarModels = new HashSet<string>();

                        while (reader.Read())
                        {
                            string brand = reader["Brand"].ToString().Trim();
                            string model = reader["Model"].ToString().Trim();
                            string fullCarName = brand + " " + model;

                            if (!addedCarModels.Contains(fullCarName))
                            {
                                string status = reader["Status"].ToString().Trim();
                                int price = Convert.ToInt32(reader["Price"]);

                                string imageUrl = defaultImageUrl;

                                if (specificCarImages.ContainsKey(fullCarName))
                                {
                                    string absolutePath = specificCarImages[fullCarName];
                                
                                    if (absolutePath.StartsWith(workspaceRoot, StringComparison.OrdinalIgnoreCase))
                                    {
                                        string relativePath = absolutePath.Substring(workspaceRoot.Length).Replace("\\", "/").TrimStart('/');
                                        imageUrl = ResolveUrl("~/" + relativePath);
                                    } else {
                                         System.Diagnostics.Debug.WriteLine($"Absolute path {absolutePath} is not within the workspace root {workspaceRoot}");
                                        
                                         imageUrl = defaultImageUrl;
                                    }
                                }
                                

                                cars.Add(new CarInfo
                                {
                                    Brand = brand,
                                    Model = model,
                                    Price = price,
                                    Status = status,
                                    ImageUrl = imageUrl
                                });

                                addedCarModels.Add(fullCarName);
                            }
                        }
                    }
                }
            }

            rptCars.DataSource = cars;
            rptCars.DataBind();
        }

        
        protected string GetPrice(object price, object status)
        {
            string carStatus = status.ToString();
            if (carStatus != "Available")
            {
                return ""; 
            }
            else
            {
                return "$" + Convert.ToInt32(price).ToString("N0") + "/день";
            }
        }

        protected string GetStatusText(object status)
        {
             string carStatus = status.ToString();
             if (carStatus != "Available")
             {
                 return "Нет в наличии";
             }
             return ""; 
        }
         protected string GetStatusClass(object status)
        {
             string carStatus = status.ToString();
             if (carStatus != "Available")
             {
                 return "unavailable";
             }
             return "";
        }

        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected string GetCarDetailUrl(object brand, object model)
        {
            string carName = $"{brand} {model}";
            switch (carName)
            {
                case "Aston Martin Vanquish":
                    return ResolveUrl("~/view/admin/AstonMartinVanquish.aspx");
                case "Audi TT":
                    return ResolveUrl("~/view/admin/AudiTT.aspx");
                default:
                    // Для других машин можно добавить дополнительные case или вернуть дефолтную страницу
                    return ResolveUrl("~/view/admin/AstonMartinVanquish.aspx");
            }
        }
    }
}