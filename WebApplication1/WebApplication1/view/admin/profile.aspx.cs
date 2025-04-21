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
            if (!ValidateInputs())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please enter valid information (English characters only)');", true);
                return;
            }

            try
            {
                SaveProfileData();
                Response.Redirect("home.aspx");
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "error", $"alert('Error: {ex.Message}');", true);
            }
        }

        private bool ValidateInputs()
        {
            return IsValidEnglish(txtName.Text) &&
                   IsValidPhone(txtPhone.Text) &&
                   IsValidEnglishAddress(txtAddress.Text);
        }

        private bool IsValidEnglish(string text)
        {
            return !string.IsNullOrWhiteSpace(text) &&
                   Regex.IsMatch(text, @"^[a-zA-Z\s'-]+$");
        }

        private bool IsValidPhone(string text)
        {
            return Regex.IsMatch(text, @"^80\d{9}$");
        }


        private bool IsValidEnglishAddress(string text)
        {
            // Разрешаем буквы, пробелы, а затем пробел и число
            return Regex.IsMatch(text, @"^[a-zA-Z\s]+ \d+$");
        }




        private void SaveProfileData()
        {
            string connStr = @"Your_Connection_String";

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();

                var cmd = new SqlCommand(
                    "INSERT INTO CustomerTbl (CustId, CustName, CustAdd, CustPhone, CustPassword) " +
                    "SELECT CustId, @Name, @Address, @Phone, CustPassword " +
                    "FROM CustomerAuthTbl WHERE CustEmail = @Email", conn);

                cmd.Parameters.AddWithValue("@Email", Session["UserEmail"]);
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);

                cmd.ExecuteNonQuery();
            }
        }
    }
}