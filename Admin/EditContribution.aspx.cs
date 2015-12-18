using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using OpenBookAllegheny;

public partial class Admin_EditContribution : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
			if (Request.UrlReferrer != null)
				Session.Add("PreviousPage", Request.UrlReferrer.AbsoluteUri);

			LoadPage();
		}
    }
	private void LoadPage()
	{
		int contributionID = Utils.IntFromQueryString("ID", 0);

		if (contributionID != 0)
		{
			frmContribution.DataSource = Admin.GetContribution(contributionID);
			frmContribution.DataBind();
		}
	}


	protected void Button1_Click(object sender, EventArgs e)
	{
		DropDownList CandidateName = (DropDownList)frmContribution.FindControl("ddlCandidateName");
		DropDownList Office = (DropDownList)frmContribution.FindControl("ddlOffice");
		DropDownList State = (DropDownList)frmContribution.FindControl("ddlState");
		RadioButtonList ContributorType = (RadioButtonList)frmContribution.FindControl("rblContributor");
		DropDownList ContributionType = (DropDownList)frmContribution.FindControl("ddlContributionType");

		TextBox Contributor = (TextBox)frmContribution.FindControl("txtContributor");
		TextBox Description = (TextBox)frmContribution.FindControl("txtDescription");
		TextBox City = (TextBox)frmContribution.FindControl("txtCity");

		TextBox Zip = (TextBox)frmContribution.FindControl("txtZip");
		TextBox Employer = (TextBox)frmContribution.FindControl("txtEmployer");
		TextBox Occupation = (TextBox)frmContribution.FindControl("txtOccupation");
		TextBox Amount = (TextBox)frmContribution.FindControl("txtAmount");
		TextBox Date = (TextBox)frmContribution.FindControl("txtDate");	
	
		int candidateName = Convert.ToInt32(CandidateName.SelectedValue);

		decimal amount = 0;
		bool amountOK = Decimal.TryParse(Amount.Text, out amount);

		int contributionType = 0;
		bool cOK = Int32.TryParse(ContributionType.SelectedValue, out contributionType);

		DateTime? dateContribution = null;

		if (!String.IsNullOrEmpty(Date.Text))
		{
			dateContribution = Convert.ToDateTime(Date.Text);
		}

		int contributionID = Utils.IntFromQueryString("ID", 0);
		
		int result = Admin.UpdateContribution(
			contributionID,
			candidateName,
			Office.SelectedValue,
			ContributorType.SelectedValue,
			Contributor.Text, 
			contributionType, 
			Description.Text,
			City.Text,
			State.Text, 
			Zip.Text,
			Employer.Text, 
			Occupation.Text, 
			amount,
			dateContribution
		);
		
		Label error = (Label)frmContribution.FindControl("lblMessage");
		if (result != 0)
		{
			error.Text = "There were problems updating this contribution. Error Code: [" + result + "]";
		}
		else
		{
			if (Session["PreviousPage"] != null)
				Response.Redirect((string)Session["PreviousPage"]);
			else
				Response.Redirect("~/Admin/Default.aspx");
		}

	}
}
