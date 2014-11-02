<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="AddContribution.aspx.cs" Inherits="Admin_AddContributionPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="about">
        <div class="large-12 columns">
            <h2>Add Campaign Contributions</h2>
            <div class="large-8 columns">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <label>Candidate Name</label></td>
                        <td>
                            <asp:DropDownList ID="ddlCandidateName" runat="server"
                                DataSourceID="CandidateDataSource"
                                DataTextField="CandidateName"
                                DataValueField="ID" />

                            <asp:SqlDataSource ID="CandidateDataSource" runat="server"
                                ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>"
                                SelectCommand="SELECT [ID], [CandidateName] FROM [tlk_candidate] ORDER BY CandidateName ASC"></asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Office</label></td>
                        <td>
                            <asp:DropDownList ID="ddlOffice" runat="server">
                                <asp:ListItem Text="Mayor" Value="mayor" />
                                <asp:ListItem Text="City Council" Value="council" />
                                <asp:ListItem Text="City Controller" Value="controller" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Contributor Type</label></td>
                        <td>
                            <asp:RadioButtonList ID="rblContributor" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Commitee" Value="co" />
                                <asp:ListItem Text="Contributor" Value="in" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Contributor/Commitee Name</label></td>
                        <td>
                            <asp:TextBox ID="txtContributor" runat="server" Width="250" />

                            <asp:RequiredFieldValidator ID="RequiredValidator1" runat="server"
                                ControlToValidate="txtContributor"
                                ErrorMessage="[error] Required Field. "
                                Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Contribution Type</label></td>
                        <td>
                            <asp:DropDownList ID="ddlContributionType" runat="server"
                                DataSourceID="ContributionTypeDataSource"
                                DataTextField="ContributionType"
                                DataValueField="ID" />

                            <asp:SqlDataSource ID="ContributionTypeDataSource" runat="server"
                                ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>"
                                SelectCommand="SELECT [ID], [ContributionType] FROM [tlk_contributionType]"></asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            <label>Contribution Description</label></td>
                        <td>
                            <asp:TextBox ID="txtDescription" TextMode="MultiLine" Width="400" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>City</label></td>
                        <td>
                            <asp:TextBox ID="txtCity" runat="server" Width="200" /></td>
                    </tr>
                    <tr>
                        <td>
                            <label>State</label></td>
                        <td>
                            <asp:DropDownList ID="ddlState" runat="server"
                                DataSourceID="SqlDataSource1"
                                DataTextField="state_name"
                                DataValueField="state_code" />
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>"
                                SelectCommand="SELECT * FROM [tlk_States] ORDER BY state_name ASC" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Zip Code</label></td>
                        <td>
                            <asp:TextBox ID="txtZip" runat="server" Width="100" /></td>
                    </tr>
                    <tr>
                        <td>
                            <label>Employer</label></td>
                        <td>
                            <asp:TextBox ID="txtEmployer" runat="server" Width="250" /></td>
                    </tr>
                    <tr>
                        <td>
                            <label>Occupation</label></td>
                        <td>
                            <asp:TextBox ID="txtOccupation" runat="server" Width="250" /></td>
                    </tr>
                    <tr>
                        <td>
                            <label>Contribution Amount</label></td>
                        <td>
                            <asp:TextBox ID="txtAmount" runat="server" Width="75" />

                            <asp:CompareValidator ID="CheckFormat1" runat="server"
                                ControlToValidate="txtAmount"
                                Operator="DataTypeCheck"
                                Type="Currency"
                                ErrorMessage="[error] Illegal format for currency. "
                                Display="Dynamic" />

                            <asp:RangeValidator ID="RangeCheck1" runat="server"
                                ControlToValidate="txtAmount"
                                Type="Currency"
                                MinimumValue="0.00" MaximumValue="999,999,999.00"
                                ErrorMessage="[error] Value greater than $999,999,999.00. "
                                Display="Dynamic" />

                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            <label>Contribution Date</label></td>
                        <td>
                            <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:ImageButton ID="Button1" runat="server" OnClick="Button1_Click" CommandName="Add" ImageUrl="~/img/addbtn.gif" />
                <asp:ImageButton ID="ImageButton1" runat="server" OnClick="AddAnother_Click" CommandName="AddAnother" ImageUrl="~/img/addanotherbtn.gif" />
                <asp:Label ID="lblMessage" runat="server" />
            </div>
            <div class="large-4 columns">
                <h4>Or select a .csv file to upload:</h4>

                <asp:FileUpload ID="FileUpload1"
                    runat="server"></asp:FileUpload>
                <asp:CustomValidator ID="CustomValidator1" runat="server"
                    ControlToValidate="FileUpload1"
                    ClientValidationFunction="validateContributionUpload" />
                <br />
                <br />

                <asp:Button ID="UploadButton"
                    Text="Upload file"
                    OnClick="UploadButton_Click"
                    runat="server"></asp:Button>

                <br />
                <br />

                <asp:Label ID="UploadStatusLabel"
                    runat="server">
                </asp:Label>

                <asp:ListBox ID="UploadErrors" runat="server" Enabled="false"/>

                <hr />

                <asp:Label ID="LengthLabel"
                    runat="server">
                </asp:Label>

                <br />
                <br />

                <asp:Label ID="ContentsLabel"
                    runat="server">
                </asp:Label>

                <asp:PlaceHolder ID="PlaceHolder1"
                    runat="server"></asp:PlaceHolder>
                <asp:HyperLink href="/documents/ContributionTemplate.csv" runat="server">
                <label>Click here to download a template</label>
                </asp:HyperLink>               
            </div>
        </div>
    </div>
</asp:Content>

