using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Mail; 
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
			MailMessage mail = new MailMessage();
			mail.From = new MailAddress("webcontact@openbookpittsburgh.com");
			mail.To.Add("douglas.anderson@pittsburghpa.gov");
			mail.Subject = "Contact Form From OpenBook Pittsburgh";
			mail.IsBodyHtml = true;
			mail.Body = "First Name: " + FNameTB.Text + "<br />";
			mail.Body += "Last Name: " + LNameTB.Text + "<br />";
			mail.Body += "Email: " + EmailTB.Text + "<br />";
			mail.Body += "Comments: " + CommentsTB.Text + "<br />"; 

			SmtpClient smtp = new SmtpClient();
			smtp.Host = "smtp-apps.apps.pittsburghpa.gov";
			smtp.Credentials = new System.Net.NetworkCredential("zeoapp", "Zeoapp-SMTP-Relay");
			//smtp.Host = "72.18.138.246";
			//smtp.Credentials = new System.Net.NetworkCredential("webcontact@openbookpittsburgh.com", "Co5V63PSKndeMUq2fw84");
			smtp.Send(mail);

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
