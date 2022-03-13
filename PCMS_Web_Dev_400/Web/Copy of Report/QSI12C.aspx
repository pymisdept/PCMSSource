<%@ Page Language="C#" MasterPageFile="~/Control/MasterReport.master" AutoEventWireup="true"
    CodeFile="QSI12C.aspx.cs" Inherits="QSI12C" %>

<%@ MasterType VirtualPath="~/Control/MasterReport.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 
   <% 
   
        string appUrl = Config.AppBaseUrl;
        string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
        string lang = SessionInfo.CurrentLanguage;
    %>
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <!-- search area begin -->                
        
        
        <!-- data area begin -->
        <tr height="100%">
            <td valign="top" class="GridViewContainer" colspan="8">
                <PC:DbDataSource runat="server" ID="dsGridView">
                </PC:DbDataSource>
                <PC:GridView runat="server" ID="gvData" DataSourceID="dsGridView" SkinID="SecuritySkin"
                     SelectValueField="ID"
                    BorderWidth="0" GridViewStyle="UserDefine" AllowMultipleSelect="False" OnRowDataBound="gvData_RowDataBound" />
            </td>
        </tr>
        <!-- data area end -->
    </table>

    <script language="javascript">
        var frmMain = document.forms[0]; 
    	var dialogFeatures = "dialogHeight:580px;dialogWidth:650px;";
    	//karrson: change url
    	// var formUrl = "ProjectItemSetup_Form.aspx";
    	var formUrl = "PurchaseOrder_Form.aspx";
    	var UpAlldialogFeaturesDownoLoadFile = "status:=no,Height=230,Width=350,top=270,left=320";
    	var UpAllDownoLoadFileUrl = "<%=appUrl %>/Control/DownLoadFile.aspx"; 
    	var lang = "<%=lang %>";
    	function DownloadFile(id)
        {
            
        }

       function Refresh() { 
            setCommand(COMMAND_REFRESH);       
            frmMain.submit();
        }
        
         function AddNew() {     
            
        }

        function Edit() {
            
        } 
        function ShowReport(sheetname)
        {
         
            var dialogFeatures = "dialogHeight:580px;dialogWidth:650px;";
            var p = {};
            
           
            //p[PARAM_URL] = "http://localhost/CrystalWeb/ReportFrame.aspx?Report=D:djfjldjaielafWeb%20ProgramsdjfjldjaielafCrystalWebdjfjldjaielafCRYSTALREPORTSdjfjldjaielafPU02_Purchase%20Order.rpt&user=1&project=00235";
            //p[PARAM_URL] = "http://localhost/CrystalWeb/ReportFrame.aspx?Report=D:djfjldjaielafWeb%20ProgramsdjfjldjaielafCrystalWebdjfjldjaielafCRYSTALREPORTSdjfjldjaielafPU02_Purchase%20Order.rpt&user=1&docentry=" + id + "&DB=" + DB;
            p[PARAM_URL] = "http://192.168.11.107/CPS_Crystal/ReportFrame.aspx?ReportName=PU02_Purchase Order v01_002.rpt&Report=E:CPS1000ProjectCPS1000PaulYCPS1000ProgrammingCPS1000WebCPS1000CPS_CrystalCPS1000CRYSTALREPORTSCPS1000PU02_Purchase Order v01_002.rpt&user=1&projectcode=08019"
            p[PARAM_MODE] = FORM_MODE_NEW;
            var ret = showForm(p, dialogFeatures);            
            if(ret==COMMAND_REFRESH)
                Refresh();
                //return true;
        }

        function Delete() {
        
        }        
       
    </script>

</asp:Content>
