using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace WebApplication1.Models
{
    public class Functions
    {
        private SqlConnection Conn;
        private SqlCommand cmd;
        private DataTable dt;
        private SqlDataAdapter sda;
        private string ConnStr;

        public Functions()
        {
            ConnStr = @"Data Source=(LocalDB)\MSSQLLocalDB;
                      AttachDbFilename=C:\Users\kiril\OneDrive\Документы\WheelDeal.mdf;
                      Integrated Security=True;
                      Connect Timeout=30;";
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