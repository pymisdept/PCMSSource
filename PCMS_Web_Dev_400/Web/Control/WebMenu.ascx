
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WebMenu.ascx.cs" Inherits="WebMenu" %>
<%@ Import Namespace="PCCore" %>

<% 
    string appUrl = Config.AppBaseUrl;
    string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
    string ReportLocation = ConfigurationManager.AppSettings["ReportLocation"].ToString();
%>
<table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:Menu runat="server" ID="HRMenu" Visible="false" DisappearAfter="500" Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="false" DynamicEnableDefaultPopOutImage="false">
                <%--           <StaticMenuItemStyle  Height="20px" Font-Size="12px" Font-Bold="True" ForeColor="White" BackColor="Black" horizontalpadding="10px" Font-Names="Arial" />
           <StaticHoverStyle Font-Underline="False" Font-Size="12px"  ForeColor="White" BackColor="#666666" Font-Names="Arial" />--%>
                <%--<DynamicMenuItemStyle Height="20px"  ForeColor="White" BackColor="Black" horizontalpadding="7px"  />
           <DynamicHoverStyle  Font-Underline="False"  ForeColor="White" BackColor="#666666"  />--%>
                <%-- 
           <StaticItemTemplate>
            <%
                string appUrl = Config.AppBaseUrl;
            %>
            
           <a href='<%=appUrl%><%# Eval("NavigateUrl") %>' class="main_menu"><asp:Literal ID="lbllink" Text='<%# Eval("Text") %>' runat="server"></asp:Literal><span style="width:10px;"></span></a>
           </StaticItemTemplate>--%>
                <%--<StaticMenuItemStyle HorizontalPadding="8px" VerticalPadding="2px" CssClass="" Font-Names="Arial"
                    Font-Bold="true" ForeColor="white" />--%>
                <StaticMenuItemStyle HorizontalPadding="20px" VerticalPadding="2px" CssClass="" Font-Names="Arial"
                    Font-Bold="true" height="10px" ForeColor="White" />
                <StaticMenuStyle />
                <DynamicMenuItemStyle VerticalPadding="3px" ForeColor="White" Height="20px" Font-Bold="true" />
                <LevelMenuItemStyles>
                    <asp:MenuItemStyle Font-Size="Larger" Height="10px" />
                </LevelMenuItemStyles>
                
                <StaticHoverStyle Font-Size="Larger" ForeColor="White" />
                <DynamicItemTemplate>
                    <%
                        string appUrl = Config.AppBaseUrl;
                    %>
                    <% if (SessionInfo.CurrentLanguage == "en-us" || SessionInfo.CurrentLanguage == "ja-JP")
                       { %>
                    
                    <a href='<%=appUrl%><%# Eval("NavigateUrl") %>'  class="sub_menu" >
                        <img src='<%=appUrl%><%# Eval("ImageUrl") %>' style="border: 0; font-size:larger;" /><asp:Literal  ID="lbllink"
                            Text='<%# Eval("Text") %>'  runat="server"></asp:Literal><span style="width: 7px;"></span></a>
                    <%}
                       else
                       {%>
                    <a href='<%=appUrl%><%# Eval("NavigateUrl") %>' class="sub_menu_zh">
                        <img src='<%=appUrl%><%# Eval("ImageUrl") %>' style="border: 0; font-size:larger;" /><asp:Literal ID="Literal1"
                            Text='<%# Eval("Text") %>' runat="server"></asp:Literal><span style="width: 7px;"></span></a>
                    <% } %>
                    
                </DynamicItemTemplate>
                
                <Items>
                
                    <%--Site Module--%>
                     <asp:MenuItem   Text="<%$ Resources:Labels,Site %>"> 
                     
                        
                        <asp:MenuItem Text="<%$ Resources:Labels,PUI01 %>" NavigateUrl="/Import/PUI01.aspx"
                        ImageUrl="/App_Themes/Default/images/arrow00.gif">
                        </asp:MenuItem>
 <%--
                        <asp:MenuItem Text="<%$ Resources:Labels,MA01M %>" ImageUrl="/App_Themes/Default/images/arrow00.gif" 
                                NavigateUrl="/Import/MAI01.aspx">
                                <asp:MenuItem  Text="<%$ Resources:Labels,MAI01 %>" NavigateUrl="/Import/MAI01.aspx"
                                    ImageUrl="/App_Themes/Default/images/arrow00.gif">
                                </asp:MenuItem>    
                               
                                <asp:MenuItem Text="<%$ Resources:Labels,MA01 %>" NavigateUrl="/Report/MA01.aspx"
                                    ImageUrl="/App_Themes/Default/images/arrow00.gif">
                                </asp:MenuItem>    
                        
                        </asp:MenuItem>    
                        --%>
                
                        <asp:MenuItem  Text="<%$ Resources:Labels,MAI01 %>" NavigateUrl="/Import/MAI01.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                        </asp:MenuItem>    
                       <%--
                         <asp:MenuItem  Text="<%$ Resources:Labels,Reverse %>" NavigateUrl="/Import/Reverse.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                        </asp:MenuItem>    
                        --%>
                       
                    
                    </asp:MenuItem>
                    
                    <%--Purchasing Module--%>
                    <asp:MenuItem   Text="<%$ Resources:Labels,Purchasing %>"> <%--NavigateUrl="../Admin_Default.aspx"--%>
                    
                        
                        <%--Material Purchase--%>
                        <%--
                        <asp:MenuItem Text="<%$ Resources:Labels,PUI02M %>"   NavigateUrl="/Import/PUI02.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                           
                            <asp:MenuItem  Text="<%$ Resources:Labels,PUI02 %>" NavigateUrl="/Import/PUI02.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                         --%>
                         
                            <asp:MenuItem Text="<%$ Resources:Labels,PU99 %>" NavigateUrl="/Report/PU99.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                        
                        
                        
                        <%--Purchase Agreement--%>
                        <asp:MenuItem  Text="<%$ Resources:Labels,PUI03M %>"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif" NavigateUrl="/Import/PUI03.aspx">
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,PUI03 %>" NavigateUrl="/Import/PUI03.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,PU03 %>" NavigateUrl="/Report/PU03.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,PU04 %>" NavigateUrl="/Report/PU04.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                        </asp:MenuItem>
                        
                        
                       <asp:MenuItem Text="<%$ Resources:Labels,PUI08 %>" NavigateUrl="/Import/PUI08.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                       </asp:MenuItem>
                       
                        <%--Confirmation Order--%>
                        <%--
                        <asp:MenuItem Text="<%$ Resources:Labels,PUI04M %>" NavigateUrl="/Import/PUI04.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,PUI04 %>" NavigateUrl="/Import/PUI04.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,PU05 %>" NavigateUrl="/Report/PU05.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                        </asp:MenuItem>
                        --%>
                        
                        <%--Goods Received--%>
                        <%--
                        <asp:MenuItem Text="<%$ Resources:Labels,PUI06 %>" NavigateUrl="/Import/PUI06.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif">
                        </asp:MenuItem>
                        --%>
                        
                        <%--Remote Desktop--%>
						<%--
                        <asp:MenuItem Text="<%$ Resources:Labels,RDP %>" NavigateUrl="/RDP/RDP.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif">
                        </asp:MenuItem>
						--%>
                                                    
                        <%--Supplier Payment Certificate--%>
                        <%--
                        <asp:MenuItem Text="<%$ Resources:Labels,PUI05M %>" NavigateUrl="/Import/PUI05.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,PUI05 %>" NavigateUrl="/Import/PUI05.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,PU08 %>" NavigateUrl="/Report/PU08.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                        </asp:MenuItem>
                        --%>

                        <%--Reports--%>                            
                        <asp:MenuItem Text="<%$ Resources:Labels,Report %>" NavigateUrl="/Report/PU06.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,PU06 %>" NavigateUrl="/Report/PU06.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            <asp:MenuItem Text="<%$ Resources:Labels,PU07 %>" NavigateUrl="/Report/PU07.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            <asp:MenuItem Text="<%$ Resources:Labels,PU09 %>" NavigateUrl="/Report/PU09.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            <asp:MenuItem Text="<%$ Resources:Labels,PU10 %>" NavigateUrl="/Report/PU10.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            <asp:MenuItem Text="<%$ Resources:Labels,PU11 %>" NavigateUrl="/Report/PU11.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            <asp:MenuItem Text="<%$ Resources:Labels,PU12 %>" NavigateUrl="/Report/PU12.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            <asp:MenuItem Text="<%$ Resources:Labels,PU13 %>" NavigateUrl="/Report/PU13.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                        </asp:MenuItem>
                    
                    </asp:MenuItem>
                    
                    <%--Commercial Module--%>
                    <asp:MenuItem Text="<%$ Resources:Labels,QuantitySurvey %>"> <%--NavigateUrl="../Admin_Default.aspx">--%>

                        <%--Project General Informaiton--%>
                        <asp:MenuItem Text="<%$ Resources:Labels,QSI01M %>" NavigateUrl="/Import/QSI01.aspx" 
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            
<%--                            <asp:MenuItem Text="<%$ Resources:Labels,QSI01 %>" NavigateUrl="/Import/QSI01.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QS01 %>" NavigateUrl="/Report/QS01.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>--%>
                        </asp:MenuItem>
                        
                        <%--Project Bill of Quantities--%>
                        <asp:MenuItem Text="<%$ Resources:Labels,QSI02 %>" NavigateUrl="/Import/QSI02.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                        </asp:MenuItem>
                            
                            <%--<asp:MenuItem Text="<%$ Resources:Labels,QS02 %>" NavigateUrl="/Report/QS02.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>--%>
                        
                        <%--Project Budget--%>
                        <asp:MenuItem Text="<%$ Resources:Labels,QSI03 %>" NavigateUrl="/Import/QSI03.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"
                            SeparatorImageUrl="~/App_Themes/Default/images/separate-line.gif">
                        </asp:MenuItem>
                        
                        <%--SubContract Bill of Quantities--%>  
                        <asp:MenuItem Text="<%$ Resources:Labels,QSI04 %>" NavigateUrl="/Import/QSI04.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                        </asp:MenuItem>
                        
                        <%--SubContract BQ Budget--%>  
                        <asp:MenuItem Text="<%$ Resources:Labels,QSI41 %>" NavigateUrl="/Import/QSI41.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif" >
                        </asp:MenuItem>
                        
                        <%--SubContractor Payment Certificate--%>
                        <asp:MenuItem Text="<%$ Resources:Labels,QSI12M %>" ImageUrl="/App_Themes/Default/images/arrow00.gif" 
                            NavigateUrl="/Import/QSI12.aspx"
                            SeparatorImageUrl="~/App_Themes/Default/images/separate-line.gif">
                            
