using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.ComponentModel;
using System.IO;
using System.Collections.Generic;
using System.Net.Mail;
using System.Globalization;

namespace OpenBookAllegheny
{
    /// <summary>
    /// Summary description for Admin
    /// </summary>
    public class Admin
    {
        public static string ONBASE_CONTRACT_PDF_PATH = "http://onbaseapp.city.pittsburgh.pa.us/OpenBookPublicData/contracts.csv";
        public static string ONBASE_CHECK_PDF_PATH = "http://onbaseapp.city.pittsburgh.pa.us/OpenBookPublicData/checks.csv";
        public static string ONBASE_INVOICE_PDF_PATH = "http://onbaseapp.city.pittsburgh.pa.us/OpenBookPublicData/invoices.csv";

        public static void UploadAlleghenyContracts(string filename)
        {
            DataTable table = CSVParser.ParseCSV(System.AppDomain.CurrentDomain.BaseDirectory + "documents\\" + filename);
            foreach (DataRow row in table.Rows)
            {
            }
        }

        public static List<string> UploadContributions(string filename, string username, int candidateID, string office)
        {
            List<string> errors = new List<string>();

            DataTable table = CSVParser.ParseCSV(filename);
            int idx = -1;

            foreach (DataRow row in table.Rows)
            {
                if (-1 == idx)
                {
                    // Skip header
                    idx++;
                    continue;
                }
                idx++;
                // There are validation rules we don't want to duplicate, so call existing Admin function to add the contribution, as if they're being entered one by one
                // Need to convert Candidate and Contribution Type to integers, amount to a decimal, and date to a datetime
                // Conversions
                int contributionTypeID;
                decimal amount;
                DateTime contributionDate;

                if (mContributionType.TryGetValue(row[2].ToString(), out contributionTypeID))
                {
                    if (Decimal.TryParse(row[8].ToString(), NumberStyles.Currency, CultureInfo.CurrentCulture, out amount))
                    {
                        if (DateTime.TryParse(row[9].ToString(), out contributionDate))
                        {
                            int result = AddContribution(candidateID, office, row[0].ToString(), row[1].ToString(), contributionTypeID, 
                                                         row[10].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(),
                                                         row[6].ToString(), row[7].ToString(), amount, username, contributionDate);

                            if (result != 0)
                            {
                                errors.Add("Error in row " + idx + ". Error Code: [" + result + "]");
                            }
                        }
                        else
                        {
                            errors.Add(row + ": " + row[9].ToString() + " is not a valid date.");
                        }
                    }
                    else
                    {
                        errors.Add(row + ": " + row[8].ToString() + " is not a valid dollar amount.");
                    }
                }
                else
                {
                    errors.Add(row + ": " + row[2].ToString() + " is not a recognized contribution type.");
                }
            }

            return errors;
        }

        public static List<string> UploadExpenditures(string filename, string username, int candidateID, string office)
        {
            List<string> errors = new List<string>();
            DataTable table = CSVParser.ParseCSV(filename);
            int idx = -1;

            foreach (DataRow row in table.Rows)
            {
                if (-1 == idx)
                {
                    // Skip header
                    idx++;
                    continue;
                }
                idx++;
                // Conversions
                decimal amount;
                DateTime expenditureDate;

                if (Decimal.TryParse(row[6].ToString(), NumberStyles.Currency, CultureInfo.CurrentCulture, out amount))
                {
                    if (DateTime.TryParse(row[7].ToString(), out expenditureDate))
                    {
                        int result = AddExpenditure(candidateID, office, row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[8].ToString(),
                                                    amount, username, expenditureDate);

                        if (result != 0)
                        {
                            errors.Add("Error in row " + idx + ". Error Code: [" + result + "] ");
                        }
                    }
                    else
                    {
                        errors.Add(row + ": " + row[6].ToString() + " is not a valid date.");
                    }
                }
                else
                {
                    errors.Add(row + ": " + row[5].ToString() + " is not a valid dollar amount.");
                }
            }


            return errors;
        }

