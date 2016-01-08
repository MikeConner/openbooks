<%@ Page Title="Open Book Pittsburgh" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" 
    Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="homepage-hero">
        <div class="row">
            <div class="large-12 columns">
                <h1>County of Allegheny government contracts.</h1>
                <p>Search Open Book Allegheny for contracts associated with an Allegheny County office.</p>
            </div>
        </div>
    </div>
    <div class="information">
        <div class="row">
            <div class="large-2 columns pane">&nbsp;</div>
            <div class="large-8 columns pane">
                <div class="large-9 columns">
                    <h3>County Transparency</h3>
                    <p>Search contracts for all Allegheny County departments.</p>
                </div>
                <div class="large-3 columns">
                    <br />
                    <br />
                    <a href="SearchContracts.aspx">Find County Contracts →</a>
                </div>
            </div>
            <div class="large-2 columns pane">&nbsp;</div>
        <!--
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
        -->
        </div>
    </div>

</asp:Content>
