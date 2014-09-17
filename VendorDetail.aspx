<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="VendorDetail.aspx.cs" Inherits="VendorDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="about">
        <div class="row">
            <div class="large-12 columns">
                <div class="panel">
                    <div class="gridboxhead">
                        <div class="gridboxleft ">
                            <h1>Vendor Details</h1>
                        </div>
                        <div class="gridboxright"></div>
                    </div>

                    <asp:Repeater ID="rptCompanyInfo" runat="server">
                        <HeaderTemplate>
                            <div class="companyinfo">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <h3><%# DataBinder.Eval(Container.DataItem, "VendorName") %></h3>

                            <div class="large-12 column">
                                <span>
                                    <h6><%# DataBinder.Eval(Container.DataItem, "Address1") %>
                                        <%# DataBinder.Eval(Container.DataItem, "Address2") %>
                                        <%# DataBinder.Eval(Container.DataItem, "Address3") %>
                                        <%# DataBinder.Eval(Container.DataItem, "City") %>, 
					                    <%# DataBinder.Eval(Container.DataItem, "State") %>
                                        <%# DataBinder.Eval(Container.DataItem, "Zip") %></h6>
                                </span>
                            </div>
                            <hr />
                        </ItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <br />
                <br />
                <div class="gridboxhead">
                    <h2>Contracts</h2>
                </div>
                <div class="results">
                    <label>Results</label>
                </div>
                <asp:Repeater ID="rptVendorDetails" runat="server">
                    <HeaderTemplate>
                        <table class="ob-gridview" cellpadding="0" cellspacing="0">
                            <tr>
                                <th>Contract #</th>
                                <th>Amount</th>
                                <th>Original Amount</th>
                                <th>Contract Description</th>
                                <th>Contract Approval Date</th>
                                <th>Contract End Date</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "even" %>' valign="top">
                            <td>
                                <a href="ContractDetail.aspx?ID=<%# Eval("ContractID") %>&sup=<%# Eval("SupplementalNo") %>">
                                    <%# Eval("SupplementalNo").ToString() == "0" ? Eval("ContractID") : Eval("ContractID") + "." + Eval("SupplementalNo")%>
                                </a>
                            </td>
                            <td><%# DataBinder.Eval(Container.DataItem, "Amount", "{0:C}") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "OriginalAmount", "{0:C}") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "Description") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "DateCountersigned", "{0:MM/dd/yyyy}")%></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "DateDuration", "{0:MM/dd/yyyy}")%></td>
                        </tr>

                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                        <div class="bottomnav">
                            <div class="bottomnavbtns">
                            </div>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
</asp:Content>

