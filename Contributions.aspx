<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="Contributions.aspx.cs" Inherits="Contributions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="mainwrap">
        <div class="gridboxhead">
            <h2>Campaign Contributions Search Results</h2>
        </div>
        <div class="results">
            <div class="resultsleft">
                <asp:Label ID="lblPageSize" runat="server" Text="View:" />

                <asp:DropDownList ID="ddlPageSize" runat="server"
                    OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged"
                    AutoPostBack="true">
                    <asp:ListItem Text="10 per page" Value="10" />
                    <asp:ListItem Text="25 per page" Value="25" />
                    <asp:ListItem Text="50 per page" Value="50" />
                    <asp:ListItem Text="100 per page" Value="100" />
                </asp:DropDownList>
            </div>
            <div class="resultsright">
                <asp:Label ID="lblCurrentPage" runat="server" />
            </div>
        </div>
        <table class="ob-gridview" cellpadding="0" cellspacing="0">
            <tr>
                <th>
                    <asp:LinkButton ID="LinkButton1" Text="Contributor" OnClick="sortContributor" runat="server" /><asp:Image ID="imgSortContributor" runat="server" /></th>
                <th>
                    <asp:LinkButton ID="LinkButton2" Text="Candidate" OnClick="sortCandidate" runat="server" /><asp:Image ID="imgSortCandidate" runat="server" /></th>
                <th>
                    <asp:LinkButton ID="LinkButton3" Text="Office Sought" OnClick="sortOffice" runat="server" /><asp:Image ID="imgSortOffice" runat="server" /></th>
                <th>
                    <asp:LinkButton ID="LinkButton4" Text="Employer" OnClick="sortEmployer" runat="server" /><asp:Image ID="imgSortEmployer" runat="server" /></th>
                <th>Occupation</th>
                <th>Address</th>
                <th>
                    <asp:LinkButton ID="LinkButton8" Text="Amount" OnClick="sortAmount" runat="server" /><asp:Image ID="imgSortAmount" runat="server" /></th>
                <th>
                    <asp:LinkButton ID="LinkButton9" Text="Date" OnClick="sortDate" runat="server" /><asp:Image ID="imgSortDate" runat="server" /></th>
                <th>Description</th>
                <th>Distance</th>
            </tr>
            <asp:Repeater ID="rptContributions" runat="server">
                <ItemTemplate>
                    <tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "even" %>' valign="top">
                        <td><%# DataBinder.Eval(Container.DataItem, "ContributorName") %></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "CandidateName") %></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "Office") %></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "Employer")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "Occupation") %></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "City")%>, 
                            <%# null == (DataBinder.Eval(Container.DataItem, "Province").ToString()) ? DataBinder.Eval(Container.DataItem, "State") : DataBinder.Eval(Container.DataItem, "Province")%> 
                            <%# DataBinder.Eval(Container.DataItem, "Zip")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "Amount", "{0:C}")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "DateContribution", "{0:MM/dd/yyyy}")%></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "Description") %></td>
                        <td>
                            <asp:Panel ID="pnlDistance" runat="server" Visible='<%# DataBinder.Eval(Container.DataItem, "distance") != DBNull.Value %>'>
                                <asp:Label ID="lblDistance" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "distance") %>' />
                                miles
                            </asp:Panel>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <div class="bottomnav">
            <div class="bottomnavbtns">
                <asp:ImageButton ID="ibtnFirstPageTop" runat="server" OnClick="FirstPage_Click" ImageUrl="~/img/firstbtn.gif" />
                <asp:ImageButton ID="ibtnPrevPageTop" runat="server" OnClick="PrevPage_Click" ImageUrl="~/img/previousbtn.gif" />
                <asp:ImageButton ID="ibtnNextPageTop" runat="server" OnClick="NextPage_Click" ImageUrl="~/img/nextbtn.gif" />
                <asp:ImageButton ID="ibtnLastPageTop" runat="server" OnClick="LastPage_Click" ImageUrl="~/img/lastbtn.gif" />
            </div>
        </div>
    </div>
</asp:Content>

