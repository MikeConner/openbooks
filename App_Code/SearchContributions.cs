using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using OpenBookAllegheny.ZipCodeDownload.Wizards;

namespace OpenBookAllegheny
{
	/// <summary>
	/// Summary description for SearchContributions
	/// </summary>
	public class SearchContributions
	{
		
		public static string GenerateQueryString(int candidateID, string office, int year1, string contributorKeywords, string contributorSearchOptions, 
													string employerKeywords, string zip, double distance)
		{
			string queryString = "~/SearchContributions.aspx?";

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
			// Contribution Date
			if (year1 != 0)
			{
				queryString += "&yr1=" + year1;
			}

			// Contributor Keywords
			if (!string.IsNullOrEmpty(contributorKeywords) && !string.IsNullOrEmpty(contributorSearchOptions))
			{
				queryString += "&contributor=" + System.Web.HttpUtility.UrlEncode(contributorKeywords) + "&ctype=" + contributorSearchOptions;
			}
			// Employer Keywords
			if (!string.IsNullOrEmpty(employerKeywords))
			{
				queryString += "&employer=" + System.Web.HttpUtility.UrlEncode(employerKeywords);
			}

			// Distance
			if (string.IsNullOrEmpty(zip) == false && distance != 0)
			{
				queryString += "&zip=" + zip + "&distance=" + distance;
			}
			return queryString;
		}

		public static SearchParamsContribution GetQueryStringValues(System.Web.HttpRequest Request)
		{
			SearchParamsContribution sp = new SearchParamsContribution();

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

			//Contribution Year
			int yr1 = Utils.GetIntFromQueryString(Request.QueryString["yr1"]);
			if (yr1 != 0)
			{
				sp.dateContribution = yr1;
			}

			// Contributor Keywords
			string contributor = Utils.GetStringFromQueryString(Request.QueryString["contributor"], true);
			if (contributor != "")
			{
				sp.contributorKeywords = contributor;
			}
			// Contributor Search Options
			string strAllowedOptions = "co in";
			string contributorSearchOptions = Utils.GetStringFromQueryString(Request.QueryString["ctype"], strAllowedOptions);
			if (contributorSearchOptions != "")
			{
				sp.contributorSearchOptions = contributorSearchOptions;
			}
			// Employer Keywords
			string employer = Utils.GetStringFromQueryString(Request.QueryString["employer"], true);
			if (employer != "")
			{
				sp.employerKeywords = employer;
			}

			// Distance
			string zip = Utils.GetStringFromQueryString(Request.QueryString["zip"], true);
			string distanceString = Utils.GetStringFromQueryString(Request.QueryString["distance"], true);

			if (zip != "" && distanceString != "")
			{
				bool zipValid = Utils.IsValidZip(zip);
				if (zipValid)
				{
					sp.zip = zip;
					double distance = Convert.ToDouble(distanceString);
					if (distance == 50 || distance == 30 || distance == 20 || distance == 10 || distance == 5)
					{
						sp.radius = distance;
					}
				}
			}
			return sp;
		}

        public static DataTable GetContributions(SearchParamsContribution sp, int pageIndex, int maximumRows, string sortColumn, string sortDirection)
        {
            return GetContributions(sp, pageIndex, maximumRows, sortColumn, sortDirection, true);
        }

		public static DataTable GetContributions(SearchParamsContribution sp, int pageIndex, int maximumRows, string sortColumn, string sortDirection, Boolean approved)
		{
			//Query Distance?
			bool QueryDistance = false;
			if(sp.zip != null && (sp.radius != 0))
			{
				QueryDistance = true;
			}
			DataTable results = new DataTable("Results");			
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("SearchContributionsSQL", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@pageIndex", SqlDbType.Int).Value = pageIndex;
					cmd.Parameters.Add("@maximumRows", SqlDbType.Int).Value = maximumRows;
					cmd.Parameters.Add("@sortColumn", SqlDbType.VarChar, 25).Value = sortColumn;
					cmd.Parameters.Add("@sortDirection", SqlDbType.Char, 4).Value = sortDirection;
					cmd.Parameters.Add("@candidateID", SqlDbType.Int).Value = (sp.candidateID == 0) ? System.DBNull.Value : (object)sp.candidateID;
					cmd.Parameters.Add("@office", SqlDbType.NVarChar, 50).Value = (sp.office == null) ? System.DBNull.Value : (object)sp.office;
					cmd.Parameters.Add("@dateContribution", SqlDbType.Int).Value = sp.dateContribution;
					cmd.Parameters.Add("@contributorKeywords", SqlDbType.NVarChar, 100).Value = (sp.contributorKeywords == null) ? System.DBNull.Value : (object)sp.contributorKeywords;
					cmd.Parameters.Add("@contributorSearchOptions", SqlDbType.Char, 2).Value = (sp.contributorSearchOptions == null) ? System.DBNull.Value : (object)sp.contributorSearchOptions;
					cmd.Parameters.Add("@employerKeywords", SqlDbType.NVarChar, 100).Value = (sp.employerKeywords == null) ? System.DBNull.Value : (object)sp.employerKeywords;
                    cmd.Parameters.Add("@approved", SqlDbType.Bit).Value = approved;

                    if(QueryDistance)
					{
						// Calcualte Origin from zip
						Coordinate originCoord = SearchContributions.CalculateZipCodeOrigin(sp.zip);
						// Calculate distances for jobs
						BoundaryWizard boundCalc = new BoundaryWizard();
						Boundary bounds = boundCalc.CalculateBoundary(originCoord, sp.radius, Measurement.Miles);
						cmd.Parameters.Add("@searchZip", SqlDbType.Bit).Value = 1;
						cmd.Parameters.Add("@boundsSouth", SqlDbType.Decimal).Value = bounds.South;
						cmd.Parameters.Add("@boundsNorth", SqlDbType.Decimal).Value = bounds.North;
						cmd.Parameters.Add("@boundsWest", SqlDbType.Decimal).Value = bounds.West;
						cmd.Parameters.Add("@boundsEast", SqlDbType.Decimal).Value = bounds.East;
					}
					conn.Open();
					cmd.ExecuteNonQuery();
					SqlDataAdapter da = new SqlDataAdapter(cmd);
					da.Fill(results);

					// Add in Distance Column whether or not used to satisfy page
					DataColumn dc = new DataColumn("distance", System.Type.GetType("System.String"));
					dc.AllowDBNull = false;
					dc.ReadOnly = false;
					results.Columns.Add(dc);
					if (QueryDistance)
					{
						// Calcualte Origin from zip
						Coordinate originCoord = SearchContributions.CalculateZipCodeOrigin(sp.zip);
						// Calculate distnace and Add to DataTable
						results = SearchContributions.AddDistanceToResults(results, originCoord, Measurement.Miles);
					}
					return results;
				}
			}
		}

