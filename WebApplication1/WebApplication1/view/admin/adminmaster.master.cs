using System;
using System.Web;
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
            if (!IsPostBack)
            {
                // Проверка авторизации и роли администратора
                if (Session["UserEmail"] == null || !IsAdmin(Session["UserEmail"].ToString()))
                {
                    Response.Redirect("~/view/admin/login.aspx");
                }
            }
        }

        private bool IsAdmin(string email)
        {
            return email.Equals("admin@gmail.com", StringComparison.OrdinalIgnoreCase);
        }

        protected string GetActiveClass(string pageName)
        {
            string currentPage = System.IO.Path.GetFileName(Request.Path);
            if (string.IsNullOrWhiteSpace(currentPage))
            {
                currentPage = "home.aspx";
            }
            return currentPage.Equals(pageName, StringComparison.OrdinalIgnoreCase) ? "active" : "";
        }

        protected string GetCurrentPageTitle()
        {
            string currentPage = System.IO.Path.GetFileName(Request.Path);
            if (string.IsNullOrWhiteSpace(currentPage))
            {
                currentPage = "home.aspx";
            }
            switch (currentPage?.ToLowerInvariant())
            {
                case "home.aspx":
                    return "Главная";
                case "cars.aspx":
                    return "Автомобили";
                case "customers.aspx":
                    return "Клиенты";
                case "rents.aspx":
                    return "Аренды";
                case "adminprofile.aspx":
                    return "Профиль";
                case "comments.aspx":
                    return "Комментарии";
                case "manageverification.aspx":
                    return "Проверка";
                case "slider.aspx":
                    return "Слайдер";
                default:
                    return "";
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Очищаем сессию
            Session.Clear();
            Session.Abandon();

            // Добавляем небольшую задержку для уверенности
            System.Threading.Thread.Sleep(50);

            // Перенаправляем на страницу входа
            Response.Redirect("~/view/admin/login.aspx");
        }
    }
}