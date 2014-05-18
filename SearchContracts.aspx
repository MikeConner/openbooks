﻿<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="SearchContracts.aspx.cs" 
Inherits="SearchContractsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="searchbox">
	<div class="searchhead"><h2>Search City Contracts</h2></div>
	<div class="searchboxbody">
	<table cellpadding="0" cellspacing="0">
		<tr>
			<td><label>City Department</label></td>
			<td><asp:DropDownList ID="ddlDepartment" runat="server" 
					DataSourceID="SqlDataSource4" 
					DataTextField="DeptName" 
					DataValueField="DeptCode" 
					AppendDataBoundItems="true">
					<asp:ListItem Text="All Organizations" Value="0" />
				</asp:DropDownList>
				<asp:SqlDataSource ID="SqlDataSource4" runat="server" 
					ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
					SelectCommand="SELECT [DeptCode], [DeptName] FROM [tlk_department] ORDER BY DeptName" />
			</td>
		</tr>
		<tr>
			<td ><label>Vendor</label></td>
			<td>
				<asp:RadioButtonList ID="rblVendorSearchOptions" runat="server" RepeatDirection="Horizontal">
					<asp:ListItem Text="Begins with" Value="B" Selected="True" />
					<asp:ListItem Text="Contains" Value="C" />
					<asp:ListItem Text="Exact" Value="E" />
				</asp:RadioButtonList>
				<asp:TextBox ID="txtVendor" runat="server" Width="200" />
			</td>
		</tr>
	
		<tr>
			<td><label>Contract Type</label></td>
			<td><asp:DropDownList ID="ddlServices" runat="server" 
					DataSourceID="SqlDataSource1" 
					DataTextField="ServiceName" 
					DataValueField="ID" 
					AppendDataBoundItems="true">
					<asp:ListItem Text="All Services" Value="0" />
				</asp:DropDownList>
				<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
					ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
					SelectCommand="SELECT [ID], [ServiceName] FROM [tlk_service] ORDER BY ServiceName" />
		
			</td>
		</tr>
		<tr>
			<td><label>Keywords</label></td>
			<td>
				<asp:TextBox ID="txtKeywords" runat="server" Width="200" />
			</td>
		</tr>
		<tr>
            <td colspan="2">
				<label>Original Contract Approval Date</label><br />
				between 
				<asp:DropDownList ID="ddlBeginMonth1" runat="server" 
					DataSourceID="SqlDataSource2" 
					DataTextField="monthName" 
					DataValueField="monthValue" /> 

				<asp:DropDownList ID="ddlBeginYear1" runat="server" 
					DataSourceID="SqlDataSource3" 
					DataTextField="yearName" 
					DataValueField="yearValue" /> 
				and 
				<asp:DropDownList ID="ddlBeginMonth2" runat="server" 
					DataSourceID="SqlDataSource2" 
					DataTextField="monthName" 
					DataValueField="monthValue" /> 
				
				<asp:DropDownList ID="ddlBeginYear2" runat="server" 
					DataSourceID="SqlDataSource3" 
					DataTextField="yearName" 
					DataValueField="yearValue" />
							
				<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
					ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
					SelectCommand="SELECT [monthName], [monthValue] FROM [tlk_month]">
				</asp:SqlDataSource>
				<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
					ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
					SelectCommand="SELECT [yearName], [yearValue] FROM [tlk_year]">
				</asp:SqlDataSource>
				</td>
		</tr>
			<tr>
			<td><label>Contract Amount</label></td>
			<td>
				<asp:DropDownList ID="ddlAmount" runat="server">
					<asp:ListItem Text="All Amounts" Value="0" />
					<asp:ListItem Text="Less than $10,000" Value="10000" />
					<asp:ListItem Text="Between $10,000 and $100,000" Value="100000" />
					<asp:ListItem Text="Between $100,000 and $250,000" Value="250000" />
					<asp:ListItem Text="Between $250,000 and $1,000,0000" Value="1000000" />
					<asp:ListItem Text="$1,000,000 or more" Value="1000001" />
				</asp:DropDownList>
			</td>
		</tr>	
	</table>
 <asp:ImageButton ID="Button1" runat="server" Text="Search" onclick="btnSearch_Click" CssClass="button" ImageUrl="~/img/searchbutton.gif"/>
</div>
</div>

</asp:Content>

