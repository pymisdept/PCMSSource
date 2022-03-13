<%@ Page Language="C#" MasterPageFile="~/Control/MasterBase.master" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="Security_ForgotPassword"  %>
<%@ Import Namespace="PCCore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BaseContentPlaceHolder" Runat="Server">
<% 
    string appUrl = Config.AppBaseUrl;
    string themeUrl = Config.GetThemeBaseUrl(Page.Theme); 
%>

<table width="450px" border="1" align="center" cellpadding="0" cellspacing="0" bordercolor="#9cb4cd" style="border-collapse:collapse; margin-top:50px; margin-bottom: 250px;">
  <tr>
    <td><table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
      <tr>
        <td height="22" background="<%=themeUrl%>/images/logintitle_bg.gif" class="UserName" style="padding-left:10px;">
        <img src="<%=themeUrl%>/images/logintitle_icon.gif" width="16" height="16" align="absmiddle">
        &nbsp;<asp:Localize ID="lblLogin" runat="server" Text="<%$ Resources:Labels,RecoverPassword %>" />
        </td>
      </tr>
      <tr>
        <td height="1" background="<%=themeUrl%>/images/linedot.gif" ></td>
      </tr>
    </table>
      <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
      <tr>
      <td colspan="2" align="center" valign="middle" height="30px;">
        <div style="width:90%;height:20px;">
          <simple:SimpleNote ID="Note" runat="server" Visible="False"/>
          <simple:SimpleValidationSummary ID="SimpleValidationSummary" runat="server" />
        </div>
      </td>
      </tr>
      <tr>
        <td colspan="2" style="padding-left:20px;padding-right:20px;">
        <PC:Label ID="Label1" runat="server"  Text="<%$ Resources:Labels,GetForgetPassword %>" LabelStyle="xLabel"/> 
        </td>
      </tr>
      <tr>
        <td colspan="2">&nbsp;</td>
      </tr>
      <tr>
        <td  nowrap style="padding-left:20px;padding-right:20px;">
        <asp:Localize ID="lblLoginName" runat="server" Text="<%$ Resources:Labels,EmailAddress %>" />
        </td>
        <td style="padding-left:20px;padding-right:20px;">
        <PC:TextBox ID="txtEmail" runat="server" Required="True" Width="260px"  CssClass="TextBoxDescription"
            SubmitOnEnter="true" RequiredErrorMessage="<%$ Resources:Messages,InputCorrectEmail %>" 
            DataType="email" ValidationErrorMessage="<%$ Resources:Messages,InputCorrectEmail %>"
         />
        </td>
      </tr>
    </table>
      <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
      <tr>
        <td>&nbsp;<input type="text" style="display:none" /></td>        
      </tr>
        <tr>
          <td height="30" align="center" valign="bottom" style="padding-bottom:30px;">
          <asp:Button id="btnOK" runat="server" Text="OK" OnClick="btnOK_Click" />&nbsp;&nbsp;&nbsp;
          <%if(Request.QueryString["from"]==null) {%>
          <asp:Button id="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"  CausesValidation="false" />
          <%} %>
          </td>
        </tr>
      <tr>
        <td>&nbsp;</td>
      </tr>
      </table></td>
  </tr>
</table>
</asp:Content>

