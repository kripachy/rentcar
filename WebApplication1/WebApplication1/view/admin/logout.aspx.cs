using System;

namespace WebApplication1.view.admin
{
    public partial class logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Clear all session variables
            Session.Clear();
            Session.Abandon();

            // Redirect to home page
            Response.Redirect("userdashboard.aspx");
        }
    }
} 