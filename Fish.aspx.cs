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
        string step1 = System.AppDomain.CurrentDomain.BaseDirectory + "App_Data\\Contracts_Step_1.csv";
        string step2 = System.AppDomain.CurrentDomain.BaseDirectory + "App_Data\\Contracts_Step_2.csv";
        
        List<string> errors = Admin.UploadPayments(step1, step2);
        foreach (string error in errors)
        {
            Console.WriteLine(error);
        }

        Response.Redirect("Default.aspx");
    }
}