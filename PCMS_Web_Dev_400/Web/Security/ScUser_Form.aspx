<%@ Page Language="C#" MasterPageFile="~/Control/MasterSecurityForm.master" AutoEventWireup="true"
    CodeFile="ScUser_Form.aspx.cs" Inherits="ScUser_Form" %>

<%@ MasterType VirtualPath="~/Control/MasterSecurityForm.master" %>
<%@ Import Namespace="PCCore" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <%
    
    int CheckboxCnt = 0;
    
    foreach (DataListItem dli in lsGroup.Items)
    {
        try
        {
            CheckBox _chk = (CheckBox)dli.FindControl("chkGroup");
            if (_chk != null)
            {
                CheckboxCnt += 1;
                //PCCore.Common.HRLog.RecordLog("Checkbox Client ID:" + _chk.ClientID);
                _chk.Attributes.Add("onclick","javascript:clickGroup('" + _chk.ClientID + "');");
            }
        }
        catch (Exception ex)
        {
            
        }
    }    
    
 %>
 <script language="javascript">
 
 var CheckBoxCnt = "<%=CheckboxCnt %>"; 
 function clickGroup(ctl)
 {
    var strID = "";
    var strKey = "";
    var chk;
    
    var ckl1 = document.getElementById(ctl);
    
    if (ckl1.checked)
    {
        for (var i=1;i<=CheckBoxCnt;i++)
        {
            
            if (i < 10)
            {
                strKey = "0" + i.toString();
            }
            if (i > 10 && i < 100)
            {
                strKey = i.toString();
            }
            strID = "ctl00_ContentPlaceHolder_lsGroup_ctl" + strKey + "_chkGroup";
            
            if (strID != ctl)
            {
                chk  = document.getElementById(strID);
                
                //chk.checked = false;
            }
        }
     }
 }
 
 </script>
    <table border="0" cellspacing="0" cellpadding="3" width="100%" height="375">        
        <tr>
            <td>
                <PC:Label LabelStyle="xLabel" runat="server" ID="lblLoginName" Text="<%$ Resources:Labels,LoginName %>" /></td>
            <td>
                :</td>
            <td>
                <PC:TextBox ID="txtLoginName" runat="server" CssClass="TextBoxForm" Required="True"
                    RequiredErrorMessage="<%$ Resources:Messages,InputLoginName %>" Text='<%# _dr["LOGINNAME"] %>' />
            </td>
            <td>
                <PC:Label LabelStyle="xLabel" runat="server" ID="lblPassword" Text="<%$ Resources:Labels,Password %>" /></td>
            <td>
                :</td>
          <td>
                <PC:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="TextBoxForm"
                    Required="True" RequiredErrorMessage="<%$ Resources:Messages,InputPassword %>" />
            </td>
        </tr>
        <tr>
            <%--<td>
                <PC:Label LabelStyle="xLabel" runat="server" ID="Label1" Text="<%$ Resources:Labels,AccessLevel %>" /></td>
            <td>
                :</td>
            <td>
                <PC:DropDownList runat="server" ID="ddlAccessLevel" Width="135" />
            </td>--%>
            <td>
                <PC:Label LabelStyle="xLabel" runat="server" ID="Label1" Text="<%$ Resources:Labels,FullName %>" /></td>
            <td>
                :</td>
            <td>
                <PC:TextBox ID="txtFullName" runat="server" CssClass="TextBoxForm" Text='<%# _dr["FULLNAME"] %>' />
            </td>
            <td>
                <PC:Label LabelStyle="xLabel" runat="server" ID="lblLocked" Text="<%$ Resources:Labels,Locked %>" />
                <asp:CheckBox ID="chkLocked" runat="server" />
            </td>
            <td>
            </td>
            <td>
                <PC:Label LabelStyle="xLabel" runat="server" ID="lblSupervisor" Text="<%$ Resources:Labels,Supervisor %>" />
                <asp:CheckBox ID="chkSupervisor" AutoPostBack="true" runat="server" 
                    oncheckedchanged="chkSupervisor_CheckedChanged" />
            </td>
        </tr>
        <!-- 20110420 -->
        <tr>
            <td>
                <PC:Label LabelStyle="xLabel" runat="server" ID="lblEmail" Text="<%$ Resources:Labels,EmailAddress %>" /></td>
            <td>
                :</td>
            <td>
                <PC:TextBox ID="txtEmail" runat="server" CssClass="TextBoxForm" Text='<%# _dr["EMAIL"] %>' />
            </td>
        </tr>
        <!-- 20110420 -->
        <tr>
            <td>
             <PC:Label ID="lblsapuser" Text="<%$ Resources:Labels,MapSAPUser %>" runat="server"></PC:Label>
            </td>
            <td>
            :
            </td>
            <td>
            <PC:TextBox ID="txtsapuser" CssClass="TextBoxForm" Width="100px" MaxLength="8" runat="server" Text='<%# _dr["sapuser"] %>'></PC:TextBox>
                
            </td>
            <!-- Martin update begin -->
             <td>
                <PC:Label LabelStyle="xLabel" runat="server" ID="Label4" Text="<%$ Resources:Labels,Reverse %>" />
                <asp:CheckBox ID="chkReverse" runat="server" />
            </td>
            <td>
                </td>
            <td>
                <PC:Label LabelStyle="xLabel" runat="server" ID="Label5" Text="<%$ Resources:Labels,BatchUpload %>" />
                <asp:CheckBox ID="chkBatch" runat="server" />
            </td>
            <!-- Martin update end -->
            
        </tr>
        <tr>
        <td>
             <PC:Label ID="Label6" Text="<%$ Resources:Labels,MapFlexUser %>" runat="server"></PC:Label>
            </td>
            <td>
            :
            </td>
            <td>
            <PC:TextBox ID="txtflexuser" CssClass="TextBoxForm" Width="100px" MaxLength="8" runat="server" Text='<%# _dr["FLEXUSER"] %>'  ></PC:TextBox>
            </td>
            <!-- Martin update begin -->
             <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <!-- Martin update end -->
            
        </tr>
        <tr>
             <td>
                <PC:Label LabelStyle="xLabel" runat="server" ID="Label3" Text="<%$ Resources:Labels,ExpireDate %>" /></td>
            <td>
                :</td>
            <td>
                <PC:TextBox ID="txtExpireDate" runat="server" DataType="Date" CssClass="TextBoxForm" />
            </td>
            <td>
                <asp:CheckBox ID="chkcopyuser" runat="server" AutoPostBack="true" 
                    Text="<%$ Resources:Labels,CopyRightsFrom %>" 
                    oncheckedchanged="chkcopyuser_CheckedChanged"  />
            </td>
            <td>:</td>
            <td>
                <PC:DropDownList ID="ddlsuser" Width="100%" runat="server"></PC:DropDownList>
            </td>
        </tr>        
        <tr style="display:none">
            <td>
                <PC:Label LabelStyle="xLabel" runat="server" ID="Label2" Text="<%$ Resources:Labels,Project %>" /></td>
            <td>
                :</td>
            <td height="50" colspan="4">
                <table cellpadding="0" cellspacing="0" border="0" width="100%" height="100%" style="border: solid 1 #999999;
                    background-color: White;">
                    <tr>
                        <td bgcolor="#dddddd">
                            <span style="width: 3;"></span>
                            
                            <PC:HrCheckBox ID="cbTotal" runat="server" RegisterClientVariable="true"  />
                            <PC:Label ID="lblall" runat="server" Text="<%$ Resources:Labels,All %>" /></td>
                    </tr>
                    <tr>
                        <td style="background-color: #999999; height: 1;">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="overflow: auto; height: 75;" class="DivCheckBox_se">
                                <asp:CheckBoxList runat="server" ID="cblProject">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr height="100%">
            <td>
                <PC:Label LabelStyle="xLabel" runat="server" ID="lblGroup" Text="<%$ Resources:Labels,Project %>" />
            </td>
            <td>
                :</td>
            <td colspan="4">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" height="100%">
                    <tr>
                        
                        <td valign="top" class="GridViewContainer">
                        <div style="overflow-x: hidden; overflow: scroll; width: 100%; height:200px" id="gridviewContainer">
                            <asp:DataList GridLines="Both"  ID="lsGroup" DataSourceID="dsGridView" DataKeyField="ID" ShowHeader="true" OnItemDataBound="lsGroup_ItemDataBound" runat="server">
                            <HeaderTemplate>
                                <table>
                                
                            </HeaderTemplate>
                            <ItemTemplate>
                            
                            <tr>
                            <td class="GridViewContainer" width="10%">
                                <asp:CheckBox ID="chkGroup"  runat="server" />
                                <PC:Label ID="lblID" width="0px" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ID")%>'></PC:Label>
                                <PC:Label ID="lblingroup" Width="0px" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ingroup")%>'></PC:Label>
                            </td>
                            <td class="GridViewContainer" width="30%">
                                <PC:Label ID="lbl1" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"groupcode")%>'></PC:Label>
                            </td>    
                            <td class="GridViewContainer" width="60%">
                                <PC:Label ID="lbl2" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"groupname")%>'></PC:Label>
                            </td>    
                            </tr>
                            
                            </ItemTemplate>
                            <FooterTemplate>
                            </table>
                            </FooterTemplate>
                            </asp:DataList>
                            <!--<PC:ListBox ID="lbGroup" Rows="10" DataSourceID="dsGridView" DataValueField="ID" DataTextField="group_1" SelectionMode="Single" runat="server">
                            </PC:ListBox>-->
                            </div>
                        </td>
                    </tr>
                    
                    <tr style="display:none">
                        <td valign="top" class="GridViewContainer">
                            <PC:DbDataSource runat="server" ID="dsGridView" AutoRestoreSelectCommand="False">
                            </PC:DbDataSource>
                            <PC:GridView runat="server" ID="gvData"  DataSourceID="dsGridView" ShowPageIndex="false"
                                SelectValueField="ID" AllowMultipleSelect="true"  MultipleSelectDataSourceField="INGROUP"
                                Width="460px" SkinID="NoPageDownSkin"  AllowSorting="false" ShowPager="false" ShowFooter="false" 
                                GridViewContainerStyle="height:137px" OnSelectedIndexChanged="gvData_SelectedIndexChanged" />
                                
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <PC:TextBox runat="server" CssClass="Invisible" ID="txtCount" RegisterClientVariable="true"></PC:TextBox>
                <PC:TextBox runat="server" CssClass="Invisible" ID="txtTotal" RegisterClientVariable="true"></PC:TextBox>
                <PC:TextBox runat="server" CssClass="Invisible" ID="txtProjectname" RegisterClientVariable="true"></PC:TextBox>
                <PC:TextBox runat="server" CssClass="Invisible" ID="txtMinPasswordLength" RegisterClientVariable="true"></PC:TextBox>
                <PC:TextBox ID="txtID" runat="server" CssClass="Invisible" Text='<%# _dr["ID"] %>' />
            </td>
        </tr>
    </table>

    <script language="javascript">
        var frmMain = document.forms[0];    	
    	var txtFName,txtUid,txtLName,txtPwd,txtHFName; 
        var count=0;
        
       function checkstatus(crl) {
           
            if(!SimpleJS.isNullOrEmpty(txtCount.value))
                count = txtCount.value - 0;
            
            if(crl.checked)
            {
                
                count = count + 1;
                txtCount.value = count;
            }
            else
            {
                count = count - 1;
                txtCount.value=count;
            }
            if(txtCount.value==txtTotal.value)
            {
                cbTotal.checked=true;
            }
            else
            {
                cbTotal.checked=false;
            }
        }

        function saveForm() {
            if (document.getElementById(txtPwd).value.length < 8) {
                alert("<%=Resources.Messages.MinPasswordLength %>" +" 8");
                document.getElementById(txtPwd).focus();
                return false;
            }
            return true;
        }
      
      
       
       
    </script>

</asp:Content>
