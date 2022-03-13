<%@ Page Language="C#" MasterPageFile="~/Control/MasterSecurity.master" AutoEventWireup="true" CodeFile="ScUseFullLink.aspx.cs" Inherits="ScUseFullLink" %>
<%@ MasterType VirtualPath="~/Control/MasterSecurity.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
      <!-- search area begin -->
      <tr style="padding-bottom:5px;">
        <td>
            <PC:Label LabelStyle="xLabel"  runat="server" ID="lblCode" Text="<%$ Resources:Labels,Search %>" /><span style="width:5px;"></span>:
            <PC:TextBox id="txtSearchBox" MaxLength="20" runat="server" CssClass="TextBoxName" SubmitOnEnter="True" /><span style="width:10px;"></span>
            <%--<PC:Label LabelStyle="xLabel"  runat="server" ID="lblName" Text="<%$ Resources:Labels,Name %>" /><span style="width:5px;"></span>:
            <PC:TextBox ID="txtDescription" runat="server" CssClass="TextBoxName" SubmitOnEnter="True" Width="200px"/>--%>
            <PC:SearchButton id="SearchButton" runat="server" />
        </td>
        <td align="right">
            <PC:ToolBar ID="tbToolBar" runat="server" />
        </td>
      </tr>      
      <!-- search area end -->
      <tr>
        <td class="pageline_se" style="padding-left:5px" nowrap width="100%" colspan="2"></td>
      </tr>
      <!-- data area begin -->
      <tr>
        <td valign="top" class="GridViewContainer" colspan="2" height="100%">
            <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">

                    <!-- data area begin -->
                    <tr>
                        <td valign="top" height="100%" class="GridViewContainer">
                            <PC:DbDataSource runat="server" ID="dsGridView" >
                            </PC:DbDataSource>
                            
                            <PC:GridView runat="server" ID="gvData" DataSourceID="dsGridView"  AutoToolTip="false"
                            SelectValueField="ID" AllowMultipleSelect="False" SkinID="SecuritySkin" OnRowCreated="gvData_RowCreated" />
                        </td>                        
                    </tr>
                <!-- data area end -->
            </table>
        </td>
      </tr>
      <!-- data area end -->
      </table>
      
      <script language="javascript">
        var frmMain = document.forms[0];
        
    	var dialogFeatures = "dialogHeight:280px;dialogWidth:710px;";
    	var formUrl = "ScUseFullLink_Form.aspx";

        function Refresh() {        
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
            var id = gvData.getCellText(row, FIELD_ID);          
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
            Refresh();            
        }
        
//        function Export() {
//            setCommand(COMMAND_EXPORT);
//            Refresh();
//        }
      </script>
      
</asp:Content>

