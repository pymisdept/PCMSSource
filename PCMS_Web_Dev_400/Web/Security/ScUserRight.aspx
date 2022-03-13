<%@ Page Language="C#" MasterPageFile="~/Control/MasterSecurity.master" AutoEventWireup="true"
    CodeFile="ScUserRight.aspx.cs" Inherits="ScUserRight" %>

<%@ MasterType VirtualPath="~/Control/MasterSecurity.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <%
    
    string appUrl = Config.AppBaseUrl;
    string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
    ibProject.ImageUrl = themeUrl + "/images/search.gif";
    string strProjectClientID = ddlProject.ClientID;
    string strProjectTextID = invProjCode.ClientID;
    string actionquery = Consts.ActionQuery;
    string strUserID = ddlUser.ClientID;
    %>
    <PC:TextBox ID="invProjCode" runat="server" Width="0px" Visible="False"></PC:TextBox>
    <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
        <!-- search area begin -->
        <tr style="padding-bottom: 5px;">
            <td width="80%">
                <PC:Label runat="server" ID="lblUser" Text="<%$ Resources:Labels,ScUser %>" LabelStyle="xLabel" /><span
                    style="width: 5px;"></span>:
                <PC:DropDownList ID="ddlUser" runat="server" AutoPostBack="true" Width="150px" 
                    onselectedindexchanged="ddlUser_SelectedIndexChanged"></PC:DropDownList>
                <PC:Label runat="server" ID="lblProject" Text="<%$ Resources:Labels,Project %>" LabelStyle="xLabel" /><span
                    style="width: 5px;"></span>:
                <PC:DropDownList ID="ddlProject" onselectedindexchanged="ddlProject_SelectedIndexChanged" runat="server" Width="300px"></PC:DropDownList>
                <PC:ImageButton ID="ibProject" runat="server" ImageUrl="<%=themeUrl %>/images/search.gif" OnClientClick="javascript:SearchProject()" />
                <span style="width: 10px;"></span>
             </td>
<!--
             <td width="20%">
                <PC:Label runat="server" ID="lblRole" Text="<%$ Resources:Labels,Role %>" 
                     LabelStyle="xLabel" CssClass="xLabel" RegisterClientVariable="False" 
                     /><span
                    style="width: 5px;"></span>:                 <PC:DropDownList ID="ddlRole" runat="server" AutoPostBack="true" Width="150px" 
                    ></PC:DropDownList>
             </td>
