<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="AddUser.aspx.cs" Inherits="Admin_AddUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="about">
<div class="large-12 columns">
<h2>Add New User</h2>
<table cellpadding="0" cellspacing="0">
	<tr>
		<td><asp:Label ID="Label1" runat="server">First Name:</asp:Label></td>
		<td>
			<asp:TextBox ID="first_name" runat="server" />
			<asp:RequiredFieldValidator id="RequiredFieldValidator3" ControlToValidate="first_name" ErrorMessage="Please enter a first name." runat="server" />
		</td>
	</tr>
	<tr>
		<td><asp:Label ID="Label2" runat="server">Last Name:</asp:Label></td>
		<td>
			<asp:TextBox ID="last_name" runat="server" />
			<asp:RequiredFieldValidator id="RequiredFieldValidator4" ControlToValidate="last_name" ErrorMessage="Please enter a last name." runat="server" />
		</td>
	</tr>
	<tr>
		<td><asp:Label ID="Label4" runat="server">Initials:</asp:Label></td>
		<td>
			<asp:TextBox ID="initials" runat="server" Width="50px" />
			<asp:RequiredFieldValidator id="RequiredFieldValidator5" ControlToValidate="initials" ErrorMessage="Please enter your initials." runat="server" />
		</td>
	</tr>
	<tr>
		<td><asp:Label ID="Label3" runat="server">Email:</asp:Label></td>
		<td><asp:TextBox ID="email" runat="server" Width="200px"/></td>
	</tr>
	<tr>
		<td><asp:Label ID="Label5" runat="server">User Name:</asp:Label></td>
		<td>
			<asp:TextBox ID="user_name" runat="server" />
			<asp:RequiredFieldValidator id="RequiredFieldValidator1" ControlToValidate="user_name" ErrorMessage="Please enter a username." runat="server" />
		</td>
	</tr>
	<tr>
		<td><asp:Label ID="Label6" runat="server">Password:</asp:Label></td>
		<td>
			<asp:TextBox ID="passwd" TextMode="Password" runat="server" />
			<asp:RequiredFieldValidator id="RequiredFieldValidator2" ControlToValidate="passwd" ErrorMessage="Please enter a password." runat="server" />
		</td>
	</tr>

</table>

<br />
<asp:ImageButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" ImageUrl="~/img/addbtn.gif" CausesValidation="true"/>

		<asp:Label ID="lblMessage" runat="server" />
		</div>
</div>
</asp:Content>

