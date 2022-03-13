<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PMMenu.ascx.cs" Inherits="Control_PMMenu" %>
<%@ Import Namespace="PCCore" %>
 <%
        string appUrl = PCCore.Config.AppBaseUrl;

        string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
        PCCore.Common.HRLog.RecordLog(themeUrl);
        //string projcode = Request.QueryString["projcode"] as string;
        //string id = Request.QueryString["id"] as string;
        //string period = Request.QueryString["period"] as string;

       
        //string b_viewmode = Convert.ToString((PCCore.PCMS.PMReport.CanModify(projcode, id) == false));
        //string b_AllowCancel = Convert.ToString((PCCore.PCMS.PMReport.CanCancel(projcode, id) == false));
        
    %>
        <img src="<%=themeUrl%>/line.jpg" style="width: 5%" />
        <PC:ImageButton ID="btnSection1" ImageUrl="<%=themeUrl%>/images/section1.jpg" runat="server" Width="7%"/>
        <img src="<%=themeUrl%>/line.jpg" style="width: 5%" />
        <PC:ImageButton ID="btnSection2" ImageUrl="<%=themeUrl%>/images/section2.jpg" runat="server" Width="7%"/>
        <img src="<%=themeUrl%>/line.jpg" style="width: 5%" />
        <PC:ImageButton ID="btnSection3" ImageUrl="<%=themeUrl%>/images/section3.jpg" runat="server" Width="7%"/>
        <img src="<%=themeUrl%>/line.jpg" style="width: 5%" />
        <PC:ImageButton ID="btnSection4" ImageUrl="<%=themeUrl%>/images/section4.jpg" runat="server" Width="7%"/>
        <img src="<%=themeUrl%>/line.jpg" style="width: 5%" />
        <PC:ImageButton ID="btnSection5" ImageUrl="<%=themeUrl%>/images/section5.jpg" runat="server" Width="7%"/>
        <img src="<%=themeUrl%>/line.jpg" style="width: 5%" />
        <PC:ImageButton ID="btnSection6" ImageUrl="<%=themeUrl%>/images/section6.jpg" runat="server" Width="7%"/>
        <img src="<%=themeUrl%>/line.jpg" style="width: 5%" />
        <PC:ImageButton ID="btnSection7" ImageUrl="<%=themeUrl%>/images/section7.jpg" runat="server" Width="7%"/>
        <img src="<%=themeUrl%>/line.jpg" style="width: 5%" />
    
