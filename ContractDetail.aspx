<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="ContractDetail.aspx.cs" Inherits="ContractDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="about">
        <div class="row">
            <div class="large-12 columns">
                <div class="panel">
                    <div class="gridboxhead">
                        <div class="gridboxleft">
                            <h2>Contract Details</h2>
                        </div>
                        <div class="gridboxright"></div>
                    </div>
                    <asp:Repeater ID="rptContractDetails" runat="server" OnItemCommand="rptContracts_ItemCommand">
                        <ItemTemplate>
                            <table class="ob-gridview" cellpadding="0" cellspacing="0">
                                <tr>
                                    <th>Amount</th>
                                    <th>Contract Description</th>
                                    <th>Contract Approval Date</th>
                                    <th>Contract End Date</th>
                                </tr>
                                <tr>
                                    <td><%# DataBinder.Eval(Container.DataItem, "Amount", "{0:C}")%></td>
                                    <td><%# DataBinder.Eval(Container.DataItem, "AggregateDescription") %></td>
                                    <td><%# DataBinder.Eval(Container.DataItem, "OrderDate", "{0:MM/dd/yyyy}")%></td>
                                    <td><%# DataBinder.Eval(Container.DataItem, "CancelDate", "{0:MM/dd/yyyy}")%></td>
                                </tr>
                            </table>
                            <div class="bottomnav">
                                <div class="bottomnavbtns"></div>
                            </div>
                            <br />
                            <div class="contractdetails">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <label>Vendor Name: </label>
                                        </td>
                                        <td><a href="VendorDetail.aspx?ID=<%# DataBinder.Eval(Container.DataItem, "VendorNo") %>">
                                            <%# DataBinder.Eval(Container.DataItem, "VendorName") %></a></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>City Department: </label>
                                        </td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "DeptName") %></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>Contract No.: </label>
                                        </td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "ContractID") %></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>Contract Type: </label>
                                        </td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "OrderType") %></td>
                                        <td>
				<asp:Panel ID="pnlContractPDF" runat="server" Visible='True'>
					<asp:ImageButton ID="ibtnContractPDF" runat="server" 
						ImageUrl="~/img/viewcontract-btn.gif"
						CommandName="ViewPDF" 
						CommandArgument='<%# Eval("ContractID") %>' />
				</asp:Panel>
				</td>
                                    </tr>
                                </table>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <br />
                    <br />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

