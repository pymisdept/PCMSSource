<%@ Page Language="C#" MasterPageFile="~/Control/MasterSecurity.master" AutoEventWireup="true"
    CodeFile="ScAuditLog.aspx.cs" Inherits="ScAuditLog" %>

<%@ MasterType VirtualPath="~/Control/MasterSecurity.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" onclick="HideDisplay();" >
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr style="padding-bottom: 5px;">
                        <td>
                            <PC:Label LabelStyle="xLabel"  runat="server" ID="lblLoginName" Text="<%$ Resources:Labels,LoginName %>" />
                        </td>
                        <td>
                            :</td>
                        <td>
                            <PC:DropDownList ID="ddlLoginName" runat="server" RegisterClientVariable="true" Width="130">
                            </PC:DropDownList></td>
                        <td>
                            <PC:Label LabelStyle="xLabel"  runat="server" ID="Label1" Text="<%$ Resources:Labels,AccessTime %>" /></td>
                        <td>
                            :</td>
                        <td>
                            <PC:TextBox ID="txtStartLogTime" runat="server" CssClass="TextBoxCode" Width="80px"
                                SubmitOnEnter="True" DataType="Date" RegisterClientVariable="True" UserDefineErrorMessage="<%$ Resources:Messages,InputCorrectDate %>" />
                            <span style="width: 5px">-</span>
                            <PC:TextBox ID="txtEndLogTime" runat="server" CssClass="TextBoxCode" DataType="Date"
                                Width="80px" SubmitOnEnter="True" UserDefineErrorMessage="<%$ Resources:Messages,InputCorrectDate %>" RegisterClientVariable="true"></PC:TextBox></td>
                        <td>
                        </td>
                    </tr>
                    <tr style="padding-bottom: 5px;">
                        <td>
                            <PC:Label LabelStyle="xLabel"  runat="server" ID="Label3" Text="<%$ Resources:Labels,Module %>" /></td>
                        <td>
                            :</td>
                        <td>
                            <PC:DropDownList ID="ddlModule" runat="server" RegisterClientVariable="true" OnSelectedIndexChanged="ddlModule_SelectedIndexChanged"
                                AutoPostBack="true" Width="130">
                            </PC:DropDownList></td>
                        <td>
                            <PC:Label LabelStyle="xLabel"  runat="server" ID="Label4" Text="<%$ Resources:Labels,Function %>" /></td>
                        <td>
                            :</td>
                        <td>
                            <PC:DropDownList ID="ddlFunction" runat="server" RegisterClientVariable="true" Width="130" onchange="selfSearch();">
                            </PC:DropDownList>
                            <PC:SearchButton ID="SearchButton" runat="server" /></td>
                        <td align="right">
                            <PC:ToolBar ID="tbToolBar" runat="server" />
                        </td>
                </table>
            </td>
        </tr>
        <!-- search area begin -->        
        <tr>
            <td class="pageline_se" align="right" style="padding-right: 4px" colspan="2">
                <PC:Label LabelStyle="xLabel"  ID="lblAction" runat="Server" Text="<%$ Resources:Labels,ActionType %>" />
                <PC:DropDownList ID="ddlAction" runat="server" Width="180px" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlAction_SelectedIndexChanged">
                </PC:DropDownList></td>            
        </tr>        
        <tr>
            <td valign="top" class="GridViewContainer" colspan="2" height="100%">
                <PC:DbDataSource runat="server" ID="dsGridView">
                </PC:DbDataSource>
                <PC:GridView runat="server" ID="gvData" DataSourceID="dsGridView" SelectValueField="ID" ClientRowDblClicked="ShowDisplayValue();"
                    AllowMultipleSelect="False" AutoToolTip="false" HiddenFields="ID,CREATED,CREATEDBY,MODIFIED,MODIFIEDBY,"
                   SkinID="SecuritySkin1" UseDataRowValueGetResoucesColumn="3,4,7">
                </PC:GridView>
            </td>
        </tr>
        <!-- data area end -->
    </table>
    <div runat="server" id="divdisplayvalue" style="display: none; position: absolute;
        left: 300px; top: 300px; padding: 0px; " class="DivGridstyle_gray"
        onclick="HideDisplay();">
        <table height="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <PC:TextBox ID="txtDisplayValue" runat="server" BorderStyle="Solid" BorderColor="#999999" BorderWidth="1"
                        BackColor="#eeeeee" RegisterClientVariable="true"  ClientReadOnly="true" TextMode="multiLine"
                        Wrap="false" Rows="8" Width="320px" />
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript">
        var frmMain = document.forms[0];        
    	var dialogFeatures = "dialogHeight:450px;dialogWidth:600px;";
    	var formUrl = "ScAuditLog_Form.aspx";
    	var divID;
    	
    	function  selfToBeExecute()
    	{
    	    divDisp = frmMain.document.getElementById(divID);
    	}
    	
        function Refresh() {
        
            if(!SimpleJS.isNullOrEmpty(txtStartLogTime.value))
           {
               if(!SimpleJS.isDateYMD(txtStartLogTime.value))
                {
                    alert(txtStartLogTime.UserDefineErrorMessage);
                    txtStartLogTime.focus();
                    return false;
                }
            }
             if(!SimpleJS.isNullOrEmpty(txtEndLogTime.value))
           {
                if(!SimpleJS.isDateYMD(txtEndLogTime.value))
                {
                    alert(txtEndLogTime.UserDefineErrorMessage);
                    txtEndLogTime.focus();
                    return false;
                }
            }
                frmMain.submit();
        }
        
        function AddNew() {
        }

        function selfSearch()
        {
             setCommand(COMMAND_REFRESH);
             frmMain.submit();
        }
        
        function Edit() 
        {               
        }

        function Save() {
            setCommand(COMMAND_SAVE);
            Refresh();

        }
        function Delete() {
        }       
                
        function ShowDisplayValue()
        {
             var row = gvData.getSelectedRow();
             var id = gvData.getCellText(row, 0);
             var content=ScAuditLog.GetLogDetail(id);
             var _Ssplit = content.value.split("&^&");
             var strContent=" ";
             var sMaxMHPosition=0;
             var sMaxLineLength =0;
             var sAllobj,sFirst,sLast;
             if (SimpleJS.isNullOrEmpty(content)) return;
             
             for(i=0;i<_Ssplit.length;i++)
             {
                if(!SimpleJS.isNullOrEmpty(_Ssplit[i]))
                {
                    sAllobj=_Ssplit[i];
                    if (sAllobj.indexOf(":") >sMaxMHPosition)
                    {
                        sMaxMHPosition =sAllobj.indexOf(":");
                    }
                    sFirst = CommandJs.padRight(sAllobj.substring(0,sAllobj.indexOf(":")),sMaxMHPosition,"&nbsp");
                    sLast = sAllobj.substring(sAllobj.indexOf(":"),sAllobj.length);
                    strContent += "&nbsp" + sFirst + "&nbsp" + sLast + "<br>";
                    if (_Ssplit[i].length > sMaxLineLength)
                    {
                        sMaxLineLength= _Ssplit[i].length;
                    }
                }
             }
             clickContentMore(strContent, sMaxLineLength);                  
        }
        var oPopup = window.createPopup();
        function clickContentMore(eContent, eMaxLinelength)
        {
            var oPopBody = oPopup.document.body;
            oPopBody.style.backgroundColor = "#ffffff";
            oPopBody.style.border = "solid black 1px";
            oPopBody.style.fontSize="12px";
            oPopBody.style.fontFamily="@Courier New"
            oPopBody.innerHTML = eContent;
            oPopBody.style.scrollbarHighlightColor ="#ffffff";
            oPopBody.style.scrollbarDarkShadowColor ="#ffffff";
            oPopBody.style.scrollbarTrackColor ="#f4f4f4";
            oPopBody.style.overflowY="auto";
            oPopBody.scroll="auto";  
            if (eMaxLinelength*5 < 620)
            {
                oPopup.show(event.x-20,event.y, 500, 355, document.body);
            }else
            {
                oPopup.show(event.x-20,event.y, eMaxLinelength*5+300, 355, document.body);
            }
        } 
                
        function GetPosX(swidth)
        {
             posX = window.event.clientX + document.body.scrollLeft; 
             screenX = window.screen.width;
            if((posX + swidth + 10) > screenX )
            {
                return (posX - swidth - 10);
            }
            else
            {
                return posX;            
            }
        }
        function GetPosY(sheight)
        {      
             posY = window.event.clientY + document.body.scrollTop;          
             screenY = window.screen.height;
            if((posY + sheight + 31) > screenY )
            {
                return (posY - sheight + 31 + 25);
            }
            else
            {
                return posY;            
            }
        }
                
        function ShowDisplay()
        {
            
            ShowDisplayValue();
            divDisp.style.display = "";
        }
        function HideDisplay()
        {
            divDisp.style.display = "none";
        }
        
        function Space(value)
        {
            var returnvalue="";
            for(var i=0;i<value;i++)
            {
                returnvalue+=" ";
            }
            return returnvalue;
        }
    </script>

</asp:Content>
