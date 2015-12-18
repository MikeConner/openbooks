using System;
using System.Data;
using System.Web;
using System.Web.Security;
using OpenBookAllegheny;

public partial class Admin_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		//if (!Page.IsPostBack)
		//{
		//    txtPassword.Text = "";
		//}
	}
	protected void btnLogin_Click(Object s, EventArgs e)
	{
		bool loginOK = false;

		try
		{
			loginOK = Auth.Login(Page, txtUserName.Text, txtPassword.Text);
		}
		catch (Exception ex)
		{
			string error = string.Empty;
			if (ex.Message == "Invalid attempt to read when no data is present.")
			{
				error = "Username not found.";
			}
			else
			{
				error = ex.Message;
			}
			lblMessage.Text = error;
			return;
		}

		if (loginOK == true)
		{
			Response.Redirect(FormsAuthentication.GetRedirectUrl(txtUserName.Text, false));
		}
		else
		{
			lblMessage.Text = "Password does not match.";
		}
	}
}
