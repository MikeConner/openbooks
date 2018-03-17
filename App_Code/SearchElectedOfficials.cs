using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for SearchElectedOfficials
/// </summary>
public class SearchElectedOfficials
{
    public SearchElectedOfficials()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static DataTable GetPoliticians(string sortColumn, string sortDirection)
    {
        DataTable table = new DataTable("ElectedOfficials");
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("ElectedOfficialsSQL", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@sortColumn", SqlDbType.VarChar, 25).Value = sortColumn;
                cmd.Parameters.Add("@sortDirection", SqlDbType.Char, 4).Value = sortDirection;
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                table.Load(reader);
                return table;
            }
        }
    }
}