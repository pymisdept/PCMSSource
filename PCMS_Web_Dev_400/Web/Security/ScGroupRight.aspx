<%@ Page Language="C#" MasterPageFile="~/Control/MasterSecurity.master" AutoEventWireup="true"
    CodeFile="ScGroupRight.aspx.cs" Inherits="ScGroupRight" %>

<%@ MasterType VirtualPath="~/Control/MasterSecurity.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
     <% 
        string appUrl = Config.AppBaseUrl;
        string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
        ibProject.ImageUrl = themeUrl + "/images/search.gif";
        string strProjectClientID = ddlProject.ClientID;
        string strProjectTextID = invProjCode.ClientID;
        string actionquery = Consts.ActionQuery;
        string strGroupID = ddlGroup.ClientID;
         
        
    %>
    <PC:TextBox ID="invProjCode" runat="server" Width="0px" Visible="true"></PC:TextBox>
    <input type="hidden" id="hdnFlag" runat="server" />
    <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
        <!-- search area begin -->
        <tr style="padding-bottom: 5px;">
            <td>
                <PC:Label LabelStyle="xLabel"  runat="server" ID="lblGroup" Text="<%$ Resources:Labels,ScGroup %>" /><span
                    style="width: 5px;"></span>:
                <PC:DropDownList ID="ddlGroup" AutoPostBack="true" runat="server" Width="150px" 
                    onselectedindexchanged="ddlGroup_SelectedIndexChanged">
                </PC:DropDownList>
                <span style="width: 10px;"></span>
                <PC:Label LabelStyle="xLabel"  runat="server" ID="lblProject" Text="<%$ Resources:Labels,Project %>" /><span
                    style="width: 5px;"></span>:                 <PC:DropDownList ID="ddlProject" runat="server" Width="320px">
                    
                </PC:DropDownList>
                <PC:ImageButton ID="ibProject" runat="server" ImageUrl="<%=themeUrl %>/images/search.gif" OnClientClick="javascript:SearchProject()" />
                <span style="width: 10px;"></span>
             </td>
            <td align="right" width="20%">
                
            </td>   
            </tr>    
            <tr>
            <td width="80%">
                <PC:Label LabelStyle="xLabel"  runat="server" ID="lblModule" Text="<%$ Resources:Labels,Module %>" /><span
                    style="width: 5px;"></span>:
                <PC:DropDownList ID="ddlModule" runat="server" Width="150px">
                </PC:DropDownList>
                <!-- Function Type -->
                <asp:CheckBox ID="chkfunctype" runat="server" AutoPostBack="true" 
                    Text= "<%$ Resources:Labels, SysFunction %>" 
                    oncheckedchanged="chkfunctype_CheckedChanged" />
                <PC:SearchButton ID="SearchButton" runat="server" OnClientClick="Search();" />
            </td>
            <td align="right">
                <PC:ToolBar ID="tbToolBar" runat="server" />
            </td>
        </tr>
        <!-- search area end -->
        <tr>
            <td class="pageline_se" style="padding-left: 5px">
            </td>
            <td class="pageline_se" align="right" style="padding-right:5px">
                <PC:Label LabelStyle="xLabel"  ID="Label1" runat="Server" Text="<%$ Resources:Labels,View %>" />
                <PC:DropDownList ID="ddlSearch" runat="server" Width="180px"  RegisterClientVariable="true" onchange="Search(this.value);"/></td>
        </tr>
        <!-- data area begin -->
        <tr height="100%">
            <td valign="top" class="GridViewContainer" colspan="2" height="100%">
                <PC:DbDataSource runat="server" ID="dsGridView"  >
                </PC:DbDataSource>
                <PC:GridView runat="server" ID="gvData" SelectValueField="IDFUNCID" ShowPageIndex="false" DataSourceID="dsGridView"
                    AutoGenerateColumns="False" OnRowCreated="gvData_RowCreated" AllowBulkEdit="True"
                    AllowPaging="True" AllowSorting="False" AutoToolTip="False" ClearDirtyRows="True"
                    ClearSelectedRowValues="True" OnInit="gvData_Init" OnRowDataBound="gvData_RowDataBound"
                    SkinID="NoPageDownSkin" GridViewContainerStyle="height:400px;overflow-y:auto;" UseDataRowValueGetResoucesColumn="5,7" 
                    HiddenFields="ID,CODE,MID,CREATED,CREATEDBY,MODIFIED,MODIFIEDBY," >
                    <Columns>
                        <asp:BoundField DataField="IDFUNCID" HeaderText="IDFUNCID" SortExpression="IDFUNCID" ReadOnly="True" />
                        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" ReadOnly="True" />
                        <asp:BoundField DataField="CODE" HeaderText="CODE" SortExpression="CODE" ReadOnly="True" />
                        <asp:BoundField DataField="NAME" HeaderText="Group Name" SortExpression="NAME" ReadOnly="True" />
                        <asp:BoundField DataField="Mid" HeaderText="Mid" ReadOnly="True" />
                        <asp:BoundField DataField="Mname" HeaderText="Module Name" SortExpression="Mname"
                            ReadOnly="True" />
                        <asp:BoundField DataField="Funcid" HeaderText="Function Id" ReadOnly="True" />
                        <asp:BoundField DataField="FuncCode" HeaderText="Function Code" SortExpression="FuncCode" ReadOnly="True" />
                        <asp:BoundField DataField="Funcname" HeaderText="Function Name" SortExpression="Funcname"
                            ReadOnly="True" />
                        <asp:BoundField DataField="PrjCode" HeaderText="Project" SortExpression="PrjCode"
                            ReadOnly="True" />
                        <asp:TemplateField HeaderText="ALL" SortExpression="FALL">
                            <headertemplate>
                                <simple:SimpleImageCheckBox id="chkHAll" runat="server" Text="<%$ Resources:Labels,All %>" />
                            </headertemplate>
                            <itemtemplate>
                                <simple:SimpleImageCheckBox id="chkFAll" runat="server" Value='<%# Convert.ToInt32(Eval("FALL")) %>' />
                                
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="QRY" SortExpression="FQRY">
                            <headertemplate>
                                <simple:SimpleImageCheckBox id="chkHQRY" runat="server" Text="<%$ Resources:Labels,QRY %>" />
                            </headertemplate>
                            <itemtemplate>
                                <simple:SimpleImageCheckBox ID="chkFQRY" runat="server" Value='<%# Convert.ToInt32(Eval("FQRY")) %>'/>
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
                    </Columns>
                </PC:GridView>
            </td>
        </tr>
        <!-- data area end -->
    </table>

    <script language="javascript">
        var frmMain = document.forms[0];
        
    	var dialogFeatures = "dialogHeight:500px;dialogWidth:700px;";
    	var formUrl = "ScGroupRight_Form.aspx";

        function Refresh() 
        {   
            setCommand(COMMAND_REFRESH);     
            frmMain.submit();
        }

        function AddNew() 
        {
        }

        function Edit()
        {
        }        

        function Save() 
        {
            setCommand(COMMAND_SAVE);
            frmMain.submit();
        }
        function Delete()
        {
        
        }
        
       function Search(evalue)
       {
         setCommand(COMMAND_REFRESH);
         frmMain.submit();      
       }
       function Search()
       {
        setCommand(COMMAND_REFRESH);
         frmMain.submit();      
       }
        
        
       function SearchProject()
	    {
	    
	    var p = {};
	    
	    var strProjectClientID = "<%=strProjectClientID %>";
	    var strProjectTextID = "<%=strProjectTextID %>";
	    var actionquery = "<%=actionquery %>";
	    
	    var strGroupID = "<%=strGroupID %>";
	    var e = document.getElementById(strGroupID); // select element
	    
        var strGroup = e.options[e.selectedIndex].value; 
        
	    //var formUrl = "../Control/Search.aspx?type=groupproject&action=" + actionquery + "&function=" + functionid;
	    var formUrl = "../Control/Search.aspx?type=groupproject&group=" + strGroup ;
	    
        p[PARAM_URL] = formUrl;
        p[PARAM_MODE] = FORM_MODE_NEW;
        
        var SearchdialogFeatures = "dialogHeight:" + 300+ "px;dialogWidth:" + 800 + "px;";
        var ret = showForm(p, SearchdialogFeatures); 
        var ddl = document.getElementById(strProjectClientID);
        
        
        if (ret != "" && ret != undefined)
        {
 
            document.getElementById(strProjectTextID).value = ret;
            document.forms[0].submit();
        }
	}
       
    
    </script>

</asp:Content>
