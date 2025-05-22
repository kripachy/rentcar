using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using WebApplication1.Models;

namespace WebApplication1.view.admin
{
    public partial class AstonMartinVanquish : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "text/html; charset=utf-8";
            Response.Charset = "utf-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.HeaderEncoding = Encoding.UTF8;

            if (!IsPostBack)
            {
                LoadCarDetails();
            }
        }

        private void LoadCarDetails()
        {
            Functions functions = new Functions();
            string query = "SELECT Price FROM CarTbl WHERE Brand = 'Aston Martin' AND Model = 'Vanquish' AND Color = 'White'";
            DataTable dt = functions.GetData(query);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                decimal price = Convert.ToDecimal(row["Price"]);
                lblPrice.Text = price.ToString("0.00");
            }
        }

        protected void btnRent_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/RentalForm.aspx?car=AstonMartinVanquish&color=White");
        }
    }
} 