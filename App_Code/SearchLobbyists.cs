using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace OpenBookPgh
{
	/// <summary>
	/// Summary description for SearchLobbyists
	/// </summary>
	public class SearchLobbyists
	{
		public static string GenerateQueryString(int lobbyistID, string lobbyistKeywords, string companyKeywords)
		{
			string queryString = "Lobbyists.aspx?";

			// LobbyistID Only Search in Admin
			if(lobbyistID != 0)
			{
				queryString += "id=" + lobbyistID;
			}
			// Regular Query Searches
			else
			{
				// Lobbyist Keywords
				if (!string.IsNullOrEmpty(lobbyistKeywords))
				{
					queryString += "&lobbyist=" + System.Web.HttpUtility.UrlEncode(lobbyistKeywords);
				}

				// Company Keywords
				else if (!string.IsNullOrEmpty(companyKeywords))
				{
					queryString += "&company=" + System.Web.HttpUtility.UrlEncode(companyKeywords);
				}
			}
			return queryString;
		}

		public static SearchParamsLobbyists GetQueryStringValues(System.Web.HttpRequest Request)
		{
			SearchParamsLobbyists sp = new SearchParamsLobbyists();

			// Lobbyist Only Search in Admin
			int lobbyistID = Utils.GetIntFromQueryString(Request.QueryString["id"]);
			if (lobbyistID != 0)
			{
				sp.lobbyistID = lobbyistID;
			}

			// Lobbyist Keywords
			string lobbyistKeywords = Utils.GetStringFromQueryString(Request.QueryString["lobbyist"], true);
			if (lobbyistKeywords != "")
			{
				sp.lobbyistKeywords = lobbyistKeywords;
			}
			// Company Keywords
			string companyKeywords = Utils.GetStringFromQueryString(Request.QueryString["company"], true);
			if (companyKeywords != "")
			{
				sp.companyKeywords = companyKeywords;
			}

			return sp;

		}

		public static DataTable GetLobbyists(SearchParamsLobbyists sp, int pageIndex, int maximumRows, string sortColumn, string sortDirection)
		{
			DataTable table = new DataTable("Lobbyists");
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("SearchLobbyistsSQL", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@pageIndex", SqlDbType.Int).Value = pageIndex;
					cmd.Parameters.Add("@maximumRows", SqlDbType.Int).Value = maximumRows;
					cmd.Parameters.Add("@sortColumn", SqlDbType.VarChar, 25).Value = sortColumn;
					cmd.Parameters.Add("@sortDirection", SqlDbType.Char, 4).Value = sortDirection;
					cmd.Parameters.Add("@lobbyistID", SqlDbType.Int).Value = (sp.lobbyistID == 0) ? System.DBNull.Value : (object)sp.lobbyistID;
					cmd.Parameters.Add("@lobbyistKeywords", SqlDbType.VarChar, 100).Value = (sp.lobbyistKeywords == null) ? System.DBNull.Value : (object)sp.lobbyistKeywords;
					cmd.Parameters.Add("@companyKeywords", SqlDbType.VarChar, 100).Value = (sp.companyKeywords == null) ? System.DBNull.Value : (object)sp.companyKeywords;	
					conn.Open();

					SqlDataReader reader = cmd.ExecuteReader();
					table.Load(reader);
					return table;
				}
			}
		}

		public static int GetLobbyistsCount(SearchParamsLobbyists sp)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("SearchLobbyistsCount", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@lobbyistID", SqlDbType.Int).Value = (sp.lobbyistID == 0) ? System.DBNull.Value : (object)sp.lobbyistID;
					cmd.Parameters.Add("@lobbyistKeywords", SqlDbType.VarChar, 100).Value = (sp.lobbyistKeywords == null) ? System.DBNull.Value : (object)sp.lobbyistKeywords;
					cmd.Parameters.Add("@companyKeywords", SqlDbType.VarChar, 100).Value = (sp.companyKeywords == null) ? System.DBNull.Value : (object)sp.companyKeywords;	
					int rowCount = 0;
					try
					{
						conn.Open();
						rowCount = (Int32)cmd.ExecuteScalar();
					}
					catch (Exception ex)
					{
						throw ex;
					}
					return (int)rowCount;
				}
			}
		}

		public static DataTable GetLobbyistsCompanies(string xmlString)
		{            
			DataTable table = new DataTable("Companies");
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("SearchLobbyistsCompaniesXML", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@xmlDocument", SqlDbType.VarChar, 1000).Value = xmlString;
					conn.Open();

					SqlDataReader reader = cmd.ExecuteReader();
					table.Load(reader);
					return table;
				}
			}
        }
   		public static DataTable GetLobbyistsCompanies(DataTable dt)  //DAS
		{
            string LobbyistIDs = "";

            foreach (DataRow sourcerow in dt.Rows)
            {
                LobbyistIDs += sourcerow["LobbyistID"].ToString() + ',';
            }

            DataTable table = new DataTable("Companies");
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SearchLobbyistsCompanies", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LobbyistIDs", LobbyistIDs);
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    table.Load(reader);
                    return table;
                }
            }
		}
	
		public static string GetXMLString(DataTable searchDT)
		{
			//<searchresults><lobbyist ID="" /></searchresults>
			string xml = string.Empty;
	        
			if (searchDT.Rows.Count > 0)
			{
				xml = "<searchresults>";
				foreach (DataRow row in searchDT.Rows)
				{
					xml += "<lobbyist LID=\"" + row["LobbyistID"].ToString() + "\"/>";
				}
				xml += "</searchresults>";
			}
			return xml;
		}

	}
}
