using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Mail;
using OpenBookAllegheny;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.SessionState;
using System.Web.UI.HtmlControls;

public partial class ReportFraud : System.Web.UI.Page
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
			MailMessage mail = new MailMessage();
			mail.From = new MailAddress("webcontact@openbookallegheny.com");
			mail.To.Add("arash@tapnology.co");
			mail.Subject = "Fraud or Waste Reported via OpenBookAllegheny.com";
			mail.IsBodyHtml = true;
			mail.Body = "First Name: " + FNameTB.Text + "<br />";
			mail.Body += "Last Name: " + LNameTB.Text + "<br />";
			mail.Body += "Email: " + EmailTB.Text + "<br />";
			mail.Body += "Location of Fraude, Waste or Abuse: " + LocationF.Text + "<br />";
			mail.Body += "Description of Fraudulent Act: " + CommentsTB.Text + "<br />";

            Admin.GetGmailClient().Send(mail);
            
			formPH.Visible = false;
			sucessPH.Visible = true;
		}
	}

	public void SetVerificationText()
	{
		Random ran = new Random();
		int no = ran.Next();
		Session["Captcha"] = no.ToString();
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
