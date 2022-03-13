<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DownLoadFile.aspx.cs" Inherits="DownLoadFile" %>

<%@ Import Namespace="PCCore" %>
<html>
<head runat="server">
    <title>DownLoad Files</title>
    <base target="_self">
    </base>
</head>
<body class="subwinbg" style="margin: 10px">
    <form runat="server">
        <% 
            string imgUrl = Config.GetImageBaseUrl(Page.Theme);
        %>
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td height="22">
                    <table width="100%" border="1" cellpadding="0" cellspacing="0" bordercolor="#999999"
                        style="border-collapse: collapse">
                        <tr>
                            <td height="22" class="infor">
                                <img src="<%=imgUrl%>/Information.gif" width="14" height="14" align="absmiddle">
                                <asp:Literal ID="litupload" Text="<%$ Resources:Messages,Selectfile %>" runat="server"></asp:Literal></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
