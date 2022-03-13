<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ErrorPage.aspx.cs" Inherits="ErrorPage" %>

<%@ Import Namespace="PCCore" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<% 
    string appUrl = Config.AppBaseUrl;
    string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PCMS Error Message Display Page</title>
    <meta http-equiv="Content-Type" content="text/html;">
    <link href="<%=themeUrl%>/main.css" rel="stylesheet" type="text/css">
</head>
<body leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
    <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>    
        
    <td  width="431"><img src="<%=themeUrl%>/images/pyelogo.jpg" width="70" ><img src="<%=themeUrl%>/images/title_2_02.gif" ></td>
    
    <%--<td align="right" valign="bottom" background="<%=themeUrl%>/images/top_bg.jpg" style="background-position:left top;background-repeat:no-repeat;"></td>--%>
    <td class="BannerRight" background="<%=themeUrl%>/images/top_bg.jpg" >
    <PC:Label ID="lblLoginID" runat="server" LabelStyle="xLabel" Font-Bold="true" ForeColor="Blue" Text="<%$ Resources:Labels,LoginID %>"/>
    <PC:Label ID="LoginID" runat="server" LabelStyle="xLabel" Width="70px" Font-Bold="true" ForeColor="Blue" Font-Italic="true"/>
    HOME &nbsp; &nbsp;SITEMAP &nbsp; &nbsp;LOGOUT &nbsp; &nbsp; &nbsp;
    </td>
    <%--<td align="right" width="90%" valign="bottom" background="<%=themeUrl%>/images/top_bg.jpg"  style="background-position:right top;background-repeat:no-repeat;"></td>    --%>
    <%--<td align="right" valign="bottom"><img src="<%=themeUrl%>/images/top_bg.jpg" ></td>--%>
    
  </tr>
  <tr>    
    <td valign="top" height="22" colspan="2" background="<%=themeUrl%>/images/lan_bg.jpg">
        <table width="100%" height="22" cellpadding="0" cellspacing="0">  
        <tr>    
        <td valign="top" height="22">
            <%--<PC:WebMenu runat="server" ID="Menu"/>--%> </td>
        
        <td valign="top" ><table width="100%" border="0" cellpadding="0" cellspacing="0"  >
          <tr>
            <td>
              <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="right" valign="bottom" style ="padding-right:20px;" ><table border="0" cellpadding="0" cellspacing="0">
                      <tr>
                        <td><a href="javascript:JumpLang('en-us');" class="languagetxt">English</a></td>
                        <td><img src="<%=themeUrl%>/images/linewhite.gif" width="23" height="23"></td>
                        <td><a href="javascript:JumpLang('zh-tw');" class="languagetxt">&#32321;&#39636;</a></td>
                        <td><img src="<%=themeUrl%>/images/linewhite.gif" width="23" height="23"></td>
                        <td><a href="javascript:JumpLang('zh-cn');" class="languagetxt">&#31616;&#20307;</a></td>
                        <td><img src="<%=themeUrl%>/images/linewhite.gif" width="23" height="23"></td>
                        <td><a href="javascript:JumpLang('ja-JP');" class="languagetxt">&#26085;&#26412;&#35486;</a></td>
                      </tr>
                  </table></td>
                  </tr>
              </table>
             </td>
          </tr>
        </table></td>
        </tr>
        </table> 
    </td>
  </tr>
        <tr>
            <td valign="top" height="85" colspan="2">
            </td>
        </tr>
        <tr>
            <td align="center" valign="top">
                <table width="518" height="194" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <%--<td align="right" background="<%=themeUrl%>/images/Error.jpg" style="padding-right: 75px">
                            <input type="image" value="sss" name="imageField" src="<%=themeUrl%>/images/error_btn.jpg"
                                onclick="javascript:getHomeUrl();"></td>--%>
                        <td align="right" valign="top" background="<%=themeUrl%>/images/Error.jpg" style="padding: 65px 20px 0 0">
                            <table border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td height="32" valign="top">
                                        <input name="button" type="button" onclick="javascript:getHomeUrl();" class="errorbtn1" id="button" value="Login"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <input name="button2" type="button" onclick="javascript:SendErrorReport();" class="errorbtn2" id="button2" value="Send Error Report"></td>
                                </tr>
                                <tr>
                                    <td height="50" align="center" valign="bottom">
                                        <a href="#" onclick="javascript:ShowError();" class="errortxt1">Click to Show Details</a></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
<%--        <tr>
            <td valign="top" align="center">
                <a href="#" onclick="javascript:SendErrorReport();" style="color: Gray;">Send Error
                    Report</a>
            </td>
        </tr>
        <tr>
            <td valign="top" align="center">
                <a href="#" onclick="javascript:ShowError();" style="color: Gray;">Click to Show Details</a>
            </td>
        </tr>--%>
        <tr>
            <td valign="top" align="left">
                <div id="divError" align="left" style="padding: 20px 10px 20px 10px; display: block;
                    vertical-align: top;">
                    <PC:Label ID="lblErrorMessage" runat="server" RegisterClientVariable="true" />
                    
                </div>
            </td>
        </tr>
        <tr>
            <td valign="top" align="center" height="90%"><PC:Label ID="error" runat="server" CssClass="Invisible" />
            </td>
        </tr>
    </table>

    <script language="javascript">
function getHomeUrl(){
//    var url;
//    var host;
//    host=window.location.host;
//    var vrl;
//    vrl=window.location.href.substring(("http://"+host+"/").length);
//    vrl=vrl.substring(0,vrl.indexOf("/"));
//    url="http://"+host+"/"+vrl+"/Default.aspx";
//    window.location.href(url); 
    var url;
    url="<%=appUrl %>/Default.aspx";
    window.location.href(url);    
}

    function ShowError()
    {
        if(document.getElementById("divError").style.display=="none")
        {
            document.getElementById("divError").style.display="";
        }
        else
        {
            document.getElementById("divError").style.display="none";
        }
    }
    
    function SendErrorReport()
    {        
        var url="<%=appUrl%>/ErrorPage_Send.aspx";
        var dialogFeatures="dialogHeight:520px;dialogWidth:700px;";
        
        window.showModalDialog(url+"?error="+escape(error.innerText),divError,"status:no;help:no;" + dialogFeatures);        
    }
    </script>

</body>
</html>
