using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using OpenBookPgh;

public partial class EditVendor : System.Web.UI.Page
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
            DataTable table = Admin.GetVendor(vendorNo);
			frmVendor.DataSource = table;
            try
            {
                frmVendor.DataBind();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Can't bind State above, because it's a Dropdown and might be null if it's a foreign vendor. 
            // Trying to bind a null value to a Dropdown will throw an exception
            RadioButtonList Nationality = (RadioButtonList)frmVendor.FindControl("rbNationality");
            TextBox Country = (TextBox)frmVendor.FindControl("Country");
            if (string.IsNullOrEmpty(Country.Text))
            {
                Nationality.SelectedValue = "US";
                DropDownList ddlState = (DropDownList)frmVendor.FindControl("ddlState");
                ddlState.SelectedValue = table.Rows[0]["State"].ToString();
            }
            else {
                Nationality.SelectedValue = "Foreign";
            }
		}
	}

	protected void Button1_Click(object sender, EventArgs e)
	{
        TextBox VendorNo = (TextBox)frmVendor.FindControl("txtVendorNo");
        string vendorNo = Utils.StringFromQueryString("id", "0", "", true);
        TextBox VendorName = (TextBox)frmVendor.FindControl("txtVendorName");
        TextBox Address1 = (TextBox)frmVendor.FindControl("txtAddress1");
        TextBox Address2 = (TextBox)frmVendor.FindControl("txtAddress2");
        TextBox Address3 = (TextBox)frmVendor.FindControl("txtAddress3");
        TextBox City = (TextBox)frmVendor.FindControl("txtCity");
        TextBox Country = (TextBox)frmVendor.FindControl("Country");
        TextBox Province = (TextBox)frmVendor.FindControl("Province");
        TextBox PostalCode = (TextBox)frmVendor.FindControl("PostalCode");

        int result = -1;

        RadioButtonList Nationality = (RadioButtonList)frmVendor.FindControl("rbNationality");
        if ("US" == Nationality.SelectedValue)
        {
            DropDownList State = (DropDownList)frmVendor.FindControl("ddlState");
            TextBox Zip = (TextBox)frmVendor.FindControl("txtZip");

            result = Admin.UpdateVendor(vendorNo, VendorNo.Text, VendorName.Text, Address1.Text, Address2.Text, Address3.Text, City.Text, State.Text, Zip.Text);
        }
        else
        {
            string country = Country.Text;
            string province = Province.Text;
            string zip = PostalCode.Text;

            result = Admin.UpdateForeignVendor(vendorNo, VendorNo.Text, VendorName.Text, Address1.Text, Address2.Text, Address3.Text, country, City.Text, province, zip);
        }

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

    protected void Nationality_SelectedIndexChanged(object sender, EventArgs e)
    {
        RequiredFieldValidator ZipValidator = (RequiredFieldValidator)frmVendor.FindControl("ZipValidator");
         RequiredFieldValidator CountryValidator = (RequiredFieldValidator)frmVendor.FindControl("CountryValidator");
         RadioButtonList Nationality = (RadioButtonList)frmVendor.FindControl("rbNationality");
         
         RequiredFieldValidator PostalCodeValidator = (RequiredFieldValidator)frmVendor.FindControl("PostalCodeValidator");
        RequiredFieldValidator ProvinceValidator = (RequiredFieldValidator)frmVendor.FindControl("ProvinceValidator");
        
         ZipValidator.Enabled = ("US" == Nationality.SelectedValue);
        CountryValidator.Enabled = ProvinceValidator.Enabled = PostalCodeValidator.Enabled = ("Foreign" == Nationality.SelectedValue);
    }
}
