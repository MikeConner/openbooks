<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="Vendors.aspx.cs" Inherits="Admin_Vendors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="about">

<div class="large-12 columns">
<div class="gridboxhead">
<div class="gridboxleft">
<h2>Admin :: Vendors</h2>
</div>
<div class="gridboxright">
</div>
</div>
<div class="results">
<asp:Label ID="lblCurrentPage" runat="server" Text="Label" />
</div>
<div class="dropboxadmin">
	<asp:DropDownList ID="ddlVendors" runat="server" 
		DataTextField="VendorName" 
		DataValueField="VendorNo"
		AutoPostBack="true" 
		AppendDataBoundItems="true" 
		onselectedindexchanged="ddlVendors_SelectedIndexChanged" Width="300px">
			<asp:ListItem Text="-- vendor --" Value="0" />
	</asp:DropDownList>
</div>
		<table class="ob-gridview" cellpadding="0" cellspacing="0">
			<tr>
				<th></th>
				<th><asp:LinkButton ID="LinkButton1" Text="Vendor&nbsp;Name" OnClick="sortVendorName" runat="server" /></th>
				<th><asp:LinkButton ID="LinkButton2" Text="Vendor&nbsp;#" OnClick="sortVendorNo" runat="server" /></th>
				<th>Address</th>
				<th>City</th>
				<th>State</th>
				<th>Zip</th>
			</tr>

<asp:Repeater ID="rptVendors" runat="server" 
	onitemcommand="rptVendors_ItemCommand">
	<ItemTemplate>
		<tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "even" %>' valign="top">
			<td>
				<asp:LinkButton ID="lb1" runat="server" 
					CommandArgument='<%# DataBinder.Eval(Container.DataItem, "VendorNo") %>' 
					CommandName="edit" 
                    CssClass ="tiny button"
					Text="edit" /> 
			</td>
			<td><%# DataBinder.Eval(Container.DataItem, "VendorName") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "VendorNo") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "Address1") %><br /><%# DataBinder.Eval(Container.DataItem, "Address2") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "City") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "State") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "Zip") %></td>
		</tr>
	</ItemTemplate>
</asp:Repeater>
   
</table>
   <div class="bottomnav">
    <div class="bottomnavbtns">
        <asp:ImageButton ID="ibtnFirstPageTop" runat="server" OnClick="FirstPage_Click" ImageUrl="~/img/firstbtn.gif" />
        <asp:ImageButton ID="ibtnPrevPageTop" runat="server" OnClick="PrevPage_Click" ImageUrl="~/img/previousbtn.gif" />
        <asp:ImageButton ID="ibtnNextPageTop" runat="server" OnClick="NextPage_Click" ImageUrl="~/img/nextbtn.gif" />
        <asp:ImageButton ID="ibtnLastPageTop" runat="server" OnClick="LastPage_Click" ImageUrl="~/img/lastbtn.gif" />
    </div>
    </div>


    </div>
</div>
</asp:Content>

