using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenBookPgh;

public partial class Admin_AddExpenditure : System.Web.UI.Page
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
	protected void LoadPage()
	{
		ddlState.SelectedValue = "PA";
		txtDate.Text = DateTime.Now.ToShortDateString();
	}
	protected void Button1_Click(object sender, EventArgs e)
	{
		int candidateID = Convert.ToInt32(ddlCandidateName.SelectedValue);
		string office = ddlOffice.SelectedValue;
		string company = txtCompany.Text;
		string address = txtAddress1.Text;
		string city = txtCity.Text;
		string state = ddlState.SelectedValue;
		string zip = txtZip.Text;
		string description = txtDescription.Text;


		decimal amount = 0;
		bool amountOK = Decimal.TryParse(txtAmount.Text, out amount);

		DateTime? dateExpenditure = null;

		if (!String.IsNullOrEmpty(txtDate.Text))
		{
			dateExpenditure = Convert.ToDateTime(txtDate.Text);
		}
		

		int result = Admin.AddExpenditure(candidateID, office, company, address, city, state, zip, description, amount, dateExpenditure);
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
		string city = txtCity.Text;
		string state = ddlState.SelectedValue;
		string zip = txtZip.Text;
		string description = txtDescription.Text;


		decimal amount = 0;
		bool amountOK = Decimal.TryParse(txtAmount.Text, out amount);

		DateTime? dateExpenditure = null;

		if (!String.IsNullOrEmpty(txtDate.Text))
		{
			dateExpenditure = Convert.ToDateTime(txtDate.Text);
		}

		//int result = 0;
		int result = Admin.AddExpenditure(candidateID, office, company, address, city, state, zip, description, amount, dateExpenditure);
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
