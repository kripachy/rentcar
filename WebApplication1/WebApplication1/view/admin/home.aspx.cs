using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebApplication1.Models;

namespace WebApplication1.view.admin
{
    public partial class home : System.Web.UI.Page
    {
        private Functions functions;

        protected void Page_Load(object sender, EventArgs e)
        {
            functions = new Functions();

            if (!IsPostBack)
            {
                // Проверяем авторизацию
                if (Session["UserEmail"] == null)
                {
                    Response.Redirect("~/view/admin/login.aspx");
                    return;
                }

                string userEmail = Session["UserEmail"].ToString();
                if (!userEmail.Equals("admin@gmail.com", StringComparison.OrdinalIgnoreCase))
                {
                    Response.Redirect("~/view/admin/userdashboard.aspx");
                    return;
                }
            }
        }
    }
}