<%@ Page Language="C#" MasterPageFile="~/Control/MasterSecurityForm.master" AutoEventWireup="true"
    CodeFile="ScGroup_Form.aspx.cs" Inherits="ScGroup_Form" %>

<%@ MasterType VirtualPath="~/Control/MasterSecurityForm.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table border="0" cellspacing="0" cellpadding="3" width="100%" height="330">
        <tr>
            <td width="15%">
                <PC:Label LabelStyle="xLabel" runat="server" ID="lblCode" Text="<%$ Resources:Labels,Code %>" /></td>
            <td>
                :</td>
            <td width="85%">
            
                <PC:TextBox ID="txtCode" MaxLength="20" runat="server" CssClass="TextBoxForm" Required="True"
                    RequiredErrorMessage="<%$ Resources:Messages,InputCode %>" Text='<%# _dr["CODE"] %>' />
            </td>
        </tr>
        <tr>
            <td>
                <PC:Label LabelStyle="xLabel" runat="server" ID="lblName" Text="<%$ Resources:Labels,Name %>" /></td>
            <td>
                :</td>
            <td>            
                <PC:TextBox ID="txtDescription" runat="server" CssClass="TextBoxForm" Required="True"
                    RequiredErrorMessage="<%$ Resources:Messages,InputDescription %>" Text='<%# _dr["NAME"] %>' />
            </td>
        </tr>
        <tr style="display:none">
            <td>
                <PC:Label LabelStyle="xLabel" runat="server" ID="Label2" Text="<%$ Resources:Labels,Project %>" /></td>
            <td>
                :</td>
            <td height="50">
                <table cellpadding="0" cellspacing="0" border="0" width="100%" height="100%" style="border: solid 1 #999999;
                    background-color: White;">
                    <tr style="display:none">
                        <td bgcolor="#dddddd">
                            <span style="width: 3;"></span>
                            <PC:HrCheckBox ID="cbTotal" runat="server" Visible="false" RegisterClientVariable="true" />
                            <PC:Label ID="lblall" runat="server" Text="<%$ Resources:Labels,All %>" /></td>
                    </tr>
                    <tr style="display:none">
                        <td style="background-color: #999999; height: 1;">
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td>
                            <div style="overflow: auto; height: 150;" class="DivCheckBox_se">
                                <asp:CheckBoxList runat="server" ID="cblProject" Enabled="false">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <PC:Label LabelStyle="xLabel" runat="server" ID="lblUsers" Text="<%$ Resources:Labels,Users %>" /></td>
        </tr>
        <tr>
            <td class="sgvGrayHeaderRow" style="padding-left: 10px" width="100%" colspan="3">
            </td>
        </tr>
        <tr height="600px">
            <td valign="top" class="GridViewContainer" colspan="3" height="100%">
                <PC:DbDataSource runat="server" ID="dsGridView" AutoRestoreSelectCommand="false">
                </PC:DbDataSource>
                <PC:GridView runat="server" ID="gvData" DataSourceID="dsGridView" ShowPageIndex="false"
                    SelectValueField="ID" AllowMultipleSelect="True" MultipleSelectDataSourceField="INGROUP"
                    Width="100%" OnRowCreated="gvData_RowCreated" SkinID="NoPageDownSkin" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <PC:TextBox runat="server" CssClass="Invisible" ID="txtCount" RegisterClientVariable="true"></PC:TextBox>
                <PC:TextBox runat="server" CssClass="Invisible" ID="txtTotal" RegisterClientVariable="true"></PC:TextBox>
                <PC:TextBox runat="server" CssClass="Invisible" ID="txtProjectname" RegisterClientVariable="true"></PC:TextBox>
                <PC:TextBox ID="txtID" runat="server" CssClass="Invisible" Text='<%# _dr["ID"] %>' />
            </td>
        </tr>
    </table>
     <script language="javascript">                  
        var count = 0;
        function checkstatus(crl) {
            if (!SimpleJS.isNullOrEmpty(txtCount.value))
             count = txtCount.value - 0;

            if (crl.checked) {
             count = count + 1;
             txtCount.value = count;
            }
            else {
             count = count - 1;
             txtCount.value = count;
            }
            if (txtCount.value == txtTotal.value) {
             cbTotal.checked = true;
            }
            else {
             cbTotal.checked = false;
            }
        }
        
        function GetSelectedRow(lnk) { 
        /*
            row = lnk;
            
            var elements = document.getElementsByClassName('sgvDataRowSelected');
            while(elements.length > 0){
                elements[0].classList.remove('sgvDataRowSelected');
            }
            
            //document.getElementById(strSelectedID).value = row.cells[1].innerHTML;
            row.classList.add("sgvDataRowSelected");
        */
        }

        function GetDoubleClickRow(lnk){
        /*
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
        */
        }
       
    </script>
</asp:Content>
