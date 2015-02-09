using System;
using OpenBookPgh;

public partial class Admin_AutoContractSync : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Admin.DownloadContractIDs();
        Admin.DownloadChecks();
        Admin.DownloadInvoices();
    }
}
