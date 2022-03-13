<%@ Page Language="C#" MasterPageFile="~/Control/MasterView.master" AutoEventWireup="true"
    CodeFile="CheckList.aspx.cs" Inherits="CheckList" %>

<%@ MasterType VirtualPath="~/Control/MasterView.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <% 
        string appUrl = Config.AppBaseUrl;
        string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
        string BackEnd = Resources.Labels.BackEnd;
        string FrontEnd = Resources.Labels.FrontEnd;
        string lang = SessionInfo.CurrentLanguage;
    %>
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        
        
              
        <tr>
            <td>
                <PC:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="Large"></PC:Label>
                </td>
            <td width="10px">
               <!-- : -->
            </td>
            <td>
              <!--  - -->
                </td>
             <td>
                 &nbsp;</td>
            <td style="width: 10px;">
              <!--  : -->
            </td>
            <td>
                &nbsp;</td>
            <td>
                
            </td>
            <td align="right">
                <PC:ReportToolBar runat="server" ID="toolbContact" />
            </td>
        </tr>
        <tr>
            <td height="5px" colspan="8">
            </td>
        </tr>
        <!-- search area end -->
        <tr style="display:none;">
            <!--grdView begin -->
            <td class="ddlpaSearch" style="padding-left: 5px">
            </td>
            <td style="padding: 3px; height: 25px;" class="ddlpaSearch" valign="middle" align="right" colspan="7">
                <PC:Label LabelStyle="xLabel" runat="server" ID="lblView" Text="<%$ Resources:Labels,View %>" />
                <PC:DropDownList ID="ddlSearch" runat="server" Width="180px">
                </PC:DropDownList>
            </td>
            <!--grdView End -->
        </tr>
        <!-- data area begin -->
        <tr height="100%">
            <td valign="top" class="GridViewContainer" colspan="8">
                <PC:DbDataSource runat="server" ID="dsGridView">
                </PC:DbDataSource>
                <PC:GridView runat="server" ID="gvData" DataSourceID="dsGridView" SkinID="SecuritySkin"
                     SelectValueField="ID" AllowPaging ="false"
                    BorderWidth="0" GridViewStyle="UserDefine" AllowMultipleSelect="False" />
            </td>
        </tr>
        <!-- data area end -->
        <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td align="right">
                <asp:Button ID="btnClose" runat="server" Text="<%$ Resources:Labels,Close %>" 
                    onclick="btnClose_Click" OnClientClick="javascript:window.close();" />
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
            alert('a');
            document.forms[0].close();
        }
    </script>

</asp:Content>
