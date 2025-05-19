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

namespace WebApplication1.view.admin
{
    public partial class cars : System.Web.UI.Page
    {
        Models.Functions Conn;
        private Dictionary<string, string> brandModelMap = new Dictionary<string, string>()
        {
            {"Ford", "Mustang S550"},
            {"Chevrolet", "Camaro"},
            {"Lamborghini", "Huracan"},
            {"Jaguar", "XJ"},
            {"Porsche", "911"},
            {"Maserati", "GranTurismo"},
            {"Aston Martin", "Vanquish"},
            {"Audi", "TT"}
        };

        private Dictionary<string, string> brandImageMap = new Dictionary<string, string>()
        {
            {"Aston Martin", "../../assets/images/logo/Aston-Martin-Logo.jpg"},
            {"Ford","../../assets/images/logo/mustang.png"},
            {"Chevrolet", "../../assets/images/logo/chevrolet.jpg"},
            {"Lamborghini", "../../assets/images/logo/Lamborghini.jpg"},
            {"Jaguar", "../../assets/images/logo/jaguar.jpg"},
            {"Porsche", "../../assets/images/logo/porsche.png"},
            {"Maserati", "../../assets/images/logo/MASERATI.jpg"},
            {"Audi", "../../assets/images/logo/audi.jpg"}
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "text/html; charset=utf-8";
            Response.Charset = "utf-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.HeaderEncoding = Encoding.UTF8;

            Conn = new Models.Functions();

            if (!IsPostBack)
            {
                carImage.Src = "~/assets/images/Слой 1.png";
                carImage.Visible = true;  // Убедимся, что картинка видна при загрузке страницы
                ddlBrand.Items.Clear();
                ddlBrand.Items.Insert(0, new ListItem("Выберите марку", ""));
                foreach (var brand in brandModelMap.Keys)
                {
                    ddlBrand.Items.Add(brand);
                }

                ddlModel.Items.Clear();
                ddlModel.Items.Add(new ListItem("Выберите модель", ""));
                ddlModel.Enabled = false;

                ddlColor.Items.Clear();
                ddlColor.Items.Add(new ListItem("Выберите цвет", ""));
                ddlColor.Items.Add(new ListItem("Красный", "Red"));
                ddlColor.Items.Add(new ListItem("Синий", "Blue"));
                ddlColor.Items.Add(new ListItem("Зелёный", "Green"));
                ddlColor.Items.Add(new ListItem("Чёрный", "Black"));
                ddlColor.Items.Add(new ListItem("Белый", "White"));
                ddlColor.Items.Add(new ListItem("Жёлтый", "Yellow"));
                ddlColor.Items.Add(new ListItem("Серый", "Gray"));
                ddlColor.Items.Add(new ListItem("Оранжевый", "Orange"));
                ddlColor.Items.Add(new ListItem("Фиолетовый", "Purple"));
                LoadCars();
            }
        }

        private List<string> GetImagePaths(string directory)
        {
            var images = new List<string>();
            if (Directory.Exists(directory))
            {
                for (int i = 1; i <= 5; i++)
                {
                    string imagePath = Path.Combine(directory, $"{i}.jpg");
                    if (File.Exists(imagePath))
                    {
                        string relativePath = imagePath.Replace(Server.MapPath("~"), "").Replace("\\", "/");
                        images.Add("~" + relativePath);
                    }
                }
            }
            return images;
        }

        private void LoadCars()
        {
            try
            {
                string query = "SELECT * FROM CarTbl";
                carlist.DataSource = Conn.GetData(query);
                carlist.DataBind();
            }
            catch (Exception ex)
            {
                ErrorMsg.InnerText = "Ошибка загрузки автомобилей: " + ex.Message;
            }
        }

        protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedBrand = ddlBrand.SelectedValue;
            if (!string.IsNullOrEmpty(selectedBrand))
            {
                ddlModel.Enabled = true;
                ddlModel.Items.Clear();
                ddlModel.Items.Add(new ListItem(brandModelMap[selectedBrand], brandModelMap[selectedBrand]));

                if (brandImageMap.ContainsKey(selectedBrand))
                {
                    carImage.Src = brandImageMap[selectedBrand];
                    carImage.Visible = true;
                }
                else
                {
                    carImage.Src = "~/assets/images/Слой 1.png";
                    carImage.Visible = true; 
                }
            }
            else
            {
                ddlModel.Enabled = false;
                ddlModel.Items.Clear();
                ddlModel.Items.Add(new ListItem("Выберите модель", ""));
                carImage.Src = "~/assets/images/Слой 1.png";
                carImage.Visible = true;  
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var colorMapping = new Dictionary<string, XLColor>(StringComparer.OrdinalIgnoreCase)
    {
        { "красный", XLColor.Red },
        { "синий", XLColor.Blue },
        { "зелёный", XLColor.Green },
        { "чёрный", XLColor.Black },
        { "белый", XLColor.White },
        { "жёлтый", XLColor.Yellow },
        { "оранжевый", XLColor.Orange }
    };

            var statusMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        { "Available", "Доступен" },
        { "Booked", "Забронирован" }
    };

