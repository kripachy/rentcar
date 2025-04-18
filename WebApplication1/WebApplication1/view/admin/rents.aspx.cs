using System;
using System.Data;
using System.Web.UI.WebControls;
using WebApplication1.Models;

namespace WebApplication1.view.admin
{
    public partial class rents : System.Web.UI.Page
    {
        Functions Conn;

        protected void Page_Load(object sender, EventArgs e)
        {
            Conn = new Functions();
            if (!IsPostBack)
            {
                ShowRents();
                LoadAvailableCars(); 
            }

        }

        private void LoadAvailableCars()
        {
            string query = "SELECT CPlateNum FROM CarTbl WHERE Status = 'Available'";
            DataTable dt = Conn.GetData(query);
            ddlCarPlate.DataSource = dt;
            ddlCarPlate.DataTextField = "CPlateNum";
            ddlCarPlate.DataValueField = "CPlateNum";
            ddlCarPlate.DataBind();
        }

        private void ShowRents()
        {
            string query = @"SELECT r.RentId, c.CPlateNum AS Car, cust.CustName AS Customer, 
                             r.RentDate, r.ReturnDate, r.Fees
                             FROM RentTbl r
                             JOIN CarTbl c ON r.Car = c.CPlateNum
                             JOIN CustomerTbl cust ON r.Customer = cust.CustId";
            gvCars.DataSource = Conn.GetData(query);
            gvCars.DataBind();
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtCustomerName.Text.Trim().Replace("'", "''");
                string address = txtCustomerAdress.Text.Trim().Replace("'", "''");
                string phone = txtCustomerPhone.Text.Trim().Replace("'", "''");
                string password = txtCustomerPassword.Text.Trim().Replace("'", "''");

                int customerId = GetCustomerIdByPhone(phone);
                if (customerId == -1)
                {
                    ShowError("Customer not found.");
                    return;
                }

                string carPlate = ddlCarPlate.SelectedValue;
                DateTime rentDate = DateTime.Now;
                DateTime returnDate = DateTime.Now.AddDays(7); // Example
                int fees = 100; // Example

                int rentId = GetAvailableRentId();

                string insertQuery = $"INSERT INTO RentTbl (RentId, Car, Customer, RentDate, ReturnDate, Fees) " +
                                     $"VALUES ({rentId}, '{carPlate}', {customerId}, '{rentDate:yyyy-MM-dd}', '{returnDate:yyyy-MM-dd}', {fees})";

                Conn.SetData(insertQuery);
                ShowSuccess("Rent added successfully.");
                ShowRents();
            }
            catch (Exception ex)
            {
                ShowError("Error adding rent: " + ex.Message);
            }
        }

        private int GetAvailableRentId()
        {
            string query = @"
                SELECT TOP 1 n AS AvailableId
                FROM (
                    SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
                    FROM RentTbl
                ) AS Numbers
                WHERE NOT EXISTS (
                    SELECT 1 FROM RentTbl WHERE RentId = Numbers.n
                )
                ORDER BY AvailableId";

            DataTable dt = Conn.GetData(query);
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0]["AvailableId"]);
            else
                return 1;
        }

        private int GetCustomerIdByPhone(string phone)
        {
            string query = $"SELECT CustId FROM CustomerTbl WHERE CustPhone = '{phone}'";
            DataTable dt = Conn.GetData(query);
            return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0][0]) : -1;
        }
        protected void Edit_Click(object sender, EventArgs e)
        {
            try
            {
                // Здесь логика редактирования записи
                ShowSuccess("Edit clicked - implement logic here.");
            }
            catch (Exception ex)
            {
                ShowError("Error during edit: " + ex.Message);
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                // Пример удаления первой записи в гриде — обнови под свой выбор
                if (gvCars.Rows.Count == 0) return;

                GridViewRow row = gvCars.Rows[0];
                int rentId = Convert.ToInt32(row.Cells[0].Text);
                string delQuery = $"DELETE FROM RentTbl WHERE RentId = {rentId}";

                Conn.SetData(delQuery);
                ShowSuccess("Rent deleted.");
                ShowRents();
            }
            catch (Exception ex)
            {
                ShowError("Error deleting rent: " + ex.Message);
            }
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

      
    }
}
