<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="Admin_Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="about">

<div class="large-12 columns">
<div class="gridboxhead">
<div class="gridboxleft">
<h2>Admin :: Users</h2>
</div>
<div class="gridboxright">
</div>
</div>

<asp:Panel ID="pnlAdmin" Visible="false" runat="server">
	<table class="ob-gridview" cellpadding="0" cellspacing="0">
		<tr>
			<th></th>
			<th>Name</th>
			<th>Email</th>
			<th>Initials</th>
			<th>Group</th>
			<th>UserName</th>
			<th></th>
		</tr>

	<asp:Repeater ID="rptUsers" runat="server" 
		onitemcommand="rptUsers_ItemCommand">
		<ItemTemplate>
		<tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "even" %>' valign="top">
			<td>
				<asp:LinkButton ID="LinkButton1" runat="server" 
					CommandArgument='<%# DataBinder.Eval(Container.DataItem, "UserID") %>' 
					CommandName="delete" 
                    CssClass =" tiny button"
					OnClientClick="javascript:if(!confirm('Delete this item?'))return false;" 
					Text="delete" /> 
			</td>
			<td><%# DataBinder.Eval(Container.DataItem, "FirstName") %> <%# DataBinder.Eval(Container.DataItem, "LastName") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "Email") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "Initials") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "PermissionGroup") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "UserName") %></td>
			<td><asp:LinkButton ID="lb1" runat="server" 
					CommandArgument='<%# DataBinder.Eval(Container.DataItem, "UserID") %>' 
					CommandName="reset" 
                CssClass ="tiny button"
					Text="reset password" />
			</td>
		</tr>
		</ItemTemplate>
	</asp:Repeater>
	</table>

</asp:Panel>

</div></div>


</asp:Content>

