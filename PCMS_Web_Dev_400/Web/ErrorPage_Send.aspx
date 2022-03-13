<%@ Page Language="C#" MasterPageFile="~/Control/MasterForm.master" AutoEventWireup="true"
    CodeFile="ErrorPage_Send.aspx.cs" Inherits="ErrorPage_Send" %>

<%@ MasterType VirtualPath="~/Control/MasterForm.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table border="0" cellspacing="0" cellpadding="3" width="100%" height="100%">
        <tr style="display:none;">
            <td width="16%">
                <PC:Label ID="Label2" runat="server" Text="<%$ Resources:Labels,Receiptor %>" LabelStyle="xLabel" /></td>
            <td>
                :</td>
            <td width="84%">
                <PC:TextBox ID="txtReceiptor" runat="server" RegisterClientVariable="true" CssClass="TextBoxCode1" Width="550px"></PC:TextBox>
            </td>
        </tr>
        <tr>
            <td width="16%">
                <PC:Label ID="Label4" runat="server" Text="<%$ Resources:Labels,From %>" LabelStyle="xLabel" /></td>
            <td>
                :</td>
            <td width="84%">
                <PC:TextBox ID="txtFromName" runat="server" RegisterClientVariable="true" CssClass="TextBoxCode1" Width="550px"></PC:TextBox>
            </td>
        </tr>
        <tr>
            <td width="16%">
                <PC:Label ID="Label5" runat="server" Text="<%$ Resources:Labels,Email %>" LabelStyle="xLabel" /></td>
            <td>
                :</td>
            <td width="84%">
                <PC:TextBox ID="txtFromEmail" runat="server" RegisterClientVariable="true" CssClass="TextBoxCode1" Width="550px"></PC:TextBox>
            </td>
        </tr>
        <tr>
            <td width="16%">
                <PC:Label ID="Label1" runat="server" Text="<%$ Resources:Labels,Subject %>" LabelStyle="xLabel" /></td>
            <td>
                :</td>
            <td width="84%">
                <PC:TextBox ID="txtSubject" runat="server" RegisterClientVariable="true" CssClass="TextBoxCode1" Width="550px"></PC:TextBox>
            </td>
        </tr>
        <tr>
            <td width="16%">
                <PC:Label ID="Label3" runat="server" Text="<%$ Resources:Labels,Description %>" LabelStyle="xLabel" /></td>
            <td>
                :</td>
            <td width="84%">
                <PC:TextBox ID="txtBody" runat="server" RegisterClientVariable="true" TextMode="multiline" CssClass="TextBoxCode1" Wrap="false" Rows="10" Width="550px" Height="100px"></PC:TextBox>
                <PC:TextBox ID="txtError" runat="server" RegisterClientVariable="true" TextMode="multiline" CssClass="Invisible"></PC:TextBox>
            </td>
        </tr>
        
        <tr>
            <td width="16%">
                <PC:Label ID="Label6" runat="server" Text="<%$ Resources:Labels,ErrorInformation %>" LabelStyle="xLabel" /></td>
            <td>
                :</td>
            <td width="84%">
                <PC:TextBox ID="txtErrorInformation" runat="server" RegisterClientVariable="true" TextMode="multiline" CssClass="TextBoxCode1" Wrap="false" Rows="5" Width="550px" Height="100px" ReadOnly="true" ></PC:TextBox>                
            </td>
        </tr>
    </table>
    <div runat="server" id="divError" ></div>
    <script language="javascript">
        var frmMain = document.forms[0];
        var error=window.dialogArguments;        
        var divError;
        
        function selectFirst()
        {
            txtErrorInformation.value=error.innerText;
        }        
        function onSaveForm()
        {
            if(!SimpleJS.isEmail(txtFromEmail.value))
            {
                alert("<%=Resources.Messages.InputCorrectEmail %>");
                txtFromEmail.focus();
                return false;
            }
            txtError.value=escape(error.innerHTML);//encodeURI             
            //Note.setText("Sending,please wait...","",null);
            Note.Note.children[0].rows(0).cells[1].children[0].children[0].children[0].children[0].innerText="Sending, please wait...";
            
            return true;
        }     
    </script>

</asp:Content>
