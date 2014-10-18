using System;
using OpenBookPgh;

public partial class Admin_AddUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		if (!IsPostBack)
		{
			if (Request.UrlReferrer != null)
				Session.Add("PreviousPage", Request.UrlReferrer.AbsoluteUri);
		}
    }
	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		string salt = Auth.CreateSalt(5);
		string passwordHash = Auth.CreatePasswordHash(passwd.Text, salt);
		try
		{
			Auth.AddUser(first_name.Text, last_name.Text, initials.Text, email.Text, user_name.Text, passwordHash, salt);
            Auth.SetUserRoles(user_name.Text, Auth.CANDIDATE_USER_ROLE);
		}
		catch (Exception ex)
		{
			lblMessage.Text = ex.Message;
		}

		if (Session["PreviousPage"] != null)
			Response.Redirect((string)Session["PreviousPage"]);
		else
			Response.Redirect("~/Admin/Default.aspx");	
	}
}
