<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="ReportFraud.aspx.cs" Inherits="ReportFraud" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="mainpagebox">
<div class="infoleftnav">
<ul>
<li><asp:HyperLink NavigateUrl="~/About.aspx" runat="server" ID="HyperLink1">About</asp:HyperLink></li>
<li><asp:HyperLink NavigateUrl="~/SearchTips.aspx" runat="server" ID="HyperLink2">Search Tips</asp:HyperLink></li>
<li><asp:HyperLink NavigateUrl="~/ReportFraud.aspx" runat="server" ID="HyperLink10">Report Fraud</asp:HyperLink></li>
<li><asp:HyperLink NavigateUrl="~/Contact.aspx" runat="server" ID="HyperLink4">Contact</asp:HyperLink></li>
</ul>

</div>

<div class="inforightcontent">
<h3>Report Fraud or Waste</h3>
<p>Report waste, fraud, mismanagement of your city tax dollars. Use the below form to report allegations of: </p>
	<ul>
		<li>Inefficiency</li>
		<li>Waste</li>
		<li>Corruption</li>
		<li>Mismanagement</li>
		<li>Abuse of city tax dollars</li>
	</ul>
<p>Complete the form below with as much information as possible. Be assured that your information will be held in the strictest of confidence. When you complete the form, click submit to send the information. If you have problems completing the form, or wish to speak with someone in person, please call at 412-255-4777.</p>
<p>This hotline works for you!</p><br />
<table id="contact" cellspacing="0">
<tr>
<td>First Name:<br />
<asp:TextBox ID="FNameTB" runat="server" />
</td>
</tr>
<tr>
<td>Last Name:<br />
<asp:TextBox ID="LNameTB" runat="server" />
</td>
</tr>
<tr>
<td>Email:<br />
<asp:TextBox ID="EmailTB" runat="server" />
<asp:RegularExpressionValidator ID="revEmail" runat="server" ValidationExpression=".*@.*\..*"
 ControlToValidate="EmailTB" ErrorMessage="Not a valid Email Address" Display="Dynamic" />
</td>
</tr>
<tr>
<td>Location of Fraud, Waste or Abuse:<br />
<asp:TextBox ID="LocationF" runat="server" />
<asp:RequiredFieldValidator ID="revLocationF" runat="server" ControlToValidate="LocationF" ErrorMessage="Location is required" Display="Dynamic" />
</td>
</tr>
<tr>
<td>Description of Fraudulent Act:<br />
<asp:TextBox ID="CommentsTB" runat="server" TextMode="MultiLine" Width="300px" Height="100px"/>
<asp:RequiredFieldValidator ID="rfvComments" runat="server" ControlToValidate="CommentsTB" ErrorMessage="Description is required" Display="Dynamic" />
</td>
</tr>                                                                                                                                         
</table>
<table><tr><td>Please Enter the Code Below:</td></tr></table>
<div><asp:CustomValidator ID="validator" runat="server" ControlToValidate="txtVerify" ErrorMessage="You have Entered a Wrong Verification Code! Please Re-Enter and Try Again!" OnServerValidate="CAPTCHAValidate"></asp:CustomValidator> <br />     
   <asp:Image ID="imCaptcha" ImageUrl="Captcha.ashx" runat="server" /><br />
   <asp:TextBox ID="txtVerify" runat="server"></asp:TextBox><br /><br />     
   <asp:ImageButton ID="btnSubmit" runat="server" AlternateText="Submit" onclick="SendMail"  ImageUrl="~/img/submitbtn.gif" CssClass="contactbtn"/>
</div>
<asp:PlaceHolder ID="formPH" runat="server" Visible="true">
</asp:PlaceHolder>
<asp:PlaceHolder ID="sucessPH" runat="server" Visible="false">
<p>Thank you for contacting Open Book Pittsburgh.</p>
</asp:PlaceHolder>
<br />
<p><b>To mail your complaint:</b><br />

City Controller's Office<br />
414 Grant St.<br />
Pittsburgh, PA 15219<br /><br />

>Face to face meetings are also available; please call 412-255-4777 to schedule an appointment.
</p>
<p><b>Investigation</b></p>
<p>The City Controller’s Office will fully investigate any suspected acts of fraud, abuse, or illegal acts.  An objective and impartial investigation will be conducted regardless of the position, title, or relationship with the City of any party who might be involved. </p>
<p>The Controller’s Office will maintain the confidentiality of any person who in good faith reports a suspected act of fraud.  The person’s identity will not be divulged without permission, unless required by law. </p>
<p>It is not required that you identify yourself.  However, without a means to contact you with follow-up questions, the investigation could come to a dead end.
</p>
</div>
</div>
</asp:Content>

