<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="Contracts.aspx.cs" Inherits="SearchResults" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<div class="mainwrap">
	<div class="gridboxhead"><h2>Contract Search Results</h2></div>
	<div class="results">
		<div class="resultsleft">
			<asp:Label ID="lblPageSize" runat="server" Text="View:" />
			<asp:DropDownList ID="ddlPageSize" runat="server"  
				OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged"
				AutoPostBack="true">
					<asp:ListItem Text="10 per page" Value="10" />
					<asp:ListItem Text="25 per page" Value="25" />
					<asp:ListItem Text="50 per page" Value="50"  />
					<asp:ListItem Text="100 per page" Value="100" />
			</asp:DropDownList>
		</div>
		<div class="resultsright">
			<asp:Label ID="lblCurrentPage" runat="server" />
		</div>
	</div>

	<table class="ob-gridview" cellpadding="0" cellspacing="0">
		<tr>
			<th><asp:LinkButton ID="LinkButton1" Text="Vendor&nbsp;Name" OnClick="sortVendor" runat="server" /><asp:Image ID="imgSortVendor" runat="server" /></th>
			<th><asp:LinkButton ID="LinkButton2" Text="Agency&nbsp;Name" OnClick="sortAgency" runat="server" /><asp:Image ID="imgSortAgency" runat="server" /></th>
			<th><asp:LinkButton ID="LinkButton3" Text="Contract&nbsp;#" OnClick="sortContractID" runat="server" /><asp:Image ID="imgSortContractID" runat="server" /></th>
			<th><asp:LinkButton ID="LinkButton4" Text="Amount" OnClick="sortAmount" runat="server" /><asp:Image ID="imgSortAmount" runat="server" /></th>
			<th><asp:LinkButton ID="LinkButton5" Text="Original<br/>Amount" OnClick="sortOriginalAmount" runat="server" /><asp:Image ID="imgSortOriginalAmount" runat="server" /></th>
			<th><asp:LinkButton ID="LinkButton6" Text="Contract&nbsp;Description" OnClick="sortDescription" runat="server" /><asp:Image ID="imgSortDescription" runat="server" /></th>
			<th><asp:LinkButton ID="LinkButton9" Text="Contract&nbsp;Type" OnClick="sortContractType" runat="server" /><asp:Image ID="imgSortContractType" runat="server" /></th>
			<th><asp:LinkButton ID="LinkButton10" Text="Contract Approval Date" OnClick="sortApprovalDate" runat="server" /><asp:Image ID="imgSortApproval" runat="server" /></th>
			<th><asp:LinkButton ID="LinkButton8" Text="Contract End Date" OnClick="sortEndDate" runat="server" /><asp:Image ID="imgSortEndDate" runat="server" /></th>
			<th></th>
		</tr>

	<asp:Repeater ID="rptContracts" runat="server" 
			onitemcommand="rptContracts_ItemCommand">
	<ItemTemplate>
		<tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "even" %>' valign="top">
			<td class="vendor"><a href="VendorDetail.aspx?ID=<%# Eval("VendorNo") %>"><%# Eval("VendorName") %></a></td>
			<td class="agency"><%# Eval("DepartmentName") %></td>
			<td class="contract">
				<a href="ContractDetail.aspx?ID=<%# Eval("ContractID") %>&sup=<%# Eval("SupplementalNo") %>">
					<%# Eval("SupplementalNo").ToString() == "0" ? Eval("ContractID") : Eval("ContractID") + "." + Eval("SupplementalNo")%>
				</a>
			</td>
			<td class="amount"><%# Eval("Amount", "{0:C}")%></td>
			<td class="oamount"><%# Eval("OriginalAmount", "{0:C}")%></td>
			<td class="description"><%# Eval("Description") %></td>
		    <td class="contracttype"><%# Eval("ServiceName") %></td>
			<td class="approvaldate"><%# Eval("DateCountersigned", "{0:MM/dd/yyyy}")%></td>
			<td class="date"><%# Eval("DateDuration", "{0:MM/dd/yyyy}")%></td>
			<td valign="middle" style="padding-right: 5px;">
				<asp:Panel ID="pnlContractPDF" runat="server" Visible='<%# Eval("HasPDF").ToString() == "True" %>'>
					<asp:ImageButton ID="ibtnContractPDF" runat="server" 
						ImageUrl="~/img/pdficon.gif"
						CommandName="ViewPDF" 
						CommandArgument='<%# Eval("ContractID") %>' />
				</asp:Panel>
			</td>
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
</asp:Content>

