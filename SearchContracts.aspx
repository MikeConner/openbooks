<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="SearchContracts.aspx.cs" 
Inherits="SearchContractsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">



<div class="search-page">
<div class="row controls">
<div class="medium-4 large-6 columns campaign-nav">
<nav>
<ul>
<li><a class="active" href="#">City Contacts</a></li>
</ul>
</nav>
</div>
<div class="medium-8 large-6 columns">
<div class="pagination right">
<span>View</span>
<span>
<a class="button dropdown" data-dropdown="drop3" href="#">5 per page</a>
<ul class="f-dropdown" data-dropdown-content="" id="drop3">
<li><a href="#">10 per page</a></li>
<li><a href="#">25 per page</a></li>
<li><a href="#">50 per page</a></li>
</ul>
</span>
<span>Results: 1 - x of xxxx</span>
</div>
</div>
</div>
<div class="row">
<div class="large-3 columns">
<button class="submit expand filter-toggle" href="#">Filter Options</button>
</div>
</div>
<div class="row">
<div class="medium-4 large-3 columns filter-controls">
<div class="search-field">
<h2>City Department</h2>

    <asp:DropDownList ID="DropDownList1" runat="server" 
					DataSourceID="SqlDataSource4" 
					DataTextField="DeptName" 
					DataValueField="DeptCode" 
					AppendDataBoundItems="true">
					<asp:ListItem Text="All Organizations" Value="0" />
				</asp:DropDownList>
				<asp:SqlDataSource ID="SqlDataSource4" runat="server" 
					ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
					SelectCommand="SELECT [DeptCode], [DeptName] FROM [tlk_department] ORDER BY DeptName" />
 
</div>
<div class="search-field">
<h2>Contract Type</h2>
<asp:DropDownList ID="DropDownList2" runat="server" 
					DataSourceID="SqlDataSource1" 
					DataTextField="ServiceName" 
					DataValueField="ID" 
					AppendDataBoundItems="true">
					<asp:ListItem Text="All Services" Value="0" />
				</asp:DropDownList>
				<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
					ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
					SelectCommand="SELECT [ID], [ServiceName] FROM [tlk_service] ORDER BY ServiceName" />
</div>
<div class="search-field">
<h2>Vendor</h2>
<asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal" hidden>
					<asp:ListItem Text="Begins with" Value="B" />
					<asp:ListItem Text="Contains" Value="C" Selected="True" />
					<asp:ListItem Text="Exact" Value="E" />
				</asp:RadioButtonList>
				<asp:TextBox ID="TextBox1" runat="server" Placeholder ="Name of Vendor... " />
</div>
<div class="search-field">
<h2>Keyword</h2>
<asp:TextBox ID="TextBox2" runat="server" placeholder="Keywords..."/>
</div>
<div class="search-field">
<h2>Contract Approval Date</h2>
<div class="row date-select">
<div class="large-12 columns">
<label class="date">From:</label>
<input placeholder="Start" type="date" id="dtmStart">
</div>
<div class="large-12 columns">
<label class="date">To:</label>
<input placeholder="End" type="date" id="dtmFinish">
</div>
</div>
</div>
<div class="search-field">
<h2>Contract Amount</h2>
<div class="range-slider">
<label>Minimum Amount</label>
<input class="input-range" max="10000" min="1" type="range" value="250" id="dblMinContract">
<span id="minContract" class="range-value">250</span>
</div>
<div class="range-slider">
<label>Maximum Amount</label>
<input class="input-range" max="10000" min="1" type="range" value="250" id="dblMaxContract">
<span id="maxContract" class="range-value">250</span>
</div>
</div>
<div class="search-field">

    <asp:Button ID="ImageButton1" runat="server" Text="Search" onclick="btnSearch_Click" CssClass="button" />

</div>
</div>





