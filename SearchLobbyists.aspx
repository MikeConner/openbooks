<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="SearchLobbyists.aspx.cs"
    Inherits="SearchLobbyistsPage" %>
<%--//DAS--%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="search-page">
        <div class="row controls">
            <div class="medium-4 large-6 columns campaign-nav">
                <nav>
                    <ul>
                        <li><a href="#">Lobbyists</a></li>
                    </ul>
                </nav>
            </div>
            <div class="medium-8 large-6 columns">
                <div class="pagination right">
                    <asp:Label ID="lblPageSize" runat="server" Text="View:" />
                    <asp:DropDownList ID="ddlPageSize" runat="server" class=" dropdown"
                        OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Text="10 per page" Value="10" />
                        <asp:ListItem Text="25 per page" Value="25" />
                        <asp:ListItem Text="50 per page" Value="50" />
                        <asp:ListItem Text="100 per page" Value="100" />
                    </asp:DropDownList>
                    <asp:Label ID="lblCurrentPage" runat="server" />
                </div>
            </div>
        </div>
        <div class="row search-row">
            <div class="medium-4 large-3 columns">
                <div class="search-field">
                    <h2>Name Search</h2>
                    <asp:TextBox ID="txtLobbyist" runat="server" MaxLength="100" />
                </div>
                <div class="search-field">
                    <h2>OR</h2>
                    <h2>Employer</h2>
                    <asp:TextBox ID="txtEmployer" runat="server" MaxLength="100" />
                </div>
                <div class="search-field">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="submit button" />
                </div>
                <div class="information-section">
                    <p>In 2009, Pittsburgh City Council past an ordinance that mandates that any person engaged in compensated lobby activities that aim to influence decisions made by Pittsburgh government officials, be registered with the Office of the City Controller.</p>
                    <p>Here you will be able to search by name and employer those persons who have registered as lobbyist with the City of Pittsburgh.  You will also find information regarding the lobbyist registration ordinance and lobbyist registration form and instructions.</p>
                    <p>
                        <b>
                            <a href="documents/Lobbyist_Registration_Form.pdf" target="_blank">Lobbyist Registration Form </a>
                            <br />
                            <a href="documents/Lobbyist_Registration_Pittsburgh_City_Code.pdf" target="_blank">Lobbyist Registration Pittsburgh City Code </a>
                            <br />
                            <a href="documents/Lobbyist_Registration_Frequently_Asked_Questions.pdf" target="_blank">Lobbyist Registration FAQ's</a>
                        </b>
                    </p>
                </div>
            </div>
            <div class="medium-8 large-9 columns">
                                <%if ("1" != Request.QueryString["click"]) { %> 
            <div id="instructions">
                <h3>Instructions</h3>
                <div class="items-container instructions-content">
                         <h4>Searching</h4>
                    <p>Search here for lobbyists.</p>
                        <h4>Search Criteria</h4>
                        <p>
                            You can filter by <b>name</b> or <b>employer</b>.
                        </p>
                        <h4>Search Results</h4>
                          <p>Since the number of results might be quite large, results are presented one page at a time. You can set the page size from 10-100</p>
                    <p> You can sort by lobbyist, employer or date. (Click on the arrow to the right to change the sort direction)</p>

                        <br />
                </div>
            </div>     
                <% } else{ %>
                <div class="search-field">
                    <h2>Sort Results by:</h2>
                    <asp:DropDownList ID="ddlSortLobbyists" CssClass="sort-dropdown" runat="server"
                        OnSelectedIndexChanged="ddlSortLobbyists_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Text="Lobbyist" Value="LobbyistName" />
                        <asp:ListItem Text="Employer" Value="EmployerName" />
                        <asp:ListItem Text="Date" Value="DateEntered" />
                    </asp:DropDownList>
                    <asp:ImageButton ID="imgSortDirection" OnClick="toggleSortDirection" runat="server" />
                </div>
                <div class="items-container">
                    <div class="results">
                        <div class="resultsleft">
                        </div>
                        <div class="resultsright">
                        </div>
                    </div>
                    <asp:Repeater ID="rptLobbyists" runat="server" OnItemDataBound="rptLobbyists_ItemDataBound">
                        <ItemTemplate>
                            <div class="item <%# Container.ItemIndex % 2 == 0 ? "" : "even" %>">
                                <span class="name-label">Full Name</span>
                                <h2><%# Eval("LobbyistName") %></h2>
                                <div class="information">
                                    <span class="position"><%# Eval("Position") %></span>
                                    <span class="status"><%#Eval ("LobbyistStatus") %></span><br />
                                    <span class="status">For City: <%# InterpretForCity(Eval("ForCity").ToString()) %></span>
                                </div>
                                <div class="label-group">
                                    <div class="label-item">
                                        <div class="type"><%# Eval("EmployerName")%></div>
                                        <div class="title"><%# Eval("Address")%> <%# Eval("City") + " " + Eval("State") + ", " + Eval("Zip")%> </div>
                                    </div>
                                </div>
                                <div class="additional-companies">
                                    <ul>
                                        <asp:Repeater ID="rptCompanies" runat="server">
                                            <ItemTemplate>
                                                <li>
                                                    <span class="company"><%# Eval("CompanyName")%>:</span>
                                                    <span class="address"><%# Eval("Address")%>,
						                             <%# Eval("City") + " " + Eval("State") + ", " + Eval("Zip")%></span>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </ul>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                    <div class="large-12 columns pagination-controls">
                        <div class="large-3 columns prev button">
                            <asp:Button ID="ibtnFirstPageTop" runat="server" OnClick="FirstPage_Click" Text="First" class="button prev" />
                        </div>
                        <div class="large-3 columns prev button">
                            <asp:Button ID="ibtnPrevPageTop" runat="server" OnClick="PrevPage_Click" Text="Previous" class="button prev" />
                        </div>
                        <div class="large-3 columns prev button">
                            <asp:Button ID="ibtnNextPageTop" runat="server" OnClick="NextPage_Click" Text="Next" class="button prev" />
                        </div>
                        <div class="large-3 columns prev button">
                            <asp:Button ID="ibtnLastPageTop" runat="server" OnClick="LastPage_Click" Text="Last" class="button prev" />
                        </div>
                    </div>
                </div>
                <%} %>
            </div>
    
        </div>
    </div>
</asp:Content>

