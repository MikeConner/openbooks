using System.Configuration;
using System;
using System.Data.SqlClient;

namespace OnBasePMS
{
    class Program
    {
        static void Main(string[] args)
        {
            String sAttr;
            sAttr = ConfigurationManager.AppSettings.Get("onbaseDBserver");
            Console.WriteLine(sAttr);

            String connectionString = "User ID=" + ConfigurationManager.AppSettings.Get("onbaseDBuser") + ";Password=" + ConfigurationManager.AppSettings.Get("onbaseDBpassword") + ";Initial Catalog=" + ConfigurationManager.AppSettings.Get("onbaseDBname") + ";Data Source=" + ConfigurationManager.AppSettings.Get("onbaseDBserver");



            SqlConnection conn = new SqlConnection(connectionString);

            string strSQL = "SELECT TOP 1000 * FROM[CityController].[dbo].[contracts]";
            SqlCommand command = new SqlCommand(strSQL, conn);
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("\t{0}\t{1}\t{2}",
                    reader[0], reader[1], reader[2]);
            }
            reader.Close();
            Console.ReadKey();
        }
    }
}
