<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="AddExpenditure.aspx.cs" Inherits="Admin_AddExpenditure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="about">

<div class="large-12 columns">
<h2>Add Campaign Expenditures</h2>

	<table cellpadding="0" cellspacing="0">
		<tr>
			<td><label>Candidate Name</label></td>
			<td><asp:DropDownList ID="ddlCandidateName" runat="server" 
					DataSourceID="CandidateDataSource" 
					DataTextField="CandidateName" 
					DataValueField="ID" />

				<asp:SqlDataSource ID="CandidateDataSource" runat="server" 
					ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
					SelectCommand="SELECT [ID], [CandidateName] FROM [tlk_candidate] ORDER BY CandidateName ASC">
				</asp:SqlDataSource>
			</td>
		</tr>
		<tr>
			<td><label>Office</label></td>
			<td><asp:DropDownList ID="ddlOffice" runat="server">
					<asp:ListItem Text="Mayor" Value="mayor" />
					<asp:ListItem Text="City Council" Value="council" />
					<asp:ListItem Text="City Controller" Value="controller" />
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td><label>Company/Individual Name</label></td>
			<td><asp:TextBox ID="txtCompany" runat="server" Width="250" />
			
				<asp:RequiredFieldValidator id="RequiredValidator1" runat="server" 
					ControlToValidate="txtCompany" 
					ErrorMessage="[error] Required Field. " 
					Display="Dynamic" />
			</td>
		</tr>
				<tr>
			<td><label>Address</label></td>
			<td><asp:TextBox ID="txtAddress1" runat="server" Width="200" /></td>
		</tr>		
		<tr>
			<td><label>City</label></td>
			<td><asp:TextBox ID="txtCity" runat="server" Width="200" /></td>
		</tr>			
		<tr>
			<td><label>State</label></td>
			<td><asp:DropDownList ID="ddlState" runat="server" 
					DataSourceID="SqlDataSource1" 
					DataTextField="state_name" 
					DataValueField="state_code" /> 
				<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
					ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
					SelectCommand="SELECT * FROM [tlk_States] ORDER BY state_name ASC" />
			</td>
		</tr>	
		<tr>
			<td><label>Zip Code</label></td>
			<td><asp:TextBox ID="txtZip" runat="server" Width="100" /></td>
		</tr>
		<tr>
			<td><label>Description</label></td>
			<td><asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Columns="75" Rows="5" /> <br />(example: Campaign blackberry purchase)</td>
		</tr>	
		<tr>
			<td><label>Expenditure Amount</label></td>
			<td><asp:TextBox ID="txtAmount" runat="server" Width="75" />
			
				<asp:CompareValidator id="CheckFormat1" runat="server" 
					ControlToValidate="txtAmount" 
					Operator="DataTypeCheck" 
					Type="Currency"
					ErrorMessage="[error] Illegal format for currency. "
					Display="Dynamic" />
					
				<asp:RangeValidator id="RangeCheck1" runat="server" 
					ControlToValidate="txtAmount"
					Type="Currency" 
					MinimumValue="0.00" MaximumValue="999,999,999.00"
					ErrorMessage="[error] Value greater than $999,999,999.00. " 
					Display="Dynamic" />
			
			</td>
		</tr>
		<tr valign="top">
			<td><label>Expenditure Date</label></td>
			<td><asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
			</td>
		</tr>
	</table>
<asp:ImageButton ID="Button1" runat="server" OnClick="Button1_Click" ImageUrl="~/img/addbtn.gif" />
<asp:ImageButton ID="ImageButton1" runat="server" OnClick="AddAnother_Click" CommandName="AddAnother" ImageUrl="~/img/addanotherbtn.gif" />

<asp:Label ID="lblMessage" runat="server" />
</div>
</div>
</asp:Content>

