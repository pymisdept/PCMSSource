<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Banner.ascx.cs" Inherits="Banner" %>
<%@ Import Namespace="PCCore" %>
<%@ Register Src="~/Control/WebMenu.ascx" TagName="WebMenu" TagPrefix="PC" %>
<!-- banner begin -->
<% 
    string appUrl = Config.AppBaseUrl;
    string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
    PCCore.Common.HRLog.RecordLog(themeUrl);
%>
<table width="100%" height="86px" border="0" cellpadding="1" cellspacing="1">  
    <tr>
        <td valign="top" colspan="2" >
            <table width="100%" height="22" cellpadding="2" cellspacing="2">  
            <tr>    
    
                <td  width="431"><img src="<%=themeUrl%>/images/pyelogo.jpg" width="70"></td>
                <td  width="431"><img src="<%=themeUrl%>/images/title_2_02.gif" ></td>
    
                <%--<td  width="431"><img src="<%=themeUrl%>/images/title_2_01.gif"></td>-->
                <%--<td align="right" valign="bottom" background="<%=themeUrl%>/images/top_bg.jpg" style="background-position:left top;background-repeat:no-repeat;"></td>--%>
                
                <%-- <td class="BannerRight" background="<%=themeUrl%>/images/top_bg.jpg" >--%>
                <td class="BannerRight">
                    <table width="100%">
                    <tr>
                        <td width="100%">
                        <PC:Label ID="lblProject" Visible="false" runat="server" LabelStyle="xLabel" Font-Bold="true" ForeColor="Blue" Text="<%$ Resources:Labels,Project %>"/>
                        <PC:Label ID="lblProjectDesc" Visible="false" runat="server" LabelStyle="xLabel" Width="80%" Font-Bold="true" ForeColor="Blue" Font-Italic="true"/>
                        </td>
                    </tr>
                    <tr>

                        
                        <%--<PC:Label ID="lblLoginID" runat="server" LabelStyle="xLabel" Font-Bold="true" ForeColor="Blue" Text="<%$ Resources:Labels,LoginID %>"/>--%>
                        <PC:Label ID="lblLoginID" runat="server" LabelStyle="xLabel" Font-Bold="true" ForeColor="Blue" Text="<%$ Resources:Labels,UserName %>"/>
                       <%if (SessionInfo.IsLogin) { %>          
                            <PC:Label> : </PC:Label>
                       <%}%>
                            
                       <PC:Label ID="LoginID" runat="server" LabelStyle="xLabel" Width="70px" Font-Bold="true" ForeColor="Blue" Font-Italic="true"/>
                        
                        <%--<img src="<%=themeUrl%>/images/home.jpg" >--%>
                        <a href="<%=appUrl%>/Admin_Default.aspx" class="BannerLogin">
                        <PC:Label ID="Label1"  runat="server" Text="<%$ Resources:Labels,Home %>" LabelStyle="xLabel"/></a>
                        
                        <!--<a href="#" class="BannerLogin"><PC:Label ID="Label2"  runat="server" Text="<%$ Resources:Labels,SiteMap %>" LabelStyle="xLabel"/></a>-->
                        <%if (SessionInfo.IsLogin) { %>                          
                            <a href="javascript:Logout();" class="BannerLogin">
                            <PC:Label ID="Literal5"  runat="server" Text="<%$ Resources:Labels,Logout %>" LabelStyle="xLabel"/></a>
                        <%}else{ %>
                            <%--<td><a href="javascript:BannerLogin();" class="toptxtlink"><asp:Literal ID="Literal6"  runat="server" Text="<%$ Resources:Labels,LOGIN %>" /></a></td>--%>
                            <a href="<%=appUrl%>/Default.aspx" class="BannerLogin">
                            <PC:Label ID="Literal9"  runat="server" Text="<%$ Resources:Labels,LOGIN %>" LabelStyle="xLabel"/></a>
                        <%} %>
                        
                    </tr>
                    <tr>
                    <%--<td><PC:Label ID="lbldate"  runat="server" Font-Bold="true" ForeColor="Blue" Font-Italic="true" LabelStyle="xLabel"></PC:Label></td>--%>
                    <td><PC:Label ID="lbldate"  runat="server" Font-Bold="true" ForeColor="Blue" Font-Italic="true" LabelStyle="xLabel"></PC:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <%if (SessionInfo.IsLogin) { %>
                        <a href="<%=appUrl%>/Security/ChangePassword.aspx" class="BannerLogin" target="_blank" onclick="window.open(this.href, 'ChangePassword',
'left=20,top=20,width=800,height=400,toolbar=0,resizable=0'); return false;">
                        <PC:Label ID="Label3"  runat="server" Text="<%$ Resources:Labels,ChangePassword %>" LabelStyle="xLabel"/></a>
                    <%}%>
                    </td>
                    </tr>
                    </table>
                </td>
    
    
                <td  width="431" valign="bottom"><img src="<%=themeUrl%>/images/small Compass logo.jpg" ></td>
            </tr>
            </table>
        </td>
 
        <%--<td align="right" width="90%" valign="bottom" background="<%=themeUrl%>/images/top_bg.jpg"  style="background-position:right top;background-repeat:no-repeat;"></td>    --%>
        <%--<td align="right" valign="bottom"><img src="<%=themeUrl%>/images/top_bg.jpg" ></td>--%>
        
    </tr>
    <tr>    
        <td valign="top" height="22" colspan="2" background="<%=themeUrl%>/images/lan_bg.jpg">
            <table width="100%" height="22" cellpadding="0" cellspacing="0">  
            <tr>    
                <td valign="top" height="22"><PC:WebMenu runat="server" ID="Menu"/> </td>
                <td valign="top" ><table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                          <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="right" valign="bottom" style ="padding-right:20px;" >
                                    <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td><a href="javascript:JumpLang('en-us');" class="languagetxt">English</a></td>
                                        <td><img src="<%=themeUrl%>/images/linewhite.gif" width="23" height="23"></td>
                                        <td><a href="javascript:JumpLang('zh-tw');" class="languagetxt">&#32321;&#39636;</a></td>
                                        <td><img src="<%=themeUrl%>/images/linewhite.gif" width="23" height="23"></td>
                                        <td><a href="javascript:JumpLang('zh-cn');" class="languagetxt">&#31616;&#20307;</a></td>
                                        <%--<td><img src="<%=themeUrl%>/images/linewhite.gif" width="23" height="23"></td>--%>
                                        <%--<td><a href="javascript:JumpLang('ja-JP');" class="languagetxt">&#26085;&#26412;&#35486;</a></td>--%>
                                    </tr>
                                    </table>
                                </td>
                            </tr>
                            </table>
                        </td>
                    </tr>
                 </td>
                 
            </tr>
            </table>
        </td>
        </tr>
        </table> 
    </td>
    
  </tr>
</table>