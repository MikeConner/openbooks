<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="mainwrap">
<div class="contractdetails">
<h3 class="admin">Welcome to the Open Book Administration Section</h3>
<p>To get started click on one of the above links.</p>
<br />
<br />
<asp:Button ID="lbtnLogout" runat="server" OnClick="lbtnLogout_Click" Text ="Logout" CssClass =" button submit"/>
</div>
</div>
</asp:Content>

