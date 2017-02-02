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

namespace OpenBookPgh
{
    /// <summary>
    /// Summary description for Admin
    /// </summary>
    public class Admin
    {
        public static string ONBASE_CONTRACT_PDF_PATH = "http://onbaseapp.city.pittsburgh.pa.us/OpenBookPublicData/contracts.csv";
        public static string ONBASE_CHECK_PDF_PATH = "http://onbaseapp.city.pittsburgh.pa.us/OpenBookPublicData/checks.csv";
        public static string ONBASE_INVOICE_PDF_PATH = "http://onbaseapp.city.pittsburgh.pa.us/OpenBookPublicData/invoices.csv";

        public static List<string> UploadPayments(string step1, string step2)
        {
            List<string> errors = new List<string>();
            Dictionary<string, decimal> amounts = new Dictionary<string, decimal>();
            Dictionary<int, string> funds = new Dictionary<int, string>();
            Dictionary<string, string> cost_centers = new Dictionary<string, string>();
            Dictionary<int, string> object_accounts = new Dictionary<int, string>();
            Dictionary<int, string> line_items = new Dictionary<int, string>();

            // Clear out old Amounts Received, and old lookup tables
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(null, conn))
                {
                    command.CommandText = "UPDATE contracts SET AmountReceived=NULL";
                    command.ExecuteNonQuery();

                    command.CommandText = "DELETE FROM cost_centers";
                    command.ExecuteNonQuery();

                    command.CommandText = "DELETE FROM funds";
                    command.ExecuteNonQuery();

                    command.CommandText = "DELETE FROM line_items";
                    command.ExecuteNonQuery();

                    command.CommandText = "DELETE FROM obj_accounts";
                    command.ExecuteNonQuery();

                    command.CommandText = "DELETE FROM payments";
                    command.ExecuteNonQuery();
                }
            }

            DataTable table = CSVParser.ParseCSV(step1);
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

                string contractID = row[0].ToString().Trim();

