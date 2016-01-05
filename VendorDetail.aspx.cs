using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using OpenBookAllegheny;

public partial class VendorDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
			LoadPage();
		}
    }
	private void LoadPage()
	{
		int vendorID = Utils.IntFromQueryString("ID", 0);

		if (vendorID != 0)
		{
			rptVendorDetails.DataSource = Admin.GetContractsByVendorID(vendorID);
			rptVendorDetails.DataBind();
		}
	}
}