            string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False";

            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM CarTbl", con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Экспорт автомобилей");

                string[] russianColumnNames = {
            "Номер лицензии",
            "Марка",
            "Модель",
            "Цена",
            "Цвет",
            "Статус"
        };

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i < russianColumnNames.Length)
                    {
                        ws.Cell(1, i + 1).Value = russianColumnNames[i];
                    }
                    else
                    {
                        ws.Cell(1, i + 1).Value = dt.Columns[i].ColumnName;
                    }
                    ws.Cell(1, i + 1).Style.Font.Bold = true;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        var value = dt.Rows[i][j].ToString();
                        var columnName = dt.Columns[j].ColumnName;
                        var cell = ws.Cell(i + 2, j + 1);

                        if (columnName.Equals("Color", StringComparison.OrdinalIgnoreCase) ||
                            columnName.Equals("Цвет", StringComparison.OrdinalIgnoreCase))
                        {
                            if (colorMapping.TryGetValue(value.Trim().ToLower(), out var xlColor))
                            {
                                cell.Style.Fill.BackgroundColor = xlColor;
                                
                                var russianColor = colorMapping.FirstOrDefault(x => x.Value.Equals(xlColor)).Key;
                                cell.Value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(russianColor);
                            }
                            else
                            {
                                cell.Value = value;
                            }
                        }
                       
                        else if (columnName.Equals("Status", StringComparison.OrdinalIgnoreCase) ||
                                 columnName.Equals("Статус", StringComparison.OrdinalIgnoreCase))
                        {
                            if (statusMapping.TryGetValue(value, out var russianStatus))
                            {
                                cell.Value = russianStatus;
                            }
                            else
                            {
                                cell.Value = value;
                            }
                        }
                       
                        else if (columnName.Equals("Price", StringComparison.OrdinalIgnoreCase))
                        {
                            if (int.TryParse(value, out int price))
                            {
                                cell.Value = price;
                                cell.Style.NumberFormat.Format = "#,##0"; 
                            }
                            else
                            {
                                cell.Value = value;
                            }
                        }
                        else
                        {
                            cell.Value = value;
                        }
                    }
                }

                ws.Columns().AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    byte[] bytes = stream.ToArray();

                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=Экспорт_автомобилей.xlsx");
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        private void ShowError(string message)
        {
            ErrorMsg.Style["color"] = "red";
            ErrorMsg.InnerText = message;
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtLicence.Text) ||
                    ddlBrand.SelectedIndex <= 0 ||
                    ddlModel.SelectedIndex < 0 ||
                    string.IsNullOrWhiteSpace(txtPrice.Text) ||
                    ddlColor.SelectedIndex <= 0)
                {
                    ErrorMsg.InnerText = "Заполните все обязательные поля";
                    return;
                }

                string CPlateNum = txtLicence.Text.Trim().Replace("'", "''");
                string checkQuery = $"SELECT COUNT(*) AS Count FROM CarTbl WHERE CPlateNum = N'{CPlateNum}'";
                var result = Conn.GetData(checkQuery);

                if (Convert.ToInt32(result.Rows[0]["Count"]) > 0)
                {
                    ErrorMsg.InnerText = "Автомобиль с таким номером уже существует";
                    return;
                }

                string Brand = ddlBrand.SelectedValue.Replace("'", "''");
                string Model = brandModelMap.ContainsKey(Brand) ? brandModelMap[Brand] : "";
                string cleanPrice = Regex.Replace(txtPrice.Text, @"[^\d]", "");
                if (!int.TryParse(cleanPrice, out int Price))
                {
                    ErrorMsg.InnerText = "Некорректная цена";
                    return;
                }

                string Color = ddlColor.SelectedValue;
                string Status = ddlAvailable.SelectedValue == "1" ? "Available" : "Booked";

                string Query = $"INSERT INTO CarTbl VALUES(N'{CPlateNum}',N'{Brand}',N'{Model}',{Price},N'{Color}',N'{Status}')";
                Conn.SetData(Query);
                LoadCars();
                ClearFields();
                ErrorMsg.Style["color"] = "green";
                ErrorMsg.InnerText = "Автомобиль успешно добавлен";
            }
            catch (Exception ex)
            {
                ErrorMsg.InnerText = "Ошибка при сохранении: " + ex.Message;
            }
        }

        protected void carlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (carlist.SelectedRow == null || carlist.SelectedDataKey == null) return;

                GridViewRow row = carlist.SelectedRow;

                txtLicence.Text = row.Cells[1].Text;
                string brand = row.Cells[2].Text;
                ddlBrand.SelectedValue = brand;
                
                // Показываем логотип марки при выборе из таблицы
                if (brandImageMap.ContainsKey(brand))
                {
                    carImage.Src = brandImageMap[brand];
                    carImage.Visible = true;
                }
                else
                {
                    carImage.Src = "~/assets/images/Слой 1.png";
                    carImage.Visible = true;  // Показываем дефолтную картинку
                }

                ddlBrand_SelectedIndexChanged(null, null);

                int price = Convert.ToInt32(carlist.SelectedDataKey["Price"]);
                txtPrice.Text = price.ToString();

                string color = row.Cells[5].Text;

                ListItem item = ddlColor.Items.Cast<ListItem>()
                    .FirstOrDefault(i => i.Value.Equals(color, StringComparison.OrdinalIgnoreCase));

                if (item != null)
                {
                    ddlColor.ClearSelection();
                    item.Selected = true;
                }
                else
                {
                    ErrorMsg.InnerText = $"Цвет '{color}' не найден в списке.";
                }

                ViewState["SelectedCarKey"] = txtLicence.Text;
            }
            catch (Exception ex)
            {
                ErrorMsg.InnerText = "Ошибка при выборе автомобиля: " + ex.Message;
            }
        }


        protected void Edit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["SelectedCarKey"] == null)
                {
                    ErrorMsg.InnerText = "Выберите автомобиль для редактирования";
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtLicence.Text) ||
                    ddlBrand.SelectedIndex <= 0 ||
                    string.IsNullOrWhiteSpace(txtPrice.Text) ||
                    ddlColor.SelectedIndex <= 0)
                {
                    ErrorMsg.InnerText = "Заполните все обязательные поля";
                    return;
                }

                string originalPlate = ViewState["SelectedCarKey"].ToString().Replace("'", "''");
                string newPlate = txtLicence.Text.Trim().Replace("'", "''");
                string Brand = ddlBrand.SelectedValue.Replace("'", "''");
                string Model = brandModelMap.ContainsKey(Brand) ? brandModelMap[Brand] : "";

                string cleanPrice = Regex.Replace(txtPrice.Text, @"[^\d]", "");
                if (!int.TryParse(cleanPrice, out int Price))
                {
                    ErrorMsg.InnerText = "Некорректная цена";
                    return;
                }

                string Color = ddlColor.SelectedValue;
                string Status = ddlAvailable.SelectedValue == "1" ? "Available" : "Booked";

                if (originalPlate != newPlate)
                {
                    string checkQuery = $"SELECT COUNT(*) AS Count FROM CarTbl WHERE CPlateNum = '{newPlate}'";
                    var result = Conn.GetData(checkQuery);
                    if (Convert.ToInt32(result.Rows[0]["Count"]) > 0)
                    {
                        ErrorMsg.InnerText = "Автомобиль с таким номером уже существует";
                        return;
                    }
                }

                string query = $"UPDATE CarTbl SET CPlateNum=N'{newPlate}', Brand=N'{Brand}', Model=N'{Model}', Price={Price}, Color=N'{Color}', Status=N'{Status}' WHERE CPlateNum=N'{originalPlate}'";
                Conn.SetData(query);
                LoadCars();
                ClearFields();
                ErrorMsg.InnerText = "Автомобиль успешно обновлён";
            }
            catch (Exception ex)
            {
                ErrorMsg.InnerText = "Ошибка при редактировании: " + ex.Message;
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["SelectedCarKey"] == null)
                {
                    ErrorMsg.InnerText = "Выберите автомобиль для удаления";
                    return;
                }

                string CPlateNum = ViewState["SelectedCarKey"].ToString().Replace("'", "''");
                string query = $"DELETE FROM CarTbl WHERE CPlateNum='{CPlateNum}'";
                Conn.SetData(query);
                LoadCars();
                ClearFields();
                ErrorMsg.InnerText = "Автомобиль успешно удалён";
            }
            catch (Exception ex)
            {
                ErrorMsg.InnerText = "Ошибка при удалении: " + ex.Message;
            }
        }

        private void ClearFields()
        {
            txtLicence.Text = "";
            ddlBrand.SelectedIndex = 0;
            ddlModel.Items.Clear();
            ddlModel.Items.Add(new ListItem("Выберите модель", ""));
            ddlModel.Enabled = false;
            txtPrice.Text = "";
            ddlColor.SelectedIndex = 0;
            ddlAvailable.SelectedIndex = 0;
            ViewState["SelectedCarKey"] = null;
            carlist.SelectedIndex = -1;
            carImage.Src = "~/assets/images/Слой 1.png";
            carImage.Visible = true;  // Показываем дефолтную картинку при очистке полей
        }
    }
}
