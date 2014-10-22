using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace OpenBookPgh
{
	/// <summary>
	/// Summary description for Admin
	/// </summary>
	public class Admin
	{
		public static int GetNextContractID()
		{
			int Result = 0;
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("GetNextContractID", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@NextContractID", SqlDbType.Int).Direction = ParameterDirection.Output;
					conn.Open();
					cmd.ExecuteNonQuery();
					Result = (int)cmd.Parameters["@NextContractID"].Value;
				}
			}
			return Result;
		}

		public static int AddContract(int contractNo, string vendorNo, int departmentID, int supplementalNo, string resolutionNo, int service,
										decimal amount, decimal originalAmount,	string description,
										DateTime? dateDuration, DateTime? dateApproval, DateTime? dateEntered)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("AddContract", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
					cmd.Parameters.Add("@ContractNo", SqlDbType.Int).Value = contractNo;
					cmd.Parameters.Add("@VendorNo", SqlDbType.NVarChar, 10).Value = vendorNo;
					cmd.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = departmentID;
					cmd.Parameters.Add("@SupplementalNo", SqlDbType.Int).Value = supplementalNo;
					cmd.Parameters.Add("@ResolutionNo", SqlDbType.NVarChar, 40).Value = resolutionNo;
					cmd.Parameters.Add("@Service", SqlDbType.Int).Value = service;
					cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;
					cmd.Parameters.Add("@OriginalAmount", SqlDbType.Decimal).Value = originalAmount;
					cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 100).Value = description;
					cmd.Parameters.Add("@DateDuration", SqlDbType.DateTime).Value = dateDuration;
					cmd.Parameters.Add("@DateCountersigned", SqlDbType.DateTime).Value = dateApproval;
					cmd.Parameters.Add("@DateEntered", SqlDbType.DateTime).Value = dateEntered;
					
					conn.Open();
					cmd.ExecuteNonQuery();
					int result = (int)cmd.Parameters["@RETURN_VALUE"].Value;
					return result;
				}
			}
		}

		public static int AddContribution(int candidateID, string office, string contributorType, string contributorName, int contributionType, 
										string description, string city, string state, string zip, string employer, string occupation, 
										decimal amount, string createdBy, DateTime? dateContribution)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("AddContribution", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
					cmd.Parameters.Add("@CandidateID", SqlDbType.Int).Value = candidateID;
					cmd.Parameters.Add("@Office", SqlDbType.NVarChar, 50).Value = office;
					cmd.Parameters.Add("@ContributorType", SqlDbType.Char, 2).Value = contributorType;
					cmd.Parameters.Add("@ContributorName", SqlDbType.NVarChar, 100).Value = contributorName;
					cmd.Parameters.Add("@ContributionType", SqlDbType.Int).Value = contributionType;
					cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 255).Value = description;					
					cmd.Parameters.Add("@City", SqlDbType.NVarChar, 50).Value = city;
					cmd.Parameters.Add("@State", SqlDbType.NVarChar, 4).Value = state;
					cmd.Parameters.Add("@Zip", SqlDbType.NVarChar, 15).Value = zip;
					cmd.Parameters.Add("@Employer", SqlDbType.NVarChar, 100).Value = employer;
					cmd.Parameters.Add("@Occupation", SqlDbType.NVarChar, 100).Value = occupation;
					cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 50).Value = createdBy;
                    cmd.Parameters.Add("@DateContribution", SqlDbType.DateTime).Value = dateContribution;

					conn.Open();
					cmd.ExecuteNonQuery();
					int result = (int)cmd.Parameters["@RETURN_VALUE"].Value;
					return result;
				}
			}
		}
		
		public static int AddExpenditure(int candidateID, string office, string company,
								string address, string city, string state, string zip, string description,
								decimal amount, string createdBy, DateTime? dateExpenditure)
								
						
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("AddExpenditure", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
					cmd.Parameters.Add("@CandidateID", SqlDbType.Int).Value = candidateID;
					cmd.Parameters.Add("@Office", SqlDbType.NVarChar, 50).Value = office;
					cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 100).Value = company;
					cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 100).Value = address;
					cmd.Parameters.Add("@City", SqlDbType.NVarChar, 50).Value = city;
					cmd.Parameters.Add("@State", SqlDbType.NVarChar, 4).Value = state;
					cmd.Parameters.Add("@Zip", SqlDbType.NVarChar, 15).Value = zip;
					cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 100).Value = description;
					cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;
					cmd.Parameters.Add("@DatePaid", SqlDbType.DateTime).Value = dateExpenditure;
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 50).Value = createdBy;

					conn.Open();
					cmd.ExecuteNonQuery();
					int result = (int)cmd.Parameters["@RETURN_VALUE"].Value;
					return result;
				}
			}
		}
		
		public static int AddCandidate(string candidate)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("AddCandidate", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
					cmd.Parameters.Add("@CandidateName", SqlDbType.NVarChar, 50).Value = candidate;

					conn.Open();
					cmd.ExecuteNonQuery();
					int result = (int)cmd.Parameters["@RETURN_VALUE"].Value;
					return result;
				}
			}
		}

		public static string AddVendor(string vendor, string vendorNo, string address1, string address2, string address3, string city, string state, string zip)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("AddVendor", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@message", SqlDbType.NVarChar, 35).Direction = ParameterDirection.Output;
					//cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

					cmd.Parameters.Add("@vendorName", SqlDbType.NVarChar, 100).Value = vendor;
					cmd.Parameters.Add("@vendorNo", SqlDbType.NVarChar, 10).Value = vendorNo;
					cmd.Parameters.Add("@address1", SqlDbType.NVarChar, 100).Value = address1;
					cmd.Parameters.Add("@address2", SqlDbType.NVarChar, 100).Value = address2;
					cmd.Parameters.Add("@address3", SqlDbType.NVarChar, 100).Value = address3;
					cmd.Parameters.Add("@city", SqlDbType.NVarChar, 50).Value = city;
					cmd.Parameters.Add("@state", SqlDbType.NVarChar, 4).Value = state;
					cmd.Parameters.Add("@zip", SqlDbType.NVarChar, 15).Value = zip;

					conn.Open();
					cmd.ExecuteNonQuery();
					//int result = (int)cmd.Parameters["@RETURN_VALUE"].Value;
					string result = cmd.Parameters["@message"].Value.ToString();
					return result;
				}
			}
		}

		public static int AddLobbyist(string lobbyist, string position, string employer, string address, string city, string state, string zip, string lobbyiststatus)
		{
			int lobbyistID = 0;
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("AddLobbyist", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@LobbyistID", SqlDbType.Int).Direction = ParameterDirection.Output;
					cmd.Parameters.Add("@Lobbyist", SqlDbType.NVarChar, 100).Value = lobbyist;
					cmd.Parameters.Add("@Position", SqlDbType.NVarChar, 50).Value = position;
					cmd.Parameters.Add("@Employer", SqlDbType.NVarChar, 100).Value = employer;
					cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 100).Value = address;
					cmd.Parameters.Add("@City", SqlDbType.NVarChar, 50).Value = city;
					cmd.Parameters.Add("@State", SqlDbType.NVarChar, 4).Value = state;
					cmd.Parameters.Add("@Zip", SqlDbType.NVarChar, 15).Value = zip;
					cmd.Parameters.Add("@LobbyistStatus", SqlDbType.NVarChar, 50).Value = lobbyiststatus;
					conn.Open();
					cmd.ExecuteNonQuery();
					lobbyistID = (int)cmd.Parameters["@LobbyistID"].Value;
				}
			}
			return lobbyistID;
		}

		public static void AddLobbyistCompany(int lobbyistID, string company, string address, string city, string state, string zip)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("AddLobbyistCompany", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@LobbyistID", SqlDbType.Int).Value = lobbyistID;
					cmd.Parameters.Add("@Company", SqlDbType.NVarChar, 100).Value = company;
					cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 100).Value = address;
					cmd.Parameters.Add("@City", SqlDbType.NVarChar, 50).Value = city;
					cmd.Parameters.Add("@State", SqlDbType.NVarChar, 4).Value = state;
					cmd.Parameters.Add("@Zip", SqlDbType.NVarChar, 15).Value = zip;
					conn.Open();
					cmd.ExecuteNonQuery();
				}
			}
		}


		public static int UpdateContract(int contractID, string vendorNo, int departmentID, int supplementalNo, int newSupplementalNo, string resolutionNo, int service,
								decimal amount, decimal originalAmount, string description,
								DateTime? dateDuration, DateTime? dateApproval)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("UpdateContract", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
					cmd.Parameters.Add("@ContractID", SqlDbType.Int).Value = contractID;
					cmd.Parameters.Add("@VendorNo", SqlDbType.NVarChar, 10).Value = vendorNo;
					cmd.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = departmentID;
					cmd.Parameters.Add("@SupplementalNo", SqlDbType.Int).Value = supplementalNo;
					cmd.Parameters.Add("@NewSupplementalNo", SqlDbType.Int).Value = newSupplementalNo;
					cmd.Parameters.Add("@ResolutionNo", SqlDbType.NVarChar, 40).Value = resolutionNo;
					cmd.Parameters.Add("@Service", SqlDbType.Int).Value = service;
					cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;
					cmd.Parameters.Add("@OriginalAmount", SqlDbType.Decimal).Value = originalAmount;
					cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 100).Value = description;
					cmd.Parameters.Add("@DateDuration", SqlDbType.DateTime).Value = dateDuration;
					cmd.Parameters.Add("@DateCountersigned", SqlDbType.DateTime).Value = dateApproval;

					conn.Open();
					cmd.ExecuteNonQuery();
					int result = (int)cmd.Parameters["@RETURN_VALUE"].Value;
					return result;
				}
			}
		}

		public static int UpdateContribution(int contributionID, int candidateID, string office, string contributorType, string contributorName,
								int contributionType, string description, string city, string state, string zip, string employer, string occupation,
								decimal amount, DateTime? dateContribution)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("UpdateContribution", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

					cmd.Parameters.Add("@ContributionID", SqlDbType.Int).Value = (contributionID == 0) ? System.DBNull.Value : (object)contributionID;
					cmd.Parameters.Add("@CandidateID", SqlDbType.Int).Value = candidateID;
					cmd.Parameters.Add("@Office", SqlDbType.NVarChar, 50).Value = office;
					cmd.Parameters.Add("@ContributorType", SqlDbType.Char, 2).Value = contributorType;
					cmd.Parameters.Add("@ContributionType", SqlDbType.Int).Value = contributionType;
					cmd.Parameters.Add("@ContributorName", SqlDbType.NVarChar, 100).Value = contributorName;
					cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 255).Value = description;	
					cmd.Parameters.Add("@City", SqlDbType.NVarChar, 50).Value = city;
					cmd.Parameters.Add("@State", SqlDbType.NVarChar, 4).Value = state;
					cmd.Parameters.Add("@Zip", SqlDbType.NVarChar, 15).Value = zip;
					cmd.Parameters.Add("@Employer", SqlDbType.NVarChar, 100).Value = employer;
					cmd.Parameters.Add("@Occupation", SqlDbType.NVarChar, 100).Value = occupation;
					cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;
					cmd.Parameters.Add("@DateContribution", SqlDbType.DateTime).Value = dateContribution;

					conn.Open();
					cmd.ExecuteNonQuery();
					int result = (int)cmd.Parameters["@RETURN_VALUE"].Value;
					return result;
				}
			}
		}

		public static int UpdateExpenditure(int expenditureID, int candidateID, string office, string company,
								string address, string city, string state, string zip, string description,
								decimal amount, DateTime? dateExpenditure)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("UpdateExpenditure", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
					cmd.Parameters.Add("@ExpenditureID", SqlDbType.Int).Value = (expenditureID == 0) ? System.DBNull.Value : (object)expenditureID;
					cmd.Parameters.Add("@CandidateID", SqlDbType.Int).Value = candidateID;
					cmd.Parameters.Add("@Office", SqlDbType.NVarChar, 50).Value = office;
					cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 100).Value = company;
					cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 100).Value = address;
					cmd.Parameters.Add("@City", SqlDbType.NVarChar, 50).Value = city;
					cmd.Parameters.Add("@State", SqlDbType.NVarChar, 4).Value = state;
					cmd.Parameters.Add("@Zip", SqlDbType.NVarChar, 15).Value = zip;
					cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 100).Value = description;
					cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;
					cmd.Parameters.Add("@DatePaid", SqlDbType.DateTime).Value = dateExpenditure;

					conn.Open();
					cmd.ExecuteNonQuery();
					int result = (int)cmd.Parameters["@RETURN_VALUE"].Value;
					return result;
				}
			}
		}

		public static int UpdateVendor(string querystringVendorNo, string vendorNo, string vendorName,
											string address1, string address2, string address3, string city, string state, string zip)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("UpdateVendor", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
					cmd.Parameters.Add("@QuerystringVendorNo", SqlDbType.NVarChar, 10).Value = (querystringVendorNo == null) ? System.DBNull.Value : (object)querystringVendorNo;
					cmd.Parameters.Add("@VendorNo", SqlDbType.NVarChar, 10).Value = (vendorNo == null) ? System.DBNull.Value : (object)vendorNo;
					cmd.Parameters.Add("@VendorName", SqlDbType.NVarChar, 100).Value = (vendorName == null) ? System.DBNull.Value : (object)vendorName;
					cmd.Parameters.Add("@Address1", SqlDbType.NVarChar, 100).Value = (address1 == null) ? System.DBNull.Value : (object)address1;
					cmd.Parameters.Add("@Address2", SqlDbType.NVarChar, 100).Value = (address2 == null) ? System.DBNull.Value : (object)address2;
					cmd.Parameters.Add("@Address3", SqlDbType.NVarChar, 100).Value = (address3 == null) ? System.DBNull.Value : (object)address3;
					cmd.Parameters.Add("@City", SqlDbType.NVarChar, 50).Value = (city == null) ? System.DBNull.Value : (object)city;
					cmd.Parameters.Add("@State", SqlDbType.NVarChar, 4).Value = (state == null) ? System.DBNull.Value : (object)state;
					cmd.Parameters.Add("@Zip", SqlDbType.NVarChar, 15).Value = (zip == null) ? System.DBNull.Value : (object)zip;

					conn.Open();
					cmd.ExecuteNonQuery();
					int result = (int)cmd.Parameters["@RETURN_VALUE"].Value;
					return result;
				}
			}
		}

		public static void UpdateLobbyist(int lobbyistID, string lobbyist, string position, string employer, string address, string city, string state, string zip, string lobbyiststatus)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("UpdateLobbyist", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@LobbyistID", SqlDbType.Int).Value = lobbyistID;
					cmd.Parameters.Add("@Lobbyist", SqlDbType.NVarChar, 100).Value = lobbyist;
					cmd.Parameters.Add("@Position", SqlDbType.NVarChar, 50).Value = position;
					cmd.Parameters.Add("@Employer", SqlDbType.NVarChar, 100).Value = employer;
					cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 100).Value = address;
					cmd.Parameters.Add("@City", SqlDbType.NVarChar, 50).Value = city;
					cmd.Parameters.Add("@State", SqlDbType.NVarChar, 4).Value = state;
					cmd.Parameters.Add("@Zip", SqlDbType.NVarChar, 15).Value = zip;
					cmd.Parameters.Add("@LobbyistStatus", SqlDbType.NVarChar, 50).Value = lobbyiststatus;
					conn.Open();
					cmd.ExecuteNonQuery();
				}
			}
		}

		public static DataTable GetLobbyist(int lobbyistID)
		{
			DataTable results = new DataTable("Results");
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("GetLobbyist", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@LobbyistID", SqlDbType.Int).Value = lobbyistID;

					conn.Open();
					cmd.ExecuteNonQuery();
					SqlDataAdapter da = new SqlDataAdapter(cmd);
					da.Fill(results);
				}
			}
			return results;
		}

		public static DataTable GetLobbyistCompanies(int lobbyistID)
		{
			DataTable results = new DataTable("Results");
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("GetLobbyistCompanies", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@LobbyistID", SqlDbType.Int).Value = lobbyistID;

					conn.Open();
					cmd.ExecuteNonQuery();
					SqlDataAdapter da = new SqlDataAdapter(cmd);
					da.Fill(results);
				}
			}
			return results;
		}

		public static DataTable GetContract(int contractID, int supplementalNo)
		{
			DataTable results = new DataTable("Results");
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("GetContract", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@ContractID", SqlDbType.Int).Value = contractID;
					cmd.Parameters.Add("@SupplementalNo", SqlDbType.Int).Value = supplementalNo;
					conn.Open();
					cmd.ExecuteNonQuery();
					SqlDataAdapter da = new SqlDataAdapter(cmd);
					da.Fill(results);

					return results;
				}
			}
		}

		public static DataTable GetContractByContractID(int contractID, int supplementalNo)
		{
			DataTable contracts = new DataTable("contracts");

			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("GetContractByContractID", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@ContractID", SqlDbType.Int).Value = contractID;
					cmd.Parameters.Add("@SupplementalNo", SqlDbType.Int).Value = supplementalNo;
					conn.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						contracts.Load(reader);
						return contracts;
					}
				}
			}
		}

		public static DataTable GetContractsByVendorID(int vendorID)
		{
			DataTable contracts = new DataTable("contracts");

			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("SELECT * FROM contracts WHERE VendorNo=" + vendorID, conn))
				{
					cmd.CommandType = CommandType.Text;
					conn.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						contracts.Load(reader);
						return contracts;
					}
				}
			}
		}

		public static DataTable GetContribution(int contributionID)
		{
			DataTable results = new DataTable("Results");
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("GetContribution", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@contributionID", SqlDbType.Int).Value = (contributionID == 0) ? System.DBNull.Value : (object)contributionID;

					conn.Open();
					cmd.ExecuteNonQuery();
					SqlDataAdapter da = new SqlDataAdapter(cmd);
					da.Fill(results);

					return results;
				}
			}
		}

		public static DataTable GetExpenditure(int expenditureID)
		{
			DataTable results = new DataTable("Results");
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("GetExpenditure", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@ExpenditureID", SqlDbType.Int).Value = (expenditureID == 0) ? System.DBNull.Value : (object)expenditureID;

					conn.Open();
					cmd.ExecuteNonQuery();
					SqlDataAdapter da = new SqlDataAdapter(cmd);
					da.Fill(results);

					return results;
				}
			}
		}

		public static DataTable GetVendor(string vendorNo)
		{
			DataTable results = new DataTable("Results");
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("GetVendor", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
					cmd.Parameters.Add("@VendorNo", SqlDbType.NVarChar, 10).Value = (vendorNo == null) ? System.DBNull.Value : (object)vendorNo;
					conn.Open();
					SqlDataAdapter da = new SqlDataAdapter(cmd);
					da.Fill(results);

					return results;
				}
			}
		}

		public static DataTable GetCompanyByVendorID(int vendorID)
		{
			DataTable company = new DataTable("company");

			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("SELECT * FROM vendors WHERE VendorNo=" + vendorID, conn))
				{
					cmd.CommandType = CommandType.Text;
					conn.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						company.Load(reader);
						return company;
					}
				}
			}
		}




		public static void DeleteContract(int contractID, int supplementalNo)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("DeleteContract", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@ContractID", SqlDbType.Int).Value = contractID;
					cmd.Parameters.Add("@SupplementalNo", SqlDbType.Int).Value = supplementalNo;

					conn.Open();
					cmd.ExecuteNonQuery();
				}
			}
		}

        public static void ApproveContribution(int contributionID, string username)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ApproveContribution", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ContributionID", SqlDbType.Int).Value = contributionID;
                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = username;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

		public static void DeleteContribution(int contributionID)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("DeleteContribution", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@contributionID", SqlDbType.Int).Value = (contributionID == 0) ? System.DBNull.Value : (object)contributionID;

					conn.Open();
					cmd.ExecuteNonQuery();
				}
			}
		}

		public static void DeleteExpenditure(int expenditureID)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("DeleteExpenditure", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@ExpenditureID", SqlDbType.Int).Value = expenditureID;
					conn.Open();
					cmd.ExecuteNonQuery();
				}
			}
		}

		public static void DeleteLobbyist(int lobbyistID)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("DeleteLobbyist", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@LobbyistID", SqlDbType.Int).Value = lobbyistID;
					conn.Open();
					cmd.ExecuteNonQuery();
				}
			}
		}

		public static void DeleteUser(int userID)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("DeleteUser", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;

					conn.Open();
					cmd.ExecuteNonQuery();
				}
			}
		}

		public static DataTable LoadVendors()
		{
			DataTable results = new DataTable("Results");
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("SELECT * FROM tlk_vendors ORDER BY VendorName", conn))
				{
					cmd.CommandType = CommandType.Text;
					conn.Open();
					cmd.ExecuteNonQuery();
					SqlDataAdapter da = new SqlDataAdapter(cmd);
					da.Fill(results);
					return results;
				}
			}
		}

		public static DataTable LoadUsers()
		{
			DataTable results = new DataTable("Results");
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("SELECT * FROM users ORDER BY username", conn))
				{
					cmd.CommandType = CommandType.Text;
					conn.Open();
					cmd.ExecuteNonQuery();
					SqlDataAdapter da = new SqlDataAdapter(cmd);
					da.Fill(results);
					return results;
				}
			}
		}

		public static DataTable LoadContractNos()
		{
			DataTable results = new DataTable("Results");
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT ContractID FROM contracts ORDER BY ContractID DESC", conn))
				{
					cmd.CommandType = CommandType.Text;
					conn.Open();
					cmd.ExecuteNonQuery();
					SqlDataAdapter da = new SqlDataAdapter(cmd);
					da.Fill(results);
					return results;
				}
			}
		}

		public static DataTable GetVendorAddress(string vendorID)
		{
			DataTable table = new DataTable();
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("GetVendor", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@VendorNo", SqlDbType.NVarChar, 10).Value = vendorID;
					conn.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						table.Load(reader);
						return table;
					}
				}
			}
		}
	}
}
