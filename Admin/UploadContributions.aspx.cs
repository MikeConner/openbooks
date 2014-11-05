using System;
using System.Collections.Generic;

using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenBookPgh;
using System.Configuration;
using System.IO;

public partial class Admin_UploadContributionsPage : System.Web.UI.Page
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

        if (Auth.ADMIN_USER_ROLE == role){
            ddlCandidateName.Enabled = true;
        }
        else
        {
            ddlCandidateName.SelectedValue = Auth.GetCandidateID(User.Identity.Name);
        }

        UploadErrors.Visible = false;
    }

    protected void UploadButton_Click(object sender, EventArgs e)
    {
        // Specify the path on the server to
        // save the uploaded file to, if we can do that. Otherwise, just use temp file (try that first)
        // String savePath = ConfigurationManager.AppSettings["fileUploadPath"];

        // Before attempting to perform operations
        // on the the file, verify that the FileUpload 
        // control contains a file.
        if (FileUpload1.HasFile)
        {
            // Append the name of the file to upload to the path.
            //savePath += FileUpload1.FileName;

            // Server.MapPath(savePath)
            string fullPath = Path.GetTempFileName().Replace(".tmp", ".csv");
            FileUpload1.PostedFile.SaveAs(fullPath);
            //FileUpload1.SaveAs(savePath);

            // Notify the user that the file was uploaded successfully.
            UploadStatusLabel.Text = "Your file was uploaded successfully. Processing...";

            UploadErrors.Items.Clear();
            UploadErrors.Visible = false;
            List<string> errors = Admin.UploadContributions(fullPath, User.Identity.Name, int.Parse(ddlCandidateName.SelectedValue), ddlOffice.SelectedValue);

            if (0 == errors.Count) {
                UploadStatusLabel.Text = "Success!";
            }
            else {
                UploadStatusLabel.Text = "There were errors during the upload.";
                UploadErrors.Visible = true;
                foreach (string error in errors)
                {
                    UploadErrors.Items.Add(error);
                }
            }
        }
        else
        {
            // Notify the user that a file was not uploaded.
            UploadStatusLabel.Text = "You did not specify a file to upload.";
        }
    }
}
