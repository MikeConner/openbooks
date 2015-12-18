using System;
using System.Net;
using OpenBookAllegheny;


public partial class AddVendor : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
        if (Auth.EnsureRole(Auth.ADMIN_USER_ROLE))
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

        string result = null;

        // If it is foreign, we have country, city, province, zip
        // If it is US, we have city, state, zip
        if ("US" == Nationality.SelectedValue)
        {
            string zip = txtZip.Text;

            result = Admin.AddVendor(vendor, vendorNo, address1, address2, address3, city, state, zip);
        }
        else
        {
            string country = Country.Text;
            string province = Province.Text;
            string zip = PostalCode.Text;

            result = Admin.AddForeignVendor(vendor, vendorNo, address1, address2, address3, country, city, province, zip);
        }

		if(!String.IsNullOrEmpty(result))
		{
			lblMessage.Text = "There were problems adding this vendor." + result;
		}
		else
		{
			if (Session["PreviousPage"] != null) {
                Response.Redirect((string)Session["PreviousPage"]);
            }
            else
            {
                Response.Redirect("~/Admin/Default.aspx");
            }
		}
	}

    protected void Nationality_SelectedIndexChanged(object sender, EventArgs e)
    {
        ZipValidator.Enabled = ("US" == Nationality.SelectedValue);
        CountryValidator.Enabled = ProvinceValidator.Enabled = PostalCodeValidator.Enabled = ("Foreign" == Nationality.SelectedValue);
    }
}
