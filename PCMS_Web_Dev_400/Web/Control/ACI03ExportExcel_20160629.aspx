<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ACI03ExportExcel.aspx.cs" Inherits="Control_ACI03ExportExcel" UICulture="UK" Culture="en-GB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <table>
    <tr>
        <td>
            <asp:Label ID="Label1" runat="server" Text="Cost Code From"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="CostCodeFr" runat="server"></asp:TextBox>
        </td>
	</tr>
 	<tr>
        <td>
            <asp:Label ID="Label2" runat="server" Text="Cost Code To"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="CostCodeTo" runat="server"></asp:TextBox>
        </td>
	</tr>
	<tr>
        <td>
            <asp:Label ID="Label3" runat="server" Text="1 - Filter by W/D Date(Payment Cert.)/VOU Date(Direct Expense), 2-Filter by A/C Period"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="PrdChoice" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label4" runat="server" Text="Period From"></asp:Label>
        &nbsp;(YYYYMM)</td>
        <td>
            <asp:TextBox ID="PeriodFr" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label5" runat="server" Text="Period To"></asp:Label>
        &nbsp;(YYYYMM)</td>
        <td>
            <asp:TextBox ID="PeriodTo" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td></td>
        <td>
            <asp:Button ID="ExportButton" runat="server" Text="Export" 
                onclick="ExportButton_Click" />
            <asp:Label ID="Error_Label" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    </table>
    </form>
</body>
</html>
