using System;
using System.Text;
using System.Web.UI;

namespace WebApplication1.view.admin
{
    public partial class userdashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "text/html; charset=utf-8";
            Response.Charset = "utf-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.HeaderEncoding = Encoding.UTF8;
        }
    }
}
