<%@ Page Language="C#" MasterPageFile="~/Control/MasterCustomize.master" AutoEventWireup="true"
    CodeFile="MAI010203.aspx.cs" Inherits="MAI010203" %>

<%@ MasterType VirtualPath="~/Control/MasterCustomize.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <% 
        string appUrl = Config.AppBaseUrl;
        string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
        
    %>
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <!-- search area begin -->                
        <tr>
            <td width="20%">
            &nbsp;
            </td>
            <td width = "40%" align="center">
                <PC:PmRptToolBar runat="server" ID="toolbContact" />
            </td>
            <td width="40%">
            &nbsp;
            </td>
        </tr>
        <tr>
        <td width="100%">
            <table width="100%">
            <tr>
            <td  align="left" width="10%">
               <PC:Label id="Label6" Font-Bold="true" runat="server" Text="<%$ Resources:Labels,Project %>"></PC:Label>
            </td >
            <td  align="left" width="5%">
               <PC:Label id="Label_1"  Font-Bold="true" runat="server" Text=":"></PC:Label>
            </td >
            <td width="25%">
                <PC:Label ID="txtProject" Font-Bold="true" runat="server" Text=""></PC:Label>
            </td>
            <td width="45%">
                <PC:Label ID="txtProjName" Font-Bold="true" runat="server" Text=""></PC:Label>
            </td>
            </tr>
            </table>
        </td>
        </tr>
        <tr>
        <td width="100%">
            <table width="100%">
            <td  align="left">
               <PC:Label id="Label2" runat="server" Text="<%$ Resources:Labels,Period %>"></PC:Label>
            </td >
            <td >
                <PC:TextBox ID="txtperiodfrom" Enabled="false" DataType ="Date" runat="server"></PC:TextBox>
            </td>
            <td  align="left">
                <PC:Label id="Label3" runat="server" Text="<%$ Resources:Labels,To %>"></PC:Label>
            </td>
            <td >
                <PC:TextBox ID="txtperiodto" Enabled="false" DataType ="Date" runat="server"></PC:TextBox>
            </td>
            </table>
        </td>
        </tr>
        <tr>
        <td width="100%">
            <PC:Label ID="Label01" Font-Bold="true"  Text="<%$ Resources:Labels,MAI010203_1 %>" runat="server"></PC:Label>
        </td>
        </tr>
        <tr>
        <td width="100%">
            <PC:TextBox ID="txtValue1"  Enabled="true" TextMode="MultiLine" Rows="3" Width="100%" runat="server"></PC:TextBox>
        </td>
        </tr>
        <tr>
        <td width="100%">
            <PC:Label ID="Label4" Font-Bold="true"  Text="<%$ Resources:Labels,MAI010203_2 %>" runat="server"></PC:Label>
        </td>
        </tr>
        <tr>
        <td width="100%">
            <PC:TextBox ID="txtValue2" Font-Bold="true" Enabled="true" TextMode="MultiLine" Rows="3" Width="100%" runat="server"></PC:TextBox>
        </td>
        </tr>
        <tr>
        <td width="100%">
            <PC:Label ID="Label5"  Font-Bold="true" Text="<%$ Resources:Labels,MAI010203_3 %>" runat="server"></PC:Label>
        </td>
        </tr>
        <tr>
        <td width="100%">
            <PC:TextBox ID="txtValue3" Enabled="true" TextMode="MultiLine" Rows="3" Width="100%" runat="server"></PC:TextBox>
        </td>
        </tr>
        <tr>
            <td height="5px" colspan="10">
            </td>
        </tr>
        <!-- search area end -->
        <tr>
        <td width="20%">
        &nbsp;
        </td>
        <td width="40%" align="center">
        
        <!-- First Title Button!-->
        <PC:ImageButton ID="btnFirst" runat="server" 
                ImageUrl="<%=themeUrl%>/images/arrow_first_black.gif" 
                onclick="btnFirst_Click" />
        
        <!-- Prev Title Button!-->
        <PC:ImageButton ID="btnPrev" runat="server" CausesValidation="false" 
                ImageUrl="<%=themeUrl%>/images/arrow_left.gif" onclick="btnPrev_Click" />
        
        <!-- Next Title Button!-->
        <PC:ImageButton ID="btnNext" runat="server" CausesValidation="false" 
                ImageUrl="<%=themeUrl%>/images/arrow_right.gif" onclick="btnNext_Click" />
        
        <!-- Last Title Button!-->
        <PC:ImageButton ID="btnLast" runat="server" CausesValidation="false" 
                ImageUrl="<%=themeUrl%>/images/arrow_last_black.gif" onclick="btnLast_Click" />
        </td>
        <td width="40%">
        &nbsp;
        </td>
        </tr>
    </table>

    <script language="javascript">
    </script>

</asp:Content>
