using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.IO;
namespace WebApplication1.view.admin
{
    public partial class cars : System.Web.UI.Page
    {
        Models.Functions Conn;

        protected void Page_Load(object sender, EventArgs e)
        {
            Conn = new Models.Functions();
            if (!IsPostBack)
            {
                ShowCars();
            }
        }

        private void ShowCars()
        {
            string query = "SELECT * FROM CarTbl";
            carlist.DataSource = Conn.GetData(query);
            carlist.DataBind();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT CPlateNum AS [Licence #], Brand, Model, Price, Color, Status FROM CarTbl";
                DataTable dt = Conn.GetData(query);

                if (dt.Rows.Count > 0)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "Cars");

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
                    ErrorMsg.InnerText = "No data to export";
                }
            }
            catch (Exception ex)
            {
                ErrorMsg.InnerText = "Export failed: " + ex.Message;
            }
        }


        protected void Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtLicence.Text == "" || txtBrand.Text == "" || txtModel.Text == "" || txtPrice.Text == "" || txtColor.Text == "")
                {
                    ErrorMsg.InnerText = "Missing Information";
                    return;
                }

                string CPlateNum = txtLicence.Text.Trim().Replace("'", "''");

                string checkQuery = $"SELECT COUNT(*) AS Count FROM CarTbl WHERE CPlateNum = '{CPlateNum}'";
                var result = Conn.GetData(checkQuery);
                if (Convert.ToInt32(result.Rows[0]["Count"]) > 0)
                {
                    ErrorMsg.InnerText = "Licence Number already exists.";
                    return;
                }

                string Brand = txtBrand.Text.Trim().Replace("'", "''");
                string Model = txtModel.Text.Trim().Replace("'", "''");
                string rawPrice = HttpUtility.HtmlDecode(txtPrice.Text);
                string cleanPrice = Regex.Replace(rawPrice, @"[^\d]", ""); 
                int Price = Convert.ToInt32(cleanPrice);

                string Color = txtColor.Text.Trim().Replace("'", "''");
                string Status = ddlAvailable.SelectedItem.Text.Replace("'", "''");

                string Query = $"INSERT INTO CarTbl VALUES('{CPlateNum}','{Brand}','{Model}',{Price},'{Color}','{Status}')";
                Conn.SetData(Query);
                ShowCars();
                ClearFields();
                ErrorMsg.InnerText = "Car Added";
            }
            catch (Exception ex)
            {
                ErrorMsg.InnerText = ex.Message;
            }
        }

        protected void carlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = carlist.SelectedRow;
            txtLicence.Text = row.Cells[1].Text;
            txtBrand.Text = row.Cells[2].Text;
            txtModel.Text = row.Cells[3].Text;

            string rawPrice = HttpUtility.HtmlDecode(row.Cells[4].Text); 
            string cleanPrice = Regex.Replace(rawPrice, @"[^\d]", "");   
            txtPrice.Text = cleanPrice;

            txtColor.Text = row.Cells[5].Text;

            string status = row.Cells[6].Text;
            ddlAvailable.SelectedValue = (status == "Available") ? "1" : "0";

            ViewState["SelectedCarKey"] = row.Cells[1].Text;
        }


        protected void Edit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["SelectedCarKey"] == null)
                {
                    ErrorMsg.InnerText = "Please select a car to edit.";
                    return;
                }

                string originalPlate = ViewState["SelectedCarKey"].ToString().Replace("'", "''"); 
                string newPlate = txtLicence.Text.Trim().Replace("'", "''");
                string Brand = txtBrand.Text.Trim().Replace("'", "''");
                string Model = txtModel.Text.Trim().Replace("'", "''");
                string rawPrice = HttpUtility.HtmlDecode(txtPrice.Text);
                string cleanPrice = Regex.Replace(rawPrice, @"[^\d]", ""); 
                int Price = Convert.ToInt32(cleanPrice);

                string Color = txtColor.Text.Trim().Replace("'", "''");
                string Status = ddlAvailable.SelectedItem.Text.Replace("'", "''");

                string query = $"UPDATE CarTbl SET CPlateNum='{newPlate}', Brand='{Brand}', Model='{Model}', Price={Price}, Color='{Color}', Status='{Status}' WHERE CPlateNum='{originalPlate}'";
                Conn.SetData(query);
                ShowCars();
                ClearFields();
                ErrorMsg.InnerText = "Car Updated";
            }
            catch (Exception ex)
            {
                ErrorMsg.InnerText = ex.Message;
            }
        }


        protected void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["SelectedCarKey"] == null)
                {
                    ErrorMsg.InnerText = "Please select a car to delete.";
                    return;
                }

                string CPlateNum = ViewState["SelectedCarKey"].ToString().Replace("'", "''");
                string query = $"DELETE FROM CarTbl WHERE CPlateNum='{CPlateNum}'";
                Conn.SetData(query);
                ShowCars();
                ClearFields();
                ErrorMsg.InnerText = "Car Deleted";
            }
            catch (Exception ex)
            {
                ErrorMsg.InnerText = ex.Message;
            }
        }

        private void ClearFields()
        {
            txtLicence.Text = "";
            txtBrand.Text = "";
            txtModel.Text = "";
            txtPrice.Text = "";
            txtColor.Text = "";
            ddlAvailable.SelectedIndex = 0;
            ViewState["SelectedCarKey"] = null;
        }
    }
}
