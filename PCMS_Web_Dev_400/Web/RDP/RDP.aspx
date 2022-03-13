<%@ Page Language="C#" MasterPageFile="~/Control/MasterBase.master" AutoEventWireup="true"
    CodeFile="RDP.aspx.cs" Inherits="RDP" %>

<%@ Import Namespace="PCCore" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BaseContentPlaceHolder" runat="Server">
    <% 
        string appUrl = Config.AppBaseUrl;
        string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
        string isLogin = PCCore.SessionInfo.IsLogin.ToString();
        string SAPServer = Config.SAPServer.ToString();
        string UserID = SessionInfo.LoginName.ToString();
        string Password = SessionInfo.UserPassword.ToString();
        
    %>
    
    
    
    
    <script language="javascript">
    var frmMain = document.forms[0];
    var isLogin = "<%=isLogin %>";
    
    if (isLogin == "True")
    {
        RunRDP();
        window.location.href = "../Admin_Default.aspx";
    } else {
        
    }
    
    function RunRDP()
    {
     var screenHeight = window.screen.height;
     var screenWidth = window.screen.width;
     var SAPServer = "<%= SAPServer %>";
     var UserID = "<%= UserID %>";
     var Password = "<%= Password %>";
     
     var dialogFeatures = "dialogHeight:" + screenHeight + "px;dialogWidth:" + screenWidth + "px;";
     var formUrl = "../RDP/B1.htm?Server=" + SAPServer + "&User=" + UserID + "&Password=" + Password;
      var p = {};
     p[PARAM_URL] = formUrl;
     
     p[PARAM_MODE] = FORM_MODE_NEW;
     var ret = showForm(p, dialogFeatures); 
    }
         
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