        public static int GetContributionsCount(SearchParamsContribution sp)
        {
            return GetContributionsCount(sp, true);
        }

		public static int GetContributionsCount(SearchParamsContribution sp, Boolean approved)
		{
			// Query Distance ?
			bool QueryDistance = false;
			if (sp.zip != null)
			{
				QueryDistance = true;
			}
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("SearchContributionsCount", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@candidateID", SqlDbType.Int).Value = (sp.candidateID == 0) ? System.DBNull.Value : (object)sp.candidateID;
					cmd.Parameters.Add("@office", SqlDbType.NVarChar, 50).Value = (sp.office == null) ? System.DBNull.Value : (object)sp.office;
					cmd.Parameters.Add("@dateContribution", SqlDbType.Int).Value = sp.dateContribution;
					cmd.Parameters.Add("@contributorKeywords", SqlDbType.NVarChar, 100).Value = (sp.contributorKeywords == null) ? System.DBNull.Value : (object)sp.contributorKeywords;
					cmd.Parameters.Add("@contributorSearchOptions", SqlDbType.Char, 2).Value = (sp.contributorSearchOptions == null) ? System.DBNull.Value : (object)sp.contributorSearchOptions;
					cmd.Parameters.Add("@employerKeywords", SqlDbType.NVarChar, 100).Value = (sp.employerKeywords == null) ? System.DBNull.Value : (object)sp.employerKeywords;
                    cmd.Parameters.Add("@approved", SqlDbType.Bit).Value = approved;
                    
                    if (QueryDistance)
					{
						// Calcualte Origin from zip
						Coordinate originCoord = SearchContributions.CalculateZipCodeOrigin(sp.zip);
						// Calculate distances for jobs
						BoundaryWizard boundCalc = new BoundaryWizard();
						Boundary bounds = boundCalc.CalculateBoundary(originCoord, sp.radius, Measurement.Miles);
						cmd.Parameters.Add("@searchZip", SqlDbType.Bit).Value = 1;
						cmd.Parameters.Add("@boundsSouth", SqlDbType.Decimal).Value = bounds.South;
						cmd.Parameters.Add("@boundsNorth", SqlDbType.Decimal).Value = bounds.North;
						cmd.Parameters.Add("@boundsWest", SqlDbType.Decimal).Value = bounds.West;
						cmd.Parameters.Add("@boundsEast", SqlDbType.Decimal).Value = bounds.East;
					}
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

		/// <summary>
		/// Uses zipcode database to calculate the origin based on Latitude & Longitude
		/// </summary>
		/// <param name="zip">Zip</param>
		/// <returns>Orgin as a Coordinate</returns>
		public static Coordinate CalculateZipCodeOrigin(string zip)
		{
			Coordinate originCoord = null;

			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				conn.Open();
				using (SqlCommand cmd = new SqlCommand("SELECT Latitude, Longitude FROM zipcodes WHERE ZIPCode = '" + zip + "' AND CityType = 'D'", conn))
				{
					using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
					{
						if (reader.Read())
						{
							originCoord = new Coordinate(Convert.ToDouble(reader["Latitude"]), Convert.ToDouble(reader["Longitude"]));
						}
					}
				}
			}
			return originCoord;
		}

		/// <summary>
		/// Adds distance rows to DataTable using zip code library distance calculations
		/// </summary>
		/// <param name="dt">DataTable dt</param>
		/// <param name="originCoord">Origin as a Coordinate</param>
		/// <param name="unit">Measurement unit</param>
		/// <returns>Distance column for DataTable</returns>
		public static DataTable AddDistanceToResults(DataTable dt, Coordinate originCoord, Measurement unit)
		{
			DistanceWizard distCalc = new DistanceWizard();

			foreach (DataRow row in dt.Rows)
			{
				Coordinate relativeCoord = new Coordinate(
					Convert.ToDouble(row["Latitude"]), Convert.ToDouble(row["Longitude"])
				);
				double distanceValue = distCalc.CalculateDistance(originCoord, relativeCoord, unit);

				row["distance"] = Math.Round(distanceValue, 2);
				row.AcceptChanges();
			}
			dt.AcceptChanges();

			return dt;
		}
	}
}
