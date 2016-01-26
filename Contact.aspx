<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="Contact.aspx.cs"
    Inherits="_Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="contact">
        <div class="row">
            <div class="large-12 columns">
                <h3 class="detail-header">Open Book Contact Information</h3>
                <div class="address">
                    <h6>Controller's Office</h6>
                    <span>104 Courthouse</span>
                    <span>436 Grant Street</span>
                    <span>Pittsburgh, PA 15219</span>
                    <span>Phone: (412) 350-4660</span>
                </div>
                <hr>
            </div>
        </div>
        <div class="row">
            <asp:PlaceHolder ID="formPH" runat="server" Visible="true">
                <div class="large-4 columns">
                    <label>
                        First Name
                        <asp:TextBox ID="FNameTB" runat="server" placeholder="First Name" />
                        <asp:RequiredFieldValidator ID="rfvFName" runat="server" ControlToValidate="FNameTB" ErrorMessage="First Name is required" Display="Dynamic" />
                    </label>
                </div>
                <div class="large-4 columns">
                    <label>
                        Last Name
                        <asp:TextBox ID="LNameTB" runat="server" placeholder="Last Name" />
                        <asp:RequiredFieldValidator ID="rfvLName" runat="server" ControlToValidate="LNameTB" ErrorMessage="Last Name is required" Display="Dynamic" />
                    </label>
                </div>
                <div class="large-4 columns">
                    <div class="row collapse">
                        <label>Email Address</label>
                        <div class="small-12 columns">
                            <asp:TextBox ID="EmailTB" runat="server" placeholder="Email Address" />
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ValidationExpression=".*@.*\..*"
                                ControlToValidate="EmailTB" ErrorMessage="Not a valid Email Address" Display="Dynamic" />
                        </div>
                    </div>
                </div>
                <div class="large-12 columns">
                    <label>
                        Comments
                    </label>
                    <asp:TextBox ID="CommentsTB" runat="server" TextMode="MultiLine" placeholder="Comments" />
                    <asp:RequiredFieldValidator ID="rfvComments" runat="server" ControlToValidate="CommentsTB" ErrorMessage="Comments are required" Display="Dynamic" />
                    Please Enter the Code Below:
                    <asp:CustomValidator ID="validator" runat="server" ControlToValidate="txtVerify" ErrorMessage="You have Entered a Wrong Verification Code! Please Re-Enter and Try Again!" OnServerValidate="CAPTCHAValidate"></asp:CustomValidator>
                    <br />
                    <asp:Image ID="imCaptcha" ImageUrl="Captcha.ashx" runat="server" /><br />
                    <asp:TextBox ID="txtVerify" runat="server"></asp:TextBox><br />
                    <br />
                    <asp:ImageButton ID="btnSubmit" runat="server" AlternateText="Submit" OnClick="SendMail" CssClass="button" />
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="sucessPH" runat="server" Visible="false">
                <p>Thank you for contacting Open Book Allegheny.</p>
                <img src="/img/tankyou-cards.jpg" />
            </asp:PlaceHolder>
        </div>
    </div>
</asp:Content>
