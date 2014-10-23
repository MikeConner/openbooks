<%@ Page Title="Open Book Pittsburgh" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" 
    Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="homepage-hero">
        <div class="row">
            <div class="large-12 columns">
                <h1>A searchable database of City of Pittsburgh government contracts, campaign finance reports, and lobbyists.</h1>
                <p>Search Open Book Pittsburgh for City contracts, campaign donations and expenditures of candidates for a City office, and lobbyists that do business with the City of Pittsburgh.    </p>
            </div>
        </div>
    </div>
    <div class="information">
        <div class="row">
            <div class="large-4 columns pane">
                <h2><a href="SearchContracts.aspx">City Contracts</a></h2>
                <p>Search contracts for all City departments.  </p>
                <a href="SearchContracts.aspx">Find City Contracts →</a>
            </div>
            <div class="large-4 columns pane">
                <h2><a href="SearchContributions.aspx">Campaign Finance</a></h2>
                <p>Search for campaign contributions and campaign expenditures of candidates for Mayor, Controller and City Council. </p>
                <a href="SearchContributions.aspx">Search Campaign Finances →</a>
            </div>
            <div class="large-4 columns pane">
                <h2><a href="SearchLobbyists.aspx">Lobbyists</a></h2>
                <p>See those who are registered as lobbyists with the City of Pittsburgh. </p>
                <a href="SearchLobbyists.aspx">Find Lobbyists →</a>
            </div>
        </div>
    </div>

</asp:Content>
