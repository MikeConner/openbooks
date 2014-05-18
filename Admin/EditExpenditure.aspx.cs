using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using OpenBookPgh;

public partial class Admin_EditExpenditure : System.Web.UI.Page
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
		int expenditureID = Utils.IntFromQueryString("ID", 0);

		if (expenditureID != 0)
		{
			frmExpenditure.DataSource = Admin.GetExpenditure(expenditureID);
			frmExpenditure.DataBind();
		}
	}

	protected void Button1_Click(object sender, EventArgs e)
	{
		DropDownList CandidateName = (DropDownList)frmExpenditure.FindControl("ddlCandidateName");
		DropDownList Office = (DropDownList)frmExpenditure.FindControl("ddlOffice");
		DropDownList State = (DropDownList)frmExpenditure.FindControl("ddlState");
		TextBox Company = (TextBox)frmExpenditure.FindControl("txtCompany");
		TextBox Address = (TextBox)frmExpenditure.FindControl("txtAddress1");
		TextBox City = (TextBox)frmExpenditure.FindControl("txtCity");
		TextBox Zip = (TextBox)frmExpenditure.FindControl("txtZip");
		TextBox Description = (TextBox)frmExpenditure.FindControl("txtDescription");
		TextBox Amount = (TextBox)frmExpenditure.FindControl("txtAmount");
		TextBox Date = (TextBox)frmExpenditure.FindControl("txtDate");
		int candidateID = Convert.ToInt32(CandidateName.SelectedValue);
		decimal amount = Convert.ToDecimal(Amount.Text);
		DateTime dateExpenditure = Convert.ToDateTime(Date.Text);

		int expenditureID = Utils.IntFromQueryString("ID", 0);

		int result = Admin.UpdateExpenditure( expenditureID, candidateID, Office.Text, Company.Text, 
			Address.Text, City.Text, State.Text, Zip.Text, Description.Text, amount, dateExpenditure);

		Label error = (Label)frmExpenditure.FindControl("lblMessage");
		if (result != 0)
		{
			error.Text = "There were problems updating this expenditure. Error Code: [" + result + "]";
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
