<%@ Page Title ="Open Book Pittsburgh"  Language="C#" MasterPageFile="~/_Masters/MasterPage.master"  AutoEventWireup="true" CodeFile="Default.aspx.cs" 
Inherits="_Default" %>

 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

  
<div class="homepage-hero">
<div class="row">
<div class="large-10 columns">
<h1>A searchable database of Pittsburgh City Government Contracts.</h1>
<p>Search from over 7,000 contracts, Lobbyists, Finance Reports or let open book find what you’re looking for</p>
</div>
</div>
</div>
<div class="information">
<div class="row">
<div class="large-4 columns pane">
<h2><a href="SearchContracts.aspx">City Contracts</a></h2>
<p>Searchable database of Pittsburgh city government contracts, administered by Pittsburgh’s Office of the City Controller.</p>
<a href="SearchContracts.aspx">Find City Contracts →</a>
</div>
<div class="large-4 columns pane">
<h2><a href="SearchContributions.aspx">Campaign Finance</a></h2>
<p>The City Controller is one of two, independently elected, citywide offices in the City of Pittsburgh.</p>
<a href="SearchContributions.aspx">Search Campaign Finances →</a>
</div>
<div class="large-4 columns pane">
<h2><a href="SearchLobbyists.aspx">Lobbyists</a></h2>
<p>Searchable database of Pittsburgh city government contracts, administered by Pittsburgh’s Office.</p>
<a href="SearchLobbyists.aspx">Find Lobbyists →</a>
</div>
</div>
</div>
<div class="finance">
<div class="row">
<div class="large-2 columns">
<img alt="Icn finance" src="img/icn-finance.png">
</div>
<div class="large-10 columns">
<h3>In addition to city government contracts</h3>
<p>Open Book Pittsburgh also gives taxpayers a look as to where elected officials and candidates for elected offices in the City of Pittsburgh receive campaign contributions, and how candidates spend those funds.</p>
</div>
</div>
</div>

</div>


 
 
    

</asp:Content>