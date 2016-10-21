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
using DataStreams.ETL;
using System.Collections;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace OpenBookAllegheny
{
    /// <summary>
    /// Summary description for Admin
    /// </summary>
    public class Admin
    {
        public static double FEE_BASED_CONTRACT_AMOUNT = 999999.99;
        public static string FEE_BASED_AMOUNT_STRING = "$999,999.99";

        public static void UploadAlleghenyContracts(String filename)
        {
            UploadAlleghenyContracts(filename, true);
        }

        public static void SSHDownload()
        {
            using (SftpClient client = new SftpClient(ConfigurationManager.AppSettings["SftpHost"].ToString(),
                                                      ConfigurationManager.AppSettings["SftpUsername"].ToString(),
                                                      ConfigurationManager.AppSettings["SftpPassword"].ToString()))
            {
                string remoteDirectory = "/OpenBook/";

                try
                {
                    client.Connect();

                    IEnumerable<SftpFile> files = client.ListDirectory(remoteDirectory, null);
                    foreach (SftpFile file in files)
                    {
                        if (!file.Name.StartsWith("."))
                        {
                            string localFile = Path.GetTempFileName();

                            try
                            {
                                using (Stream tempFile = File.OpenWrite(localFile))
                                {
                                    client.DownloadFile(file.FullName, tempFile, null);
                                }

                                Admin.UploadAlleghenyContracts(localFile);
                            }
                            finally
                            {
                                File.Delete(localFile);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    client.Disconnect();
                }
            }
        }

        public static void UploadAlleghenyContracts(string filename, bool reset)
        {
            DataTable depts = new DataTable();
            DataTable types = new DataTable();
            DataTable vendors = new DataTable();
            DataTable accounts = new DataTable();
            accounts.Clear();
            accounts.Columns.Add("ContractID", typeof(int));
            accounts.Columns.Add("AccountNo", typeof(string));
            accounts.Columns.Add("Description", typeof(string));

            // Would rather use a HashSet, but not in this version of .NET
            Dictionary<int, int> contractIds = new Dictionary<int, int>();

            DataTable contracts = new DataTable();
            contracts.Clear();
            contracts.Columns.Add("ContractID", typeof(int));
            contracts.Columns.Add("ContractTypeID", typeof(int));
            contracts.Columns.Add("VendorNo", typeof(int));
            contracts.Columns.Add("VendorName", typeof(string));
            contracts.Columns.Add("DepartmentID", typeof(int));
            contracts.Columns.Add("Amount", typeof(decimal));
            contracts.Columns.Add("OrderDate", typeof(DateTime));
            contracts.Columns.Add("CancelDate", typeof(DateTime));
            contracts.Columns.Add("AccountNo", typeof(string));
            contracts.Columns.Add("ExecutiveAction", typeof(string));
            contracts.Columns.Add("Comments", typeof(string));

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AlleghenyCountyConnectionString"].ConnectionString))
            {
                try
                {
                    conn.Open();

                    using (SqlDataAdapter a = new SqlDataAdapter("SELECT * FROM tlk_department", conn))
                    {
                        a.Fill(depts);
                        depts.PrimaryKey = new DataColumn[] { depts.Columns["DeptCode"] };
                    }

                    using (SqlDataAdapter a = new SqlDataAdapter("SELECT * FROM order_types", conn))
                    {
                        a.Fill(types);
                        types.PrimaryKey = new DataColumn[] { types.Columns["ID"] };
                    }

                    using (SqlDataAdapter a = new SqlDataAdapter("SELECT * FROM vendors", conn))
                    {
                        a.Fill(vendors);
                        vendors.PrimaryKey = new DataColumn[] { vendors.Columns["VendorNo"] };
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            //string fname = System.AppDomain.CurrentDomain.BaseDirectory + "documents\\" + filename;
            DataTable table = CSVParser.ParseCSV(filename);
            bool first = true;
            foreach (DataRow row in table.Rows)
            {
                if (first)
                {
                    first = false;
                    continue;
                }

                string rawOrderType = row[0].ToString();
                string vendorName = row[4].ToString();
                string accountNo = row[7].ToString();
                string execAction = row[8].ToString();
                string desc1 = row[9].ToString();
                string rawDeptName = row[10].ToString();
                string desc2 = row[12].ToString();

                int contractID;
                double price;
                int vendorNo;
                DateTime start, end;

                // There can be format exceptions with things like blank lines
                try
                {
                    contractID = Int32.Parse(row[1].ToString());
                    price = Double.Parse(row[2].ToString(), NumberStyles.Number);
                    vendorNo = Int32.Parse(row[3].ToString());
                    start = DateTime.Parse(row[5].ToString());
                    end = DateTime.Parse(row[6].ToString());
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(row);

                    continue;
                }

                int deptId = -1;
                int typeId = -1;

                string comments = desc2;
                if (desc1 != desc2)
                {
                    comments += ";" + desc1;
                }

                DataRow[] r = depts.Select("DeptName = '" + rawDeptName + "'");
                if (1 == r.Length)
                {
                    deptId = (int)((DataRow)r.GetValue(0)).ItemArray[0];
                }
                else if (0 == r.Length)
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AlleghenyCountyConnectionString"].ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand();

                        cmd.CommandText = "INSERT INTO tlk_department (DeptName) OUTPUT INSERTED.DeptCode VALUES (@name)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@name", rawDeptName);
                        cmd.Connection = conn;

                        conn.Open();

                        deptId = (int)cmd.ExecuteScalar();

                        // Add so that select will find it
                        DataRow workRow = depts.NewRow();
                        workRow["DeptCode"] = deptId;
                        workRow["DeptName"] = rawDeptName;
                        depts.Rows.Add(workRow);
                    }
                }
                else
                {
                    throw new Exception("Duplicate DeptName: " + rawDeptName);
                }

                r = types.Select("OrderType = '" + rawOrderType + "'");
                if (1 == r.Length)
                {
                    typeId = (int)((DataRow)r.GetValue(0)).ItemArray[0];
                }
                else if (0 == r.Length)
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AlleghenyCountyConnectionString"].ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand();

                        cmd.CommandText = "INSERT INTO order_types (OrderType) OUTPUT INSERTED.ID VALUES (@type)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@type", rawOrderType);
                        cmd.Connection = conn;

                        conn.Open();

                        typeId = (int)cmd.ExecuteScalar();

                        // Add so that select will find it
                        DataRow workRow = types.NewRow();
                        workRow["ID"] = typeId;
                        workRow["OrderType"] = rawOrderType;
                        types.Rows.Add(workRow);
                    }
                }
                else
                {
                    throw new Exception("Duplicate OrderType: " + rawOrderType);
                }

                if (!vendors.Rows.Contains(vendorNo))
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AlleghenyCountyConnectionString"].ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand();

                        cmd.CommandText = "INSERT INTO vendors (VendorNo, VendorName) VALUES (@vendorNo, @vendorName)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@vendorNo", vendorNo);
                        cmd.Parameters.AddWithValue("@vendorName", vendorName);
                        cmd.Connection = conn;

                        conn.Open();

                        cmd.ExecuteNonQuery();

                        DataRow workRow = vendors.NewRow();
                        workRow["VendorNo"] = vendorNo;
                        workRow["VendorName"] = vendorName;
                        vendors.Rows.Add(workRow);
                    }
                }

                // Is this a new contract?
                if (!contractIds.ContainsKey(contractID))
                {
                    // It's a new one
                    DataRow contractRow = contracts.NewRow();

                    contractRow["ContractID"] = contractID;
                    contractRow["ContractTypeID"] = typeId;
                    contractRow["VendorNo"] = vendorNo;
                    contractRow["VendorName"] = vendorName;
                    contractRow["DepartmentID"] = deptId;
                    contractRow["Amount"] = price;
                    contractRow["OrderDate"] = start;
                    contractRow["CancelDate"] = end;
                    contractRow["ExecutiveAction"] = execAction;

                    contracts.Rows.Add(contractRow);

                    contractIds.Add(contractID, 1);
                }

                DataRow accountRow = accounts.NewRow();

                accountRow["ContractID"] = contractID;
                accountRow["AccountNo"] = accountNo;
                accountRow["Description"] = comments;

                accounts.Rows.Add(accountRow);
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AlleghenyCountyConnectionString"].ConnectionString))
            {
                conn.Open();

                if (reset)
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM accounts", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd = new SqlCommand("DELETE FROM contracts", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }

                using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
                {
                    //Set destination table name
                    //to table previously created.
                    bulkcopy.DestinationTableName = "dbo.contracts";
                    bulkcopy.ColumnMappings.Add("ContractID", "[ContractID]");
                    bulkcopy.ColumnMappings.Add("ContractTypeID", "[ContractTypeID]");
                    bulkcopy.ColumnMappings.Add("VendorNo", "[VendorNo]");
                    bulkcopy.ColumnMappings.Add("VendorName", "[VendorName]");
                    bulkcopy.ColumnMappings.Add("DepartmentID", "[DepartmentID]");
                    bulkcopy.ColumnMappings.Add("Amount", "[Amount]");
                    bulkcopy.ColumnMappings.Add("OrderDate", "[OrderDate]");
                    bulkcopy.ColumnMappings.Add("CancelDate", "[CancelDate]");
                    bulkcopy.ColumnMappings.Add("ExecutiveAction", "[ExecutiveAction]");
                    /*
                     * ValidatingDataReader will give details of any error - uncomment in case of exception
                     * 
                     * using (ValidatingDataReader validator = new ValidatingDataReader(contracts.CreateDataReader(), conn, bulkcopy))
                    {
                        bulkcopy.WriteToServer(validator);
                    } */
                    try
                    {
                        bulkcopy.WriteToServer(contracts);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
                {
                    //Set destination table name
                    //to table previously created.
                    bulkcopy.DestinationTableName = "dbo.accounts";
                    bulkcopy.ColumnMappings.Add("ContractID", "[ContractID]");
                    bulkcopy.ColumnMappings.Add("AccountNo", "[AccountNo]");
                    bulkcopy.ColumnMappings.Add("Description", "[Description]");

                    /*
                     * ValidatingDataReader will give details of any error - uncomment in case of exception
                     * 
                    using (ValidatingDataReader validator = new ValidatingDataReader(accounts.CreateDataReader(), conn, bulkcopy))
                    {
                        bulkcopy.WriteToServer(validator);
                    } */

                    try
                    {
                        bulkcopy.WriteToServer(accounts);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
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

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AlleghenyCountyConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetContractByContractID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ContractID", SqlDbType.NVarChar, 50).Value = contractID;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(contracts);

                    FilterAggregateDescription(contracts, conn);

                    return contracts;
                }
            }
        }

        public static DataTable GetContractsByVendorID(int vendorID)
        {
            DataTable contracts = new DataTable("contracts");

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AlleghenyCountyConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetContractsByVendor", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@VendorID", SqlDbType.Int).Value = vendorID;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(contracts);

                    FilterAggregateDescription(contracts, conn);

                    return contracts;
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

        public static void FilterAggregateDescription(DataTable results, SqlConnection conn)
        {
            // Post-process AggregateDescription to extract unique values
            char[] delimiters = new char[] { ',', ';', '/' };
            foreach (DataRow row in results.Rows)
            {
                string[] descFields = row["AggregateDescription"].ToString().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                ArrayList fields = new ArrayList();
                foreach (string field in descFields)
                {
                    if (!fields.Contains(field))
                    {
                        fields.Add(field);
                    }
                }
                string aggregate = "";
                foreach (string field in fields)
                {
                    aggregate += field + " ";
                }

                row["AggregateDescription"] = aggregate.Trim();
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
            mail.From = new MailAddress("webcontact@openbookallegheny.com");
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
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AlleghenyCountyConnectionString"].ConnectionString))
            {
                /*conn.Open();
                 
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
                } */
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
