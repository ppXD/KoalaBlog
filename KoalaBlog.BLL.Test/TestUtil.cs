using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Test
{
    class TestUtil
    {
        public static void CleanUpData()
        {
            // Run the clean up script 
            System.Configuration.AppSettingsReader appReader = new System.Configuration.AppSettingsReader();
            string connStr = appReader.GetValue("KoalaBlog.ConnectionString", typeof(string)).ToString();
            SqlConnection conn = new SqlConnection(connStr);
            System.IO.StreamReader reader = new System.IO.StreamReader("../../../../SQL/cleanup.sql");
            string content = reader.ReadToEnd();
            SqlCommand cmd = new SqlCommand(content);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = conn;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
