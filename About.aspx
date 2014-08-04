<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="About.aspx.cs" 
Inherits="_About" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="mainpagebox">
<div class="infoleftnav">

<div class="about">
<div class="row">
<div class="large-12 columns">
<div class="openbook">
<h3>About Open Book Pittsburgh</h3>
<h5 class="subheader">Open Book Pittsburgh, is a searchable database of Pittsburgh city government contracts, administered by Pittsburgh’s Office of the City Controller. Open Book Pittsburgh gives Pittsburgh taxpayers access as to how City leaders are spending your tax dollars.</h5>
<p>In addition to city government contracts, Open Book Pittsburgh also gives taxpayers a look as to where elected officials and candidates for elected offices in the City of Pittsburgh receive campaign contributions, and how candidates spend those funds.</p>
</div>
<hr>
<div class="controller">
<h3>About Pittsburgh's City Controller</h3>
<div class="row">
<div class="large-8 columns">
<p>The City Controller is one of two, independently elected, citywide offices in the City of Pittsburgh, the other being the Mayor. The term of office for the City Controller is four years, and is not term limited.</p>
<p>The City Controller is the fiscal watchdog for the citizens of the City of Pittsburgh. It is the job of the Controller to protect city tax dollars from waste, fraud and abuse. The Controller does this by conducting audits of all city departments and city authorities such as the Urban Redevelopment Authority (URA), Pittsburgh Parking Authority, Pittsburgh Water and Sewer Authority and the Pittsburgh Housing Authority. Through audits of city departments and authorities, the Controller makes recommendations on how to make those departments more effective, efficient and how to better spend city tax dollars.</p>
</div>
<div class="large-4 columns">
<img alt="Lamb" src="img/lamb.jpg">
</div>
</div>
<div class="row">
<div class="large-12 columns">
<blockquote>
In addition to auditing city departments, the City Controller reviews and approves city contracts and is also charged with reporting to the citizens of Pittsburgh, the Mayor and City Council the state of the city’s fiscal condition. Every year, the City Controller issues the Comprehensive Annual Financial Report (CAFR). The CAFR provides detailed information on Pittsburgh’s short-term and long-term financial outlook.
<cite>Open Book</cite>
</blockquote>
<p>Lastly, the City Controller sits on the boards of the city’s Municipal Pension Fund and the Comprehensive Municipal Fund.</p>

<ul>
<li><asp:HyperLink NavigateUrl="~/SearchTips.aspx" runat="server" ID="SearchTips">Search Tips</asp:HyperLink></li>
<li><asp:HyperLink NavigateUrl="~/ReportFraud.aspx" runat="server" ID="HyperLink10">Report Fraud</asp:HyperLink></li>
<li><asp:HyperLink NavigateUrl="~/Contact.aspx" runat="server" ID="Contact">Contact</asp:HyperLink></li>
</ul>

</div>
</div>
</div>
</div>
</div>
</div>
</div>

</asp:Content>

