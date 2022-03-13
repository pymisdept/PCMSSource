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

public partial class Control_QS43ExportExcel : System.Web.UI.Page
{

    enum QS43FieldMap : int
    {
        SourceKey = 4,
        SubContrName_En = 8,
        SubContrName_Ch = 9,
        CertNo = 11,
        Description = 12,
        CurWorkDone = 15,
        CurVariation = 16,
        CurDaywork = 17,
        CurOnSite = 18,
        CurRetention = 20,
        CurContraCharge = 21,
        PeriodEnd = 23,
        SCSubDate = 24,
        DueDate = 25,
        EarlyReleaseDate = 26,
        DateOut = 27,
        SCAppAmount = 28,
        CumWorkDone = 30,
        CumVariation = 31,
        CumDaywork = 32,
        CumOnSite = 33,
        CumRetention = 35,
        CumContraCharge = 36,
        Status = 38
    }

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
            int CurRow = 1;
            int StartDataRow = 5;
            int LastDataRow;
            int CCSummaryStartRow;
            int TotalCol = 11;
            int InvAmtCol = 13;
            int CostCodeCol = 18;
            int GrandTotRow;
            int CCSummaryTotRow;

            sqlstr = "select * from [QS43_Detail1](" + Request.QueryString["ProjectCode"] + "," + Request.QueryString["ProjectCode"] + ") order by SourceKey, CertNo";

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

            int targetRow, targetCol;

            targetRow = 1; targetCol = 1;
            ws.Cells[targetRow, targetCol] = "Sub-Contractor's Payment Certificate Record - Site QS Report";

            ws.get_Range(ws.Cells[targetRow, targetCol], ws.Cells[targetRow, targetCol]).Font.Bold = true;
            ws.get_Range(ws.Cells[targetRow, targetCol], ws.Cells[targetRow, targetCol]).Font.Size = 16;
            ws.get_Range(ws.Cells[targetRow, targetCol], ws.Cells[targetRow, targetCol]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);

