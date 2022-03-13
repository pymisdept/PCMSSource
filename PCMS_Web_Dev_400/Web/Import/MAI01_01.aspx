<%@ Page Language="C#" MasterPageFile="~/Control/MasterCustomize.master" ValidateRequest="false"
    AutoEventWireup="true" CodeFile="MAI01_01.aspx.cs" Inherits="MAI01_01" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Control/PMMenu.ascx" TagName="PMMenu" TagPrefix="PC" %>
<%@ MasterType VirtualPath="~/Control/MasterCustomize.master" %>
<%@ Register Assembly="PCCore" Namespace="PCCore.Control.PmsReport" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" EnableViewState="true"
    runat="Server">
    <!-- banner begin -->
    <input type="hidden" id="_HRProc" runat="server" />
    <asp:ScriptManager ID="scmgr" runat="server">
    </asp:ScriptManager>
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <!-- search area begin -->
        <tr>
            <td style="width: 82%" >
                <asp:Label Width="500" ID="txtProject" Font-Bold="true" runat="server"></asp:Label> 
                 <asp:Label ID="txtDocEntry" Font-Bold="true" runat="server" Text="" Visible="False"></asp:Label>
                <asp:Label ID="txtCurrentTab" Font-Bold="true" runat="server" Text="1" Visible="False"></asp:Label>
                <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" />
                <asp:Button ID="btnPost" Text="Post" runat="server" OnClick="btnPost_Click" />              
            </td>
         </tr>
        <tr>
            <td style="width: 82%"><asp:Label Width="500" ID="txtProjectName" Font-Bold="true" runat="server"></asp:Label>
            <asp:Label ID="txtProjectStatus" ForeColor="Red" runat="server" Text=""></asp:Label>
           </td>
        </tr>
        <tr>
            <td style="width: 82%">
                <asp:Button Width="110" Height="50" ID="tab01_Visbtn" Text="1. Executive Summary"
                    runat="server" OnClick="tab01_Visbtn_Click" />
                <asp:Button Width="110" Height="50" ID="tab02_Visbtn" Text="2. Production Report"
                    runat="server" OnClick="tab02_Visbtn_Click" />
                <asp:Button Width="110" Height="50" ID="tab03_Visbtn" Text="3. Financial Report"
                    runat="server" OnClick="tab03_Visbtn_Click" />
                <asp:Button Width="110" Height="50" ID="tab04_Visbtn" Text="4. Manpower and Plant"
                    runat="server" OnClick="tab04_Visbtn_Click" />
                <asp:Button Width="110" Height="50" ID="tab05_Visbtn" Text="5. Safety, Health & Environmental Issues"
                    runat="server" OnClick="tab05_Visbtn_Click" />
                <asp:Button Width="110" Height="50" ID="tab06_Visbtn" Text="6. Quality Assurance Issues"
                    runat="server" OnClick="tab06_Visbtn_Click" />
                <asp:Button Width="100" Height="50" ID="tab07_Visbtn" Text="7. Photo Upload" runat="server"
                    OnClick="tab07_Visbtn_Click" />
            </td>
        </tr>
    </table>
    <table border="1">
        <tr>
            <td width="100%" height="100%">
                <table>
                    <tr>
                        <td width="100%">
                            <asp:Panel ID="Folder01" runat="server">
                                <asp:Panel ID="Tab01_folder" Visible="true" runat="server">
                                    <table width="800" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td width="770">
                                                1. Executive Summary
                                            </td>
                                            <td align="right">
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <asp:Button ID="Tab01_AddBtn" runat="server" Text="Add Row" OnClick="Tab01_AddBtn_Click" />
                                    <br />
                                    <asp:GridView ID="Tab01_01" runat="server" Width="800" AutoGenerateColumns="False"
                                        Style="margin-right: 0px" OnRowCommand="Tab01_01_RowCommand">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab01_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" TextMode="MultiLine" Height="45" Width="700" ID="Tab01_Detail"
                                                        CommandName="Tab01_01_02" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Detail") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" CommandName="Tab01_01_03"
                                                        runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                            <asp:Panel ID="Folder02" runat="server" Visible="False" runat="server">
                                2. Production Report<br />
                                <br />
                                <asp:Panel ID="Tab02_01_folder" Visible="true" runat="server">
                                    <table width="800" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td width="770">
                                                2.1 Project Particular
                                                <asp:Button ID="Tab02_01_AddBtn" runat="server" Text="Add Row" OnClick="Tab02_01_AddBtn_Click" />
                                            </td>
                                            <td align="right">
                                                <asp:ImageButton ID="Col_Tab02_01" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                    runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="Tab02_01" runat="server" Width="800" AutoGenerateColumns="False" OnRowCommand="Tab02_01_RowCommand"
                                        Style="margin-right: 0px">
                                        <HeaderStyle Height="0" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab02_01_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="200" ID="Tab02_01_Label" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TableLabel") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="300" ID="Tab02_01_Detail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TableContent") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="100" ID="Tab02_01_UOM" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UOM") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" CommandName="Tab02_01_03"
                                                        runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab02_01" runat="Server" TargetControlID="Tab02_01_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab02_01"
                                    CollapseControlID="Col_Tab02_01" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab02_01" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />
                                <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            2.2 Scope of Works
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab02_02" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="Tab02_02_folder" Visible="true" runat="server">
                                    <asp:TextBox Width="0" ID="Tab02_02_DocEntry" runat="server" />
                                    <asp:TextBox TextMode="MultiLine" Wrap="true" Height="60" Width="800" ID="Tab02_02_ScopeOfWork"
                                        runat="server" Text='Test' /><br />
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab02_02" runat="Server" TargetControlID="Tab02_02_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab02_02"
                                    CollapseControlID="Col_Tab02_02" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab02_02" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />
                                <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            2.3 Overall Progress Review
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab02_03" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="Tab02_03_folder" Visible="true" runat="server">
                                    <asp:Button ID="Tab02_03_AddBtn" runat="server" Text="Add Row" OnClick="Tab02_03_AddBtn_Click" /><br />
                                    <asp:GridView ID="Tab02_03" runat="server" Width="800" OnRowCommand="Tab02_03_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab02_03_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox TextMode="MultiLine" Wrap="true" Height="45" Width="700" CommandName="Tab02_03_02"
                                                        ID="Tab02_03_Detail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Detail") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" runat="server"
                                                        CommandName="Tab02_03_03" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab02_03" runat="Server" TargetControlID="Tab02_03_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab02_03"
                                    CollapseControlID="Col_Tab02_03" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab02_03" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />
                                <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            2.4 Progress Summary
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab02_04" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="Tab02_04_folder" Visible="true">
                                    <asp:Button ID="Tab02_04_AddBtn" runat="server" Text="Add Row" OnClick="Tab02_04_AddBtn_Click" /><br />
                                    <asp:GridView ID="Tab02_04" runat="server" Width="800" OnRowCommand="Tab02_04_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab02_04_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No.">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="30" ID="Tab02_04_LineNum" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNum") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Section / Location / Activity">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="60" ID="Tab02_04_Type" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Type") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Planned Commencement Date">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Date" Width="90" ID="Tab02_04_CommenceDate" runat="server"
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "CommenceDate") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Planned Completion Date">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Date" Width="90" ID="Tab02_04_CompletionDate" runat="server"
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "CompletionDate") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Contract Period">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="60" ID="Tab02_04_ContractPeriod" runat="server"
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "ContractPeriod") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="% of Completion">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="80" ID="Tab02_04_CompletionPercent" runat="server"
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "CompletionPercent") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Anticipated Completion Date">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Date" Width="90" ID="Tab02_04_ACompletionDate" runat="server"
                                                        Text='<%# DataBinder.Eval(Container.DataItem, "ACompletionDate") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Day of Ahead(+) / Delay(-)">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="80" ID="Tab02_04_Days" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Days") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" runat="server"
                                                        CommandName="Tab02_04_03" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab02_04" runat="Server" TargetControlID="Tab02_04_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab02_04"
                                    CollapseControlID="Col_Tab02_04" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab02_04" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />
                                <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            2.5 Instruction / Information Received
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab02_05" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="Tab02_05_folder" Visible="true">
                                    <asp:Button ID="Tab02_05_Addbtn" runat="server" Text="Add Row" OnClick="Tab02_05_AddBtn_Click" /><br />
                                    <asp:GridView ID="Tab02_05" runat="server" Width="800" OnRowCommand="Tab02_05_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab02_05_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No.">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="30" ID="Tab02_05_LineNum" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNum") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Type of Instruction">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="200" ID="Tab02_05_Type" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Type") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Cum. No. of Instruction Received">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="200" ID="Tab02_05_InRec" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InRec") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Cum. No. Received for Last Report">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="200" ID="Tab02_05_InRep" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InRep") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" runat="server"
                                                        CommandName="Tab02_05_03" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab02_05" runat="Server" TargetControlID="Tab02_05_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab02_05"
                                    CollapseControlID="Col_Tab02_05" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab02_05" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />
                                <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            2.6 Information Requested
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab02_06" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="Tab02_06_folder" Visible="true">
                                    <asp:Button ID="Tab02_06_AddBtn" runat="server" Text="Add Row" OnClick="Tab02_06_AddBtn_Click" /><br />
                                    <asp:GridView ID="Tab02_06" runat="server" Width="800" OnRowCommand="Tab02_06_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab02_06_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No.">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="30" ID="Tab02_06_LineNum" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNum") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Type of Information Requested">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="200" ID="Tab02_06_Type" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Type") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Cum. No. of Requisition Sent">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="200" ID="Tab02_06_InRec" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InRec") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Cum. No. Sent for Last Report">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="200" ID="Tab02_06_InRep" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InRep") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" runat="server"
                                                        CommandName="Tab02_06_03" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab02_06" runat="Server" TargetControlID="Tab02_06_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab02_06"
                                    CollapseControlID="Col_Tab02_06" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab02_06" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                            </asp:Panel>
                            <asp:Panel ID="Folder03" runat="server" Visible="False">
                                3. Financial Commercial Reports<br />
                                <br />
                                <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            3.1 Payment Application
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab03_01" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="Tab03_01_folder" Visible="true">
                                    <asp:GridView ID="Tab03_01" runat="server" Width="800" AutoGenerateColumns="False"
                                        Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab03_01_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Integer" Width="30" ID="Tab03_01_LineNum" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNum") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="400" ID="Tab03_01_Description" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="100" ID="Tab03_01_LastMonth" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LastMonth") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="100" ID="Tab03_01_ThisMonth" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ThisMonth") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="100" ID="Tab03_01_NextMonth" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NextMonth") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab03_01" runat="Server" TargetControlID="Tab03_01_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab03_01"
                                    CollapseControlID="Col_Tab03_01" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab03_01" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />
                                <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            3.2 Payment Certification
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab03_02" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="Tab03_02_folder" Visible="true">
                                    <asp:GridView ID="Tab03_02" runat="server" Width="800" AutoGenerateColumns="False"
                                        Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab03_02_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Double" Wrap="true" Width="30" ID="Tab03_02_LineNum" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNum") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="400" ID="Tab03_02_Description" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Last Month">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" ID="Tab03_02_LastMonth" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LastMonth") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="This Month">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="100" ID="Tab03_02_ThisMonth" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ThisMonth") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Next Month">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="100" ID="Tab03_02_NextMonth" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NextMonth") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <asp:Button ID="Tab03_A2_AddBtn" runat="server" Text="Add Row" OnClick="Tab03_A2_AddBtn_Click" /><br />
                                    <asp:GridView ID="Tab03_A2" runat="server" Width="800" OnRowCommand="Tab03_A2_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab03_A2_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox TextMode="MultiLine" Wrap="true" Height="45" Width="700" CommandName="Tab03_A2_02"
                                                        ID="Tab03_A2_Detail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Detail") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" runat="server"
                                                        CommandName="Tab03_A2_03" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab03_02" runat="Server" TargetControlID="Tab03_02_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab03_02"
                                    CollapseControlID="Col_Tab03_02" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab03_02" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />
                                <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            3.3 Variations Status
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab03_03" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="Tab03_03_Folder" Visible="true">
                                    <br />
                                    <asp:GridView ID="Tab03_03" runat="server" Width="800" AutoGenerateColumns="False"
                                        Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab03_03_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="30" ID="Tab03_03_LineNum" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNum") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="200" ID="Tab03_03_Description" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No.">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="200" ID="Tab03_03_No" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "No") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Approx. Amt. HK$'000">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="200" ID="Tab03_03_Amount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab03_03" runat="Server" TargetControlID="Tab03_03_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab03_03"
                                    CollapseControlID="Col_Tab03_03" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab03_03" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />
                                <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            3.4 EOT Claims Status
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab03_04" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="Tab03_04_Folder" Visible="true">
                                    <asp:Button ID="Tab03_04_AddBtn" runat="server" Text="Add Row" OnClick="Tab03_04_AddBtn_Click" /><br />
                                    <br />
                                    <asp:GridView ID="Tab03_04" runat="server" Width="800" OnRowCommand="Tab03_04_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab03_04_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No.">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="30" ID="Tab03_04_LineNum" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNum") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Section">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="50" ID="Tab03_04_Section" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Section") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Contract Period (days)">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="90" ID="Tab03_04_ContractPeriod" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ContractPeriod") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Anticipated Completion Period (days)">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="90" ID="Tab03_04_ACompletionPeriod" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ACompletionPeriod") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="EOT Required (days)">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="90" ID="Tab03_04_EOTRequired" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EOTRequired") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="EOT Submitted (days)">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="90" ID="Tab03_04_EOTSubmitted" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EOTSubmitted") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="EOT Granted (days)">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="90" ID="Tab03_04_EOTGranted" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EOTGranted") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Anticipated Further EOT Granted (days)">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="90" ID="Tab03_04_AEOTGranted" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AEOTGranted") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="LD Exposure (days)">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="90" ID="Tab03_04_LDExposure" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LDExposure") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" runat="server"
                                                        CommandName="Tab03_04_03" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <asp:TextBox Width="0" ID="Tab03_04_02_DocEntry" runat="server" />
                                    <asp:TextBox Wrap="true" Height="60" Width="800" ID="Tab03_04_02_Remark" runat="server" /><br />
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab03_04" runat="Server" TargetControlID="Tab03_04_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab03_04"
                                    CollapseControlID="Col_Tab03_04" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab03_04" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />
                                <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            3.5 Liquidated Damages
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab03_05" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="Tab03_05_Folder" Visible="true">
                                    <asp:Button ID="Tab03_05_AddBtn" runat="server" Text="Add Row" OnClick="Tab03_05_AddBtn_Click" /><br />
                                    <br />
                                    <asp:GridView ID="Tab03_05" runat="server" Width="800" OnRowCommand="Tab03_05_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab03_05_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No.">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="30" ID="Tab03_05_LineNum" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNum") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Section">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="50" ID="Tab03_05_Section" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Section") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Contract Period (days)">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Double" Width="90" ID="Tab03_05_LDDays" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LDDays") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Anticipated Completion Period (days)">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Double" Width="90" ID="Tab03_05_LDExposure" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LDExposure") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="EOT Required (days)">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Double" Width="90" ID="Tab03_05_LDPotential" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LDPotential") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="EOT Submitted (days)">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Double" Width="90" ID="Tab03_05_LDDeducted" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LDDeucted") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="EOT Granted (days)">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Double" Width="90" ID="Tab03_05_ARecoverable" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ARecoverable") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Anticipated Further EOT Granted (days)">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Double" Width="90" ID="Tab03_05_Potential" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Potential") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" runat="server"
                                                        CommandName="Tab03_05_03" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <asp:TextBox Width="0" ID="Tab03_05_02_DocEntry" runat="server" />
                                    <asp:TextBox Wrap="true" TextMode="MultiLine" Height="60" Width="800" ID="Tab03_05_02_Remark"
                                        runat="server" /><br />
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab03_05" runat="Server" TargetControlID="Tab03_05_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab03_05"
                                    CollapseControlID="Col_Tab03_05" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab03_05" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />
                                <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            3.6 Cost Claims Status
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab03_06" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="Tab03_06_Folder" Visible="true">
                                    <asp:GridView ID="Tab03_06" runat="server" Width="800" AutoGenerateColumns="False"
                                        Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab03_06_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="30" ID="Tab03_06_LineNum" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNum") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Section">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="350" ID="Tab03_06_Description" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No.">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="150" ID="Tab03_06_No" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "No") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Approx. Amt. HK$’000">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="150" ID="Tab03_06_Amt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amt") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <asp:TextBox Width="0" ID="Tab03_06_02_DocEntry" runat="server" />
                                    <asp:TextBox Wrap="true" TextMode="MultiLine" Height="60" Width="800" ID="Tab03_06_02_Remark"
                                        runat="server" /><br />
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab03_06" runat="Server" TargetControlID="Tab03_06_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab03_06"
                                    CollapseControlID="Col_Tab03_06" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab03_06" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                            </asp:Panel>
                            <asp:Panel ID="Folder04" runat="server" Visible="False">
                                4. Manpower and Plant
                                <asp:TextBox Wrap="true" Height="60" Width="800" ID="Tab04_02_02_Remark" runat="server" /><br />
                                <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            4.1. Monthly Manpower Report
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab04_01" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="Tab04_01_Folder" Visible="true">
                                    <asp:Button ID="Tab04_01_AddBtn" runat="server" Text="Add Row" OnClick="Tab04_01_AddBtn_Click" /><br />
                                    <br />
                                    <asp:GridView ID="Tab04_01" runat="server" Width="800" OnRowCommand="Tab04_01_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab04_01_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="30" ID="Tab04_01_LineNum" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNum") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Position">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="300" ID="Tab04_01_Position" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Position") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Planned">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="100" ID="Tab04_01_Planned" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Planned") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Actual">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="100" ID="Tab04_01_Actual" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Actual") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Differential">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="100" ID="Tab04_01_Differential" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Differential") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" runat="server"
                                                        CommandName="Tab04_01_03" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab04_01" runat="Server" TargetControlID="Tab04_01_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab04_01"
                                    CollapseControlID="Col_Tab04_01" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab04_01" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />
                                <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            4.2 Monthly Plant Report
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab04_02" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="Tab04_02_Folder" Visible="true">
                                    <asp:Button ID="Tab04_02_AddBtn" runat="server" Text="Add Row" OnClick="Tab04_02_AddBtn_Click" /><br />
                                    <br />
                                    <asp:GridView ID="Tab04_02" runat="server" Width="800" OnRowCommand="Tab04_02_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab04_02_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="30" ID="Tab04_02_LineNum" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNum") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Plant">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="300" ID="Tab04_02_Plant" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Plant") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Planned">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="100" ID="Tab04_02_Planned" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Planned") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Actual">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="100" ID="Tab04_02_Actual" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Actual") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Differential">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="100" ID="Tab04_02_Differential" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Differential") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" runat="server"
                                                        CommandName="Tab04_02_03" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <asp:TextBox Width="0" ID="Tab04_02_02_DocEntry" runat="server" />
                                    
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab04_02" runat="Server" TargetControlID="Tab04_02_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab04_02"
                                    CollapseControlID="Col_Tab04_02" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab04_02" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />
                            </asp:Panel>
                            <asp:Panel ID="Folder05" runat="server" Visible="False">
                                5. Safety, Health &amp; Environmental Issues<br />
                                <br />
                                <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            Monthly / Annual Accident Statistics
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab05_01" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="Tab05_01_Folder" Visible="true">
                                    <asp:GridView ID="Tab05_01" runat="server" Width="800" AutoGenerateColumns="False"
                                        Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab05_01_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="30" ID="Tab05_01_LineNum" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNum") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="300" ID="Tab05_01_Description" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="This Month">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="100" ID="Tab05_01_ThisMonth" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ThisMonth") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Accumulated">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="100" ID="Tab05_01_Accumulated" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Accumulated") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab05_01" runat="Server" TargetControlID="Tab05_01_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab05_01"
                                    CollapseControlID="Col_Tab05_01" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab05_01" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />
                                <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            Penalty &amp; Fine Status
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab05_02" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="Tab05_02_Folder" Visible="true">
                                    <asp:Button ID="Tab05_02_AddBtn" runat="server" Text="Add Row" OnClick="Tab05_02_AddBtn_Click" /><br />
                                    <br />
                                    <asp:GridView ID="Tab05_02" runat="server" Width="800" OnRowCommand="Tab05_02_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab05_02_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="30" ID="Tab05_02_LineNum" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNum") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="300" ID="Tab05_02_Description" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No. of Cases This Month">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Double" Width="100" ID="Tab05_02_No" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "No") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Amount in This Month (HK$)">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="100" ID="Tab05_02_Amt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amt") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Cum. No. of Cases">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Double" Width="100" ID="Tab05_02_CumNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CumNo") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Cum. Amount (HK$)">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="100" ID="Tab05_02_CumAmt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CumAmt") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" runat="server"
                                                        CommandName="Tab05_02_03" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <asp:TextBox Width="0" ID="Tab05_02_02_DocEntry" runat="server" />
                                    <asp:TextBox Wrap="true" Height="60" Width="800" ID="Tab05_02_02_Remark" runat="server" /><br />
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab05_02" runat="Server" TargetControlID="Tab05_02_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab05_02"
                                    CollapseControlID="Col_Tab05_02" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab05_02" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />
                                
                            </asp:Panel>
                            <asp:Panel ID="Folder06" runat="server" Visible="False">
                            <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            6. Quality Assurance Issues
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab06_01" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="Tab06_01_Folder" runat="server" Visible="TRUE">
                                    <asp:Button ID="Tab06_01_AddBtn" runat="server" Text="Add Row" OnClick="Tab06_01_AddBtn_Click" />
                                    <br />
                                    <asp:GridView ID="Tab06_01" runat="server" Width="800" OnRowCommand="Tab06_01_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab06_01_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" TextMode="MultiLine" Height="45" Width="700" ID="Tab06_Detail"
                                                        runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Detail") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" CommandName="Tab06_01_03"
                                                        runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab06_01" runat="Server" TargetControlID="Tab06_01_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab06_01"
                                    CollapseControlID="Col_Tab06_01" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab06_01" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />
                                <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            6.1 Summary NCRs Status
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab06_02" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="Tab06_02_Folder" Visible="true">
                                
                                    <asp:Button ID="Tab06_02_AddBtn" runat="server" Text="Add Row" OnClick="Tab06_02_AddBtn_Click" /><br />
                                    <br />
                                    <asp:GridView ID="Tab06_02" runat="server" Width="800" OnRowCommand="Tab06_02_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab06_02_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Type of Report">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="30" ID="Tab06_02_Type" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Type") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Raised This Period">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="300" ID="Tab06_02_R_Period" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "R_Period") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Raised To-date">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="70" ID="Tab06_02_R_ToDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "R_ToDate") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Closed This Period">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="70" ID="Tab06_02_C_Period" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "C_Period") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Closed To-date">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="70" ID="Tab06_02_C_ToDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "C_ToDate") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Outstanding This Period">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="70" ID="Tab06_02_O_Period" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "O_Period") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Outstanding To-date">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="String" Width="70" ID="Tab06_02_O_ToDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "O_ToDate") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" runat="server"
                                                        CommandName="Tab06_02_03" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab06_02" runat="Server" TargetControlID="Tab06_02_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab06_02"
                                    CollapseControlID="Col_Tab06_02" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab06_02" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />
                                <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            6.2 Method Statement Status
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab06_03" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="Tab06_03_Folder" Visible="true">
                                    <asp:GridView ID="Tab06_03" runat="server" Width="800"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab06_03_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No. of Submission">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="50" ID="Tab06_03_Num" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Num") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No Objection">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="50" ID="Tab06_03_NoObj" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NoObj") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No Objection Subject to Comment">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="50" ID="Tab06_03_NoObjSub" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NoObjSub") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Rejection">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="50" ID="Tab06_03_Rej" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Rej") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Not Yet Replied">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="50" ID="Tab06_03_NotYet" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NotYet") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Button ID="Tab06_A3_AddBtn" runat="server" Text="Add Row" OnClick="Tab06_A3_AddBtn_Click" /><br />
                                    <asp:GridView ID="Tab06_A3" runat="server" Width="800" OnRowCommand="Tab06_A3_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab06_A3_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Late Submission">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="70" ID="Tab06_A3_LS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LS") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Original Submission Date">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Date" Width="70" ID="Tab06_A3_OS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OS") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Revised Submission Date">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Date" Width="70" ID="Tab06_A3_RS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RS") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" runat="server"
                                                        CommandName="Tab06_A3_03" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Button ID="Tab06_B3_AddBtn" runat="server" Text="Add Row" OnClick="Tab06_B3_AddBtn_Click" /><br />
                                    <asp:GridView ID="Tab06_B3" runat="server" Width="800" OnRowCommand="Tab06_B3_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab06_B3_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Submission to be made in the coming period (From xx to xx)">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="70" ID="Tab06_B3_SP" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SP") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Planned Submission Date">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Date" Width="70" ID="Tab06_B3_PS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PS") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" runat="server"
                                                        CommandName="Tab06_B3_03" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab06_03" runat="Server" TargetControlID="Tab06_03_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab06_03"
                                    CollapseControlID="Col_Tab06_03" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab06_03" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />
                        <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            6.3 Design Submission Status
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab06_04" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="Tab06_04_Folder" Visible="true">
                                    <asp:GridView ID="Tab06_04" runat="server" Width="800"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab06_04_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No. of Submission">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="50" ID="Tab06_04_Num" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Num") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No Objection">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="50" ID="Tab06_04_NoObj" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NoObj") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No Objection Subject to Comment">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="50" ID="Tab06_04_NoObjSub" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NoObjSub") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Rejection">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="50" ID="Tab06_04_Rej" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Rej") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Not Yet Replied">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="50" ID="Tab06_04_NotYet" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NotYet") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Button ID="Tab06_A4_AddBtn" runat="server" Text="Add Row" OnClick="Tab06_A4_AddBtn_Click" /><br />
                                    <asp:GridView ID="Tab06_A4" runat="server" Width="800" OnRowCommand="Tab06_A4_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab06_A4_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Late Submission">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="70" ID="Tab06_A4_LS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LS") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Original Submission Date">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Date" Width="70" ID="Tab06_A4_OS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OS") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Revised Submission Date">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Date" Width="70" ID="Tab06_A4_RS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RS") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" runat="server"
                                                        CommandName="Tab06_A4_03" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Button ID="Tab06_B4_AddBtn" runat="server" Text="Add Row" OnClick="Tab06_B4_AddBtn_Click" /><br />
                                    <asp:GridView ID="Tab06_B4" runat="server" Width="800" OnRowCommand="Tab06_B4_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab06_B4_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Submission to be made in the coming period (From xx to xx)">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="70" ID="Tab06_B4_SP" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SP") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Planned Submission Date">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Date" Width="70" ID="Tab06_B4_PS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PS") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" runat="server"
                                                        CommandName="Tab06_B4_03" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab06_04" runat="Server" TargetControlID="Tab06_04_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab06_04"
                                    CollapseControlID="Col_Tab06_04" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab06_04" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />
                                <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            6.4 Material Submission Status
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab06_05" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="Tab06_05_Folder" Visible="true">
                                    <asp:GridView ID="Tab06_05" runat="server" Width="800"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab06_05_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No. of Submission">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="50" ID="Tab06_05_Num" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Num") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No Objection">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="50" ID="Tab06_05_NoObj" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NoObj") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No Objection Subject to Comment">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="50" ID="Tab06_05_NoObjSub" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NoObjSub") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Rejection">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="50" ID="Tab06_05_Rej" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Rej") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Not Yet Replied">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="50" ID="Tab06_05_NotYet" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NotYet") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Button ID="Tab06_A5_AddBtn" runat="server" Text="Add Row" OnClick="Tab06_A5_AddBtn_Click" /><br />
                                    <asp:GridView ID="Tab06_A5" runat="server" Width="800" OnRowCommand="Tab06_A5_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab06_A5_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Late Submission">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="70" ID="Tab06_A5_LS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LS") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Original Submission Date">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Date" Width="70" ID="Tab06_A5_OS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OS") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Revised Submission Date">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Date" Width="70" ID="Tab06_A5_RS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RS") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" runat="server"
                                                        CommandName="Tab06_A5_03" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Button ID="Tab06_B5_AddBtn" runat="server" Text="Add Row" OnClick="Tab06_B5_AddBtn_Click" /><br />
                                    <asp:GridView ID="Tab06_B5" runat="server" Width="800" OnRowCommand="Tab06_B5_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab06_B5_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Submission to be made in the coming period (From xx to xx)">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="70" ID="Tab06_B5_SP" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SP") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Planned Submission Date">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Date" Width="70" ID="Tab06_B5_PS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PS") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" runat="server"
                                                        CommandName="Tab06_B5_03" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab06_05" runat="Server" TargetControlID="Tab06_05_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab06_05"
                                    CollapseControlID="Col_Tab06_05" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab06_05" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />   
                                <table width="800" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="770">
                                            6.5 As-built Drawing Submission Status
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="Col_Tab06_06" ImageUrl="../App_Themes/Default/images/collapse_expend.jpg"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel runat="server" ID="Tab06_06_Folder" Visible="true">
                                    <asp:GridView ID="Tab06_06" runat="server" Width="800"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab06_06_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No. of Submission">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="50" ID="Tab06_06_Num" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Num") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No Objection">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="50" ID="Tab06_06_NoObj" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NoObj") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="No Objection Subject to Comment">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="50" ID="Tab06_06_NoObjSub" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NoObjSub") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Rejection">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="50" ID="Tab06_06_Rej" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Rej") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Not Yet Replied">
                                                <ItemTemplate>
                                                    <asp:TextBox Width="50" ID="Tab06_06_NotYet" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NotYet") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Button ID="Tab06_A6_AddBtn" runat="server" Text="Add Row" OnClick="Tab06_A6_AddBtn_Click" /><br />
                                    <asp:GridView ID="Tab06_A6" runat="server" Width="800" OnRowCommand="Tab06_A6_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab06_A6_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Late Submission">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="70" ID="Tab06_A6_LS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LS") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Original Submission Date">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Date" Width="70" ID="Tab06_A6_OS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OS") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Revised Submission Date">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Date" Width="70" ID="Tab06_A6_RS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RS") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" runat="server"
                                                        CommandName="Tab06_A6_03" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Button ID="Tab06_B6_AddBtn" runat="server" Text="Add Row" OnClick="Tab06_B6_AddBtn_Click" /><br />
                                    <asp:GridView ID="Tab06_B6" runat="server" Width="800" OnRowCommand="Tab06_B6_RowCommand"
                                        AutoGenerateColumns="False" Style="margin-right: 0px">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:TextBox Width="0" ID="Tab06_B6_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Submission to be made in the coming period (From xx to xx)">
                                                <ItemTemplate>
                                                    <asp:TextBox Wrap="true" Width="70" ID="Tab06_B6_SP" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SP") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Planned Submission Date">
                                                <ItemTemplate>
                                                    <PC:TextBox DataType="Date" Width="70" ID="Tab06_B6_PS" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PS") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" runat="server"
                                                        CommandName="Tab06_B6_03" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    
                                    <br />
                                </asp:Panel>
                                <cc1:CollapsiblePanelExtender ID="Panel_Col_Tab06_06" runat="Server" TargetControlID="Tab06_06_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab06_06"
                                    CollapseControlID="Col_Tab06_06" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab06_06" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                                <br />     
                            </asp:Panel>
                            <asp:Panel ID="Folder07" runat="server" Visible="false">
                                <asp:FileUpload ID="FileUpload" runat="server" Width="350" />
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                                <asp:GridView ID="Tab07_01" runat="server" Width="800" OnRowCommand="Tab07_01_RowCommand"
                                    AutoGenerateColumns="False" Style="margin-right: 0px">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:TextBox Width="0" ID="Tab07_01_DocEntry" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocEntry") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:Image ID="Tab07_01_Image" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "FileName")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:TextBox Width="0" ID="Tab07_01_File" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FileName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Delete" ImageUrl="../App_Themes/Default/images/delete.gif" CommandName="Tab07_01_03"
                                                    runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                            <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="Server" TargetControlID="Tab06_01_folder"
                                    CollapsedSize="0" ExpandedSize="1" Collapsed="True" ExpandControlID="Col_Tab06_01"
                                    CollapseControlID="Col_Tab06_01" AutoCollapse="False" AutoExpand="False" ScrollContents="True"
                                    ImageControlID="Col_Tab06_01" ExpandedImage="../App_Themes/Default/images/collapse_expend.jpg"
                                    CollapsedImage="../App_Themes/Default/images/collapse_Close.jpg" ExpandDirection="Vertical" />
                        </td>
                    </tr>
                    <tr>
                        <td width="100%">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
