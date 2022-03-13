<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CrystalFrame.aspx.vb" Inherits="CrystalWeb.CrystalFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Crystal Report Web ProtoType (Only for Testing)</title>
</head>
	<frameset cols="200,*" border="0" frameSpacing="0" frameBorder="0">
		<frame name="F1R1" src="CrystalReportsSelection.aspx" frameBorder="no" noResize scrolling=no>
		<frame name="F1R2" src="ReportFrame.aspx" frameBorder="no">
	</frameset>
</html>
