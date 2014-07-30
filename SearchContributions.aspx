<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" 
AutoEventWireup="true" CodeFile="SearchContributions.aspx.cs" 
Inherits="_SearchContributionsPageClass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    
<div class="search-page">
<div class="row controls">
<div class="medium-4 large-6 columns campaign-nav">
<nav>
<ul>
<li><a class="active" href="SearchContributions.aspx">Campaign Contributions</a></li>
<li><a href="SearchExpenditures.aspx">Campaign Expenditures</a></li>
</ul>
</nav>
</div>
<div class="medium-8 large-6 columns">
<div class="pagination right">
<span>View</span>
<span>
<a class="button dropdown" data-dropdown="drop3" href="http://openbookpgh.herokuapp.com/campaign-contributions#">5 per page</a>
<ul class="f-dropdown" data-dropdown-content="" id="drop3">
<li><a href="http://openbookpgh.herokuapp.com/campaign-contributions#">10 per page</a></li>
<li><a href="http://openbookpgh.herokuapp.com/campaign-contributions#">25 per page</a></li>
<li><a href="http://openbookpgh.herokuapp.com/campaign-contributions#">50 per page</a></li>
</ul>
</span>
<span>Results: 1 - 10 of 11487</span>
</div>
</div>
</div>
<div class="row">
<div class="medium-4 large-3 columns">
<div class="search-field">
<h2>Candidate Name</h2>

    <asp:DropDownList ID="ddlCandidateName" runat="server" 
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


</div>
<div class="search-field">
<h2>Office</h2>
<asp:DropDownList ID="ddlOffice" runat="server">
					<asp:ListItem Text="All" Value="all" />
					<asp:ListItem Text="Mayor" Value="mayor" />
					<asp:ListItem Text="City Council" Value="council" />
					<asp:ListItem Text="City Controller" Value="controller" />
				</asp:DropDownList>
</div>
<div class="search-field">
<h2>Year</h2>
		   <asp:DropDownList ID="ddldateContribution" runat="server" 
					DataSourceID="SqlDataSource3" 
					DataTextField="yearName" 
					DataValueField="yearValue" /> 
					
				<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
					ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
					SelectCommand="SELECT [yearName], [yearValue] FROM [tlk_year]">
				</asp:SqlDataSource>
</div>
<div class="search-field ">
<h2>Search by</h2>

<asp:RadioButtonList ID="rblContributorSearch" runat="server" RepeatDirection="Horizontal">
	<asp:ListItem Text="Commitee" Value="co" Selected="True" />
	<asp:ListItem Text="Contributor" Value="in" />
</asp:RadioButtonList>


  
    
<asp:TextBox ID="txtContributor" runat="server" placeholder="Contributor or Committee..."  />
</div>
<div class="search-field">
<h2>Employer</h2>
    <asp:TextBox ID="txtEmployer" runat="server" placeholder="Employer..." />

</div>

<div class="search-field">
    <h2>Distance From Zip code</h2>
    <asp:DropDownList ID="ddlDistance" runat="server">
					<asp:ListItem Text="Any" Value="0" />
					<asp:ListItem Text="50 miles" Value="50" />
					<asp:ListItem Text="30 miles" Value="30" />
					<asp:ListItem Text="20 miles" Value="20" />
					<asp:ListItem Text="10 miles" Value="10" />
					<asp:ListItem Text="5 miles" Value="5" />				
				</asp:DropDownList>
				 
				 <asp:TextBox ID="txtZip"  MaxLength="5" runat="server" placeholder="ZipCode..."/>


</div>
<div class="search-field">

     <asp:Button ID="Button1" runat="server" Text="Search" onclick="Button1_Click" CssClass="submit button" />

