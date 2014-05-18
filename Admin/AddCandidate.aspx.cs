using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenBookPgh;

public partial class Admin_AddCandidate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
			if (Request.UrlReferrer != null)
				Session.Add("PreviousPage", Request.UrlReferrer.AbsoluteUri);
        }
    }
	protected void Button1_Click(object sender, EventArgs e)
	{
		string candidate = txtCandidateName.Text;
		
		int result = Admin.AddCandidate(candidate);
		if (result != 0)
		{
			lblMessage.Text = "There were problems adding this candidate. Error Code: [" + result + "]";
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
