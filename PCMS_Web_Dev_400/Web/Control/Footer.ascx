<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Footer.ascx.cs" Inherits="Footer" %>
<%@ Import Namespace="PCCore" %>
<!-- footer begin -->
<% 
    string appUrl = Config.AppBaseUrl;
    string themeUrl = Config.GetThemeBaseUrl(Page.Theme); 
%>
<table width="100%" height="20px" border="0" cellpadding="0" cellspacing="0">  
  <tr>
    <td background="<%=themeUrl%>/images/bottom_Line.gif">&nbsp;</td>
  </tr>
   <tr>
    <td  bgcolor="#DEDEDE" ><img src="<%=themeUrl%>/images/spacer.gif" width="1" height="2"></td>
  </tr>
   <tr>
    <td  bgcolor="#E8E8E8" ><img src="<%=themeUrl%>/images/spacer.gif" width="1" height="8"></td>
  </tr>
     <tr>
    <td class="footer" ><img src="<%=themeUrl%>/images/spacer.gif" width="1" height="8"><PC:Label ID="Label1" runat="server"  Text="" /> <PC:Label ID="Label2" runat="server"  Text="" LabelStyle="xLabel" /></td>
  </tr>
</table>
<!-- footer end -->
