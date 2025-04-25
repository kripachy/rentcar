using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace WebApplication1
{
    public partial class profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Session["UserEmail"] == null)
            {
                Response.Redirect("login.aspx");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Пожалуйста, введите корректную информацию');", true);
                return;
            }

            try
            {
                SaveProfileData();
                Response.Redirect("home.aspx");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "error", $"alert('Ошибка: {ex.Message}');", true);
            }
        }

        private bool ValidateInput()
        {
            return ValidateName(txtName.Text) &&
                   ValidatePhone(txtPhone.Text) &&
                   ValidateAddress(txtAddress.Text);
        }

        private bool ValidateName(string text)
        {
            return !string.IsNullOrWhiteSpace(text) &&
                   Regex.IsMatch(text, @"^[a-zA-Zа-яА-ЯёЁ\s'-]+$");
        }

        private bool ValidatePhone(string text)
        {
            return Regex.IsMatch(text, @"^80\d{9}$");
        }

        private bool ValidateAddress(string text)
        {
            return Regex.IsMatch(text.Trim(), @"^[a-zA-Zа-яА-ЯёЁ0-9\s,.-]+$");
        }

        private void SaveProfileData()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True;Connect Timeout=30;";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var getIdCmd = new SqlCommand("SELECT CustId FROM CustomerAuthTbl WHERE CustEmail = @Email", connection);
                getIdCmd.Parameters.AddWithValue("@Email", Session["UserEmail"]);

                object result = getIdCmd.ExecuteScalar();
                if (result == null)
                {
                    throw new Exception("Пользователь не найден");
                }

                int custId = (int)result;
                var updateCmd = new SqlCommand(
                    "UPDATE CustomerTbl SET CustName = @Name, CustAdd = @Address, CustPhone = @Phone WHERE CustId = @CustId", connection);

                updateCmd.Parameters.AddWithValue("@Name", txtName.Text);
                updateCmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                updateCmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                updateCmd.Parameters.AddWithValue("@CustId", custId);

                updateCmd.ExecuteNonQuery();
            }
        }
    }
}
