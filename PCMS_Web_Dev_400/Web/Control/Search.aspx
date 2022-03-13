<%@ Page Language="C#" MasterPageFile="~/Control/MasterSearch.master"  AutoEventWireup="true"
    CodeFile="Search.aspx.cs" Inherits="_Search" %>

<%@ MasterType VirtualPath="~/Control/MasterSearch.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <%
    string isLogin = PCCore.SessionInfo.IsLogin.ToString().ToLower();
    PCCore.Common.HRLog.RecordLog("isLogin:" + isLogin);
    
%>
    
   <script language="javascript" type="text/javascript">
//        alert("Search");
        var _isLogin = "<%=isLogin %>";
        
        if (_isLogin == "true")
        {
            //window.returnValue = "";
        } else {
            alert("Session timeout. Please re-login.");
            self.close();
            //window.returnValue = "error";
        }
        //alert(window.returnValue);
        
       
    </script>
</asp:Content>
