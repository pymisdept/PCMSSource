<%@ Page Language="C#" MasterPageFile="~/Control/MasterProjectReport.master" AutoEventWireup="true"
    CodeFile="PU15.aspx.cs" Inherits="PU07" %>

<%@ MasterType VirtualPath="~/Control/MasterProjectReport.master" %>
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
                &nbsp;</td>
            <%--<td width="10px">
                :
            </td>--%>
            <td>
                <%-- <PC:TextBox ID="txtFileName" runat="server" CssClass="TextBoxName" SubmitOnEnter="True"  Width="200px" /></td>--%>
            <td>
                <span style="padding-left: 5px;" />
                <PC:Label runat="server" ID="lblDepartment" LabelStyle="xLabel" Text="<%$ Resources:Labels,Project %>" />
            </td>
            <td>
                :</td>
            <td style="width: 10px;">
                <PC:DropDownList runat="server" ID="ddlProject" RegisterClientVariable="true"
                    Width="300px" />
            </td>
            <td style="width: 10px;">
                <PC:Label runat="server" ID="Label1" Visible="false" LabelStyle="xLabel" Text="<%$ Resources:Labels,To %>" />
            </td>
            <td style="width: 10px;">
                <PC:DropDownList runat="server" ID="ddltProject" Visible="false" RegisterClientVariable="true"
                    Width="300px" />
            </td>
            <td colspan="4" style="width: 25px">
                <PC:GoButton id="SearchButton" runat="server" Width="25px" OnClientClick="javascript:if(!PrintProjectReport()) return false;" />
                                     
            </td colspan="4">
            <td>
                <PC:ToolBar runat="server" Visible="false" ID="toolbContact" />
            </td>
        </tr>
        <tr>
            <td height="5px" colspan="10">
                &nbsp;</td>
        </tr>        
        <tr>
            <td>
                <span style="padding-left: 5px;" />
                <%--<PC:Label LabelStyle="xLabel" runat="server" ID="lblSearch" Text="<%$ Resources:Labels,Date %>" />--%>
            </td>
            <td width="10px">
                <%-- :--%>
            </td>
            <td>
                <%-- <PC:TextBox ID="txtStartDate" runat="server" CssClass="TextBoxName" SubmitOnEnter="True"
                    DataType ="Date" Width="75px" />-
                <PC:TextBox ID="txtEndDate" runat="server" CssClass="TextBoxName" SubmitOnEnter="True"
                    DataType ="Date"  Width="75px" /></td>--%>
             <td>
                 &nbsp;<td>
                <span style="padding-left: 5px;" />
                <%-- <PC:Label runat="server" ID="Label1" LabelStyle="xLabel" Text="<%$ Resources:Labels,UploadBy %>" />--%>
            </td>
            <td style="width: 10px;">
                <%-- :--%>
            </td>
            <td style="width: 10px;">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td align="right">
                &nbsp;</td>
        </tr>
        <tr>
            <td height="5px" colspan="10">
            </td>
        </tr>
        <!-- search area end -->
        <!-- Karrson: Confirmation Button -->
        <tr height="100%">
            <td valign="top" align="left" class="Confirm" colspan="10">
                <%--<asp:Button ID="btnConfirm" Text="<%$Resources:Labels,Confirmation %>" 
                    runat="server" OnClientClick="javascript:if(!Confirm()) return false;" onclick="btnConfirm_Click" />--%>
            </td>
        </tr>
        <tr style="display:none;">
            <!--grdView begin -->
            <td class="ddlpaSearch" style="padding-left: 5px">
            </td>
            <td style="padding: 3px; height: 25px;" class="ddlpaSearch" valign="middle" 
                align="right" colspan="9">
                <PC:Label LabelStyle="xLabel" runat="server" ID="lblView" Text="<%$ Resources:Labels,View %>" />
                <%--<PC:DropDownList ID="ddlSearch" runat="server" Width="180px">
                </PC:DropDownList>--%>
                
            </td>
            <!--grdView End -->
        </tr>
        <!-- data area begin -->
        <tr height="100%">
            <td valign="top" class="GridViewContainer" colspan="10">
                <PC:DbDataSource runat="server" ID="dsGridView">
                </PC:DbDataSource>
                <%--<PC:GridView runat="server" ID="gvData" DataSourceID="dsGridView" SkinID="SecuritySkin"
                     SelectValueField="descid" DataKeyNames="descid" onselectedindexchanging="gvData_SelectedIndexChanging"  SelectedIndex="0" OnRowCommand="gvData_RowCommand"
                    BorderWidth="0" GridViewStyle="UserDefine" AllowMultipleSelect="False"  OnSorted="gvData_Sorted" OnRowDataBound="gvData_RowDataBound" OnSelectedIndexChanged="gvData_SelectedChanged" />--%>
            </td>
        </tr>
        <!-- data area end -->
        
    </table>

    <script language="javascript">
        var frmMain = document.forms[0]; 
    	var dialogFeatures = "dialogHeight:580px;dialogWidth:650px;";
    	var formUrl = "PUI02_Form.aspx";
    	
    	var dialogFeatures = "scrollbars=yes,menubar=no,toolbar=no,status:=no,dialogHeight:768px;dialogWidth:1024px;,top=10,left=10";
    	var UpAllDownoLoadFileUrl = "<%=appUrl %>/Control/DownLoadFile.aspx"; 
    	var ViewDocumentUrl = "<%=appUrl %>/Report/ViewData.aspx";
    	var backend = "<%=BackEnd %>";
    	var frontend = "<%=FrontEnd %>"; 
    	var lang = "<%=lang %>"; 
    	
    	
    	function Search()
    	{
    	    var projindex = ddlProject.selectedIndex;
            var projValue = ddlProject.options[projindex].value;
            var tprojindex = ddltProject.selectedIndex;
            var tprojValue = ddltProject.options[tprojindex].value;
    	    var p = {};
            var DB = backend;
            if (DB == backend)
            {
                if (lang == "zh-tw") 
                {
                    p[PARAM_URL] = "http://localhost/CrystalWeb/ReportFrame.aspx?Report=D:djfjldjaielafWeb%20ProgramsdjfjldjaielafCrystalWebdjfjldjaielafCRYSTALREPORTSdjfjldjaielafPU02_Purchase%20Order.rpt&user=1&docentry=" + id + "&DB=" + DB;
                }
                if (lang == "zh-cn")
                {
                    p[PARAM_URL] = "http://localhost/CrystalWeb/ReportFrame.aspx?Report=D:djfjldjaielafWeb%20ProgramsdjfjldjaielafCrystalWebdjfjldjaielafCRYSTALREPORTSdjfjldjaielafPU02_Purchase%20Order.rpt&user=1&docentry=" + id + "&DB=" + DB;
                }
                if (lang == "en-us")
                {
                    p[PARAM_URL] = "http://localhost/CrystalWeb/ReportFrame.aspx?Report=D:djfjldjaielafWeb%20ProgramsdjfjldjaielafCrystalWebdjfjldjaielafCRYSTALREPORTSdjfjldjaielafPU02_Purchase%20Order.rpt&user=1&docentry=" + id + "&DB=" + DB;
                }
            } 
            else
            {
                if (lang == "zh-tw") 
                {
                    p[PARAM_URL] = "http://localhost/CrystalWeb/ReportFrame.aspx?Report=D:djfjldjaielafWeb%20ProgramsdjfjldjaielafCrystalWebdjfjldjaielafCRYSTALREPORTSdjfjldjaielafPU02_Purchase%20Order.rpt&user=1&docentry=" + id + "&DB=" + DB;
                }
                if (lang == "zh-cn")
                {
                    p[PARAM_URL] = "http://localhost/CrystalWeb/ReportFrame.aspx?Report=D:djfjldjaielafWeb%20ProgramsdjfjldjaielafCrystalWebdjfjldjaielafCRYSTALREPORTSdjfjldjaielafPU02_Purchase%20Order.rpt&user=1&docentry=" + id + "&DB=" + DB;
                }
                if (lang == "en-us")
                {
                    p[PARAM_URL] = "http://localhost/CrystalWeb/ReportFrame.aspx?Report=D:djfjldjaielafWeb%20ProgramsdjfjldjaielafCrystalWebdjfjldjaielafCRYSTALREPORTSdjfjldjaielafPU02_Purchase%20Order.rpt&user=1&docentry=" + id + "&DB=" + DB;
                }
            }
            
            p[PARAM_MODE] = FORM_MODE_NEW;
    	    if (projValue != "-2") 
    	    {
    	        //window.open(pmrptlink,"_blank",dialogFeatures);
    	        showForm(p,dialogFeatures);
    	        return true;
    	    } else
    	    {
    	        return false;
    	    }
    	}
   
       
       
    </script>

</asp:Content>