<%-- Commented by Eric                           <asp:MenuItem Text="<%$ Resources:Labels,QSI12 %>" NavigateUrl="/Import/QSI12.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QS13 %>" NavigateUrl="/Report/QS13.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif" >
                            </asp:MenuItem>--%>
                            
                        </asp:MenuItem>
                        
                        <%--Interim Payment Grouping--%>    
                         <asp:MenuItem Text="<%$ Resources:Labels,QSI24M %>"  
                            ImageUrl="/App_Themes/Default/images/arrow00.gif" NavigateUrl="/Import/QSI24.aspx"
                            >
                             
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QSI24 %>" NavigateUrl="/Import/QSI24.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            <asp:MenuItem Text="<%$ Resources:Labels,QS34 %>" NavigateUrl="/Report/QS34.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif" > 
                            </asp:MenuItem>
                        </asp:MenuItem>        
                                 
                        <%--Payment Application--%>
                        <asp:MenuItem Text="<%$ Resources:Labels,QSI17M %>" ImageUrl="/App_Themes/Default/images/arrow00.gif" 
                            NavigateUrl="/Import/QSI17.aspx" >
                            
<%-- Commented by Eric                           <asp:MenuItem Text="<%$ Resources:Labels,QSI17 %>" NavigateUrl="/Import/QSI17.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            <asp:MenuItem Text="<%$ Resources:Labels,QS22 %>" NavigateUrl="/Report/QS22.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>--%>
                        </asp:MenuItem>
                        
                        <%--Payment Certificate--%>
                         <asp:MenuItem Text="<%$ Resources:Labels,QSI18 %>" NavigateUrl="/Import/QSI18.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>

                        <%--Other Income Application --%>
                        <asp:MenuItem Text="<%$ Resources:Labels,QSI43 %>" ImageUrl="/App_Themes/Default/images/arrow00.gif" 
                            NavigateUrl="/Import/QSI43.aspx" >
                        </asp:MenuItem>
                        <%--Other Income Certificate --%>
                        <asp:MenuItem Text="<%$ Resources:Labels,QSI44 %>" ImageUrl="/App_Themes/Default/images/arrow00.gif" 
                            NavigateUrl="/Import/QSI44.aspx" SeparatorImageUrl="~/App_Themes/Default/images/separate-line.gif">
                        </asp:MenuItem>
                        
                        <%--Project Report--%>
                        <asp:MenuItem Text="<%$ Resources:Labels,QSI40 %>"  ImageUrl="/App_Themes/Default/images/arrow00.gif"
                            NavigateUrl="/Import/QSI40.aspx">
                        </asp:MenuItem>
                        
                        <%--Variation Order--%>
                        <%--
                        <asp:MenuItem Text="<%$ Resources:Labels,QSI23M %>"  SeparatorImageUrl="~/App_Themes/Default/images/separate-line.gif" 
                            ImageUrl="/App_Themes/Default/images/arrow00.gif" NavigateUrl="/Import/QSI23.aspx" >
                        --%>
