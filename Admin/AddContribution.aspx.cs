using System;
using System.Collections.Generic;

using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenBookPgh;
using System.Configuration;
using System.IO;

public partial class Admin_AddContributionPage : System.Web.UI.Page
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

        if (Auth.CANDIDATE_USER_ROLE == role)
        {
            // If this is a candidate user, restrict to that user's candidate
            ddlCandidateName.SelectedValue = Auth.GetCandidateID(User.Identity.Name);
            ddlCandidateName.Enabled = false;
        }

		rblContributor.SelectedValue = "in";
		ddlState.SelectedValue = "PA";
        ddlCountry.SelectedValue = "US";
		txtDate.Text = DateTime.Now.ToShortDateString();
    }
	protected void Button1_Click(object sender, EventArgs e)
	{
		int candidateID = Convert.ToInt32(ddlCandidateName.SelectedValue);
		string office = ddlOffice.SelectedValue;
		string contributorType = rblContributor.SelectedValue;
		string contributor = txtContributor.Text;
		string description = txtDescription.Text;
        string address = txtStreet.Text;
		string city = txtCity.Text;
		string state = ddlState.SelectedValue;
        string province = txtProvince.Text;
		string zip = txtZip.Text;
        string country = ddlCountry.SelectedValue;
		string employer = txtEmployer.Text;
		string occupation = txtOccupation.Text;

		decimal amount = 0;
		bool amountOK = Decimal.TryParse(txtAmount.Text, out amount);

		int contributionType = 0;
		bool cOK = Int32.TryParse(ddlContributionType.SelectedValue, out contributionType);


		DateTime? dateContribution = null;

		if (!string.IsNullOrEmpty(txtDate.Text))
		{
			dateContribution = Convert.ToDateTime(txtDate.Text);
		}

        if (false == string.IsNullOrEmpty(province))
        {
            state = "FR";
        }

		int result = Admin.AddContribution(candidateID, office, contributorType, contributor, contributionType, description, address, city, state, province, zip, 
												country, employer, occupation, amount, User.Identity.Name, dateContribution);
		if (result != 0)
		{
			lblMessage.Text = "There were problems adding this contribution. Error Code: [" + result + "]";
		}
		else
		{
			if (Session["PreviousPage"] != null)
				Response.Redirect((string)Session["PreviousPage"]);
			else
				Response.Redirect("~/Admin/Default.aspx");
		}
	}
	protected void AddAnother_Click(object sender, EventArgs e)
	{
		int candidateID = Convert.ToInt32(ddlCandidateName.SelectedValue);
		string office = ddlOffice.SelectedValue;
		string contributorType = rblContributor.SelectedValue;
		string contributor = txtContributor.Text;
		string description = txtDescription.Text;
        string address = txtStreet.Text;
		string city = txtCity.Text;
		string state = ddlState.SelectedValue;
        string province = txtProvince.Text;
		string zip = txtZip.Text;
        string country = ddlCountry.SelectedValue;
		string employer = txtEmployer.Text;
		string occupation = txtOccupation.Text;

		decimal amount = 0;
		bool amountOK = Decimal.TryParse(txtAmount.Text, out amount);

		int contributionType = 0;
		bool cOK = Int32.TryParse(ddlContributionType.SelectedValue, out contributionType);


		DateTime? dateContribution = null;

		if (!string.IsNullOrEmpty(txtDate.Text))
		{
			dateContribution = Convert.ToDateTime(txtDate.Text);
		}

        if (false == string.IsNullOrEmpty(province))
        {
            state = "FR";
        }

        int result = Admin.AddContribution(candidateID, office, contributorType, contributor, contributionType, description, address, city, state, province, zip,
		                                        country, employer, occupation, amount, User.Identity.Name, dateContribution);												
		if (result != 0)
		{
			lblMessage.Text = "There were problems adding this contribution. Error Code: [" + result + "]";
		}
		else
		{
			// Update label and reset text boxes
			lblMessage.Text = "Record Added for " + contributor;
			txtContributor.Text = "";
			txtDescription.Text = "";
			txtCity.Text = "";
			txtZip.Text = "";
            txtProvince.Text = "";
			txtEmployer.Text = "";
			txtOccupation.Text = "";
			txtAmount.Text = "0";
            txtStreet.Text = "";
            ddlCountry.SelectedValue = "US";
			
			// Set Focus
			txtContributor.Focus();
		}
	}
}
