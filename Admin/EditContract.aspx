<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="EditContract.aspx.cs" 
Inherits="Admin_EditContract" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="mainwrap">
<h2>Edit Contract</h2>

<asp:FormView ID="frmContract" runat="server">
	<ItemTemplate>
	<table cellpadding="0" cellspacing="0">
	<tr>
		<td><label>Contract No.</label></td>
		<td><asp:TextBox ID="TextBox1" runat="server" Width="100" Text='<%# Bind("ContractID") %>' ReadOnly="true" Enabled="false"/></td>
	</tr>
	<tr>
		<td><label>Supplement No.</label></td>
		<td><asp:TextBox ID="txtSupplementalNo" runat="server" Width="25" Text='<%# Bind("SupplementalNo") %>'/> (example: 1)</td>
	</tr>
	<tr>
		<td><label>Resolution No.</label></td>
		<td><asp:TextBox ID="txtResolutionNo" runat="server" Width="200" Text='<%# Bind("ResolutionNo") %>' /> (example: RS-320-99, 210-00)</td>
	</tr>
	<tr>
		<td><label>City Department</label></td>
		<td><asp:DropDownList ID="ddlDepartment" runat="server" 
				DataSourceID="SqlDataSource4" 
				DataTextField="DeptName" 
				DataValueField="DeptCode" 
				SelectedValue='<%# Bind("DepartmentID") %>'
				AppendDataBoundItems="true" 
				>
				<asp:ListItem Text="" Value="0" />
			</asp:DropDownList>
			<asp:SqlDataSource ID="SqlDataSource4" runat="server" 
				ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
				SelectCommand="SELECT [DeptCode], [DeptName] FROM [tlk_department] ORDER BY DeptName" />
		</td>
	</tr>
		<tr>
			<td><label>Vendor</label></td>
			<td><asp:DropDownList ID="ddlVendors" runat="server" 
				DataSourceID="SqlDataSource2" 
				DataTextField="VendorName" 
				DataValueField="VendorNo" 
				SelectedValue='<%# Bind("VendorNo") %>'
				/>
			<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
				ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
				SelectCommand="SELECT [ID], [VendorNo], [VendorName] FROM [tlk_vendors] ORDER BY VendorName" /> <a href="AddVendor.aspx">[+] Vendor</a></td>
		</tr>
		<tr>
			<td><label>Contract Type</label></td>
			<td><asp:DropDownList ID="ddlServices" runat="server" 
					DataSourceID="SqlDataSource1" 
					DataTextField="ServiceName" 
					DataValueField="ID" 
					SelectedValue='<%# Bind("Service") %>'
					/>
				<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
					ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
					SelectCommand="SELECT [ID], [ServiceName] FROM [tlk_service] ORDER BY ServiceName" />
			</td>
		</tr>
		<tr>
			<td><label>Contract Approval Date</label></td>
			<td><asp:TextBox ID="txtDateApproval" runat="server" Width="100" Text='<%# Bind("DateCountersigned", "{0:MM/dd/yyyy}") %>' /></td>
		</tr>
		<tr>
			<td><label>Contract End Date</label></td>
			<td><asp:TextBox ID="txtDateDuration" runat="server" Width="100" Text='<%# Bind("DateDuration", "{0:MM/dd/yyyy}") %>'/></td>
		</tr>
		<tr>
			<td><label>Date Entered</label></td>
			<td><asp:TextBox ID="txtDateEntered" runat="server" Width="100" Text='<%# Bind("DateEntered", "{0:MM/dd/yyyy}") %>'/></td>
		</tr>
		
		<tr>
			<td><label>Contract Amount</label></td>
			<td><asp:TextBox ID="txtAmount" runat="server" Width="100" Text='<%# Bind("Amount") %>' />
			
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
		<tr>
			<td><label>Original Amount</label></td>
			<td><asp:TextBox ID="txtOriginalAmount" runat="server" Width="100" Text='<%# Bind("OriginalAmount") %>' />
			
				<asp:CompareValidator id="CompareValidator2" runat="server" 
					ControlToValidate="txtOriginalAmount" 
					Operator="DataTypeCheck" 
					Type="Currency"
					ErrorMessage="[error] Illegal format for currency. "
					Display="Dynamic" />
					
				<asp:RangeValidator id="RangeValidator2" runat="server" 
					ControlToValidate="txtOriginalAmount"
					Type="Currency" 
					MinimumValue="0.00" MaximumValue="999,999,999.00"
					ErrorMessage="[error] Value greater than $999,999,999.00. " 
					Display="Dynamic" />
			</td>
		</tr>
				<tr>
			<td><label>Description</label></td>
			<td><asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Columns="75" Rows="5" Text='<%# Bind("Description") %>' /> <br />(example: LOT MAINTENANCE ON VACANT LOTS 532, 533, 534 IN BEECHVIEW)</td>
		</tr>
</table>
<asp:ImageButton ID="Button1" runat="server" OnClick="Button1_Click" ImageUrl="~/img/savebtn.gif" />


<asp:Label ID="lblMessage" runat="server" />
	</ItemTemplate>
</asp:FormView>
</div>
</asp:Content>

