<%@ Page Title="" Language="C#" MasterPageFile="~/resources/global/master/HrAuto.Master" 
AutoEventWireup="true" CodeBehind="single.aspx.cs" Inherits="_HRAutomation.Single" Theme="HrUpload" %>
<%@ MasterType VirtualPath="~/resources/global/master/HrAuto.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="post">
				<h2 class="title"><a href="#">SINGLE File Encryption and Transmission</a></h2>
				<p class="meta"><em>Use the form below to transmit a single 834 Interface vendor file.</em></p>
				<div class="entry">
					<p>Use this form for submitting a <span style="color:#ff0000;">SINGLE FILE to a SINGLE VENDOR</span>.</p>	
					<HRAutomation:SingleFileForm runat="server" id="MainControl" />
			    </div>
	 </div>
</asp:Content>
