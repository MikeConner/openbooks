<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="UploadExpenditures.aspx.cs" Inherits="Admin_UploadExpendituresPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="about">
        <h2>Upload Campaign Expenditures</h2>

        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <label>Candidate Name</label></td>
                <td>
                    <asp:DropDownList ID="ddlCandidateName" runat="server"
                        DataSourceID="CandidateDataSource"
                        DataTextField="CandidateName"
                        Enabled="false"
                        DataValueField="ID" />

                    <asp:SqlDataSource ID="CandidateDataSource" runat="server"
                        ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>"
                        SelectCommand="SELECT [ID], [CandidateName] FROM [tlk_candidate] ORDER BY CandidateName ASC"></asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td>
                    <label>Candidate's office</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlOffice" runat="server">
                        <asp:ListItem>Mayor</asp:ListItem>
                        <asp:ListItem>City Council</asp:ListItem>
                        <asp:ListItem>City Controller</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <asp:FileUpload ID="FileUpload1"
            runat="server"></asp:FileUpload>
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

        <br />
        <br />
        <asp:ListBox ID="UploadErrors" runat="server" Enabled="false" Height="100" />

        <hr />

        <asp:Label ID="LengthLabel"
            runat="server">
        </asp:Label>

        <br />

        <asp:Label ID="ContentsLabel"
            runat="server">
        </asp:Label>

        <asp:PlaceHolder ID="PlaceHolder1"
            runat="server"></asp:PlaceHolder>
        <h2>Instructions</h2>
        <ul>
            <li>Download the .csv template to a local file</li>
            <li>Open in Excel and enter one contribution per row</li>
            <li>Choose values for fields that apply to the entire set of contributions (Candidate Name, Office)</li>
            <li>Enter values for all other fields, per the template.</li>
            <li><strong>You must re-upload as a .csv file, not a native Excel file.</strong> In Excel, "Save As" and choose the "CSV (comma delimited)" format</li>
            <li>Use the file chooser and upload button above to upload your .csv file to the server</li>
        </ul>

        <asp:HyperLink href="/documents/ExpenditureTemplate.csv" runat="server">
                <h3>Click here to download the template</h3>
        </asp:HyperLink>
        <br />
        <br />
        <br />
        <br />
    </div>
</asp:Content>