            targetRow = 2; targetCol = 1;
            ws.Cells[targetRow, targetCol] = "Project Code";
            ws.get_Range(ws.Cells[targetRow, targetCol], ws.Cells[targetRow, targetCol]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
            
            targetRow = 3; targetCol = 1;
            ws.Cells[targetRow, targetCol] = "Project Name";
            ws.get_Range(ws.Cells[targetRow, targetCol], ws.Cells[targetRow, targetCol]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);

            targetRow = 2; targetCol = 2;
            ws.get_Range(ws.Cells[targetRow, targetCol], ws.Cells[targetRow, targetCol]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);

            targetRow = 3; targetCol = 2;
            ws.get_Range(ws.Cells[targetRow, targetCol], ws.Cells[targetRow, targetCol]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);

            targetRow = 2; targetCol = 24;
            ws.Cells[targetRow, targetCol] = "User Name";
            ws.get_Range(ws.Cells[targetRow, targetCol], ws.Cells[targetRow, targetCol]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);

            targetRow = 3; targetCol = 24;
            ws.Cells[targetRow, targetCol] = "Print Date";
            ws.get_Range(ws.Cells[targetRow, targetCol], ws.Cells[targetRow, targetCol]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);

            targetRow = 2; targetCol = 25;
            ws.Cells[targetRow, targetCol] = Request.QueryString["UserName"];
            ws.get_Range(ws.Cells[targetRow, targetCol], ws.Cells[targetRow, targetCol]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);

            targetRow = 3; targetCol = 25;
            ws.Cells[targetRow, targetCol] = "'" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm");
            ws.get_Range(ws.Cells[targetRow, targetCol], ws.Cells[targetRow, targetCol]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);


            CurRow = StartDataRow;

                string _connectionString = ConfigurationManager.ConnectionStrings["SAP2"].ConnectionString.ToString();
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    try
                    {
                        conn.Open();
                        SqlDataAdapter da = new SqlDataAdapter(sqlstr, conn);
                        System.Data.DataTable dt = new System.Data.DataTable();
                        da.Fill(dt);
                        DataColumnCollection dcCollection = dt.Columns;

                        List<string> colList = new List<string>();
                        colList.Add("SubContractor No."); 
                        colList.Add("Name of Sub-Contractor"); 
                        colList.Add("Name (In Chinese)");
                        colList.Add("Description");
                        colList.Add("Cert No.");
                        colList.Add("Status");
                        colList.Add("Work Done Period Ending");
                        colList.Add("SC Submission Date");
                        colList.Add("SC Applied Amount");
                        colList.Add("Cumulative\nWork Done\n(a)");
                        colList.Add("Cumulative\nMaterial On Site\n(b)");
                        colList.Add("Cumulative\nVariation\n(c)");
                        colList.Add("Cumulative\nDaywork\n(d)");
                        colList.Add("Cumulative\nRetention\n(e)");
                        colList.Add("Cumulative\nContra Charge\n(f)");
                        colList.Add("Total Cert.\nTo Date\n(HK$)\n(g)=(a)to(f)");
                        colList.Add("This Period\nWork Done");
                        colList.Add("This Period\nMaterial on Site");
                        colList.Add("This Period\nVariations");
                        colList.Add("This Period\nDayworks");
                        colList.Add("This Period\nRetention");
                        colList.Add("This Period\nContra Charge");
                        colList.Add("This\nPayment\nCert. (HK$)");
                        colList.Add("Due Date");
                        colList.Add("Early Released\nDate");
                        colList.Add("Date Out to\nHead Office");

                        //for (int i = 0; i <= dt.Columns.Count; i++)
                        //{
                        //    xlApp.Cells[CurRow, i + 1] = dcCollection[i].ToString();
                        //    ws.get_Range(ws.Cells[CurRow, i + 1], ws.Cells[CurRow, i + 1]).Font.Bold = true;
                        //    ws.get_Range(ws.Cells[CurRow, i + 1], ws.Cells[CurRow, i + 1]).Font.Underline = true;
                        //}
                        for (int i = 0; i < colList.Count; i++)
                        {
                            xlApp.Cells[CurRow, i + 1] = colList[i];
                            ws.get_Range(ws.Cells[CurRow, i + 1], ws.Cells[CurRow, i + 1]).Font.Bold = true;
                            ws.get_Range(ws.Cells[CurRow, i + 1], ws.Cells[CurRow, i + 1]).Font.Underline = true;
                        }

                        CurRow += 1;

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            int ins_col = 1;
                            int cum_amount = 0;
                            int cur_amount = 0;

                            xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.SourceKey].ToString();
                            xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.SubContrName_En].ToString();
                            xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.SubContrName_Ch].ToString();
                            xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.Description].ToString();
                            xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.CertNo].ToString();
                            xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.Status].ToString();

                            if (dt.Rows[j][(int)QS43FieldMap.PeriodEnd].ToString().Length > 0)
                                xlApp.Cells[CurRow, ins_col++] = "'" + dt.Rows[j][(int)QS43FieldMap.PeriodEnd].ToString().Substring(0, 10);
                            else
                                ins_col++;

                            if (dt.Rows[j][(int)QS43FieldMap.SCSubDate].ToString().Length > 0)
                                xlApp.Cells[CurRow, ins_col++] = "'" + dt.Rows[j][(int)QS43FieldMap.SCSubDate].ToString().Substring(0, 10);
                            else
                                ins_col++;

                            xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.SCAppAmount].ToString();

                            //xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.CumWorkDone].ToString();
                            //xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.CumOnSite].ToString();
                            //xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.CumVariation].ToString();
                            //xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.CumDaywork].ToString();
                            //xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.CumRetention].ToString();
                            //xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.CumContraCharge].ToString();

                            xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.CumWorkDone].ToString();
                            xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.CumOnSite].ToString();
                            xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.CumVariation].ToString();
                            xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.CumDaywork].ToString();
                            xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.CumRetention].ToString();
                            xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.CumContraCharge].ToString();

                            //cum_amount = (int)dt.Rows[j][(int)QS43FieldMap.CumWorkDone] + (int)dt.Rows[j][(int)QS43FieldMap.CumOnSite] +
                            //    (int)dt.Rows[j][(int)QS43FieldMap.CumVariation] + (int)dt.Rows[j][(int)QS43FieldMap.CumDaywork] +
                            //    (int)dt.Rows[j][(int)QS43FieldMap.CumRetention] +(int)dt.Rows[j][(int)QS43FieldMap.CumContraCharge];

                            string formula = string.Format("=SUM($J{0}:$O{1})", CurRow, CurRow);
                            xlApp.Cells[CurRow, ins_col++] = formula;

                            xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.CurWorkDone].ToString();
                            xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.CurOnSite].ToString();
                            xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.CurVariation].ToString();
                            xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.CurDaywork].ToString();
                            xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.CurRetention].ToString();
                            xlApp.Cells[CurRow, ins_col++] = dt.Rows[j][(int)QS43FieldMap.CurContraCharge].ToString();
                            
                            //cur_amount = (int)dt.Rows[j][(int)QS43FieldMap.CurWorkDone] + (int)dt.Rows[j][(int)QS43FieldMap.CurOnSite] +
                            //    (int)dt.Rows[j][(int)QS43FieldMap.CurVariation] + (int)dt.Rows[j][(int)QS43FieldMap.CurDaywork] +
                            //    (int)dt.Rows[j][(int)QS43FieldMap.CurRetention] + (int)dt.Rows[j][(int)QS43FieldMap.CurContraCharge];

                            formula = string.Format("=SUM($Q{0}:$V{1})", CurRow, CurRow);
                            xlApp.Cells[CurRow, ins_col++] = formula;

                            if (dt.Rows[j][(int)QS43FieldMap.DueDate].ToString().Length > 0)
                                xlApp.Cells[CurRow, ins_col++] = "'" + dt.Rows[j][(int)QS43FieldMap.DueDate].ToString().Substring(0, 10);
                            else
                                ins_col++;

                            if (dt.Rows[j][(int)QS43FieldMap.EarlyReleaseDate].ToString().Length > 0)
                                xlApp.Cells[CurRow, ins_col++] = "'" + dt.Rows[j][(int)QS43FieldMap.EarlyReleaseDate].ToString().Substring(0, 10);
                            else
                                ins_col++;

                            if (dt.Rows[j][(int)QS43FieldMap.DateOut].ToString().Length > 0)
                                xlApp.Cells[CurRow, ins_col++] = "'" + dt.Rows[j][(int)QS43FieldMap.DateOut].ToString().Substring(0, 10);
                            else
                                ins_col++;

                            CurRow += 1;

                        }

                        sqlstr = "select prjcode,U_ProjectFullName from oprj where u_docentry='" + Request.QueryString["ProjectCode"] + "'";
                        da = new SqlDataAdapter(sqlstr, conn);
                        dt = new System.Data.DataTable();
                        da.Fill(dt);
                        string prjCode = dt.Rows[0]["prjcode"].ToString();
                        string prjName = dt.Rows[0]["U_ProjectFullName"].ToString();

                        ws.Cells[2, 2] = prjCode;
                        ws.Cells[3, 2] = prjName;

                        ((Range)ws.Cells[1, 5]).EntireColumn.NumberFormat = "000";

                        ((Range)ws.Cells[1, 9]).EntireColumn.NumberFormat = "#,##0.00_);(#,##0.00)";
                        ((Range)ws.Cells[1, 10]).EntireColumn.NumberFormat = "#,##0.00_);(#,##0.00)";
                        ((Range)ws.Cells[1, 11]).EntireColumn.NumberFormat = "#,##0.00_);(#,##0.00)";
                        ((Range)ws.Cells[1, 12]).EntireColumn.NumberFormat = "#,##0.00_);(#,##0.00)";
                        ((Range)ws.Cells[1, 13]).EntireColumn.NumberFormat = "#,##0.00_);(#,##0.00)";
                        ((Range)ws.Cells[1, 14]).EntireColumn.NumberFormat = "#,##0.00_);(#,##0.00)";
                        ((Range)ws.Cells[1, 15]).EntireColumn.NumberFormat = "#,##0.00_);(#,##0.00)";
                        ((Range)ws.Cells[1, 16]).EntireColumn.NumberFormat = "#,##0.00_);(#,##0.00)";
                        ((Range)ws.Cells[1, 17]).EntireColumn.NumberFormat = "#,##0.00_);(#,##0.00)";
                        ((Range)ws.Cells[1, 18]).EntireColumn.NumberFormat = "#,##0.00_);(#,##0.00)";
                        ((Range)ws.Cells[1, 19]).EntireColumn.NumberFormat = "#,##0.00_);(#,##0.00)";
                        ((Range)ws.Cells[1, 20]).EntireColumn.NumberFormat = "#,##0.00_);(#,##0.00)";
                        ((Range)ws.Cells[1, 21]).EntireColumn.NumberFormat = "#,##0.00_);(#,##0.00)";
                        ((Range)ws.Cells[1, 22]).EntireColumn.NumberFormat = "#,##0.00_);(#,##0.00)";
                        ((Range)ws.Cells[1, 23]).EntireColumn.NumberFormat = "#,##0.00_);(#,##0.00)";


                        //((Range)ws.Cells[1, 7]).EntireColumn. = "dd/mm/yyyy";
                        //((Range)ws.Cells[1, 8]).EntireColumn.NumberFormatLocal = "dd/mm/yyyy";
                        //((Range)ws.Cells[1, 24]).EntireColumn.NumberFormatLocal = "dd/mm/yyyy";
                        //((Range)ws.Cells[1, 25]).EntireColumn.NumberFormatLocal = "dd/mm/yyyy";
                        //((Range)ws.Cells[1, 26]).EntireColumn.NumberFormatLocal = "dd/mm/yyyy";
                        //auto filter
                        ws.get_Range(ws.Cells[StartDataRow, 1], ws.Cells[CurRow, 26]).AutoFilter(1, Missing.Value, XlAutoFilterOperator.xlAnd, Missing.Value, true);

                        ////freeze window
                        //xlApp.ActiveWindow.SplitRow = 2;
                        //xlApp.ActiveWindow.SplitColumn = 4;
                        //xlApp.ActiveWindow.FreezePanes = true;

                        //set row height
                        ((Range)ws.Cells[1, 1]).EntireRow.RowHeight = 28.5;
                        ((Range)ws.Cells[1, 1]).EntireRow.VerticalAlignment = XlVAlign.xlVAlignCenter;
                        ((Range)ws.Cells[2, 1]).EntireRow.RowHeight = 31.5;
                        ((Range)ws.Cells[2, 1]).EntireRow.VerticalAlignment = XlVAlign.xlVAlignTop;
                        ((Range)ws.Cells[5, 1]).EntireRow.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                        //set column width
                        ((Range)ws.Cells[1, 1]).EntireColumn.ColumnWidth = 26;
                        ((Range)ws.Cells[1, 2]).EntireColumn.ColumnWidth = 40;
                        ((Range)ws.Cells[1, 2]).EntireColumn.WrapText = true;
                        ((Range)ws.Cells[1, 3]).EntireColumn.ColumnWidth = 30;
                        ((Range)ws.Cells[1, 3]).EntireColumn.WrapText = true;
                        ((Range)ws.Cells[1, 4]).EntireColumn.ColumnWidth = 40;
                        ((Range)ws.Cells[1, 4]).EntireColumn.WrapText = true;
                        ((Range)ws.Cells[1, 5]).EntireColumn.ColumnWidth = 15;
                        ((Range)ws.Cells[1, 6]).EntireColumn.ColumnWidth = 13;
                        ((Range)ws.Cells[1, 7]).EntireColumn.ColumnWidth = 33;
                        ((Range)ws.Cells[1, 8]).EntireColumn.ColumnWidth = 28;
                        ((Range)ws.Cells[1, 9]).EntireColumn.ColumnWidth = 27;
                        ((Range)ws.Cells[1, 10]).EntireColumn.ColumnWidth = 18.5;
                        ((Range)ws.Cells[1, 11]).EntireColumn.ColumnWidth = 22.5;
                        ((Range)ws.Cells[1, 12]).EntireColumn.ColumnWidth = 15.5;
                        ((Range)ws.Cells[1, 13]).EntireColumn.ColumnWidth = 15.5;
                        ((Range)ws.Cells[1, 14]).EntireColumn.ColumnWidth = 19;
                        ((Range)ws.Cells[1, 15]).EntireColumn.ColumnWidth = 21.5;
                        ((Range)ws.Cells[1, 16]).EntireColumn.ColumnWidth = 18;
                        ((Range)ws.Cells[1, 17]).EntireColumn.ColumnWidth = 18;
                        ((Range)ws.Cells[1, 18]).EntireColumn.ColumnWidth = 22.5;
                        ((Range)ws.Cells[1, 19]).EntireColumn.ColumnWidth = 18;
                        ((Range)ws.Cells[1, 20]).EntireColumn.ColumnWidth = 18;
                        ((Range)ws.Cells[1, 21]).EntireColumn.ColumnWidth = 18;
                        ((Range)ws.Cells[1, 22]).EntireColumn.ColumnWidth = 22;
                        ((Range)ws.Cells[1, 23]).EntireColumn.ColumnWidth = 18.5;
                        ((Range)ws.Cells[1, 24]).EntireColumn.ColumnWidth = 16;
                        ((Range)ws.Cells[1, 25]).EntireColumn.ColumnWidth = 21.5;
                        ((Range)ws.Cells[1, 26]).EntireColumn.ColumnWidth = 20;
                        //seek project code

                        ws.Name = "SubContr Cert Record";
                        wb.Saved = true;
                        Random nRandom = new Random(DateTime.Now.Millisecond);

                        string fname = Server.MapPath("..\\Temp\\QS43_" + nRandom.Next().ToString() + ".xls");
                        //string fname = Server.MapPath("..\\Temp\\QS43_125125.xls");
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
                        xlApp.Cells[1, 1] = ex.Message.ToString();
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

