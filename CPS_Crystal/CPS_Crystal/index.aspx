<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="index.aspx.vb" Inherits="CrystalWeb.index1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Crystal Report Selection</title>
    <link rel="stylesheet" type="text/css" href="style.css" />
</head>
<body>
    <form id="IndexForm" runat="server">
    <div>
        <asp:Table ID="Table1" runat="server">
        <asp:TableRow>
        <asp:TableCell><asp:Label  Width="150" ID="USER_Label" runat="server" Text="User ID"></asp:Label></asp:TableCell>
        <asp:TableCell><asp:DropDownList  width="200" ID="USER_DropDownList" runat="server" DataValueField="DataValueField" DataTextField="DataTextField">
            </asp:DropDownList></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
        <asp:TableCell><asp:Label  Width="150" ID="PRJ_Label" runat="server" Text="Project Code"></asp:Label></asp:TableCell>
        <asp:TableCell><asp:DropDownList  width="200" ID="PRJ_DropDownList" runat="server" DataValueField="DataValueField" DataTextField="DataTextField">
            </asp:DropDownList></asp:TableCell>
        </asp:TableRow>        
       <asp:TableRow>
        <asp:TableCell><asp:Label Width="150" ID="REPORT_Label" runat="server" Text="Report List"></asp:Label></asp:TableCell>
        <asp:TableCell><asp:ListBox height="350" width="400" ID="REPORT_ListBox" runat="server" DataValueField="DataValueField" DataTextField="DataTextField"></asp:ListBox></asp:TableCell>
        </asp:TableRow> 
        <asp:TableRow>
        <asp:TableCell>&nbsp;</asp:TableCell>
        <asp:TableCell><asp:Button ID="REPORT_Button" runat="server" Text="Load Report" /></asp:TableCell>
        </asp:TableRow> 
            
    </asp:Table>
    </div>  
    </form>
</body>
</html>
