<%@ Page Language="C#" MasterPageFile="~/Control/MasterSecurityForm.master" AutoEventWireup="true"
    CodeFile="ScUseFullLink_Form.aspx.cs" Inherits="ScUseFullLink_Form" %>

<%@ MasterType VirtualPath="~/Control/MasterSecurityForm.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <table width="100%" border="0" cellspacing="0" cellpadding="3">
        <tr>
            <td width="15%">
                <PC:Label LabelStyle="xLabel"  runat="server" ID="lblCode" Text="<%$ Resources:Labels,Name %>"  />
            </td>
            <td width="2%">
                :
            </td>
            <td>
                <PC:TextBox ID="txtName" runat="server" CssClass="TextBoxForm" Required="True" RequiredErrorMessage="<%$ Resources:Messages,InputName %>"
                    Text='<%# _dr["Name"] %>' Width="500px" MaxLength="350" />
            </td>
        </tr>
        <tr>
            <td width="15%">
                <PC:Label LabelStyle="xLabel"  runat="server" ID="lblName" Text="<%$ Resources:Labels,UrlAddress %>" /></td>
            <td width="2%">
                :
            </td>
            <td>
                <PC:TextBox ID="txtUrlAddress" runat="server" CssClass="TextBoxForm" Required="True" MaxLength="350"
                    Width="500px" RequiredErrorMessage="<%$ Resources:Messages,InputDescription %>"
                    Text='<%# _dr["UrlAddress"] %>' />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <PC:TextBox ID="txtID" runat="server" CssClass="Invisible" Text='<%# _dr["ID"] %>' />
            </td>
        </tr>
        <tr>
            <td width="15%">
                <PC:Label LabelStyle="xLabel"  runat="server" ID="Label1" Text="<%$ Resources:Labels,Priority %>" /></td>
            <td width="2%">
                :
            </td>
            <td>
               <PC:DropDownList ID="ddlPriority" runat="server" Width="100px" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <PC:TextBox ID="TextBox2" runat="server" CssClass="Invisible" Text='<%# _dr["ID"] %>' />
            </td>
        </tr>
        
    </table>
</asp:Content>
