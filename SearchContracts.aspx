<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="SearchContracts.aspx.cs"
    Inherits="SearchContractsPage" %>

<%@ MasterType VirtualPath="~/_Masters/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="search-page">
        <div class="row controls">
            <div class="medium-4 large-6 columns campaign-nav">
                <nav>
                    <ul>
                        <li><a href="#">City Contracts</a></li>
                    </ul>
                </nav>
            </div>
            <div class="medium-8 large-6 columns">
                <div class="pagination right">
                    <asp:Label ID="lblPageSize" runat="server" Text="View:" />
                    <asp:DropDownList ID="ddlPageSize" runat="server"
                        OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Text="10 per page" Value="10" />
                        <asp:ListItem Text="25 per page" Value="25" />
                        <asp:ListItem Text="50 per page" Value="50" />
                        <asp:ListItem Text="100 per page" Value="100" />
                    </asp:DropDownList>
                    <asp:Label ID="lblCurrentPage" runat="server" CssClass="results" />
                </div>
            </div>
        </div>
        <div class="row search-row">
            <div class="medium-4 large-3 columns">
                <div class="search-field">
                    <h2>Department</h2>
                    <asp:DropDownList ID="CityDepartment" runat="server" AppendDataBoundItems="true"></asp:DropDownList>
                </div>
                <div class="search-field">
                    <h2>Contract Type</h2>
                    <asp:DropDownList ID="ContractType" runat="server" AppendDataBoundItems="true"></asp:DropDownList>
                </div>
                <div class="search-field">
                    <h2>Vendor Name</h2>
                    <asp:RadioButtonList ID="rbVendor" runat="server" RepeatDirection="Vertical" hidden>
                        <asp:ListItem Text="Begins with" Value="B" />
                        <asp:ListItem Text="Contains" Value="C" Selected="True" />
                        <asp:ListItem Text="Exact" Value="E" />
                    </asp:RadioButtonList>
                    <asp:TextBox ID="Vendor" runat="server" Placeholder="Name of Vendor... " />
                </div>
                <div class="search-field">
                    <h2>Keyword</h2>
                    <asp:TextBox ID="Keywords" runat="server" placeholder="Keyword..." />
                </div>
                <div class="search-field">
                    <h2>Contract Number</h2>
                    <asp:TextBox ID="ContractID" runat="server" placeholder="Contract Number..." />
                </div>
                <div class="search-field">
                    <h2>Contract Approval Date</h2>
                    <div class="row date-select">
                        <div class="large-12 columns">
                            <label class="date">From:</label>
                            <% if (null == sp)
                               { %>
                            <input placeholder="Start" type="date" id="dtmStart" name="dtmStart" value="">
                            <% }
                               else
                               { %>
                            <input placeholder="Start" type="date" id="dtmStart" name="dtmStart" value="<%=sp.beginDate.ToString("yyyy-MM-dd")%>">
                            <%  } %>
                        </div>

                        <div class="large-12 columns">
                            <label class="date">To:</label>
                            <% if (null == sp)
                               { %>
                            <input placeholder="End" type="date" id="dtmFinish" name="dtmFinish" value="">
                            <% }
                               else
                               { %>
                            <input placeholder="End" type="date" id="dtmFinish" name="dtmFinish" value="<%=sp.endDate.ToString("yyyy-MM-dd")%>">
                            <%  } %>
                        </div>
                    </div>
                </div>
                <div class="search-field">
                    <h2>Contract Amount</h2>
                    <div class="range-slider">
                        <label>Minimum Amount</label>
                        <input class="input-range" max="10000" min="1" type="range" value="250" id="dblMinContract" name="dblMinContract">
                        <span id="minContract" class="range-value">1</span>
                    </div>
                    <div class="range-slider">
                        <input type="hidden" id="MaxContractField" value="<%= maxContractAmount %>" />
                        <input type="hidden" id="StickyMinContract" value="<%= stickyMinContract %>" />
                        <input type="hidden" id="StickyMaxContract" value="<%= stickyMaxContract %>" />
                        <label>Maximum Amount</label>
                        <input class="input-range" min="1" type="range" id="dblMaxContract" name="dblMaxContract">
                        <span id="maxContract" class="range-value">36000000</span>
                    </div>
                </div>
                <div class="search-field">
                    <asp:Button ID="ImageButton1" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="button" />
                </div>
            </div>
            <div class="medium-8 large-9 columns">
                <%if ("1" != Request.QueryString["click"]) { %> 
                <div id ="instructions" >
                    <h3>Instructions</h3>
                    <div class="items-container instructions-content">
                        <h4>Searching</h4>
                    <p>Search here for city contracts. You can filter by <b>department</b>, <b>contract type</b>, <b>approval date</b>, or restrict to a contract dollar amount range. You can also search by specific <b>vendor names</b> or <b>keywords</b>.</p>
                        <h4>Search Criteria</h4>
                        <p>
                            <b>Contract Amount</b> can be selected based on a range from the minimum to maximum.  Use the sliders to select the maximum or minimum amounts.
                        </p>
                        <p> <b>Contract Approval Date</b> can be selected based on a range using the two date widgets to the left.  </p>
                        <h4>Search Results</h4>
                    <p>Since the number of results might be quite large, contracts are presented one page at a time. </p>
                        <p>You can set the page size from 10-100, and also sort by most of these criteria (click on the arrow to the right to change the sort direction).</p>
                        <br />
                </div>
            </div> 
                <% } else { %>
                <div class="search-field">
                    <h2>Sort Results by:</h2>
                    <asp:DropDownList ID="ddlSortContracts" CssClass="sort-dropdown" runat="server"
                        OnSelectedIndexChanged="ddlSortContracts_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Text="Contract Amount" Value="amount" />
                        <asp:ListItem Text="Department" Value="DepartmentID" />
                        <asp:ListItem Text="Contract Type" Value="Service" />
                        <asp:ListItem Text="Vendor Name" Value="VendorName" />
                        <asp:ListItem Text="Approval Date" Value="DateCountersigned" />
                    </asp:DropDownList>
                    <asp:ImageButton ID="imgSortDirection" OnClick="toggleSortDirection" runat="server" />
                </div>
                <div class="items-container">
                    <asp:Repeater ID="rptContracts" runat="server" OnItemCommand="rptContracts_ItemCommand">
                        <ItemTemplate>
                            <div class="item">
                                <h2><a href="VendorDetail.aspx?ID=<%# Eval("VendorNo") %>"><%# Eval("VendorName") %>  </a></h2>
                                <div class="price-group">
                                    <span class="original">Currrent Contract Amount: <%# Eval("Amount", "{0:C}")%></span>
                                    <span class="current"><%# Eval("OriginalAmount", "{0:C}")%> Original Contract Amount</span>

                                </div>
                                <div class="label-group">
                                    <div class="label-item">
                                        <div class="type">Type</div>
                                        <div class="title"><%# Eval("ServiceName") %></div>
                                    </div>
                                    <div class="label-item">
                                        <div class="type">Department</div>
                                        <div class="title"><%# Eval("DepartmentName") %></div>
                                    </div>
                                </div>
                                <div class="agenda">
                                    <span class="title">Contract 
                        				     <i><b><a href="ContractDetail.aspx?ID=<%# Eval("ContractID") %>&sup=<%# Eval("SupplementalNo") %>">
                                                 <%# Eval("SupplementalNo").ToString() == "0" ? Eval("ContractID") : Eval("ContractID") + "." + Eval("SupplementalNo")%>
                                             </a></b></i>
                                        Term:</span>
                                    <span><%# Eval("DateCountersigned", "{0:MM/dd/yyyy}")%> —<%# Eval("DateDuration", "{0:MM/dd/yyyy}")%></span>
                                </div>
                                <div class="description">
                                    <p><%# Eval("Description") %></p>
                                </div>
                                <span class="current">
                                    <asp:Panel class="PDFPanel" ID="pnlContractPDF" runat="server" Visible='<%# Eval("HasPDF").ToString() == "True" %>'>
                                        <label>Contract</label>
                                        <asp:ImageButton ID="ibtnContractPDF" runat="server"
                                            ImageUrl="~/img/pdficon.gif"
                                            CommandName="ViewPDF"
                                            CommandArgument='<%# Eval("ContractID") %>' />
                                    </asp:Panel>

                                    <asp:Panel class="PDFPanel" ID="pnlCheckPDF" runat="server" Visible='<%# Eval("HasCheck").ToString() == "True" %>'>
                                        <label>Check</label>
                                        <asp:ImageButton ID="ibtnCheckPDF" runat="server"
                                            ImageUrl="~/img/pdficon.gif"
                                            CommandName="ViewCheck"
                                            CommandArgument='<%# Eval("ContractID") %>' />
                                    </asp:Panel>

                                    <asp:Panel class="PDFPanel" ID="pnlInvoicePDF" runat="server" Visible='<%# Eval("HasInvoice").ToString() == "True" %>'>
                                        <label>Invoice</label>
                                        <asp:ImageButton ID="ibtnInvoicePDF" runat="server"
                                            ImageUrl="~/img/pdficon.gif"
                                            CommandName="ViewInvoice"
                                            CommandArgument='<%# Eval("ContractID") %>' />
                                    </asp:Panel>
                                </span>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="bottomnav">
                    <div class="bottomnavbtns">
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
                </div>
                <% }%>
            </div>
        
        </div>
    </div>
</asp:Content>

