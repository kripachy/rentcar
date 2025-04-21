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

        protected void gvCars_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCars.EditIndex = e.NewEditIndex;
            ShowRents();
        }

        protected void gvCars_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCars.EditIndex = -1;
            ShowRents();
        }

        protected void gvCars_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvCars.Rows[e.RowIndex];
                int rentId = Convert.ToInt32(gvCars.DataKeys[e.RowIndex].Value);

                string car = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
                string customer = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();
                string rentDate = ((TextBox)row.Cells[3].Controls[0]).Text.Trim();
                string returnDate = ((TextBox)row.Cells[4].Controls[0]).Text.Trim();
                string fees = ((TextBox)row.Cells[5].Controls[0]).Text.Trim();

                // Обновляем только RentTbl, а имя клиента и номер машины лучше не менять в продакшене.
                string query = $"UPDATE RentTbl SET RentDate = '{rentDate}', ReturnDate = '{returnDate}', Fees = {fees} WHERE RentId = {rentId}";
                Conn.SetData(query);

                ShowSuccess("Rent updated successfully.");
                gvCars.EditIndex = -1;
                ShowRents();
            }
            catch (Exception ex)
            {
                ShowError("Error updating rent: " + ex.Message);
            }
        }

        protected void gvCars_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int rentId = Convert.ToInt32(gvCars.DataKeys[e.RowIndex].Value);
                string query = $"DELETE FROM RentTbl WHERE RentId = {rentId}";
                Conn.SetData(query);
                ShowSuccess("Rent deleted successfully.");
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
