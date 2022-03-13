<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ACI03ExportPDF.aspx.cs" Inherits="Control_ACI03ExportPDF" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AC03 Export PDF</title>
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
            <asp:Label ID="Label3" runat="server" Text="1 - Filter by W/D Date (Payment Cert.)/ VOU Date (Direct Expense), 2 - Filter by A/C Period"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="PrdChoice" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label4" runat="server" Text="Period From"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="PeriodFr" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label5" runat="server" Text="Period To"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="PeriodTo" runat="server"></asp:TextBox>
        </td>
    </tr>
        <tr>
        <td>
            <asp:Label ID="Label6" runat="server" Text="Transaction Type"></asp:Label>
            <br />
            <asp:Label ID="Label7" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;(A - All, C - Client Payment Cert, D - Direct Expenses,"></asp:Label>
            <br />
            <asp:Label ID="Label8" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;M - Supplier Payment Cert, S - Subcontractor Payment Cert)"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="TransType" runat="server"></asp:TextBox>
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
