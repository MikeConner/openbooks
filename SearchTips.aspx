<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="SearchTips.aspx.cs"
    Inherits="_SearchTips" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="contact">
        <div class="row">
            <div class="large-12 columns">
                <div class="inforightcontent">
                    <h3>Open Book Search Tip's</h3>
                    <h4>Searching By City Contracts</h4>
                    <ul>
                        <li>Select the desired City Department from the drop down box. This box will display a list of City Departments. You can select only one.</li>
                        <li>Type in your the Vendor name you are searching. Does not have to be the exact name.  If you do not know the vendor name, leave blank.</li>
                        <li>Select the desired Contract Type from the dropdown menu.  This box will display a list of Contract Types.  You can select only one.</li>
                        <li>Type in any keywords associated with this contract, separated by commas.</li>
                        <li>Select the contract approval date to narrow your search.</li>
                        <li>Select the amount the contract was for and then Click Search.</li>
                    </ul>
                    <h4>Searching By Political Contributions</h4>
                    <ul>
                        <li>Select a Candidate name from the dropdown box.  This box will display a list of political cadidates.  You can only select one.</li>
                        <li>Select the Office.</li>
                        <li>Search by Committee or Contributor. Then type in the text field.</li>
                        <li>Type in the employer you wish to search in the Text field.</li>
                    </ul>
                    <h4>Searching By Political Expenditures</h4>
                    <ul>
                        <li>Select the desired Candidate from the drop down box.  This box will display a list of Cadidates.</li>
                        <li>Type in the Vendor name you are searching.</li>
                        <li>Type in any keywords into the text box provided.</li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
