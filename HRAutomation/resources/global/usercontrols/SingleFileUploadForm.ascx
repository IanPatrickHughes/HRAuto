<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SingleFileUploadForm.ascx.cs" 
Inherits="_HRAutomation.resources.global.usercontrols.SingleFileUploadForm" %>



<div id="transmit-allcontainer"> 
    <asp:DropDownList runat="server" ID="ddlVendorList" Width="275" style="float:left;"></asp:DropDownList><br /><br /><br />
    <asp:CheckBox runat="server" ID="cboIsTest" Checked="true" Text="Is Test?" style="float:left;margin-right:15px;" /> 
    <asp:Button runat="server" ID="btnRunHRAutomation" Text="Transmit Selected File" class="links" style="float:left;"  />
</div>

<asp:Panel runat="server" ID="pnlResponse" CssClass="response-container" Visible="false">
    <asp:Literal runat="server" ID="ltlResultText" />
</asp:Panel>