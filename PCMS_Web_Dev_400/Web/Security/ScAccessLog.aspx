<%@ Page Language="C#" MasterPageFile="~/Control/MasterSecurity.master" AutoEventWireup="true"
    CodeFile="ScAccessLog.aspx.cs" Inherits="ScAccessLog" %>

<%@ MasterType VirtualPath="~/Control/MasterSecurity.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" onclick="HideDisplay();">
        <!-- search area begin -->
        <tr style="padding-bottom:5px;">
            <td>
                    <PC:Label LabelStyle="xLabel"  runat="server" ID="lblLoginName" Text="<%$ Resources:Labels,LoginName %>" /><span style="width:5px;"></span>:
                    <PC:DropDownList runat="server" ID="ddlLoginName" RegisterClientVariable="true"></PC:DropDownList>
                   <%-- <span style="width:10px;"></span>
                    <PC:Label LabelStyle="xLabel"  runat="server" ID="lblAction" Text="<%$ Resources:Labels,Action %>" /><span style="width:5px;"></span>:
                    <PC:DropDownList ID="ddlAction" runat="server" onchange="javascript:setChange();"
                        Width="100px">
                        <asp:ListItem>-All-</asp:ListItem>                       
                        <asp:ListItem>Enter</asp:ListItem>                        
                        <asp:ListItem>Login</asp:ListItem>
                        <asp:ListItem>Logout</asp:ListItem>
                        <asp:ListItem>Timeout</asp:ListItem>
                     
                    </PC:DropDownList>--%>
                    <span style="width:10px;"></span>
                    <PC:Label LabelStyle="xLabel"  runat="server" ID="Label1" Text="<%$ Resources:Labels,AccessTime %>" /><span style="width:5px;"></span>:
                    <PC:TextBox ID="txtStartLogTime" runat="server" CssClass="TextBoxCode" Width="80px" SubmitOnEnter="True" DataType="Date" RegisterClientVariable="true" UserDefineErrorMessage="<%$ Resources:Messages,InputCorrectDate %>" />
                    <span style="width:10px;"> -</span>  
                   <PC:TextBox ID="txtEndLogTime" runat="server" CssClass="TextBoxCode" DataType="Date" Width="80px" SubmitOnEnter="True" RegisterClientVariable="true" UserDefineErrorMessage="<%$ Resources:Messages,InputCorrectDate %>"></PC:TextBox>
                    <PC:SearchButton ID="SearchButton" runat="server" />
        
            </td>
            <td align="right">
                <PC:ToolBar ID="tbToolBar" runat="server" />
            </td>
        </tr>

        <!-- search area end -->
        <tr>
            <td class="pageline_se" style="padding-left:5px; " nowrap width="100%" colspan="2"  align=right>
            <PC:DropDownList ID="ddlActiveType" runat="server" Width="150px" RegisterClientVariable="true" onchange="Refresh();" />
            </td>
        </tr>
        <!-- data area begin -->
        <tr>
            <td valign="top" class="GridViewContainer" colspan="2" height="100%">
                <PC:DbDataSource runat="server" ID="dsGridView">
                </PC:DbDataSource> 
                <PC:GridView runat="server" ID="gvData"   OnPageIndexChanging="gvData_OnPageIndexChanging"  SelectValueField="ID"  DataSourceID="dsGridView"
                    AllowMultipleSelect="False" AutoToolTip="False" HiddenFields="ID,CREATED,CREATEDBY,MODIFIED,MODIFIEDBY"
                     SkinID="SecuritySkin" UseDataRowValueGetResoucesColumn="4" >     
                </PC:GridView>
            </td>
        </tr>
        <!-- data area end -->
    </table>
    <div runat="server" id="divdisplayvalue"  style="display:none;position:absolute;left:300px;top:300px;padding:0px; background-color:#edfab0;border:0px" onclick="ShowDisplay();">
        <table height="100%" style="border-collapse:collapse" border="1" cellpadding=0 bordercolor="#7e9319" cellspacing=0>
            <tr>
                <td >
                    <PC:TextBox ID="txtDisplayValue" runat="server" BorderStyle="Solid" BorderColor="#edfab0" BackColor="#edfab0" RegisterClientVariable="true" ReadOnly="true" TextMode="multiLine"  Wrap="true" Rows="8"  Width="300px" />
            </tr>
        </table>
    </div>
    <script language="javascript" >
        var frmMain = document.forms[0];        
    	var dialogFeatures = "dialogHeight:520px;dialogWidth:700px;";
    	var formUrl = "ScAccessLog_Form.aspx";
    	var divID;
    	
      function Refresh()
       { 
        
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
            setCommand(COMMAND_REFRESH);
            frmMain.submit();
        }
        
        function AddNew() {
        }

        function Edit() {
//            var row = gvData.getSelectedRow();
//            if(row==null) {
//                //alert(MSG_SELECT_EDIT);
//                return;
//            }
//            var id = gvData.getCellText(row, 1);    
//            //var content = gvData.getCellText(row, 8);        
//                
//                alert(id.value);
//            var p = {};
//            p[PARAM_URL] = formUrl;
//            p[PARAM_MODE] = FORM_MODE_EDIT;
//            p[PARAM_ID] = id;
//            p[PARAM_WIN] = window;
//            var ret = showForm(p, dialogFeatures);            
////            if(needRefresh(ret))
////               Refresh();
               
        }

        function Save() {
            setCommand(COMMAND_SAVE);
            Refresh();

        }
        function Delete() {
        }
        
