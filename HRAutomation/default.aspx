<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="_HRAutomation._default" 
MasterPageFile="~/resources/global/master/HrAuto.Master" Theme="HrUpload"  %>
<%@ MasterType VirtualPath="~/resources/global/master/HrAuto.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="post">
				<h2 class="title"><a href="#">Perform File Encryption and Transmission</a></h2>
				<p class="meta"><em>Use the form below to transmit our 834 Interface vendor files.</em></p>
				<div class="entry">
					<p>Remember clicking below and firing this event you are initiating an action, which will upload a file to the established vendor. 
					If this action is performed multiple times on a given day or week;  this <span style="color:#ff0000;">MAY RESULT IN MULTIPLE FILES BEING SUBMITTED TO THE VENDOR</span>.</p>
					<HRAutomation:AutoUserForm runat="server" id="MainControl" />
			    </div>
	 </div>
</asp:Content>
