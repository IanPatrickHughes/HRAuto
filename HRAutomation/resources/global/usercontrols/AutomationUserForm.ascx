<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AutomationUserForm.ascx.cs" Inherits="_HRAutomation.resources.global.usercontrols.AutomationUserForm" %>

<div id="last-action" >
    <h2 style="float:left;">Last Submission Date of Files:</h2><span id="last-actiondate" ><asp:Literal runat="server" ID="ltlLastRunDate" /></span>
</div>

<div id="transmit-allcontainer">
    <asp:CheckBox runat="server" ID="cboIsTest" Checked="true" Text="Is Test?" style="float:left;margin-right:15px;" /> <asp:Button runat="server" ID="btnRunHRAutomation" Text="Transmit All Files" class="links" OnClick="btnRunHRAutomation_Click" />
</div>

<asp:Panel runat="server" ID="pnlResponse" CssClass="response-container" Visible="false">
    <asp:Literal runat="server" ID="ltlResultText" />
</asp:Panel>