//        function Export() {
//            setCommand(COMMAND_EXPORT);
//            Refresh();
//        }
//        
             
        function ShowDisplayValue()
        {           
             var row = gvData.getSelectedRow();
             var id = gvData.getCellText(row, 0);
             var content=ScAccessLog.GetLogDetail(id);
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
                    strContent += "&nbsp" + sFirst + sLast + "<br>";
                    sAllobj= "&nbsp" + sFirst + sLast + "<br>";
                    if (_Ssplit[i].length > sMaxLineLength)
                    {
                        sMaxLineLength= _Ssplit[i].length;
                    }
                }
             }
             clickContentMore(strContent,sMaxLineLength);    

//         txtDisplayValue.innerText = strContent;    
//		   frmMain.document.getElementById(divID).style.left = GetPosX(310);
//		   frmMain.document.getElementById(divID).style.top = GetPosY(210);
//         frmMain.document.getElementById(divID).style.display = "";
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
            //oPopBody.style.overflowX="auto";
//            oPopBody.style.scrollbarBaseColor ="#ffffff";
//            oPopBody.style.scrollbarFaceColor ="#e4e4e4";
//            oPopBody.style.scrollbar3dLightColor ="#B6B6B6";
//            oPopBody.style.scrollbarShadowColor ="#B6B6B6";
            oPopBody.style.scrollbarHighlightColor ="#ffffff";
            oPopBody.style.scrollbarDarkShadowColor ="#ffffff";
//            oPopBody.style.scrollbarArrowColor ="#6c7f13";
            oPopBody.style.scrollbarTrackColor ="#f4f4f4";
            oPopBody.style.overflowY="auto";
            oPopBody.scroll="auto";            
            if ( eMaxLinelength*5 < 520)
            {
                oPopup.show(event.x-20,event.y, 530, 355, document.body);
            }else
            {
               oPopup.show(event.x-20,event.y, eMaxLinelength*5+350, 355, document.body);
            }
        }        
        
        function GetPosX(width)
        {
            var posX = window.event.clientX + document.body.scrollLeft; 
            var screenX = window.screen.width;
            if((posX + width + 10) > screenX )
            return posX - width - 10;
            else
            return posX;            
        }
        function GetPosY(height)
        {      
            var posY = window.event.clientY + document.body.scrollTop;          
            var screenY = window.screen.height;
            if((posY + height + 31) > screenY )
            return posY - height + 31 + 25;
            else
            return posY;            
        }
                
        function ShowDisplay()
        {
            frmMain.document.getElementById(divID).style.display = "";
        }
        function HideDisplay()
        {
            frmMain.document.getElementById(divID).style.display = "none";
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
