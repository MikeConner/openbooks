﻿<div id="wrap">
	<div id="headerinternal">
		<div class="headerwrapinternal">
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
	<div class="internalnavigation">
		<div class="internalleftnav">
	  <ul>
		<li><asp:HyperLink ID="HyperLink3" Text="Home" NavigateUrl="~/Default.aspx" runat="server" /></li>
		<li><span>Search:</span></li>
		<li><asp:HyperLink ID="hl1" Text="City Contracts" NavigateUrl="~/SearchContracts.aspx" runat="server" /></li>
		<li><asp:HyperLink ID="hl2" Text="Campaign Finance" NavigateUrl="~/SearchContributions.aspx" runat="server" /></li>
		<li class="lobbypad"><asp:HyperLink ID="HyperLink6" Text="Lobbyists" NavigateUrl="~/SearchLobbyists.aspx" runat="server" /></li>
		</ul>
		</div>
		<div class="internalrightnav">
			<ul>
				<li><asp:HyperLink ID="HyperLink1" Text="About" NavigateUrl="~/About.aspx" runat="server" /></li>
				<li><asp:HyperLink ID="HyperLink2" Text="Search Tips" NavigateUrl="~/SearchTips.aspx" runat="server" /></li>
				<li><asp:HyperLink ID="HyperLink10" Text="Report Fraud" NavigateUrl="~/ReportFraud.aspx" runat="server" /></li>
				<li><asp:HyperLink ID="HyperLink4" Text="Contact" NavigateUrl="~/Contact.aspx" runat="server" /></li>
			</ul>
		</div>
	 </div>

    
<div id="footerinternal">
	<div class="footerwrapinternal">
		<div class="footerleft">
			<div class="copyright">
			<p>Copyright <asp:Literal ID="CopyYear" runat="server" />, Pittsburgh City Controller</p>
			</div>
		 </div>
		 <div class="footerright">
			<div class="contactus">
			<p><asp:HyperLink ID="HyperLink5" Text="Contact Us" NavigateUrl="~/Contact.aspx" runat="server" /></p>
			</div>
		</div>
	</div>
</div>