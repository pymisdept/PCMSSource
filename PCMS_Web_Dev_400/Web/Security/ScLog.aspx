<%@ Page Language="C#" MasterPageFile="~/Control/MasterSecurity.master" AutoEventWireup="true"
    CodeFile="ScLog.aspx.cs" Inherits="ScLog" %>

<%@ MasterType VirtualPath="~/Control/MasterSecurity.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" onclick="HideDisplay();">
        <!-- search area begin -->
        <tr style="padding-bottom:5px;">
            <td>
                    <PC:Label LabelStyle="xLabel"  runat="server" ID="lblLoginName" Text="<%$ Resources:Labels,LoginName %>" /><span style="width:5px;"></span>:
                    <PC:TextBox ID="txtLoginName" runat="server" CssClass="TextBoxCode" SubmitOnEnter="True" />
                    <span style="width:10px;"></span>
                    <PC:Label LabelStyle="xLabel"  runat="server" ID="lblAction" Text="<%$ Resources:Labels,Action %>" /><span style="width:5px;"></span>:
                    <PC:DropDownList ID="ddlAction" runat="server" onchange="javascript:setChange();"
                        Width="100px">
                        <asp:ListItem>-All-</asp:ListItem>                       
                        <asp:ListItem>Enter</asp:ListItem>                        
                        <asp:ListItem>Login</asp:ListItem>
                        <asp:ListItem>Logout</asp:ListItem>
                        <asp:ListItem>Timeout</asp:ListItem>
                     
                    </PC:DropDownList>
                    <span style="width:10px;"></span>
                    <PC:Label LabelStyle="xLabel"  runat="server" ID="Label1" Text="<%$ Resources:Labels,StartTime %>" /><span style="width:5px;"></span>:
                    <PC:TextBox ID="txtStartLogTime" runat="server" CssClass="TextBoxCode" Width="80px" SubmitOnEnter="True" DataType="Date" />
                    <span style="width:10px;"></span>
                    <PC:Label LabelStyle="xLabel"  runat="server" ID="lblEndTime" Text="<%$ Resources:Labels,EndTime %>" /><span style="width:5px;"></span>:
                   <PC:TextBox ID="txtEndLogTime" runat="server" CssClass="TextBoxCode" DataType="Date" Width="80px" SubmitOnEnter="True"></PC:TextBox>
                    <PC:SearchButton ID="SearchButton" runat="server" />
        
            </td>
            <td align="right">
                <PC:ToolBar ID="tbToolBar" runat="server" />
            </td>
        </tr>

        <!-- search area end -->
        <tr>
            <td class="pageline_se" style="padding-left:5px" nowrap width="100%" colspan="2"></td>
        </tr>
        <!-- data area begin -->
        <tr>
            <td valign="top" class="GridViewContainer" colspan="2" height="100%">
                <PC:DbDataSource runat="server" ID="dsGridView">
                </PC:DbDataSource>
                <PC:GridView runat="server" ID="gvData" DataSourceID="dsGridView" SelectValueField="ID"
                    AllowMultipleSelect="False" AutoToolTip="false" HiddenFields="ID,CREATED,CREATEDBY,MODIFIED,MODIFIEDBY,CONTENT," ClientRowDblClicked="ShowDisplayValue();"
                    OnRowDataBound="gvData_RowDataBound" SkinID="SecuritySkin">                    
                </PC:GridView>
            </td>
        </tr>
        <!-- data area end -->
    </table>
    <div runat="server" id="divdisplayvalue"  style="display:none;position:absolute;left:300px;top:300px;padding:0px; background-color:#edfab0;border:0px" onclick="ShowDisplay();">
        <table height="100%" style="border-collapse:collapse" border="1" cellpadding=0 bordercolor="#7e9319" cellspacing=0>
            <tr>
                <td>
                    <PC:TextBox ID="txtDisplayValue" runat="server" BorderStyle="Solid" BorderColor="#edfab0" BackColor="#edfab0" RegisterClientVariable="true" ReadOnly="true" TextMode="multiLine"  Wrap="true" Rows="8"  Width="300px" />
            </tr>
        </table>
    </div>
    <script language="javascript">
        var frmMain = document.forms[0];        
    	var dialogFeatures = "dialogHeight:520px;dialogWidth:700px;";
    	var formUrl = "ScLog_Form.aspx";
    	var divID;
    	
        function Refresh() {        
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
        
        //add by kamte
//        var checkDown = false; 
//        var offsetX,offsetY;
//       
//              
//        
//        function mousemove()
//        {
//        
//            if(checkDown == false) return;  
//           
//             var posX = window.event.clientX + document.body.scrollLeft; 
//             var posY = window.event.clientY + document.body.scrollTop;
//     
//             frmMain.document.getElementById(divID).style.left = posX + offsetX;
//		     frmMain.document.getElementById(divID).style.top = posY + offsetY;          
//        }  
//        function setcheckdown()
//        {
//            checkDown = false;
//            alert(checkDown);
//        }
//        function mousedown()
//        {
//            checkDown=true; 
//            
//             var top =  frmMain.document.getElementById(divID).style.top;
//             var left = frmMain.document.getElementById(divID).style.left; 
//             var posX = window.event.clientX + document.body.scrollLeft; 
//             var posY = window.event.clientY + document.body.scrollTop;
//            
//            offsetX = posX - parseInt(left);
//            offsetY = posY - parseInt(top);  
////            alert(parseInt(posX) + " " + parseInt(posY));          
//        }     
                
        function ShowDisplayValue()
        {           
             var row = gvData.getSelectedRow();
             var content = gvData.getCellText(row, 7);
             var split = content.split("&^&");
             var strContent=" ";
           
            if(split.length <= 1) return; 
             for(i=0;i<split.length;i++)
             {
                if(!SimpleJS.isNullOrEmpty(split[i]))
                    strContent += split[i] + " \r\n";            
             }
             txtDisplayValue.innerText = strContent;             
		    
		    
		     frmMain.document.getElementById(divID).style.left = GetPosX(310);
		     frmMain.document.getElementById(divID).style.top = GetPosY(210);
             frmMain.document.getElementById(divID).style.display = "";
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
    </script>

</asp:Content>
