<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="Faqs.aspx.cs" 
Inherits="_Faqs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="mainpagebox">
<div class="infoleftnav">
<ul>
<li><asp:HyperLink NavigateUrl="~/About.aspx" runat="server" ID="About">About</asp:HyperLink></li>
<li><asp:HyperLink NavigateUrl="~/SearchTips.aspx" runat="server" ID="SearchTips">Search Tips</asp:HyperLink></li>
<li><asp:HyperLink NavigateUrl="~/Faqs.aspx" runat="server" ID="Faqs">Faqs</asp:HyperLink></li>
<li><asp:HyperLink NavigateUrl="~/Contact.aspx" runat="server" ID="Contact">Contact</asp:HyperLink></li>
</ul>
</div>

<div class="inforightcontent">
<h3>Faq's</h3>
<p>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p>
<p>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p>
</div>
</div>
</asp:Content>

