using System;
using System.Net;
using OpenBookAllegheny;

public partial class Admin_AddUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Auth.EnsureRole(Auth.ADMIN_USER_ROLE))
        {
            if (!IsPostBack)
            {
                if (Request.UrlReferrer != null)
                    Session.Add("PreviousPage", Request.UrlReferrer.AbsoluteUri);
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
	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		string salt = Auth.CreateSalt(5);
		string passwordHash = Auth.CreatePasswordHash(passwd.Text, salt);
		try
		{
			Auth.AddUser(first_name.Text, last_name.Text, initials.Text, email.Text, user_name.Text, passwordHash, salt);
            Auth.SetUserRoles(user_name.Text, Auth.ADMIN_USER_ROLE);
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
