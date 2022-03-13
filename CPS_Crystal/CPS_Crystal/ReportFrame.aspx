<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportFrame.aspx.vb" Inherits="CrystalWeb.ReportA" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=12.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head" runat="server">
    <title>Loading...</title>
</head>
<body topmargin=0 leftmargin=0>
    <form id="form2" runat="server">
    <div>
        <cr:crystalreportviewer id="CrystalReportViewer1" runat="server" 
            autodatabind="True" GroupTreeImagesFolderUrl="" Height="100%" 
            ReportSourceID="CrystalReport" ToolbarImagesFolderUrl="" ToolPanelWidth="300px" 
            Width="100%"></cr:crystalreportviewer>
        <CR:CrystalReportSource ID="CrystalReport" runat="server">
        </CR:CrystalReportSource>
    
    </div>
    </form>
</body>
</html>