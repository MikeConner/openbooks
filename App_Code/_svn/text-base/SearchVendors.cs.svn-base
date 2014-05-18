using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace OpenBookPgh
{
	/// <summary>
	/// Summary description for SearchVendors
	/// </summary>
	public class SearchVendors
	{
		public static string GenerateQueryString(int vendorNo)
		{
			string queryString = "~/Vendors.aspx?";

			// Vendor Number (aka vendorID)
			if (vendorNo != 0)
			{
				queryString += "&ID=" + vendorNo.ToString();
			}
			return queryString;
		}
		public static SearchParamsVendor GetQueryStringValues(System.Web.HttpRequest Request)
		{
			SearchParamsVendor sp = new SearchParamsVendor();

			// City Department
			int vendorNo = Utils.GetIntFromQueryString(Request.QueryString["ID"]);
			if (vendorNo != 0)
			{
				sp.vendorID = vendorNo;
			}
			return sp;
		}
		public static DataTable GetVendors(SearchParamsVendor sp, int pageIndex, int maximumRows, string sortColumn, string sortDirection)
		{
			DataTable results = new DataTable("Results");
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("SearchVendorsSQL", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@pageIndex", SqlDbType.Int).Value = pageIndex;
					cmd.Parameters.Add("@maximumRows", SqlDbType.Int).Value = maximumRows;
					cmd.Parameters.Add("@sortColumn", SqlDbType.VarChar, 25).Value = sortColumn;
					cmd.Parameters.Add("@sortDirection", SqlDbType.Char, 4).Value = sortDirection;
					cmd.Parameters.Add("@VendorID", SqlDbType.Int).Value = (sp.vendorID == 0) ? System.DBNull.Value : (object)sp.vendorID;

					conn.Open();
					cmd.ExecuteNonQuery();
					SqlDataAdapter da = new SqlDataAdapter(cmd);
					da.Fill(results);

					return results;
				}
			}
		}

		public static int GetVendorsCount(SearchParamsVendor sp)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("SearchVendorsCount", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@VendorID", SqlDbType.Int).Value = (sp.vendorID == 0) ? System.DBNull.Value : (object)sp.vendorID;

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