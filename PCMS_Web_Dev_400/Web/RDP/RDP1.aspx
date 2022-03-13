<%@ Page Language="C#" MasterPageFile="~/Control/MasterRDP.master" AutoEventWireup="true"
    CodeFile="RDP1.aspx.cs" Inherits="RDP1" %>

<%@ Import Namespace="PCCore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BaseContentPlaceHolder" runat="Server">
    <% 
        string appUrl = Config.AppBaseUrl;
        string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
        string isLogin = PCCore.SessionInfo.IsLogin.ToString();
        string SAPServer = Config.SAPServer.ToString();
        string UserID = SessionInfo.LoginName.ToString();
        string Password = SessionInfo.UserPassword.ToString();
        
        PCCore.Common.HRLog.RecordLog(SAPServer);
        PCCore.Common.HRLog.RecordLog(UserID);
        PCCore.Common.HRLog.RecordLog(Password);    
        string link = "./B1.htm?Server=" + SAPServer + "User=" + UserID + "Pass=" + Password;
    %>
    
    
    
    
    
    
    <script language="javascript">
    var frmMain = document.forms[0];
    var SAPServer = "<%=SAPServer %>";
    var UserID = "<%=UserID %>";
    var Password = "<%=Password %>";
         
    //Iframe1.src = "./B1.htm?Server=" + SAPServer + "&User=" + UserID + "&Pass=" + Password;     
    
    
    function selfToBeExecute()
    {
        
        
        //div_Announcement.insertAdjacentHTML("afterBegin", Admin_Default.GetHtmlTable());
        //if (typeof(Admin_Default) != "undefined" && Admin_Default !=null)
       // {
            //divUseFullLink.outerHTML =Admin_Default.GetHtmlTable("").value;
       // }
    }
    
    var oPopup = window.createPopup();
    function clickUsefullMore()
    {
        var oPopBody = oPopup.document.body;
        oPopBody.style.backgroundColor = "lightyellow";
        oPopBody.style.border = "solid black 1px";
       
        oPopBody.innerHTML = oDialog.innerHTML;
        oPopup.show(100, 100, 50, 400, document.body);
    }

    </script>

</asp:Content>
