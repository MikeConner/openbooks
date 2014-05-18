<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="Contact.aspx.cs" 
Inherits="_Contact" %>

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
<h3>Open Book Contact Information</h3>
<p><b>Controller's Office
</b><br />
414 Grant Street<br />
Pittsburgh, PA  15219<br />
Phone: (412) 255-2054 </p>
<p><b>Contact Form</b></p>
<table id="contact" cellspacing="0">
<tr>
<td>First Name:</td>
<td><asp:TextBox ID="FNameTB" runat="server" />
<asp:RequiredFieldValidator ID="rfvFName" runat="server" ControlToValidate="FNameTB" ErrorMessage="First Name is required" Display="Dynamic" />
</td>
</tr>
<tr>
<td>Last Name:</td>
<td><asp:TextBox ID="LNameTB" runat="server" />
<asp:RequiredFieldValidator ID="rfvLName" runat="server" ControlToValidate="LNameTB" ErrorMessage="Last Name is required" Display="Dynamic" />
</td>
</tr>
<tr>
<td>Email:</td>
<td><asp:TextBox ID="EmailTB" runat="server" />
<asp:RegularExpressionValidator ID="revEmail" runat="server" ValidationExpression=".*@.*\..*"
 ControlToValidate="EmailTB" ErrorMessage="Not a valid Email Address" Display="Dynamic" />
</td>
</tr>
<tr>
<td>Comments:</td>
<td><asp:TextBox ID="CommentsTB" runat="server" TextMode="MultiLine" Width="300px" Height="100px"/>
<asp:RequiredFieldValidator ID="rfvComments" runat="server" ControlToValidate="CommentsTB" ErrorMessage="Comments are required" Display="Dynamic" />
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
</div>
</div>
</asp:Content>
