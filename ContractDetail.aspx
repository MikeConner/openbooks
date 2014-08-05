<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="ContractDetail.aspx.cs" Inherits="ContractDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    
<div class="about">
<div class="row">
<div class="large-12 columns">
<div class="panel">
	<div class="gridboxhead">
		<div class="gridboxleft"><h2>Contract Details</h2></div>
		<div class="gridboxright"></div>
	</div>

<asp:Repeater ID="rptContractDetails" runat="server" onitemcommand="rptContracts_ItemCommand">
	<ItemTemplate>
		<table class="ob-gridview" cellpadding="0" cellspacing="0">
			<tr>
				<th>Amount</th>
				<th>Original Amount</th>
				<th>Contract Description</th>
				<th>Contract Approval Date</th>
				<th>Contract End Date</th>
			</tr>
			<tr>
				<td><%# DataBinder.Eval(Container.DataItem, "Amount", "{0:C}")%></td>
				<td><%# DataBinder.Eval(Container.DataItem, "OriginalAmount", "{0:C}")%></td>
				<td><%# DataBinder.Eval(Container.DataItem, "Description") %></td>
				<td><%# DataBinder.Eval(Container.DataItem, "DateCountersigned", "{0:MM/dd/yyyy}")%></td>
				<td><%# DataBinder.Eval(Container.DataItem, "DateDuration", "{0:MM/dd/yyyy}")%></td>
			</tr>
		</table>
	<div class="bottomnav">		
		<div class="bottomnavbtns"></div>
	</div>
<br />
	<div class="contractdetails">
		<table cellpadding="0" cellspacing="0">
			<tr>
				<td><label>Vendor Name: </label></td>
				<td><a href="VendorDetail.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "VendorNo") %>"><%# DataBinder.Eval(Container.DataItem, "VendorName") %></a></td>
			</tr>
			<tr>
				<td><label>City Department: </label></td>
				<td><%# DataBinder.Eval(Container.DataItem, "DeptName") %></td>
			</tr>
			<tr>
				<td><label>Contract No.: </label></td>
				<td><%# DataBinder.Eval(Container.DataItem, "ContractID") %></td>
			</tr>
			<tr>
				<td><label>Supplement No.: </label></td>
				<td><%# DataBinder.Eval(Container.DataItem, "SupplementalNo") %></td>
			</tr>
			<tr>
				<td><label>Resolution No.: </label></td>
				<td><%# DataBinder.Eval(Container.DataItem, "ResolutionNo") %></td>
			</tr>
			<tr>
				<td><label>Comments: </label></td>
				<td><%# DataBinder.Eval(Container.DataItem, "Comments") %></td>
			</tr>
			<tr>
				<td><label>Contract Type: </label></td>
				<td><%# DataBinder.Eval(Container.DataItem, "ServiceName") %></td>
			</tr>
			<tr>
				<td >
				<asp:Panel ID="pnlContractPDF" runat="server" Visible='<%# Eval("HasPDF").ToString() == "True" %>'>
					<asp:Button ID="ibtnContractPDF" runat="server" 
						class = "button submit"
                        text ="View Contract"
						CommandName="ViewPDF" 
						CommandArgument='<%# Eval("ContractID") %>' />
				</asp:Panel>
				</td>
			</tr>
		</table>
	</div>
	</ItemTemplate>
</asp:Repeater>



<br />
<br />

</div>
    </div>
</asp:Content>

