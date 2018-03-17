<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="ElectedOfficials.aspx.cs"
    Inherits="ElectedOfficials" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="mainwrap">
        <div class="gridboxhead">
            <div class="gridboxleft">
                <h2>Elected Officials</h2>
            </div>
            <div class="gridboxright"></div>
        </div>

        <table class="ob-gridview" cellpadding="0" cellspacing="0">
            <tr>
                <th>Image</th>
                <th>
                    <asp:LinkButton ID="lb1" Text="Name" OnClick="sortOfficial" runat="server" /></th>
                <th>Office</th>
                <th>Committee</th>
                <th>
                    <asp:LinkButton ID="lb2" Text="Salary" OnClick="sortSalary" runat="server" /></th>
                <th>Personal Page</th>
                <th>Disclosure Page</th>
            </tr>
            <asp:Repeater ID="rptOfficials" runat="server">
                <ItemTemplate>
                    <tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "even" %>' valign="top">
                        <td>
                            <!--<asp:Image ImageUrl="data:image;base64," + Convert.ToBase64String(<%# Eval("ImageUrl") %>)" />-->
                            <asp:Image height="100" Width="100" ImageUrl=<%# WrapFilename(Eval("ImageUrl").ToString()) %> runat="server" />
                        </td>
                        <td><%# Eval("Name") %></td>
                        <td><%# Eval("Office") %></td>
                        <td><%# Eval("Committee") %></td>
                        <td><%# string.Format("{0:C}", Eval("Salary")) %></td>
                        <td>
                            <asp:HyperLink NavigateUrl=<%# Eval("PersonalPage") %> Target="_blank" Text='<%# string.IsNullOrEmpty(Eval("PersonalPage").ToString()) ? "Unavailable" : "Link"%>' runat="server"></asp:HyperLink>
                        </td>
                        <td>
                            <asp:HyperLink NavigateUrl=<%# Eval("DisclosureLink") %> Target="_blank" Text='<%# string.IsNullOrEmpty(Eval("DisclosureLink").ToString()) ? "Unavailable" : "Link"%>' runat="server"></asp:HyperLink>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
</asp:Content>

