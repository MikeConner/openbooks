<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="ReportFraud.aspx.cs" Inherits="ReportFraud" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    
<div class="contact">
<div class="row">
<div class="large-12 columns">


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
     




     
<hr>
</div>
</div>
<div class="row">
<div class="large-4 columns">
<label>
First Name
   <asp:TextBox ID="FNameTB" runat="server" />
<asp:RequiredFieldValidator ID="rfvFName" runat="server" ControlToValidate="FNameTB" ErrorMessage="First Name is required" Display="Dynamic" />
</label>
</div>
<div class="large-4 columns">
<label>
Last Name
<asp:TextBox ID="LNameTB" runat="server" />
<asp:RequiredFieldValidator ID="rfvLName" runat="server" ControlToValidate="LNameTB" ErrorMessage="Last Name is required" Display="Dynamic" />
</label>
</div>
<div class="large-4 columns">
<div class="row collapse">
<label>Email Address</label>
<div class="small-12 columns">
   <asp:TextBox ID="EmailTB" runat="server" />
<asp:RegularExpressionValidator ID="revEmail" runat="server" ValidationExpression=".*@.*\..*"
 ControlToValidate="EmailTB" ErrorMessage="Not a valid Email Address" Display="Dynamic" />
 
</div>

</div>


</div>



<div class ="large-12 columns">

    
         <label>
Location of Fraud, Waste or Abuse:
     
 </label>
<asp:TextBox ID="LocationF" runat="server" />
<asp:RequiredFieldValidator ID="revLocationF" runat="server" ControlToValidate="LocationF" ErrorMessage="Location is required" Display="Dynamic" />
</div>


<div class="large-12 columns">
<label>
Description of Fraudulent Act:
</label>

    <asp:TextBox ID="CommentsTB" runat="server" TextMode="MultiLine" />
<asp:RequiredFieldValidator ID="rfvComments" runat="server" ControlToValidate="CommentsTB" ErrorMessage="Description is required" Display="Dynamic" />
 

    Please Enter the Code Below:
   <asp:CustomValidator ID="validator" runat="server" ControlToValidate="txtVerify" ErrorMessage="You have Entered a Wrong Verification Code! Please Re-Enter and Try Again!" OnServerValidate="CAPTCHAValidate"></asp:CustomValidator> <br />     
   <asp:Image ID="imCaptcha" ImageUrl="Captcha.ashx" runat="server" /><br />
   <asp:TextBox ID="txtVerify" runat="server"></asp:TextBox><br /><br />     
   
   <asp:Button ID="btnSubmit" runat="server" AlternateText="Submit" Text="Submit" onclick="SendMail"    CssClass="button submit"/>

 

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

