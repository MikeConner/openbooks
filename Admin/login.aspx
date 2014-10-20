<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="Admin_login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="about">
        <div class="row">
            <div class="large-12 columns">
                <div class="openbook">
                    <div class="row">
                        <br />
                        <br />

                        <div class="large-3 columns">
                            <b>Username</b>
                        </div>
                        <div class="large-8 columns">
                            <asp:TextBox ID="txtUserName" runat="server" />
                        </div>

                    </div>
                    <div class="row">
                        <div class="large-3 columns">
                            <b>Password</b>
                        </div>
                        <div class="large-8 columns">
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" EnableViewState="false" />
                        </div>
                    </div>
                    <div class="row right">
                        <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" class="button submit" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:Label ID="lblMessage" runat="server" />
</asp:Content>

