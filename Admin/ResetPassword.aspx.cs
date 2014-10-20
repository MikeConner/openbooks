using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenBookPgh;

public partial class Admin_ResetPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		if (!IsPostBack)
		{
            if (Request.UrlReferrer != null)
            {
                Session.Add("PreviousPage", Request.UrlReferrer.AbsoluteUri);
            }
		}
    }
	protected void Button1_Click(object sender, EventArgs e)
	{
		string password = txtPassword.Text;
		string password2 = txtPassword2.Text;

        if (password == password2)
        {
            int userID = Convert.ToInt32(Request.QueryString["id"]);
            string salt = Auth.CreateSalt(5);
            string passwordHash = Auth.CreatePasswordHash(password, salt);
            try
            {
                Auth.ResetPassword(userID, passwordHash, salt);
                if (Session["PreviousPage"] != null)
                {
                    Response.Redirect((string)Session["PreviousPage"]);
                }
                else
                {
                    Response.Redirect("~/Admin/Default.aspx");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        else
        {
            lblMessage.Text = "Passwords do not match.";

        }
	}
}
