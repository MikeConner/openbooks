using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace OpenBookAllegheny
{
    class CSVParser
    {
        public static DataTable ParseCSV(string path)
        {
            if (!File.Exists(path))
            {
                Admin.Log("CSVParser: File doesn't Exist", path);
                return null;
            }

            string full = Path.GetFullPath(path);
            string file = Path.GetFileName(full);
            string dir = Path.GetDirectoryName(full);

            //create the "database" connection string 
            string connString = "Provider=Microsoft.Jet.OLEDB.4.0;"
              + "Data Source=\"" + dir + "\\\";"
              + "Extended Properties=\"text;HDR=No;FMT=Delimited\"";

            Admin.Log("CSVParser: connString", connString);
            Admin.Log("CSVParser: full", full);
            //create the database query
            string query = "SELECT * FROM " + file;

            Admin.Log("CSVParser: Query", query);
            //create a DataTable to hold the query results
            DataTable dTable = new DataTable();

            //create an OleDbDataAdapter to execute the query
            OleDbDataAdapter dAdapter = new OleDbDataAdapter(query, connString);

            try
            {
                //fill the DataTable
                dAdapter.Fill(dTable);
                Admin.Log("CSVParser: Filled DataTable", "Success");
            }
            catch (InvalidOperationException ex)
            {
                Admin.Log("CSVParser: Error during Ole", ex.Message);
                Console.WriteLine(ex.Message);
            }

            dAdapter.Dispose();

            return dTable;
        }
    }
}
