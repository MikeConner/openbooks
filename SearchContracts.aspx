<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/MasterPage.master" AutoEventWireup="true" CodeFile="SearchContracts.aspx.cs" 
Inherits="SearchContractsPage" %>
<%@ MasterType virtualpath="~/_Masters/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <div class="search-page">
    <div class="row controls">
      <div class="medium-4 large-6 columns campaign-nav">
        <nav>
          <ul>
            <li><h2>City Contracts</h2></li>
          </ul>
        </nav>
      </div>

      <div class="medium-8 large-6 columns">
        <div class="pagination right">
          <asp:Label ID="lblPageSize" runat="server" Text="View:" />
          <asp:DropDownList ID="ddlPageSize" runat="server"  
				OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged"
				AutoPostBack="true">
					<asp:ListItem Text="10 per page" Value="10" />
					<asp:ListItem Text="25 per page" Value="25" />
					<asp:ListItem Text="50 per page" Value="50"  />
					<asp:ListItem Text="100 per page" Value="100" />
		  </asp:DropDownList>
          <asp:Label ID="lblCurrentPage" runat="server" /> 
        </div>
      </div>
    </div>

    <div class="row">
      <div class="medium-4 large-3 columns">
        <div class="search-field">
          <h2>City Department</h2>
          <asp:DropDownList ID="CityDepartment" runat="server" 
					DataSourceID="SqlDataSource4" 
					DataTextField="DeptName" 
					DataValueField="DeptCode" 
					AppendDataBoundItems="true">
					<asp:ListItem Text="All Organizations" Value="0" />
		  </asp:DropDownList>
		  <asp:SqlDataSource ID="SqlDataSource4" runat="server" 
					ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
					SelectCommand="SELECT [DeptCode], [DeptName] FROM [tlk_department] ORDER BY DeptName" />
 
        </div>

        <div class="search-field">
          <h2>Contract Type</h2>
          <asp:DropDownList ID="ContractType" runat="server" 
					DataSourceID="SqlDataSource1" 
					DataTextField="ServiceName" 
					DataValueField="ID" 
					AppendDataBoundItems="true">
					<asp:ListItem Text="All Services" Value="0" />
		  </asp:DropDownList>
		  <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
					ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
					SelectCommand="SELECT [ID], [ServiceName] FROM [tlk_service] ORDER BY ServiceName" />
        </div>

        <div class="search-field">
          <h2>Vendor Name</h2>
          <asp:RadioButtonList ID="rbVendor" runat="server" RepeatDirection="Vertical">
					<asp:ListItem Text="Begins with" Value="B" />
					<asp:ListItem Text="Contains" Value="C" Selected="True" />
					<asp:ListItem Text="Exact" Value="E" />
		  </asp:RadioButtonList>
		  <asp:TextBox ID="Vendor" runat="server" Placeholder ="Name of Vendor... " />
        </div>

        <div class="search-field">
          <h2>Keyword</h2>
          <asp:TextBox ID="Keywords" runat="server" placeholder="Keywords..."/>
        </div>

        <div class="search-field">
          <h2>Contract Approval Date</h2>
          <div class="row date-select">
            <div class="large-12 columns">
              <label class="date">From:</label>
              <input placeholder="Start" type="date" id="dtmStart" name="dtmStart">
            </div>

            <div class="large-12 columns">
              <label class="date">To:</label>
              <input placeholder="End" type="date" id="dtmFinish" name="dtmFinish">
            </div>
          </div>
        </div>

        <div class="search-field">
          <h2>Contract Amount</h2>
          <div class="range-slider">
            <label>Minimum Amount</label>
            <input class="input-range" max="10000" min="1" type="range" value="250" id="dblMinContract" name="dblMinContract">
            <span id="minContract" class="range-value">250</span>
          </div>
          <div class="range-slider">
            <asp:HiddenField ID="MaxContractField" runat="server" />
            <label>Maximum Amount</label>
            <input class="input-range" max="10000" min="1" type="range" value="250" id="dblMaxContract" name="dblMaxContract">
            <span id="maxContract" class="range-value">250</span>
          </div>
        </div>

        <div class="search-field">
          <asp:Button ID="ImageButton1" runat="server" Text="Search" onclick="btnSearch_Click" CssClass="button" />
        </div>
      </div> 

      <div class="medium-8 large-9 columns">
        <div class="items-container">
          <div class="item">
            <div class="results">
		      <div class="resultsleft"></div>
		      <div class="resultsright"></div>
	        </div>
            <table class="ob-gridview" cellpadding="0" cellspacing="0">
		      <tr>
			    <th><asp:LinkButton ID="LinkButton1" Text="Vendor&nbsp;Name" OnClick="sortVendor" runat="server" /><asp:Image ID="imgSortVendor" runat="server" /></th>
			    <th><asp:LinkButton ID="LinkButton2" Text="Agency&nbsp;Name" OnClick="sortAgency" runat="server" /><asp:Image ID="imgSortAgency" runat="server" /></th>
			    <th><asp:LinkButton ID="LinkButton3" Text="Contract&nbsp;#" OnClick="sortContractID" runat="server" /><asp:Image ID="imgSortContractID" runat="server" /></th>
			    <th><asp:LinkButton ID="LinkButton4" Text="Amount" OnClick="sortAmount" runat="server" /><asp:Image ID="imgSortAmount" runat="server" /></th>
			    <th><asp:LinkButton ID="LinkButton5" Text="Original<br/>Amount" OnClick="sortOriginalAmount" runat="server" /><asp:Image ID="imgSortOriginalAmount" runat="server" /></th>
			    <th><asp:LinkButton ID="LinkButton6" Text="Contract&nbsp;Description" OnClick="sortDescription" runat="server" /><asp:Image ID="imgSortDescription" runat="server" /></th>
			    <th><asp:LinkButton ID="LinkButton9" Text="Contract&nbsp;Type" OnClick="sortContractType" runat="server" /><asp:Image ID="imgSortContractType" runat="server" /></th>
			    <th><asp:LinkButton ID="LinkButton10" Text="Contract Approval Date" OnClick="sortApprovalDate" runat="server" /><asp:Image ID="imgSortApproval" runat="server" /></th>
			    <th><asp:LinkButton ID="LinkButton8" Text="Contract End Date" OnClick="sortEndDate" runat="server" /><asp:Image ID="imgSortEndDate" runat="server" /></th>
			    <th></th>
		      </tr>
	          <asp:Repeater ID="rptContracts" runat="server" OnItemCommand="rptContracts_ItemCommand">
	            <ItemTemplate>
		          <tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "even" %>' valign="top">
			        <td class="vendor"><a href="VendorDetail.aspx?ID=<%# Eval("VendorNo") %>"><%# Eval("VendorName") %></a></td>
			        <td class="agency"><%# Eval("DepartmentName") %></td>
			        <td class="contract">
				      <a href="ContractDetail.aspx?ID=<%# Eval("ContractID") %>&sup=<%# Eval("SupplementalNo") %>">
					    <%# Eval("SupplementalNo").ToString() == "0" ? Eval("ContractID") : Eval("ContractID") + "." + Eval("SupplementalNo")%>
				      </a>
			        </td>
			        <td class="amount"><%# Eval("Amount", "{0:C}")%></td>
			        <td class="oamount"><%# Eval("OriginalAmount", "{0:C}")%></td>
			        <td class="description"><%# Eval("Description") %></td>
		            <td class="contracttype"><%# Eval("ServiceName") %></td>
			        <td class="approvaldate"><%# Eval("DateCountersigned", "{0:MM/dd/yyyy}")%></td>
			        <td class="date"><%# Eval("DateDuration", "{0:MM/dd/yyyy}")%></td>
			        <td valign="middle" style="padding-right: 5px;">
				      <asp:Panel ID="pnlContractPDF" runat="server" Visible='<%# Eval("HasPDF").ToString() == "True" %>'>
					    <asp:ImageButton ID="ibtnContractPDF" runat="server" 
						ImageUrl="~/img/pdficon.gif"
						CommandName="ViewPDF" 
						CommandArgument='<%# Eval("ContractID") %>' />
				      </asp:Panel>
			        </td>
		          </tr>
	            </ItemTemplate>
	          </asp:Repeater>
            </table>

    	    <div class="bottomnav">
		      <div class="bottomnavbtns">
		        <asp:ImageButton ID="ibtnFirstPageTop" runat="server" OnClick="FirstPage_Click" />
			    <asp:ImageButton ID="ibtnPrevPageTop" runat="server" OnClick="PrevPage_Click" />
			    <asp:ImageButton ID="ibtnNextPageTop" runat="server" OnClick="NextPage_Click" />
			    <asp:ImageButton ID="ibtnLastPageTop" runat="server" OnClick="LastPage_Click" />
		      </div>
            </div>
          </div>
        </div>
      </div>

      <div class="large-12 columns pagination-controls">
        <div class="prev">
          <a href="#">Previous</a>
        </div>
        <div class="next">
          <a href="#">Next</a>
        </div>
      </div>
    </div>
  </div>
</asp:Content>

