<%@ Page Language="C#" MasterPageFile="~/Control/MasterForm.master" AutoEventWireup="true"
    CodeFile="MA08_Form_20111017.aspx.cs" Inherits="MA08_Form" %>

<%@ MasterType VirtualPath="~/Control/MasterForm.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
<%
    string functioncode = SessionInfo.CurrentFunction ;
    string functionid = SessionInfo.CurrentFunctionID ;
    string lang = SessionInfo.CurrentLanguage;
    string appUrl = Config.AppBaseUrl;
    
    string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
    PCCore.Common.HRLog.RecordLog("ThemeUrl," + themeUrl);
   
    
    
%>

    <PC:TextBox ID="txtID" runat="server" CssClass="Invisible" Text='<%# _dr["ID"] %>'
        RegisterClientVariable="true" />
    <table border="0" cellspacing="0" cellpadding="3" width="100%" height="60px">
        <tr style="display:none;">
            <td width="20%">
                <PC:Label LabelStyle="xLabel" ID="Label8" runat="server" Text="<%$ Resources:Labels,FileName %>" />
            </td>
            <td>
                :
            </td>
            <td width="80%">
                <PC:TextBox ID="txtFileName" runat="server" CssClass="TextBoxForm" Width="480px"
                    RegisterClientVariable="true">
                </PC:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <PC:Label LabelStyle="xLabel" ID="Label1" runat="server" Text="<%$ Resources:Labels,Section %>" />
            </td>
            <td>
                :
            </td>
            <td>
                <%--modify by karrson on 2009-05-26: add table to store download button --%>
                <table>
                <tr>
                <td width="60%"><PC:DropDownList ID="ddlSectionCode" runat="server" CssClass="TextBoxForm" Width="400px" RegisterClientVariable="true">
                </PC:DropDownList></td>
                <td width="40%"><asp:Button ID="btnDownload" runat="server" Text="<%$ Resources:Common,Download %>" OnClientClick="javascript:if(!downloadTemplate()) return false;"
                                   Visible="false" OnClick="btnDownload_Click" />
                </td>
                </tr>
                </table>
                
            </td>
        </tr>
        <tr>
            <td width="20%">
                <PC:Label LabelStyle="xLabel" ID="Label4" runat="server" Text="<%$ Resources:Labels,CutOffDate %>" />
            </td>
            <td>
                :
            </td>
            <td width="80%"><PC:TextBox DataType="Date" ID="txtcutoff" runat="server"></PC:TextBox></td>
        </tr>
        <tr style="display:none">
            <td width="20%">
                <PC:Label LabelStyle="xLabel" ID="lblManual" runat="server" Text="<%$ Resources:Labels,Manual %>" />
            </td>
            <td>
                :
            </td>
            <td width="80%">
                <input id="Manual_CheckBox" name="Manual" type="checkbox" />       
            </td>
        </tr>
        <tr style="display:none;">
            <td>
                <PC:Label LabelStyle="xLabel" ID="Label2" runat="server" Text="<%$ Resources:Labels,Description %>" />
            </td>
            <td>
                :
            </td>
            <td>
                <PC:TextBox ID="txtDescription" runat="server" CssClass="TextBoxForm" Width="480px" RegisterClientVariable="true">
                </PC:TextBox>
            </td>
        </tr>
        <tr style="display:none;">
            <td>
                <PC:Label LabelStyle="xLabel" ID="Label3" runat="server" Text="<%$ Resources:Labels,Remarks %>" />
            </td>
            <td>
                :
            </td>
            <td>
                <PC:TextBox ID="txtRemarks" runat="server" CssClass="TextBoxForm" Width="480px" TextMode="MultiLine" Height="100px" RegisterClientVariable="true">
                </PC:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3" nowrap>
                <PC:UpdateLoad ID="UpdateLoad" runat="server" RegisterClientVariable="true" UserDefineRecordID="txtID" />
            </td>
            
        </tr>
    </table>

    <script language="javascript">
    
        var frmMain = document.forms[0];

    	var dialogFeatures = "dialogHeight:580px;dialogWidth:650px;";
    	var formUrl = "QSI40_Form.aspx";
    	var UpAlldialogFeaturesDownoLoadFile = "status:=no,Height=230,Width=350,top=270,left=320";
    	var UpAllDownoLoadFileUrl = "../Control/DownLoadTemplate.aspx"; 
    	//var ProjectCode = document.forms.item[0].ddlProject[document.forms.item[0].ddlProject.selectedIndex].value;
    	
    	function downloadTemplate()
        {
       // alert("test");
            var index = ddlProject.selectedIndex;
            var project = ddlProject.options[index].value;
            _RequestStr = UpAllDownoLoadFileUrl+"?Func=QSI40&Proj=" + project;             
            window.open(_RequestStr,"_blank", UpAlldialogFeaturesDownoLoadFile,false); //open a new blank widows,if click open ,open in it                
            
            return true;
        }
        function selectFirst() {
        }
        function onSaveForm() {
            return true;
        }
    </script>

</asp:Content>
