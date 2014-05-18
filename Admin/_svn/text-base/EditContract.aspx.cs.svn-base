using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using OpenBookPgh;

public partial class Admin_EditContract : System.Web.UI.Page
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
		int contractID = Utils.IntFromQueryString("ID", 0);
		int supplementalNo = Utils.IntFromQueryString("sup", 0);

		if (contractID != 0)
		{
			frmContract.DataSource = Admin.GetContract(contractID, supplementalNo);
			frmContract.DataBind();
		}
	}

	protected void Button1_Click(object sender, EventArgs e)
	{
		DropDownList Department = (DropDownList)frmContract.FindControl("ddlDepartment");
		DropDownList Vendor = (DropDownList)frmContract.FindControl("ddlVendors");
		DropDownList Service = (DropDownList)frmContract.FindControl("ddlServices");

		TextBox SupplementalNo = (TextBox)frmContract.FindControl("txtSupplementalNo");
		TextBox ResolutionNo = (TextBox)frmContract.FindControl("txtResolutionNo");
		TextBox DateDuration = (TextBox)frmContract.FindControl("txtDateDuration");
		TextBox DateApproval = (TextBox)frmContract.FindControl("txtDateApproval");
		TextBox DateEntered = (TextBox)frmContract.FindControl("txtDateEntered");
		TextBox Amount = (TextBox)frmContract.FindControl("txtAmount");
		TextBox OriginalAmount = (TextBox)frmContract.FindControl("txtOriginalAmount");
		TextBox Description = (TextBox)frmContract.FindControl("txtDescription");

		int contractID = Utils.IntFromQueryString("ID", 0);
		string vendorNo = Vendor.SelectedValue;
		string resolutionNo = ResolutionNo.Text;

		//int departmentID = Convert.ToInt32(Department.SelectedValue);		
		int service = Convert.ToInt32(Service.SelectedValue);


		int departmentID = 0;
		int newSupplementalNo = 0;
		decimal amount = 0;
		decimal originalAmount = 0;
		bool deptOK = Int32.TryParse(Department.SelectedValue, out departmentID);
		bool supplmentalOK = Int32.TryParse(SupplementalNo.Text, out newSupplementalNo);
		bool amountOK = Decimal.TryParse(Amount.Text, out amount);
		bool originalOK = Decimal.TryParse(OriginalAmount.Text, out originalAmount);

		DateTime? dateDuration = null;
		DateTime? dateApproval = null;
		DateTime? dateEntered = null;

		if (!String.IsNullOrEmpty(DateDuration.Text))
		{
			dateDuration = Convert.ToDateTime(DateDuration.Text);
		}
		if (!String.IsNullOrEmpty(DateApproval.Text))
		{
			dateApproval = Convert.ToDateTime(DateApproval.Text);
		}
		if (!String.IsNullOrEmpty(DateEntered.Text))
		{
			dateEntered = Convert.ToDateTime(DateEntered.Text);
		}

		int supplementalNo = Utils.IntFromQueryString("sup", 0);


		int result = Admin.UpdateContract(contractID, vendorNo, departmentID, supplementalNo, newSupplementalNo, 
		resolutionNo, service, amount, originalAmount, Description.Text, dateDuration, dateApproval);

		Label error = (Label)frmContract.FindControl("lblMessage");
		if (result != 0)
		{
			error.Text = "That contract#/supplemental # is already in use and can not be used.";
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
