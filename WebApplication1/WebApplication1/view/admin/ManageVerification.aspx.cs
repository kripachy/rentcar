using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Models;

namespace WebApplication1.view.admin
{
    public partial class ManageVerification : System.Web.UI.Page
    {
        // Control declarations (missing designer file)
        protected global::System.Web.UI.WebControls.GridView gvLicenses;
        protected global::System.Web.UI.WebControls.Label lblMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               // Check admin access if needed
               LoadData();
            }
        }

        private void LoadData()
        {
            using (var conn = new SqlConnection(Functions.GetConnectionString()))
            {
                conn.Open();
                string query = @"
SELECT l.LicenseId, l.CustomerId, c.CustUserName as Username, l.LicenseNumber, l.IssuedDate, l.Status, l.FileId
FROM DrivingLicense l
JOIN CustomerTbl c ON l.CustomerId = c.CustId
WHERE l.Status = 'Pending'
ORDER BY l.CreatedAt DESC";
                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);
                        gvLicenses.DataSource = dt;
                        gvLicenses.DataBind();
                    }
                }
            }
        }

        protected void gvLicenses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var cbl = e.Row.FindControl("cblCategories") as CheckBoxList;
                if (cbl != null)
                {
                    using (var conn = new SqlConnection(Functions.GetConnectionString()))
                    {
                        conn.Open();
                        var cmd = new SqlCommand("SELECT CategoryId, Name FROM CarCategory ORDER BY CategoryId", conn);
                        using (var reader = cmd.ExecuteReader())
                        {
                            cbl.DataSource = reader;
                            cbl.DataTextField = "Name";
                            cbl.DataValueField = "CategoryId";
                            cbl.DataBind();
                        }
                    }
                    // By default, pre-check 'Эконом' and 'Стандарт' or based on experience? 
                    // Let's leave it to admin as requested.
                }
            }
        }

        protected void gvLicenses_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Approve")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int licenseId = Convert.ToInt32(gvLicenses.DataKeys[rowIndex].Value);
                var row = gvLicenses.Rows[rowIndex];
                
                var cbl = row.FindControl("cblCategories") as CheckBoxList;
                var hfCustId = row.FindControl("hfCustomerId") as HiddenField;
                int customerId = Convert.ToInt32(hfCustId.Value);

                SaveAllowedCategories(customerId, cbl);
                UpdateStatus(licenseId, "Approved", "Проверено администратором. Категории назначены.");
                lblMsg.Text = "Документ одобрен, категории назначены.";
                lblMsg.CssClass = "text-success";
            }
            else if (e.CommandName == "Reject")
            {
                int licenseId = Convert.ToInt32(e.CommandArgument);
                UpdateStatus(licenseId, "Rejected", "Отклонено администратором.");
                RevertCustomerExperience(licenseId);
                lblMsg.Text = "Документ отклонен.";
                lblMsg.CssClass = "text-danger";
            }
            LoadData();
        }

        private void SaveAllowedCategories(int customerId, CheckBoxList cbl)
        {
            using (var conn = new SqlConnection(Functions.GetConnectionString()))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // Clean existing
                        var del = new SqlCommand("DELETE FROM CustomerAllowedCategory WHERE CustomerId=@c", conn, tran);
                        del.Parameters.AddWithValue("@c", customerId);
                        del.ExecuteNonQuery();

                        // Add new
                        foreach (ListItem item in cbl.Items)
                        {
                            if (item.Selected)
                            {
                                var ins = new SqlCommand("INSERT INTO CustomerAllowedCategory (CustomerId, CategoryId) VALUES (@c, @cat)", conn, tran);
                                ins.Parameters.AddWithValue("@c", customerId);
                                ins.Parameters.AddWithValue("@cat", item.Value);
                                ins.ExecuteNonQuery();
                            }
                        }
                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        private void UpdateStatus(int id, string status, string comment)
        {
            using (var conn = new SqlConnection(Functions.GetConnectionString()))
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE DrivingLicense SET Status=@s, Comment=@c, VerifiedAt=GETDATE() WHERE LicenseId=@id", conn);
                cmd.Parameters.AddWithValue("@s", status);
                cmd.Parameters.AddWithValue("@c", comment);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        private void RevertCustomerExperience(int licenseId)
        {
            using (var conn = new SqlConnection(Functions.GetConnectionString()))
            {
                conn.Open();
                // Get CustomerId
                var getCust = new SqlCommand("SELECT CustomerId FROM DrivingLicense WHERE LicenseId=@id", conn);
                getCust.Parameters.AddWithValue("@id", licenseId);
                object custIdObj = getCust.ExecuteScalar();
                if (custIdObj != null) 
                {
                    int custId = (int)custIdObj;
                    // Reset experience
                    var reset = new SqlCommand("UPDATE CustomerTbl SET DrivingExperienceYears=0, LicenseIssueDate=NULL WHERE CustId=@c", conn);
                    reset.Parameters.AddWithValue("@c", custId);
                    reset.ExecuteNonQuery();
                }
            }
        }
    }
}
