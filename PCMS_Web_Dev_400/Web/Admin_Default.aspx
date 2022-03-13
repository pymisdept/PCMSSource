<%@ Page Language="C#" MasterPageFile="~/Control/MasterBase.master" AutoEventWireup="true"
    CodeFile="Admin_Default.aspx.cs" Inherits="Admin_Default" %>

<%@ Import Namespace="PCCore" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BaseContentPlaceHolder" runat="Server">
    <% 
        string appUrl = Config.AppBaseUrl;
        string themeUrl = Config.GetThemeBaseUrl(Page.Theme); 
    %>
    
    
    
    <div id="div_SelectProject" visible="false" style="width:100%;height:10px;overflow:auto;" >
        <table width="80%" visible="false" height="100%" border="0" align="center" cellpadding="0" cellspacing="10"
            >
            <tr>
            <td width="100%">
                <PC:Note ID="Note" runat="server" />
            </td>
            </tr>
            <tr>
                <td width="100%" height="20%" align="center">
                    <table  visible="false" width="100%">
                    
                    <tr visible="false">
                    <td width="20%">
                        <PC:Label Visible="false" ID="LebelProject" Font-Bold="true" runat="server" Text="<%$ Resources:Labels,SearchProject %>"></PC:Label>
                    </td>
                    <td width="80%">
                    <PC:TextBox Visible="false" ID="txtSearchProject" runat="server" Width="60%"></PC:TextBox><PC:ImageButton ID="btnProjSearch" Visible="false" runat="server" onclick="btnProjSearch_Click" 
                         />
                    </td>
                    
                    </tr>
                    </table>
                    
                    
                    
                </td>
            </tr>
            
            <tr runat="server" visible="false" id="trSearchProj">
            <td width="100%" height="80%" class="GridViewContainer">
                <PC:SAPDbDataSource ID="dsPrjSearch" runat="server"></PC:SAPDbDataSource>
                <PC:PrjSearchGridView id="gvPrjSearch"  AllowPaging="false" DataSourceID="dsPrjSearch" SkinID="SecuritySkin" SelectValueField="prjcode" DataKeyNames="docentry" runat="server"></PC:PrjSearchGridView>
                
                
            
            </td>
            </tr>
            <tr visible="false">
            <td width="100%" align="right">
                <table visible="false" width="100%">
                <tr visible="false">
                <td width="30%" align="left">
                    <asp:CheckBox Visible="false"  ID="chkAllProj" runat="server" AutoPostBack="true" 
                        Text="<%$ Resources:Labels,ChooseAllProject %>" 
                        oncheckedchanged="chkAllProj_CheckedChanged" />
                
                </td>
                <td width="70%" align="right">
                    <asp:Button Visible="false" ID="btnSubmitProj" runat="server" 
                        Text="<%$ Resources:Labels,Submit%>" OnClick="btnSubmit_Click" />
                </td>
                </tr>
                </table>
                
            </td>
            </tr>
        </table>    
    </div>
    
    

    <table width="100%" id="tblAnnouncement" runat="server" height="100%" border="0" align="center" cellpadding="0" cellspacing="10">
        <tr>
        <td>
            <asp:Button ID="btnRead1" runat="server" width="150px"
                            Text="<%$ Resources:Labels, MarkasRead %>" OnClick="btnRead1_Click"/>
            <asp:Button ID="btnUnRead1" runat="server"  width="150px"
                            Text="<%$ Resources:Labels, MarkasUnread %>" OnClick="btnUnRead1_Click"/>
                            <br />
                <%--<div id="div_Announcement"  style="width: 100%;" class="DivGridstyle_gray"></div>--%>
                <PC:DbDataSource runat="server" ID="dsGridView" >
                    </PC:DbDataSource>
                    
                    <PC:GridView runat="server" ID="gvData" DataSourceID="dsGridView"  AutoToolTip="false"
                    SelectValueField="ID" AllowMultipleSelect="False" OnRowCreated="gvData_RowCreated"  OnRowDataBound="gvData_RowDataBound" SkinID="SecuritySkin" />
                    <br />
                
            </td>
        </tr>
        <tr>
        <td>
        <asp:Button ID="btnRead2" runat="server" width="150px"
                                Text="<%$ Resources:Labels, MarkasRead %>" OnClick="btnRead2_Click"  />
                <asp:Button ID="btnUnRead2" runat="server"  width="150px"
                                Text="<%$ Resources:Labels, MarkasUnRead %>" OnClick="btnUnRead2_Click"  />
        </td>
        </tr>
    </table>    
    
    
    
    <script language="javascript">
    var frmMain = document.forms[0];
         
    function selfToBeExecute()
    {
        
        
        //div_Announcement.insertAdjacentHTML("afterBegin", Admin_Default.GetHtmlTable());
        //if (typeof(Admin_Default) != "undefined" && Admin_Default !=null)
       // {
            //divUseFullLink.outerHTML =Admin_Default.GetHtmlTable("").value;
       // }
    }
    
    var oPopup = window.createPopup();
    function clickUsefullMore()
    {
        var oPopBody = oPopup.document.body;
        oPopBody.style.backgroundColor = "lightyellow";
        oPopBody.style.border = "solid black 1px";
       
        oPopBody.innerHTML = oDialog.innerHTML;
        oPopup.show(100, 100, 50, 400, document.body);
    }

   function SearchStaff0() {
   //var frmMain=document.form(0);
   var _src="./PersonalQuery.aspx";
   var tmpUrl = window.location.href;
   var _staff=frmMain.txtStaffSearch.value;
   
   //var _backUrl=SimpleJS.getUrlKeyValue("src");
   var _backUrl=SimpleJS.getUrlKeyValue("src");
   
   if (tmpUrl.indexOf("PersonalQuery.aspx")==-1)
   {
        
        window.location.href(_src+"?src="+tmpUrl+"&staff="+_staff);
   }
   else
   {        
        var url="PersonalQuery.aspx?src="+_backUrl+"&staff="+_staff;
        window.location.href(url);    
   }  
}

function SearchStaff() {
        
        var url="<%=appUrl%>/Personal/Personal.aspx";       
        var _staff=txtStaffSearch.value;  
        url+="?staff="+ escape(_staff)+"&menu=1";
        
        window.location.href(url);   
        }
        
        function Submit() {
            
            var row = gvPrjSearch.getSelectedRow();
            //var status = gvData.getCellText(row, 12);
            
            var selected = gvPrjSearch.getSelectedRowValue();
            
           
            
                if (selected != "")
                {
                    setCommand(COMMAND_SUBMIT);
                    frmMain.submit();
                    setCommand(COMMAND_REFRESH);
                    frmMain.submit();
                    
                } else {
                    alert('please select project');
                }
            
        }
        
        function ViewAnnouncement(id,type,userid)
        {
            var dialogFeatures = "dialogHeight:" + 300+ "px;dialogWidth:" + 600 + "px;";
            
            var p = {};
	        var formUrl = "./AnnouncementView.aspx?msgid=" + id + "&msgtype=" + type + "&msguserid=" + userid;
            p[PARAM_URL] = formUrl;
            p[PARAM_MODE] = FORM_MODE_NEW;
            
            var ret = showForm(p, dialogFeatures); 
            document.forms[0].submit();
        
        
        }
    </script>

</asp:Content>
