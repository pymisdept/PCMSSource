<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MAI01Menu.ascx.cs" Inherits="MAI01Menu" %>

<%@ Import Namespace="PCCore" %>
<script language="javascript" src="../JavaScript/TreeView.js"></script>
<script language="javascript" src="../JavaScript/AdapterUtils.js"></script>
<% 
string appUrl = Config.AppBaseUrl;
string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
string projcode = null;
string id = null;
string period=null;
    
try 
{
       projcode = Request.QueryString["projcode"].ToString();
       id = Request.QueryString["id"].ToString();
       period= Request.QueryString["period"].ToString();
} catch (Exception ex)
{
}
%>
<table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td height="42" valign="top">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="162px" valign="top">                        
                        <table width="100%" height="560px" border="0" cellspacing="0" cellpadding="0">
                            <tr>                                
                                
                                <td height="128" valign="top" background="<%=themeUrl%>/images/Security_left.gif">
                                </td>
                            </tr>
                            <tr>                            
                                <td  valign="top" background="<%=themeUrl%>/images/personnel_left_02.gif"
                                    style="padding: 10px;padding-left:5px;padding-top:3px;">
                                    <div class ="DivGridstyle_se">
                                    <table height="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td valign="top" height="100%">
                                                    <PC:ListBar ID="SLB" runat="server" ListBarStyle="UserDefine" Height="100%"  RenderHiddenGroup="true"  >
                                                        <ListGroups>
                                                            <simple:ListBarGroup ID="bsEmployee1" runat="server" Text="<%$ Resources:Labels,Employment %>">
                                                                <ListItems>
                                                                    
                                                                </ListItems>
                                                            </simple:ListBarGroup>                                                                                                                                                                            
                                                        </ListGroups>
                                                    </PC:ListBar>
                                                    
                                                    
                                                    <%-- ==================================QuantitySurvey Module===========================================--%>
                                                <ul class="ul_se">
                                                <li class="AspNet-TreeView-Root"><%if (SessionInfo.CurrentFunction.ToUpper() == "MAI0101")
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal1"  runat="server" Text="<%$ Resources:Labels,MAI0101 %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Import/MAI0101.aspx?projcode=<%=projcode%>&id=<%=id%>&period=<%=period%>"  onclick="javascript:SaveData();" class="left_petxt"><asp:Literal ID="Literal2"  runat="server" Text="<%$ Resources:Labels,MAI0101 %>" /></a> <%}%>
                                                </li>
                                                
                                                <li><%--<%if (SessionInfo.CurrentFunction.ToUpper() == "MAI0102")
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal3"  runat="server" Text="<%$ Resources:Labels,MAI0102 %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Import/MAI0102.aspx?projcode=<%=projcode%>" class="left_petxt"><asp:Literal ID="Literal4"  runat="server" Text="<%$ Resources:Labels,MAI0102 %>" /></a> <%}%>
                                                      --%>
                                                      <div class="left_seseltxt"><b><asp:Literal ID="Literal3"  runat="server" Text="<%$ Resources:Labels,MAI0102 %>" /></b></div>
                                                      <oi>
                                                        <%if (SessionInfo.CurrentFunction.ToUpper() == "MAI010201")
                                                        {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal15"  runat="server" Text="&nbsp;&nbsp;<%$ Resources:Labels,MAI010201 %>" /></div><%}
                                                        else
                                                        {%> <a href="<%=appUrl %>/Import/MAI010201.aspx?projcode=<%=projcode%>&id=<%=id%>&period=<%=period%>" onclick="javascript:SaveData();" class="left_petxt"><asp:Literal ID="Literal16"  runat="server" Text="&nbsp;&nbsp;<%$ Resources:Labels,MAI010201 %>" /></a> <%}%>
                                                      
                                                      
                                                      </oi> 
                                                      <br />
                                                      <oi>
                                                        <%if (SessionInfo.CurrentFunction.ToUpper() == "MAI010202")
                                                        {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal17"  runat="server" Text="<%$ Resources:Labels,MAI010202 %>" /></div><%}
                                                        else
                                                        {%> <a href="<%=appUrl %>/Import/MAI010202.aspx?projcode=<%=projcode%>&id=<%=id%>&period=<%=period%>" onclick="javascript:SaveData();" class="left_petxt"><asp:Literal ID="Literal18"  runat="server" Text="<%$ Resources:Labels,MAI010202 %>" /></a> <%}%>
                                                      
                                                      
                                                      </oi> 
                                                      <br />
                                                      <oi>
                                                        <%if (SessionInfo.CurrentFunction.ToUpper() == "MAI010203")
                                                        {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal19"  runat="server" Text="<%$ Resources:Labels,MAI010203 %>" /></div><%}
                                                        else
                                                        {%> <a href="<%=appUrl %>/Import/MAI010203.aspx?projcode=<%=projcode%>&id=<%=id%>&period=<%=period%>" onclick="javascript:SaveData();" class="left_petxt"><asp:Literal ID="Literal20"  runat="server" Text="<%$ Resources:Labels,MAI010203 %>" /></a> <%}%>
                                                      
                                                      
                                                      </oi> 
                                                      <br />
                                                      <oi>
                                                        <%if (SessionInfo.CurrentFunction.ToUpper() == "MAI010204")
                                                        {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal21"  runat="server" Text="<%$ Resources:Labels,MAI010204 %>" /></div><%}
                                                        else
                                                        {%> <a href="<%=appUrl %>/Import/MAI010204.aspx?projcode=<%=projcode%>&id=<%=id%>&period=<%=period%>" onclick="javascript:SaveData();" class="left_petxt"><asp:Literal ID="Literal22"  runat="server" Text="<%$ Resources:Labels,MAI010204 %>" /></a> <%}%>
                                                      
                                                      
                                                      </oi> 
                                                </li>                                                
                                                <li><%--if (SessionInfo.CurrentFunction.ToUpper() == "MAI0103")
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal5"  runat="server" Text="<%$ Resources:Labels,MAI0103 %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Import/MAI0103.aspx?projcode=<%=projcode%>" class="left_petxt"><asp:Literal ID="Literal6"  runat="server" Text="<%$ Resources:Labels,MAI0103 %>" /></a> <%}%>--%>
                                                      <div class="left_seseltxt"><b><asp:Literal ID="Literal4"  runat="server" Text="<%$ Resources:Labels,MAI0103 %>" /></b></div>
                                                      <oi>
                                                        <%if (SessionInfo.CurrentFunction.ToUpper() == "MAI010301")
                                                        {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal23"  runat="server" Text="<%$ Resources:Labels,MAI010301 %>" /></div><%}
                                                        else
                                                        {%> <a href="<%=appUrl %>/Import/MAI010301.aspx?projcode=<%=projcode%>&id=<%=id%>&period=<%=period%>" onclick="javascript:SaveData();" class="left_petxt"><asp:Literal ID="Literal24"  runat="server" Text="<%$ Resources:Labels,MAI010301 %>" /></a> <%}%>
                                                      
                                                      
                                                      </oi> 
                                                      <br />
                                                      <oi>
                                                        <%if (SessionInfo.CurrentFunction.ToUpper() == "MAI010302")
                                                        {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal25"  runat="server" Text="<%$ Resources:Labels,MAI010302 %>" /></div><%}
                                                        else
                                                        {%> <a href="<%=appUrl %>/Import/MAI010302.aspx?projcode=<%=projcode%>&id=<%=id%>&period=<%=period%>" onclick="javascript:SaveData();" class="left_petxt"><asp:Literal ID="Literal26"  runat="server" Text="<%$ Resources:Labels,MAI010302 %>" /></a> <%}%>
                                                      
                                                      
                                                      </oi> 
                                                      <br />
                                                      <oi>
                                                        <%if (SessionInfo.CurrentFunction.ToUpper() == "MAI010303")
                                                        {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal27"  runat="server" Text="<%$ Resources:Labels,MAI010303 %>" /></div><%}
                                                        else
                                                        {%> <a href="<%=appUrl %>/Import/MAI010303.aspx?projcode=<%=projcode%>&id=<%=id%>&period=<%=period%>" onclick="javascript:SaveData();" class="left_petxt"><asp:Literal ID="Literal28"  runat="server" Text="<%$ Resources:Labels,MAI010303 %>" /></a> <%}%>
                                                      
                                                      
                                                      </oi> 
                                                      <br />
                                                      <oi>
                                                        <%if (SessionInfo.CurrentFunction.ToUpper() == "MAI01030301")
                                                        {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal33"  runat="server" Text="<%$ Resources:Labels,MAI01030301 %>" /></div><%}
                                                        else
                                                        {%> <a href="<%=appUrl %>/Import/MAI01030301.aspx?projcode=<%=projcode%>&id=<%=id%>&period=<%=period%>" onclick="javascript:SaveData();" class="left_petxt"><asp:Literal ID="Literal34"  runat="server" Text="<%$ Resources:Labels,MAI01030301 %>" /></a> <%}%>
                                                      
                                                      
                                                      </oi> 
                                                      <br />
                                                      <oi>
                                                        <%if (SessionInfo.CurrentFunction.ToUpper() == "MAI010304")
                                                        {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal29"  runat="server" Text="<%$ Resources:Labels,MAI010304 %>" /></div><%}
                                                        else
                                                        {%> <a href="<%=appUrl %>/Import/MAI010304.aspx?projcode=<%=projcode%>&id=<%=id%>&period=<%=period%>" onclick="javascript:SaveData();" class="left_petxt"><asp:Literal ID="Literal30"  runat="server" Text="<%$ Resources:Labels,MAI010304 %>" /></a> <%}%>
                                                      
                                                      
                                                      </oi> 
                                                      <br />
                                                      <oi>
                                                        <%if (SessionInfo.CurrentFunction.ToUpper() == "MAI010305")
                                                        {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal31"  runat="server" Text="<%$ Resources:Labels,MAI010305 %>" /></div><%}
                                                        else
                                                        {%> <a href="<%=appUrl %>/Import/MAI010305.aspx?projcode=<%=projcode%>&id=<%=id%>&period=<%=period%>" onclick="javascript:SaveData();" class="left_petxt"><asp:Literal ID="Literal32"  runat="server" Text="<%$ Resources:Labels,MAI010305 %>" /></a> <%}%>
                                                      
                                                      
                                                      </oi>
                                                      <br />
                                                </li> 
                                                <li><%if (SessionInfo.CurrentFunction.ToUpper() == "MAI0104")
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal7"  runat="server" Text="<%$ Resources:Labels,MAI0104 %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Import/MAI0104.aspx?projcode=<%=projcode%>&id=<%=id%>&period=<%=period%>" onclick="javascript:SaveData();" class="left_petxt"><asp:Literal ID="Literal8"  runat="server" Text="<%$ Resources:Labels,MAI0104 %>" /></a> <%}%>
                                                </li>                                                                                               
                                                <li><%if (SessionInfo.CurrentFunction.ToUpper() == "MAI0105")
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal9"  runat="server" Text="<%$ Resources:Labels,MAI0105 %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Import/MAI0105.aspx?projcode=<%=projcode%>&id=<%=id%>&period=<%=period%>" onclick="javascript:SaveData();" class="left_petxt"><asp:Literal ID="Literal10"  runat="server" Text="<%$ Resources:Labels,MAI0105 %>" /></a> <%}%>
                                                </li>                                          
                                                <li><%if (SessionInfo.CurrentFunction.ToUpper() == "MAI0106")
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal11"  runat="server" Text="<%$ Resources:Labels,MAI0106 %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Import/MAI0106.aspx?projcode=<%=projcode%>&id=<%=id%>&period=<%=period%>" onclick="javascript:SaveData();" class="left_petxt"><asp:Literal ID="Literal12"  runat="server" Text="<%$ Resources:Labels,MAI0106 %>" /></a> <%}%>
                                                </li>                                          
                                                <li><%if (SessionInfo.CurrentFunction.ToUpper() == "MAI0107")
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal13"  runat="server" Text="<%$ Resources:Labels,MAI0107 %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Import/MAI0107.aspx?projcode=<%=projcode%>&id=<%=id%>&period=<%=period%>" onclick="javascript:SaveData();" class="left_petxt"><asp:Literal ID="Literal14"  runat="server" Text="<%$ Resources:Labels,MAI0107 %>" /></a> <%}%>
                                                </li>    
                                                <li><%if (SessionInfo.CurrentFunction.ToUpper() == "MAI010801")
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal5"  runat="server" Text="<%$ Resources:Labels,MAI010801 %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Import/MAI010801.aspx?projcode=<%=projcode%>&id=<%=id%>&period=<%=period%>" onclick="javascript:SaveData();" class="left_petxt"><asp:Literal ID="Literal6"  runat="server" Text="<%$ Resources:Labels,MAI010801 %>" /></a> <%}%>
                                                </li>                                                         
                                                </ul>
                                                
                                                
                                            </td>
                                        </tr>
                                    </table></div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

<script type="text/javascript">
function SaveData() { 
            
            alert(window.location);
            
            setCommand(COMMAND_SAVE); 
            alert(getCommand());
            document.forms[0].submit();
            return true;
        }     
</script>

        
       