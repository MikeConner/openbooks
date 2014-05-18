﻿using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenBookPgh;

public partial class Admin_AddContributionPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		if(!IsPostBack)
		{
			if (Request.UrlReferrer != null)
				Session.Add("PreviousPage", Request.UrlReferrer.AbsoluteUri);

			LoadPage();
		}
    }
    protected void LoadPage()
    {
		rblContributor.SelectedValue = "in";
		ddlState.SelectedValue = "PA";
		txtDate.Text = DateTime.Now.ToShortDateString();
    }
	protected void Button1_Click(object sender, EventArgs e)
	{
		int candidateID = Convert.ToInt32(ddlCandidateName.SelectedValue);
		string office = ddlOffice.SelectedValue;
		string contributorType = rblContributor.SelectedValue;
		string contributor = txtContributor.Text;
		string description = txtDescription.Text;
		string city = txtCity.Text;
		string state = ddlState.SelectedValue;
		string zip = txtZip.Text;
		string employer = txtEmployer.Text;
		string occupation = txtOccupation.Text;

		decimal amount = 0;
		bool amountOK = Decimal.TryParse(txtAmount.Text, out amount);

		int contributionType = 0;
		bool cOK = Int32.TryParse(ddlContributionType.SelectedValue, out contributionType);


		DateTime? dateContribution = null;

		if (!String.IsNullOrEmpty(txtDate.Text))
		{
			dateContribution = Convert.ToDateTime(txtDate.Text);
		}

		int result = Admin.AddContribution(candidateID, office, contributorType, contributor, contributionType, description, city, state, zip, 
												employer, occupation, amount, dateContribution);
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
		string city = txtCity.Text;
		string state = ddlState.SelectedValue;
		string zip = txtZip.Text;
		string employer = txtEmployer.Text;
		string occupation = txtOccupation.Text;

		decimal amount = 0;
		bool amountOK = Decimal.TryParse(txtAmount.Text, out amount);

		int contributionType = 0;
		bool cOK = Int32.TryParse(ddlContributionType.SelectedValue, out contributionType);


		DateTime? dateContribution = null;

		if (!String.IsNullOrEmpty(txtDate.Text))
		{
			dateContribution = Convert.ToDateTime(txtDate.Text);
		}

		//int result = 0;
		int result = Admin.AddContribution(candidateID, office, contributorType, contributor, contributionType, description, city, state, zip,
		                                        employer, occupation, amount, dateContribution);
												
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
			txtEmployer.Text = "";
			txtOccupation.Text = "";
			txtAmount.Text = "0";
			
			// Set Focus
			txtContributor.Focus();
		}
	}
}
