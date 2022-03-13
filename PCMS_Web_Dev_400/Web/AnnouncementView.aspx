<%@ Page Language="C#" MasterPageFile="~/Control/MasterView.master" AutoEventWireup="true"
    CodeFile="AnnouncementView.aspx.cs" Inherits="AnnouncementView" %>

<%@ MasterType VirtualPath="~/Control/MasterView.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <% 
        string appUrl = Config.AppBaseUrl;
        string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
        string BackEnd = Resources.Labels.BackEnd;
        string FrontEnd = Resources.Labels.FrontEnd;
        string lang = SessionInfo.CurrentLanguage;
    %>
    <table width="100%" border="1" cellpadding="0" cellspacing="0">
        <tr>
            <td width="30%">
                <% // Title        %>
                  
                <PC:Label ID="Label1" runat="server" Text="<%$ Resources:Labels,Title  %>" Font-Bold="true"></PC:Label>
            </td>
            
            <td>
                <PC:TextBox ID="txtTitle" Width="100%" runat="server" Enabled="false"></PC:TextBox>
            </td>
        </tr>
        <tr>
            <td width="30%">
                <% // Sender        %>
                  
                <PC:Label ID="Label2" runat="server" Text="<%$ Resources:Labels,Sender  %>" Font-Bold="true"></PC:Label>
            </td>
            
            <td>
                <PC:TextBox ID="txtSender" Width="100%" runat="server" Enabled="false"></PC:TextBox>
            </td>
        </tr>
        <tr>
            <td width="30%">
                <% // Project        %>
                  
                <PC:Label ID="Label3" runat="server" Text="<%$ Resources:Labels,Project  %>" Font-Bold="true"></PC:Label>
            </td>
            
            <td>
                <PC:TextBox ID="txtProject" Width="100%" runat="server" Enabled="false"></PC:TextBox>
            </td>
        </tr>
        <tr>
            <td width="30%">
                <% // Body        %>
                  
                <PC:Label ID="Label4" runat="server" Text="<%$ Resources:Labels,Body  %>" Font-Bold="true"></PC:Label>
            </td>
            
            <td>
                <asp:TextBox ID="txtbody" TextMode="MultiLine" runat="server" Width="100%" Rows="5" Enabled="false"></asp:TextBox>
                
                
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnClose" runat="server" Text="<%$ Resources:Labels, Close %>" OnClientClick="javascript:window.close();" />
            </td>
        </tr>
        
    </table>

    <script language="javascript">
        var frmMain = document.forms[0]; 
    	var dialogFeatures = "dialogHeight:580px;dialogWidth:650px;";
    	
       function Refresh() { 
            setCommand(COMMAND_REFRESH);       
            frmMain.submit();
        }
        
        function CloseWindow()
        {
            
            document.forms[0].close();
        }
    </script>

</asp:Content>
