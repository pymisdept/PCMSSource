<%@ Page Language="C#" MasterPageFile="~/Control/MasterSecurity.master" AutoEventWireup="true"
    CodeFile="PUI99.aspx.cs" Inherits="PUI99" %>

<%@ MasterType VirtualPath="~/Control/MasterSecurity.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <% 
        string appUrl = Config.AppBaseUrl;
        string themeUrl = Config.GetThemeBaseUrl(Page.Theme); 
        
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
                <PC:ToolBar runat="server" ID="toolbContact" />
            </td>
        </tr>
        <tr>
            <td height="5px" colspan="8">
            </td>
        </tr>
        <!-- search area end -->
        <!-- Karrson: Confirmation Button -->
        <tr height="100%">
            <td valign="top" align="left" class="Confirm" colspan="8">
                <asp:Button ID="btnConfirm" Text="<%$Resources:Labels,Confirmation %>" 
                    runat="server" OnClientClick="javascript:if(!Confirm()) return false;" onclick="btnConfirm_Click" />
            </td>
        </tr>
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
                    BorderWidth="0" GridViewStyle="UserDefine" AllowMultipleSelect="False"  OnSorted="gvData_Sorted" OnRowDataBound="gvData_RowDataBound" OnSelectedIndexChanged="gvData_SelectedChanged" />
            </td>
        </tr>
        <!-- data area end -->
        
    </table>

    <script language="javascript">
        var frmMain = document.forms[0]; 
    	var dialogFeatures = "dialogHeight:580px;dialogWidth:650px;";
    	var formUrl = "PUI99_Form.aspx";
    	var UpAlldialogFeaturesDownoLoadFile = "menubar=no,toolbar=no,status:=no,Height=230,Width=350,top=270,left=320";
    	var UpAllDownoLoadFileUrl = "<%=appUrl %>/Control/DownLoadFile.aspx"; 
    	var ViewDocumentUrl = "<%=appUrl %>/Report/ViewData.aspx";
    	
    	function DownloadFile(id)
        {
            if (id !="")
            {           
               _RequestStr = UpAllDownoLoadFileUrl+"?UpRecordID="+id;             
               window.open(_RequestStr,"_blank", UpAlldialogFeaturesDownoLoadFile,false); //open a new blank widows,if click open ,open in it                
            }
        }
        
        function CheckList(id)
        {
            if (id !="")
            {   
                var p = {};
                p[PARAM_URL] = ViewDocumentUrl+"?ID="+id+"&FILETYPE=2099";
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
        
         function AddNew_OLD() {     
            var p = {};
            p[PARAM_URL] = formUrl;
            p[PARAM_MODE] = FORM_MODE_NEW;
            var ret = showForm(p, dialogFeatures);            
            if(ret==COMMAND_REFRESH)
                Refresh();
        }
        
        
        
        function Confirm() {
            var selected;
            var status;
            var row = gvData.getSelectedRow();
            status = gvData.getCellText(row, 11);
            selected = gvData.getSelectedRowValue();
            if(!SimpleJS.isNullOrEmpty(selected)) {
                if (status == "UA") {
                    if (!confirm("Are you sure to be confirm this record?"))
                    {
                       
                        return;
                        
                    } else {
                         setCommand(COMMAND_CONFIRM);
                            frmMain.submit(); 
                    }
                }
                           
            }
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
        
       
       
    </script>

</asp:Content>
