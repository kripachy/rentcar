using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ClosedXML.Excel;

namespace WebApplication1.view.admin
{
    public partial class customers : System.Web.UI.Page
    {
        Models.Functions Conn;

        protected void Page_Load(object sender, EventArgs e)
        {
            Conn = new Models.Functions();
            if (!IsPostBack)
            {
                ShowCustomers();
            }
        }

        private void ShowCustomers()
        {
            string query = "SELECT * FROM CustomerTbl";
            gvCustomers.DataSource = Conn.GetData(query);
            gvCustomers.DataBind();
        }

        protected void gvCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = gvCustomers.SelectedRow;
                if (row != null)
                {
                    // Получаем CustId из DataKeys
                    string custId = gvCustomers.DataKeys[row.RowIndex].Value.ToString();
                    ViewState["SelectedCustomerId"] = custId;

                    // Получаем значения из ячеек (учитывая CommandField)
                    txtCustomerName.Text = HttpUtility.HtmlDecode(row.Cells[2].Text); // Name
                    txtCustomerAdress.Text = HttpUtility.HtmlDecode(row.Cells[3].Text); // Address
                    txtCustomerPhone.Text = HttpUtility.HtmlDecode(row.Cells[4].Text); // Phone

                    // Для пароля используем FindControl, так как он может быть скрыт
                    Label lblPassword = (Label)row.FindControl("lblPassword");
                    if (lblPassword != null)
                    {
                        txtCustomerPassword.Text = HttpUtility.HtmlDecode(lblPassword.Text);
                    }
                    else
                    {
                        // Альтернативный вариант, если FindControl не работает
                        txtCustomerPassword.Text = HttpUtility.HtmlDecode(row.Cells[5].Text);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Error selecting customer: " + ex.Message);
            }
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtCustomerName.Text.Trim().Replace("'", "''");
                string address = txtCustomerAdress.Text.Trim().Replace("'", "''");
                string phone = txtCustomerPhone.Text.Trim().Replace("'", "''");
                string password = txtCustomerPassword.Text.Trim().Replace("'", "''");

                if (!IsValidPhoneNumber(phone))
                {
                    ShowError("Invalid phone number format.");
                    return;
                }

                string checkQuery = $"SELECT COUNT(*) FROM CustomerTbl WHERE CustPhone = '{phone}'";
                DataTable result = Conn.GetData(checkQuery);

                if (Convert.ToInt32(result.Rows[0][0]) > 0)
                {
                    ShowError("A customer with this phone number already exists.");
                    return;
                }

                string query = $"INSERT INTO CustomerTbl (CustName, CustAdd, CustPhone, CustPassword) " +
                               $"VALUES ('{name}', '{address}', '{phone}', '{password}')";
                Conn.SetData(query);

                ShowCustomers();
                ClearFields();
                ShowSuccess("Customer Added");
            }
            catch (Exception ex)
            {
                ShowError("Error adding customer: " + ex.Message);
            }
        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["SelectedCustomerId"] == null)
                {
                    ShowError("Please select a customer to edit.");
                    return;
                }

                string id = ViewState["SelectedCustomerId"].ToString();
                string name = txtCustomerName.Text.Trim().Replace("'", "''");
                string address = txtCustomerAdress.Text.Trim().Replace("'", "''");
                string phone = txtCustomerPhone.Text.Trim().Replace("'", "''");
                string password = txtCustomerPassword.Text.Trim().Replace("'", "''");

                if (!IsValidPhoneNumber(phone))
                {
                    ShowError("Invalid phone number format.");
                    return;
                }

                string checkQuery = $"SELECT COUNT(*) FROM CustomerTbl WHERE CustPhone = '{phone}' AND CustId <> {id}";
                DataTable result = Conn.GetData(checkQuery);

                if (Convert.ToInt32(result.Rows[0][0]) > 0)
                {
                    ShowError("This phone number is already used by another customer.");
                    return;
                }

                string query = $"UPDATE CustomerTbl SET CustName='{name}', CustAdd='{address}', " +
                               $"CustPhone='{phone}', CustPassword='{password}' WHERE CustId={id}";
                Conn.SetData(query);

                ShowCustomers();
                ClearFields();
                ShowSuccess("Customer Updated");
            }
            catch (Exception ex)
            {
                ShowError("Error updating customer: " + ex.Message);
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["SelectedCustomerId"] == null)
                {
                    ShowError("Please select a customer to delete.");
                    return;
                }

                string id = ViewState["SelectedCustomerId"].ToString();
                ShowSuccess($"Selected Customer ID: {id}"); // Добавим вывод для отладки

                // Удаляем сначала аренды
                string deleteRentalsQuery = $"DELETE FROM RentTbl WHERE CustId = {id}";
                Conn.SetData(deleteRentalsQuery);

                // Потом клиента
                string deleteCustomerQuery = $"DELETE FROM CustomerTbl WHERE CustId = {id}";
                Conn.SetData(deleteCustomerQuery);

                ShowCustomers();
                ClearFields();
                ShowSuccess("Customer and related rentals deleted.");
            }
            catch (Exception ex)
            {
                ShowError("Error deleting customer: " + ex.Message);
            }
        }




        private void ClearFields()
        {
            txtCustomerName.Text = "";
            txtCustomerAdress.Text = "";
            txtCustomerPhone.Text = "";
            txtCustomerPassword.Text = "";
            ViewState["SelectedCustomerId"] = null;
        }

        private bool IsValidPhoneNumber(string phone)
        {
            return Regex.IsMatch(phone, @"^\d{10,15}$");
        }

        private void ShowError(string message)
        {
            ErrorMsg.Text = message;
            ErrorMsg.CssClass = "text-danger";
            ErrorMsg.Visible = true;
        }

        private void ShowSuccess(string message)
        {
            ErrorMsg.Text = message;
            ErrorMsg.CssClass = "text-success";
            ErrorMsg.Visible = true;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT CustId, CustName, CustAdd, CustPhone, CustPassword FROM CustomerTbl";
                DataTable dt = Conn.GetData(query);

                if (dt.Rows.Count > 0)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "Customers");

                        using (MemoryStream ms = new MemoryStream())
                        {
                            wb.SaveAs(ms);
                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;filename=CustomerExport_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
                            Response.BinaryWrite(ms.ToArray());
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
                else
                {
                    ShowError("No data to export.");
                }
            }
            catch (Exception ex)
            {
                ShowError("Export failed: " + ex.Message);
            }
        }
    }
}