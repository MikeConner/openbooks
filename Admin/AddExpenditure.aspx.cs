﻿using System;
using System.Collections.Generic;

using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenBookPgh;

public partial class Admin_AddExpenditure : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Auth.EnsureRole(Auth.CANDIDATE_USER_ROLE))
        {
            if (!IsPostBack)
            {
                if (Request.UrlReferrer != null)
                    Session.Add("PreviousPage", Request.UrlReferrer.AbsoluteUri);

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

        // If this is a candidate user, restrict to that user's candidate
        if (Auth.CANDIDATE_USER_ROLE == role)
        {
            // Select and fix the candidate
            ddlCandidateName.SelectedValue = Auth.GetCandidateID(User.Identity.Name);
            ddlCandidateName.Enabled = false;
        }

        ddlState.SelectedValue = "PA";
        ddlCountry.SelectedValue = "US";
		txtDate.Text = DateTime.Now.ToShortDateString();
	}
	protected void Button1_Click(object sender, EventArgs e)
	{
		int candidateID = Convert.ToInt32(ddlCandidateName.SelectedValue);
		string office = ddlOffice.SelectedValue;
		string company = txtCompany.Text;
		string address = txtAddress1.Text;
        string address2 = txtAddress2.Text;
		string city = txtCity.Text;
		string state = ddlState.SelectedValue;
        string province = txtProvince.Text;
		string zip = txtZip.Text;
        string country = ddlCountry.SelectedValue;
		string description = txtDescription.Text;


		decimal amount = 0;
		bool amountOK = Decimal.TryParse(txtAmount.Text, out amount);

		DateTime? dateExpenditure = null;

		if (false == string.IsNullOrEmpty(txtDate.Text))
		{
			dateExpenditure = Convert.ToDateTime(txtDate.Text);
		}

        if ((province != null) && (province != ""))
        {
            state = "FR";
        }

        int result = Admin.AddExpenditure(candidateID, office, company, address, address2, city, state, province, zip, country, description, amount, User.Identity.Name, dateExpenditure);
		if (result != 0)
		{
			lblMessage.Text = "There were problems adding this expenditure. Error Code: [" + result + "]";
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
		string company = txtCompany.Text;
		string address = txtAddress1.Text;
        string address2 = txtAddress2.Text;
		string city = txtCity.Text;
		string state = ddlState.SelectedValue;
        string province = txtProvince.Text;
        string country = ddlCountry.SelectedValue;
		string zip = txtZip.Text;
		string description = txtDescription.Text;


		decimal amount = 0;
		bool amountOK = Decimal.TryParse(txtAmount.Text, out amount);

		DateTime? dateExpenditure = null;

		if (!String.IsNullOrEmpty(txtDate.Text))
		{
			dateExpenditure = Convert.ToDateTime(txtDate.Text);
		}

        if ((province != null) && (province != ""))
        {
            state = "FR";
        }

        int result = Admin.AddExpenditure(candidateID, office, company, address, address2, city, state, province, zip, country, description, amount, User.Identity.Name, dateExpenditure);
		if (result != 0)
		{
			lblMessage.Text = "There were problems adding this expenditure. Error Code: [" + result + "]";
		}
		else
		{
			// Update label and reset text boxes
			lblMessage.Text = "Record Added for " + company;
			txtCompany.Text = "";
			txtAddress1.Text = "";
			txtCity.Text = "";
			txtZip.Text = "";
			txtDescription.Text = "";
			txtAmount.Text = "0";

			// Set Focus
			txtCompany.Focus();
		}
	}
}
