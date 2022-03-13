<%@ Page Language="C#" MasterPageFile="~/Control/MasterBase.master" AutoEventWireup="true"
    CodeFile="UnderConstruction.aspx.cs" Inherits="Security_AccessDenied" Title="Untitled Page" %>

<%@ Import Namespace="PCCore" %>
<asp:Content ID="Content1" ContentPlaceHolderID="BaseContentPlaceHolder" runat="Server">
    <% 
        string appUrl = Config.AppBaseUrl;
        string themeUrl = Config.GetThemeBaseUrl(Page.Theme); 
    %>
    <div>
        <!--<table>
	    <tr>
	    <td>
	        <img src="<%= themeUrl %>/images/warninglarge.gif" />
	    </td>
	    <td>
	        <div class="ErrorTitle"><asp:Literal runat ="server"  Text="<%$ Resources:Labels,AccessDenied %>" />  </div>
	    </td>
	    </tr>
	    </table>		
		<br />
		<!--Sorry, you are not authorized for this page.-->
		<!--
		<asp:Literal runat ="server"   Text="<%$ Resources:Messages,NotAuthorized %>" /><br /><br />
		<!--Please contact your system administrator for furthur help.-->
		<!--
		<asp:Literal runat ="server" Text="<%$ Resources:Messages,PleaseContact %>" />
		<br /><br />
		-->
		
        <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center" valign="top" style="padding-top: 85px">
                    <table width="518" height="194" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="top" background="<%= themeUrl %>/images/AccessDenied.jpg" style="padding: 30px 0 0 105px">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td class="Errortitletxt" style="padding-bottom: 40px">
                                            <asp:Literal ID="Literal1" runat ="server"  Text="<%$ Resources:Labels,AccessDenied %>" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <p><asp:Literal ID="Literal2" runat ="server"   Text="<%$ Resources:Messages,NotAuthorized %>" /></p>
                                            <p><asp:Literal ID="Literal3" runat ="server"   Text="<%$ Resources:Messages,PleaseContact %>" /></p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
