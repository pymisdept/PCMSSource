<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Copy of NavMenu.ascx.cs" Inherits="NavMenu" %>

<%@ Import Namespace="PCCore" %>
<script language="javascript" src="../JavaScript/TreeView.js"></script>
<script language="javascript" src="../JavaScript/AdapterUtils.js"></script>
<% 
string appUrl = Config.AppBaseUrl;
string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
%>
<table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td height="42" valign="top">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="162px" valign="top">                        
                        <table width="100%" height="560px" border="0" cellspacing="0" cellpadding="0">
                            <tr>                                
                                <%if (SessionInfo.CurrentModule.ToUpper() == Consts.SecurityModule)
                                  {%>
                                <td height="128" valign="top" background="<%=themeUrl%>/images/Security_left.gif">
                                <%} %>    
                                   <%if (SessionInfo.CurrentModule.ToUpper() == Consts.LinkModule)
                                     {%>
                                <td height="128" valign="top" background="<%=themeUrl%>/images/link_left.gif">
                                <%} %> 
                                   
                                <%if (SessionInfo.CurrentModule.ToUpper() == "COMMON")
                                  {%>
                                <td height="128" valign="top" background="<%=themeUrl%>/images/Leave_left2.gif">
                                <%} %>       
                                <%if (SessionInfo.CurrentModule.ToUpper() == "QUANTITYSURVEY")
                                  {%>
                                <td height="128" valign="top" background="<%=themeUrl%>/images/Security_left.gif">
                                <%} %>     
                                <%if (SessionInfo.CurrentModule.ToUpper() == "PURCHASING")
                                  {%>
                                <td height="128" valign="top" background="<%=themeUrl%>/images/Security_left.gif">
                                <%} %>   
                                <%if (SessionInfo.CurrentModule.ToUpper() == "ACCOUNT")
                                  {%>
                                <td height="128" valign="top" background="<%=themeUrl%>/images/Security_left.gif">
                                <%} %>                           
                                <%if (SessionInfo.CurrentModule.ToUpper() == "MANAGEMENT")
                                  {%>
                                <td height="128" valign="top" background="<%=themeUrl%>/images/Security_left.gif">
                                <%} %>                           
                                <%if (SessionInfo.CurrentModule.ToUpper() == "SECURITY")
                                  {%>
                                <td height="128" valign="top" background="<%=themeUrl%>/images/Security_left.gif">
                                <%} %>                           
                                </td>
                            </tr>
                            <tr>                            
                                <td  valign="top" background="<%=themeUrl%>/images/personnel_left_02.gif"
                                    style="padding: 10px;padding-left:5px;padding-top:3px;">
                                    <%if (SessionInfo.CurrentModule.ToUpper() == Consts.SecurityModule)
                                      {%>                           
                                    <div class ="DivGridstyle_se">
                                    <%} %>
                                      <%if (SessionInfo.CurrentModule.ToUpper() == Consts.LinkModule)
                                        {%>                           
                                    <div class ="DivGridstyle_se">
                                    <%} %>
                                    <%if (SessionInfo.CurrentModule.ToUpper() == "QUANTITYSURVEY")
                                      {%>                           
                                    <div class ="DivGridstyle_se">
                                    <%} %>
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
                                               
                                                <%if (SessionInfo.CurrentModule.ToUpper() == "QUANTITYSURVEY")
                                                  {%>
                                                <%--<ul class="ul_se">
                                                <li class="AspNet-TreeView-Root"><%if (SessionInfo.CurrentFunction.ToUpper() == "PROJECTITEMSETUP")
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal1"  runat="server" Text="<%$ Resources:Labels,ProjectItemSetup %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Import/ProjectItemSetup.aspx" class="left_petxt"><asp:Literal ID="Literal2"  runat="server" Text="<%$ Resources:Labels,ProjectItemSetup %>" /></a><li>ABC</li><li>DEF</li> <%}%>
                                                </li>
                                                
                                                <li><%if (SessionInfo.CurrentFunction.ToUpper() == "SUBCONTRACTORPAYMENTCERTIFICATE")
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal3"  runat="server" Text="<%$ Resources:Labels,SubContractorPaymentCertificate %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Admin_Default.aspx" class="left_petxt"><asp:Literal ID="Literal4"  runat="server" Text="<%$ Resources:Labels,SubContractorPaymentCertificate %>" /></a> <%}%>
                                                </li>                                                
                                                </ul>--%>
                                                <%} %> 
                                                
                                                <%--<%if (SessionInfo.CurrentModule.ToUpper() == "PROJECT")
                                                  {%>
                                                <ul class="ul_se">
                                                <li><%if (SessionInfo.CurrentFunction.ToUpper() == "PROJECTSTATUSIMPORT")
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal9"  runat="server" Text="<%$ Resources:Labels,ProjectStatusImport %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Import/ProjectStatusImport.aspx" class="left_petxt"><asp:Literal ID="Literal10"  runat="server" Text="<%$ Resources:Labels,ProjectStatusImport %>" /></a> <%}%>
                                                </li>
                                                
                                                <li><%if (SessionInfo.CurrentFunction.ToUpper() == "INSTRUCTIONLISTIMPORT")
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal11"  runat="server" Text="<%$ Resources:Labels,InstructionListImport %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Import/InstructionListImport.aspx" class="left_petxt"><asp:Literal ID="Literal12"  runat="server" Text="<%$ Resources:Labels,InstructionListImport %>" /></a> <%}%>
                                                </li>
                                                
                                                <li><%if (SessionInfo.CurrentFunction.ToUpper() == "PORJECTREPORTIMPORT")
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal13"  runat="server" Text="<%$ Resources:Labels,PorjectReportImport %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Import/PorjectReportImport.aspx" class="left_petxt"><asp:Literal ID="Literal14"  runat="server" Text="<%$ Resources:Labels,PorjectReportImport %>" /></a> <%}%>
                                                </li>
                                                
                                                <li><%if (SessionInfo.CurrentFunction.ToUpper() == "BUDGETADJUSTMENTIMPORT")
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal15"  runat="server" Text="<%$ Resources:Labels,BudgetAdjustmentImport %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Import/BudgetAdjustmentImport.aspx" class="left_petxt"><asp:Literal ID="Literal16"  runat="server" Text="<%$ Resources:Labels,BudgetAdjustmentImport %>" /></a> <%}%>
                                                </li>
                                                
                                                </ul>
                                                <%} %> --%>
                                               
                                               <%-- ==================================SecurityModule===========================================--%>
                                               
                                                <%if (SessionInfo.CurrentModule.ToUpper() == Consts.SecurityModule)
                                                  {%>
                                                <ul class="ul_se">
                                                <li><%if (SessionInfo.CurrentFunction.ToUpper() == Consts.ScGroupFunc)
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal  runat="server" Text="<%$ Resources:Labels,ScGroup %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Security/ScGroup.aspx" class="left_petxt"><asp:Literal  runat="server" Text="<%$ Resources:Labels,ScGroup %>" /></a> <%}%>
                                                </li>
                                                <li><%if (SessionInfo.CurrentFunction.ToUpper() == Consts.ScGroupRightFunc)
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal  runat="server" Text="<%$ Resources:Labels,ScGroupRight %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Security/ScGroupRight.aspx" class="left_petxt"><asp:Literal runat="server" Text="<%$ Resources:Labels,ScGroupRight %>" /></a> <%}%>
                                                </li>    
                                                <li><%if (SessionInfo.CurrentFunction.ToUpper() == Consts.ScUserFunc)
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal  runat="server" Text="<%$ Resources:Labels,ScUser %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Security/ScUser.aspx" class="left_petxt"><asp:Literal runat="server" Text="<%$ Resources:Labels,ScUser %>" /></a> <%}%>
                                                </li>
                                                <li><%if (SessionInfo.CurrentFunction.ToUpper() == Consts.ScUserRightFunc)
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal  runat="server" Text="<%$ Resources:Labels,ScUserRight %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Security/ScUserRight.aspx" class="left_petxt"><asp:Literal runat="server" Text="<%$ Resources:Labels,ScUserRight %>" /></a> <%}%>
                                                </li>
                                                <%--<li><%if (SessionInfo.CurrentFunction.ToUpper() == Consts.ScFunctionFunc)
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal248"  runat="server" Text="<%$ Resources:Labels,ScFunction %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Security/ScFunction.aspx" class="left_petxt"><asp:Literal ID="Literal249" runat="server" Text="<%$ Resources:Labels,ScFunction %>" /></a> <%}%>
                                                </li> --%>
                                                <li><%if (SessionInfo.CurrentFunction.ToUpper() == Consts.ScLogFunc)
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal  runat="server" Text="<%$ Resources:Labels,ScAccessLog %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Security/ScAccessLog.aspx" class="left_petxt"><asp:Literal  runat="server" Text="<%$ Resources:Labels,ScAccessLog %>" /></a> <%}%>
                                                </li> 
                                                <li><%if (SessionInfo.CurrentFunction.ToUpper() == Consts.ScAuditTrailFunc)
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal32"  runat="server" Text="<%$ Resources:Labels,ScAuditLog %>" /></div><%}
                                                      else
                                                      {%><a href="<%=appUrl %>/Security/ScAuditLog.aspx" class="left_petxt"><asp:Literal ID="Literal33" runat="server" Text="<%$ Resources:Labels,ScAuditLog %>" /></a><%}%>
                                                </li> 
                                                <li><%if (SessionInfo.CurrentFunction.ToUpper() == "SCANNOUNCEMENT")
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal276"  runat="server" Text="<%$ Resources:Labels,ScAnnouncement %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Security/ScAnnouncement.aspx" class="left_petxt"><asp:Literal ID="Literal277" runat="server" Text="<%$ Resources:Labels,ScAnnouncement %>" /></a><%}%>
                                                </li>  
                                                <%--<li><%if (SessionInfo.CurrentFunction.ToUpper() == "SCCUSTERHOMEPAGE")
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal120"  runat="server" Text="<%$ Resources:Labels,ScCusterHomePage %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Security/ScCusterHomePage.aspx" class="left_petxt"><asp:Literal ID="Literal121" runat="server" Text="<%$ Resources:Labels,ScCusterHomePage %>" /></a><%}%>
                                                </li>       --%>                                          
                                                
                                                <li><%if (SessionInfo.CurrentFunction.ToUpper() == "SCUSEFULLLINK")
                                                      {%>
                                                            <div class="left_seseltxt"><asp:Literal ID="Literal274"  runat="server" Text="<%$ Resources:Labels,ScUseFullLink %>" /></div><%}
                                                      else
                                                      {%> <a href="<%=appUrl %>/Security/ScUseFullLink.aspx" class="left_petxt"><asp:Literal ID="Literal275" runat="server" Text="<%$ Resources:Labels,ScUseFullLink %>" /></a><%}%>
                                                </li>
                                                </ul>
                                                <%} %>                                               
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