<div class="medium-8 large-9 columns">
<div class="items-container">
<div class="item">
<h2>SMAIL, KATHLEEN</h2>
<div class="price-group">
<span class="original">Contract Amount: $34,723.31</span>
<span class="current">$34,723.31 Spent</span>
</div>
<div class="label-group">
<div class="label-item">
<div class="type">Type</div>
<div class="title">Grant Agreement</div>
</div>
<div class="label-item">
<div class="type">Agency Name</div>
<div class="title">Personnel/Civil Service</div>
</div>
</div>
<div class="agenda">
<span class="title">Contract Agenda:</span>
<span>04/30/2014 — 03/14/2014</span>
</div>
<div class="description">
<p>Memo/Severance Incentive Program</p>
</div>
</div>
<div class="item">
<h2>YMCA OF GTR. PGH.- HAZELWOOD SEEDS TO SOUP d/b/a YMCA</h2>
<div class="price-group">
<span class="original">Contract Amount: $34,723.31</span>
<span class="current">$34,723.31 Spent</span>
</div>
<div class="label-group">
<div class="label-item">
<div class="type">Type</div>
<div class="title">Grant Agreement</div>
</div>
<div class="label-item">
<div class="type">Agency Name</div>
<div class="title">Personnel/Civil Service</div>
</div>
</div>
<div class="agenda">
<span class="title">Contract Agenda:</span>
<span>03/28/2014 — 05/31/2014</span>
</div>
<div class="description">
<p>Independant Employee Assistance Provider</p>
</div>
</div>
<div class="item">
<h2>VARIOUS VENDORS - COUNTY CONTRACTS</h2>
<div class="price-group">
<span class="original">Contract Amount: $34,723.31</span>
<span class="current">$34,723.31 Spent</span>
</div>
<div class="label-group">
<div class="label-item">
<div class="type">Type</div>
<div class="title">Grant Agreement</div>
</div>
<div class="label-item">
<div class="type">Agency Name</div>
<div class="title">Personnel/Civil Service</div>
</div>
</div>
<div class="agenda">
<span class="title">Contract Agenda:</span>
<span>04/23/2014 — 12/31/2014</span>
</div>
<div class="description">
<p>Athletic Supplies &amp; Equipment/Spec. 7178- 48252, 48263, 48271, 48272</p>
</div>
</div>
<div class="item">
<h2>YMCA OF GTR. PGH.- HAZELWOOD SEEDS TO SOUP d/b/a YMCA</h2>
<div class="price-group">
<span class="original">Contract Amount: $34,723.31</span>
<span class="current">$34,723.31 Spent</span>
</div>
<div class="label-group">
<div class="label-item">
<div class="type">Type</div>
<div class="title">Grant Agreement</div>
</div>
<div class="label-item">
<div class="type">Agency Name</div>
<div class="title">Personnel/Civil Service</div>
</div>
</div>
<div class="agenda">
<span class="title">Contract Agenda:</span>
<span>03/28/2014 — 05/31/2014</span>
</div>
<div class="description">
<p>Independant Employee Assistance Provider</p>
</div>
</div>
<div class="item">
<h2>YMCA OF GTR. PGH.- HAZELWOOD SEEDS TO SOUP d/b/a YMCA</h2>
<div class="price-group">
<span class="original">Contract Amount: $34,723.31</span>
<span class="current">$34,723.31 Spent</span>
</div>
<div class="label-group">
<div class="label-item">
<div class="type">Type</div>
<div class="title">Grant Agreement</div>
</div>
<div class="label-item">
<div class="type">Agency Name</div>
<div class="title">Personnel/Civil Service</div>
</div>
</div>
<div class="agenda">
<span class="original">Contract Amount: $34,723.31</span>
<span class="current">$34,723.31 Spent</span>
</div>
<div class="description">
<p>Independant Employee Assistance Provider</p>
</div>
</div>
</div>
<div class="large-12 columns pagination-controls">
<div class="prev">
<a href="http://openbookpgh.herokuapp.com/city-contracts#">Previous</a>
</div>
<div class="next">
<a href="http://openbookpgh.herokuapp.com/city-contracts#">Next</a>
</div>
</div>
</div>
</div>
</div>

</asp:Content>

