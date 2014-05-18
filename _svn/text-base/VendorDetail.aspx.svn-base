<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="VendorDetail.aspx.cs" Inherits="VendorDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<div class="contractdetails">
<asp:Repeater ID="rptCompanyInfo" runat="server">
	<HeaderTemplate>
		<div class="companyinfo">	
	</HeaderTemplate>
	<ItemTemplate>
		<h3><%# DataBinder.Eval(Container.DataItem, "VendorName") %></h3>
		<table cellpadding="0" cellspacing="0">
			<tr>
				<td><%# DataBinder.Eval(Container.DataItem, "Address1") %></td>
			</tr>		
			<tr>
				<td><%# DataBinder.Eval(Container.DataItem, "Address2") %></td>
			</tr>	
			<tr>
				<td><%# DataBinder.Eval(Container.DataItem, "Address3") %></td>
			</tr>
			<tr>
				<td><%# DataBinder.Eval(Container.DataItem, "City") %>, 
					<%# DataBinder.Eval(Container.DataItem, "State") %> 
					<%# DataBinder.Eval(Container.DataItem, "Zip") %>
				</td>
			</tr>			
		</table>
	</ItemTemplate>
	<FooterTemplate>
	</FooterTemplate>
	</asp:Repeater>
</div>
<br />
<br />
<div class="gridboxhead">
<h2>Contracts</h2>
</div>
<div class="results">
<label>Results</label>
</div>
<asp:Repeater ID="rptVendorDetails" runat="server">
	<HeaderTemplate>
	<table class="ob-gridview" cellpadding="0" cellspacing="0">
		<tr>
			<th>Contract #</th>
			<th>Amount</th>
			<th>Original Amount</th>
			<th>Contract Description</th>
			<th>Contract Approval Date</th>
			<th>Contract End Date</th>
		</tr>
	</HeaderTemplate>
	<ItemTemplate>
		<tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "even" %>' valign="top">
			<td>
				<a href="ContractDetail.aspx?ID=<%# Eval("ContractID") %>&sup=<%# Eval("SupplementalNo") %>">
					<%# Eval("SupplementalNo").ToString() == "0" ? Eval("ContractID") : Eval("ContractID") + "." + Eval("SupplementalNo")%>
				</a>
			</td>
			<td><%# DataBinder.Eval(Container.DataItem, "Amount", "{0:C}") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "OriginalAmount", "{0:C}") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "Description") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "DateCountersigned", "{0:MM/dd/yyyy}")%></td>
			<td><%# DataBinder.Eval(Container.DataItem, "DateDuration", "{0:MM/dd/yyyy}")%></td>
		</tr>

	</ItemTemplate>
	<FooterTemplate>
	</table>
<div class="bottomnav">		
<div class="bottomnavbtns">
</div>
</div>
</div>	
			
	</FooterTemplate>
</asp:Repeater>

</asp:Content>