</div>
</div>
<div class="medium-8 large-9 columns">
<div class="items-container">
<div class="item">
<span class="name-label">Contributer</span>
<h2>Caroline H. Stewart</h2>
<div class="label-group">
<div class="label-item">
<div class="type">Office sought</div>
<div class="title">Mayor</div>
</div>
<div class="label-item">
<div class="type">Candidate</div>
<div class="title">William Peduto</div>
</div>
</div>
<div class="details">
<ul>
<li>
<span class="key">Date</span>
<span class="value">05/12/2009</span>
</li>
<li>
<span class="key">Amount</span>
<span class="value">$100</span>
</li>
<li>
<span class="key">Address</span>
<span class="value">Pittsburgh, PA 15217</span>
</li>
</ul>
</div>
</div>
<div class="item">
<span class="name-label">Contributer</span>
<h2>Caroline H. Stewart</h2>
<div class="label-group">
<div class="label-item">
<div class="type">Office sought</div>
<div class="title">Mayor</div>
</div>
<div class="label-item">
<div class="type">Candidate</div>
<div class="title">William Peduto</div>
</div>
</div>
<div class="details">
<ul>
<li>
<span class="key">Date</span>
<span class="value">05/12/2009</span>
</li>
<li>
<span class="key">Amount</span>
<span class="value">$100</span>
</li>
<li>
<span class="key">Address</span>
<span class="value">Pittsburgh, PA 15217</span>
</li>
</ul>
</div>
</div>
<div class="item">
<span class="name-label">Contributer</span>
<h2>Caroline H. Stewart</h2>
<div class="label-group">
<div class="label-item">
<div class="type">Office sought</div>
<div class="title">Mayor</div>
</div>
<div class="label-item">
<div class="type">Candidate</div>
<div class="title">William Peduto</div>
</div>
</div>
<div class="details">
<ul>
<li>
<span class="key">Date</span>
<span class="value">05/12/2009</span>
</li>
<li>
<span class="key">Amount</span>
<span class="value">$100</span>
</li>
<li>
<span class="key">Address</span>
<span class="value">Pittsburgh, PA 15217</span>
</li>
</ul>
</div>
</div>
<div class="item">
<span class="name-label">Contributer</span>
<h2>Caroline H. Stewart</h2>
<div class="label-group">
<div class="label-item">
<div class="type">Office sought</div>
<div class="title">Mayor</div>
</div>
<div class="label-item">
<div class="type">Candidate</div>
<div class="title">William Peduto</div>
</div>
</div>
<div class="details">
<ul>
<li>
<span class="key">Date</span>
<span class="value">05/12/2009</span>
</li>
<li>
<span class="key">Amount</span>
<span class="value">$100</span>
</li>
<li>
<span class="key">Address</span>
<span class="value">Pittsburgh, PA 15217</span>
</li>
</ul>
</div>
</div>
<div class="item">
<span class="name-label">Contributer</span>
<h2>Caroline H. Stewart</h2>
<div class="label-group">
<div class="label-item">
<div class="type">Office sought</div>
<div class="title">Mayor</div>
</div>
<div class="label-item">
<div class="type">Candidate</div>
<div class="title">William Peduto</div>
</div>
</div>
<div class="details">
<ul>
<li>
<span class="key">Date</span>
<span class="value">05/12/2009</span>
</li>
<li>
<span class="key">Amount</span>
<span class="value">$100</span>
</li>
<li>
<span class="key">Address</span>
<span class="value">Pittsburgh, PA 15217</span>
</li>
</ul>
</div>
</div>
<div class="item">
<span class="name-label">Contributer</span>
<h2>Caroline H. Stewart</h2>
<div class="label-group">
<div class="label-item">
<div class="type">Office sought</div>
<div class="title">Mayor</div>
</div>
<div class="label-item">
<div class="type">Candidate</div>
<div class="title">William Peduto</div>
</div>
</div>
<div class="details">
<ul>
<li>
<span class="key">Date</span>
<span class="value">05/12/2009</span>
</li>
<li>
<span class="key">Amount</span>
<span class="value">$100</span>
</li>
<li>
<span class="key">Address</span>
<span class="value">Pittsburgh, PA 15217</span>
</li>
</ul>
</div>
</div>
<div class="item">
<span class="name-label">Contributer</span>
<h2>Caroline H. Stewart</h2>
<div class="label-group">
<div class="label-item">
<div class="type">Office sought</div>
<div class="title">Mayor</div>
</div>
<div class="label-item">
<div class="type">Candidate</div>
<div class="title">William Peduto</div>
</div>
</div>
<div class="details">
<ul>
<li>
<span class="key">Date</span>
<span class="value">05/12/2009</span>
</li>
<li>
<span class="key">Amount</span>
<span class="value">$100</span>
</li>
<li>
<span class="key">Address</span>
<span class="value">Pittsburgh, PA 15217</span>
</li>
</ul>
</div>
</div>
</div>
<div class="large-12 columns pagination-controls">
<div class="prev">
<a href="#">Previous</a>
</div>
<div class="next">
<a href="#">Next</a>
</div>
</div>
</div>
</div>
</div>
     











     

</asp:Content>

