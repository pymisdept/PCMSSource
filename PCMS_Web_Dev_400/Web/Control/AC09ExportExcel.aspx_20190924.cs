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
using PCCore;

using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;

using SimpleControls;
using SimpleControls.Web;
using System.Reflection;

public partial class Control_QS48ExportExcel : System.Web.UI.Page
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

            int CurRow = 1;
            //int CurCol = 1;
            int StartDataRow=5;
            int LastDataRow;
            
            bool hasRow = false;
            DateTime rptPeriod = Convert.ToDateTime(Request.QueryString["rPeriod"]);
            string projCode = Request.QueryString["Proj"] == "-2" ? "" : Request.QueryString["Proj"];
            string ledgerCode = Request.QueryString["Ledger"] == "-2" ? "" : Request.QueryString["Ledger"];

            string userid = SessionInfo.UserId;
            string username = SessionInfo.LoginName;
            string displayname = Request.QueryString["UserName"];
            string isS = SessionInfo.IsSupervisor ? "Y" : "N";
            string sqlstr2; 
            string _connectionString = ConfigurationManager.ConnectionStrings["SAP2"].ConnectionString.ToString();
            
            Microsoft.Office.Interop.Excel.Application xlApp = null;
            Workbook wb = null;
            Worksheet ws = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();

                    if (projCode != "")            // Check by Project
                    {
                        sqlstr2 = string.Format("EXEC sp_getPRSummary '{0}','{1}','{2}','{3}','{4}'", projCode, "P", userid, isS, rptPeriod.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        sqlstr2 = string.Format("EXEC sp_getPRSummary '{0}','{1}','{2}','{3}','{4}'", ledgerCode, "L", userid, isS, rptPeriod.ToString("yyyy-MM-dd"));
                    }
                    
                    SqlDataAdapter da = new SqlDataAdapter(sqlstr2, conn);
                    da.SelectCommand.CommandTimeout = 300;
                    System.Data.DataTable dt = new System.Data.DataTable();
                    
                    da.Fill(dt);

                    DataColumnCollection dcCollection = dt.Columns;

                    xlApp = new Microsoft.Office.Interop.Excel.Application();

                    if (xlApp == null)
                    {
                        Error_Label.Text = "EXCEL could not be started. Check that your office installation and project references are correct.";
                        return;
                    }
                    xlApp.Visible = false;

                    wb = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                    ws = (Worksheet)wb.Worksheets[1];

                    if (ws == null)
                    {
                        Error_Label.Text = "Worksheet could not be created. Check that your office installation and project references are correct.";
                    }

                    xlApp.ActiveWindow.Zoom = 75;
                    ws.get_Range(ws.Cells, ws.Cells).Font.Name = "Times New Roman";
                    ws.get_Range(ws.Cells, ws.Cells).Font.Size = 12;
                    ws.Cells[1, 1] = "Project Report Summary";
                    ws.get_Range(ws.Cells[1, 1], ws.Cells[1, 1]).Font.Bold = true;
                    ws.get_Range(ws.Cells[1, 1], ws.Cells[1, 1]).Font.Underline = true;
                    ws.get_Range(ws.Cells[1, 1], ws.Cells[1, 1]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);

                    ws.Cells[1, 15] = "Print Date:";
                    ws.Cells[1, 16] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    ((Range)ws.Cells[1, 15]).HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    ((Range)ws.Cells[1, 16]).HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    ws.get_Range(ws.Cells[1, 15], ws.Cells[1, 16]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);

                    ws.Cells[2, 15] = "User Name:";
                    ws.Cells[2, 16] = displayname;
                    ((Range)ws.Cells[2, 15]).HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    ((Range)ws.Cells[2, 16]).HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    ws.get_Range(ws.Cells[2, 4], ws.Cells[2, 5]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);

                    if (projCode != "")
                    {
                        ws.Cells[2, 1] = "Project:";
                        ws.Cells[2, 2] = projCode;
                    }
                    else
                    {
                        ws.Cells[2, 1] = "Ledger";
                        ws.Cells[2, 2] = ledgerCode;
                    }
                    ((Range)ws.Cells[2, 1]).HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    ((Range)ws.Cells[2, 2]).HorizontalAlignment = XlHAlign.xlHAlignLeft;

                    ws.Cells[3, 1] = "Report Period:";
                    ((Range)ws.Cells[3, 1]).HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    ws.Cells[3, 2] = rptPeriod.ToString("yyyy-MM-dd"); ;
                    ((Range)ws.Cells[3, 2]).HorizontalAlignment = XlHAlign.xlHAlignLeft;

                    CurRow = StartDataRow;

                    xlApp.Cells[CurRow + 1, 1] = "Project Code";
                    xlApp.Cells[CurRow + 1, 2] = "Ledger";
                    xlApp.Cells[CurRow + 1, 3] = "Co Name";
                    xlApp.Cells[CurRow + 1, 4] = "Report Month";
                    xlApp.Cells[CurRow + 1, 5] = "Booked Cost To Date";
                    xlApp.Cells[CurRow, 5] = "(a)";
                    xlApp.Cells[CurRow + 1, 6] = "Cost Accrual";
                    xlApp.Cells[CurRow, 6] = "(b)";
                    xlApp.Cells[CurRow + 1, 7] = "OS_CC";
                    xlApp.Cells[CurRow, 7] = "(c)";
                    xlApp.Cells[CurRow + 1, 8] = "Total Cost To Date (with OS_CC)";
                    xlApp.Cells[CurRow, 8] = "(a)+(b)+(c)";
                    xlApp.Cells[CurRow + 1, 9] = "Total Cost To Date (without OS_CC)";
                    xlApp.Cells[CurRow, 9] = "(a)+(b)";
                    xlApp.Cells[CurRow + 1, 10] = "Gross Income To Date";
                    xlApp.Cells[CurRow, 10] = "(d)";
                    xlApp.Cells[CurRow + 1, 11] = "Income Accrual";
                    xlApp.Cells[CurRow, 11] = "(e)";
                    xlApp.Cells[CurRow + 1, 12] = "Total Income To Date";
                    xlApp.Cells[CurRow, 12] = "(d)+(e)";
                    xlApp.Cells[CurRow + 1, 13] = "Final Income";
                    xlApp.Cells[CurRow, 13] = "(f)";
                    xlApp.Cells[CurRow + 1, 14] = "Final Cost";
                    xlApp.Cells[CurRow, 14] = "(g)";
                    xlApp.Cells[CurRow + 1, 15] = "Final Margin";
                    xlApp.Cells[CurRow, 15] = "(h)";
                    xlApp.Cells[CurRow + 1, 16] = "Anticipated Completion";
                    xlApp.Cells[CurRow, 16] = "(i)";

                    ws.get_Range(ws.Cells[CurRow, 1], ws.Cells[CurRow + 1, 16]).HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    ws.get_Range(ws.Cells[CurRow + 1, 1], ws.Cells[CurRow + 1, 16]).Interior.ColorIndex = 35;

                    CurRow += 2;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        hasRow = true;

                        xlApp.Cells[CurRow, 1] = dt.Rows[i]["PrjCode"].ToString();                  //A
                        xlApp.Cells[CurRow, 2] = dt.Rows[i]["CompLedger"].ToString();               //B
                        xlApp.Cells[CurRow, 3] = dt.Rows[i]["CompName"].ToString();                 //C

                        if (dt.Rows[i]["ReportPeriod"].ToString() != "")                            //D
                        {
                            xlApp.Cells[CurRow, 4] = Convert.ToDateTime(dt.Rows[i]["ReportPeriod"].ToString()).ToString("yyyy-MM-dd");               
                            if (dt.Rows[i]["ReportPeriod"].ToString() == rptPeriod.ToString())
                                ((Range)ws.Cells[CurRow, 4]).Font.Bold = true;
                        }

                        xlApp.Cells[CurRow, 5] = dt.Rows[i]["BookedCost"].ToString();               //E
                        xlApp.Cells[CurRow, 6] = dt.Rows[i]["AccrualCost"].ToString();              //F
                        xlApp.Cells[CurRow, 7] = dt.Rows[i]["OutstandingCC"].ToString();            //G
                        xlApp.Cells[CurRow, 8] = String.Format("=E{0}+F{0}+G{0}", CurRow);          //H
                        xlApp.Cells[CurRow, 9] = String.Format("=E{0}+F{0}", CurRow);               //I
                        xlApp.Cells[CurRow, 10] = dt.Rows[i]["GrossIncome"].ToString();             //J
                        xlApp.Cells[CurRow, 11] = dt.Rows[i]["AccrualIncome"].ToString();           //K
                        xlApp.Cells[CurRow, 12] = String.Format("=J{0}+K{0}", CurRow);              //L
                        xlApp.Cells[CurRow, 13] = dt.Rows[i]["FinalIncome"].ToString();             //M
                        xlApp.Cells[CurRow, 14] = dt.Rows[i]["FinalCost"].ToString();               //N
                        xlApp.Cells[CurRow, 15] = String.Format("=M{0}-N{0}", CurRow);              //O
                        if (dt.Rows[i]["AntCompDate"].ToString() != "")                             //P
                            xlApp.Cells[CurRow, 16] = Convert.ToDateTime(dt.Rows[i]["AntCompDate"].ToString()).ToString("yyyy-MM-dd");                

                        CurRow++;
                    }

                    ((Range)ws.Cells[1, 1]).Font.Size = 16;

                    ((Range)ws.Cells[1, 2]).Cells.WrapText = true;
                    ((Range)ws.Cells[2, 2]).Cells.WrapText = true;

                    ((Range)ws.Cells[1, 4]).EntireColumn.NumberFormat = "yyyy-MM-dd";
                    ((Range)ws.Cells[1, 5]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 6]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 7]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 8]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 9]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 10]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 11]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 12]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 13]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 14]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 15]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 16]).EntireColumn.NumberFormat = "yyyy-MM-dd";

                    ((Range)ws.Cells[1, 16]).NumberFormat = "yyyy-MM-dd HH:mm";
                    ((Range)ws.Cells[3, 2]).NumberFormat = "yyyy-MM-dd";

                    ws.get_Range("A6", "P6").AutoFilter(1, Type.Missing, XlAutoFilterOperator.xlAnd, Type.Missing, true);
                    
                    ((Range)ws.Cells[7, 5]).Select();
                    xlApp.ActiveWindow.FreezePanes = true;
                    ((Range)ws.Cells[1, 1]).Select();

                    //set column width
                    ws.Columns.AutoFit();                   

                    ((Range)ws.Cells[1, 1]).EntireColumn.ColumnWidth = 20;
                    ((Range)ws.Cells[1, 2]).EntireColumn.ColumnWidth = 15;
                    ((Range)ws.Cells[1, 3]).EntireColumn.ColumnWidth = 15;
                  
                    ws.Name = "PRSummary";
                    wb.Saved = true;

                    Random nRandom = new Random(DateTime.Now.Millisecond);

                    string fname = Server.MapPath("..\\Temp\\AC09_" + nRandom.Next().ToString() + ".xls");
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

