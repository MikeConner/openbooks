using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenBookPgh;

public partial class SearchExpendituresPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
	protected void Button1_Click(object sender, EventArgs e)
	{
		int candidateID = Convert.ToInt32(ddlCandidateName.SelectedValue);
		string office = ""; // used in admin portion only
		string vendor = txtVendor.Text;
		string vendorSearchOptions = rblVendorSearchOptions.SelectedValue;
		string keywords = txtKeywords.Text;

		string queryString = SearchExpenditures.GenerateQueryString(candidateID, office, vendor, vendorSearchOptions, keywords);
		Response.Redirect(queryString);
	}
}
