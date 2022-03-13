<%@ Page Language="C#" MasterPageFile="~/Control/MasterCustomize.master" AutoEventWireup="true"
    CodeFile="MAI010201.aspx.cs" Inherits="MAI010201" %>

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
        <tr width="100%">
        <td width="100%">
            <table width="100%">
            <td  align="left" width="40%">
               <PC:Label id="Label4" runat="server" Font-Bold="true" Text="<%$ Resources:Labels,MAI010201_1 %>"></PC:Label>
            </td >
            <td width="5%">
                :
            </td> 
            <td  align="left" width="40%">
                <PC:TextBox ID="txtValue1" width="100%"  runat="server"></PC:TextBox>
            </td>
            <td >
                <PC:Label id="Label5" runat="server" Text=""></PC:Label>
            </td>
            </table>
        
        </td>
        </tr>
        <tr width="100%">
        <td width="100%">
            <table width="100%">
            <td  align="left" width="40%">
               <PC:Label id="Label6" runat="server" Font-Bold="true" Text="<%$ Resources:Labels,MAI010201_2 %>"></PC:Label>
            </td >
            <td width="5%">
                :
            </td>
            <td  align="left" width="40%">
                <PC:TextBox ID="txtValue2" Font-Bold="true" width="100%"  runat="server"></PC:TextBox>
                
            </td>
            <td >
                <PC:Label id="Label7" runat="server" Text=""></PC:Label>
            </td>
            </table>
        
        </td>
        </tr>
        <tr width="100%">
        <td width="100%">
            <table width="100%">
            <td  align="left" width="40%">
               <PC:Label id="Label8" runat="server" Font-Bold="true" Text="<%$ Resources:Labels,MAI010201_3 %>"></PC:Label>
            </td >
            <td width="5%">
                :
            </td>
            <td  align="left" width="40%">
                <PC:TextBox ID="txtValue3" width="100%" runat="server"></PC:TextBox>
                
            </td>
            <td >
                <PC:Label id="Label9" runat="server" Text=""></PC:Label>
            </td>
            </table>
        
        </td>
        </tr>
        <tr width="100%">
        
        <tr width="100%">
        <td width="100%">
            <table width="100%">
            <td  align="left" width="40%">
               <PC:Label id="Label12" runat="server" Font-Bold="true" Text="<%$ Resources:Labels,MAI010201_4 %>"></PC:Label>
            </td >
            <td width="5%">
                :
            </td>
            <td  align="left" width="40%">
                <PC:TextBox ID="txtValue4" width="100%" runat="server"></PC:TextBox>
                
            </td>
            <td >
                <PC:Label id="Label13" runat="server" Text=""></PC:Label>
            </td>
            </table>
        
        </td>
        </tr>
        <tr width="100%">
        <td width="100%">
            <table width="100%">
            <td  align="left" width="40%">
               <PC:Label id="Label14" runat="server" Font-Bold="true" Text="<%$ Resources:Labels,MAI010201_5 %>"></PC:Label>
            </td >
            <td width="5%">
                :
            </td>
            <td  align="left" width="40%">
                <PC:TextBox ID="txtValue5" width="100%" runat="server"></PC:TextBox>
                
            </td>
            <td >
                <PC:Label id="Label15" runat="server" Text=""></PC:Label>
            </td>
            </table>
        
        </td>
        </tr>
        <tr width="100%">
        <td width="100%">
            <table width="100%">
            <td  align="left" width="40%">
               <PC:Label id="Label16" runat="server" Font-Bold="true" Text="<%$ Resources:Labels,MAI010201_6 %>"></PC:Label>
            </td >
            <td width="5%">
                :
            </td>
            <td  align="left" width="40%">
                <PC:TextBox ID="txtValue6" width="100%"  runat="server"></PC:TextBox>
                
            </td>
            <td >
                <PC:Label id="Label17" runat="server" Text=""></PC:Label>
            </td>
            </table>
        
        </td>
        </tr>
        <tr width="100%">
        <td width="100%">
            <table width="100%">
            <td  align="left" width="40%">
               <PC:Label id="Label18" runat="server" Font-Bold="true" Text="<%$ Resources:Labels,MAI010201_7 %>"></PC:Label>
            </td >
            <td width="5%">
                :
            </td>
            <td  align="left" width="40%">
                <PC:TextBox ID="txtValue7" width="90%" DataType="Date"  runat="server"></PC:TextBox>
                
            </td>
            <td >
                <PC:Label id="Label19" runat="server" Text=""></PC:Label>
            </td>
            </table>
        
        </td>
        </tr>
        <tr width="100%">
        <td width="100%">
            <table width="100%">
            <td  align="left" width="40%">
               <PC:Label id="Label20" runat="server" Font-Bold="true" Text="<%$ Resources:Labels,MAI010201_8 %>"></PC:Label>
            </td >
            <td width="5%">
                :
            </td>
            <td  align="left" width="40%">
                <PC:TextBox ID="txtValue8" width="90%"  DataType="Date" runat="server"></PC:TextBox>
                
            </td>
            <td >
                <PC:Label id="Label21" runat="server" Text=""></PC:Label>
            </td>
            </table>
        
        </td>
        </tr>
        <tr width="100%">
        <td width="100%">
            <table width="100%">
            <td  align="left" width="40%">
               <PC:Label id="Label22" runat="server" Font-Bold="true" Text="<%$ Resources:Labels,MAI010201_9 %>"></PC:Label>
            </td >
            <td width="5%">
                :
            </td>
            <td  align="left" width="40%">
                <PC:TextBox ID="txtValue9" width="100%"  DataType="Integer" runat="server"></PC:TextBox>
                
            </td>
            <td >
                <PC:Label id="Label23" runat="server" Text=""></PC:Label>
            </td>
            </table>
        
        </td>
        </tr>
        <tr width="100%">
        <td width="100%">
            <table width="100%">
           <td  align="left" width="40%">
               <PC:Label id="Label24" runat="server" Font-Bold="true" Text="<%$ Resources:Labels,MAI010201_10 %>"></PC:Label>
            </td >
            <td width="5%">
                :
            </td>
            <td  align="left" width="40%">
                <PC:TextBox ID="txtValue10" width="100%" DataType="Integer"  runat="server"></PC:TextBox>
                
            </td>
            <td >
                <PC:Label id="Label25" runat="server" Text=""></PC:Label>
            </td>
            </table>
        
        </td>
        </tr>
        <tr width="100%">
        <td width="100%">
            <table width="100%">
            <td  align="left" width="40%">
               <PC:Label id="Label26" runat="server" Font-Bold="true" Text="<%$ Resources:Labels,MAI010201_11 %>"></PC:Label>
            </td >
            <td width="5%">
                :
            </td>
            <td  align="left" width="40%">
                <PC:TextBox ID="txtValue11" width="100%" Font-Bold="true" DataType="Integer"  runat="server"></PC:TextBox>
                
            </td>
            <td >
                <PC:Label id="Label27" runat="server" Text=""></PC:Label>
            </td>
            </table>
        
        </td>
        </tr>
        <tr width="100%">
        <td width="100%">
            <table width="100%">
            <td  align="left" width="40%">
               <PC:Label id="Label28" runat="server" Font-Bold="true" Text="<%$ Resources:Labels,MAI010201_12 %>"></PC:Label>
            </td >
            <td width="5%">
                :
            </td>
            <td  align="left" width="40%">
                <PC:TextBox ID="txtValue12" width="90%" DataType="Date"  runat="server"></PC:TextBox>
                
            </td>
            <td >
                <PC:Label id="Label29" runat="server" Text=""></PC:Label>
            </td>
            </table>
        
        </td>
        </tr>
        <tr width="100%">
        <td width="100%">
            <table width="100%">
            <td  align="left" width="40%">
               <PC:Label id="Label30" runat="server" Font-Bold="true" Text="<%$ Resources:Labels,MAI010201_13 %>"></PC:Label>
            </td >
            <td width="5%">
                :
            </td>
            <td  align="left" width="40%">
                <PC:TextBox ID="txtValue13" width="100%" DataType="Integer" Enabled="false" runat="server"></PC:TextBox>
                
            </td>
            <td >
                <PC:Label id="Label31" runat="server" Text=""></PC:Label>
            </td>
            </table>
        
        </td>
        </tr>
        <tr width="100%">
        <td width="100%">
            <table width="100%">
            <td  align="left" width="40%">
               <PC:Label id="Label32" runat="server" Font-Bold="true" Text="<%$ Resources:Labels,MAI010201_14 %>"></PC:Label>
            </td >
            <td width="5%">
                :
            </td>
            <td  align="left" width="40%">
                <PC:TextBox ID="txtValue14" width="100%" DataType="Currency"   runat="server"></PC:TextBox>
                
            </td>
            <td >
                <PC:Label id="Label33" runat="server" Text=""></PC:Label>
            </td>
            </table>
        
        </td>
        </tr>
        <td width="100%">
            <table width="100%">
            <td  align="left" width="40%">
               <PC:Label id="Label10" runat="server" Font-Bold="true" Text="<%$ Resources:Labels,MAI010201_15 %>"></PC:Label>
            </td >
            <td width="5%">
                :
            </td>
            <td  align="left" width="40%">
                <PC:TextBox ID="txtValue15" width="100%" DataType="Integer" Enabled="false" runat="server"></PC:TextBox>
                
            </td>
            <td >
                <PC:Label id="Label11" runat="server" Text=""></PC:Label>
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
//        var frmMain = document.forms[0]; 
//    	var dialogFeatures = "dialogHeight:580px;dialogWidth:650px;";
//    	var formUrl = "PUI02_Form.aspx";
//    	var UpAlldialogFeaturesDownoLoadFile = "menubar=no,toolbar=no,status:=no,Height=230,Width=350,top=270,left=320";
//    	var UpAllDownoLoadFileUrl = "<%=appUrl %>/Control/DownLoadFile.aspx"; 
//    	var ViewDocumentUrl = "<%=appUrl %>/Report/ViewData.aspx";
//    	
//    	function DownloadFile(id)
//        {
//            if (id !="")
//            {           
//               _RequestStr = UpAllDownoLoadFileUrl+"?UpRecordID="+id;             
//               window.open(_RequestStr,"_blank", UpAlldialogFeaturesDownoLoadFile,false); //open a new blank widows,if click open ,open in it                
//            }
//        }
        
