using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Models;

namespace WebApplication1.view
{
    public partial class login : Page
    {
        private Functions functions;

        protected void Page_Load(object sender, EventArgs e)
        {
            functions = new Functions();
            if (!IsPostBack)
            {
                // If user is already logged in, redirect to home page
                if (Session["UserEmail"] != null)
                {
                    Response.Redirect("~/view/user/home.aspx"); // Redirect to user home for any logged-in user
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ShowError("Пожалуйста, заполните все поля");
                return;
            }

            // Check credentials in the authorization table
            string query = $"SELECT * FROM CustomerAuthTbl WHERE CustEmail = '{email}' AND CustPassword = '{password}'";
            DataTable dt = functions.GetData(query);

            if (dt.Rows.Count > 0)
            {
                // Save user email and id in session (fetch UserId from CustomerAuthTbl)
                Session["UserEmail"] = email;
                // Assuming CustId is available in CustomerAuthTbl or can be fetched based on email
                // For now, we won't handle UserId here to simplify, but it might be needed later.

                // Redirect to user home page after successful login
                Response.Redirect("~/view/user/home.aspx");
            }
            else
            {
                ShowError("Неверный email или пароль");
            }
        }

        private void ShowError(string message)
        {
            ErrorMsg.Text = message;
            ErrorMsg.Visible = true;
        }

        // Ensure controls are declared if they are used in the ASPX but not automatically generated
        protected TextBox txtEmail;
        protected TextBox txtPassword;
        protected Button btnLogin;
        protected Label ErrorMsg;
    }
} 