<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="AddContract.aspx.cs" Inherits="AddContract" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="about">

<div class="large-12 columns">
<h2>Add a Contract</h2>

<asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="true" CssClass="error-box" />

<table cellpadding="0" cellspacing="0">
	<tr>
		<td><label>Contract No.</label></td>
		<td><asp:TextBox ID="txtContractNo" runat="server" Width="100" /> 
			
			<asp:RequiredFieldValidator ID="reqContractNo" runat="server" 
				ControlToValidate="txtContractNo" ErrorMessage="Please enter a Contract No." Display="None"   />
			
			<asp:RangeValidator ID="valContractNo" runat="server" 
				ControlToValidate="txtContractNo" Type="Integer"
				MinimumValue="0" MaximumValue="99999999"
				ErrorMessage="Contract No. does not appear valid." Display="None" />
		</td>
	</tr>
	<tr>
		<td><label>Supplement No.</label></td>
		<td><asp:TextBox ID="txtSupplementalNo" runat="server" Width="25" /> (example: 1)
		
			<asp:RangeValidator ID="valSupplementalNo" runat="server" 
				ControlToValidate="txtSupplementalNo" Type="Integer"
				MinimumValue="0" MaximumValue="999"
				ErrorMessage="Supplemental No. does not appear valid." Display="None" />
		
		</td>
	</tr>
	<tr>
		<td><label>Resolution No.</label></td>
		<td><asp:TextBox ID="txtResolutionNo" runat="server" Width="200" /> (example: RS-320-99, 210-00)</td>
	</tr>
	<tr>
		<td><label>City Department</label></td>
		<td><asp:DropDownList ID="ddlDepartment" runat="server" 
				Width="450"
				DataSourceID="SqlDataSource4" 
				DataTextField="DeptName" 
				DataValueField="DeptCode" 
				>
			</asp:DropDownList>
			<asp:SqlDataSource ID="SqlDataSource4" runat="server" 
				ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
				SelectCommand="SELECT [DeptCode], [DeptName] FROM [tlk_department] ORDER BY DeptName" />
		</td>
	</tr>
		<tr>
			<td><label>Vendor</label></td>
			<td><asp:DropDownList ID="ddlVendors" runat="server" 
				Width="450"
				DataSourceID="SqlDataSource2" 
				DataTextField="VendorName" 
				DataValueField="VendorNo" 
				AutoPostBack="true"
				OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged"
				/>
			<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
				ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
				SelectCommand="SELECT [ID], [VendorNo], [VendorName] FROM [tlk_vendors] ORDER BY VendorName" /> <a href="AddVendor.aspx">[+] Vendor</a>
				
				<br />
			<asp:Panel ID="pnlVendor" runat="server">
				<asp:Repeater ID="rptVendor" runat="server">
					<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "VendorName")%><br />
						<%# DataBinder.Eval(Container.DataItem, "Address1")%><br />
						<%# DataBinder.Eval(Container.DataItem, "Address2")%><br />
						<%# DataBinder.Eval(Container.DataItem, "City")%>, 
						<%# DataBinder.Eval(Container.DataItem, "State")%> 
						<%# DataBinder.Eval(Container.DataItem, "Zip")%>
					</ItemTemplate>
				</asp:Repeater>
			</asp:Panel>
			
			</td>
		</tr>
		<tr>
			<td><label>Contract Type</label></td>
			<td><asp:DropDownList ID="ddlServices" runat="server" 
					DataSourceID="SqlDataSource1" 
					DataTextField="ServiceName" 
					DataValueField="ID" />
				<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
					ConnectionString="<%$ ConnectionStrings:CityControllerConnectionString %>" 
					SelectCommand="SELECT [ID], [ServiceName] FROM [tlk_service] ORDER BY ServiceName" />
			</td>
		</tr>
		<tr>
			<td><label>Date Entered</label></td>
			<td><asp:TextBox ID="txtDateEntered" runat="server" Width="100" /></td>
		</tr>
		<tr>
			<td><label>Contract End Date</label></td>
			<td><asp:TextBox ID="txtDateDuration" runat="server" Width="100" /></td>
		</tr>
		<tr>
			<td><label>Contract Approval Date</label></td>
			<td><asp:TextBox ID="txtDateApproval" runat="server" Width="100" /></td>
		</tr>
		
		<tr>
			<td><label>Contract Amount</label></td>
			<td><asp:TextBox ID="txtAmount" runat="server" Width="100" />
			
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
			<td><asp:TextBox ID="txtOriginalAmount" runat="server" Width="100" />
			
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
			<td><asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Columns="75" Rows="5" /> <br />(example: LOT MAINTENANCE ON VACANT LOTS 532, 533, 534 IN BEECHVIEW)</td>
		</tr>
</table>
<asp:ImageButton ID="Button1" runat="server" OnClick="Button1_Click" ImageUrl="~/img/addbtn.gif" />
<asp:ImageButton ID="ImageButton1" runat="server" OnClick="AddAnother_Click" CommandName="AddAnother" ImageUrl="~/img/addanotherbtn.gif" />

<asp:Label ID="lblMessage" runat="server" />
</div>
    </div>

</asp:Content>
