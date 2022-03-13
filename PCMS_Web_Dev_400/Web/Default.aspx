<%@ Page Language="C#" MasterPageFile="~/Control/MasterBase.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Import Namespace="PCCore" %>
<asp:Content ID="Content2" ContentPlaceHolderID="BaseContentPlaceHolder" runat="Server">
    <% 
        string appUrl = Config.AppBaseUrl;
        string themeUrl = Config.GetThemeBaseUrl(Page.Theme); 
    %>
    <table width="100%" border="0" align="center" style="vertical-align: top" cellpadding="0" height="100%" cellspacing="0" bordercolor="#9cb4cd">
        <tr>
            <td width="182" valign="top" bgcolor="#636363" height="100%">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="Limg_L">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <% if (SessionInfo.CurrentLanguage.ToLower() == "en-us")
                                           { %>
                                        <img src="<%=themeUrl%>/images/Limg_login.jpg" width="180" height="59" />
                                        <%}
                                           else
                                           { %>
                                        <img src="<%=themeUrl%>/images/Limg_login_zh.jpg" width="180" height="59" />
                                        <%} %>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 0 5 0 5">
                                        <table width="170" height="100%" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td height="28" class="Llabel1">
                                                    <PC:Label ID="lblLoginName" runat="server" Text="<%$ Resources:Labels,LoginName %>" LabelStyle="xLabel" />
                                                </td>
                                                <td align="right" valign="middle">
                                                    <PC:TextBox ID="txtLoginName" runat="server" Required="False" ObjectId="10000" Style="width: 100px" RegisterClientVariable="true"></PC:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="28" class="Llabel1">
                                                    <PC:Label ID="lblPassword" runat="server" Text="<%$ Resources:Labels,Password %>" LabelStyle="xLabel" />
                                                </td>
                                                <td align="right" valign="middle">
                                                    <PC:TextBox ID="txtPassword" runat="server" TextMode="Password" Required="false" ObjectId="10000" ShowRequiredStar="false" Style="width: 100px" RegisterClientVariable="true" onkeyup="javascript: if(window.event.keyCode==13){Login();}">
                                                    </PC:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="28" class="Llabel1">
                                                    <PC:Label ID="Label13" runat="server" Text="<%$ Resources:Common,Database %>" LabelStyle="xLabel" />
                                                </td>
                                                <td align="right" valign="middle">
                                                    <PC:DropDownList ID="ddlDatabase" runat="server" Required="false" 
                                                        ObjectId="10000" ShowRequiredStar="false" Style="width: 100px" 
                                                        RegisterClientVariable="true" onchange="javascript: DatabaseChanged();" 
                                                        onselectedindexchanged="ddlDatabase_SelectedIndexChanged">
                                                    </PC:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="30" align="right" valign="top" style="padding-right: 5px">
                                        <input name="btnLogin" type="button" class="Lbtn" value="<%=Resources.Common.btnLogin %>" style="width: 58px" onclick="javascript:Login();return false;" />
                                        <input name="btnReset" type="reset" class="Lbtn" value="<%=Resources.Common.btnReset %>" style="width: 58px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="padding-right: 6px;">
                                        <a class="ALforgot" href="javascript:ForgotPassword();">
                                            <PC:Label ID="Label12" runat="server" Text="<%$ Resources:Labels,ForgotPassword %>" LabelStyle="xLabel" /></a>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="5" valign="middle">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trStaffSearch" style="display: none;">
                        <td class="Limg_L">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <% if (SessionInfo.CurrentLanguage.ToLower() == "en-us")
                                           { %>
                                        <img src="<%=themeUrl%>/images/Limg_search.jpg" width="180" height="59" />
                                        <%}
                                           else
                                           { %>
                                        <img src="<%=themeUrl%>/images/Limg_search_zh.jpg" width="180" height="59" />
                                        <%} %>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="36" align="center" valign="middle">
                                        <PC:TextBox ID="txtStaffSearch" runat="server" Required="False" ObjectId="10000" Style="width: 130px" RegisterClientVariable="true" onkeyup="javascript: if(window.event.keyCode==13){StaffSearch0();}"></PC:TextBox>
                                        <input name="btnSearch" type="button" class="Lbtn" value="<%=Resources.Common.btnSearch %>" style="width: 35px" onclick="javascript:StaffSearch0();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" height="25" style="padding-left: 6px">
                                        <%--<a href="javascript:AdvSearchStaff();" class="ALforgot">
                                            <PC:Label ID="lblAdvSearch" runat="server" Text="<%$ Resources:Labels,AdvancedSearch %>"
                                                Font-Size="8"></PC:Label></a>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td id="tdCustomerLogoLeft" runat="server" style="width:180px;">
                            <asp:Image ID="CustomerLogoLeft" ImageUrl="App_Themes/Source_Customerlogo_left.gif" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td id="tdCustomerLogoLeft2" runat="server" style="width:180px;">
                            <asp:Image ID="CustomerLogoLeft2" ImageUrl="App_Themes/Source_Customerlogo_left2.gif" runat="server" />
                        </td>
                    </tr>
                    
                </table>
            </td>
            <td align="center" width="100%" valign="top" style="padding: 20 35 0 35;" class="Lhomebg" height="100%">
                <div id="div_CHP" style="width: 100%;" class="DivGridstyle_gray">
                </div>
            </td>
            
            <td width="190px" valign="top" bgcolor="#959595" height="100%" style="background-color: #ffffff; " >
                <table width="100%" border="0" cellspacing="0" cellpadding="0" height="100%">
                    <tr>
                        <td width="190" valign="top" height="150">
                            <img src="<%=themeUrl%>/images/image04.jpg" width="190" height="150" />
                        </td>
                    </tr>
                    <tr>
                        <td class="homerowstd" id="UsefullLinkTd" runat="server" valign="top"  >
                           
                        </td>
                    </tr>
                    
                </table>
            </td>
        </tr>
        
    </table>

    <script language="javascript" type="text/javascript">
        var formUrl = "SearchResult.aspx";
        var dialogFeatures = "dialogHeight:250px;dialogWidth:950px;";
        var frmMain = document.forms[0];
        var sRSACheckValue;

        
