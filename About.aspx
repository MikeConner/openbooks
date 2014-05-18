<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="About.aspx.cs" 
Inherits="_About" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="mainpagebox">
<div class="infoleftnav">
<ul>
<li><asp:HyperLink NavigateUrl="~/About.aspx" runat="server" ID="About">About</asp:HyperLink></li>
<li><asp:HyperLink NavigateUrl="~/SearchTips.aspx" runat="server" ID="SearchTips">Search Tips</asp:HyperLink></li>
<li><asp:HyperLink NavigateUrl="~/ReportFraud.aspx" runat="server" ID="HyperLink10">Report Fraud</asp:HyperLink></li>
<li><asp:HyperLink NavigateUrl="~/Contact.aspx" runat="server" ID="Contact">Contact</asp:HyperLink></li>
</ul>
</div>

<div class="inforightcontent">
<h3>About Open Book Pittsburgh</h3>
<p>Open Book Pittsburgh, is a searchable database of Pittsburgh city government contracts, administered by Pittsburgh’s Office of the City Controller.  
Open Book Pittsburgh gives Pittsburgh taxpayers access as to how City leaders are spending your tax dollars.</p> 
	 
<p>In addition to city government contracts, Open Book Pittsburgh also gives taxpayers a look as to where elected officials and 
candidates for elected offices in the City of Pittsburgh receive campaign contributions, and how candidates spend those funds.</p>

<h3>About Pittsburgh's City Controller</h3>
<p>The City Controller is one of two, independently elected, citywide offices in the City of Pittsburgh, the other being the Mayor.  The term of office for 
the City Controller is four years, and is not term limited.</p>

<p>The City Controller is the fiscal watchdog for the citizens of the City of Pittsburgh.  It is the job of the Controller to protect city tax dollars from 
waste, fraud and abuse.  The Controller does this by conducting audits of all city departments and city authorities such as the Urban Redevelopment 
Authority (URA), Pittsburgh Parking Authority, Pittsburgh Water and Sewer Authority and the Pittsburgh Housing Authority.  Through audits of city departments 
and authorities, the Controller makes recommendations on how to make those departments more effective, efficient and how to better spend city tax dollars.</p>

<p>In addition to auditing city departments, the City Controller reviews and approves city contracts and is also charged with reporting to the citizens 
of Pittsburgh, the Mayor and City Council the state of the city’s fiscal condition.  Every year, the City Controller issues the 
<a href="http://www.city.pittsburgh.pa.us/co/html/cafr.html" target="_blank">Comprehensive Annual Financial Report (CAFR)</a>.  
The CAFR provides detailed information on Pittsburgh’s short-term and long-term financial outlook.</p>

<p>Lastly, the City Controller sits on the boards of the city’s <a href="http://www.city.pittsburgh.pa.us/pension/index.html" target="_blank">Municipal Pension Fund</a> 
and the <a href="http://www.city.pittsburgh.pa.us/mayor/BAC/bac_pages/comp_municipal_pension.html" target="_blank">Comprehensive Municipal Fund</a>.</p>

</div>
</div>

</asp:Content>

