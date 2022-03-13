<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ctrDemo.aspx.cs" Inherits="ctrDemo" %>

<%@ Register Src="ctlTreel.ascx" TagName="ctlTreel" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;&nbsp;<uc1:ctlTreel ID="CtlTreel1" runat="server" />
    </div>
    </form>
</body>
</html>
