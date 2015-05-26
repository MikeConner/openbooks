using System.Configuration;
using System;
using System.Data.SqlClient;


// Configuration:  create DB user on ONBase with db_datareader (and public) access to onbase.  SQL authentication.  schema db_datareader / database role also db_datareader
// server role: public

// Integrated security Not coded right now.  TODO: write code to allow for integrated security

//We should also ask about diffs vs. doing the full set.  Ideal is full set and we get everything to a temp table and they deal with deltas.
//What to do about removed documents (if we do all set, we don't have to worry about it here) (same w/ modified)

// Also ask about PDf viewer as default
// also ask if we are providing link (Url ) field  - that will be a parameter
// ask about column headings
// Group by: To get the doc count we need group_by BUT... we won't have unique docIDs to share anymore (or what do we share here?)


//instructions:     Find ONBdocketNumberOwner (K325 for test system) - This is the docket number keyword
//                  Find ONBkeyValueDatefieldTable (keyitem167 for test system) - this is the date filed keyword
//                  Find ONBDocketNumberTable (hsi.keyitem325 for test system.  hsi.keyitem + number for DocketNumber) - Docket number
//                  


namespace OnBasePMS
{
    class Program
    {
        static private string OnBaseConnectionString;
        static private string PMSConnectionString;
        static private string OnBaseSQLString;



        // if args are empty, try to do both sides of the ETL
        // if args has value 'OnbaseOnly' - only perform onbase portion
        // if args has value 'PMSonly' - only do the data insert
        // if args has value 'help' - show the previuos options
        // args can be used to do diff - we will need a way of persisting a time stamp
       