<%--                            <asp:MenuItem Text="<%$ Resources:Labels,QSI23 %>" NavigateUrl="/Import/QSI23.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>--%>
                            <%--
                            <asp:MenuItem Text="<%$ Resources:Labels,QS33 %>" NavigateUrl="/Report/QS33.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem> --%>
                            
<%--                            <asp:MenuItem Text="<%$ Resources:Labels,QSI07 %>" NavigateUrl="/Import/QSI07.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QSI08 %>" NavigateUrl="/Import/QSI08.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            
                            </asp:MenuItem>--%>
                        
                           
                            <asp:MenuItem Text="<%$ Resources:Labels,QSI42 %>" NavigateUrl="/Import/QSI42.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <%--Project Accrual Cost--%>
                            <asp:MenuItem Text="<%$ Resources:Labels,QSI33 %>" ImageUrl="/App_Themes/Default/images/arrow00.gif"
                                NavigateUrl="/Import/QSI33.aspx" SeparatorImageUrl="~/App_Themes/Default/images/separate-line.gif">
                            </asp:MenuItem>
                        
                        
                        <%--Payment Application by IP Grouping--%>
                        <%--

                        <asp:MenuItem Text="<%$ Resources:Labels,QSI27M %>" ImageUrl="/App_Themes/Default/images/arrow00.gif" 
                            NavigateUrl="/Import/QSI27.aspx">
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QSI27 %>" NavigateUrl="/Import/QSI27.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            <asp:MenuItem Text="<%$ Resources:Labels,QS44 %>" NavigateUrl="/Report/QS44.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                        </asp:MenuItem>
                        --%>
                        
                        <%--Payment Cert by IP Grouping--%>                      
                        <%--
                        <asp:MenuItem Text="<%$ Resources:Labels,QSI28 %>" NavigateUrl="/Import/QSI28.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif" SeparatorImageUrl="~/App_Themes/Default/images/separate-line.gif">
                        </asp:MenuItem>
                        --%>
                        
                        
                        <%--Project Accrual Cost--%>
                   
                        
                        <%--Project Accrual Income--%>
                        <%-- %>
                        <asp:MenuItem Text="<%$ Resources:Labels,QSI26 %>"  ImageUrl="/App_Themes/Default/images/arrow00.gif"
                            NavigateUrl="/Import/QSI26.aspx">
                        </asp:MenuItem>
                        -->
                        
                        <%--Project Trends Details--%>
                        
                        <%--IP Status for Current Projects--%>
                        <asp:MenuItem Text="<%$ Resources:Labels,QSI30 %>"  ImageUrl="/App_Themes/Default/images/arrow00.gif"
                             NavigateUrl="/Import/QSI30.aspx">
                        </asp:MenuItem>
                        
                        <%--Cash Out for Current Projects Upload and Download--%>
                        <asp:MenuItem Text="<%$ Resources:Labels,ACI01 %>"  ImageUrl="/App_Themes/Default/images/arrow00.gif"
                            NavigateUrl="/Import/ACI01.aspx">
                        </asp:MenuItem>
                        
                        <%--Project Fiscal Report - Year Forecast--%>
                        <asp:MenuItem Text="<%$ Resources:Labels,QSI29 %>"  ImageUrl="/App_Themes/Default/images/arrow00.gif"
                            NavigateUrl="/Import/QSI29.aspx">
                        </asp:MenuItem>                        
                        
                        <%--Project Fiscal Report - General Expense Upload and Download--%>                                 
                        <asp:MenuItem Text="<%$ Resources:Labels,ACI02 %>"  ImageUrl="/App_Themes/Default/images/arrow00.gif"
                            NavigateUrl="/Import/ACI02.aspx" >
                        </asp:MenuItem>
                        
                        <%--Annual Project Forecast and Budget--%>
                        <asp:MenuItem Text="<%$ Resources:Labels,QSI21M %>" ImageUrl="/App_Themes/Default/images/arrow00.gif" 
                            NavigateUrl="/Import/QSI21.aspx">
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QSI21 %>" NavigateUrl="/Import/QSI21.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QS29 %>" NavigateUrl="/Report/QS29.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                        </asp:MenuItem>
                        
                        <%--Annual Project Cashflow Budget--%>
                        <asp:MenuItem Text="<%$ Resources:Labels,QSI22M %>" ImageUrl="/App_Themes/Default/images/arrow00.gif" 
                            NavigateUrl="/Import/QSI22.aspx" >
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QSI22 %>" NavigateUrl="/Import/QSI22.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QS30 %>" NavigateUrl="/Report/QS30.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                        </asp:MenuItem>
                        
                       <%--Project Status--%>
                        <asp:MenuItem Text="<%$ Resources:Labels,QSI31 %>"  ImageUrl="/App_Themes/Default/images/arrow00.gif"
                            NavigateUrl="/Import/QSI31.aspx">
                        </asp:MenuItem>
                      
                       <%--Project Budget Transfer--%>
                        <asp:MenuItem Text="<%$ Resources:Labels,QSI32 %>"  ImageUrl="/App_Themes/Default/images/arrow00.gif"
                            NavigateUrl="/Import/QSI32.aspx">
                        </asp:MenuItem> 
                        
                        <%--Reports--%>
                        <asp:MenuItem Text="<%$ Resources:Labels,Report %>" ImageUrl="/App_Themes/Default/images/arrow00.gif" 
                            NavigateUrl="/Report/QS15.aspx">
                            
                            <%--<asp:MenuItem Text="<%$ Resources:Labels,QS14 %>" NavigateUrl="/Report/QS14.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>--%>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QS46 %>"  SeparatorImageUrl="~/App_Themes/Default/images/separate-line.gif" 
                                NavigateUrl="/Report/QS46.aspx" ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QS15 %>" NavigateUrl="/Report/QS15.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QS16 %>" NavigateUrl="/Report/QS16.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QS17 %>"  SeparatorImageUrl="~/App_Themes/Default/images/separate-line.gif" 
                                NavigateUrl="/Report/QS17.aspx" ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QS45 %>"  SeparatorImageUrl="~/App_Themes/Default/images/separate-line.gif" 
                                NavigateUrl="/Report/QS45.aspx" ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
