<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="AddCandidate.aspx.cs" Inherits="Admin_AddCandidate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="mainwrap">
<div class="contractdetails">
<h2>Add a Candidate</h2>

	<table class="companydetails" cellpadding="0" cellspacing="0">
		<tr>
			<td><label>Vendor Name </label></td>
			<td><asp:TextBox ID="txtCandidateName" runat="server" Width="200" />
			
				<asp:RequiredFieldValidator id="CandidateNameValidator" runat="server" 
					ControlToValidate="txtCandidateName" 
					ErrorMessage="[error] Required Field. " 
					Display="Dynamic" />
				
			</td>
		</tr>
	</table>
	<br />
	<asp:ImageButton ID="Button1" runat="server" OnClick="Button1_Click" ImageUrl="~/img/addbtn.gif" />

	<asp:Label ID="lblMessage" runat="server" />
</div>
</div>
</asp:Content>

