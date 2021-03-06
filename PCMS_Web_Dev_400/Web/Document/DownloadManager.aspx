<%@ Page Language="C#" MasterPageFile="~/Control/MasterDlnMgr.master" AutoEventWireup="true"
    CodeFile="DownloadManager.aspx.cs" Inherits="DownloadManager" %>

<%@ MasterType VirtualPath="~/Control/MasterDlnMgr.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <% 
        string appUrl = Config.AppBaseUrl;
        string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
        ibProject.ImageUrl = themeUrl + "/images/search.gif";
        string strProjectClientID = ddlProject.ClientID;
        string strProjectTextID = invProjCode.ClientID;
        string actionquery = Consts.ActionQuery;
        
    %>
    <PC:TextBox ID="invProjCode" runat="server" Width="0px" Visible="false"></PC:TextBox>    
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <!-- search area begin -->                
        <tr>
            <td>
                <span style="padding-left: 5px;" />
                <PC:Label runat="server" ID="lblDivision" LabelStyle="xLabel" Text="<%$ Resources:Labels,DocumentName %>" />
            </td>
            <td width="20px">
                :
            </td>
            <td>
                <PC:DropDownList ID="ddlTemplate" runat="server" RegisterClientVariable="true" Width="250px" />
                <PC:TextBox ID="txtFileName" Visible="false" runat="server" CssClass="TextBoxName" SubmitOnEnter="True"  Width="150px" />
            </td>
            <td>
                <span style="padding-left: 5px;" />
                <PC:Label runat="server" ID="lblDepartment" LabelStyle="xLabel" Text="<%$ Resources:Labels,Project %>" />
            </td>
            <td style="width: 10px;">
                :
            </td>
            <td colspan="3">
                <PC:DropDownList runat="server" ID="ddlProject" RegisterClientVariable="true" Width="330px" />
                <PC:ImageButton ID="ibProject" runat="server" ImageUrl="<%=themeUrl %>/images/search.gif" OnClientClick="javascript:SearchProject()" />
            </td>
        </tr>
        <tr>
            <td height="5px" colspan="8">
            </td>
        </tr>        
        <tr>
            <td >
                <span style="padding-left: 5px;" />
                <PC:Label LabelStyle="xLabel" runat="server" ID="lblSearch" Text="<%$ Resources:Labels,Date %>" />
            </td>
            <td width="20px">
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
                <PC:Label runat="server" ID="Label1" Visible="false"  LabelStyle="xLabel" Text="<%$ Resources:Labels,UploadBy %>" />
            </td>
            <td style="width: 10px;">
                
            </td>
            <td>
                <PC:DropDownList runat="server" visible="false" ID="ddlUser" RegisterClientVariable="true"
                    Width="200px" />
                    <asp:CheckBox ID="ckbDownloaded" runat="server" Text="<%$ Resources:Labels,DownloadedFile %>" Checked="false" />
            </td>
            <td>
                <PC:SearchButton id="SearchButton" runat="server" />
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
                     SelectValueField="descid" DataKeyNames="descid"  onselectedindexchanging="gvData_SelectedIndexChanging" SelectedIndex="0" OnRowCommand="gvData_RowCommand"
                    BorderWidth="0" GridViewStyle="UserDefine" AllowMultipleSelect="False"  OnSorted="gvData_Sorted" OnRowDataBound="gvData_RowDataBound" OnSelectedIndexChanged="gvData_SelectedChanged" AllowSorting ="true"  />
            </td>
        </tr>
        <!-- data area end -->
        
    </table>

    <script language="javascript">
        var frmMain = document.forms[0]; 
    	var dialogFeatures = "dialogHeight:80px;dialogWidth:50px;";
    	var formUrl = "PUI01_Form.aspx";
    	var UpAlldialogFeaturesDownoLoadFile = "menubar=no,toolbar=no,status:=no,Height=230,Width=350,top=270,left=320";
    	var UpAllDownoLoadFileUrl = "<%=appUrl %>/Control/DownLoadTemplate.aspx"; 
    	var ViewDocumentUrl = "<%=appUrl %>/Report/ViewData.aspx";
    	
    	function DownloadFile(id,ProjCode,FunctionCode)
        {
            if (id !="")
            {           
                
               _RequestStr = UpAllDownoLoadFileUrl+"?DlnID="+id + "&ProjCode=" + ProjCode + "&FunctionCode=" + FunctionCode ;             
               //window.open(_RequestStr,"_blank", UpAlldialogFeaturesDownoLoadFile,false); //open a new blank widows,if click open ,open in it                
               window.location.href = _RequestStr;
               //Refresh();
            }
        }
        
        function CheckList(id)
        {
            if (id !="")
            {   
                var p = {};
                p[PARAM_URL] = ViewDocumentUrl+"?ID="+id+"&FILETYPE=2003";
                p[PARAM_MODE] = FORM_MODE_NEW;
                var ret = showForm(p, dialogFeatures);            
                if(ret==COMMAND_REFRESH)
                    Refresh();        
               
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
        
       function SearchProject()
	{
	    
	    var p = {};
	    
	    var strProjectClientID = "<%=strProjectClientID %>";
	    var strProjectTextID = "<%=strProjectTextID %>";
	    var actionquery = "<%=actionquery %>";
	    var formUrl = "../Control/Search.aspx?type=project&action=" + actionquery + "&function=" + functionid;
	    
        p[PARAM_URL] = formUrl;
        p[PARAM_MODE] = FORM_MODE_NEW;
        
        var SearchdialogFeatures = "dialogHeight:" + 300+ "px;dialogWidth:" + 800 + "px;";
        var ret = showForm(p, SearchdialogFeatures); 
        var ddl = document.getElementById(strProjectClientID);
        
        
        if (ret != "" && ret != undefined)
        {
              //alert(ret);    
//            for(var i=0;i<=ddl.length-1;i=i+1)
//            {
//                
//                var ddlText=ddl.options[i].outerText;
//                alert(ddlText);
//                if(ddlText==ret)
//                {
//                    ddl.selectedIndex=i;
//                    //ddl.onchange();
//                    break;
//                }
//            } 
            
            document.getElementById(strProjectTextID).value = ret;
            //alert(document.getElementById(strProjectTextID).value);
            //document.getElementById(strProjectClientID).selectedvalue = ret;
            //document.getElementById(strProjectClientID).selectedIndex = 10;
            //frmMain.submit();
            document.forms[0].submit();
        }
	}
       
    </script>

</asp:Content>
