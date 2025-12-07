using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Configuration;
using System.IO;

namespace WebApplication1.Models
{
    public class Functions
    {
        private SqlConnection Conn;
        private SqlCommand cmd;
        private DataTable dt;
        private SqlDataAdapter sda;
        private string ConnStr;

        // Централизованный метод для получения строки подключения
        public static string GetConnectionString()
        {
            // Пытаемся получить из Web.config
            try
            {
                var configConnStr = WebConfigurationManager.ConnectionStrings["DefaultConnection"];
                if (configConnStr != null && !string.IsNullOrEmpty(configConnStr.ConnectionString))
                {
                    // Заменяем |DataDirectory| на реальный путь
                    string connStr = configConnStr.ConnectionString;
                    string dataDir = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
                    if (string.IsNullOrEmpty(dataDir))
                    {
                        dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
                    }
                    string dbPath = Path.Combine(dataDir, "WheelDeal.mdf");
                    
                    // Создаем директорию, если её нет
                    if (!Directory.Exists(dataDir))
                    {
                        Directory.CreateDirectory(dataDir);
                    }
                    
                    return $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;Connect Timeout=30;";
                }
            }
            catch { }

            // Если не получилось, используем путь вне OneDrive (в локальной папке пользователя)
            string localDbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WheelDeal", "WheelDeal.mdf");
            string localDbDir = Path.GetDirectoryName(localDbPath);
            
            // Создаем директорию, если её нет
            if (!Directory.Exists(localDbDir))
            {
                Directory.CreateDirectory(localDbDir);
            }

            return $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={localDbPath};Integrated Security=True;Connect Timeout=30;";
        }

        public Functions()
        {
            ConnStr = GetConnectionString();
            Conn = new SqlConnection(ConnStr);
            cmd = new SqlCommand();
            cmd.Connection = Conn;
        }

        public DataTable GetData(string Query)
        {
            dt = new DataTable();
            using (sda = new SqlDataAdapter(Query, Conn))
            {
                sda.Fill(dt);
            }
            return dt;
        }

        public int SetData(string Query)
        {
            int rcnt = 0;
            try
            {
                if (Conn.State == ConnectionState.Closed)
                {
                    Conn.Open();
                }
                cmd.CommandText = Query;
                rcnt = cmd.ExecuteNonQuery();
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
            return rcnt;
        }
    }
}