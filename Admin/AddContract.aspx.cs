using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenBookPgh;

public partial class AddContract : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			if (Request.UrlReferrer != null)
				Session.Add("PreviousPage", Request.UrlReferrer.AbsoluteUri);

			LoadDefaults();
		}
	}
	private int _ContractID
	{
		get
		{
			int var = Utils.IntFromQueryString("ID", 0);
			return var;
		}
	}
	private int _NextContractID
	{
		get
		{
			int var = Admin.GetNextContractID();
			return var;
		}
	}



	protected void LoadDefaults()
    {
        String un = User.Identity.Name;
//        User.IsInRole("admin");

        int contractNo = 0;
		if (_ContractID != 0)
		{
			contractNo = _ContractID;
		}
		else
		{
			contractNo = _NextContractID;
		}
		txtContractNo.Text = contractNo.ToString();

		//txtDateDuration.Text = DateTime.Now.ToShortDateString();
		//txtDateApproval.Text = DateTime.Now.ToShortDateString();
		txtDateEntered.Text = DateTime.Now.ToShortDateString();
		//ddlServices.SelectedItem.Value = "82";
		ddlServices.SelectedValue = "79";
	}

	protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
	{
		rptVendor.DataSource = Admin.GetVendorAddress(ddlVendors.SelectedValue);
		rptVendor.DataBind();
	}

	protected void Button1_Click(object sender, EventArgs e)
	{
		// Add Contract through Helper
		int result = AddContractHelper();
		if (result == -1)
		{
			lblMessage.Text = "Contract#/Supplemental# already in use.";
		}
		else if (result == -2)
		{
			lblMessage.Text = "Contract# can not be greater than the next contract #.";
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
		// Add Contract through Helper
		int result = AddContractHelper();
		if (result == -1)
		{
			lblMessage.Text = "Contract#/Supplemental# already in use.";
		}
		else if (result == -2)
		{
			lblMessage.Text = "Contract# can not be greater than the next contract #.";
		}
		else
		{
			// Update label and reset text boxes
			lblMessage.Text = "Record Added for Vendor: " + ddlVendors.SelectedItem.Text;
			txtResolutionNo.Text = "";
			txtSupplementalNo.Text = "";
			txtDescription.Text = "";
			txtAmount.Text = "0";
			txtOriginalAmount.Text = "";
			txtDateApproval.Text = "";
			txtDateDuration.Text = "";
			txtDateEntered.Text = "";
		}
	}

	private int AddContractHelper()
	{
		int result = 0;
		bool sendRequest = false;

		string resolutionNo = txtResolutionNo.Text;
		string vendorNo = ddlVendors.SelectedValue;
		string description = txtDescription.Text;
		int departmentID = Convert.ToInt32(ddlDepartment.SelectedValue);
		int service = Convert.ToInt32(ddlServices.SelectedValue);

		int supplementalNo = 0;
		decimal amount = 0;
		decimal originalAmount = 0;
		bool supplemntalOK = Int32.TryParse(txtSupplementalNo.Text, out supplementalNo);
		bool amountOK = Decimal.TryParse(txtAmount.Text, out amount);
		bool originalOK = Decimal.TryParse(txtOriginalAmount.Text, out originalAmount);

		DateTime? dateDuration = null;
		DateTime? dateApproval = null;
		DateTime? dateEntered = null;

		if (!String.IsNullOrEmpty(txtDateDuration.Text))
		{
			dateDuration = Convert.ToDateTime(txtDateDuration.Text);
		}
		if (!String.IsNullOrEmpty(txtDateApproval.Text))
		{
			dateApproval = Convert.ToDateTime(txtDateApproval.Text);
		}
		if (!String.IsNullOrEmpty(txtDateEntered.Text))
		{
			dateEntered = Convert.ToDateTime(txtDateEntered.Text);
		}

		int contractNo = Convert.ToInt32(txtContractNo.Text);
		if (contractNo == _NextContractID)
		{
			// standard - let DB auto assign and increment
			contractNo = 0;
			sendRequest = true;
		}
		else if (contractNo > _NextContractID)
		{
			// return error - can not go past current contract
			sendRequest = false;
			result = -2;
		}
		else
		{
			// check if contract #/sup# in use - SP does the work for now
			sendRequest = true;
		}

		if (sendRequest)
		{
			result = Admin.AddContract(contractNo, vendorNo, departmentID, supplementalNo, resolutionNo, service,
				amount, originalAmount, description, dateDuration, dateApproval, dateEntered);
            String un = User.Identity.Name;

		}
		return result;

	}

}
