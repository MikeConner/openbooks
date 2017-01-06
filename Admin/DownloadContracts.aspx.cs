using System;
using System.Collections.Generic;

using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenBookPgh;
using System.Configuration;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Globalization;

public partial class Admin_DownloadContractsPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Auth.EnsureRole(Auth.CANDIDATE_USER_ROLE))
        {
            if (!IsPostBack)
            {
                if (Request.UrlReferrer != null)
                {
                    Session.Add("PreviousPage", Request.UrlReferrer.AbsoluteUri);
                }
                LoadPage();
            }
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
    protected void LoadPage()
    {
        string role = Auth.GetUserRoles(User.Identity.Name);

        if (Auth.ADMIN_USER_ROLE != role){
            Response.Redirect("Error.aspx");
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        DataTable contracts = Admin.GetContracts();
        if (contracts.Rows.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            string[] columnNames = new string[contracts.Columns.Count];

            for (int x = 0; x < contracts.Columns.Count; x++)
            {
                columnNames[x] = contracts.Columns[x].ColumnName;
            }

            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in contracts.Rows)
            {
                string[] fields = new string[contracts.Columns.Count];

                fields[0] = row["ContractID"].ToString(); 
                fields[1] = String.Format("{0:MM/dd/yyyy}", row["StartDate"]);
                fields[2] = '"' + row["VendorName"].ToString() + '"';
                fields[3] = '"' + row["Description"].ToString() + '"';
                fields[4] = row["ServiceName"].ToString();
                fields[5] = row["Amount"].ToString();
                fields[6] = '"' + row["ResolutionNo"].ToString() + '"';
                fields[7] = String.Format("{0:MM/dd/yyyy}", row["Duration"]);
                fields[8] = row["OriginalAmount"].ToString();

                sb.AppendLine(string.Join(",", fields));
            }

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=Contracts.csv");
            Response.AddHeader("content-type", "text/plain");

            using (StreamWriter writer = new StreamWriter(Response.OutputStream))
            {
                writer.WriteLine(sb.ToString());
            }
            Response.End();

            lblDownloadCompleted.Visible = true;
        }
        else
        {
            lblProgress.Text = "No contracts to download.";
        }
    }

    private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        lblProgress.Text = e.ProgressPercentage.ToString() + "% complete.";
    }

    private void Completed(object sender, AsyncCompletedEventArgs e)
    {
        lblProgress.Text = "100% Download completed!";
    }
}
