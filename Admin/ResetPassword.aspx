<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="ResetPassword.aspx.cs" Inherits="Admin_ResetPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="about">
<div class="row">
<div class="large-12 columns">
<div class="contractdetails">
<h2>Reset Password</h2>

	<table cellpadding="0" cellspacing="0">
		<tr>
			<td><label>New Password </label></td>
			<td><asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="200" />
			
				<asp:RequiredFieldValidator id="PasswordValidator" runat="server" 
					ControlToValidate="txtPassword" 
					ErrorMessage="[error] Required Field. " 
					Display="Dynamic" />
				
			</td>
		</tr>
		<tr>
			<td><label>Confirm New Password </label></td>
			<td><asp:TextBox ID="txtPassword2" runat="server" TextMode="Password" Width="200" />
			
				<asp:RequiredFieldValidator id="PasswordValidator2" runat="server" 
					ControlToValidate="txtPassword2" 
					ErrorMessage="[error] Required Field. " 
					Display="Dynamic" />
				
			</td>
		</tr>
	</table>
	<br />
	<asp:ImageButton ID="Button1" runat="server" OnClick="Button1_Click" ImageUrl="~/img/savebtn.gif" />

	<asp:Label ID="lblMessage" runat="server" />
</div>
</div></div></div>
</asp:Content>

