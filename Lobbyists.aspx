<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="Lobbyists.aspx.cs"
    Inherits="Search_Lobbyists" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="mainwrap">
        <div class="gridboxhead">
            <div class="gridboxleft">
                <h2>Lobbyist Registrations</h2>
            </div>
            <div class="gridboxright"></div>
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
                    <asp:LinkButton ID="lb1" Text="Full&nbsp;Name" OnClick="sortLobbyist" runat="server" /></th>
                <th>Position</th>
                <th>
                    <asp:LinkButton ID="lb2" Text="Employer" OnClick="sortEmployer" runat="server" /></th>
                <th>Additional Companies</th>
                <th>Status&nbsp;</th>
            </tr>
            <asp:Repeater ID="rptLobbyists" runat="server"
                OnItemDataBound="rptLobbyists_ItemDataBound">
                <ItemTemplate>
                    <tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "even" %>' valign="top">
                        <td><%# Eval("LobbyistName") %></td>
                        <td><%# Eval("Position") %></td>
                        <td>
                            <b><%# Eval("EmployerName")%></b><br />
                            <%# Eval("Address")%><br />
                            <%# Eval("City") + " " + Eval("State") + ", " + Eval("Zip")%><br />
                        </td>
                        <td>
                            <asp:Repeater ID="rptCompanies" runat="server">
                                <ItemTemplate>
                                    <b><%# Eval("CompanyName")%></b>,
						<%# Eval("Address")%>,
						<%# Eval("City") + " " + Eval("State") + ", " + Eval("Zip")%><br />
                                </ItemTemplate>
                            </asp:Repeater>
                            <td><%#Eval ("LobbyistStatus") %>&nbsp;&nbsp;</td>
                        <td><asp:CheckBox Checked=<%# Eval("ForCity")%> runat="server"/></td>
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

