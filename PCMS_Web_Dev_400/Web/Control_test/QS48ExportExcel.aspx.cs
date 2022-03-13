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

            String sqlstr;
            int CurRow = 1;
            //int CurCol = 1;
            int StartDataRow=4;
            int LastDataRow;
            int TotalCol=3;

            bool hasRow = false;

            String cc_fm = Request.QueryString["FCC"];
            String cc_to = Request.QueryString["TCC"];
            String username = Request.QueryString["UserName"];

            sqlstr = "SELECT AcctCode, AcctName, U_ReportCode FROM OACT WHERE Frozen = 'N' AND U_CostType IN ('E','M','N','P','PS','S','CC') AND AcctCode BETWEEN '" + cc_fm + "' AND '" + cc_to + "'" ;
            string _connectionString = ConfigurationManager.ConnectionStrings["SAP2"].ConnectionString.ToString();

            Microsoft.Office.Interop.Excel.Application xlApp = null;
            Workbook wb = null;
            Worksheet ws = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sqlstr, conn);
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
                    ws.Cells[1, 1] = "Cost Code List";
                    ws.get_Range(ws.Cells[1, 1], ws.Cells[1, 1]).Font.Bold = true;
                    ws.get_Range(ws.Cells[1, 1], ws.Cells[1, 1]).Font.Underline = true;
                    ws.get_Range(ws.Cells[1, 1], ws.Cells[1, 1]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);

                    ws.Cells[1, 4] = "Print Date:";
                    ws.Cells[1, 5] = DateTime.Now.ToString("dd-MMM-yyyy HH:mm");
                    ((Range)ws.Cells[1, 4]).HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 5]).HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ws.get_Range(ws.Cells[1, 4], ws.Cells[1, 5]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);

                    ws.Cells[2, 4] = "User Name:";
                    ws.Cells[2, 5] = username;
                    ((Range)ws.Cells[2, 4]).HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[2, 5]).HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ws.get_Range(ws.Cells[2, 4], ws.Cells[2, 5]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                    
                    ws.Cells[2, 1] = "Cost Code Range:";
                    ((Range)ws.Cells[2, 1]).VerticalAlignment = XlVAlign.xlVAlignTop;
                    ws.Cells[2, 2] = cc_fm + " To " + cc_to;

                    CurRow = StartDataRow;
                    
                    xlApp.Cells[CurRow, 1] = "Cost Code";
                    xlApp.Cells[CurRow, 2] = "Report Code";
                    xlApp.Cells[CurRow, 3] = "Cost Description";
                    
                    ws.get_Range(ws.Cells[CurRow, 1], ws.Cells[CurRow + 1, 1]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 2], ws.Cells[CurRow + 1, 2]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 3], ws.Cells[CurRow + 1, 3]).Merge(Type.Missing);

                    ws.get_Range(ws.Cells[StartDataRow, 1], ws.Cells[dt.Rows.Count + StartDataRow, TotalCol]).NumberFormat = "@";

                    CurRow += 2;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        hasRow = true;
                            
                        xlApp.Cells[CurRow, 1] = dt.Rows[i]["AcctCode"].ToString();
                        xlApp.Cells[CurRow, 2] = dt.Rows[i]["U_ReportCode"].ToString();
                        xlApp.Cells[CurRow, 3] = dt.Rows[i]["AcctName"].ToString();
                        
                        CurRow++;
                    }

                    //auto filter
                    ws.get_Range(ws.Cells[StartDataRow, 1], ws.Cells[StartDataRow, TotalCol]).Font.Bold = true;
                    ws.get_Range(ws.Cells[StartDataRow, 1], ws.Cells[CurRow - 1, TotalCol]).AutoFilter(1, Missing.Value, XlAutoFilterOperator.xlAnd, Missing.Value, true);
                                       
                    
                    ((Range)ws.Cells[1, 1]).Font.Size = 16;

                    ((Range)ws.Cells[1, 2]).Cells.WrapText = true;
                    ((Range)ws.Cells[2, 2]).Cells.WrapText = true;

                    ws.get_Range(ws.Cells[1, 4], ws.Cells[1, 5]).NumberFormat = "dd-mmm-yy HH:mm";

                    //set column width
                    ((Range)ws.Cells[1, 1]).EntireColumn.ColumnWidth = 20;
                    ((Range)ws.Cells[1, 2]).EntireColumn.ColumnWidth = 20;
                    ((Range)ws.Cells[1, 3]).EntireColumn.ColumnWidth = 100;
                    ((Range)ws.Cells[1, 4]).EntireColumn.ColumnWidth = 15;
                    ((Range)ws.Cells[1, 5]).EntireColumn.ColumnWidth = 25;

                    ws.Name = "CostCode";
                    wb.Saved = true;

                        Random nRandom = new Random(DateTime.Now.Millisecond);

                        string fname = Server.MapPath("..\\Temp\\QS48_" + nRandom.Next().ToString() + ".xls");
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