//        function ViewErrorMessage(id)
//        {
//            if (id !="")
//            {   
//                var p = {};
//                p[PARAM_URL] = ViewDocumentUrl+"?ID="+id;;
//                p[PARAM_MODE] = FORM_MODE_NEW;
//                var ret = showForm(p, dialogFeatures);            
//                if(ret==COMMAND_REFRESH)
//                    Refresh();        
//               
//            }
//        }
        

//       function Refresh() { 
//            setCommand(COMMAND_REFRESH);       
//            frmMain.submit();
//        }
        
//         function AddNew() {     
//            var p = {};
//            p[PARAM_URL] = formUrl;
//            p[PARAM_MODE] = FORM_MODE_NEW;
//            var ret = showForm(p, dialogFeatures);            
//            if(ret==COMMAND_REFRESH)
//                Refresh();
//        }
        
        
        
//        function Confirm() {
//            var selected;
//             selected = gvData.getSelectedRowValue();
//             if(!SimpleJS.isNullOrEmpty(selected)) {
//               if (!confirm("Are you sure to be confirm this record?"))
//                return;
//             
//                setCommand(COMMAND_CONFIRM);
//                frmMain.submit();            
//            }
//        }
//        function Edit() {
//            var row = gvData.getSelectedRow();
//            if(row==null) {
//                alert(MSG_SELECT_EDIT);
//                return;
//            }
//            var id = gvData.getCellText(row, 1);          
//            var p = {};
//            p[PARAM_URL] = formUrl;
//            p[PARAM_MODE] = FORM_MODE_EDIT;
//            p[PARAM_ID] = id;

//            var ret = showForm(p, dialogFeatures);
//            if(ret==COMMAND_REFRESH) {
//                Refresh();
//            }
//        } 

//        function Delete() {
//            var selected; 
//            if(gvData.allowMultipleSelect) {
//                selected = gvData.getSelectedRowValues();
//            }else{                    
//                selected = gvData.getSelectedRowValue();
//            }
//            if(SimpleJS.isNullOrEmpty(selected)) {
//                alert(MSG_SELECT_DELETE);
//                return;
//            }
//            if(!confirm(PROMPT_CONFIRM_DELETE)) 
//                return;
//            setCommand(COMMAND_DELETE);
//            frmMain.submit();            
//        }     
        /* Save PM's Report Value to Draft*/
        function Save()
        {
            
            //var r = MAI01.Save();
            
        }        
       
       
    </script>

</asp:Content>
