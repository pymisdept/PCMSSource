<%@ Page Language="C#" MasterPageFile="~/Control/MasterCustomize.master" AutoEventWireup="true"
    CodeFile="MAI010204.aspx.cs" Inherits="MAI010204" %>

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
               <PC:Label id="Label1" Font-Bold="true" runat="server" Text="<%$ Resources:Labels,Project %>"></PC:Label>
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
               <PC:Label id="Label2" runat="server" Font-Bold="true"  Text="<%$ Resources:Labels,Period %>"></PC:Label>
            </td >
            <td >
                <PC:TextBox ID="txtperiodfrom" Enabled="false" DataType ="Date" runat="server"></PC:TextBox>
            </td>
            <td  align="left">
                <PC:Label id="Label3" runat="server" Font-Bold="true" Text="<%$ Resources:Labels,To %>"></PC:Label>
            </td>
            <td >
                <PC:TextBox ID="txtperiodto" Enabled="false" DataType ="Date" runat="server"></PC:TextBox>
            </td>
            </table>
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
        
        </table>
        <table width="100%">
        <tr>
        <td width="100%">
            <table width="100%" border="1">
                <tr style="background-color:Green">
                  <td width="20%">
                    
                  </td>
                  <td width="14%">
                    <PC:Label ID="Label10" runat="server"  Font-Bold="true"  Text="<%$ Resources:Labels,MAI010204_9%>"></PC:Label>
                  </td>
                  <td width="14%">
                    
                  </td>
                  <td width="14%">
                    
                  </td>
                  <td width="14%">
                    <PC:Label ID="Label13" runat="server"  Font-Bold="true"  Text="<%$ Resources:Labels,MAI010204_10%>"></PC:Label>
                  </td>
                  <td width="14%">
                    
                  </td>
                  <td width="14%">
                    
                  </td>
                </tr>
                <tr style="background-color:Green">
                  <td width="20%">
                    
                  </td>
                  <td width="14%">
                    <PC:Label ID="Label5" runat="server"  Font-Bold="true"  Text="<%$ Resources:Labels,MAI010204_6%>"></PC:Label>
                  </td>
                  <td width="14%">
                    <PC:Label ID="Label6" runat="server"  Font-Bold="true"  Text="<%$ Resources:Labels,MAI010204_7%>"></PC:Label>
                  </td>
                  <td width="14%">
                    <PC:Label ID="Label7" runat="server"  Font-Bold="true"  Text="<%$ Resources:Labels,MAI010204_8%>"></PC:Label>
                  </td>
                  <td width="14%">
                    <PC:Label ID="Label8" runat="server"  Font-Bold="true"  Text="<%$ Resources:Labels,MAI010204_6%>"></PC:Label>
                  </td>
                  <td width="14%">
                    <PC:Label ID="Label9" runat="server"  Font-Bold="true"  Text="<%$ Resources:Labels,MAI010204_7%>"></PC:Label>
                  </td>
                  <td width="14%">
                    <PC:Label ID="Label4" runat="server"  Font-Bold="true"  Text="<%$ Resources:Labels,MAI010204_8%>"></PC:Label>
                  </td>
                </tr>
                <tr>
                  <td width="20%">
                    <PC:Label ID="lbl1" runat="server"  Font-Bold="true"  Text="<%$ Resources:Labels,MAI010204_1%>"></PC:Label>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtFOrgin_1"  runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtFVisied_1" runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtFVariance_1" runat="server" Width="80%" Enabled="false" DataType="Integer"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtSOrgin_1" runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtSVisied_1" runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtSVariance_1" runat="server" Width="80%" Enabled="false" DataType="Integer"></PC:TextBox>
                  </td>
                </tr>
		        <tr>
                  <td width="20%">
                    <PC:Label ID="lbl2" runat="server"  Font-Bold="true"  Text="<%$ Resources:Labels,MAI010204_2%>"></PC:Label>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtFOrgin_2" runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtFVisied_2" runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtFVariance_2" runat="server" Width="80%" Enabled="false" DataType="Integer"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtSOrgin_2" runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtSVisied_2" runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtSVariance_2" runat="server" Width="80%" Enabled="false" DataType="Integer"></PC:TextBox>
                  </td>
                </tr>
		        <tr>
                  <td width="20%">
                    <PC:Label ID="lbl3" runat="server"  Font-Bold="true"  Text="<%$ Resources:Labels,MAI010204_3%>"></PC:Label>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtFOrgin_3" runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtFVisied_3" runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtFVariance_3" runat="server" Width="80%" Enabled="false" DataType="Integer"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtSOrgin_3" runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtSVisied_3" runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtSVariance_3" runat="server" Width="80%" Enabled="false" DataType="Integer"></PC:TextBox>
                  </td>
                </tr>
		        <tr>
                  <td width="20%">
                    <PC:Label ID="lbl4" runat="server"  Font-Bold="true"  Text="<%$ Resources:Labels,MAI010204_4%>"></PC:Label>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtFOrgin_4" runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtFVisied_4" runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtFVariance_4" runat="server" Width="80%" Enabled="false" DataType="Integer"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtSOrgin_4" runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtSVisied_4" runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtSVariance_4" runat="server" Width="80%" Enabled="false" DataType="Integer"></PC:TextBox>
                  </td>
                </tr>
		        <tr>
                  <td width="20%">
                    <PC:Label ID="lbl5"  Font-Bold="true" runat="server" Text="<%$ Resources:Labels,MAI010204_5%>"></PC:Label>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtFOrgin_5" runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtFVisied_5" runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtFVariance_5" runat="server" Width="80%" Enabled="false" DataType="Integer"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtSOrgin_5" runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtSVisied_5" runat="server" Width="80%" Enabled="true" DataType="Date"></PC:TextBox>
                  </td>
                  <td width="14%">
                    <PC:TextBox ID="txtSVariance_5" runat="server" Width="80%" Enabled="false" DataType="Integer"></PC:TextBox>
                  </td>
                </tr>
                
            </table>
        </td>
        </tr>
        <tr>
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
        function Variance(dt1,dt2,v)
        {
            //Date d1 = new Date(document.getElementById(dt1).value
        }
       
       
    </script>

</asp:Content>
