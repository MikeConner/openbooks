<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="Contracts.aspx.cs" 
Inherits="Admin_Contracts_new" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="about">

<div class="large-12 columns">
	<div class="gridboxhead">
		<div class="gridboxleft"><h2>Admin :: Contracts</h2></div>
		<div class="gridboxright"></div>
	</div>
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
	<div class="dropboxadmin">
		<asp:DropDownList ID="ddlVendors" runat="server" 
			DataTextField="VendorName" 
			DataValueField="VendorNo"
			AutoPostBack="true" 
			AppendDataBoundItems="true" 
			onselectedindexchanged="ddlVendors_SelectedIndexChanged" Width="300px">
				<asp:ListItem Text="-- vendor --" Value="0" />
		</asp:DropDownList>
		<asp:DropDownList ID="ddlContractNos" runat="server" 
			DataTextField="ContractID" 
			DataValueField="ContractID"
			AutoPostBack="true" 
			AppendDataBoundItems="true" 
			onselectedindexchanged="ddlContractNos_SelectedIndexChanged">
				<asp:ListItem Text="-- contract # --" Value="0" />
		</asp:DropDownList>
	</div>
		<table class="ob-gridview" cellpadding="0" cellspacing="0">
			<tr>
				<th></th>
				<th><asp:LinkButton ID="LinkButton1" Text="Vendor&nbsp;Name" OnClick="sortVendor" runat="server" /></th>
				<th><asp:LinkButton ID="LinkButton7" Text="Second Vendor&nbsp;Name" runat="server" /></th>
				<th><asp:LinkButton ID="LinkButton2" Text="Agency&nbsp;Name" OnClick="sortAgency" runat="server" /></th>
				<th><asp:LinkButton ID="LinkButton3" Text="Contract&nbsp;#" OnClick="sortContractID" runat="server" /></th>
				<th><asp:LinkButton ID="LinkButton4" Text="Amount" OnClick="sortAmount" runat="server" /></th>
				<th><asp:LinkButton ID="LinkButton5" Text="Original<br/>Amount" OnClick="sortOriginalAmount" runat="server" /></th>
				<th><asp:LinkButton ID="LinkButton6" Text="Contract&nbsp;Description" OnClick="sortDescription" runat="server" /></th>
				<th><asp:LinkButton ID="LinkButton9" Text="Contract&nbsp;Type" OnClick="sortContractType" runat="server" /></th>
				<th><asp:LinkButton ID="LinkButton10" Text="Approval&nbsp;Date" OnClick="sortApprovalDate" runat="server" /></th>
				<th><asp:LinkButton ID="LinkButton8" Text="End&nbsp;Date" OnClick="sortEndDate" runat="server" /></th>
			</tr>

<asp:Repeater ID="rptContracts" runat="server" 
	onitemcommand="rptContracts_ItemCommand">
	<ItemTemplate>
		<tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "even" %>' valign="top">
			<td nowrap>
				<asp:LinkButton ID="lb1" runat="server" 
					CommandArgument='<%# Eval("ContractID") + "@" + Eval("SupplementalNo")%>'
					CommandName="edit"
                    CssClass ="button tiny" 
					Text="edit" /> 
				<asp:LinkButton ID="lb2" runat="server" 
					CommandArgument='<%# Eval("ContractID") + "@" + Eval("SupplementalNo")%>' 
					CommandName="delete" 
                    CssClass ="button tiny" 
					OnClientClick="javascript:if(!confirm('Delete this item?'))return false;"
					Text="delete" /> 
				<asp:LinkButton ID="lb3" runat="server" 
					CommandArgument='<%# Eval("ContractID") + "@" + Eval("SupplementalNo")%>' 
                    CssClass ="button tiny" 
					CommandName="view" 
					Text="view" />
				
			</td>
			<td class="vendor"><%# DataBinder.Eval(Container.DataItem, "VendorName") %></td>
			<td class="vendor"><%# DataBinder.Eval(Container.DataItem, "SecondVendorName") %></td>
			<td class="agency"><%# DataBinder.Eval(Container.DataItem, "DepartmentName") %></td>
			<td class="contract">
				<%# Eval("SupplementalNo").ToString() == "0" ? Eval("ContractID") : Eval("ContractID") + "." + Eval("SupplementalNo")%>
			
			</td>
			<td class="amount"><%# DataBinder.Eval(Container.DataItem, "Amount", "{0:C}")%></td>
			<td class="oamount"><%# DataBinder.Eval(Container.DataItem, "OriginalAmount", "{0:C}")%></td>
			<td class="description"><%# DataBinder.Eval(Container.DataItem, "Description") %></td>
		    <td class="contracttype"><%# DataBinder.Eval(Container.DataItem, "ServiceName") %></td>
			<td class="approvaldate"><%# DataBinder.Eval(Container.DataItem, "DateCountersigned", "{0:MM/dd/yyyy}")%></td>
			<td class="date"><%# DataBinder.Eval(Container.DataItem, "DateDuration", "{0:MM/dd/yyyy}")%></td>
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
</div></div>


</asp:Content>

