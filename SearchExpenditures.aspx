<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="SearchExpenditures.aspx.cs" 
Inherits="SearchExpendituresPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     


    
<div class="search-page">
<div class="row controls">
<div class="medium-4 large-6 columns campaign-nav">
<nav>
<ul>
<li><a href="SearchContributions.aspx">Campaign Contributions</a></li>
<li><a class="active" href="SearchExpenditures.aspx">Campaign Expenditures</a></li>
</ul>
</nav>
</div>
<div class="medium-8 large-6 columns">
<div class="pagination right">
<span>View</span>
<span>
<a class="button dropdown" data-dropdown="drop3" href="http://openbookpgh.herokuapp.com/campaign-expenditures#">5 per page</a>
<ul class="f-dropdown" data-dropdown-content="" id="drop3">
<li><a href="http://openbookpgh.herokuapp.com/campaign-expenditures#">10 per page</a></li>
<li><a href="http://openbookpgh.herokuapp.com/campaign-expenditures#">25 per page</a></li>
<li><a href="http://openbookpgh.herokuapp.com/campaign-expenditures#">50 per page</a></li>
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
<h2>Vendor</h2>

    <asp:RadioButtonList ID="rblVendorSearchOptions" runat="server" RepeatDirection="Horizontal" hidden>
					<asp:ListItem Text="Begins with" Value="B" Selected="True" />
					<asp:ListItem Text="Contains" Value="C" />
					<asp:ListItem Text="Exact" Value="E" />
				</asp:RadioButtonList>
				<asp:TextBox ID="txtVendor" runat="server" placeholder="Name of vendor..." />
 
</div>
<div class="search-field">
<h2>Keywords</h2>

    <asp:TextBox ID="txtKeywords" runat="server" placeholder="Keywords..." />

 
</div>
<div class="search-field">

     <asp:Button ID="ImageButton1" runat="server" Text = "Search" AlternateText="Search" onclick="Button1_Click" CssClass="submit button" />
</div>
</div>
<div class="medium-8 large-9 columns">
<div class="items-container">
<div class="item">
<span class="name-label">Company or Individual</span>
<h2>GoDaddy.com</h2>
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
<span class="value">No Address</span>
</li>
</ul>
</div>
<div class="description">
<span>Food for pitt students, clean ups district</span>
</div>
</div>
<div class="item">
<span class="name-label">Company or Individual</span>
<h2>GoDaddy.com</h2>
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
<span class="value">No Address</span>
</li>
</ul>
</div>
<div class="description">
<span>Food for pitt students, clean ups district</span>
</div>
</div>
<div class="item">
<span class="name-label">Company or Individual</span>
<h2>GoDaddy.com</h2>
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
<span class="value">No Address</span>
</li>
</ul>
</div>
<div class="description">
<span>Food for pitt students, clean ups district</span>
</div>
</div>
<div class="item">
<span class="name-label">Company or Individual</span>
<h2>GoDaddy.com</h2>
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
<span class="value">No Address</span>
</li>
</ul>
</div>
<div class="description">
<span>Food for pitt students, clean ups district</span>
</div>
</div>
<div class="item">
<span class="name-label">Company or Individual</span>
<h2>GoDaddy.com</h2>
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
<span class="value">No Address</span>
</li>
</ul>
</div>
<div class="description">
<span>Food for pitt students, clean ups district</span>
</div>
</div>
<div class="item">
<span class="name-label">Company or Individual</span>
<h2>GoDaddy.com</h2>
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
<span class="value">No Address</span>
</li>
</ul>
</div>
<div class="description">
<span>Food for pitt students, clean ups district</span>
</div>
</div>
<div class="item">
<span class="name-label">Company or Individual</span>
<h2>GoDaddy.com</h2>
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
<span class="value">No Address</span>
</li>
</ul>
</div>
<div class="description">
<span>Food for pitt students, clean ups district</span>
</div>
</div>
</div>
<div class="large-12 columns pagination-controls">
<div class="prev">
<a href="http://openbookpgh.herokuapp.com/campaign-expenditures#">Previous</a>
</div>
<div class="next">
<a href="http://openbookpgh.herokuapp.com/campaign-expenditures#">Next</a>
</div>
</div>
</div>
</div>
</div>

</div>



</asp:Content>

