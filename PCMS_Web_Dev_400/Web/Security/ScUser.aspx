<%@ Page Language="C#" MasterPageFile="~/Control/MasterSecurity.master" AutoEventWireup="true"  CodeFile="ScUser.aspx.cs" Inherits="ScUser" %>
<%@ MasterType VirtualPath="~/Control/MasterSecurity.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <!-- search area begin -->
        <tr style="padding-bottom:5px;">
            <td>                
                <PC:Label LabelStyle="xLabel"  runat="server" ID="lblLoginName" Text="<%$ Resources:Labels,LoginName %>" /><span style="width:5px;"></span>:               
                <PC:DropDownList ID="ddlLoninName" runat="server"  RegisterClientVariable="true"></PC:DropDownList>
                <span style="width:10px;"></span>
                <PC:Label LabelStyle="xLabel"  runat="server" ID="lblFullName" Text="<%$ Resources:Labels,FullName %>" /><span style="width:5px;"></span>:
                <PC:TextBox ID="txtFullName" runat="server" CssClass="TextBoxName" SubmitOnEnter="True" Width="230" />
                <PC:SearchButton ID="SearchButton" runat="server" />
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
            <td valign="top" class="GridViewContainer" colspan="2" height="100%" >
                <PC:DbDataSource runat="server" ID="dsGridView">
                </PC:DbDataSource>
                <PC:GridView runat="server" ID="gvData" DataSourceID="dsGridView" SelectValueField="ID" AutoGenerateColumns="False" ClearSelectedRowValues="True"
                    ClientRowDblClicked="if(typeof(Edit)=='function') Edit();" 
                    HiddenFields="ID,CREATED,CREATEDBY,MODIFIED,MODIFIEDBY,"  OnRowCreated="gvData_RowCreated" SkinID="UserGroupSkin">
                    <Columns>
                        <asp:BoundField DataField="id" SortExpression="id" HeaderText="ID" />
                        <asp:BoundField DataField="fullname" SortExpression="fullname" HeaderText="Full Name" />
                        <asp:BoundField DataField="loginname" SortExpression="loginname" HeaderText="Login Name" />                        
                        <asp:TemplateField HeaderText="Locked" SortExpression="locked">
                        <ItemTemplate>
                        <asp:CheckBox id="chkLocked" runat="server"  Checked='<%# Eval("LOCKED").ToString()=="1" %>'  Enabled="False"></asp:CheckBox>                         
                        </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Supervisor" SortExpression="Supervisor">
                        <ItemTemplate>
                        <asp:CheckBox id="chkSupervisor" runat="server"  Checked='<%# Eval("SUPERVISOR").ToString()=="1" %>'  Enabled="False"></asp:CheckBox> 
                        </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reverse" SortExpression="Reverse" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                        <asp:CheckBox id="chkReverse" runat="server"  Checked='<%# Eval("REVERSE").ToString()=="1" %>'  Enabled="False"></asp:CheckBox>                         
                        </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>                   
                </PC:GridView>
            </td>
        </tr>
        <!-- data area end -->
    </table>

    <script language="javascript">
        var frmMain = document.forms[0];
        
    	var dialogFeatures = "dialogHeight:600px;dialogWidth:700px;";
    	var formUrl = "ScUser_Form.aspx";

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
            var id = gvData.getCellText(row, FIELD_ID);          
            var p = {};
            p[PARAM_URL] = formUrl;
            p[PARAM_MODE] = FORM_MODE_EDIT;
            p[PARAM_ID] = id;

            var ret = showForm(p, dialogFeatures);
            if(ret==COMMAND_REFRESH)
                Refresh();
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
        
        function GetSelectedRow(lnk) { 
            row = lnk;
            
            var elements = document.getElementsByClassName('sgvDataRowSelected');
            while(elements.length > 0){
                elements[0].classList.remove('sgvDataRowSelected');
            }
            
            //document.getElementById(strSelectedID).value = row.cells[1].innerHTML;
            row.classList.add("sgvDataRowSelected");
        }
        
        function GetDoubleClickRow(lnk){
            var row = lnk;
            if(row==null) {
                alert(MSG_SELECT_EDIT);
                return;
            }
            
            var id = row.cells[0].innerHTML;          
            var p = {};
            p[PARAM_URL] = formUrl;
            p[PARAM_MODE] = FORM_MODE_EDIT;
            p[PARAM_ID] = id;

            var ret = showForm(p, dialogFeatures);
            if(ret==COMMAND_REFRESH) {
                Refresh();
            }
        }
    </script>

</asp:Content>
