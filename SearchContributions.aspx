<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" 
AutoEventWireup="true" CodeFile="SearchContributions.aspx.cs" 
Inherits="_SearchContributionsPageClass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:Panel ID="search" Visible="true" runat="server">

<div id="searchboxcontainer">
	<div class="searchboxnavactive">
		<ul>
		    <li><a href="SearchExpenditures.aspx"><span>Expenditures</span></a></li>
		</ul>
	</div>
	<div class="searchboxnav">
		<ul>
			<li><a href="SearchContributions.aspx"><span>Contributions</span></a></li>   
		</ul>
	</div>
</div>
<div id="searchbox">
	<div class="searchhead"><h2>Campaign Contributions</h2></div>
	<div class="contributionboxbody">
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
			<td><label>Office</label></td>
			<td><asp:DropDownList ID="ddlOffice" runat="server">
					<asp:ListItem Text="All" Value="all" />
					<asp:ListItem Text="Mayor" Value="mayor" />
					<asp:ListItem Text="City Council" Value="council" />
					<asp:ListItem Text="City Controller" Value="controller" />
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
		   <td><label>Year</label></td>
		   <td>
		   <asp:DropDownList ID="ddldateContribution" runat="server" 
					DataSourceID="SqlDataSource3" 
					DataTextField="yearName" 
					DataValueField="yearValue" /> 
					
				<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
					ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
					SelectCommand="SELECT [yearName], [yearValue] FROM [tlk_year]">
				</asp:SqlDataSource>		
		   
		   </td>
		</tr>
		<tr>
			<td><label>Search By</label></td>
			<td>
				<asp:RadioButtonList ID="rblContributorSearch" runat="server" RepeatDirection="Horizontal">
					<asp:ListItem Text="Commitee" Value="co" Selected="True" />
					<asp:ListItem Text="Contributor" Value="in" />
				</asp:RadioButtonList>

				<asp:TextBox ID="txtContributor" runat="server" Width="200" />
			</td>
		</tr>

		<tr>
			<td><label>Employer</label></td>
			<td><asp:TextBox ID="txtEmployer" runat="server" Width="200" /></td>
		</tr>
		<tr>
			<td><label>Distance within</label></td>
			<td>
				<asp:DropDownList ID="ddlDistance" runat="server">
					<asp:ListItem Text="Any" Value="0" />
					<asp:ListItem Text="50 miles" Value="50" />
					<asp:ListItem Text="30 miles" Value="30" />
					<asp:ListItem Text="20 miles" Value="20" />
					<asp:ListItem Text="10 miles" Value="10" />
					<asp:ListItem Text="5 miles" Value="5" />				
				</asp:DropDownList>
				 of zip code 
				 <asp:TextBox ID="txtZip" Width="50" MaxLength="5" runat="server" />
			</td>
		</tr>
	</table>
 <asp:ImageButton ID="Button1" runat="server" Text="Search" onclick="Button1_Click" CssClass="button" ImageUrl="~/img/searchbutton.gif"/>

  
</div>
   
</div>


</asp:Panel>

</asp:Content>

