<%@ Page Language="C#" MasterPageFile="~/Control/MasterForm.master" AutoEventWireup="true"
    CodeFile="ScAnnouncement_Form.aspx.cs" Inherits="ScAnnouncement_Form" %>

<%@ MasterType VirtualPath="~/Control/MasterForm.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <%
        string _supervisor = Convert.ToString(PCCore.SessionInfo.IsSupervisor); 
         string strProjectTextClientID = invProject.ClientID;
   string strProjectClientID = ddlProject.ClientID;
   string appUrl = Config.AppBaseUrl;
   string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
   string userid = SessionInfo.UserId;
   ibSearchProject.ImageUrl = themeUrl + "/images/search.gif";
            
    %>
    <table width="100%" border="0" cellspacing="0" cellpadding="3">
        <tr>
            <td>
                <PC:Label runat="server" ID="lblmethod" Text="<%$ Resources:Labels, Type %>"></PC:Label>
            </td>
            <td>
                
                <asp:RadioButtonList ID="rbMth" runat="server" AutoPostBack="true" 
                    onselectedindexchanged="RadioButtonList1_SelectedIndexChanged">
                    <asp:ListItem Text="<%$ Resources:Labels, ByProject %>" Value="p"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:Labels, Golbal %>" Value="a"></asp:ListItem>
                    <asp:ListItem Text="<%$ Resources:Labels, Function %>" Value="f"></asp:ListItem>
                    
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>
                <PC:Label runat="server" ID="Label3" Text="<%$ Resources:Labels, Function %>"></PC:Label>
            </td>
            <td>
                
                <PC:DropDownList ID="ddlFunction" runat="server"></PC:DropDownList>
            </td>
        </tr>
        
    </table>
    <table id="tblProject" runat="server" width="100%" border="0" cellspacing="0" cellpadding="3">
    <tr visible="false">
        <td bgcolor="#dddddd">
            <span style="width: 3;"></span>
            <PC:HrCheckBox ID="cbTotal" runat="server" RegisterClientVariable="true" />
            <PC:Label ID="lblall" runat="server" Text="<%$ Resources:Labels,All %>" /></td>
    </tr>
    <tr visible="false">
        <td style="background-color: #999999; height: 1;">
        </td>
    </tr>
    <tr visible="false">
        <td>
            <div style="overflow: auto; height: 75;" class="DivCheckBox_se">
                <asp:CheckBoxList runat="server" ID="cblProject">
                </asp:CheckBoxList>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <PC:Label ID="lblProject" runat="server" Text="<%$ Resources:Labels,Project %>"></PC:Label>
        </td>
    </tr>
    <tr>
        
        <td>
            <PC:DropDownList AutoPostBack="true" ID="ddlProject" runat="server" Width="80%" 
                onselectedindexchanged="ddlProject_SelectedIndexChanged" ></PC:DropDownList>
            <PC:ImageButton  runat="server" ID="ibSearchProject" OnClientClick="javascript:_SearchProject();" />
                
        </td>
    </tr>
    </table>
    <table id="tblUser" runat="server" width="100%" border="0" cellspacing="0" cellpadding="3">
    <tr>
            <td colspan="3">
                <PC:Label LabelStyle="xLabel" runat="server" ID="lblUsers" Text="<%$ Resources:Labels,Users %>" /></td>
        </tr>
        <tr>
            <td class="sgvGrayHeaderRow" style="padding-left: 10px" width="100%" colspan="3">
            </td>
        </tr>
        
        <tr height="100%">
            <td valign="top" class="GridViewContainer" colspan="3" height="100%">
                <PC:DbDataSource runat="server" ID="dsGridView" AutoRestoreSelectCommand="false">
                </PC:DbDataSource>
                <PC:GridView runat="server" EnableViewState="true" ID="gvData" DataSourceID="dsGridView" ShowPageIndex="false"
                    SelectValueField="ID" AllowMultipleSelect="True" MultipleSelectDataSourceField="INID"
                    Width="100%" OnRowCreated="gvData_RowCreated" SkinID="NoPageDownSkin" />
            </td>
        </tr>
    </table>
    <table width="100%" border="0" cellspacing="0" cellpadding="3">
            
            
            
            <tr>
            <td width="15%">
                <PC:Label LabelStyle="xLabel"  runat="server" ID="lblCode" Text="<%$ Resources:Labels,AnnouncementTitle %>"  />
            </td>
            <td width="2%">
                :
            </td>
            <td>
                <%--<PC:TextBox ID="txtAnnouncementTitle" runat="server" CssClass="TextBoxForm" Required="True" RequiredErrorMessage="<%$ Resources:Messages,InputAnnouncementTitle %>"
                    Text='<%# _dr["TITLE"] %>' Width="500px" />--%>
                    
                <PC:TextBox ID="txtAnnouncementTitle" runat="server" CssClass="TextBoxForm" 
                    Text='<%# _dr["TITLE"] %>' Width="500px" />
            </td>
        </tr>
        <tr>
            <td width="15%">
                <PC:Label LabelStyle="xLabel"  runat="server" ID="lblName" Text="<%$ Resources:Labels,Body %>" /></td>
            <td width="2%">
                :
            </td>
            <td>
                <%--<PC:TextBox ID="txtBody" runat="server" CssClass="TextBoxForm" Required="True" MaxLength="350"
                    TextMode="MultiLine" Height="200px" Width="500px" RequiredErrorMessage="<%$ Resources:Messages,InputBody %>"
                    Text='<%# _dr["BODY"] %>' />
                --%>    
                 <PC:TextBox ID="txtBody" runat="server" CssClass="TextBoxForm" MaxLength="350"
                    TextMode="MultiLine" Height="200px" Width="500px" 
                    Text='<%# _dr["BODY"] %>' />
            </td>
        </tr>
        <tr>
            <td width="15%">
                <PC:Label LabelStyle="xLabel"  runat="server" ID="Label2" Text="<%$ Resources:Labels,EffectiveDate %>" /></td>
            <td width="2%">
                :
            </td>
            <td>
                <PC:TextBox ID="txtEffectiveDate" runat="server" CssClass="TextBoxForm" DataType="date" Required="true" RegisterClientVariable="true" 
                    Width="120px" RequiredErrorMessage="<%$ Resources:Messages,InputEffectiveDate %>"
                    Text='<%# _dr["EFFECTIVEDATE"] %>' />
            </td>
        </tr>
        <tr>
            <td width="15%">
                <PC:Label LabelStyle="xLabel"  runat="server" ID="Label1" Text="<%$ Resources:Labels,ExpiryDate %>" /></td>
            <td width="2%">
                :
            </td>
            <td>
                <PC:TextBox ID="txtExpiryDate" runat="server" CssClass="TextBoxForm" DataType="date"  RegisterClientVariable="true" 
                    Width="120px"  />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <PC:TextBox ID="txtID" runat="server" CssClass="Invisible" Text='<%# _dr["ID"] %>' />
            </td>
        </tr>        
        <PC:TextBox ID="invProject" runat="server" Width="0px"></PC:TextBox>
    </table>
    
    <script language="javascript" type="text/javascript" >
        function onSaveForm()
        {
            if(txtExpiryDate.value!="" && (txtEffectiveDate.value > txtExpiryDate.value))
            {
                alert("<%=Resources.Messages.ExpiryDateLaterThanEffectDate %>");
                txtExpiryDate.focus();
                return false;
            }
            
            return true;
        }
    
    function _SearchProject()
	{
	    
	    var userid = "<%=userid %>";
	    var p = {};
	    var strProjectTextClientID = "<%=strProjectTextClientID %>";
	    var formUrl = "../Control/Search.aspx?type=userproject&user=" + userid;
	    
	    var dialogFeatures =  "dialogHeight:" + 400+ "px;dialogWidth:" + 650 + "px;";
        p[PARAM_URL] = formUrl;
        p[PARAM_MODE] = FORM_MODE_NEW;
        
        var ret = showForm(p, dialogFeatures); 
        
        
        if (ret != "" && ret != undefined)
        {
            
            document.getElementById(strProjectTextClientID).value = ret;
            //document.getElementById(strProjectClientID).selectedvalue = ret;
            //document.getElementById(strProjectClientID).selectedIndex = 10;
            //frmMain.submit();
            document.forms[0].submit();
        }
	}
    </script>
</asp:Content>
