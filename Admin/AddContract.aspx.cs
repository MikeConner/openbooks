using System;
using System.Collections.Generic;

using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenBookPgh;

public partial class AddContract : System.Web.UI.Page
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

                LoadDefaults();
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
	private string _ContractID
	{
		get
		{
			string var = Utils.StringFromQueryString("ID", null, null, true);
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
        string contractNo = null;
		if (string.IsNullOrEmpty(_ContractID))
		{
            contractNo = _NextContractID.ToString();
        }
		else
		{
            contractNo = _ContractID;
        }
		txtContractNo.Text = contractNo;

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

		return Admin.AddContract(txtContractNo.Text, vendorNo, departmentID, supplementalNo, resolutionNo, service,
			amount, originalAmount, description, dateDuration, dateApproval, dateEntered);
	}
}
