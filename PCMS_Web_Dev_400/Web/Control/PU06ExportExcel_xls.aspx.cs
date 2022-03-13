using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Text;

using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;

using SimpleControls;
using SimpleControls.Web;
using System.Reflection;

public partial class Control_PU06ExportExcel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (String.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["SAP2"].ConnectionString) == true)
            {
                Error_Label.Text = "SAP2 can not be blank in configuration file";
                return;
            }

            String sqlstr;
            int CurRow=1;
            int StartDataRow=3;
            int LastDataRow;
            int CCSummaryStartRow;
            int TotalCol=13;
            int InvAmtCol=15;
            int CostCodeCol=18;
            int GrandTotRow;
            int CCSummaryTotRow;

            sqlstr = "select prjcode as [Project Code], prjname as [Project Name], MRNum as [SMR No.], [SMR Line No.], REPLACE(CONVERT(VARCHAR(9), MRDate, 6), ' ', '-') as [SMR Date], ";
            sqlstr += "ponum as [PO No.], CardName as [Supplier Name], ' ' as dummy, [Description], Unit, Quantity as Qty, rate as [Unit Rate], polinetotal as [Total], ";
            sqlstr += "supinvnum as [Invoice No.], certlinetotal as [Invoice Amt], noofpayment as [Pay'tNo.], certnum as [CertNo.], ";
            sqlstr += "case when costcode is null then '-' when costcode='' then '-' else costcode end as [Cost Code], details as Remarks, getdate() as CurrentDate ";
            sqlstr += "from PU06_Data(" + Request.QueryString["ProjectCode"] + "," + Request.QueryString["ProjectCode"] + ") order by mrnum, mrdate, 'smr line no.'";

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                Error_Label.Text = "EXCEL could not be started. Check that your office installation and project references are correct.";
                return;
            }
            xlApp.Visible = false;

            Workbook wb = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet ws = (Worksheet)wb.Worksheets[1];

            if (ws == null)
            {
                Error_Label.Text = "Worksheet could not be created. Check that your office installation and project references are correct.";
            }

            xlApp.ActiveWindow.Zoom = 75;
            ws.get_Range(ws.Cells, ws.Cells).Font.Name = "Times New Roman";
            ws.get_Range(ws.Cells, ws.Cells).Font.Size = 12;
            ws.Cells[1, 1] = "MATERIAL ORDER LIST (SMR) in HK$";
            ws.get_Range(ws.Cells[1, 1], ws.Cells[1, 1]).Font.Bold = true;
            ws.get_Range(ws.Cells[1, 1], ws.Cells[1, 1]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);

            ws.Cells[1, 9] = "Print Date:";
            ((Range)ws.Cells[1, 9]).HorizontalAlignment = XlHAlign.xlHAlignRight;
            ws.get_Range(ws.Cells[1, 9], ws.Cells[1, 10]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);

            CurRow = StartDataRow - 1;

            string _connectionString = ConfigurationManager.ConnectionStrings["SAP2"].ConnectionString.ToString();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sqlstr, conn);
                    da.SelectCommand.CommandTimeout = 180;
                    System.Data.DataTable dt = new System.Data.DataTable();
                    da.Fill(dt);
                    DataColumnCollection dcCollection = dt.Columns;
                    for (int i = 1; i < dt.Rows.Count + 1; i++)
                    {

                        if (i == 1)
                        {
                            ws.Cells[1, 10] = dt.Rows[0]["CurrentDate"].ToString();
                        }

                        for (int j = 1; j < dt.Columns.Count; j++)
                        {

                            if (i == 1)
                            {
                                xlApp.Cells[CurRow, j] = dcCollection[j - 1].ToString();
                                xlApp.Cells[CurRow + 1, j] = dt.Rows[i - 1][j - 1].ToString();

                                ws.get_Range(ws.Cells[CurRow, j], ws.Cells[CurRow, j]).Font.Bold = true;
                                ws.get_Range(ws.Cells[CurRow, j], ws.Cells[CurRow, j]).Font.Underline = true;
                                ws.get_Range(ws.Cells[CurRow, j], ws.Cells[CurRow, j]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                            }
                            else

                                xlApp.Cells[CurRow + 1, j] = dt.Rows[i - 1][j - 1].ToString();
                        }
                        CurRow++;
                    }

                    LastDataRow = CurRow;
                    ws.get_Range(ws.Cells[CurRow+1, 11], ws.Cells[CurRow+8, 14]).Font.Bold = true;
                    CurRow=CurRow + 2;
                    GrandTotRow = CurRow;
                    ws.Cells[CurRow, TotalCol-1] = "Grand Total:";
                    ((Range)ws.Cells[CurRow, TotalCol - 1]).HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ws.get_Range(ws.Cells[CurRow, TotalCol], ws.Cells[CurRow, TotalCol]).Formula = "=+SUM(M" + StartDataRow + ":M" + LastDataRow + ")";
                    ((Range)ws.Cells[CurRow, TotalCol]).Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                    ((Range)ws.Cells[CurRow, TotalCol]).Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlDouble;
                    ws.get_Range(ws.Cells[CurRow, InvAmtCol], ws.Cells[CurRow, InvAmtCol]).Formula = "=+SUM(O" + StartDataRow + ":O" + LastDataRow + ")";
                    ((Range)ws.Cells[CurRow, InvAmtCol]).Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                    ((Range)ws.Cells[CurRow, InvAmtCol]).Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlDouble;
                    CurRow = CurRow + 3;
                    ws.Cells[CurRow, TotalCol-1] = "Filtered Total:";
                    ((Range)ws.Cells[CurRow, TotalCol - 1]).HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ws.get_Range(ws.Cells[CurRow, TotalCol], ws.Cells[CurRow, TotalCol]).Formula = "=+SUBTOTAL(9,M" + StartDataRow + ":M" + LastDataRow + ")";
                    ((Range)ws.Cells[CurRow, TotalCol]).Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                    ((Range)ws.Cells[CurRow, TotalCol]).Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;
                    ws.get_Range(ws.Cells[CurRow, InvAmtCol], ws.Cells[CurRow, InvAmtCol]).Formula = "=+SUBTOTAL(9,O" + StartDataRow + ":O" + LastDataRow + ")";
                    ((Range)ws.Cells[CurRow, InvAmtCol]).Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                    ((Range)ws.Cells[CurRow, InvAmtCol]).Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;
                    CurRow = CurRow + 3;
                    ws.Cells[CurRow, TotalCol] = "Cost Code Summary  :";
                    CurRow = CurRow + 1;
                    CCSummaryStartRow = CurRow;

                    //Cost Code Summary
                    sqlstr = "select distinct case when costcode is null then '-' when costcode='' then '-' else costcode end as costcode from PU06_Data(" + Request.QueryString["ProjectCode"] + "," + Request.QueryString["ProjectCode"] + ") order by costcode";
                    da = new SqlDataAdapter(sqlstr, conn);
                    da.SelectCommand.CommandTimeout = 180;
                    dt = new System.Data.DataTable();
                    da.Fill(dt);
                    for (int i = 1; i < dt.Rows.Count + 1; i++)
                    {
                        xlApp.Cells[CurRow, CostCodeCol] = dt.Rows[i - 1][0].ToString();
                        ws.get_Range(ws.Cells[CurRow, TotalCol], ws.Cells[CurRow, TotalCol]).Formula = "=+SUMIF(R$" + StartDataRow + ":R$" + LastDataRow + ",R"+CurRow+",M$"+StartDataRow+":M$"+LastDataRow+ ")";
                        ws.get_Range(ws.Cells[CurRow, InvAmtCol], ws.Cells[CurRow, InvAmtCol]).Formula = "=+SUMIF(R$" + StartDataRow + ":R$" + LastDataRow + ",R" + CurRow + ",O$" + StartDataRow + ":O$" + LastDataRow + ")";
                        CurRow++;
                    }
                    CCSummaryTotRow = CurRow + 1;
                    ws.get_Range(ws.Cells[CurRow+1, TotalCol], ws.Cells[CurRow+1, TotalCol]).Formula = "=+SUM(M" + CCSummaryStartRow + ":M" + CurRow + ")";
                    ((Range)ws.Cells[CurRow+1, TotalCol]).Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                    ((Range)ws.Cells[CurRow+1, TotalCol]).Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlDouble;
                    ws.get_Range(ws.Cells[CurRow+1, InvAmtCol], ws.Cells[CurRow+1, InvAmtCol]).Formula = "=+SUM(O" + CCSummaryStartRow + ":O" + CurRow + ")";
                    ((Range)ws.Cells[CurRow + 1, InvAmtCol]).Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                    ((Range)ws.Cells[CurRow + 1, InvAmtCol]).Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlDouble;
                    CurRow = CurRow + 3;
                    ws.get_Range(ws.Cells[CurRow, TotalCol], ws.Cells[CurRow, TotalCol]).Formula = "=+M" + CCSummaryTotRow + "-M" + GrandTotRow;
                    ws.get_Range(ws.Cells[CurRow, InvAmtCol], ws.Cells[CurRow, InvAmtCol]).Formula = "=+O" + CCSummaryTotRow + "-O" + GrandTotRow;
                    ws.get_Range(ws.Cells[CCSummaryStartRow, TotalCol], ws.Cells[CurRow, CostCodeCol]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(255,255,153));

                    //change format
                    ws.Cells[2, 8] = "";
                    ((Range)ws.Cells[1, 5]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    ((Range)ws.Cells[1, 5]).EntireColumn.NumberFormat = "dd-mmm-yy";
                    ((Range)ws.Cells[1, 10]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 10]).HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    ws.get_Range(ws.Cells[1, 10], ws.Cells[1, 11]).Merge(Type.Missing);
                    ((Range)ws.Cells[1, 11]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 12]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 13]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 13]).EntireColumn.NumberFormat = "#,##0.00";
                    ((Range)ws.Cells[1, 14]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    ((Range)ws.Cells[1, 15]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 15]).EntireColumn.NumberFormat = "#,##0.00";

                    //auto filter
                    ws.get_Range(ws.Cells[StartDataRow - 1, 1], ws.Cells[LastDataRow, 18]).AutoFilter(1, Missing.Value, XlAutoFilterOperator.xlAnd, Missing.Value, true);

                    //freeze window
                    xlApp.ActiveWindow.SplitRow = 2;
                    xlApp.ActiveWindow.SplitColumn = 7;
                    xlApp.ActiveWindow.FreezePanes = true;

                    //set row height
                    ((Range)ws.Cells[1, 1]).EntireRow.RowHeight = 28.5;
                    ((Range)ws.Cells[1, 1]).EntireRow.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    ((Range)ws.Cells[2, 1]).EntireRow.RowHeight = 36.5;
                    ((Range)ws.Cells[2, 1]).EntireRow.VerticalAlignment = XlVAlign.xlVAlignTop;

                    //set column width
                    ((Range)ws.Cells[1, 1]).EntireColumn.ColumnWidth = 13;
                    ((Range)ws.Cells[1, 2]).EntireColumn.ColumnWidth = 0;
                    ((Range)ws.Cells[1, 3]).EntireColumn.ColumnWidth = 14;
                    ((Range)ws.Cells[1, 4]).EntireColumn.ColumnWidth = 0;
                    ((Range)ws.Cells[1, 5]).EntireColumn.ColumnWidth = 11;
                    ((Range)ws.Cells[1, 6]).EntireColumn.ColumnWidth = 17;
                    ((Range)ws.Cells[1, 7]).EntireColumn.ColumnWidth = 26;
                    ((Range)ws.Cells[1, 8]).EntireColumn.ColumnWidth = 0;
                    ((Range)ws.Cells[1, 9]).EntireColumn.ColumnWidth = 30;
                    ((Range)ws.Cells[1, 10]).EntireColumn.ColumnWidth = 7;
                    ((Range)ws.Cells[1, 11]).EntireColumn.ColumnWidth = 10;
                    ((Range)ws.Cells[1, 12]).EntireColumn.ColumnWidth = 11.5;
                    ((Range)ws.Cells[1, 13]).EntireColumn.ColumnWidth = 16;
                    ((Range)ws.Cells[1, 14]).EntireColumn.ColumnWidth = 16;
                    ((Range)ws.Cells[1, 15]).EntireColumn.ColumnWidth = 16;
                    ((Range)ws.Cells[1, 16]).EntireColumn.ColumnWidth = 8;
                    ((Range)ws.Cells[1, 17]).EntireColumn.ColumnWidth = 8;
                    ((Range)ws.Cells[1, 18]).EntireColumn.ColumnWidth = 12;
                    ((Range)ws.Cells[1, 19]).EntireColumn.ColumnWidth = 15;

                    //seek project code
                    sqlstr = "select prjcode from oprj where u_docentry='" + Request.QueryString["ProjectCode"] + "'";
                    da = new SqlDataAdapter(sqlstr, conn);
                    dt = new System.Data.DataTable();
                    da.Fill(dt);
                    ws.Name = dt.Rows[0]["prjcode"].ToString();
                    
                    wb.Saved = true;

                    Random nRandom = new Random(DateTime.Now.Millisecond);

                    string fname = Server.MapPath("..\\Temp\\PU06_" + nRandom.Next().ToString() + ".xls");
                    if (System.IO.File.Exists(fname))
                    {
                        System.IO.File.Delete(fname);
                        wb.SaveCopyAs(fname);
                    }
                    else
                        wb.SaveCopyAs(fname);

                    wb.Close(null, null, null);

                    FileInfo fileDet = new System.IO.FileInfo(fname);
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileDet.Name));
                    Response.AddHeader("Content-Length", fileDet.Length.ToString());
                    Response.ContentType = "application/ms-excel";
                    Response.WriteFile(fileDet.FullName);
                    Response.End();

                }
                catch (Exception ex)
                {
                    Error_Label.Text = ex.Message.ToString();
                    conn.Close();
                }
                finally
                {
                    xlApp.Workbooks.Close();
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(wb);
                    xlApp.Quit();
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(xlApp);
                    ws = null;
                    wb = null;
                    xlApp = null;
                    GC.Collect();
                }
            }
        }
    }
}
