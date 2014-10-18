<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="Lobbyists.aspx.cs" 
Inherits="Admin_Lobbyists" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="about">

<div class="large-12 columns">
	<div class="gridboxhead">
		<div class="gridboxleft"><h2>Admin :: Lobbyists</h2></div>
		<div class="gridboxright"></div>
	</div>
	<div class="results">
		<div class="resultsleft">
			<asp:Label ID="lblPageSize" runat="server" Text="View:" />
			<asp:DropDownList ID="ddlPageSize" runat="server"  
				OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged"
				AutoPostBack="true">
					<asp:ListItem Text="10 per page" Value="10" />
					<asp:ListItem Text="25 per page" Value="25" />
					<asp:ListItem Text="50 per page" Value="50"  />
					<asp:ListItem Text="100 per page" Value="100" />
			</asp:DropDownList>
		</div>
		<div class="resultsright">
			<asp:Label ID="lblCurrentPage" runat="server" />
		</div>
	</div>
	<div class="dropboxadmin">
	
		<asp:DropDownList ID="ddlLobbyists" runat="server" 
			DataSourceID="dsLobbyists"
			DataTextField="FullName"
			DataValueField="ID" 
			AutoPostBack="true"
			AppendDataBoundItems="true"
			OnSelectedIndexChanged="ddlLobbyists_SelectedIndexChanged">
				<asp:ListItem Text="-- candidate --" Value="0" />
		</asp:DropDownList>			


		<asp:SqlDataSource ID="dsLobbyists" runat="server" 
			ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
			SelectCommand="SELECT [ID], [FullName] FROM [lobbyists] ORDER BY FullName ASC"  >
		</asp:SqlDataSource>
	
		<asp:Label ID="lbl1" runat="server" Text="Search by Company: " />
		<asp:TextBox ID="txtCompanySearch" runat="server" MaxLength="100" />
		<asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/searchbutton.gif" OnClick="btnSearch_Click" CssClass="searchbutton"  />

	</div>
		<table class="ob-gridview" cellpadding="0" cellspacing="0">
			<tr>
				<th></th>
				<th><asp:LinkButton ID="lb1" Text="Full&nbsp;Name" OnClick="sortLobbyist" runat="server" /></th>
				<th>Position</th>
				<th><asp:LinkButton ID="lb2" Text="Employer" OnClick="sortEmployer" runat="server" /></th>
				<th>Additional Companies</th>
				<!--<th><asp:LinkButton ID="lb3" Text="Date&nbsp;Entered" OnClick="sortDate" runat="server" /></th>-->
				<th>Status&nbsp;</th>
			</tr>

<asp:Repeater ID="rptLobbyists" runat="server" 
	OnItemCommand="rptLobbyists_ItemCommand" 
	OnItemDataBound="rptLobbyists_ItemDataBound">
	<ItemTemplate>
		<tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "even" %>' valign="top">
			<td nowrap>
				<asp:LinkButton ID="lb1" runat="server" 
					CommandArgument='<%# DataBinder.Eval(Container.DataItem, "LobbyistID") %>' 
					CommandName="edit" 
                    CssClass =" tiny button"
					Text="edit" /> 
				<asp:LinkButton ID="lb2" runat="server" 
					CommandArgument='<%# DataBinder.Eval(Container.DataItem, "LobbyistID") %>' 
					CommandName="delete" 
                    CssClass =" tiny button"
					OnClientClick="javascript:if(!confirm('Delete this item?'))return false;"
					Text="delete" />
			</td>
			<td><%# Eval("LobbyistName") %></td>
			<td><%# Eval("Position") %></td>
			<td>
				<b><%# Eval("EmployerName")%></b><br/>
				<%# Eval("Address")%><br/>
				<%# Eval("City") + " " + Eval("State") + ", " + Eval("Zip")%><br/>
			</td>
			<td><asp:Repeater ID="rptCompanies" runat="server">
					<ItemTemplate>
						<b><%# Eval("CompanyName")%></b>,
						<%# Eval("Address")%>,
						<%# Eval("City") + " " + Eval("State") + ", " + Eval("Zip")%><br/>
					</ItemTemplate>
				</asp:Repeater>
			</td>
			<!--<td class="date"><%# DataBinder.Eval(Container.DataItem, "DateEntered", "{0:MM/dd/yyyy}")%></td>-->
			<td><%#Eval ("LobbyistStatus") %>&nbsp;&nbsp;</td>
		</tr>
	</ItemTemplate>
</asp:Repeater>
   
</table>
   <div class="bottomnav">
    <div class="bottomnavbtns">
        <asp:ImageButton ID="ibtnFirstPageTop" runat="server" OnClick="FirstPage_Click" ImageUrl="~/img/firstbtn.gif" />
        <asp:ImageButton ID="ibtnPrevPageTop" runat="server" OnClick="PrevPage_Click" ImageUrl="~/img/previousbtn.gif" />
        <asp:ImageButton ID="ibtnNextPageTop" runat="server" OnClick="NextPage_Click" ImageUrl="~/img/nextbtn.gif" />
        <asp:ImageButton ID="ibtnLastPageTop" runat="server" OnClick="LastPage_Click" ImageUrl="~/img/lastbtn.gif" />
    </div>
    </div>
</div>
    </div>


</asp:Content>