        public static void DownloadChecks()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                conn.Open();

                // This stored procedure takes a table-valued parameter containing the list of ContractsIDs (i.e., a 1-column table with a row for each Contract)
                using (SqlCommand cmd = new SqlCommand("SetCheckFlags", conn))
                {
                    // Create the 1-column table with string type. Previously it was "float" type, which removed all the contracts with dashes and letters
                    //   These alphanumeric contract numbers work when sent to OnBase though, so we shouldn't be deleting them!
                    //   Change everything to a string to handle it. 
                    DataTable table = new DataTable();
                    table.Columns.Add("ContractID", typeof(string));
                    // There are duplicates in the source data. The table has a unique index; remove them here, before trying to write
                    table.Columns["ContractID"].Unique = true;

                    MemoryStream stream = new MemoryStream(new WebClient().DownloadData(new Uri(ONBASE_CHECK_PDF_PATH)));
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            try
                            {
                                // The source data also contains entries with spaces, like "48324 (#2)", which don't work. Just ignore these
                                string contractID = line.Trim();

                                if (!contractID.Contains(" "))
                                {
                                    table.Rows.Add(contractID);
                                }
                            }
                            catch (Exception ex)
                            {
                                // Ignore duplicates
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }

                    if (table.Rows.Count > 0)
                    {
                        SqlParameter pList = new SqlParameter("oid", SqlDbType.Structured);
                        pList.TypeName = "dbo.OnbaseIDTableType";
                        pList.Value = table;

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(pList);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void DownloadInvoices()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                conn.Open();

                // This stored procedure takes a table-valued parameter containing the list of ContractsIDs (i.e., a 1-column table with a row for each Contract)
                using (SqlCommand cmd = new SqlCommand("SetInvoiceFlags", conn))
                {
                    // Create the 1-column table with string type. Previously it was "float" type, which removed all the contracts with dashes and letters
                    //   These alphanumeric contract numbers work when sent to OnBase though, so we shouldn't be deleting them!
                    //   Change everything to a string to handle it. 
                    DataTable table = new DataTable();
                    table.Columns.Add("ContractID", typeof(string));
                    // There are duplicates in the source data. The table has a unique index; remove them here, before trying to write
                    table.Columns["ContractID"].Unique = true;

                    MemoryStream stream = new MemoryStream(new WebClient().DownloadData(new Uri(ONBASE_INVOICE_PDF_PATH)));
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            try
                            {
                                // The source data also contains entries with spaces, like "48324 (#2)", which don't work. Just ignore these
                                string contractID = line.Trim();

                                if (!contractID.Contains(" "))
                                {
                                    table.Rows.Add(contractID);
                                }
                            }
                            catch (Exception ex)
                            {
                                // Ignore duplicates
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }

                    if (table.Rows.Count > 0)
                    {
                        SqlParameter pList = new SqlParameter("oid", SqlDbType.Structured);
                        pList.TypeName = "dbo.OnbaseIDTableType";
                        pList.Value = table;

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(pList);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void DownloadContractIDs()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                conn.Open();

                // This stored procedure takes a table-valued parameter containing the list of ContractsIDs (i.e., a 1-column table with a row for each Contract)
                using (SqlCommand cmd = new SqlCommand("InsertContractIDs", conn))
                {
                    // Create the 1-column table with string type. Previously it was "float" type, which removed all the contracts with dashes and letters
                    //   These alphanumeric contract numbers work when sent to OnBase though, so we shouldn't be deleting them!
                    //   Change everything to a string to handle it. 
                    DataTable table = new DataTable();
                    table.Columns.Add("ContractID", typeof(string));
                    // There are duplicates in the source data. The table has a unique index; remove them here, before trying to write
                    table.Columns["ContractID"].Unique = true;

                    MemoryStream stream = new MemoryStream(new WebClient().DownloadData(new Uri(ONBASE_CONTRACT_PDF_PATH)));
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            try
                            {
                                // The source data also contains entries with spaces, like "48324 (#2)", which don't work. Just ignore these
                                string contractID = line.Trim();

                                if (!contractID.Contains(" "))
                                {
                                    table.Rows.Add(contractID);
                                }
                            }
                            catch (Exception ex)
                            {
                                // Ignore duplicates
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }

                    if (table.Rows.Count > 0)
                    {
                        SqlParameter pList = new SqlParameter("oid", SqlDbType.Structured);
                        pList.TypeName = "dbo.OnbaseIDTableType";
                        pList.Value = table;

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(pList);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

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

        public static int AddContract(string contractNo, string vendorNo, string vendorName, string secondVendorNo, string secondVendorName,
                                      int departmentID, int supplementalNo, string resolutionNo, int service,
                                      decimal amount, decimal originalAmount, string description,
                                      DateTime? dateDuration, DateTime? dateApproval, DateTime? dateEntered)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddContract", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add("@ContractNo", SqlDbType.NVarChar, 50).Value = contractNo;
                    cmd.Parameters.Add("@VendorNo", SqlDbType.NVarChar, 10).Value = vendorNo;
                    cmd.Parameters.Add("@VendorName", SqlDbType.NVarChar, 50).Value = vendorName;
                    cmd.Parameters.Add("@SecondVendorNo", SqlDbType.NVarChar, 10).Value = secondVendorNo;
                    cmd.Parameters.Add("@SecondVendorName", SqlDbType.NVarChar, 50).Value = secondVendorName;
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
                                string address, string address2, string city, string state, string zip, string description,
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
                    cmd.Parameters.Add("@Address2", SqlDbType.NVarChar, 100).Value = address2;
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

                    return cmd.Parameters["@message"].Value.ToString();
                }
            }
        }

        public static string AddForeignVendor(string vendor, string vendorNo, string address1, string address2, string address3, string country, string city, string province, string zip)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddForeignVendor", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@message", SqlDbType.NVarChar, 35).Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

                    cmd.Parameters.Add("@vendorName", SqlDbType.NVarChar, 100).Value = vendor;
                    cmd.Parameters.Add("@vendorNo", SqlDbType.NVarChar, 10).Value = vendorNo;
                    cmd.Parameters.Add("@address1", SqlDbType.NVarChar, 100).Value = address1;
                    cmd.Parameters.Add("@address2", SqlDbType.NVarChar, 100).Value = address2;
                    cmd.Parameters.Add("@address3", SqlDbType.NVarChar, 100).Value = address3;
                    cmd.Parameters.Add("@country", SqlDbType.NVarChar, 50).Value = country;
                    cmd.Parameters.Add("@city", SqlDbType.NVarChar, 50).Value = city;
                    cmd.Parameters.Add("@province", SqlDbType.NVarChar, 50).Value = province;
                    cmd.Parameters.Add("@zip", SqlDbType.NVarChar, 15).Value = zip;

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    return cmd.Parameters["@message"].Value.ToString();
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


        public static int UpdateContract(string contractID, string vendorNo, string vendorName, string secondVendorNo, string secondVendorName, int departmentID, int supplementalNo, 
                                int newSupplementalNo, string resolutionNo, int service,
                                decimal amount, decimal originalAmount, string description,
                                DateTime? dateDuration, DateTime? dateApproval)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateContract", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add("@ContractID", SqlDbType.NVarChar, 50).Value = contractID;
                    cmd.Parameters.Add("@VendorNo", SqlDbType.NVarChar, 10).Value = vendorNo;
                    cmd.Parameters.Add("@VendorName", SqlDbType.NVarChar, 50).Value = vendorName;
                    cmd.Parameters.Add("@SecondVendorNo", SqlDbType.NVarChar, 10).Value = secondVendorNo;
                    cmd.Parameters.Add("@SecondVendorName", SqlDbType.NVarChar, 50).Value = secondVendorName;
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

        public static int UpdateForeignVendor(string querystringVendorNo, string vendorNo, string vendorName,
                                              string address1, string address2, string address3, string country, string city, string province, string zip)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateForeignVendor", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add("@QuerystringVendorNo", SqlDbType.NVarChar, 10).Value = (querystringVendorNo == null) ? System.DBNull.Value : (object)querystringVendorNo;
                    cmd.Parameters.Add("@VendorNo", SqlDbType.NVarChar, 10).Value = (vendorNo == null) ? System.DBNull.Value : (object)vendorNo;
                    cmd.Parameters.Add("@VendorName", SqlDbType.NVarChar, 100).Value = (vendorName == null) ? System.DBNull.Value : (object)vendorName;
                    cmd.Parameters.Add("@Address1", SqlDbType.NVarChar, 100).Value = (address1 == null) ? System.DBNull.Value : (object)address1;
                    cmd.Parameters.Add("@Address2", SqlDbType.NVarChar, 100).Value = (address2 == null) ? System.DBNull.Value : (object)address2;
                    cmd.Parameters.Add("@Address3", SqlDbType.NVarChar, 100).Value = (address3 == null) ? System.DBNull.Value : (object)address3;
                    cmd.Parameters.Add("@Country", SqlDbType.NVarChar, 50).Value = (country == null) ? System.DBNull.Value : (object)country;
                    cmd.Parameters.Add("@City", SqlDbType.NVarChar, 50).Value = (city == null) ? System.DBNull.Value : (object)city;
                    cmd.Parameters.Add("@Province", SqlDbType.NVarChar, 50).Value = (province == null) ? System.DBNull.Value : (object)province;
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

        public static DataTable GetContract(string contractID, int supplementalNo)
        {
            DataTable results = new DataTable("Results");
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetContract", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ContractID", SqlDbType.NVarChar, 50).Value = contractID;
                    cmd.Parameters.Add("@SupplementalNo", SqlDbType.Int).Value = supplementalNo;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(results);

                    return results;
                }
            }
        }

