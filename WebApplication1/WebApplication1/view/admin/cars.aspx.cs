using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.IO;
using System.Collections.Generic;
using System.Text;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "text/html; charset=utf-8";
            Response.Charset = "utf-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.HeaderEncoding = Encoding.UTF8;

            Conn = new Models.Functions();

            if (!IsPostBack)
            {
                ddlBrand.Items.Clear();
                ddlBrand.Items.Insert(0, new ListItem("Выберите марку", ""));
                foreach (var brand in brandModelMap.Keys)
                {
                    ddlBrand.Items.Add(brand);
                }

                ddlModel.Items.Clear();
                ddlModel.Items.Add(new ListItem("Выберите модель", ""));
                ddlModel.Enabled = false;

                LoadCars();
            }
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

            if (!string.IsNullOrWhiteSpace(selectedBrand) && brandModelMap.ContainsKey(selectedBrand))
            {
                ddlModel.Items.Clear();
                ddlModel.Items.Add(new ListItem(brandModelMap[selectedBrand], selectedBrand));
                ddlModel.SelectedIndex = 0;
                ddlModel.Enabled = false;
            }
            else
            {
                ddlModel.Items.Clear();
                ddlModel.Items.Add(new ListItem("Выберите модель", ""));
                ddlModel.Enabled = false;
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT CPlateNum AS [Licence], Brand, Model, Price, Color, Status FROM CarTbl";
                DataTable dt = Conn.GetData(query);

                if (dt.Rows.Count > 0)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("Cars");
                        ws.Cell(1, 1).InsertTable(dt, false);
                        ws.Columns().AdjustToContents();

                        using (MemoryStream ms = new MemoryStream())
                        {
                            wb.SaveAs(ms);
                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;filename=CarRental_Export_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
                            Response.BinaryWrite(ms.ToArray());
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
                else
                {
                    ErrorMsg.InnerText = "Нет данных для экспорта";
                }
            }
            catch (Exception ex)
            {
                ErrorMsg.InnerText = "Ошибка экспорта: " + ex.Message;
            }
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtLicence.Text) ||
                    ddlBrand.SelectedIndex <= 0 ||
                    ddlModel.SelectedIndex < 0 ||
                    string.IsNullOrWhiteSpace(txtPrice.Text) ||
                    string.IsNullOrWhiteSpace(txtColor.Text))
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
                string Color = txtColor.Text.Trim().Replace("'", "''");

                // Исправлено: сохраняем "Available" или "Booked"
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

                // Номер
                txtLicence.Text = row.Cells[1].Text;

                // Марка
                string brand = row.Cells[2].Text;
                ddlBrand.SelectedValue = brand;

                // Автоматически устанавливаем модель
                ddlBrand_SelectedIndexChanged(null, null);

                // Цена
                int price = Convert.ToInt32(carlist.SelectedDataKey["Price"]);
                txtPrice.Text = price.ToString();

                // Цвет
                txtColor.Text = row.Cells[5].Text;

                // Статус
                Label lblStatus = (Label)row.FindControl("lblStatus");
                if (lblStatus != null)
                {
                    // Исправлено: правильно определяем статус
                    string statusText = lblStatus.Text.Trim();
                    ddlAvailable.SelectedValue = (statusText == "Доступен" || statusText == "Available") ? "1" : "0";
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
                    string.IsNullOrWhiteSpace(txtColor.Text))
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

                string Color = txtColor.Text.Trim().Replace("'", "''");

                // Исправлено: сохраняем "Available" или "Booked"
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
            txtColor.Text = "";
            ddlAvailable.SelectedIndex = 0;
            ViewState["SelectedCarKey"] = null;
            carlist.SelectedIndex = -1;
        }
    }
}