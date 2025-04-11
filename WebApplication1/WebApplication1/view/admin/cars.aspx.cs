using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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


        public override void VerifyRenderingInServerForm(Control control)
        {
            
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtLicence.Text == "" || txtBrand.Text == "" || txtModel.Text == "" || txtPrice.Text == "" || txtColor.Text == "")
                {
                    ErrorMsg.InnerText = "Missing Information";
                }
                else
                {
                    string CPlateNum = txtLicence.Text;
                    string Brand = txtBrand.Text;
                    string Model = txtModel.Text;
                    int Price = Convert.ToInt32(txtPrice.Text);
                    string Color = txtColor.Text;
                    string Status = ddlAvailable.SelectedItem.Text;
                    string Query = "insert into CarTbl values('{0}','{1}','{2}','{3}','{4}','{5}')";
                    Query = String.Format(Query, CPlateNum, Brand, Model, Price, Color, Status);
                    Conn.SetData(Query);
                    ShowCars(); 
                    ErrorMsg.InnerText = "Car Added";

                }

            }
            catch(Exception Ex) 
            {
                /*throw;*/
                ErrorMsg.InnerText = Ex.Message;
            }
        }
    }
}