//        var AddressInfo = AddressInfo();
//        
//        alert(AddressInfo.address);
//            
//        function AddressInfo()
//        {
//            var hostname = undefined;
//            var address = undefined;

//            try {
//            var sock = new java.net.Socket();
//            sock.bind(new java.net.InetSocketAddress('0.0.0.0', 0));
//            sock.connect(new java.net.InetSocketAddress(document.domain, (!document.location.port)?80:document.location.port));
//            hostname = sock.getLocalAddress().getHostName();
//            address = sock.getLocalAddress().getHostAddress();
//            } catch (e) {}

//            return {hostname: hostname, address: address};
//        }
        function checkRSA(pLoginName)
        {
            if (_Default.IsEnableRSASecurityChecking().value)
            {
                return window.showModalDialog("RSASecurity.aspx?loginName=" + pLoginName, "_blank", "dialogHeight:240px;dialogWidth:425px;scroll:off;help:off;status:off;");
            } else
            {
                return "Successfully";
            }
        }

        function Login()
        {
            var reg = /(\')/g;
            var uid = txtLoginName.value.replace(reg, "''");
            var pwd = txtPassword.value.replace(reg, "''");
            txtPassword.value = "";
            var db = ddlDatabase.value;
            var r = _Default.Login(uid, pwd, db);
            var sWinH;
            var sWinW;

            if (r.error != null)
            {
                alert(r.error.Message);
                return false;
            }
            else
            {
                var ret = r.value;
            }

            //var ret=_Default.Login(uid,pwd).value;
            //Init Wind X
            if (_Default.WinScrollHeightOrWidth("H").value > 0)
            {
                sWinH = _Default.WinScrollHeightOrWidth("H").value;
            }
            if (_Default.WinScrollHeightOrWidth("W").value > 0)
            {
                sWinW = _Default.WinScrollHeightOrWidth("W").value;
            }


            if (ret == "SystemUser")
            {
                sRSACheckValue = checkRSA(uid)
                if (sRSACheckValue != undefined && sRSACheckValue == "Successfully")
                {
                    //f_open_window_max("Admin_Default.aspx", "_blank", sWinW, sWinH);
                    //window.open('close.htm', '_self');
                    window.open('Admin_Default.aspx', '_self');
                }
            }
            else
            {
                if (ret == "null")
                {
                    window.document.location.reload();
                }
                else
                {
                    alert("<%=Resources.Messages.InvalidUserPassword %>");
                    txtLoginName.focus();
                    return false;
                }
            }
        }

        function StaffSearch0()
        {
            if (SimpleJS.isNullOrEmpty(txtStaffSearch.value))
            {
                alert("<%=Resources.Messages.InputStaffIDOrStaffName %>");
                txtStaffSearch.focus();
                return;
            }
            SelfStaffSearch(0, escape(txtStaffSearch.value));
        }

        function SelfStaffSearch(type, typevalue)
        {
            var search = "";
            var url = "";
            var p = {};

            if (SimpleJS.isNullOrEmpty(type)) return;
            if (SimpleJS.isNullOrEmpty(typevalue)) return;

            switch (type)
            {
                case 0:
                    url = formUrl + "?staff=" + typevalue;
                    break;
                case 1:
                    url = formUrl + "?Company=" + typevalue;
                    break;
                case 2:
                    url = formUrl + "?CostCentre=" + typevalue;
                    break;
                case 3:
                    url = formUrl + "?Division=" + typevalue;
                    break;
                case 4:
                    url = formUrl + "?Department=" + typevalue;
                    break;
                case 5:
                    url = formUrl + "?Grade=" + typevalue;
                    break;
                case 6:
                    url = formUrl + "?Position=" + typevalue;
                    break;
                case 7:
                    url = formUrl + "?Section=" + typevalue;
                    break;
                case 8:
                    url = formUrl + "?StaffType=" + typevalue;
                    break;
                case 9:
                    url = formUrl + "?Unit=" + typevalue;
                    break;
                case 10:
                    url = formUrl + "?WorkGroup=" + typevalue;
                    break;
                case 11:
                    url = formUrl + "?WorkingLocation=" + typevalue + "";
                    break;

            }
            var p = {};
            p[PARAM_URL] = url;
            return SimpleJS.showModalDialog(url, p, dialogFeatures);
        }

        function ShowLeaveDetail(type)
        {
            var url = "";

            if (SimpleJS.isNullOrEmpty(type)) return;

            switch (type)
            {
                case 0:
                    url = "StaffInformation/TodayLeave.aspx";
                    break;
                case 1:
                    url = "StaffInformation/PlanningLeave.aspx";
                    break;
                case 2:
                    url = "StaffInformation/TodayDutyTravel.aspx";
                    break;
                case 3:
                    url = "StaffInformation/PlanningDutyTravel.aspx";
                    break;

            }
            window.location.href(url);
        }

        function ForgotPassword()
        {
            window.location.href("<%=Config.AppBaseUrl %>/ForgotPassword.aspx");
        }

        function AdvSearchStaff()
        {
            window.location.href("<%=Config.AppBaseUrl %>/AdvancedSearch_StaffNotLogin.aspx");
        }

        function selfToBeExecute() {    
                
            div_CHP.insertAdjacentHTML("afterBegin", _Default.GetHtmlTable(ddlDatabase.value).value);
            runSetDisaplyStaffSearch();
            runSetDisaplyStaffInformation();

        }
        function runSetDisaplyStaffSearch()
        {
            if (_Default.IsShowStaffSarch(ddlDatabase.value).value)
            {
                trStaffSearch.style.display = "";
            } else
            {
                trStaffSearch.style.display = "none";
            }
        }
        function runSetDisaplyStaffInformation()
        {
           
        }
        function DatabaseChanged()
        {
            div_CHP.innerHTML = "";
            div_CHP.insertAdjacentHTML("afterBegin", _Default.GetHtmlTable(ddlDatabase.value).value);
            runSetDisaplyStaffSearch();
            runSetDisaplyStaffInformation();
            GetHoliday();
        }
        function ChangeCalendar()
        {
            var oldmonth = document.getElementById(txtCalendar0).value.substring(0, 7);
            var month = document.getElementById('txtCalendar').value.substring(0, 7);
            if (month == oldmonth)
            {
                //alert("==");
                return;
            }

            GetHoliday();
        }
        function GetHoliday()
        {
            var month = document.getElementById('txtCalendar').value.substring(0, 7);
            var db = ddlDatabase.value;
            var holiday = _Default.GetHoliday(db, month).value;

            var holidayHtml = "<table width='100%' border='0' cellspacing='0' cellpadding='0' >";
            var i;
            if (holiday != null)
            {
                //            for(i=0;i<holiday.Rows.length;i++)
                //            {            
                //                holidayHtml += "<tr>";
                //                holidayHtml += "<td width='45' height='18' valign='top' class='Ltxt2'><strong>"+ holiday.Rows[i]["date"] + "</strong></td>";
                //            
                //                holidayHtml += "<td valign='top' class='Ltxt2'>"+ holiday.Rows[i]["name"] + "</td>";
                //                holidayHtml += "</tr>";
                //            } 

                for (i = 0; i < holiday.Rows.length; i++)
                {
                    holidayHtml += "<tr>";
                    holidayHtml += "<td width='20' height='18' align='center' valign='top'><img src='<%=themeUrl%>/images/Ldot.gif' width='7' height='15'></td>";
                    holidayHtml += "<td valign='top' class='Ltxt2'>" + holiday.Rows[i]["clddate"] + " - " + holiday.Rows[i]["name"] + "</td>";
                    holidayHtml += "</tr>";
                }
            }
            holidayHtml += "</table>";
            document.getElementById(tdHoliday).innerHTML = holidayHtml;

            document.getElementById(txtCalendar0).value = document.getElementById('txtCalendar').value;
        }
    
    
    
    </script>

</asp:Content>
