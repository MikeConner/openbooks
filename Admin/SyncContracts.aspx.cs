using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Net;
using System.Threading;
using OpenBookAllegheny;

public partial class Admin_SyncContracts : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Auth.EnsureRole(Auth.ADMIN_USER_ROLE))
        {
            if (!IsPostBack)
            {
                if (Request.UrlReferrer != null)
                {
                    Session.Add("PreviousPage", Request.UrlReferrer.AbsoluteUri);
                }
            }
            //Response.AppendHeader("Refresh", "2");
        }
        else
        {
            Response.Status = "403 Forbidden";
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            Response.StatusDescription = "Permission failure";
            // Could also redirect to "/Error.aspx"
            Response.Redirect("Default.aspx");
        }
    }

    protected void SynchronizeData(object sender, EventArgs e)
    {
        btnSync.Enabled = false;

        lblSyncStatus.Text = "Synchronizing Contracts...";
        Admin.DownloadContractIDs();

        lblSyncStatus.Text = "Synchronizing Checks...";
        Admin.DownloadChecks();

        lblSyncStatus.Text = "Synchronizing Invoices...";
        Admin.DownloadInvoices();

        lblSyncStatus.Text = "Done!";

        btnSync.Enabled = false;
    }
}