        public static DataTable GetContractByContractID(string contractID, int supplementalNo)
        {
            DataTable contracts = new DataTable("contracts");

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetContractByContractID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ContractID", SqlDbType.NVarChar, 50).Value = contractID;
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

        public static void DeleteContract(string contractID, int supplementalNo)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteContract", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ContractID", SqlDbType.NVarChar, 50).Value = contractID;
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

        public static void ApproveExpenditure(int expenditureID, string username)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ApproveExpenditure", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@expenditureID", SqlDbType.Int).Value = expenditureID;
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

        public static void SendMail(string to, string[] cc, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("webcontact@openbookpittsburgh.com");
            mail.To.Add(to);
            if (null != cc)
            {
                foreach (string carbon in cc)
                {
                    mail.CC.Add(carbon);
                }

            }
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = body;

            GetGmailClient().Send(mail);
        }

        static Admin()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM tlk_candidate", conn))
                {
                    DataTable candidates = new DataTable("candidates");

                    cmd.CommandType = CommandType.Text;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        candidates.Load(reader);

                        foreach (DataRow row in candidates.Rows)
                        {
                            mCandidateMap.Add(row["CandidateName"].ToString(), Int32.Parse(row["ID"].ToString()));
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM tlk_contributionType", conn))
                {
                    DataTable contributionTypes = new DataTable("contributionTypes");

                    cmd.CommandType = CommandType.Text;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        contributionTypes.Load(reader);

                        foreach (DataRow row in contributionTypes.Rows)
                        {
                            mContributionType.Add(row["ContributionType"].ToString(), Int32.Parse(row["ID"].ToString()));
                        }
                    }
                }
            }
        }

        public static SmtpClient GetGmailClient()
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Timeout = 10000;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtp.Credentials = new System.Net.NetworkCredential("openbookpgh@gmail.com", "\"7ca264b38bfa497cb1de\"");

            return smtp;
        }

        private static SortedDictionary<string, int> mCandidateMap = new SortedDictionary<string, int>();
        private static SortedDictionary<string, int> mContributionType = new SortedDictionary<string, int>();
    }
}
