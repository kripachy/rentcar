using System;
using System.IO;
using System.Web.UI;

namespace WebApplication1.view.admin
{
    public partial class adminprofile : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Проверяем, является ли пользователь администратором
                if (Session["UserEmail"] == null || !Session["UserEmail"].ToString().Equals("admin@gmail.com", StringComparison.OrdinalIgnoreCase))
                {
                    Response.Redirect("~/view/admin/login.aspx");
                    return;
                }

                // Загружаем текущие учетные данные администратора
                LoadAdminCredentials();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Логика сохранения изменений
            SaveAdminCredentials();
        }

        private void LoadAdminCredentials()
        {
            // Логика для чтения email и пароля из login.aspx.cs
        }

        private void SaveAdminCredentials()
        {
            // Логика для записи нового email и пароля в login.aspx.cs
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMsg.Text = message;
            lblMsg.CssClass = isSuccess ? "d-block text-center mt-3 text-success" : "d-block text-center mt-3 text-danger";
            lblMsg.Visible = true;
        }
    }
} 