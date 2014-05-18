﻿using System;
using OpenBookPgh;


public partial class AddVendor : System.Web.UI.Page
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
	}
	protected void Button1_Click(object sender, EventArgs e)
	{
		string vendor = txtVendorName.Text;
		string vendorNo = txtVendorNo.Text;
		string address1 = txtAddress1.Text;
		string address2 = txtAddress2.Text;
		string address3 = txtAddress3.Text;
		string city = txtCity.Text;
		string state = ddlState.SelectedValue;
		string zip = txtZip.Text;

		string result = Admin.AddVendor(vendor, vendorNo, address1, address2, address3, city, state, zip);
		if(!String.IsNullOrEmpty(result))
		{
			lblMessage.Text = "There were problems adding this vendor." + result;
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
