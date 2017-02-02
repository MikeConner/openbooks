using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenBookPgh;

public partial class Admin_DownloadContributions : System.Web.UI.Page
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

        if (Auth.ADMIN_USER_ROLE != role)
        {
            Response.Redirect("Error.aspx");
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        DataTable contributions = Admin.GetContributions();
        if (contributions.Rows.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            string[] columnNames = new string[contributions.Columns.Count];

            for (int x = 0; x < contributions.Columns.Count; x++)
            {
                columnNames[x] = contributions.Columns[x].ColumnName;
            }

            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in contributions.Rows)
            {
                string[] fields = new string[contributions.Columns.Count];

                fields[0] = row["CandidateName"].ToString();
                fields[1] = row["OfficeName"].ToString();
                fields[2] = row["ContributionType"].ToString();
                fields[3] = row["ContributorTypeName"].ToString();
                fields[4] = "\"" + row["ContributorName"].ToString() + "\"";
                fields[5] = "\"" + row["City"].ToString() + "\"";
                fields[6] = row["State"].ToString();
                fields[7] = "\"" + row["Employer"].ToString() + "\"";
                fields[8] = "\"" + row["Occupation"].ToString() + "\"";
                fields[9] = row["Amount"].ToString();
                fields[10] = String.Format("{0:MM/dd/yyyy}", row["ContributionDate"]);

                sb.AppendLine(string.Join(",", fields));
            }

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=Contributions.csv");
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
            lblProgress.Text = "No contributions to download.";
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