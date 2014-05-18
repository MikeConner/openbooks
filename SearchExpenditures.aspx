<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="SearchExpenditures.aspx.cs" 
Inherits="SearchExpendituresPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:Panel ID="search" Visible="true" runat="server">
<div id="searchboxcontainer">
	<div class="searchboxnav">
		<ul>
			<li><a href="SearchExpenditures.aspx"><span>Expenditures</span></a></li>
		</ul>
	</div>
	<div class="searchboxnavactive">
		<ul>
		    <li><a href="SearchContributions.aspx"><span>Contributions</span></a></li>
		</ul>
	</div>
</div>
<div id="searchbox">
	<div class="searchhead"><h2>Campaign Expenditures</h2></div>
	<div class="expendituresboxbody">
	<table cellpadding="0" cellspacing="0">
		<tr>
			<td><label>Candidate Name</label></td>
			<td><asp:DropDownList ID="ddlCandidateName" runat="server" 
					DataSourceID="CandidateDataSource" 
					DataTextField="CandidateName" 
					DataValueField="ID" 
					AppendDataBoundItems="true">
					<asp:ListItem Text="All" Value="0" />
				</asp:DropDownList>
   				<asp:SqlDataSource ID="CandidateDataSource" runat="server" 
					ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
					SelectCommand="SELECT [ID], [CandidateName] FROM [tlk_candidate] ORDER BY CandidateName ASC">
				</asp:SqlDataSource>
			</td>
		</tr>
		<tr>
			<td><label>Vendor</label></td>
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
			<td><label>Keywords</label></td>
			<td><asp:TextBox ID="txtKeywords" runat="server" Width="200" /></td>
		</tr>
	</table>
	 <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="Search" onclick="Button1_Click" CssClass="button" ImageUrl="~/img/searchbutton.gif"/>
</div>
</div>

</asp:Panel>

</asp:Content>