-->
             </tr>
             <tr>
             <td align="left" width="80%">   
                <PC:Label LabelStyle="xLabel" runat="server" ID="lblModule" Text="<%$ Resources:Labels,Module %>" /><span
                    style="width: 5px;"></span>:                 <PC:DropDownList ID="ddlModule" runat="server" Width="150px">
                </PC:DropDownList>     
                <!-- Function Type -->
                <asp:CheckBox ID="chkfunctype" runat="server" AutoPostBack="true" 
                     Text= "<%$ Resources:Labels, SysFunction %>" 
                       oncheckedchanged="chkfunctype_CheckedChanged" />           
                <PC:SearchButton ID="SearchButton" runat="server"  OnClientClick="Search();" />
                
            </td>
            <td align="right" width="20%">
            <PC:ToolBar ID="tbToolBar" runat="server" />
                <%--<table>
                <tr>
                <td width="50%">
                <PC:ImageButton ID="__ibRefresh" runat="server" OnClick="__ibRefresh_Click" />
                </td>
                <td width="50%">
                <PC:ImageButton ID="__ibSave" runat="server" OnClick="__ibSave_Click"  /> 
                </td>
                </tr>
                </table>--%>
            </td>
        </tr>
        <!-- search area end -->
        <tr>
            <td class="pageline_se" style="padding-left: 5px">
            </td>
            <td class="pageline_se" align="right" style="padding-right: 5px">
                <PC:Label LabelStyle="xLabel"  ID="Label1" runat="Server" Text="<%$ Resources:Labels,View %>" />
                <PC:DropDownList ID="ddlSearch" runat="server" Width="180px" RegisterClientVariable="true"
                    onchange="Search(this.value);" /></td>
        </tr>
        <!-- data area begin -->
        <tr id="trAnimation" style="display:none">
        <td><asp:Image ID="imgAnimation" ImageUrl="<%=themeUrl %>/images/activity.gif" runat="server" /></td>
        </tr>
        <tr height="100%">
            <td valign="top" class="GridViewContainer" colspan="2" height="100%">
                <PC:DbDataSource runat="server" ID="dsGridView">
                </PC:DbDataSource>
                <PC:GridView runat="server" ID="gvData" DataSourceID="dsGridView" SelectValueField="IDFUNCID"
                    ShowPageIndex="false" AutoGenerateColumns="False" AutoToolTip="false" AllowBulkEdit="True"
                    AllowPaging="True" AllowSorting="False" OnRowCreated="gvData_RowCreated" ClearDirtyRows="True"
                    ClearSelectedRowValues="True" ClientRowDblClicked="if(typeof(Edit)=='function') Edit();"
    
                       HiddenFields="ID,MID,CREATED,CREATEDBY,MODIFIED,MODIFIEDBY,"
                    lastSelectedRow="-1" lastSelectedRowCss="" OnInit="gvData_Init" OnRowDataBound="gvData_RowDataBound"
                    SkinID="NoPageDownSkin" GridViewContainerStyle="height:400px;overflow-y:auto;"
                    UseDataRowValueGetResoucesColumn="5,7">
                    <Columns>
                        <asp:BoundField DataField="IDFUNCID" HeaderText="IDFUNCID" SortExpression="IDFUNCID"
                            ReadOnly="True" />
                        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" ReadOnly="True" />
                        <asp:BoundField DataField="LOGINNAME" HeaderText="Login Name" SortExpression="LOGINNAME"
                            ReadOnly="True" />
                        <asp:BoundField DataField="FULLNAME" HeaderText="User Name" SortExpression="FULLNAME"
                            ReadOnly="True" />
                        <asp:BoundField DataField="Mid" HeaderText="Mid" ReadOnly="True" />
                        <asp:BoundField DataField="Mname" HeaderText="Module Name" SortExpression="Mname"
                            ReadOnly="True" />
                        <asp:BoundField DataField="Funcid" HeaderText="Funcid" ReadOnly="True" />
                        <asp:BoundField DataField="FuncCode" HeaderText="Function Code" SortExpression="FuncCode"
                            ReadOnly="True" />
                      <asp:BoundField DataField="Funcname" HeaderText="Function Name" SortExpression="Funcname"
                            ReadOnly="True" />
                        <asp:BoundField DataField="PrjCode" HeaderText="Project" SortExpression="PrjCode"
                            ReadOnly="True" />
                        <asp:TemplateField HeaderText="ALL" SortExpression="FALL">
                            <headertemplate>
                                <simple:SimpleImageCheckBox id="chkHAll" runat="server"  Text="<%$ Resources:Labels,All %>" />
                            </headertemplate>
                            <itemtemplate>
                                <simple:SimpleImageCheckBox id="chkFAll" runat="server"  Value="0" />
                                
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="QRY" SortExpression="FQRY">
                            <headertemplate>
                                <simple:SimpleImageCheckBox id="chkHQRY" runat="server" Text="<%$ Resources:Labels,QRY %>" />
                            </headertemplate>
                            <itemtemplate>
                                <simple:SimpleImageCheckBox ID="chkFQRY" runat="server"   Value='<%# Convert.ToInt32(Eval("FQRY")) %>'/>
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="NEW" SortExpression="FNEW">
                            <headertemplate>
                                <simple:SimpleImageCheckBox id="chkHNEW" runat="server" Text="<%$ Resources:Labels,NEW %>" />
                            </headertemplate>
                            <itemtemplate>
                                <simple:SimpleImageCheckBox ID="chkFNEW" runat="server" Value='<%# Convert.ToInt32(Eval("FNEW")) %>'/>      
                            
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="EDT" SortExpression="FEDT">
                            <headertemplate>
                                <simple:SimpleImageCheckBox id="chkHEDT" runat="server" Text="<%$ Resources:Labels,EDT %>" />
                            </headertemplate>
                            <itemtemplate>
                                <simple:SimpleImageCheckBox ID="chkFEDT" runat="server" Value='<%# Convert.ToInt32(Eval("FEDT")) %>'/>
                            
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DEL" SortExpression="FDEL">
                            <headertemplate>
                                <simple:SimpleImageCheckBox id="chkHDEL" runat="server" Text="<%$ Resources:Labels,DEL %>" />
                            </headertemplate>
                            <itemtemplate>
                                <simple:SimpleImageCheckBox ID="chkFDEL" runat="server" Value='<%# Convert.ToInt32(Eval("FDEL")) %>'/>
                            
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="APR" SortExpression="FAPR">
                            <headertemplate>
                                <simple:SimpleImageCheckBox id="chkHAPR" runat="server" Text="<%$ Resources:Labels,APR %>" />
                            </headertemplate>
                            <itemtemplate>
                                <simple:SimpleImageCheckBox ID="chkFAPR" runat="server" Value='<%# Convert.ToInt32(Eval("FAPR")) %>'/>
                            
                            </itemtemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataRowStyle CssClass="sgvEmptyRow" />
                    <PagerSettings Visible="False" />
                    <RowStyle CssClass="sgvDataRow" />
                    <HeaderStyle CssClass="sgvHeaderRow" />
                </PC:GridView>
            </td>
        </tr>
        <!-- data area end -->
    </table>
                                                        
    <script language="javascript">

        var frmMain = document.forms[0];

        var dialogFeatures = "dialogHeight:500px;dialogWidth:700px;";
        var formUrl = "ScGroupRight_Form.aspx";

        function Refresh() {
            setCommand(COMMAND_REFRESH);
            frmMain.submit();
        }

        function AddNew() {}

        function Edit() {}        

        function Save(){
            setCommand(COMMAND_SAVE);
            frmMain.submit();
        }

        function Delete() {}

        function Search(evalue){
            Refresh();        
        }   

        function Search(){
            Refresh();
        }

        function SearchProject(){
            var p = {};
            var strProjectClientID = "<%=strProjectClientID %>";
            var strProjectTextID = "<%=strProjectTextID %>";
            var actionquery = "<%=actionquery %>";
            var strUserID = "<%=strUserID %>";
            var e = document.getElementById(strUserID); // select element
            var strUser = e.options[e.selectedIndex].value; 

            //var formUrl = "../Control/Search.aspx?type=groupproject&action=" + actionquery + "&function=" + functionid;
            var formUrl = "../Control/Search.aspx?type=userproject&user=" + strUser ;

            p[PARAM_URL] = formUrl;
            p[PARAM_MODE] = FORM_MODE_NEW;

            var SearchdialogFeatures = "dialogHeight:" + 300+ "px;dialogWidth:" + 800 + "px;";
            var ret = showForm(p, SearchdialogFeatures); 
            var ddl = document.getElementById(strProjectClientID);

            if (ret != "" && ret != undefined){
                document.getElementById(strProjectTextID).value = ret;
                document.forms[0].submit();
            }
        }
        // Raik Debug Start
        function GetSelectedRow(lnk) { 
            row = lnk;
            
            var elements = document.getElementsByClassName('sgvDataRowSelected');
            while(elements.length > 0){
                elements[0].classList.remove('sgvDataRowSelected');
            }
            
            //document.getElementById(strSelectedID).value = row.cells[1].innerHTML;
            row.classList.add("sgvDataRowSelected");
        }
        
        function GetDoubleClickRow(lnk){}
        
        var tbl = document.getElementById("ctl00_ContentPlaceHolder_gvData");

        if (tbl != null) {
            for (var i = 1; i < tbl.rows.length; i++) {
                for (var j = 0; j < tbl.rows[i].cells.length; j++){
                    tbl.rows[i].cells[j].onclick = function () { 
                        ChangeCheckBox(this); 
                    };
                }
            }
            for (var j = 0; j < tbl.rows[0].cells.length; j++){
                tbl.rows[0].cells[j].onclick = function () { 
                    ChangeHeaderCheckBox(this); 
                };
            }
        }
        
        function ChangeCheckBox(cel) {
            //alert(cel.innerHTML);
            var Checkedicon = "./check.gif";
            var UnCheckedicon = "./uncheck.gif";
            
            var CurrentItem = cel.childNodes[1].id;
            var CurrentHiddenItem = cel.childNodes[1].id+'_v';
            
            if (document.getElementById(CurrentHiddenItem).value == '0;1')
            {
                document.getElementById(CurrentItem).setAttribute("src",Checkedicon);
                document.getElementById(CurrentHiddenItem).value = '1;1';
            }
            else if (document.getElementById(CurrentHiddenItem).value == '1;1')
            {
                document.getElementById(CurrentItem).setAttribute("src",UnCheckedicon);
                document.getElementById(CurrentHiddenItem).value = '0;1';
            }
        }
        
        function ChangeHeaderCheckBox(cel) {
            //alert(cel.cellIndex);
            var tbl2 = document.getElementById("ctl00_ContentPlaceHolder_gvData");
            var Checkedicon = "./check.gif";
            var UnCheckedicon = "./uncheck.gif";            
            var CurrentItem;
            var CurrentHiddenItem;
            var CurrentCel;
            var CurrentHeader = tbl2.rows[0].cells[cel.cellIndex].childNodes[1].id;
            if (tbl2 != null) {
                //Header Unchecked
                if (document.getElementById(CurrentHeader+'_v').value == '0;1'){
                    //alert("Header Checked");
                    document.getElementById(CurrentHeader).setAttribute("src",Checkedicon);
                    document.getElementById(CurrentHeader+'_v').value = '1;1';
                    for (var i = 1; i < tbl2.rows.length; i++) {
                        if (tbl2.rows[i].cells[cel.cellIndex].innerHTML.trim() != ""){
                            //alert(tbl2.rows[i].cells[cel.cellIndex].innerHTML.trim());
                            //ChangeCheckBox(tbl2.rows[i].cells[cel.cellIndex]);
                            CurrentCel = tbl2.rows[i].cells[cel.cellIndex];
                            CurrentItem = CurrentCel.childNodes[1].id;
                            CurrentHiddenItem = CurrentCel.childNodes[1].id+'_v';
                            
                            if (document.getElementById(CurrentHiddenItem).value == '0;1')
                            {
                                document.getElementById(CurrentItem).setAttribute("src",Checkedicon);
                                document.getElementById(CurrentHiddenItem).value = '1;1';
                            }
                        }
                    }
                }
                //Header Checked
                else if (document.getElementById(CurrentHeader+'_v').value == '1;1'){
                    //alert("Header Unchecked");
                    document.getElementById(CurrentHeader).setAttribute("src",UnCheckedicon);
                    document.getElementById(CurrentHeader+'_v').value = '0;1';
                    for (var i = 1; i < tbl2.rows.length; i++) {
                        if (tbl2.rows[i].cells[cel.cellIndex].innerHTML.trim() != ""){
                            //alert(tbl2.rows[i].cells[cel.cellIndex].innerHTML.trim());
                            //ChangeCheckBox(tbl2.rows[i].cells[cel.cellIndex]);
                            CurrentCel = tbl2.rows[i].cells[cel.cellIndex];
                            CurrentItem = CurrentCel.childNodes[1].id;
                            CurrentHiddenItem = CurrentCel.childNodes[1].id+'_v';
                            
                            if (document.getElementById(CurrentHiddenItem).value == '1;1')
                            {
                                document.getElementById(CurrentItem).setAttribute("src",UnCheckedicon);
                                document.getElementById(CurrentHiddenItem).value = '0;1';
                            }
                        }
                    }
                }
                
            }
        }
        // Raik Debug End
    </script>
</asp:Content>

   

