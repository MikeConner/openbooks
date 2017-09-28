using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using OpenBookPgh;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.SessionState;
using System.Web.UI.HtmlControls;


public partial class _Contact : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetVerificationText();
        }
    }
    protected void SendMail(object sender, EventArgs e) 
	{ 
		if (!IsValid) 
		{ 
			return; 
		} 
		else 
		{ 
			string body = "First Name: " + FNameTB.Text + "<br />" +
			              "Last Name: " + LNameTB.Text + "<br />" +
			              "Email: " + EmailTB.Text + "<br />" +
			              "Comments: " + CommentsTB.Text + "<br />";

            Admin.SendMail("douglas.anderson@pittsburghpa.gov", null, "Contact Form From OpenBook Pittsburgh", body);
            Admin.SendMail("mark.ptak@pittsburghpa.gov", null, "Contact Form From OpenBook Pittsburgh", body);
           
			formPH.Visible = false;
			sucessPH.Visible = true;
		} 
    }

	public void SetVerificationText()
	{
         Random oRandom = new Random();
         int iNumber = oRandom.Next(100000, 999999);
         Session["Captcha"] = iNumber.ToString();
	}

    protected void CAPTCHAValidate(object source, ServerValidateEventArgs args)
    {
		if (Session["Captcha"] != null)
		{
			if (txtVerify.Text != Session["Captcha"].ToString())
			{
				SetVerificationText();
				args.IsValid = false;
				return;
			}
		}
		else
		{
			SetVerificationText();
			args.IsValid = false;
			return;
		}
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
		if (!Page.IsValid)
		{
			return;
		}
        //Save the content
        Response.Write("You are not a SPAMMER!!!");
        SetVerificationText();
    }
}
