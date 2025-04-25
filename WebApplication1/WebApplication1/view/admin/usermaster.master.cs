using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.view.admin
{
    public partial class usermaster : System.Web.UI.MasterPage
    {
        protected string GetActiveClass(string pageName)
        {
            string currentPage = System.IO.Path.GetFileName(Request.Path);
            return currentPage.Equals(pageName, StringComparison.OrdinalIgnoreCase)
                ? "nav-link active text-danger"
                : "nav-link text-danger";
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
