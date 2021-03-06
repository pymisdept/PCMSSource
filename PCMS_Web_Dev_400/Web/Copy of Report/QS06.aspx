<%@ Page Language="C#" MasterPageFile="~/Control/MasterReport.master" AutoEventWireup="true"
    CodeFile="QS06.aspx.cs" Inherits="QS06" %>

<%@ MasterType VirtualPath="~/Control/MasterReport.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
   <% 
   
        string appUrl = Config.AppBaseUrl;
        string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
        string BackEnd = Resources.Labels.BackEnd;
        string FrontEnd = Resources.Labels.FrontEnd;
        string lang = SessionInfo.CurrentLanguage;
    %>
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <!-- search area begin -->                
        <tr>
            <td>
                <span style="padding-left: 5px;" />
                <PC:Label runat="server" ID="lblDivision" LabelStyle="xLabel" Text="<%$ Resources:Labels,FileName %>" />
            </td>
            <td width="10px">
                :
            </td>
            <td>
                <PC:TextBox ID="txtFileName" runat="server" CssClass="TextBoxName" SubmitOnEnter="True"  Width="200px" />
            </td>
            <td>
                <span style="padding-left: 5px;" />
                <PC:Label runat="server" ID="lblDepartment" LabelStyle="xLabel" Text="<%$ Resources:Labels,Project %>" />
            </td>
            <td style="width: 10px;">
                :
            </td>
            <td colspan="3">
                <PC:DropDownList runat="server" ID="ddlProject" RegisterClientVariable="true"
                    Width="200px" />
            </td>
        </tr>
        <tr>
            <td height="5px" colspan="8">
            </td>
        </tr>        
        <tr>
            <td>
                <span style="padding-left: 5px;" />
                <PC:Label LabelStyle="xLabel" runat="server" ID="lblSearch" Text="<%$ Resources:Labels,Date %>" />
            </td>
            <td width="10px">
                :
            </td>
            <td>
                <PC:TextBox ID="txtStartDate" runat="server" CssClass="TextBoxName" SubmitOnEnter="True"
                    DataType ="Date" Width="75px" />-
                <PC:TextBox ID="txtEndDate" runat="server" CssClass="TextBoxName" SubmitOnEnter="True"
                    DataType ="Date"  Width="75px" />
            </td>
             <td>
                <span style="padding-left: 5px;" />
                <PC:Label runat="server" ID="Label1" LabelStyle="xLabel" Text="<%$ Resources:Labels,UploadBy %>" />
            </td>
            <td style="width: 10px;">
                :
            </td>
            <td>
                <PC:DropDownList runat="server" ID="ddlUser" RegisterClientVariable="true"
                    Width="200px" />
            </td>
            <td>
                <PC:SearchButton id="SearchButton" runat="server" />
            </td>
            <td align="right">
                <PC:ToolBar runat="server" Visible="false" ID="toolbContact" />
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
    	var backend = "<%=BackEnd %>";
    	var frontend = "<%=FrontEnd %>"; 
    	var lang = "<%=lang %>"; 
    	
    	function DownloadFile(id)
        {
            if (id !="")
            {           
               _RequestStr = UpAllDownoLoadFileUrl+"?UpRecordID="+id;             
               window.open(_RequestStr,"_blank", UpAlldialogFeaturesDownoLoadFile,false); //open a new blank widows,if click open ,open in it                
            }
        }

       function Refresh() { 
            setCommand(COMMAND_REFRESH);       
            frmMain.submit();
        }
        
         function AddNew() {     
            var p = {};
            p[PARAM_URL] = formUrl;
            p[PARAM_MODE] = FORM_MODE_NEW;
            var ret = showForm(p, dialogFeatures);            
            if(ret==COMMAND_REFRESH)
                Refresh();
        }

        function Edit() {
            var row = gvData.getSelectedRow();
            if(row==null) {
                alert(MSG_SELECT_EDIT);
                return;
            }
            var id = gvData.getCellText(row, 1);          
            var p = {};
            p[PARAM_URL] = formUrl;
            p[PARAM_MODE] = FORM_MODE_EDIT;
            p[PARAM_ID] = id;

            var ret = showForm(p, dialogFeatures);
            if(ret==COMMAND_REFRESH) {
                Refresh();
            }
        } 
        function ShowReport(id,DB)
        {
         
            var dialogFeatures = "dialogHeight:580px;dialogWidth:650px;";
            var p = {};
            
           
            //p[PARAM_URL] = "http://localhost/CrystalWeb/ReportFrame.aspx?Report=D:djfjldjaielafWeb%20ProgramsdjfjldjaielafCrystalWebdjfjldjaielafCRYSTALREPORTSdjfjldjaielafPU02_Purchase%20Order.rpt&user=1&project=00235";
            //p[PARAM_URL] = "http://localhost/CrystalWeb/ReportFrame.aspx?Report=D:djfjldjaielafWeb%20ProgramsdjfjldjaielafCrystalWebdjfjldjaielafCRYSTALREPORTSdjfjldjaielafPU02_Purchase%20Order.rpt&user=1&docentry=" + id + "&DB=" + DB;
            if (DB == backend)
            {
                if (lang == "zh-cn")
                {
                    p[PARAM_URL] = "http://192.168.11.107/CPS_Crystal/ReportFrame.aspx?ReportName=PU02_Purchase Order v01_002.rpt&Report=E:CPS1000ProjectCPS1000PaulYCPS1000ProgrammingCPS1000WebCPS1000CPS_CrystalCPS1000CRYSTALREPORTSCPS1000PU02_Purchase Order v01_002.rpt&user=1&projectcode=08019"
                }
                if (lang == "zh-tw")
                {
                    p[PARAM_URL] = "http://192.168.11.107/CPS_Crystal/ReportFrame.aspx?ReportName=PU02_Purchase Order v01_002.rpt&Report=E:CPS1000ProjectCPS1000PaulYCPS1000ProgrammingCPS1000WebCPS1000CPS_CrystalCPS1000CRYSTALREPORTSCPS1000PU02_Purchase Order v01_002.rpt&user=1&projectcode=08019"
                }
                if (lang == "en-us")
                {
                    p[PARAM_URL] = "http://192.168.11.107/CPS_Crystal/ReportFrame.aspx?ReportName=PU02_Purchase Order v01_002.rpt&Report=E:CPS1000ProjectCPS1000PaulYCPS1000ProgrammingCPS1000WebCPS1000CPS_CrystalCPS1000CRYSTALREPORTSCPS1000PU02_Purchase Order v01_002.rpt&user=1&projectcode=08019"
                }
            } else
            {
                if (lang == "zh-cn")
                {
                    p[PARAM_URL] = "http://192.168.11.107/CPS_Crystal/ReportFrame.aspx?ReportName=PU02_Purchase Order v01_002.rpt&Report=E:CPS1000ProjectCPS1000PaulYCPS1000ProgrammingCPS1000WebCPS1000CPS_CrystalCPS1000CRYSTALREPORTSCPS1000PU02_Purchase Order v01_002.rpt&user=1&projectcode=08019"
                }
                if (lang == "zh-tw")
                {
                    p[PARAM_URL] = "http://192.168.11.107/CPS_Crystal/ReportFrame.aspx?ReportName=PU02_Purchase Order v01_002.rpt&Report=E:CPS1000ProjectCPS1000PaulYCPS1000ProgrammingCPS1000WebCPS1000CPS_CrystalCPS1000CRYSTALREPORTSCPS1000PU02_Purchase Order v01_002.rpt&user=1&projectcode=08019"
                }
                if (lang == "en-us")
                {
                    p[PARAM_URL] = "http://192.168.11.107/CPS_Crystal/ReportFrame.aspx?ReportName=PU02_Purchase Order v01_002.rpt&Report=E:CPS1000ProjectCPS1000PaulYCPS1000ProgrammingCPS1000WebCPS1000CPS_CrystalCPS1000CRYSTALREPORTSCPS1000PU02_Purchase Order v01_002.rpt&user=1&projectcode=08019"
                }
            }
            
            p[PARAM_MODE] = FORM_MODE_NEW;
            var ret = showForm(p, dialogFeatures);            
            if(ret==COMMAND_REFRESH)
                Refresh();
                //return true;
        }
         function ShowReport_BE(id)
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
        
//http://192.168.11.103/CrystalWeb/ReportFrame.aspx?Report=D:djfjldjaielafWeb%20ProgramsdjfjldjaielafCrystalWebdjfjldjaielafCRYSTALREPORTSdjfjldjaielafPU02_Purchase%20Order.rpt&user=1&project=00235
        function Delete() {
            var selected; 
            if(gvData.allowMultipleSelect) {
                selected = gvData.getSelectedRowValues();
            }else{                    
                selected = gvData.getSelectedRowValue();
            }
            if(SimpleJS.isNullOrEmpty(selected)) {
                alert(MSG_SELECT_DELETE);
                return;
            }
            if(!confirm(PROMPT_CONFIRM_DELETE)) 
                return;
            setCommand(COMMAND_DELETE);
            frmMain.submit();            
        }        
       
    </script>

</asp:Content>
