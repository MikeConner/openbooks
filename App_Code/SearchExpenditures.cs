using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace OpenBookPgh
{
	/// <summary>
	/// Summary description for SearchExpenditures
	/// </summary>
	public class SearchExpenditures
	{
		public static string GenerateQueryString(int candidateID, string office, int year1, string vendorKeywords, string vendorSearchOptions, string keywords)
		{
			string queryString = "~/SearchExpenditures.aspx?";

			// Candidate ID
			if (candidateID != 0)
			{
				queryString += "&candidate=" + candidateID;
			}
			//Political Office
			if (!String.IsNullOrEmpty(office) && office != "all")
			{
				queryString += "&office=" + office;
			}
            // Date Paid
            if (year1 != 0)
            {
                queryString += "&yr1=" + year1;
            }
            // Keywords
			if (!string.IsNullOrEmpty(keywords))
			{
				queryString += "&q=" + System.Web.HttpUtility.UrlEncode(keywords);
			}
			// Vendor Keywords
			if (!string.IsNullOrEmpty(vendorKeywords) && !string.IsNullOrEmpty(vendorSearchOptions))
			{
				queryString += "&vendor=" + System.Web.HttpUtility.UrlEncode(vendorKeywords) + "&vtype=" + vendorSearchOptions;
			}
			return queryString;
		}

		public static SearchParamsExpenditures GetQueryStringValues(System.Web.HttpRequest Request)
		{
			SearchParamsExpenditures sp = new SearchParamsExpenditures();

			// Candidate ID
			int candidate = Utils.GetIntFromQueryString(Request.QueryString["candidate"]);
			if (candidate != 0)
			{
				sp.candidateID = candidate;
			}
			// Political Office
			string strAllowedOffice = "mayor council controller";
			string office = Utils.GetStringFromQueryString(Request.QueryString["office"], strAllowedOffice);
			if (office != "")
			{
				sp.office = office;
			}
            // Year Paid
            int yr1 = Utils.GetIntFromQueryString(Request.QueryString["yr1"]);
            if (yr1 != 0)
            {
                sp.datePaid = yr1;
            }
            // Vendor Keywords
			string vendorKeywords = Utils.GetStringFromQueryString(Request.QueryString["vendor"], true);
			if (vendorKeywords != "")
			{
				sp.vendorKeywords = vendorKeywords;
			}
			// Vendor Search Options
			string strAllowedOptions = "C B E";
			string vendorSearchOptions = Utils.GetStringFromQueryString(Request.QueryString["vtype"], strAllowedOptions);
			if (vendorSearchOptions != "")
			{
				sp.vendorSearchOptions = vendorSearchOptions;
			}
			// Keywords
			string keywordsString = Utils.GetStringFromQueryString(Request.QueryString["q"], true);
			if (keywordsString != "")
			{
				sp.keywords = keywordsString;
			}
			return sp;
		}

		public static DataTable GetExpenditures(SearchParamsExpenditures sp, int pageIndex, int maximumRows, string sortColumn, string sortDirection)
		{
			DataTable results = new DataTable("Results");
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("SearchExpendituresSQL", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@pageIndex", SqlDbType.Int).Value = pageIndex;
					cmd.Parameters.Add("@maximumRows", SqlDbType.Int).Value = maximumRows;
					cmd.Parameters.Add("@sortColumn", SqlDbType.VarChar, 25).Value = sortColumn;
					cmd.Parameters.Add("@sortDirection", SqlDbType.Char, 4).Value = sortDirection;
					cmd.Parameters.Add("@candidateID", SqlDbType.Int).Value = (sp.candidateID == 0) ? System.DBNull.Value : (object)sp.candidateID;
					cmd.Parameters.Add("@office", SqlDbType.NVarChar, 50).Value = (sp.office == null) ? System.DBNull.Value : (object)sp.office;
                    cmd.Parameters.Add("@datePaid", SqlDbType.Int).Value = sp.datePaid;
                    cmd.Parameters.Add("@vendorKeywords", SqlDbType.VarChar, 100).Value = (sp.vendorKeywords == null) ? System.DBNull.Value : (object)sp.vendorKeywords;
					cmd.Parameters.Add("@vendorSearchOptions", SqlDbType.Char, 1).Value = (sp.vendorSearchOptions == null) ? System.DBNull.Value : (object)sp.vendorSearchOptions;
					cmd.Parameters.Add("@keywords", SqlDbType.VarChar, 100).Value = (sp.keywords == null) ? System.DBNull.Value : (object)sp.keywords;

					conn.Open();
					cmd.ExecuteNonQuery();
					SqlDataAdapter da = new SqlDataAdapter(cmd);
					da.Fill(results);

					return results;
				}
			}
		}

		public static int GetExpendituresCount(SearchParamsExpenditures sp)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("SearchExpendituresCount", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@candidateID", SqlDbType.Int).Value = (sp.candidateID == 0) ? System.DBNull.Value : (object)sp.candidateID;
					cmd.Parameters.Add("@office", SqlDbType.NVarChar, 50).Value = (sp.office == null) ? System.DBNull.Value : (object)sp.office;
                    cmd.Parameters.Add("@datePaid", SqlDbType.Int).Value = sp.datePaid;
                    cmd.Parameters.Add("@vendorKeywords", SqlDbType.VarChar, 100).Value = (sp.vendorKeywords == null) ? System.DBNull.Value : (object)sp.vendorKeywords;
					cmd.Parameters.Add("@vendorSearchOptions", SqlDbType.Char, 1).Value = (sp.vendorSearchOptions == null) ? System.DBNull.Value : (object)sp.vendorSearchOptions;
					cmd.Parameters.Add("@keywords", SqlDbType.VarChar, 100).Value = (sp.keywords == null) ? System.DBNull.Value : (object)sp.keywords;

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
	}
}