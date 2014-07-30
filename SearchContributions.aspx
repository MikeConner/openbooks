﻿<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" 
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
</span>
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
<div class="items-container">
<asp:Repeater ID="rptContributions" runat="server">
<ItemTemplate>
<div class="item">
<span class="name-label">Contributer</span>
<h2><%# DataBinder.Eval(Container.DataItem, "ContributorName") %></h2>
<div class="label-group">
<div class="label-item">
<div class="type">Office sought</div>
<div class="title"><%# DataBinder.Eval(Container.DataItem, "Office") %></div>
</div>
<div class="label-item">
<div class="type">Candidate</div>
<div class="title"><%# DataBinder.Eval(Container.DataItem, "CandidateName") %></div>
</div>
<div class="label-item">
<div class="type">Employer</div>
<div class="title"><%# DataBinder.Eval(Container.DataItem, "Employer")%></div>
</div>
<div class="label-item">
<div class="type">Occupation</div>
<div class="title"><%# DataBinder.Eval(Container.DataItem, "Occupation") %></div>
</div>

</div>
<div class="details">
<ul>
<li>
<span class="key">Date</span>
<span class="value"><%# DataBinder.Eval(Container.DataItem, "DateContribution", "{0:MM/dd/yyyy}")%></span>
</li>
<li>
<span class="key">Amount</span>
<span class="value"><%# DataBinder.Eval(Container.DataItem, "Amount", "{0:C}")%></span>
</li>
<li>
<span class="key">Address</span>
<span class="value"><%# DataBinder.Eval(Container.DataItem, "City")%>, <%# DataBinder.Eval(Container.DataItem, "State")%> <%# DataBinder.Eval(Container.DataItem, "Zip")%></span>
</li>
<li>
<span class="description">Description</span>
<span><%# DataBinder.Eval(Container.DataItem, "Description") %></span>
</li>
<li>
<span class="key">pnlDistance</span>
<span class="value"><%# DataBinder.Eval(Container.DataItem, "distance") != DBNull.Value %></span>
</li>
<li>
<span class="key">lblDistance</span>
<span class="value"><%# DataBinder.Eval(Container.DataItem, "distance") %> miles</span>
</li>

</ul>
</div>
</div>
</ItemTemplate>
</asp:Repeater>
    	<div class="bottomnav">
		<div class="bottomnavbtns">
		    <asp:ImageButton ID="ibtnFirstPageTop" runat="server" OnClick="FirstPage_Click" />
			<asp:ImageButton ID="ibtnPrevPageTop" runat="server" OnClick="PrevPage_Click" />
			<asp:ImageButton ID="ibtnNextPageTop" runat="server" OnClick="NextPage_Click" />
			<asp:ImageButton ID="ibtnLastPageTop" runat="server" OnClick="LastPage_Click" />
		</div>
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

