<%@ Page Language="C#" MasterPageFile="~/Control/MasterForm.master" AutoEventWireup="true"
    CodeFile="ScAuditLog_Form.aspx.cs" Inherits="ScAuditLog_Form" %>

<%@ MasterType VirtualPath="~/Control/MasterForm.master" %>
<%@ Import Namespace="PCCore" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table border="0" cellspacing="0" cellpadding="3" width="100%">
        <tr>
            <td>
                <PC:TextBox ID="txt1" runat="server" RegisterClientVariable="true" CssClass="TextBoxCode" TextMode="multiLine" Height="200" Width="100%" ClientReadOnly="true"></PC:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