<%--
                            <asp:MenuItem Text="<%$ Resources:Labels,QS25 %>" NavigateUrl="/Report/QS25.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QS26 %>" NavigateUrl="/Report/QS26.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                     
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QS27%>" NavigateUrl="/Report/QS27.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
--%>                            
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QS28 %>" NavigateUrl="/Report/QS28.aspx" 
                                ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            <%--
                            <asp:MenuItem Text="<%$ Resources:Labels,QS35 %>" NavigateUrl="/Report/QS35.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QS18 %>" NavigateUrl="/Report/QS18.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QS19 %>" NavigateUrl="/Report/QS19.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QS20 %>" NavigateUrl="/Report/QS20.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            --%>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QS43 %>" NavigateUrl="/Report/QS43.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QS47 %>"  NavigateUrl="/Report/QS47.aspx" ImageUrl="/App_Themes/Default/images/arrow00.gif" SeparatorImageUrl="~/App_Themes/Default/images/separate-line.gif" >
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,QS48 %>" SeparatorImageUrl="~/App_Themes/Default/images/separate-line.gif"  NavigateUrl="/Report/QS48.aspx" ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                        </asp:MenuItem>
                    </asp:MenuItem>
                    
                    <%--Accounts Module--%>
                    <asp:MenuItem Text="<%$ Resources:Labels,Account %>"> <%--NavigateUrl="../Admin_Default.aspx">--%>
                    
                           

                        
                        <%--Reports--%>
                        <%--
                        <asp:MenuItem Text="<%$ Resources:Labels,Report %>" ImageUrl="/App_Themes/Default/images/arrow00.gif" 
                            NavigateUrl="/Report/AC01.aspx">
                        --%>
                            <asp:MenuItem Text="<%$ Resources:Labels,AC01 %>" NavigateUrl="/Report/AC01.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,AC02 %>" NavigateUrl="/Report/AC02.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            <asp:MenuItem Text="<%$ Resources:Labels,AC03 %>" NavigateUrl="/Report/AC03.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            <asp:MenuItem Text="<%$ Resources:Labels,AC04 %>" NavigateUrl="/Report/AC04.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            <asp:MenuItem Text="<%$ Resources:Labels,AC05 %>" NavigateUrl="/Report/AC05.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            <asp:MenuItem Text="<%$ Resources:Labels,AC06 %>" NavigateUrl="/Report/AC06.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            <asp:MenuItem Text="<%$ Resources:Labels,AC07 %>" NavigateUrl="/Report/AC07.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,AC08 %>" NavigateUrl="/Report/AC08.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <asp:MenuItem Text="<%$ Resources:Labels,AC09 %>" NavigateUrl="/Report/AC09.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif">
                            </asp:MenuItem>
                            
                            <%--<asp:MenuItem Text="<%$ Resources:Labels,CashFlowReconciliation %>" NavigateUrl="/Admin_Default.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                                --%>
                        </asp:MenuItem>
                         
                    

                    
                    <%--Management Module--%>
                    <asp:MenuItem Text="<%$ Resources:Labels,Management %>"> <%--NavigateUrl="../Admin_Default.aspx">--%>
                        
                        
                        <%--Non-Project Base Income and Expenditure--%>
                        <asp:MenuItem Text="<%$ Resources:Labels,MAI02 %>"  ImageUrl="/App_Themes/Default/images/arrow00.gif"
                            NavigateUrl="/Import/MAI02.aspx">
                        </asp:MenuItem> 
                        
                        <%--Reports--%>
                        <asp:MenuItem Text="<%$ Resources:Labels,Report %>" NavigateUrl="/Report/MA02.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                        
                           
                            <asp:MenuItem Text="<%$ Resources:Labels,MA02 %>" NavigateUrl="/Report/MA02.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>    
                                
                             <asp:MenuItem Text="<%$ Resources:Labels,MA03 %>" NavigateUrl="/Report/MA03.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>    
                             
                             <asp:MenuItem Text="<%$ Resources:Labels,MA04 %>" NavigateUrl="/Report/MA04.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>    
                             
                             <asp:MenuItem Text="<%$ Resources:Labels,MA05 %>" NavigateUrl="/Report/MA05.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>    
                             
                             <asp:MenuItem Text="<%$ Resources:Labels,MA06 %>" NavigateUrl="/Report/MA06.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>    
                             
                             <asp:MenuItem Text="<%$ Resources:Labels,MA07 %>" NavigateUrl="/Report/MA07.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>    
                                
                             <asp:MenuItem Text="<%$ Resources:Labels,MA08 %>" NavigateUrl="/Report/MA08.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>    
                                
                             <asp:MenuItem Text="<%$ Resources:Labels,MA09 %>" NavigateUrl="/Report/MA09.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>    
                             
                             <asp:MenuItem Text="<%$ Resources:Labels,MA10 %>" NavigateUrl="/Report/MA10.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>    
                             
                             <asp:MenuItem Text="<%$ Resources:Labels,MA11 %>" NavigateUrl="/Report/MA11.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>    
                                
                             <asp:MenuItem Text="<%$ Resources:Labels,MA12 %>" NavigateUrl="/Report/MA12.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>    
                             
                             <asp:MenuItem Text="<%$ Resources:Labels,MA13 %>" NavigateUrl="/Report/MA13.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>    
                                
                             <asp:MenuItem Text="<%$ Resources:Labels,MA14 %>" NavigateUrl="/Report/MA14.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>    
                                
                             <asp:MenuItem Text="<%$ Resources:Labels,MA15 %>" NavigateUrl="/Report/MA15.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>    
                                
                             <asp:MenuItem Text="<%$ Resources:Labels,MA16 %>" NavigateUrl="/Report/MA16.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>    
                                
                             <asp:MenuItem Text="<%$ Resources:Labels,MA17 %>" NavigateUrl="/Report/MA17.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>    
                                
                             <asp:MenuItem Text="<%$ Resources:Labels,MA18 %>" NavigateUrl="/Report/MA18.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>    
                                
                             <asp:MenuItem Text="<%$ Resources:Labels,MA19 %>" NavigateUrl="/Report/MA19.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>    
                       
                        </asp:MenuItem>
                       
                    </asp:MenuItem>
                    
                    <%--Tools Module--%>
                    <asp:MenuItem Text="<%$ Resources:Labels,Tools %>">
                   
                        <asp:MenuItem Text="<%$ Resources:Labels,DownloadManager %>" NavigateUrl="/Document/DownloadManager.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif">
                        </asp:MenuItem> 
						<%--
                        <asp:MenuItem Text="<%$ Resources:Labels,UploadManager %>" NavigateUrl="/Document/DownloadManager.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif" SeparatorImageUrl="~/App_Themes/Default/images/separate-line.gif">
                        </asp:MenuItem>    
						--%>
						<%-- 
                        <asp:MenuItem Text="<%$ Resources:Labels,ScAnnouncement %>" NavigateUrl="/security/ScAnnouncement.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>                        
                        <asp:MenuItem Text="<%$ Resources:Labels,ScUseFullLink %>" NavigateUrl="/security/ScUseFullLink.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        --%>
                    </asp:MenuItem>
                    
                    <%--Security Module--%>
                    <asp:MenuItem Text="<%$ Resources:Labels,Security %>"> <%--NavigateUrl="../security/ScGroup.aspx">--%>
                        <asp:MenuItem Text="<%$ Resources:Labels,ScGroup %>" NavigateUrl="/security/ScGroup.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,ScUser %>" NavigateUrl="/security/ScUser.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,ScUserRight %>" NavigateUrl="/security/ScUserRight.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <%--<asp:MenuItem Text="<%$ Resources:Labels,ScFunction %>" NavigateUrl="/security/ScFunction.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>--%>
                        <asp:MenuItem Text="<%$ Resources:Labels,ScAccessLog %>" NavigateUrl="/security/ScAccessLog.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,ScAuditLog %>" NavigateUrl="/security/ScAuditLog.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif" SeparatorImageUrl="~/App_Themes/Default/images/separate-line.gif">
                        </asp:MenuItem>
                        
                        <asp:MenuItem Text="<%$ Resources:Labels,SEI01 %>" NavigateUrl="/Import/SEI01.aspx"
                                ImageUrl="/App_Themes/Default/images/arrow00.gif">
                                
                        </asp:MenuItem> 
                        <%--<asp:MenuItem Text="<%$ Resources:Labels,CustomizingFieldPage %>" NavigateUrl="/security/CustomizingFieldPage.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif" SeparatorImageUrl="~/App_Themes/Default/images/separate-line.gif">
                        </asp:MenuItem>--%>
                    </asp:MenuItem>
                    
                </Items>
                <%--<Items>
                    <asp:MenuItem Text="<%$ Resources:Labels,Project %>" NavigateUrl="~/Import/ProjectStatusImport.aspx?_slbg=0&amp;_slbi=0">
                        <asp:MenuItem Text="<%$ Resources:Labels,ProjectStatusImport %>" NavigateUrl="/Import/ProjectStatusImport.aspx?_slbg=0&amp;_slbi=0"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,InstructionListImport %>" NavigateUrl="/Import/InstructionListImport.aspx?_slbg=0&amp;_slbi=1"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,PorjectReportImport %>" NavigateUrl="/Import/PorjectReportImport.aspx?_slbg=0&amp;_slbi=1"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,BudgetAdjustmentImport %>" NavigateUrl="/Import/BudgetAdjustmentImport.aspx?_slbg=0&amp;_slbi=1"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                    </asp:MenuItem>
                    <asp:MenuItem Text="<%$ Resources:Labels,ARInvoice %>" NavigateUrl="~/Import/PaymentApplicationImport.aspx?_slbg=0&amp;_slbi=0">
                        <asp:MenuItem Text="<%$ Resources:Labels,PaymentApplicationImport %>" NavigateUrl="/Import/PaymentApplicationImport.aspx?_slbg=0&amp;_slbi=0"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,PaymentCertExport %>" NavigateUrl="/Import/PaymentCertExport.aspx?_slbg=0&amp;_slbi=1"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                    </asp:MenuItem>
                    <asp:MenuItem Text="<%$ Resources:Labels,SubContractor %>" NavigateUrl="~/Import/SubConPaymentCertImport.aspx?_slbg=0&amp;_slbi=0">
                        <asp:MenuItem Text="<%$ Resources:Labels,SubConPaymentCertImport %>" NavigateUrl="/Import/SubConPaymentCertImport.aspx?_slbg=0&amp;_slbi=0"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,SubConPaymentCertStatusImport %>" NavigateUrl="/Import/SubConPaymentCertStatusImport.aspx?_slbg=0&amp;_slbi=1"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                    </asp:MenuItem>
                    <asp:MenuItem Text="<%$ Resources:Labels,Purchasing %>" NavigateUrl="~/Import/PurchaseAgreementImport.aspx?_slbg=0&amp;_slbi=0">
                        <asp:MenuItem Text="<%$ Resources:Labels,PurchaseAgreementImport %>" NavigateUrl="/Import/PurchaseAgreementImport.aspx?_slbg=0&amp;_slbi=0"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,PurchaseAgreementExport %>" NavigateUrl="/Import/PurchaseAgreementExport.aspx?_slbg=0&amp;_slbi=1"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,SMRImport %>" NavigateUrl="/Import/SMRImport.aspx?_slbg=0&amp;_slbi=1"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,SMRExport %>" NavigateUrl="/Import/SMRExport.aspx?_slbg=0&amp;_slbi=1"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,PurchaseOrderImport %>" NavigateUrl="/Import/PurchaseOrderImport.aspx?_slbg=0&amp;_slbi=1"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,PurchaseOrderExport %>" NavigateUrl="/Import/PurchaseOrderExport.aspx?_slbg=0&amp;_slbi=1"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,SupplyPaymentCertImport %>" NavigateUrl="/Import/SupplyPaymentCertImport.aspx?_slbg=0&amp;_slbi=1"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,APSettlementStatusImport %>" NavigateUrl="/Import/APSettlementStatusImport.aspx?_slbg=0&amp;_slbi=1"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                    </asp:MenuItem>
                    <asp:MenuItem Text="<%$ Resources:Labels,Report %>" >
                    </asp:MenuItem>
                    <asp:MenuItem Text="<%$ Resources:Labels,Security %>" NavigateUrl="~/security/ScGroup.aspx">
                        <asp:MenuItem Text="<%$ Resources:Labels,ScGroup %>" NavigateUrl="/security/ScGroup.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,ScGroupRight %>" NavigateUrl="/security/ScGroupRight.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,ScUser %>" NavigateUrl="/security/ScUser.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,ScUserRight %>" NavigateUrl="/security/ScUserRight.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>                        
                        <asp:MenuItem Text="<%$ Resources:Labels,ScAccessLog %>" NavigateUrl="/security/ScAccessLog.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,ScAuditLog %>" NavigateUrl="/security/ScAuditLog.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif" SeparatorImageUrl="~/App_Themes/Default/images/separate-line.gif">
                        </asp:MenuItem>
                        <asp:MenuItem Text="<%$ Resources:Labels,ScAnnouncement %>" NavigateUrl="/security/ScAnnouncement.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>                        
                        <asp:MenuItem Text="<%$ Resources:Labels,ScUseFullLink %>" NavigateUrl="/security/ScUseFullLink.aspx"
                            ImageUrl="/App_Themes/Default/images/arrow00.gif"></asp:MenuItem>                        
                    </asp:MenuItem>
                </Items>--%>
            </asp:Menu>
        </td>
    </tr>
</table>

<script language="javascript">

function Click() {
    alert('Click');
    return false;
}
</script>
