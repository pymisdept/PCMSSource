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

public partial class Control_QS47ExportExcel : System.Web.UI.Page
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
            String sqlstr2;
            int CurRow = 1;
            //int CurCol = 1;
            int StartDataRow=8;
            int LastDataRow;
            int CCSummaryStartRow;
            int TotalCol=21;
            int InvAmtCol=13;
            int CostCodeCol=18;
            int GrandTotRow;
            int CCSummaryTotRow;

            String username = string.Empty;
            String prjCode = string.Empty;
            String prjDesc = string.Empty;

            bool hasRow = false;

            DateTime sd = Convert.ToDateTime(Request.QueryString["StartDate"]);
            DateTime ed = Convert.ToDateTime(Request.QueryString["EndDate"]);

            username = Request.QueryString["UserName"];

            sqlstr = "EXEC sp_getSCSummary " + Request.QueryString["FPrjCode"] + ",'" + Request.QueryString["StartDate"] + "','" + Request.QueryString["EndDate"] +"'";
            sqlstr2 = "SELECT PrjCode, U_ProjectFullName from OPRJ where U_DocEntry = " + Request.QueryString["FPrjCode"];

            string _connectionString = ConfigurationManager.ConnectionStrings["SAP2"].ConnectionString.ToString();

            Microsoft.Office.Interop.Excel.Application xlApp = null;
            Workbook wb = null;
            Worksheet ws = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sqlstr2, conn);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    da.Fill(dt);
                    prjCode = dt.Rows[0][0].ToString();
                    prjDesc = dt.Rows[0][1].ToString();

                    da = new SqlDataAdapter(sqlstr, conn);
                    dt = new System.Data.DataTable();
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
                    ws.Cells[1, 1] = "SC Payment Cert Summary";
                    ws.get_Range(ws.Cells[1, 1], ws.Cells[1, 1]).Font.Bold = true;
                    ws.get_Range(ws.Cells[1, 1], ws.Cells[1, 1]).Font.Underline = true;
                    ws.get_Range(ws.Cells[1, 1], ws.Cells[1, 1]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);

                    ws.Cells[1, 20] = "Print Date:";
                    ws.Cells[1, 21] = DateTime.Now.ToString("dd-MMM-yyyy HH:mm");
                    ((Range)ws.Cells[1, 20]).HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ws.get_Range(ws.Cells[1, 20], ws.Cells[1, 21]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);

                    ws.Cells[2, 20] = "User Name:";
                    ws.Cells[2, 21] = username;
                    ((Range)ws.Cells[2, 20]).HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ws.get_Range(ws.Cells[2, 20], ws.Cells[2, 21]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                    
                    ws.Cells[3, 1] = "Project:";
                    ((Range)ws.Cells[3, 1]).VerticalAlignment = XlVAlign.xlVAlignTop;
                    ws.Cells[3, 2] = prjCode + " - " + prjDesc;
                    ws.Cells[4, 1] = "Due Date:";
                    ws.Cells[4, 2] = sd.ToString("dd-MMM-yy") + " to " + ed.ToString("dd-MMM-yy");

                    CurRow = StartDataRow - 3;

                    ws.get_Range(ws.Cells[CurRow, 3], ws.Cells[CurRow+1, 4]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 5], ws.Cells[CurRow+1, 6]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 7], ws.Cells[CurRow+1, 20]).Merge(Type.Missing);

                    ws.get_Range(ws.Cells[CurRow, 3], ws.Cells[CurRow+1, 20]).Interior.ColorIndex = 35;
                    ws.get_Range(ws.Cells[CurRow, 3], ws.Cells[CurRow + 1, 20]).Font.Bold = true;
                    ws.get_Range(ws.Cells[CurRow, 3], ws.Cells[CurRow + 1, 20]).Font.Size = 12;
                    ws.get_Range(ws.Cells[CurRow, 3], ws.Cells[CurRow + 1, 20]).Borders.get_Item(XlBordersIndex.xlInsideVertical).LineStyle = XlLineStyle.xlContinuous;
                    ws.get_Range(ws.Cells[CurRow, 3], ws.Cells[CurRow + 1, 20]).Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                    ws.get_Range(ws.Cells[CurRow, 3], ws.Cells[CurRow + 1, 20]).Borders.get_Item(XlBordersIndex.xlEdgeLeft).LineStyle = XlLineStyle.xlContinuous;
                    ws.get_Range(ws.Cells[CurRow, 3], ws.Cells[CurRow + 1, 20]).Borders.get_Item(XlBordersIndex.xlEdgeRight).LineStyle = XlLineStyle.xlContinuous;
                    //ws.get_Range(ws.Cells[CurRow, 3], ws.Cells[CurRow, 20]).Borders.get_Item(XlBordersIndex.xlInsideHorizontal).LineStyle = XlLineStyle.xlContinuous;
                    ws.get_Range(ws.Cells[CurRow, 3], ws.Cells[CurRow + 1, 20]).HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    ws.get_Range(ws.Cells[CurRow, 3], ws.Cells[CurRow + 1, 20]).VerticalAlignment = XlVAlign.xlVAlignBottom;

                    xlApp.Cells[CurRow, 3] = "Budget / Commitment";
                    xlApp.Cells[CurRow, 5] = "Buying Gain / Lost";
                    xlApp.Cells[CurRow, 7] = "Payment Status";

                    CurRow += 2;
                    ws.get_Range(ws.Cells[CurRow, 1], ws.Cells[CurRow + 1, TotalCol]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Silver);
                    
                    xlApp.Cells[CurRow, 1] = "SC No.";
                    xlApp.Cells[CurRow, 2] = "Sub-contractor";
                    xlApp.Cells[CurRow, 3] = "Contract Sum";
                    xlApp.Cells[CurRow, 4] = "Committed Budget";
                    xlApp.Cells[CurRow, 5] = "Exclude non-recoverable VO";
                    xlApp.Cells[CurRow, 6] = "Include non-recoverable VO";
                    xlApp.Cells[CurRow, 7] = "Latest Cert";
                    xlApp.Cells[CurRow, 8] = "Period Ending";
                    xlApp.Cells[CurRow, 9] = "Due Date";
                    xlApp.Cells[CurRow, 10] = "This Payment $";
                    xlApp.Cells[CurRow, 11] = "Status";
                    xlApp.Cells[CurRow, 12] = "Contract Work Certified";
                    
                    ws.get_Range(ws.Cells[CurRow, 1], ws.Cells[CurRow + 1, 1]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 2], ws.Cells[CurRow + 1, 2]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 3], ws.Cells[CurRow + 1, 3]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 4], ws.Cells[CurRow + 1, 4]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 5], ws.Cells[CurRow + 1, 5]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 6], ws.Cells[CurRow + 1, 6]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 7], ws.Cells[CurRow + 1, 7]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 8], ws.Cells[CurRow + 1, 8]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 9], ws.Cells[CurRow + 1, 9]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 10], ws.Cells[CurRow + 1, 10]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 11], ws.Cells[CurRow + 1, 11]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 12], ws.Cells[CurRow + 1, 12]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 13], ws.Cells[CurRow, 14]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 15], ws.Cells[CurRow, 16]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 17], ws.Cells[CurRow, 18]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 19], ws.Cells[CurRow + 1, 19]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 20], ws.Cells[CurRow + 1, 20]).Merge(Type.Missing);
                    ws.get_Range(ws.Cells[CurRow, 21], ws.Cells[CurRow + 1, 21]).Merge(Type.Missing);

                    xlApp.Cells[CurRow, 13] = "Recoverable VO / DW";
                    xlApp.Cells[CurRow + 1, 13] = "Certified";
                    xlApp.Cells[CurRow + 1, 14] = "Provision";

                    xlApp.Cells[CurRow, 15] = "Non-Recoverable VO / DW";
                    xlApp.Cells[CurRow + 1, 15] = "Certified";
                    xlApp.Cells[CurRow + 1, 16] = "Provision";

                    xlApp.Cells[CurRow, 17] = "Contra-charges";
                    xlApp.Cells[CurRow + 1, 17] = "Deducted";
                    xlApp.Cells[CurRow + 1, 18] = "Provision";

                    xlApp.Cells[CurRow, 19] = "Retention";
                    xlApp.Cells[CurRow, 20] = "Total Certified";
                    xlApp.Cells[CurRow, 21] = "Anticipated Final Cost";

                    CurRow += 2;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        hasRow = true;

                        decimal RVODW_prov = Convert.ToDecimal(dt.Rows[i][14].ToString()) - Convert.ToDecimal(dt.Rows[i][13].ToString());
                        decimal NRVODW_prov = Convert.ToDecimal(dt.Rows[i][16].ToString()) - Convert.ToDecimal(dt.Rows[i][15].ToString());
                        decimal CC_prov = Convert.ToDecimal(dt.Rows[i][18].ToString()) - Convert.ToDecimal(dt.Rows[i][17].ToString());

                        xlApp.Cells[CurRow, 1] = dt.Rows[i][3].ToString().Substring(9,5);              //A
                        xlApp.Cells[CurRow, 2] = dt.Rows[i][4].ToString();              //B
                        xlApp.Cells[CurRow, 3] = dt.Rows[i][7].ToString();              //C
                        xlApp.Cells[CurRow, 4] = dt.Rows[i][8].ToString();              //D
                        xlApp.Cells[CurRow, 5] = String.Format("=D{0}-C{0}",CurRow);    //E
                        xlApp.Cells[CurRow, 6] = String.Format("=D{0}-C{0}-O{0}-P{0}", CurRow);                //F
                        xlApp.Cells[CurRow, 7] = dt.Rows[i][5].ToString();              //G
                        xlApp.Cells[CurRow, 8] = Convert.ToDateTime(dt.Rows[i][9].ToString()).ToString("dd-MMM-yy");              //H
                        xlApp.Cells[CurRow, 9] = Convert.ToDateTime(dt.Rows[i][10].ToString()).ToString("dd-MMM-yy");             //I
                        xlApp.Cells[CurRow, 10] = dt.Rows[i][11].ToString();            //J
                        xlApp.Cells[CurRow, 11] = dt.Rows[i][6].ToString();             //K
                        xlApp.Cells[CurRow, 12] = dt.Rows[i][12].ToString();            //L
                        xlApp.Cells[CurRow, 13] = dt.Rows[i][13].ToString();            //M
                        xlApp.Cells[CurRow, 14] = RVODW_prov.ToString();                //N
                        xlApp.Cells[CurRow, 15] = dt.Rows[i][15].ToString();            //O
                        xlApp.Cells[CurRow, 16] = NRVODW_prov.ToString();               //P
                        xlApp.Cells[CurRow, 17] = dt.Rows[i][17].ToString();            //Q
                        xlApp.Cells[CurRow, 18] = CC_prov.ToString();                   //R
                        xlApp.Cells[CurRow, 19] = dt.Rows[i][19].ToString();            //S
                        xlApp.Cells[CurRow, 20] = String.Format("=L{0}+M{0}+O{0}+Q{0}", CurRow);    //T       
                        xlApp.Cells[CurRow, 21] = String.Format("=IF(L{0}>C{0},L{0}+M{0}+N{0}+O{0}+P{0}+Q{0}+R{0},C{0}+M{0}+N{0}+O{0}+P{0}+Q{0}+R{0})", CurRow);    //U

                        CurRow++;
                    }


                    ws.get_Range(ws.Cells[StartDataRow - 1, 1], ws.Cells[CurRow - 1, TotalCol]).Borders.get_Item(XlBordersIndex.xlInsideVertical).LineStyle = XlLineStyle.xlContinuous;
                    ws.get_Range(ws.Cells[StartDataRow - 1, 1], ws.Cells[CurRow - 1, TotalCol]).Borders.get_Item(XlBordersIndex.xlInsideHorizontal).LineStyle = XlLineStyle.xlContinuous;
                    ws.get_Range(ws.Cells[StartDataRow - 1, 1], ws.Cells[CurRow - 1, TotalCol]).Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
                    ws.get_Range(ws.Cells[StartDataRow - 1, 1], ws.Cells[CurRow - 1, TotalCol]).Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;
                    ws.get_Range(ws.Cells[StartDataRow - 1, 1], ws.Cells[CurRow - 1, TotalCol]).Borders.get_Item(XlBordersIndex.xlEdgeLeft).LineStyle = XlLineStyle.xlContinuous;
                    ws.get_Range(ws.Cells[StartDataRow - 1, 1], ws.Cells[CurRow - 1, TotalCol]).Borders.get_Item(XlBordersIndex.xlEdgeRight).LineStyle = XlLineStyle.xlContinuous;

                    //auto filter
                    ws.get_Range(ws.Cells[StartDataRow - 1, 1], ws.Cells[CurRow - 1, TotalCol]).AutoFilter(1, Missing.Value, XlAutoFilterOperator.xlAnd, Missing.Value, true);

                    CurRow++;

                    xlApp.Cells[CurRow, 2] = "Works Total:";
                    if (hasRow)
                    {
                        xlApp.Cells[CurRow, 3] = String.Format("=SUMIF(A{0}:A{1},\"<>NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 2, "C");
                        xlApp.Cells[CurRow, 4] = String.Format("=SUMIF(A{0}:A{1},\"<>NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 2, "D");
                        xlApp.Cells[CurRow, 5] = String.Format("=SUMIF(A{0}:A{1},\"<>NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 2, "E");
                        xlApp.Cells[CurRow, 6] = String.Format("=SUMIF(A{0}:A{1},\"<>NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 2, "F");
                        xlApp.Cells[CurRow, 10] = String.Format("=SUMIF(A{0}:A{1},\"<>NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 2, "J");
                        xlApp.Cells[CurRow, 12] = String.Format("=SUMIF(A{0}:A{1},\"<>NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 2, "L");
                        xlApp.Cells[CurRow, 13] = String.Format("=SUMIF(A{0}:A{1},\"<>NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 2, "M");
                        xlApp.Cells[CurRow, 14] = String.Format("=SUMIF(A{0}:A{1},\"<>NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 2, "N");
                        xlApp.Cells[CurRow, 15] = String.Format("=SUMIF(A{0}:A{1},\"<>NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 2, "O");
                        xlApp.Cells[CurRow, 16] = String.Format("=SUMIF(A{0}:A{1},\"<>NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 2, "P");
                        xlApp.Cells[CurRow, 17] = String.Format("=SUMIF(A{0}:A{1},\"<>NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 2, "Q");
                        xlApp.Cells[CurRow, 18] = String.Format("=SUMIF(A{0}:A{1},\"<>NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 2, "R");
                        xlApp.Cells[CurRow, 19] = String.Format("=SUMIF(A{0}:A{1},\"<>NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 2, "S");
                        xlApp.Cells[CurRow, 20] = String.Format("=SUMIF(A{0}:A{1},\"<>NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 2, "T");
                        xlApp.Cells[CurRow, 21] = String.Format("=SUMIF(A{0}:A{1},\"<>NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 2, "U");
                    }

                    CurRow++;
                    xlApp.Cells[CurRow, 2] = "Nominated Sub-contractors Total:";
                    if (hasRow)
                    {
                        xlApp.Cells[CurRow, 3] = String.Format("=SUMIF(A{0}:A{1},\"NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 3, "C");
                        xlApp.Cells[CurRow, 4] = String.Format("=SUMIF(A{0}:A{1},\"NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 3, "D");
                        xlApp.Cells[CurRow, 5] = String.Format("=SUMIF(A{0}:A{1},\"NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 3, "E");
                        xlApp.Cells[CurRow, 6] = String.Format("=SUMIF(A{0}:A{1},\"NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 3, "F");
                        xlApp.Cells[CurRow, 10] = String.Format("=SUMIF(A{0}:A{1},\"NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 3, "J");
                        xlApp.Cells[CurRow, 12] = String.Format("=SUMIF(A{0}:A{1},\"NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 3, "L");
                        xlApp.Cells[CurRow, 13] = String.Format("=SUMIF(A{0}:A{1},\"NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 3, "M");
                        xlApp.Cells[CurRow, 14] = String.Format("=SUMIF(A{0}:A{1},\"NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 3, "N");
                        xlApp.Cells[CurRow, 15] = String.Format("=SUMIF(A{0}:A{1},\"NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 3, "O");
                        xlApp.Cells[CurRow, 16] = String.Format("=SUMIF(A{0}:A{1},\"NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 3, "P");
                        xlApp.Cells[CurRow, 17] = String.Format("=SUMIF(A{0}:A{1},\"NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 3, "Q");
                        xlApp.Cells[CurRow, 18] = String.Format("=SUMIF(A{0}:A{1},\"NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 3, "R");
                        xlApp.Cells[CurRow, 19] = String.Format("=SUMIF(A{0}:A{1},\"NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 3, "S");
                        xlApp.Cells[CurRow, 20] = String.Format("=SUMIF(A{0}:A{1},\"NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 3, "T");
                        xlApp.Cells[CurRow, 21] = String.Format("=SUMIF(A{0}:A{1},\"NS*\",{2}{0}:{2}{1})", StartDataRow + 1, CurRow - 3, "U");
                    }
                    ws.get_Range(ws.Cells[CurRow, 2], ws.Cells[CurRow, TotalCol]).Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlContinuous;

                    CurRow++;
                    xlApp.Cells[CurRow, 2] = "Total:";
                    if (hasRow)
                    {
                        xlApp.Cells[CurRow, 3] = String.Format("=SUM({2}{0},{2}{1})", CurRow - 2, CurRow - 1, "C");
                        xlApp.Cells[CurRow, 4] = String.Format("=SUM({2}{0},{2}{1})", CurRow - 2, CurRow - 1, "D");
                        xlApp.Cells[CurRow, 5] = String.Format("=SUM({2}{0},{2}{1})", CurRow - 2, CurRow - 1, "E");
                        xlApp.Cells[CurRow, 6] = String.Format("=SUM({2}{0},{2}{1})", CurRow - 2, CurRow - 1, "F");
                        xlApp.Cells[CurRow, 10] = String.Format("=SUM({2}{0},{2}{1})", CurRow - 2, CurRow - 1, "J");
                        xlApp.Cells[CurRow, 12] = String.Format("=SUM({2}{0},{2}{1})", CurRow - 2, CurRow - 1, "L");
                        xlApp.Cells[CurRow, 13] = String.Format("=SUM({2}{0},{2}{1})", CurRow - 2, CurRow - 1, "M");
                        xlApp.Cells[CurRow, 14] = String.Format("=SUM({2}{0},{2}{1})", CurRow - 2, CurRow - 1, "N");
                        xlApp.Cells[CurRow, 15] = String.Format("=SUM({2}{0},{2}{1})", CurRow - 2, CurRow - 1, "O");
                        xlApp.Cells[CurRow, 16] = String.Format("=SUM({2}{0},{2}{1})", CurRow - 2, CurRow - 1, "P");
                        xlApp.Cells[CurRow, 17] = String.Format("=SUM({2}{0},{2}{1})", CurRow - 2, CurRow - 1, "Q");
                        xlApp.Cells[CurRow, 18] = String.Format("=SUM({2}{0},{2}{1})", CurRow - 2, CurRow - 1, "R");
                        xlApp.Cells[CurRow, 19] = String.Format("=SUM({2}{0},{2}{1})", CurRow - 2, CurRow - 1, "S");
                        xlApp.Cells[CurRow, 20] = String.Format("=SUM({2}{0},{2}{1})", CurRow - 2, CurRow - 1, "T");
                        xlApp.Cells[CurRow, 21] = String.Format("=SUM({2}{0},{2}{1})", CurRow - 2, CurRow - 1, "U");
                    }
                    ws.get_Range(ws.Cells[CurRow, 2], ws.Cells[CurRow, TotalCol]).Borders.get_Item(XlBordersIndex.xlEdgeBottom).LineStyle = XlLineStyle.xlDouble;

                    //change format

                    ((Range)ws.Cells[1, 1]).Font.Size = 16;

                    ((Range)ws.Cells[1, 2]).EntireColumn.WrapText = true;
                    ((Range)ws.Cells[1, 11]).EntireColumn.WrapText = true;

                    ((Range)ws.Cells[1, 3]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 3]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 4]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 5]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 6]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 7]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    ((Range)ws.Cells[1, 10]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 12]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 13]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 14]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 15]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 16]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 17]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 18]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 19]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 20]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    ((Range)ws.Cells[1, 21]).EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignRight;

                    ws.get_Range(ws.Cells[StartDataRow - 1, 1], ws.Cells[StartDataRow, TotalCol]).HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    ws.get_Range(ws.Cells[3, 1], ws.Cells[3, 1]).VerticalAlignment = XlVAlign.xlVAlignTop;
                    
                    ((Range)ws.Cells[1, 3]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 4]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 5]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 6]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 10]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 12]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 13]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 14]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 15]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 16]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 17]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 18]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 19]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 20]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    ((Range)ws.Cells[1, 21]).EntireColumn.NumberFormat = "#,##0.00_);[Red](#,##0.00)";
                    //
                    ws.get_Range(ws.Cells[1, 20], ws.Cells[1, 21]).NumberFormat = "dd-mmm-yy HH:mm";

                    //set column width
                    ((Range)ws.Cells[1, 1]).EntireColumn.ColumnWidth = 11;
                    ((Range)ws.Cells[1, 2]).EntireColumn.ColumnWidth = 55;
                    ((Range)ws.Cells[1, 3]).EntireColumn.ColumnWidth = 20;
                    ((Range)ws.Cells[1, 4]).EntireColumn.ColumnWidth = 20;
                    ((Range)ws.Cells[1, 5]).EntireColumn.ColumnWidth = 28;
                    ((Range)ws.Cells[1, 6]).EntireColumn.ColumnWidth = 28;
                    ((Range)ws.Cells[1, 7]).EntireColumn.ColumnWidth = 11;
                    ((Range)ws.Cells[1, 8]).EntireColumn.ColumnWidth = 17;
                    ((Range)ws.Cells[1, 9]).EntireColumn.ColumnWidth = 17;
                    ((Range)ws.Cells[1, 10]).EntireColumn.ColumnWidth = 18;
                    ((Range)ws.Cells[1, 11]).EntireColumn.ColumnWidth = 17.5;
                    ((Range)ws.Cells[1, 12]).EntireColumn.ColumnWidth = 24;
                    ((Range)ws.Cells[1, 13]).EntireColumn.ColumnWidth = 18;
                    ((Range)ws.Cells[1, 14]).EntireColumn.ColumnWidth = 16;
                    ((Range)ws.Cells[1, 15]).EntireColumn.ColumnWidth = 16;
                    ((Range)ws.Cells[1, 16]).EntireColumn.ColumnWidth = 16;
                    ((Range)ws.Cells[1, 17]).EntireColumn.ColumnWidth = 16;
                    ((Range)ws.Cells[1, 18]).EntireColumn.ColumnWidth = 16;
                    ((Range)ws.Cells[1, 19]).EntireColumn.ColumnWidth = 16;
                    ((Range)ws.Cells[1, 20]).EntireColumn.ColumnWidth = 24;
                    ((Range)ws.Cells[1, 21]).EntireColumn.ColumnWidth = 24;

                    //freeze window
                    xlApp.ActiveWindow.SplitRow = 8;
                    xlApp.ActiveWindow.SplitColumn = 2;
                    xlApp.ActiveWindow.FreezePanes = true;

                        ws.Name = "SC Summary";
                        wb.Saved = true;

                        Random nRandom = new Random(DateTime.Now.Millisecond);

                        string fname = Server.MapPath("..\\Temp\\QS47_" + nRandom.Next().ToString() + ".xls");
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

