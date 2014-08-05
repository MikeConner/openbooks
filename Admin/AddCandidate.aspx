<%@ Page Title="" Language="C#" MasterPageFile="~/_Masters/AdminMasterPage.master" AutoEventWireup="true" CodeFile="AddCandidate.aspx.cs" Inherits="Admin_AddCandidate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
<div class="about">
<div class="row">
<div class="large-12 columns">
<div class="contractdetails">
<h2>Add a Candidate</h2>

	<table class="companydetails" cellpadding="0" cellspacing="0">
		<tr>
			<td><label>Candidate Name </label></td>
			<td><asp:TextBox ID="txtCandidateName" runat="server"  />
			
				<asp:RequiredFieldValidator id="CandidateNameValidator" runat="server" 
					ControlToValidate="txtCandidateName" 
					ErrorMessage="[error] Required Field. " 
					Display="Dynamic" />
				
			</td>
		</tr>
	</table>
	<br />
	<asp:Button ID="Button1" runat="server" OnClick="Button1_Click" text="Add Candidate"  CssClass =" submit button"/>

	<asp:Label ID="lblMessage" runat="server" />
</div>
</div>
    </div></div>
</asp:Content>

