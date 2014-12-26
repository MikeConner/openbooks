<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="Contributions.aspx.cs" Inherits="Admin_Contributions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="about">

<div class="large-12 columns">
	<div class="gridboxhead">
		<div class="gridboxleft"><h2>Admin :: Campaign Contributions</h2></div>
		<div class="gridboxright"></div>
	</div>
	<div class="results">
		<div class="resultsleft">
			<asp:Label ID="lblPageSize" runat="server" Text="View:" />
			<asp:DropDownList ID="ddlPageSize" runat="server"  
				OnSelectedIndexChanged="ddlSelection_Changed"
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
		<asp:DropDownList ID="ddlOffice" runat="server"
			AutoPostBack="true"
			OnSelectedIndexChanged="ddlSelection_Changed">
				<asp:ListItem Text="-- office --" Value="0" />
				<asp:ListItem Text="Mayor" Value="mayor" />
				<asp:ListItem Text="City Council" Value="council" />
				<asp:ListItem Text="City Controller" Value="controller" />
		</asp:DropDownList>

		<asp:DropDownList ID="ddlCandidateName" runat="server" 
			DataSourceID="CandidateDataSource" 
			DataTextField="CandidateName" 
			DataValueField="ID" 
			AutoPostBack="true"
			AppendDataBoundItems="true"
			OnSelectedIndexChanged="ddlSelection_Changed">
				<asp:ListItem Text="-- candidate --" Value="0" />
		</asp:DropDownList>

        <asp:CheckBox ID="cbApproved" runat="server" AutoPostBack="true" OnCheckedChanged="cbApproved_CheckedChanged"/>
        Approved
		<asp:SqlDataSource ID="CandidateDataSource" runat="server" 
			ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
			SelectCommand="SELECT [ID], [CandidateName] FROM [tlk_candidate] ORDER BY CandidateName ASC">
		</asp:SqlDataSource>
	</div>

		<table class="ob-gridview" cellpadding="0" cellspacing="0">
			<tr>
				<th></th>
				<th><asp:LinkButton ID="LinkButton1" Text="Contributor" OnClick="sortContributor" runat="server" /></th>
				<th><asp:LinkButton ID="LinkButton2" Text="Candidate" OnClick="sortCandidate" runat="server" /></th>
				<th><asp:LinkButton ID="LinkButton3" Text="Office Sought" OnClick="sortOffice" runat="server" /></th>
				<th><asp:LinkButton ID="LinkButton4" Text="Employer" OnClick="sortEmployer" runat="server" /></th>
				<th>Occupation</th>
				<th>Address</th>
				<th><asp:LinkButton ID="LinkButton8" Text="Amount" OnClick="sortAmount" runat="server" /></th>
				<th><asp:LinkButton ID="LinkButton9" Text="Date" OnClick="sortDate" runat="server" /></th>
				<th>Description</th>
			</tr>

<asp:Repeater ID="rptContributions" runat="server" 
				onitemcommand="rptContributions_ItemCommand">
	<ItemTemplate>
		<tr class='<%# Container.ItemIndex % 2 == 0 ? "" : "even" %>' valign="top">
			<td nowrap>
				<asp:LinkButton ID="lb1" runat="server" 
					CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ContributionID") %>' 
					CommandName="edit" CssClass="tiny button " 
					Text="edit" />  
				<asp:LinkButton ID="lb2" runat="server" 
					CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ContributionID") %>' 
					CommandName="delete" 
                    CssClass="tiny button " 
					OnClientClick="javascript:if(!confirm('Delete this item?'))return false;"
					Text="delete" />
                <asp:LinkButton ID="lb3" runat="server" 
                    CommandName="approve"
                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ContributionID") %>'
                    Text="approve"
                    Enabled='<%# (false == (Boolean)DataBinder.Eval(Container.DataItem, "Approved")) && ((string)DataBinder.Eval(Container.DataItem, "CreatedBy") != User.Identity.Name) %>' 
                    CssClass="tiny button " />
			</td>
			<td><%# DataBinder.Eval(Container.DataItem, "ContributorName") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "CandidateName") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "Office") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "Employer")%></td>
			<td><%# DataBinder.Eval(Container.DataItem, "Occupation") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "City")%>, <%# DataBinder.Eval(Container.DataItem, "State")%> <%# DataBinder.Eval(Container.DataItem, "Zip")%></td>
			<td><%# DataBinder.Eval(Container.DataItem, "Amount", "{0:C}")%></td>
			<td><%# DataBinder.Eval(Container.DataItem, "DateContribution", "{0:MM/dd/yyyy}")%></td>
			<td><%# DataBinder.Eval(Container.DataItem, "Description") %></td>
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
</div></div>

</asp:Content>

