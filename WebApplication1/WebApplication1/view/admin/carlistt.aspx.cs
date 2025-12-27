using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebApplication1.App_Start;

namespace WebApplication1.view.admin
{
    public partial class carlistt : Page
    {
        protected global::System.Web.UI.WebControls.Label lblError;
        
        public class CarInfo
        {
            public string Plate { get; set; }
            public string Brand { get; set; }
            public string Model { get; set; }
            public int Price { get; set; }
            public string Status { get; set; }
            public string ImageUrl { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Session["UserEmail"] == null)
                    {
                        Session["ReturnUrl"] = Request.RawUrl;
                        Response.Redirect("~/view/admin/login.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                        return;
                    }

                    CarImageBootstrapper.EnsureSeeded(Server);
                    ddlSort.SelectedValue = "Price ASC";
                    LoadCars(ddlSort.SelectedValue);
                } 
                else 
                {
                    LoadCars(ddlSort.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Ошибка загрузки: " + ex.Message + "<br/>" + ex.StackTrace;
                lblError.Visible = true;
            }
        }

        private void LoadCars(string sortBy)
        {
            try
            {
                List<CarInfo> cars = new List<CarInfo>();
                string constr = WebApplication1.Models.Functions.GetConnectionString();
                
                // Пробуем получить lookup, если упадет - просто пустой словарь
                Dictionary<string, int?> imageLookup = new Dictionary<string, int?>();
                try {
                     imageLookup = CarImageBootstrapper.GetMainImageLookup();
                } catch { }

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    
                    // Группируем по Бренду и Модели, чтобы в каталоге не было дубликатов одной и той же машины
                    // Выбираем минимальную цену и лучший статус (Available приоритетнее из-за алфавитного порядка)
                    string query = @"
                        SELECT 
                            MIN(CPlateNum) as CPlateNum, 
                            Brand, 
                            Model, 
                            MIN(Color) as Color, 
                            MIN(Price) as Price, 
                            MIN(Status) as Status 
                        FROM CarTbl 
                        GROUP BY Brand, Model";

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
                                string carKey = (brand + "|" + model).ToLower();

                                if (!addedCarModels.Contains(carKey))
                                {
                                    string plate = reader["CPlateNum"].ToString();
                                    string color = reader["Color"]?.ToString().Trim() ?? "";
                                    string status = reader["Status"] == DBNull.Value ? "Available" : reader["Status"].ToString().Trim();
                                    int price = reader["Price"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Price"]);

                                    string imageUrl = "";
                                    try {
                                        imageUrl = CarImageBootstrapper.BuildImageUrl(this, plate, brand, model, color, imageLookup);
                                    } catch {
                                        imageUrl = ResolveUrl("~/assets/images/default-car.png.png");
                                    }

                                    cars.Add(new CarInfo
                                    {
                                        Plate = plate,
                                        Brand = brand,
                                        Model = model,
                                        Price = price,
                                        Status = status,
                                        ImageUrl = imageUrl
                                    });

                                    addedCarModels.Add(carKey);
                                }
                            }
                        }
                    }
                }

                rptCars.DataSource = cars;
                rptCars.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = "Ошибка в LoadCars: " + ex.Message + "<br/>Stack: " + ex.StackTrace;
                lblError.Visible = true;
            }
        }

        
        protected string GetPrice(object price, object status)
        {
            if (status == null || status == DBNull.Value) return "";
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
             if (status == null || status == DBNull.Value) return "Нет в наличии";
             string carStatus = status.ToString();
             if (carStatus != "Available")
             {
                 return "Нет в наличии";
             }
             return ""; 
        }

         protected string GetStatusClass(object status)
        {
             if (status == null || status == DBNull.Value) return "unavailable";
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

        protected string GetCarDetailUrl(object plate)
        {
            return ResolveUrl($"~/view/admin/CarDetails.aspx?plate={plate}");
        }

        protected string GetOnErrorScript()
        {
            return "this.onerror=null; this.src='" + ResolveUrl("~/assets/images/default-car.png.png") + "';";
        }
    }
}