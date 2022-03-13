<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RequestDownload.aspx.cs" Inherits="RequestDownload" %>

<%@ Import Namespace="PCCore" %>
<html>
<head runat="server">
    <title>Download Template</title>
    <base target="_self">
    </base>
</head>
<%
    
    //bool sessionOn = ((HttpContext.Current.Session != null) && (HttpContext.Current != null));
    //string isLogin = sessionOn.ToString().ToLower();    
    string isLogin = SessionInfo.IsLogin.ToString().ToLower();    
%>

<body class="subwinbg" style="margin: 10px" visible="false">
    <script language="javascript" type="text/javascript">
        var _isLogin = "<%=isLogin %>";
        //alert(_isLogin);
        if (_isLogin == "true")
        {
            window.returnValue = "";
        } else {
            alert("Session timeout. Plaase re-login.");
            window.returnValue = "error";
        }
        //alert(window.returnValue);
    </script>
    <form runat="server">
        <% 
            string imgUrl = Config.GetImageBaseUrl(Page.Theme);
        %>
        <input type="hidden" id="txtstatus" value="" />
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td height="22">
                    <table width="100%" border="1" cellpadding="0" cellspacing="0" bordercolor="#999999"
                        style="border-collapse: collapse">
                        
                        <tr>
                            <td height="22" class="infor">
                                <img src="<%=imgUrl%>/Information.gif" width="14" height="14" align="absmiddle">
                                <asp:Literal ID="litupload" Text="<%$ Resources:Messages,DlnProcessing %>" runat="server"></asp:Literal></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>

</html>

