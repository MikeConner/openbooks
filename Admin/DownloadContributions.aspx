<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" Async="true" AutoEventWireup="true" CodeFile="DownloadContributions.aspx.cs" Inherits="Admin_DownloadContributions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="about">
        <h2>Download Contributions</h2>
         <div>
            <!-- Select a directory destination for the file. (Depending on your browser, you may need to select a file in that directory.)
        <asp:FileUpload runat="server" id="downloadDir" type="file" webkitdirectory directory multiple /> -->
        </div>
        <!-- <div style="margin-left: 10px; margin-right: 10px;" id="progressbar" />

        <asp:Label ID="lblProgress" runat="server">0%</asp:Label> -->

        <p>
            Click <asp:LinkButton ID="btnDownload" Text="here" runat="server" OnClick="btnDownload_Click" /> to download all contributions to a CSV file.
        </p>
        <asp:Label ID="lblDownloadCompleted" visible="false" runat="server"> 
            Download completed!
        </asp:Label>
   </div>

</asp:Content>