                try
                {
                    decimal amountReceived = decimal.Parse(row[1].ToString(), NumberStyles.Currency);
                    
                    amounts.Add(contractID, amountReceived);
                }
                catch (FormatException ex)
                {
                    errors.Add("Step 1. Error in row " + idx + " (" + ex.Message + ")");
                }
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(null, conn))
                {
                    /*SqlParameter idParam = new SqlParameter("@id", SqlDbType.NVarChar, 50);
                    SqlParameter amountParam = new SqlParameter("@amount", SqlDbType.Decimal, 0);
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(amountParam);

                    command.Prepare(); */
                    SqlTransaction transaction = conn.BeginTransaction();
                    command.Transaction = transaction;

                    int batch = 1000;

                    foreach (KeyValuePair<string, decimal> entry in amounts)
                    {
                        //idParam.Value = entry.Key;
                        //amountParam.Value = entry.Value;
                        command.CommandText = String.Format("UPDATE contracts SET AmountReceived = {0} WHERE ContractID = '{1}'", entry.Value, entry.Key);

                        command.ExecuteNonQuery();
                        batch--;

                        if (0 == batch)
                        {
                            try
                            {
                                transaction.Commit();

                                transaction = conn.BeginTransaction();
                                command.Transaction = transaction;
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                throw;
                            }

                            batch = 1000;
                        }
                    }

                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            table = CSVParser.ParseCSV(step2);
            idx = -1;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(null, conn))
                {
                    SqlTransaction transaction = conn.BeginTransaction();
                    command.Transaction = transaction;

                    int batch = 1000;

                    foreach (DataRow row in table.Rows)
                    {
                        if (-1 == idx)
                        {
                            // Skip header
                            idx++;
                            continue;
                        }
                        idx++;

                        try
                        {
                            string contractID = row[3].ToString().Trim();
                            int fundID = int.Parse(row[4].ToString());
                            string fund = SqlEscape(row[5].ToString());
                            string centerID = row[6].ToString().Trim();
                            string center = SqlEscape(row[7].ToString());
                            int accountID = int.Parse(row[8].ToString());
                            string account = SqlEscape(row[9].ToString());
                            int itemID = string.IsNullOrEmpty(row[10].ToString()) ? -1 : int.Parse(row[10].ToString());
                            string item = -1 == itemID ? null : SqlEscape(row[11].ToString());

                            decimal amountReceived = decimal.Parse(row[13].ToString(), NumberStyles.Currency);

                            if (false == funds.ContainsKey(fundID))
                            {
                                funds.Add(fundID, fund);
                            }

                            if (false == cost_centers.ContainsKey(centerID))
                            {
                                cost_centers.Add(centerID, center);
                            }

                            if (false == object_accounts.ContainsKey(accountID))
                            {
                                object_accounts.Add(accountID, account);
                            }

                            if ((-1 != itemID) && (false == line_items.ContainsKey(itemID)))
                            {
                                line_items.Add(itemID, item);
                            }

                            // Ignore 0 records
                            if (0 == amountReceived)
                            {
                                continue;
                            }

                            if (-1 == itemID)
                            {
                                command.CommandText = string.Format("INSERT INTO payments (ContractID,Fund,CostCenter,ObjectAccount,AmountReceived) VALUES('{0}',{1},'{2}',{3},{4})", contractID, fundID, centerID, accountID, amountReceived);
                            }
                            else
                            {
                                command.CommandText = string.Format("INSERT INTO payments VALUES('{0}',{1},'{2}',{3},{4},{5})", contractID, fundID, centerID, accountID, itemID, amountReceived);
                            }
                            command.ExecuteNonQuery();
                            batch--;

                            if (0 == batch)
                            {
                                try
                                {
                                    transaction.Commit();

                                    transaction = conn.BeginTransaction();
                                    command.Transaction = transaction;
                                }
                                catch (Exception)
                                {
                                    transaction.Rollback();
                                    throw;
                                }

                                batch = 1000;
                            }
                        }
                        catch (FormatException ex)
                        {
                            errors.Add("Step 2. Error in row " + idx + " (" + ex.Message + ")");
                        }
                    }
                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(null, conn))
                {
                    SqlTransaction transaction = conn.BeginTransaction();
                    command.Transaction = transaction;

                    foreach (KeyValuePair<int, string> entry in funds)
                    {
                        command.CommandText = String.Format("INSERT INTO funds VALUES({0},'{1}')", entry.Key, entry.Value);

                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }

                    transaction = conn.BeginTransaction();
                    command.Transaction = transaction;
                    foreach (KeyValuePair<string, string> entry in cost_centers)
                    {
                        command.CommandText = String.Format("INSERT INTO cost_centers VALUES('{0}','{1}')", entry.Key, entry.Value);

                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }

                    transaction = conn.BeginTransaction();
                    command.Transaction = transaction;
                    foreach (KeyValuePair<int, string> entry in object_accounts)
                    {
                        command.CommandText = String.Format("INSERT INTO obj_accounts VALUES({0},'{1}')", entry.Key, entry.Value);

                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }

                    transaction = conn.BeginTransaction();
                    command.Transaction = transaction;
                    foreach (KeyValuePair<int, string> entry in line_items)
                    {
                        command.CommandText = String.Format("INSERT INTO line_items VALUES({0},'{1}')", entry.Key, entry.Value);

                        command.ExecuteNonQuery();
                    }
                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return errors;
        }

        private static string SqlEscape(string input)
        {
            return input.Trim().Replace("(", "[").Replace(")", "]").Replace("'", "''");
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

        public static DataTable GetPaymentsByContractID(string contractID)
        {
            DataTable rawPayments = new DataTable("raw-payments");
            DataTable payments = new DataTable("payments");
            payments.Columns.Add("Fund", typeof(String));
            payments.Columns.Add("Center", typeof(String));
            payments.Columns.Add("Account", typeof(String));
            payments.Columns.Add("Item", typeof(String));
            payments.Columns.Add("TotalPaid", typeof(Decimal));

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                conn.Open();

                if (0 == mFundsMap.Count)
                {
                    // Initialize lookup tables
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM funds", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable funds = new DataTable("Funds");
                            funds.Load(reader);
                            foreach (DataRow row in funds.Rows)
                            {
                                mFundsMap.Add(int.Parse(row["ID"].ToString()), row["Name"].ToString().Trim());
                            }
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM cost_centers", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable centers = new DataTable("CostCenters");
                            centers.Load(reader);
                            foreach (DataRow row in centers.Rows)
                            {
                                mCostCenterMap.Add(row["ID"].ToString().Trim(), row["Name"].ToString().Trim());
                            }
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM obj_accounts", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable accounts = new DataTable("Accounts");
                            accounts.Load(reader);
                            foreach (DataRow row in accounts.Rows)
                            {
                                mAccountMap.Add(int.Parse(row["ID"].ToString()), row["Name"].ToString().Trim());
                            }
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM line_items", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable items = new DataTable("Items");
                            items.Load(reader);
                            foreach (DataRow row in items.Rows)
                            {
                                mLineItemMap.Add(int.Parse(row["ID"].ToString()), row["Name"].ToString().Trim());
                            }
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand("GetPaymentsByContractID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ContractID", SqlDbType.NVarChar, 50).Value = contractID;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        rawPayments.Load(reader);
                        foreach (DataRow row in rawPayments.Rows)
                        {
                            // Read Raw table (with codes), look up the codes, and create new DataTable with text values
                            DataRow paymentsRow = payments.Rows.Add();
                            paymentsRow["Fund"] = mFundsMap[int.Parse(row["Fund"].ToString())];
                            paymentsRow["Center"] = mCostCenterMap[row["CostCenter"].ToString()];
                            paymentsRow["Account"] = mAccountMap[int.Parse(row["ObjectAccount"].ToString())];
                            paymentsRow["Item"] = mLineItemMap[int.Parse(row["LineItem"].ToString())];
                            paymentsRow["TotalPaid"] = decimal.Parse(row["Total"].ToString());
                        }

                        return payments;
                    }
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

        public static DataTable GetContracts()
        {
            DataTable results = new DataTable("Contracts");

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                string cmdText = "SELECT ContractID, CAST(DateCountersigned AS date) AS StartDate, v.VendorName, Description, s.ServiceName, Amount, ResolutionNo, CAST(DateDuration AS date) AS Duration, OriginalAmount FROM contracts c LEFT JOIN vendors v ON c.vendorNo = v.vendorNo LEFT JOIN tlk_service s ON c.Service = s.ID;";
                
                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        results.Load(reader);
                    }
                }
            }

            return results;
        }

        public static DataTable GetContributions()
        {
            DataTable results = new DataTable("Contributions");

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetContributions", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        results.Load(reader);
                    }
                }
            }

            return results;
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
        private static SortedDictionary<int, string> mFundsMap = new SortedDictionary<int, string>();
        private static SortedDictionary<string, string> mCostCenterMap = new SortedDictionary<string, string>();
        private static SortedDictionary<int, string> mAccountMap = new SortedDictionary<int, string>();
        private static SortedDictionary<int, string> mLineItemMap = new SortedDictionary<int, string>();

    }
}
