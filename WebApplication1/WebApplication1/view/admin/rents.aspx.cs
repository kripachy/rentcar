using System;
using System.Data;
using System.Data.SqlClient;
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
            }
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

                
                string query = $"UPDATE RentTbl SET RentDate = '{rentDate}', ReturnDate = '{returnDate}', Fees = {fees} WHERE RentId = {rentId}";
                Conn.SetData(query);

                ShowRents();
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void gvCars_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int rentId = Convert.ToInt32(gvCars.DataKeys[e.RowIndex].Value);
                string carPlateNum = null;

                
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbfilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True;Connect Timeout=30;";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction(); 

                    try
                    {
                        
                        string getCarPlateQuery = "SELECT Car FROM RentTbl WHERE RentId = @RentId";
                        using (SqlCommand cmd = new SqlCommand(getCarPlateQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@RentId", rentId);
                            object result = cmd.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                            {
                                carPlateNum = result.ToString();
                            }
                        }

                      
                        string deleteRentQuery = "DELETE FROM RentTbl WHERE RentId = @RentId";
                        using (SqlCommand cmd = new SqlCommand(deleteRentQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@RentId", rentId);
                            cmd.ExecuteNonQuery();
                        }

                     
                        if (!string.IsNullOrEmpty(carPlateNum))
                        {
                            string updateCarStatusQuery = "UPDATE CarTbl SET Status = 'Available' WHERE CPlateNum = @CPlateNum";
                            using (SqlCommand cmd = new SqlCommand(updateCarStatusQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@CPlateNum", carPlateNum);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit(); 

                        ShowRents();
                    }
                    catch (Exception exT)
                    {
                        transaction.Rollback(); 
                        throw exT;
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
        }
    }
}
