using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace WebApplication1.view.admin
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class CheckColorAvailability : System.Web.Services.WebService
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False";

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CheckAvailability(string brand, string model, string color)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Debug query to see all cars of this model
                    string debugQuery = "SELECT Brand, Model, Color, Status FROM CarTbl WHERE Brand = 'Audi' AND Model = 'TT'";
                    using (SqlCommand debugCmd = new SqlCommand(debugQuery, conn))
                    {
                        using (SqlDataReader reader = debugCmd.ExecuteReader())
                        {
                            Debug.WriteLine("=== All Audi TT cars in database ===");
                            while (reader.Read())
                            {
                                Debug.WriteLine($"Brand: {reader["Brand"]}, Model: {reader["Model"]}, Color: {reader["Color"]}, Status: {reader["Status"]}");
                            }
                        }
                    }

                    // Main availability check query
                    string query = @"
                        SELECT COUNT(*) 
                        FROM CarTbl 
                        WHERE Brand = 'Audi' 
                        AND Model = 'TT' 
                        AND Color = @Color 
                        AND Status = 'Available'";

                    Debug.WriteLine($"Checking availability for: Brand=Audi, Model=TT, Color={color}");
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Color", color);
                        int count = (int)cmd.ExecuteScalar();
                        Debug.WriteLine($"Found {count} available cars for color {color}");
                        
                        return new JavaScriptSerializer().Serialize(new { available = count > 0 });
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking availability: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return new JavaScriptSerializer().Serialize(new { available = false, error = ex.Message });
            }
        }
    }
} 