        static void Main(string[] args)
        {
            // if arg say to do Onbase Side, proceed with Onbase:

            if (buildOnBConnectionString() == 0)
            {
                if (buildOnBSQL() == 0)
                {
                    Console.ReadKey();
                    // catch anything / every possible error  - put to console 
                    SqlConnection conn = new SqlConnection(OnBaseConnectionString);
                    SqlCommand command = new SqlCommand(OnBaseSQLString, conn);

                    //try this - catch any errors, log them out if they do not work
                    conn.Open();

                    //store query results in (csv? sql inserts?)
                    // make sure everything can pull property
                    SqlDataReader reader = command.ExecuteReader();
                    string filePath = ConfigurationManager.AppSettings.Get("FilePathForPMSsqlInsert");


                    string SQLInsert = "";

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filePath, true))
                    {
                        while (reader.Read())
                        {
                            SQLInsert = "INSERT INTO TEMPTABLENAME ([docketNum],[doc_type],[date_filed],[docid],[itemnum],[itemname],[batchnum],[status]) " +
                            " VALUES (< " + reader[0] + ", varchar(30),>" +
                            ",< "+ reader[1] + ", char(66),>" +
                            ",< "+ reader[2] + ", datetime,> " +
                            ",< " + reader[3] + ", int,> " +
                            ",< " + reader[4] + ", int,> " +
                            ",< " + reader[5] + ", char(255),> " +
                            ",< " + reader[6] + ", int,> " +
                            ",< " + reader[7] + ", int,>); \n\r";

                            file.WriteLine(SQLInsert);

                            Console.WriteLine("\t{0}\t{1}\t{2}",
                                reader[0], reader[1], reader[2]);
                        }
                    }
                    
                    reader.Close();
                    Console.ReadKey();

                }
            }
            // if args say to do PMS Side 
            if (buildPMSConnectionString() == 0)
            {
            }

        }


         static private int buildOnBConnectionString()
        {
            //throw any errors when reading config file, otherwise be verbose about connection string built (minus password)
            //update class variable with onb connection string

            //return 0 if everything is ok
            //return error code if something when wrong
            //send email if something is wrong?  (do we have code for this?)

            OnBaseConnectionString = "User ID=" + ConfigurationManager.AppSettings.Get("onbaseDBuser") + ";Password=" + ConfigurationManager.AppSettings.Get("onbaseDBpassword") + ";Initial Catalog=" + ConfigurationManager.AppSettings.Get("onbaseDBname") + ";Data Source=" + ConfigurationManager.AppSettings.Get("onbaseDBserver");
            Console.WriteLine(OnBaseConnectionString);
            return 0;

        }



        static private int buildOnBSQL()
        {
            string SQLselect = "";
            string SQLFrom = "";
            string SQLWhere = "";


            SQLselect = "SELECT " + ConfigurationManager.AppSettings.Get("ONBdocketNumberOwner") + "." +
                ConfigurationManager.AppSettings.Get("ONBdocketNumberField")
                + " AS 'docketNum', " +

                "(select " + ConfigurationManager.AppSettings.Get("ONBitemtypeField") +
                " from " + ConfigurationManager.AppSettings.Get("ONBitemTypeTable") + "  where " +
                ConfigurationManager.AppSettings.Get("ONBitemtypeField") + " = i." +
                ConfigurationManager.AppSettings.Get("ONBitemtypeField") + ") as doc_type, " +

                "(select " + ConfigurationManager.AppSettings.Get("ONBkeyvaluedateField") +
                " from " + ConfigurationManager.AppSettings.Get("ONBkeyValueDatefieldTable") +
                " where " + ConfigurationManager.AppSettings.Get("ONBitemnumField") + " = i." +
                ConfigurationManager.AppSettings.Get("ONBitemnumField") + ") as date_filed" +

                ", i." + ConfigurationManager.AppSettings.Get("ONBitemnumField") + " as docid," +
                " i.* ";

            SQLFrom =  " FROM " + ConfigurationManager.AppSettings.Get("ONBhsiItemDataTable")   + "  i " + 
                "LEFT JOIN " + ConfigurationManager.AppSettings.Get("ONBDocketNumberTable") + " " +
                ConfigurationManager.AppSettings.Get("ONBdocketNumberOwner") + " ON " +
                ConfigurationManager.AppSettings.Get("ONBdocketNumberOwner") + 
                "." + ConfigurationManager.AppSettings.Get("ONBitemnumField") + " = i." +
                ConfigurationManager.AppSettings.Get("ONBitemnumField");


            SQLWhere = " WHERE " + ConfigurationManager.AppSettings.Get("ONBdocketNumberOwner") + "." + 
                ConfigurationManager.AppSettings.Get("ONBitemnumField") + " IS NOT NULL ";

            OnBaseSQLString = SQLselect + " " + SQLFrom + " " + SQLWhere;
            Console.WriteLine(OnBaseSQLString);
            //pick values for tables / owners 
            // build query to fetch fields for output


            /*


DECLARE @doc_type INT;
SET @doc_type = 229;

SELECT
k325.keyvaluechar AS 'docketNum', 
(select itemtypename from [ONBASE].[hsi].[itemtype] where itemtypenum = i.itemtypenum) as doc_type,
(select keyvaluedate  from hsi.keyitem167 where itemnum = i.itemnum) as date_filed,
i.itemnum as docid,
i.*

FROM hsi.itemdata i

LEFT JOIN hsi.keyitem325 k325 ON k325.itemnum = i.itemnum
 
WHERE   k325.itemnum IS NOT NULL  

// GROUP BY DOC_TYPE, inlcude how many there are of each one - So what is docID

/*
table: hsi.itemdata
itemdata table: hsi.itemdata
keyword table: hsi.keyitem325
Joinfieldkey: k325.itemnum
DocTypeField: his.itemdata.itemtypenum 
DocType: 119 (or whatever the doc type is there) 
DocTypeFilter: Boolean (on/off)
Field1- "docketnumber" : k325.keyvaluechar
Field2- "Doc Type": SubQuery:  (select itemtypename from [ONBASE].[hsi].[itemtype] where itemtypenum = i.itemtypenum) as doc_type,
Field3- : date filed : keyitemnumber for date filed + match using itemnum to document (select keyvaluedate  from hsi.keyitem167 where itemnum = 1558
DocId: i. Itemnum

URL: 
*/



            return 0;

        }
        static private int buildPMSConnectionString()
        {
            //throw any errors when reading config file, otherwise be verbose about connection string built (minus password)
            //update class variable with onb connection string

            //return 0 if everything is ok
            //return error code if something when wrong
            //send email if something is wrong?  (do we have code for this?)

            PMSConnectionString = "User ID=" + ConfigurationManager.AppSettings.Get("PMSDBuser") + ";Password=" + ConfigurationManager.AppSettings.Get("PMSDBpassword") + ";Initial Catalog=" + ConfigurationManager.AppSettings.Get("PMSDBname") + ";Data Source=" + ConfigurationManager.AppSettings.Get("PMSDBserver");
            Console.WriteLine(PMSConnectionString);
            return 0;

        }

    }
}
