<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="EditVendor.aspx.cs" Inherits="Admin_EditVendor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="about">
<div class="row">
<div class="large-12 columns">
	<h2>Edit Vendor</h2>
<asp:FormView ID="frmVendor" runat="server">
	<ItemTemplate>
	<table class="companydetails" cellpadding="0" cellspacing="0">
		<tr>
			<td><label>Vendor Name</label></td>
			<td><asp:TextBox ID="txtVendorName" runat="server" Width="200" Text='<%# Bind("VendorName") %>'/>
			
				<asp:RequiredFieldValidator id="VendorNameValidator" runat="server" 
					ControlToValidate="txtVendorName" 
					ErrorMessage="[error] Required Field. " 
					Display="Dynamic" />
				
			</td>
		</tr>
		<tr>
			<td><label>Vendor No.</label></td>
			<td><asp:TextBox ID="txtVendorNo" runat="server" Width="75" MaxLength="10" Text='<%# Bind("VendorNo") %>' ReadOnly="true" Enabled="false" />
			
			
				<asp:RequiredFieldValidator id="VendorNoValidator" runat="server" 
					ControlToValidate="txtVendorNo" 
					ErrorMessage="[error] Required Field. " 
					Display="Dynamic" />
				<asp:RegularExpressionValidator ID="VendorNoRegex" runat="server" 
					ControlToValidate="txtVendorNo" 
					ValidationExpression="^[0-9]{10}$"
					ErrorMessage="[error] Vendor Numbers are numeric and 10 digits in length." 
					Display="Dynamic" />
			</td>
		</tr>
		<tr>
			<td><label>Address</label></td>
			<td><asp:TextBox ID="txtAddress1" runat="server" Width="200" Text='<%# Bind("Address1") %>'/>
				<asp:RequiredFieldValidator id="Address1Validator" runat="server" 
					ControlToValidate="txtAddress1" 
					ErrorMessage="[error] Required Field. " 
					Display="Dynamic" />
			</td>
		</tr>		
		<tr>
			<td><label>Address (cont.)</label></td>
			<td><asp:TextBox ID="txtAddress2" runat="server" Width="200" Text='<%# Bind("Address2") %>'/></td>
		</tr>			
		<tr>
			<td><label>Address (cont.)</label></td>
			<td><asp:TextBox ID="txtAddress3" runat="server" Width="200" Text='<%# Bind("Address3") %>'/></td>
		</tr>	
		<tr>
			<td><label>City</label></td>
			<td><asp:TextBox ID="txtCity" runat="server" Width="200" Text='<%# Bind("City") %>' />
				<asp:RequiredFieldValidator id="CityValidator" runat="server" 
					ControlToValidate="txtCity" 
					ErrorMessage="[error] Required Field. " 
					Display="Dynamic" />
			</td>
		</tr>			
		<tr>
			<td><label>State</label></td>
			<td><asp:DropDownList ID="ddlState" runat="server" 
					DataSourceID="SqlDataSource1" 
					DataTextField="state_name" 
					DataValueField="state_code" 
					SelectedValue='<%# Bind("State") %>'
					/> 
				<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
					ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
					SelectCommand="SELECT * FROM [tlk_States] ORDER BY state_name ASC" />
			</td>
		</tr>
		<tr>
			<td><label>Zip Code</label></td>
			<td><asp:TextBox ID="txtZip" runat="server" Width="75" MaxLength="10" Text='<%# Bind("Zip") %>'/>
				<asp:RequiredFieldValidator id="ZipValidator" runat="server" 
					ControlToValidate="txtZip" 
					ErrorMessage="[error] Required Field. " 
					Display="Dynamic" />
				
				<asp:RegularExpressionValidator ID="ZipRegex" runat="server" 
					ControlToValidate="txtZip" 
					ValidationExpression="\d{5}(-\d{4})?" 
					ErrorMessage="[error] Zip code does not appear valid." 
					Display="Dynamic" />
			</td>
		</tr>	
	</table>
<asp:ImageButton ID="Button1" runat="server" OnClick="Button1_Click" ImageUrl="~/img/savebtn.gif" />
<asp:Label ID="lblMessage" runat="server" />

</ItemTemplate>
</asp:FormView>
</div></div></div>
</asp:Content>

