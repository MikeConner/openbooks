﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace OpenBookPgh
{
	/// <summary>
	/// Summary description for SearchContracts
	/// </summary>
	public class SearchContracts
	{
		public static string GenerateQueryString(int vendorID, int contractID, string vendorKeywords, string vendorSearchOptions, int cityDept, int contractType, string keywords,
													string month1, int year1, string month2, int year2, int contractAmt)
		{
			string queryString = "~/Contracts.aspx?";

			// Vendor/ContractID Only Search in Admin
			if(vendorID != 0 || contractID != 0)
			{
				if(vendorID !=0)
				{
					queryString += "vendorNo=" + vendorID;
				}
				else
				{
					queryString += "contractNo=" + vendorID;
				}
			
				
			}
			// Regular Query Searches
			else
			{
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
				// City Department
				if (cityDept != 0)
				{
					queryString += "&dept=" + cityDept.ToString();
				}
				// Contract Type
				if (contractType != 0)
				{
					queryString += "&ctype=" + contractType.ToString();
				}

				// Begin & End Date Filter
				if (year1 != 0 && year2 != 0)
				{
					queryString += "&mo1=" + month1 + "&yr1=" + year1 + "&mo2=" + month2 + "&yr2=" + year2;
				}
				// Contract Amount
				if (contractAmt != 0)
				{
					queryString += "&amt=" + contractAmt.ToString();
				}
			}
			return queryString;
		}

		public static SearchParamsContract GetQueryStringValues(System.Web.HttpRequest Request)
		{
			SearchParamsContract sp = new SearchParamsContract();

			// Vendor Only Search in Admin
			int vendorNo = Utils.GetIntFromQueryString(Request.QueryString["vendorNo"]);
			if (vendorNo != 0)
			{
				sp.vendorID = vendorNo;
			}
			// ContractID Only Search in Admin
			int contractNo = Utils.GetIntFromQueryString(Request.QueryString["contractNo"]);
			if (contractNo != 0)
			{
				sp.contractID = contractNo;
			}

			// Vendor Keywords
			string vendorKeywords = Utils.GetStringFromQueryString(Request.QueryString["vendor"], true);
			if (vendorKeywords != "")
			{
				sp.vendorKeywords = vendorKeywords;
			}
			// Vendor Search Options
			string strAllowed = "C B E";
			string vendorSearchOptions = Utils.GetStringFromQueryString(Request.QueryString["vtype"], strAllowed);
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

			// City Department
			int dept = Utils.GetIntFromQueryString(Request.QueryString["dept"]);
			if (dept != 0)
			{
				sp.cityDept = dept;
			}

			// Contract Type
			int contract = Utils.GetIntFromQueryString(Request.QueryString["ctype"]);
			if (contract != 0)
			{
				sp.contractType = contract;
			}

			// Date Filters
			string strAllowedMonths = "01 02 03 04 05 06 07 08 09 10 11 12";
			int yr1 = Utils.GetIntFromQueryString(Request.QueryString["yr1"]);
			int yr2 = Utils.GetIntFromQueryString(Request.QueryString["yr2"]);
			string mo1 = Utils.GetStringFromQueryString(Request.QueryString["mo1"], strAllowedMonths);
			string mo2 = Utils.GetStringFromQueryString(Request.QueryString["mo2"], strAllowedMonths);

			if (yr1 != 0 && yr2 != 0)
			{
				if (mo1 != "" && mo2 != "")
				{
					sp.beginDate = Convert.ToDateTime(mo1 + "/01/" + yr1.ToString());
					sp.endDate = Convert.ToDateTime(mo2 + "/01/" + yr2.ToString());
				}
			}

			// Contract Amount
			int amount = Utils.GetIntFromQueryString(Request.QueryString["amt"]);
			if (amount != 0)
			{
				sp.contractAmt = amount;
			}


			return sp;

		}

		public static DataTable GetContracts(SearchParamsContract sp, int pageIndex, int maximumRows, string sortColumn, string sortDirection)
		{
			DataTable results = new DataTable("Results");
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("SearchContractsSQL", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@pageIndex", SqlDbType.Int).Value = pageIndex;
					cmd.Parameters.Add("@maximumRows", SqlDbType.Int).Value = maximumRows;
					cmd.Parameters.Add("@sortColumn", SqlDbType.VarChar, 25).Value = sortColumn;
					cmd.Parameters.Add("@sortDirection", SqlDbType.Char, 4).Value = sortDirection;
					cmd.Parameters.Add("@contractID", SqlDbType.Int).Value = (sp.contractID == 0) ? System.DBNull.Value : (object)sp.contractID;
					cmd.Parameters.Add("@vendorID", SqlDbType.Int).Value = (sp.vendorID == 0) ? System.DBNull.Value : (object)sp.vendorID;	
					cmd.Parameters.Add("@vendorKeywords", SqlDbType.VarChar, 100).Value = (sp.vendorKeywords == null) ? System.DBNull.Value : (object)sp.vendorKeywords;
					cmd.Parameters.Add("@vendorSearchOptions", SqlDbType.Char, 1).Value = (sp.vendorSearchOptions == null) ? System.DBNull.Value : (object)sp.vendorSearchOptions;
					cmd.Parameters.Add("@cityDept", SqlDbType.Int).Value = (sp.cityDept == 0) ? System.DBNull.Value : (object)sp.cityDept;
					cmd.Parameters.Add("@contractType", SqlDbType.Int).Value = (sp.contractType == 0) ? System.DBNull.Value : (object)sp.contractType;
					cmd.Parameters.Add("@keywords", SqlDbType.VarChar, 100).Value = (sp.keywords == null) ? System.DBNull.Value : (object)sp.keywords;
					cmd.Parameters.Add("@beginDate", SqlDbType.DateTime).Value = sp.beginDate;
					cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = sp.endDate;
					cmd.Parameters.Add("@contractAmt", SqlDbType.Int).Value = (sp.contractAmt == 0) ? System.DBNull.Value : (object)sp.contractAmt;

					conn.Open();
					cmd.ExecuteNonQuery();
					SqlDataAdapter da = new SqlDataAdapter(cmd);
					da.Fill(results);

					return results;
				}
			}
		}

		public static int GetContractsCount(SearchParamsContract sp)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("SearchContractsCount", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@contractID", SqlDbType.Int).Value = (sp.contractID == 0) ? System.DBNull.Value : (object)sp.contractID;
					cmd.Parameters.Add("@vendorID", SqlDbType.Int).Value = (sp.vendorID == 0) ? System.DBNull.Value : (object)sp.vendorID;		
					cmd.Parameters.Add("@vendorKeywords", SqlDbType.VarChar, 100).Value = (sp.vendorKeywords == null) ? System.DBNull.Value : (object)sp.vendorKeywords;
					cmd.Parameters.Add("@vendorSearchOptions", SqlDbType.Char, 1).Value = (sp.vendorSearchOptions == null) ? System.DBNull.Value : (object)sp.vendorSearchOptions;
					cmd.Parameters.Add("@cityDept", SqlDbType.Int).Value = (sp.cityDept == 0) ? System.DBNull.Value : (object)sp.cityDept;
					cmd.Parameters.Add("@contractType", SqlDbType.Int).Value = (sp.contractType == 0) ? System.DBNull.Value : (object)sp.contractType;
					cmd.Parameters.Add("@keywords", SqlDbType.VarChar, 100).Value = (sp.keywords == null) ? System.DBNull.Value : (object)sp.keywords;
					cmd.Parameters.Add("@beginDate", SqlDbType.DateTime).Value = sp.beginDate;
					cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = sp.endDate;
					cmd.Parameters.Add("@contractAmt", SqlDbType.Int).Value = (sp.contractAmt == 0) ? System.DBNull.Value : (object)sp.contractAmt;

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
