using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Security;
using OpenBookAllegheny;

public partial class _Masters_AdminMasterPage : System.Web.UI.MasterPage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		// causes page not to be cached
		Response.Cache.SetCacheability(HttpCacheability.NoCache);

		// check if user is logged in

		if (HttpContext.Current.User != null)
		{
			if (HttpContext.Current.User.Identity.IsAuthenticated)
			{
				FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                string role = id.Ticket.UserData;
                panelViewAdminFunction.Visible = Auth.ADMIN_USER_ROLE == role;
                panelAddAdminFunction.Visible = Auth.ADMIN_USER_ROLE == role;
                panelCandidateFunction.Visible = Auth.CANDIDATE_USER_ROLE == role;

				panelLoggedIn.Visible = true;
				panelNotLoggedIn.Visible = false;
			}
		}
		else
		{
			panelLoggedIn.Visible = false;
			panelNotLoggedIn.Visible = true;
		}
	}

	protected void lbtnLogout_Click(object sender, EventArgs e)
	{
		Auth.Logout(Page);
	}
   
   

}
