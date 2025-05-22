using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.view.admin
{
    public partial class adminmaster : MasterPage
    {
        protected HyperLink lnkHome;
        protected HyperLink lnkHomeNav;
        protected HyperLink lnkCars;
        protected HyperLink lnkRents;
        protected HyperLink lnkReturns;
        protected Literal litUserEmail;
        protected Button btnLogout;

        protected void Page_Load(object sender, EventArgs e)
        {
            // We can potentially add a check here if ANY user is logged in, 
            // but we will remove admin-specific checks for now.
            // If you need general user authentication on admin pages, let me know.
        }
    }
}