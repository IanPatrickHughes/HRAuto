<%@ Page Title="" Language="C#" MasterPageFile="~/resources/global/master/HrAuto.Master" AutoEventWireup="true" CodeBehind="sorry.aspx.cs" 
Inherits="_HRAutomation.sorry" Theme="HrUpload"  %>
<%@ MasterType VirtualPath="~/resources/global/master/HrAuto.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="post">
				<h2 class="title"><a href="#">Access Denied</a></h2>
				<p class="meta"><em>You are currently not permitted to view this site.</em></p>
				<div class="entry">
					<p>Hey there, <%= CurrentUserName %>. At this time you do not have sufficient permissions to use this site.</p>
					<p>If you feel this is incorrect, please contact us at <a href="mailto:help@yoursite.com?subject=HR Auto Permissions">help@yoursite.com</a>.</p>
			        <p style="margin-top:40px;">We appologize for any inconvience.</p>
			    </div>
	 </div>
</asp:Content>
