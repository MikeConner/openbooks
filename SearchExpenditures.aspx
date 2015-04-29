<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="SearchExpenditures.aspx.cs" Inherits="SearchExpendituresPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                    <span>
                        <asp:Label ID="lblPageSize" runat="server" Text="View:" />
                        <asp:DropDownList ID="ddlPageSize" runat="server"
                            OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged"
                            AutoPostBack="true">
                            <asp:ListItem Text="10 per page" Value="10" />
                            <asp:ListItem Text="25 per page" Value="25" />
                            <asp:ListItem Text="50 per page" Value="50" />
                            <asp:ListItem Text="100 per page" Value="100" />
                        </asp:DropDownList>
                        <asp:Label ID="lblCurrentPage" runat="server" />
                    </span>
                </div>
            </div>
        </div>
        <div class="row search-row">
            <div class="medium-4 large-3 columns">
                <div class="search-field">
                    <h2>Candidate</h2>
                    <asp:DropDownList ID="ddlCandidateName" runat="server" AppendDataBoundItems="true"></asp:DropDownList>
                </div>
                <div class="search-field">
                    <h2>Office Sought</h2>
                    <asp:DropDownList ID="ddlOffice" runat="server">
                        <asp:ListItem Text="All" Value="all" />
                        <asp:ListItem Text="Mayor" Value="mayor" />
                        <asp:ListItem Text="City Council" Value="council" />
                        <asp:ListItem Text="City Controller" Value="controller" />
                    </asp:DropDownList>
                </div>
                <div class="search-field">
                    <h2>Year</h2>
                    <asp:DropDownList ID="ddldatePaid" runat="server" AppendDataBoundItems="true" />
                </div>
                <div class="search-field">
                    <h2>Company or Individual</h2>
                    <asp:RadioButtonList ID="rblVendorSearchOptions" runat="server" RepeatDirection="Horizontal" hidden>
                        <asp:ListItem Text="Begins with" Value="B" />
                        <asp:ListItem Text="Contains" Value="C" Selected="True" />
                        <asp:ListItem Text="Exact" Value="E" />
                    </asp:RadioButtonList>
                    <asp:TextBox ID="txtVendor" runat="server" placeholder="Name of vendor..." />
                </div>
                <div class="search-field">
                    <h2>Keywords</h2>
                    <asp:TextBox ID="txtKeywords" runat="server" placeholder="Keywords..." />
                </div>
                <div class="search-field">
                    <asp:Button ID="ImageButton1" runat="server" Text="Search" AlternateText="Search" OnClick="btnSearch_Click" CssClass="submit button" />
                </div>
            </div>
            <div class="medium-8 large-9 columns">

                 <%if ("1" != Request.QueryString["click"]) { %> 
                <div id="instructions">
                    <h3>Instructions</h3>
                    <div class="items-container instructions-content">


                   <h4>Searching</h4>
                     <p>Search here for campaign expenditures.</p>
                        <h4>Search Criteria</h4>
                        <p>
                            You can filter by candidate, office, or date.</p>
                        <p>You can also search using specific contributors or employers.</p>
                        
                        <p> Finally, you can limit the search to a geographic area centered on a zipcode.
                        </p>
                        <h4>Search Results</h4>

                        <p>Since the number of results might be quite large, expenditures are presented one page at a time. You can set the page size from 10-100.</p>
                        
                        <p> You can also sort by most of these criteria. (Click on the arrow to the right to change the sort direction)</p>





                       
                        
                    </div>
                </div> 
                <% } else { %>    
                <div class="search-field">
                    <h2>Sort Results by:</h2>
                    <asp:DropDownList ID="ddlSortExpenditures" CssClass="sort-dropdown" runat="server"
                        OnSelectedIndexChanged="ddlSortExpenditures_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Text="Candidate" Value="CandidateName" />
                        <asp:ListItem Text="Office" Value="Office" />
                        <asp:ListItem Text="Company/Individual" Value="CompanyName" />
                        <asp:ListItem Text="Amount" Value="Amount" />
                        <asp:ListItem Text="Date" Value="DatePaid" />
                    </asp:DropDownList>
                    <asp:ImageButton ID="imgSortDirection" OnClick="toggleSortDirection" runat="server" />
                </div>
                <div class="items-container">
                    <asp:Repeater ID="rptExpenditures" runat="server">
                        <ItemTemplate>
                            <div class="item">
                                <span class="name-label">Company or Individual</span>
                                <h2><%# DataBinder.Eval(Container.DataItem, "CompanyName") %></h2>
                                <div class="label-group">
                                    <div class="label-item">
                                        <div class="type">Office sought</div>
                                        <div class="title"><%# DataBinder.Eval(Container.DataItem, "Office") %></div>
                                    </div>
                                    <div class="label-item">
                                        <div class="type">Candidate</div>
                                        <div class="title"><%# DataBinder.Eval(Container.DataItem, "CandidateName") %></div>
                                    </div>
                                </div>
                                <div class="details">
                                    <ul>
                                        <li>
                                            <span class="key">Date</span>
                                            <span class="value"><%# DataBinder.Eval(Container.DataItem, "DatePaid", "{0:MM/dd/yyyy}")%></span>
                                        </li>
                                        <li>
                                            <span class="key">Amount</span>
                                            <span class="value"><%# DataBinder.Eval(Container.DataItem, "Amount", "{0:C}")%></span>
                                        </li>
                                        <li>
                                            <span class="key">Address</span>
                                            <span class="value"><%# DataBinder.Eval(Container.DataItem, "Address1")%>, <%# DataBinder.Eval(Container.DataItem, "City")%>, <%# DataBinder.Eval(Container.DataItem, "State")%> <%# DataBinder.Eval(Container.DataItem, "Zip")%></span>
                                        </li>
                                    </ul>
                                </div>
                                <div class="description">
                                    <span><%# DataBinder.Eval(Container.DataItem, "Description") %></span>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
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
                <%} %>
            </div>
    
        </div>
    </div>
</asp:Content>

