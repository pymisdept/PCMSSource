<%@ Page Language="C#" MasterPageFile="~/Control/MasterForm.master" AutoEventWireup="true"
    CodeFile="PUI05_Form.aspx.cs" Inherits="PUI05_Form" %>

<%@ MasterType VirtualPath="~/Control/MasterForm.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
<%
    string functioncode = SessionInfo.CurrentFunction ;
    string functionid = SessionInfo.CurrentFunctionID ;
    string lang = SessionInfo.CurrentLanguage;
    string appUrl = Config.AppBaseUrl;
    
    string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
    PCCore.Common.HRLog.RecordLog("ThemeUrl," + themeUrl);
    ibProject.ImageUrl = themeUrl + "/images/search.gif";
    
    
%>

    <PC:TextBox ID="txtID" runat="server" CssClass="Invisible" Text='<%# _dr["ID"] %>'
        RegisterClientVariable="true" /><table border="0" cellspacing="0" cellpadding="3" width="100%" height="60px">
        <tr style="display:none;">
            <td width="20%">
                <PC:Label LabelStyle="xLabel" ID="Label8" runat="server" Text="<%$ Resources:Labels,FileName %>" />
            </td>
            <td>
                :
            </td>
            <td width="80%">
                <PC:TextBox ID="txtFileName" runat="server" CssClass="TextBoxForm" Width="480px"
                    RegisterClientVariable="true" ontextchanged="txtFileName_TextChanged">
                &nbsp;&nbsp;
                </PC:TextBox>
            </td>
        </tr>
        <tr>
        <td>
                <PC:Label LabelStyle="xLabel" ID="Label1" runat="server" Text="<%$ Resources:Labels,Project %>" />
            </td>
            <td>
                :
            </td>
             <td width="60%">
                <PC:DropDownList ID="ddlProject" runat="server" CssClass="TextBoxForm" 
                    Width="430px" RegisterClientVariable="true" AutoPostBack="true"
                     onselectedindexchanged="ddlProject_SelectedIndexChanged"></PC:DropDownList>
                     <PC:ImageButton ID="ibProject" runat="server" ImageUrl="<%=themeUrl %>/images/search.gif" OnClientClick="javascript:SearchProject()" />
            </td>
         </tr>
         <tr>
            <td width="30%">
                <PC:Label LabelStyle="xLabel" ID="Label4" runat="server" Text="<%$ Resources:Labels,InputType %>" 
                        AutoPostBack="true"/>
            </td>
            <td>
                :
            </td>
            <td width="70%">
                <PC:DropDownList ID="ddlInputType" runat="server" CssClass="TextBoxForm" 
                    Width="480px" RegisterClientVariable="true" AutoPostBack="true"
                    onselectedindexchanged="ddlInputType_SelectedIndexChanged"></PC:DropDownList>
            </td>
        </tr>
         
         <tr>
            <td width="30%">
                <PC:Label LabelStyle="xLabel" ID="Label5" runat="server" Text="<%$ Resources:Labels,Supplier %>" />
            </td>
            <td>
                :
            </td>
            <td width="70%">
                <PC:DropDownList ID="ddlVendor" runat="server" CssClass="TextBoxForm" 
                    Width="480px" RegisterClientVariable="true" AutoPostBack="true"
                    onselectedindexchanged="ddlVendor_SelectedIndexChanged"></PC:DropDownList>
            </td>
        </tr>
         <tr>
         
         <tr>
         <%--
         <td width = "30%">
                
                <PC:Label LabelStyle="xLabel" ID="Label5" runat="server" Width ="70px" Text="<%$ Resources:Labels,BaseNo %>" />
            </td>
            <td>
              :  
            </td>
            <td width = "70%">
                <PC:DropDownList ID="ddlmrno" Visible="true" runat="server" CssClass="TextBoxForm" 
                    Width="480px" RegisterClientVariable="true"
                    onselectedindexchanged="ddlmrno_SelectedIndexChanged">
                </PC:DropDownList>
            </td>
           --%>
            <td width="40%"><asp:Button ID="btnDownload" runat="server" Text="<%$ Resources:Common,Download %>" OnClientClick="javascript:if(!downloadTemplate()) return false;"
                                    Visible="false" OnClick="btnDownload_Click" />
                
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
                &nbsp;&nbsp;
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
                &nbsp;&nbsp;
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
    	var formUrl = "PUI05_Form.aspx";
    	var UpAlldialogFeaturesDownoLoadFile = "status:=no,Height=1030,Width=1150,top=270,left=320";
    	var UpAllDownoLoadFileUrl = "../Control/DownLoadTemplate.aspx"; 
    	//var ProjectCode = document.forms.item[0].ddlProject[document.forms.item[0].ddlProject.selectedIndex].value;
    	
    	function downloadTemplate()
        {
       // alert("test");
            var projindex = ddlProject.selectedIndex
            var projValue = ddlProject.options[projindex].value
            var pctypeindex  = ddlInputType.selectedIndex
            var pctypeValue = ddlInputType.options[pctypeindex].value   
                       
               _RequestStr = UpAllDownoLoadFileUrl+"?Func=PUI05&Proj="  + projValue + "&extend=" + pctypeValue;
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
