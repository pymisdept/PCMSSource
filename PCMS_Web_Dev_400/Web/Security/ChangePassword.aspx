<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" %>

<%@ Import Namespace="PCCore" %>
<html>
<head id="Head1" runat="server">
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate">    
    <base target="_self">
    </base>
    <%--    <link href="~/Control/gridview-leave.css" rel="stylesheet" type="text/css" />--%>
</head>
<% 
    string appUrl = Config.AppBaseUrl;
    string themeUrl = Config.GetThemeBaseUrl(Page.Theme); 
%>
<body>
    <form id="form1" runat="server">
        <table width="90%" border="1" align="center" cellpadding="0" cellspacing="0" bordercolor="#9cb4cd"
            style="border-collapse: collapse; margin-top: 20px; margin-bottom: 20px; margin-left: 20px;
            margin-right: 20px;">
            <tr>
                <td>
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
                        <tr>
                            <td height="21" background="<%=themeUrl%>/images/logintitlebg.gif" class="greenheadertxt"
                                style="padding-left: 20px" runat="server">
                                <asp:Localize ID="lblTitle" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td height="1" bgcolor="#CCCCCC">
                            </td>
                        </tr>
                    </table>
                    <table width="100%" border="0" cellpadding="3" cellspacing="0" bgcolor="#FFFFFF">
                        <tr>
                            <td colspan="2" align="center" valign="middle" height="30px;">
                                <div style="width: 100%; height: 20px;">
                                    <PC:Note ID="Note" runat="server" Visible="False" />
                                    <simple:SimpleValidationSummary ID="SimpleValidationSummary" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap style="padding-left: 20px;">
                                <asp:Localize ID="lblOldPwd" runat="server" Text="<%$ Resources:Labels,OldPassword %>" />
                            </td>
                            <td style="padding-left: 20px;">
                                <PC:TextBox ID="txtOldPwd" runat="server" Required="True" CssClass="TextBoxInput"
                                    SubmitOnEnter="true" RequiredErrorMessage="<%$ Resources:Messages,InputOldPassword %>" TextMode="Password"
                                    Width="200px" MaxLength="20" />
                            </td>
                        </tr>
                        <tr>
                            <td nowrap style="padding-left: 20px;">
                                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Labels,NewPassword %>" />
                            </td>
                            <td style="padding-left: 20px;">
                                <PC:TextBox ID="txtNewPwd1" runat="server" Required="True" CssClass="TextBoxInput"
                                    SubmitOnEnter="true" RequiredErrorMessage="<%$ Resources:Messages,InputNewPassword %>" TextMode="Password"
                                    Width="200px" MaxLength="20" />
                            </td>
                        </tr>
                        <tr>
                            <td nowrap style="padding-left: 20px;">
                                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Labels,RepeatNewPassword %>" />
                            </td>
                            <td style="padding-left: 20px;">
                                <PC:TextBox ID="txtNewPwd2" runat="server" Required="True" CssClass="TextBoxInput"
                                    SubmitOnEnter="true" RequiredErrorMessage="<%$ Resources:Messages,InputReatNewPassword %>" TextMode="Password"
                                    Width="200px" MaxLength="20" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
                        <tr>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td height="30" align="center" valign="bottom" style="padding-bottom: 30px;">
                                <asp:Button ID="btnOK" runat="server" Text="<%$ Resources:Common,Save %>" OnClick="btnOK_Click" CssClass="btn" />&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:Common,Close %>" OnClientClick="JavaScript:window.close();"
                                    CausesValidation="false" CssClass="btn" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
