using System;
using System.Web.UI;

namespace WebApplication1.view.admin
{
    public partial class info : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Можно добавить дополнительную логику при необходимости
            }
        }
    }
}