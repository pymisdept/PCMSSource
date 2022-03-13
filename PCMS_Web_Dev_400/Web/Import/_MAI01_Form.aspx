<%@ Page Language="C#" MasterPageFile="~/Control/MasterForm.master" AutoEventWireup="true"
    CodeFile="MAI01_Form.aspx.cs" Inherits="MAI01_Form" %>

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
    string PMReportClientID = PMReportID.ClientID;
    
%>
    <PC:TextBox ID="txtID" runat="server" CssClass="Invisible" Text='<%# _dr["ID"] %>'
        RegisterClientVariable="true" />
    
    <input type="hidden" id="PMReportID" runat="server" />
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
                &nbsp;
                </PC:TextBox>
            </td>
        </tr>
        <tr>
            <td width="20%">
                <PC:Label LabelStyle="xLabel" ID="Label4" runat="server" width = "90px" Text="<%$ Resources:Labels,Project %>" />
            </td>
            <td width="5%">
                :
            </td>
            <td>
              
                    <PC:DropDownList ID="ddlProject" runat="server"
                        CssClass="TextBoxForm" Width="90%" RegisterClientVariable="true" 
                        onselectedindexchanged="ddlProject_SelectedIndexChanged" AutoPostBack="true">
                </PC:DropDownList>
                <PC:ImageButton ID="ibProject" runat="server" ImageUrl="<%=themeUrl %>/images/search.gif" OnClientClick="javascript:SearchProject()" />
                
                
            </td>
        </tr>
         </td>
        </tr>
        <tr style="display:none">
            <td>
                <PC:Label LabelStyle="xLabel" ID="Label1" Visible="true" runat="server" Text="<%$ Resources:Labels,BaseNo %>" />
            </td>
            <td>
              :  
            </td>
            
                <%--modify by karrson on 2009-05-26: add table to store download button --%>
                <%--
            <td>    
                <table>
                <tr>--%>
                <td width="60%"><PC:DropDownList ID="ddlmrno" Visible="true" runat="server" 
                        CssClass="TextBoxForm" Width="480px" RegisterClientVariable="true">
                </PC:DropDownList></td>
                
                <%--
                <td width="40%"><asp:Button ID="btnDownload" runat="server" Text="<%$ Resources:Common,Download %>" OnClientClick="javascript:if(!downloadTemplate()) return false;"
                                    Visible="false" OnClick="btnDownload_Click" />
                </td>
                </tr>
                </table>
                
            </td>--%>
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
                &nbsp;
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
                &nbsp;
                </PC:TextBox>
            </td>
        </tr>
        <tr style="display:none">
            <td colspan="3" nowrap>
                <PC:UpdateLoad ID="UpdateLoad" runat="server" RegisterClientVariable="true" UserDefineRecordID="txtID" />
            </td>
        </tr>
    </table>
    
    <script language="javascript">
    
        var frmMain = document.forms[0];
        var appUrl = "<%=appUrl %>";
    	var dialogFeatures = "dialogHeight:400px;dialogWidth:650px;";
    	var formUrl = "MAI01_Form.aspx";
    	var UpAlldialogFeaturesDownoLoadFile = "status:=no,Height=230,Width=350,top=270,left=320";
    	var UpAllDownoLoadFileUrl = "../Control/DownLoadTemplate.aspx"; 
    	//var ProjectCode = document.forms.item[0].ddlProject[document.forms.item[0].ddlProject.selectedIndex].value;
    	
    	function downloadTemplate()
        {
        
       var index = ddlProject.selectedIndex;
       var project = ddlProject.options[index].value;
       var inputTypeIndex = ddlInputType.selectedIndex
       var inputTypeValue = ddlInputType.options[inputTypeIndex].value
       
       
       _RequestStr = UpAllDownoLoadFileUrl+"?Func=MAI01&Proj=" + project +  "&extend=" + inputTypeValue;
       window.open(_RequestStr,"_blank", UpAlldialogFeaturesDownoLoadFile,false); //open a new blank widows,if click open ,open in it                
            
            return true;
        }
        function selectFirst() {
        }
        function onSaveForm() {
            return true;
        }
        function PrjRptFrom(id)
        {
            
        }
        function PMReport()
        {
            
            
            var PMReportClientID = "<%=PMReportClientID %>";
            var id = document.getElementById(PMReportClientID).value;
            
            var p = {};
            // CheckList Report Version
            //p[PARAM_URL] = ReportLocation + "?Report=" + ReportName + "&DocEntry=" + id;
            // CheckList Screen
            
            var pmurl =  "../Import/MAI01_01.aspx?DocEntry=" + id + "&isNew=Y";
            //var pmurl =  "../Import/MAI01_01.aspx";
            
            p[PARAM_URL] = pmurl;
            p[PARAM_MODE] = FORM_MODE_NEW;
            //p[PARAM_ID] = id;
            // Report Link Alert Debug
            var height = screen.height;
            var width = screen.width;
            
           var dialogFeatures2 = "dialogHeight:700px;dialogWidth:900px;";
           //var dialogFeatures2 = "";
           
           
            //var ret = showForm(p, dialogFeatures2);            
            //var ret = showModelessForm(p,dialogFeatures2);
            
            
            window.returnValue = id;
            self.close();
            
            
        }
    </script>

</asp:Content>
