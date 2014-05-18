<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="Admin_login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="mainwrap">
 <div class="contractdetails">
<p><b>Username:</b><br /><asp:TextBox id="txtUserName" runat="server" /></p>
<p><b>Password:</b><br /><asp:TextBox id="txtPassword" runat="server" TextMode="Password" EnableViewState="false" /></p>
<p></p>

<asp:ImageButton ID="btnLogin" runat="server" OnClick="btnLogin_Click" ImageUrl="~/img/loginbtn.gif" />

<asp:Label ID="lblMessage" runat="server" />
</div>
</div>
</asp:Content>

