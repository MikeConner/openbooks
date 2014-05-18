using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using OpenBookPgh;

using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ContractDetail : System.Web.UI.Page
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
		int contractID = Utils.IntFromQueryString("ID", 0);
		int supplementalNo = Utils.IntFromQueryString("sup", 0); // zero is standard for single contracts

		if (contractID != 0)
		{
			rptContractDetails.DataSource = Admin.GetContractByContractID(contractID, supplementalNo);
			rptContractDetails.DataBind();

			// Check local Onbase table if PDF exists
			//int result = Admin.CheckOnbaseContract(contractID);
			//if(result != -1)
			//{
			//    pnlContractPDF.Visible = true;
			//    hlContractPDF.NavigateUrl = "http://onbaseapp.city.pittsburgh.pa.us/PublicAccess/Contracts.aspx?OBKey__138_1=" + contractID;
			//}
		}
	}
	protected void rptContracts_ItemCommand(object source, RepeaterCommandEventArgs e)
	{
		if (e.CommandName == "ViewPDF")
		{
			int contractID = Convert.ToInt32(e.CommandArgument.ToString());
			OpenNewWindow("http://onbaseapp.city.pittsburgh.pa.us/PublicAccess/Contracts.aspx?OBKey__138_1=" + contractID);
		}
	}
	public void OpenNewWindow(string url)
	{
		ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
	}

}
