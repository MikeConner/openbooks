using OpenBookPgh;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Fish : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string dir = System.AppDomain.CurrentDomain.BaseDirectory + "App_Data\\Payments.csv";
        List<string> errors = Admin.UploadFinancials(dir);
        foreach (string error in errors)
        {
            Console.WriteLine(error);
        }
    }
}