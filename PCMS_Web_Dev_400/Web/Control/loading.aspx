<%@ Page Language="C#" AutoEventWireup="true" CodeFile="loading.aspx.cs" Inherits="Control_loading" %>
<%@ Import Namespace="PCCore" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>

<%
        int showtime = Config.UploadWaitSecond;
        string appUrl = Config.AppBaseUrl;
        string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
        
     %>
     
<body style="background-color:Transparent; background:transparent;">
    
    <form id="form1" runat="server" style="background-color:Transparent">
    
    <div>
    
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <table width="100%">
    <tr style="height:100%" valign="middle">
    <td width="100%" align="center">
    <asp:Image ID="img1" runat="server" ImageUrl="<%=themeUrl%>/images/loading.gif" /><br />
    </td>
    </tr>
    <tr>
    <td width="100%" align="center">
    <asp:Label ID="lbl1" runat="server" Text="<%$ Resources:Labels,UploadProcessingMessage %>"></asp:Label>
    </td>
    </tr>
    </table>
    
    </div>
    </form>
    
</body>
<script language="javascript" type="text/javascript">
//var num = 0;
//var tim;
//var _showtime = "<%=showtime%>";
////alert(_showtime);
//window.status='';


//function showLoad()
//{
//    num ++;
//    //alert(num);
//    if (num == _showtime)
//    {
//    window.clearTimeout(tim);
//    window.close();
//    
//    } else {
//        
//        tim = window.setTimeout("showLoad()",1000);
//    }
//}
//showLoad();   
</script>
</html>
