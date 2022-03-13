<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CrystalReportsSelection.aspx.vb" Inherits="CrystalWeb.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Crystal Report Selection</title>
</head>
<body topmargin=0 leftmargin=0>
    <form id="form1" runat="server" height=100%>
    <div>
        <asp:ListBox AutoPostBack ID="ReportList" runat="server" Height="100%" Width="200px" DataValueField="Value" DataTextField="Description">
        </asp:ListBox>
    </div>
    </form>
</body>
</html>
