<%@ Page Title ="Open Book Pittsburgh"  Language="C#" MasterPageFile="~/_Masters/MasterPage.master"  AutoEventWireup="true" CodeFile="Default.aspx.cs" 
Inherits="_Default" %>

 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 
<div id="wrap">
	<div id="header">
		<div class="headerwrap">
			<div class="logo">
				<a id="A5" href="~/Default.aspx" runat="server">
					<asp:image id="Img1" ImageUrl="~/img/logo.gif" runat="server" alt="Open Book Pittsburgh" />
				</a>
			</div>
			<div class="controllerlogo">
				<a id="A1" href="http://www.city.pittsburgh.pa.us/co/" target="blank" runat="server">
					<asp:Image ID="Img2" ImageUrl="~/img/controllersymbol.gif" runat="server" alt="Pittsburgh City Controller"/>
				</a>
			</div>
		</div> 
	</div>
	<div class="gridwrap">
<div class="feature">
	<h2>What is Open Book Pittsburgh?</h2>
	<p>Open Book Pittsburgh, is a searchable database of Pittsburgh city government contracts, administered by Pittsburgh’s Office of the City Controller.  
	Open Book Pittsburgh gives Pittsburgh taxpayers access as to how City leaders are spending your tax dollars.</p> 
	 
	<p>In addition to city government contracts, Open Book Pittsburgh also gives taxpayers a look as to where elected officials and 
	candidates for elected offices in the City of Pittsburgh receive campaign contributions, and how candidates spend those funds.</p>
</div>
	<div class="navigation">
		<div class="leftnav">
		<ul>
		<li><span>Search:</span></li>
			<li><asp:HyperLink ID="hl1" Text="City Contracts" NavigateUrl="~/SearchContracts.aspx" runat="server" /></li>
			<li><asp:HyperLink ID="hl2" Text="Campaign Finance" NavigateUrl="~/SearchContributions.aspx" runat="server" /></li>
			<li class="lobbypadhome"><asp:HyperLink ID="HyperLink3" Text="Lobbyists" NavigateUrl="~/SearchLobbyists.aspx" runat="server" /></li>
		</ul>
		
		</div>
		<div class="rightnav">
			<ul>
				<li><asp:HyperLink ID="HyperLink1" Text="About" NavigateUrl="~/About.aspx" runat="server" /></li>
				<li><asp:HyperLink ID="HyperLink2" Text="Search Tips" NavigateUrl="~/SearchTips.aspx" runat="server" /></li>
				<%-- <li><asp:HyperLink ID="HyperLink3" Text="Faq's" NavigateUrl="~/Faqs.aspx" runat="server" /></li> --%>
				<li><asp:HyperLink ID="HyperLink4" Text="Contact" NavigateUrl="~/Contact.aspx" runat="server" /></li>
			</ul>
		</div>
	</div>
<div class="subcontainer">
    <div class="subcontainerleft">

		<h3>About Pittsburgh's City Controller</h3>

		<p>The City Controller is one of two, independently elected, citywide offices in the City of Pittsburgh, the other being the Mayor.  The term of office for 
		the City Controller is four years, and is not term limited.</p>

		<p>The City Controller is the fiscal watchdog for the citizens of the City of Pittsburgh.  It is the job of the Controller to protect city tax dollars from 
		waste, fraud and abuse.  The Controller does this by conducting audits of all city departments and city authorities such as the Urban Redevelopment 
		Authority (URA), Pittsburgh Parking Authority, Pittsburgh Water and Sewer Authority and the Pittsburgh Housing Authority.  Through audits of city departments 
		and authorities, the Controller makes recommendations on how to make those departments more effective, efficient and how to better spend city tax dollars. </p>
		<p><a href="About.aspx">Read More>></a></p>
 </div>
    
    <div class="subcontainerright">
        <div class="subcontainerbox">
			<h2>Quick Links</h2>
			
			<div class="rf"> 
				<a href="ReportFraud.aspx"><img src="img/rf_btn.png" alt="Report Fraud"/></a>
			</div>
			
			<p>See how state leaders are spending your tax dollars. Search Commonwealth of Pennsylvania contracts at <a href="http://contracts.patreasury.org/search.aspx" target="_blank">Pennsylvania Contracts E-Library</a> .</p>

			<p>Other government links:</p>
			<ul>
				<li><a href="http://www.city.pittsburgh.pa.us/co/" target="_blank">Office of the City Controller</a></li>
				<li><a href="http://www.city.pittsburgh.pa.us" target="_blank">City of Pittsburgh</a></li>
				<li><a href="http://www.alleghenycounty.us" target="_blank">Allegheny County</a></li>
				<li><a href="http://www.pa.gov" target="_blank">Commonwealth of Pennsylvania</a></li>
			</ul>
        </div>
    </div>
</div>
</div>
 
 
    

</asp:Content>