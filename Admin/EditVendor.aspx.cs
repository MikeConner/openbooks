using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using OpenBookPgh;

public partial class Admin_EditVendor : System.Web.UI.Page
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
		string vendorNo = Utils.StringFromQueryString("id", "0", "", true);

		if (vendorNo != "0")
		{
			frmVendor.DataSource = Admin.GetVendor(vendorNo);
			frmVendor.DataBind();
		}
	}

	protected void Button1_Click(object sender, EventArgs e)
	{
		DropDownList State = (DropDownList)frmVendor.FindControl("ddlState");

		TextBox VendorName = (TextBox)frmVendor.FindControl("txtVendorName");
		TextBox VendorNo = (TextBox)frmVendor.FindControl("txtVendorNo");
		TextBox Address1 = (TextBox)frmVendor.FindControl("txtAddress1");
		TextBox Address2 = (TextBox)frmVendor.FindControl("txtAddress2");
		TextBox Address3 = (TextBox)frmVendor.FindControl("txtAddress3");
		TextBox City = (TextBox)frmVendor.FindControl("txtCity");
		TextBox Zip = (TextBox)frmVendor.FindControl("txtZip");

		string vendorNo = Utils.StringFromQueryString("id", "0", "", true);

		int result = Admin.UpdateVendor(vendorNo, VendorNo.Text, VendorName.Text, Address1.Text, Address2.Text, 
			Address3.Text, City.Text, State.Text, Zip.Text);

		Label error = (Label)frmVendor.FindControl("lblMessage");
		if (result != 0)
		{
			error.Text = "There were problems updating this vendor. Error Code: [" + result + "]";
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
