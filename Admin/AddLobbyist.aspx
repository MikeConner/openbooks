<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="AddLobbyist.aspx.cs" 
Inherits="Admin_AddLobbyist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="about">

<div class="large-12 columns">
<h2>Register Lobbyist</h2>

	<table>
		<tr>
			<td><label>Full Name of Lobbyist: </label></td>
			<td><asp:TextBox ID="txtLobbyist" runat="server" Width="200" MaxLength="100" />
			
				<asp:RequiredFieldValidator id="val1" runat="server" 
					ControlToValidate="txtLobbyist" 
					ErrorMessage="[error] Required Field. " 
					Display="Dynamic" 
					ValidationGroup="LobbyistGroup" />
			</td>
		</tr>
		<tr>
			<td><label>Position (i.e. partner/owner/manager): </label></td>
			<td><asp:TextBox ID="txtPosition" runat="server" Width="200" MaxLength="50" />
			
				<asp:RequiredFieldValidator id="val2" runat="server" 
					ControlToValidate="txtPosition" 
					ErrorMessage="[error] Required Field. " 
					Display="Dynamic" 
					ValidationGroup="LobbyistGroup" />
			</td>
		</tr>
		<tr>
			<td><label>Employer Name: </label></td>
			<td><asp:TextBox ID="txtEmployer" runat="server" Width="200" MaxLength="50" />
			
				<asp:RequiredFieldValidator id="val3" runat="server" 
					ControlToValidate="txtEmployer" 
					ErrorMessage="[error] Required Field. " 
					Display="Dynamic"
					ValidationGroup="LobbyistGroup" />
			</td>
		</tr>
		<tr>
			<td><label>Business Address: </label></td>
			<td><asp:TextBox ID="txtEmpAddress" runat="server" Width="200" MaxLength="50" /></td>
		</tr>
		<tr>
			<td><label>City: </label></td>
			<td><asp:TextBox ID="txtEmpCity" runat="server" Width="200" MaxLength="50" /></td>
		</tr>
		<tr>
			<td><label>State: </label></td>
			<td><asp:DropDownList ID="ddlEmpState" runat="server" 
					DataSourceID="dsState" 
					DataTextField="state_name" 
					DataValueField="state_code" 					
					AppendDataBoundItems="true">
						<asp:ListItem Text="-- select --" Value="" />
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td><label>Zip Code: </label></td>
			<td><asp:TextBox ID="txtEmpZip" runat="server" Width="200" MaxLength="50" /></td>
		</tr>
		<tr>
			<td><label>Status (i.e. expired, terminated, completed):</label></td>
			<td><asp:TextBox ID="txtLobbyistStatus" runat="server" Width="200" MaxLength="50" /></td>
		</tr>
        <tr>
            <td><label>On Behalf of City:</label></td>
            <td><asp:CheckBox ID="cbForCity" runat="server"/></td>
        </tr>
	</table>

<br />
<h2>Additional Companies</h2>

	<table>
		<tr>
			<td><label>Company Name: </label></td>
			<td><asp:TextBox ID="txtCompany" runat="server" Width="200" MaxLength="50" />
			
				<asp:RequiredFieldValidator id="val4" runat="server" 
					ControlToValidate="txtCompany" 
					ErrorMessage="[error] Required Field. " 
					Display="Dynamic" 
					ValidationGroup="CompanyGroup" />
			</td>
		</tr>
		<tr>
			<td><label>Address: </label></td>
			<td><asp:TextBox ID="txtAddress" runat="server" Width="200" MaxLength="50" /></td>
		</tr>
		<tr>
			<td><label>City: </label></td>
			<td><asp:TextBox ID="txtCity" runat="server" Width="200" MaxLength="50" /></td>
		</tr>
		<tr>
			<td><label>State: </label></td>
			<td><asp:DropDownList ID="ddlState" runat="server" 
					DataSourceID="dsState" 
					DataTextField="state_name" 
					DataValueField="state_code" 					
					AppendDataBoundItems="true">
						<asp:ListItem Text="-- select --" Value="" />
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td><label>Zip Code: </label></td>
			<td><asp:TextBox ID="txtZip" runat="server" Width="200" MaxLength="50" /></td>
		</tr>
	</table>

	<asp:ImageButton ID="btnAddCompany" runat="server" ImageUrl="~/img/addbtn.gif" OnClick="btnAddLineItems_Click" ValidationGroup="CompanyGroup" /><br />



	<asp:SqlDataSource ID="dsState" runat="server" 
		ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
		SelectCommand="SELECT * FROM [tlk_States] ORDER BY state_name ASC" />
	
	<div class="lobbyistgrid">
		<asp:GridView ID="grvLineItems" runat="server" 
			AutoGenerateColumns="False"
			Width="800px"
			OnRowDeleting="grvLineItems_RowDeleting" 
			EmptyDataText="&nbsp; Please enter companies/organizations represented by lobbyist above.">
			<Columns>
				<asp:CommandField ShowDeleteButton="True" DeleteText="x" />
				<asp:BoundField DataField="company" HeaderText="Company" />
				<asp:BoundField DataField="address" HeaderText="Address" />
				<asp:BoundField DataField="city" HeaderText="City" />
				<asp:BoundField DataField="state" HeaderText="State" />
				<asp:BoundField DataField="zip" HeaderText="Zip" />
			</Columns>
		</asp:GridView>
	</div>

    <asp:ImageButton ID="btnSave" runat="server" ImageUrl="~/img/savebtn.gif" OnClick="Button1_Click" ValidationGroup="LobbyistGroup" />

	<asp:Label ID="lblMessage" runat="server" />

</div></div>

</asp:Content>

