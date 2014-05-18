using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using OpenBookPgh;


using System.Web.Security;
using System.Security.Principal;


public partial class Admin_Users : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
        if (!IsPostBack)
        {
			if (Request.UrlReferrer != null)
				Session.Add("PreviousPage", Request.UrlReferrer.AbsoluteUri);

			LoadPage();
		}


		string[] authData = ((FormsIdentity)User.Identity).Ticket.UserData.Split(new char[] { '|' });
		string authRole = authData[0];
		if (authRole == "admin")
        {
			pnlAdmin.Visible = true;
		}


		
		
	}
	private void LoadPage()
	{
		// Load Users Repeater
		rptUsers.DataSource = Admin.LoadUsers();
		rptUsers.DataBind();
	}
	
	protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
	{
		if (e.CommandName == "reset")
		{
			Response.Redirect("ResetPassword.aspx?id=" + e.CommandArgument.ToString());
		}
		if (e.CommandName == "delete")
		{
			Admin.DeleteUser(Convert.ToInt32(e.CommandArgument.ToString()));
			if (Session["PreviousPage"] != null)
				Response.Redirect((string)Session["PreviousPage"]);
			else
				Response.Redirect("~/Admin/Default.aspx");
		}

	}


}
