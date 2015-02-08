<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="SyncContracts.aspx.cs" 
Inherits="Admin_SyncContracts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="mainwrap">
        <div class="gridboxhead">
            <div class="gridboxleft">
                <h2>Onbase Data Synchronization</h2>
            </div>
            <div class="gridboxright"></div>
        </div>
    </div>
    <div class="results">
        <asp:Label ID="lblSyncStatus" runat="server" Text="Ready to Synchronize Data"/>
        <br />
        <br />
        <asp:Button ID="btnSync" OnClick="SynchronizeData" Text="Synchronize Data" runat="server" />
    </div>
</asp:Content>
