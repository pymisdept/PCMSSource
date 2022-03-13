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

using System.Text.RegularExpressions;

using System.ComponentModel;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

using SimpleControls;
using SimpleControls.Web;
using System.IO;

public partial class Control_ACI03ExportExcel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CostCodeFr.Text = "00000";
            CostCodeTo.Text = "ZZZZZ";
            PrdChoice.Text = "1";
            PeriodFr.Text = "200901";
            PeriodTo.Text = "200912";
        }
    }
    static public bool IsNumeric(string target)
    {
        if (!Regex.IsMatch(target, "^[0-9.]+$"))
        {
            return false;
        }
        return true;
    }
    static public bool IsAlphabet(string target)
    {
        if (!Regex.IsMatch(target, "^[0-9a-zA-Z.]+$"))
        {
            return false;
        }
        return true;
    }
    protected void SetExcelValue(
        Excel._Worksheet oSheet,
        string type,
        int row,
        int col,
        object value,
        int number_of_decimal,
        string dateformat,
        string halignment,
        Boolean IsBold,
        int FontSize,
        System.Drawing.Color colour,
        Boolean border_top,
        Boolean border_bottom,
        Boolean border_left,
        Boolean border_right)
    {
        Excel.Range oRng = null;
        object Missing = System.Type.Missing;
        string temp = Convert.ToChar(col + Convert.ToChar('A') + 1) + row.ToString();
        if (type.ToUpper() == "N")
        {
            string decimalstring = "";
            for (int i = 1; i <= number_of_decimal; i++)
                decimalstring = decimalstring + "0";
            if (number_of_decimal > 0)
                oSheet.get_Range(temp, temp).NumberFormat = "#,##0." + decimalstring + "_);(#,##0." + decimalstring + ")";
            else
                oSheet.get_Range(temp, temp).NumberFormat = "#,##0_);(#,##0)";
        }
        else if (type.ToUpper() == "S")
        {
            oSheet.Cells[row, col] = "'" + value.ToString();
        }
        else if (type.ToUpper() == "D")
        {
            oRng.set_Value(Missing, value);
            oSheet.get_Range(temp, temp).NumberFormat = dateformat;
        }
        oSheet.get_Range(temp, temp).AutoFit();

        oSheet.get_Range(temp, temp).Font.Size = FontSize;
        if (IsBold == true)
            oSheet.get_Range(temp, temp).Font.Bold = true;
        if (border_top == true)
        {
            Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
            border.LineStyle = Excel.XlLineStyle.xlContinuous;
        }
        else if (border_bottom == true)
        {
            Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
            border.LineStyle = Excel.XlLineStyle.xlContinuous;
        }
        else if (border_left == true)
        {
            Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeLeft];
            border.LineStyle = Excel.XlLineStyle.xlContinuous;
        }
        else if (border_right == true)
        {
            Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeRight];
            border.LineStyle = Excel.XlLineStyle.xlContinuous;
        }
        if (halignment.ToUpper() == "R")
            oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
        else if (halignment.ToUpper() == "L")
            oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
        else if (halignment.ToUpper() == "C")
            oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

        oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(colour);
    }
    protected void ExportButton_Click(object sender, EventArgs e)
    {
        Error_Label.Text = "";
        if (String.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["SAP2"].ConnectionString) == true)
        {
            Error_Label.Text = "SAP2 can not be blank in configuration file";
            return;
        }
        if (String.IsNullOrEmpty(CostCodeFr.Text) == true)
        {
            Error_Label.Text = "Cost Code From can not be blank";
            return;
        }
        if (String.IsNullOrEmpty(CostCodeTo.Text) == true)
        {
            Error_Label.Text = "Cost Code To can not be blank";
            return;
        }
        if (String.IsNullOrEmpty(PrdChoice.Text) == true)
        {
            Error_Label.Text = "PrdChoice can not be blank";
            return;
        }
        if (String.IsNullOrEmpty(PeriodFr.Text) == true)
        {
            Error_Label.Text = "Period From can not be blank";
            return;
        }
        if (String.IsNullOrEmpty(PeriodTo.Text) == true)
        {
            Error_Label.Text = "Period To can not be blank";
            return;
        }
        if (IsAlphabet(CostCodeFr.Text) == false)
        {
            Error_Label.Text = "Cost code From is wrong alphabet";
            return;
        }
        if (IsAlphabet(CostCodeTo.Text) == false)
        {
            Error_Label.Text = "Cost code To is wrong alphabet";
            return;
        }
        if (IsAlphabet(PeriodFr.Text) == false)
        {
            Error_Label.Text = "PeriodFr is wrong alphabet";
            return;
        }
        if (IsAlphabet(PeriodTo.Text) == false)
        {
            Error_Label.Text = "PeriodTo is wrong alphabet";
            return;
        }
        if (IsAlphabet(PrdChoice.Text) == false)
        {
            Error_Label.Text = "PrdChoice is wrong alphabet";
            return;
        }
        if (CostCodeFr.Text.Length != 5 || CostCodeTo.Text.Length != 5)
        {
            Error_Label.Text = "Cost code length should be 5";
            return;
        }
        if (PeriodFr.Text.Length != 6 || PeriodTo.Text.Length != 6)
        {
            Error_Label.Text = "Period length should be 6";
            return;
        }
        if (IsNumeric(PrdChoice.Text) == false)
        {
            Error_Label.Text = "PrdChoice should be numeric";
            return;
        }

        if (PeriodFr.Text.Length == 6 && PeriodTo.Text.Length == 6)
        {
            DateTime result = DateTime.Now;
            if (DateTime.TryParse("1-" + PeriodFr.Text.Substring(4, 2) + "-" + PeriodFr.Text.Substring(0, 4), out result) == false)
            {
                Error_Label.Text = "Period From is not correct date format";
                return;
            }
            if (DateTime.TryParse("1-" + PeriodTo.Text.Substring(4, 2) + "-" + PeriodTo.Text.Substring(0, 4), out result) == false)
            {
                Error_Label.Text = "Period To is not correct date format";
                return;
            }
        }

        if (Convert.ToInt32(PeriodTo.Text) < Convert.ToInt32(PeriodFr.Text))
        {
            Error_Label.Text = "Period To cannot be earlier than Period From";
            return;
        }
        if (!String.IsNullOrEmpty(Request.QueryString["ProjectCode"]) && !String.IsNullOrEmpty(Request.QueryString["UserID"])
           && !String.IsNullOrEmpty(CostCodeFr.Text)
           && !String.IsNullOrEmpty(CostCodeTo.Text)
           && !String.IsNullOrEmpty(PrdChoice.Text)
           && !String.IsNullOrEmpty(PeriodFr.Text)
           && !String.IsNullOrEmpty(PeriodTo.Text)
           )
        {
            //File.AppendAllText(@"E:\MartinDebugAC03.txt", "Begin");
            //File.AppendAllText(@"E:\MartinDebugAC03.txt", Request.QueryString["ProjectCode"].ToString());
            //File.AppendAllText(@"E:\MartinDebugAC03.txt", Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", ""));
            //File.AppendAllText(@"E:\MartinDebugAC03.txt", Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", ""));
            

            Random ranObj = new Random();
            int start = 1;
            int end = 100;
            int number = ranObj.Next(start, end);

            int intRow = 1;
            object Missing = System.Type.Missing;
            Excel._Workbook oWB = null;
            Excel._Worksheet oSheet = null;
            Excel.Range oRng = null;
            Excel.Application oXL = new Excel.Application();

            string previous_cost_code = "";
            string previous_cost_desc = "";
            int CostCode_Row = 3;
            int Period_Row = 3;
            int Project_Row = 2;
            string TempStoreStr = "";
            string projectname = "";
            List<String> SubTotal_RowList = new List<String>();
            Boolean StartOrderItemOne = true;
            double subtotal = 0;
            double total = 0;
            int BeforeTotalRow = 0;

            try
            {
                oXL.Visible = false;
                oXL.DisplayAlerts = false;

                Boolean StartRow = true;
                //Get a new workbook.
                oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;
                //double Grant_Total_Of_InvAmt = 0;
                //double Grant_Total_Of_AmtDeduct = 0;
                //double Grant_Total_Of_DIFF = 0;

                //File.AppendAllText((@"E:\MartinExcelDebug.txt", "SAP2");
                string _connectionString = ConfigurationManager.ConnectionStrings["SAP2"].ConnectionString.ToString();
                //File.AppendAllText((@"E:\MartinExcelDebug.txt", "after SAP2");
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        TempStoreStr = "CREATE TABLE #TR_Rpt (SortLevel1              int,";
                        TempStoreStr = TempStoreStr + "PrjCode					nvarchar(8),";
	                    TempStoreStr = TempStoreStr + "PrjName					nvarchar(150),";
	                    TempStoreStr = TempStoreStr + "CostCode				nvarchar(15),";
	                    TempStoreStr = TempStoreStr + "CostCodeDesc		nvarchar(100),";
	                    TempStoreStr = TempStoreStr + "CostType				nvarchar(8),";
	                    TempStoreStr = TempStoreStr + "CostTypeDesc		nvarchar(30),";
	                    TempStoreStr = TempStoreStr + "Period						nvarchar(5),";
	                    TempStoreStr = TempStoreStr + "TrxDate					datetime,";
	                    TempStoreStr = TempStoreStr + "TrxType					nvarchar(120),";
	                    TempStoreStr = TempStoreStr + "Contact					nvarchar(15),";
	                    TempStoreStr = TempStoreStr + "ContactName		nvarchar(100),";
	                    TempStoreStr = TempStoreStr + "Ref							nvarchar(30),";
	                    TempStoreStr = TempStoreStr + "OrderRef				nvarchar(20),";
	                    TempStoreStr = TempStoreStr + "VoucherNo			nvarchar(100),";
	                    TempStoreStr = TempStoreStr + "BatchNo					nvarchar(20),";
	                    TempStoreStr = TempStoreStr + "TrxDesc					nvarchar(100),";
	                    TempStoreStr = TempStoreStr + "qty							numeric(19,6),";
	                    TempStoreStr = TempStoreStr + "amount					numeric(19,6), ";
                        TempStoreStr = TempStoreStr + "U_ReportCode            nvarchar(8), ";
                        TempStoreStr = TempStoreStr + "AC_PERIOD				nvarchar(100))";

                        SqlCommand command_a = new SqlCommand(TempStoreStr, connection);
                        connection.Open();

                        command_a.Connection = connection;
                        command_a.CommandTimeout = 60000;
                        command_a.ExecuteNonQuery();

                        //File.AppendAllText(@"E:\MartinDebugAC03.txt", "CreatTable");

                        TempStoreStr = "INSERT INTO #TR_Rpt exec sp_AC03_Detail '" + Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "");
                        TempStoreStr = TempStoreStr + "', '" + Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "");
                        TempStoreStr = TempStoreStr + "', '" + CostCodeFr.Text;
                        TempStoreStr = TempStoreStr + "', '" + CostCodeTo.Text;
                        TempStoreStr = TempStoreStr + "'," + Convert.ToInt32(PrdChoice.Text);
                        TempStoreStr = TempStoreStr + ",'" + PeriodFr.Text;
                        TempStoreStr = TempStoreStr + "','" + PeriodTo.Text+ "'";
    
                        SqlCommand command_b = new SqlCommand(TempStoreStr, connection);
                        command_b.Connection = connection;
                        command_b.CommandTimeout = 60000;
                        command_b.ExecuteNonQuery();

                        //File.AppendAllText(@"E:\MartinDebugAC03.txt", "Insert into");

                        TempStoreStr = "select z.SortLevel1, z.PrjCode, z.PrjName, z.CostCode, z.CostCodeDesc, z.U_ReportCode, z.CostType, z.CostTypeDesc, z.TrxDate, z.TrxType, z.Ref, z.BatchNo, z.VoucherNo, z.amount, z.AC_PERIOD, z.orderitem";
                        TempStoreStr = TempStoreStr + " from ( ";
                        TempStoreStr = TempStoreStr + " select SortLevel1, PrjCode, PrjName, CostCode, CostCodeDesc, U_ReportCode, CostType, CostTypeDesc, TrxDate, TrxType, Ref, BatchNo, VoucherNo, amount, AC_PERIOD, 1 as orderitem";
                        TempStoreStr = TempStoreStr + " FROM #TR_Rpt ";
                        TempStoreStr = TempStoreStr + " union all ";
                        //TempStoreStr = TempStoreStr + " select '' as SortLevel1, '' as PrjCode, '' as PrjName, CostCode, CostCodeDesc, '' as U_ReportCode, '' as CostType, '' as CostTypeDesc, '' as TrxDate, '' as TrxType, '' as Ref, '' as BatchNo, '' as VoucherNo, sum(amount), '' as AC_PERIOD, 2 as orderitem";
                        TempStoreStr = TempStoreStr + " select '' as SortLevel1, PrjCode, PrjName, CostCode, CostCodeDesc, U_ReportCode, CostType, CostTypeDesc, '' as TrxDate, '' as TrxType, '' as Ref, '' as BatchNo, '' as VoucherNo, sum(amount), '' as AC_PERIOD, 2 as orderitem";
                        //TempStoreStr = TempStoreStr + " FROM #TR_Rpt group by CostCode,CostCodeDesc ";
                        TempStoreStr = TempStoreStr + " FROM #TR_Rpt group by CostCode,CostCodeDesc, PrjCode, PrjName, U_ReportCode, CostType, CostTypeDesc ";
                        TempStoreStr = TempStoreStr + " ) as z order by z.CostCode, z.orderitem, z.TrxDate";
    
                        SqlCommand command_c = new SqlCommand(TempStoreStr, connection);

                        //File.AppendAllText(@"E:\MartinDebugAC03.txt", "Select");
                        /*
                        SqlCommand command = new SqlCommand("sp_AC03_Detail", connection);
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@FDocEntry", Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "")));
                        command.Parameters.Add(new SqlParameter("@TDocEntry", Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "")));

                        command.Parameters.Add(new SqlParameter("@CostCodeFr", CostCodeFr.Text));
                        command.Parameters.Add(new SqlParameter("@CostCodeTo", CostCodeTo.Text));
                        command.Parameters.Add(new SqlParameter("@PrdChoice", Convert.ToInt32(PrdChoice.Text)));
                        command.Parameters.Add(new SqlParameter("@PeriodFr", PeriodFr.Text));
                        command.Parameters.Add(new SqlParameter("@PeriodTo", PeriodTo.Text));
                        */
                        
                        command_c.Connection = connection;
                        command_c.CommandTimeout = 60000;

                        intRow = 1;
                        //oSheet.Cells[intRow, 3] = "Print Date and Time:" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                        //intRow = intRow + 1;
                        oSheet.Cells[intRow, 1] = "COST TRANSACTIONS BY NOMINATED PERIOD - ACCOUNT INFO";
                        oSheet.Cells[intRow, 2] = "(BY W/D DATE / VOC. DATE)";
                        if (PrdChoice.Text == "1")
                        {
                            oSheet.Cells[intRow, 2] = "(BY W/D DATE / VOC. DATE)";
                        }
                        else
                        {
                            oSheet.Cells[intRow, 2] = "(BY A/C PERIOD)";
                        }

                        oSheet.Cells[Project_Row, 1] = "Project :";
                        oSheet.Cells[Project_Row, 9] = "Base Currency: HKD";

                        oSheet.Cells[CostCode_Row, 1] = "Cost Code From";
                        oSheet.Cells[CostCode_Row, 2] = "'" + CostCodeFr.Text;
                        oSheet.Cells[CostCode_Row, 3] = "To";
                        oSheet.Cells[CostCode_Row, 4] = "'" + CostCodeTo.Text;
                        oSheet.Cells[Period_Row, 5] = "Period from";

                        

                        DateTime fromdate = DateTime.Now;
                        if (DateTime.TryParse("1-" + PeriodFr.Text.Substring(4, 2) + "-" + PeriodFr.Text.Substring(0, 4), out fromdate) == true)
                        {
                            if (PeriodFr.Text.Substring(4, 2) == "01")
                                oSheet.Cells[Period_Row, 6] = "'Jan/" + fromdate.ToString("yyyy");
                            else if (PeriodFr.Text.Substring(4, 2) == "02")
                                oSheet.Cells[Period_Row, 6] = "'Feb/" + fromdate.ToString("yyyy");
                            else if (PeriodFr.Text.Substring(4, 2) == "03")
                                oSheet.Cells[Period_Row, 6] = "'Mar/" + fromdate.ToString("yyyy");
                            else if (PeriodFr.Text.Substring(4, 2) == "04")
                                oSheet.Cells[Period_Row, 6] = "'Apr/" + fromdate.ToString("yyyy");
                            else if (PeriodFr.Text.Substring(4, 2) == "05")
                                oSheet.Cells[Period_Row, 6] = "'May/" + fromdate.ToString("yyyy");
                            else if (PeriodFr.Text.Substring(4, 2) == "06")
                                oSheet.Cells[Period_Row, 6] = "'Jun/" + fromdate.ToString("yyyy");
                            else if (PeriodFr.Text.Substring(4, 2) == "07")
                                oSheet.Cells[Period_Row, 6] = "'Jul/" + fromdate.ToString("yyyy");
                            else if (PeriodFr.Text.Substring(4, 2) == "08")
                                oSheet.Cells[Period_Row, 6] = "'Aug/" + fromdate.ToString("yyyy");
                            else if (PeriodFr.Text.Substring(4, 2) == "09")
                                oSheet.Cells[Period_Row, 6] = "'Sep/" + fromdate.ToString("yyyy");
                            else if (PeriodFr.Text.Substring(4, 2) == "10")
                                oSheet.Cells[Period_Row, 6] = "'Oct/" + fromdate.ToString("yyyy");
                            else if (PeriodFr.Text.Substring(4, 2) == "11")
                                oSheet.Cells[Period_Row, 6] = "'Nov/" + fromdate.ToString("yyyy");
                            else if (PeriodFr.Text.Substring(4, 2) == "12")
                                oSheet.Cells[Period_Row, 6] = "'Dec/" + fromdate.ToString("yyyy");
                        }
                        else
                        {
                            oSheet.Cells[Period_Row, 6] = "";
                        }
                        oSheet.Cells[Period_Row, 7] = "To";

                        DateTime todate = DateTime.Now;
                        if (DateTime.TryParse("1-" + PeriodTo.Text.Substring(4, 2) + "-" + PeriodTo.Text.Substring(0, 4), out todate) == true)
                        {
                            if( PeriodTo.Text.Substring(4, 2)== "01")
                                oSheet.Cells[Period_Row, 8] = "'Jan/" + todate.ToString("yyyy");
                            else if (PeriodTo.Text.Substring(4, 2) == "02")
                                oSheet.Cells[Period_Row, 8] = "'Feb/" + todate.ToString("yyyy");
                            else if (PeriodTo.Text.Substring(4, 2) == "03")
                                oSheet.Cells[Period_Row, 8] = "'Mar/" + todate.ToString("yyyy");
                            else if (PeriodTo.Text.Substring(4, 2) == "04")
                                oSheet.Cells[Period_Row, 8] = "'Apr/" + todate.ToString("yyyy");
                            else if (PeriodTo.Text.Substring(4, 2) == "05")
                                oSheet.Cells[Period_Row, 8] = "'May/" + todate.ToString("yyyy");
                            else if (PeriodTo.Text.Substring(4, 2) == "06")
                                oSheet.Cells[Period_Row, 8] = "'Jun/" + todate.ToString("yyyy");
                            else if (PeriodTo.Text.Substring(4, 2) == "07")
                                oSheet.Cells[Period_Row, 8] = "'Jul/" + todate.ToString("yyyy");
                            else if (PeriodTo.Text.Substring(4, 2) == "08")
                                oSheet.Cells[Period_Row, 8] = "'Aug/" + todate.ToString("yyyy");
                            else if (PeriodTo.Text.Substring(4, 2) == "09")
                                oSheet.Cells[Period_Row, 8] = "'Sep/" + todate.ToString("yyyy");
                            else if (PeriodTo.Text.Substring(4, 2) == "10")
                                oSheet.Cells[Period_Row, 8] = "'Oct/" + todate.ToString("yyyy");
                            else if (PeriodTo.Text.Substring(4, 2) == "11")
                                oSheet.Cells[Period_Row, 8] = "'Nov/" + todate.ToString("yyyy");
                            else if (PeriodTo.Text.Substring(4, 2) == "12")
                                oSheet.Cells[Period_Row, 8] = "'Dec/" + todate.ToString("yyyy");
                        }
                        else
                        {
                            oSheet.Cells[Period_Row, 8] = PeriodTo.Text;
                        }
                        oSheet.Cells[Period_Row, 9] = "# Indicates Foreign Currency transaction";

                        intRow = Period_Row + 2;

                        /*          
          SortLevel1
          PrjCode
          PrjName
          CostCode
          CostCodeDesc
          CostType
          CostTypeDesc
          Period
          TrxDate
          TrxType
          Contact
          ContactName
          Ref
          OrderRef
          VoucherNo
          BatchNo
          TrxDesc
          qty
          amount
          U_ReportCode
          AC_PERIOD
                         */
                        //File.AppendAllText(@"E:\MartinDebugAC03.txt", "before command_c");
                        using (SqlDataReader datareader = command_c.ExecuteReader())
                        {
                            while (datareader.Read())
                            {
                                if (datareader.HasRows == true)
                                {
                                    if (StartRow == true)
                                    {
                                        //File.AppendAllText(@"E:\MartinDebugAC03.txt", "in command_c");
                                        StartRow = false;
                                        for (int i = 0; i < datareader.FieldCount; i++)
                                        {
                                            if (datareader.GetName(i) == "CostCode")
                                            {
                                                oSheet.Cells[intRow, 1] = "Cost Code";
                                            }
                                            if (datareader.GetName(i) == "CostCodeDesc")
                                            {
                                                oSheet.Cells[intRow, 2] = "Description";
                                            }
                                            if (datareader.GetName(i) == "U_ReportCode")
                                            {
                                                oSheet.Cells[intRow, 3] = "Report Code";
                                            }
                                            if (datareader.GetName(i) == "CostType")
                                            {
                                                oSheet.Cells[intRow, 4] = "Cost Type";
                                            }

                                            if (datareader.GetName(i) == "CostTypeDesc")
                                            {
                                                oSheet.Cells[intRow, 5] = "Description";
                                            }
                                            //***************
                                            if (datareader.GetName(i) == "TrxDate")
                                            {
                                                oSheet.Cells[intRow, 6] = "W/D Date/VOC Date";
                                            }

                                            if (datareader.GetName(i) == "AC_PERIOD")
                                            {
                                                oSheet.Cells[intRow, 7] = "A/C PERIOD";
                                            }
                                            if (datareader.GetName(i) == "TrxType")
                                            {
                                                oSheet.Cells[intRow, 8] = "Particular";
                                            }

                                            if (datareader.GetName(i) == "Ref")
                                            {
                                                oSheet.Cells[intRow, 9] = "Cert. No./PCMS No.";
                                            }
                                            if (datareader.GetName(i) == "VoucherNo")
                                            {
                                                oSheet.Cells[intRow, 10] = "Voucher No.";

                                                string temp = "E" + intRow.ToString();
                                                Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                            }

                                            if (datareader.GetName(i) == "amount")
                                            {
                                                oSheet.Cells[intRow, 11] = "Amount";
                                                oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                            }

                                            Excel.Border border5 = oSheet.get_Range("A" + intRow.ToString(), "K" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                            border5.LineStyle = Excel.XlLineStyle.xlContinuous;
                                            //***********************
                                        }
                                        oSheet.get_Range("A" + intRow.ToString(), "K" + intRow.ToString()).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                                        intRow = intRow + 1;
                                    }
                                    if (datareader["CostCode"] != System.DBNull.Value)
                                    {
                                        //Subtotal
                                        //if (previous_cost_code != "" && previous_cost_code != datareader["CostCode"].ToString())
                                        if (datareader["orderitem"].ToString() == "2")
                                        {
                                            //oSheet.Cells[intRow, 1] = previous_cost_code;
                                            oSheet.Cells[intRow, 1] = datareader["CostCode"].ToString();

                                            oSheet.Cells[intRow, 7] = "Total for " + previous_cost_desc;
                                            oSheet.get_Range("G" + intRow.ToString(), "G" + intRow.ToString()).Font.Bold = true;
                                            SubTotal_RowList.Add(intRow.ToString());

                                            oSheet.Cells[intRow, 8] = subtotal.ToString();
                                            oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                                            subtotal = 0;

                                            string temp5 = "H" + intRow.ToString();
                                            Excel.Border border5 = oSheet.get_Range(temp5, temp5).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                            border5.LineStyle = Excel.XlLineStyle.xlContinuous;

                                            //intRow = intRow + 1;

                                            StartOrderItemOne = true;
                                        }
                                        //if (previous_cost_code == "" || previous_cost_code != datareader["CostCode"].ToString())
                                        //if (datareader["orderitem"].ToString() == "1" && StartOrderItemOne == true)
                                        //{
                                            //StartOrderItemOne = false;
                                            if (datareader["PrjCode"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[Project_Row, 2] = datareader["PrjCode"].ToString();
                                            }
                                            if (datareader["PrjName"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[Project_Row, 3] = datareader["PrjName"].ToString();
                                                projectname = datareader["PrjName"].ToString();
                                            }
                                            //Heading Data
                                            if (datareader["CostCode"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[intRow, 1] = datareader["CostCode"].ToString();

                                                if (previous_cost_code != datareader["CostCode"].ToString() || String.IsNullOrEmpty(previous_cost_code))
                                                {
                                                    oSheet.get_Range("A" + intRow.ToString(), "A" + intRow.ToString()).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                                    oSheet.get_Range("B" + intRow.ToString(), "B" + intRow.ToString()).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                                }

                                                previous_cost_code = datareader["CostCode"].ToString();
                                            }
                                            if (datareader["CostCodeDesc"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[intRow, 2] = datareader["CostCodeDesc"].ToString();    
                                                previous_cost_desc = datareader["CostCodeDesc"].ToString();
                                            }
                                            if (datareader["U_ReportCode"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[intRow, 3] = datareader["U_ReportCode"].ToString();
                                            }
                                            if (datareader["CostType"] != System.DBNull.Value)
                                                oSheet.Cells[intRow, 4] = datareader["CostType"].ToString();
                                            if (datareader["CostTypeDesc"] != System.DBNull.Value)
                                                oSheet.Cells[intRow, 5] = datareader["CostTypeDesc"].ToString();

                                        //}
                                        if (datareader["orderitem"].ToString() == "1")
                                        {
                                            if (datareader["TrxDate"] != System.DBNull.Value)
                                            {
                                                //oSheet.Cells[intRow, 6] = Convert.ToDateTime(datareader["TrxDate"]);
                                                oRng = oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString());
                                                oRng.set_Value(Missing, datareader["TrxDate"]);
                                                oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString()).NumberFormat = "dd-MMM-yyyy";

                                                //oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            }
                                            if (datareader["AC_PERIOD"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[intRow, 7] = "'" + Convert.ToDateTime(datareader["AC_PERIOD"]).ToString("MMM") + "/" + Convert.ToDateTime(datareader["AC_PERIOD"]).Year.ToString();
                                                //oSheet.get_Range("G" + intRow.ToString(), "G" + intRow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            }
                                            //else
                                                //oSheet.get_Range("G" + intRow.ToString(), "G" + intRow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                                            if (datareader["TrxType"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[intRow, 8] = datareader["TrxType"].ToString();
                                                //oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            }
                                            //else
                                                //oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                                            if (datareader["Ref"] != System.DBNull.Value)
                                            {
                                                if (String.IsNullOrEmpty(datareader["Ref"].ToString()) == true)
                                                    oSheet.Cells[intRow, 9] = "'0";
                                                else
                                                    oSheet.Cells[intRow, 9] = datareader["Ref"].ToString();
                                                //oSheet.get_Range("I" + intRow.ToString(), "I" + intRow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            }
                                            else
                                            {
                                                oSheet.Cells[intRow, 9] = "'0";
                                                //oSheet.get_Range("I" + intRow.ToString(), "I" + intRow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            }

                                            if (datareader["VoucherNo"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[intRow, 10] = datareader["VoucherNo"].ToString();
                                                //oSheet.get_Range("J" + intRow.ToString(), "J" + intRow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            }
                                            //else
                                                //oSheet.get_Range("J" + intRow.ToString(), "J" + intRow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            //else
                                            //oSheet.Cells[intRow, 4] = "0";

                                            if (datareader["amount"] != System.DBNull.Value)
                                            {
                                                /*
                                                SetExcelValue(
                                                    oSheet,
                                                    "N",
                                                    intRow,
                                                    6,
                                                    datareader["amount"],
                                                    2,
                                                    "",
                                                    "R",
                                                    false,
                                                    10,
                                                    System.Drawing.Color.White,
                                                    false,
                                                    false,
                                                    false,
                                                    false);
                                                */
                                                oSheet.Cells[intRow, 11] = datareader["amount"].ToString();
                                                oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                                if (!String.IsNullOrEmpty(datareader["amount"].ToString()))
                                                {
                                                    subtotal = subtotal + Convert.ToDouble(datareader["amount"].ToString());
                                                    total = total + Convert.ToDouble(datareader["amount"].ToString());
                                                }
                                                //oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            }
                                        }
                                    }
                                    
                                    intRow = intRow + 1;
                                }
                            }
                        }
                        TempStoreStr = "drop table #TR_Rpt";
                        SqlCommand command_d = new SqlCommand(TempStoreStr, connection);
                        command_d.Connection = connection;
                        command_d.CommandTimeout = 60000;
                        command_d.ExecuteNonQuery();

                        oSheet.get_Range("A5", "A5").AutoFilter(5, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                        oSheet.get_Range("B5", "B5").AutoFilter(5, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                        oSheet.get_Range("C5", "C5").AutoFilter(5, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                        oSheet.get_Range("D5", "D5").AutoFilter(5, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                        oSheet.get_Range("E5", "E5").AutoFilter(5, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                        oSheet.get_Range("F5", "F5").AutoFilter(5, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                        oSheet.get_Range("G5", "G5").AutoFilter(5, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                        oSheet.get_Range("H5", "H5").AutoFilter(5, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                        oSheet.get_Range("I5", "I5").AutoFilter(5, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                        oSheet.get_Range("J5", "J5").AutoFilter(5, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                        oSheet.get_Range("K5", "K5").AutoFilter(5, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);

                        oRng = oSheet.get_Range("A6", "A65536");
                        oRng.Font.Name = "Courier New";
                        oRng = oSheet.get_Range("C6", "G65536");
                        oRng.Font.Name = "Courier New";

                        oRng = oSheet.get_Range("B6", "B65536");
                        oRng.Font.Name = "Times New Roman";

                        oRng = oSheet.get_Range("H6", "K65536");
                        oRng.Font.Name = "Times New Roman";
                        
                        oRng = oSheet.get_Range("A1", "K65536");
                        oRng.EntireColumn.AutoFit();
                        /*
                        oSheet.Columns.HorizontalAlignment = 1;
                        oSheet.Columns.VerticalAlignment = -4160;
                        oSheet.Columns.Orientation = 0;
                        oSheet.Columns.AddIndent = false;
                        oSheet.Columns.IndentLevel = 0;
                        oSheet.Columns.ShrinkToFit = false;
                        oSheet.Columns.ReadingOrder = -5002;
                        oSheet.Columns.MergeCells = false;
                        oSheet.Columns.Font.Name = "Arial";
                        oSheet.Columns.Font.FontStyle = "Normal";
                        oSheet.Columns.Font.Size = 10;
                        oSheet.Columns.Font.Strikethrough = false;
                        oSheet.Columns.Font.Superscript = false;
                        oSheet.Columns.Font.Subscript = false;
                        oSheet.Columns.Font.OutlineFont = false;
                        oSheet.Columns.Font.Shadow = false;
                        oSheet.Columns.Font.Underline = -4142;
                        oSheet.Columns.Font.ColorIndex = -4105;
                        */
                        // final group's subtotal
                        /*
                        oSheet.Cells[intRow, 1] = previous_cost_code;
                        oSheet.Cells[intRow, 3] = "Total for " + previous_cost_desc;
                        oSheet.get_Range("C" + intRow.ToString(), "C" + intRow.ToString()).Font.Bold = true;

                        oSheet.Cells[intRow, 6] = subtotal.ToString();
                        oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                        subtotal = 0;

                        string temp10 = "F" + intRow.ToString();
                        Excel.Border border10 = oSheet.get_Range(temp10, temp10).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border10.LineStyle = Excel.XlLineStyle.xlContinuous;
                        */
                        // Whole Total
                        intRow = intRow + 1;
                        oSheet.Cells[intRow, 3] = "Total for " + projectname;
                        oSheet.get_Range("C" + intRow.ToString(), "C" + intRow.ToString()).Font.Bold = true;

                        //oSheet.Cells[intRow, 11] = total.ToString();
                        BeforeTotalRow = (intRow - 1);
                        oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).Formula = "=SUBTOTAL(9,K6:K" + (intRow - 1).ToString() + ")";
                        oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                        Excel.Border borderfinalsum = oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        borderfinalsum.LineStyle = Excel.XlLineStyle.xlDouble;

                        string temp12 = "K" + intRow.ToString();
                        Excel.Border border12 = oSheet.get_Range(temp12, temp12).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border12.LineStyle = Excel.XlLineStyle.xlContinuous;

                        foreach(string s in SubTotal_RowList)
                            oSheet.get_Range("G" + s, "G" + s).Font.Bold = true;
                        
                        intRow = intRow + 2;
                        StartRow = true;
                        total = 0;

                        SqlCommand command2 = new SqlCommand("sp_AC03_Detail_CostTypeSummary", connection);
                        command2.CommandType = CommandType.StoredProcedure;
                        command2.Parameters.Add(new SqlParameter("@FDocEntry", Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "")));
                        command2.Parameters.Add(new SqlParameter("@TDocEntry", Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "")));

                        command2.Parameters.Add(new SqlParameter("@CostCodeFr", CostCodeFr.Text));
                        command2.Parameters.Add(new SqlParameter("@CostCodeTo", CostCodeTo.Text));
                        command2.Parameters.Add(new SqlParameter("@PrdChoice", Convert.ToInt32(PrdChoice.Text)));
                        command2.Parameters.Add(new SqlParameter("@PeriodFr", PeriodFr.Text));
                        command2.Parameters.Add(new SqlParameter("@PeriodTo", PeriodTo.Text));


                        command2.Connection = connection;
                        command2.CommandTimeout = 60000;

                        using (SqlDataReader datareader = command2.ExecuteReader())
                        {
                            while (datareader.Read())
                            {
                                if (datareader.HasRows == true)
                                {
                                    if (StartRow == true)
                                    {
                                        StartRow = false;
                                        for (int i = 0; i < datareader.FieldCount; i++)
                                        {
                                            if (datareader.GetName(i) == "CostType")
                                            {
                                                oSheet.Cells[intRow, 3] = "Cost Type";

                                                string temp = "C" + intRow.ToString();
                                                Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                            }
                                            if (datareader.GetName(i) == "CostTypeDesc")
                                            {
                                                oSheet.Cells[intRow, 6] = "Cost Type Description";

                                                string temp = "F" + intRow.ToString();
                                                Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                            }
                                            if (datareader.GetName(i) == "Tot_Amount")
                                            {
                                                oSheet.Cells[intRow, 8] = "Amount";

                                                oRng = oSheet.get_Range("C" + intRow.ToString(), "H" + intRow.ToString());
                                                oRng.Font.Name = "Times New Roman";
                                                oRng.Font.Size = 12;

                                                oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                                //oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                                                string temp = "H" + intRow.ToString();
                                                Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                intRow = intRow + 1;

                                            }
                                        }
                                    }
                                    if (datareader["CostType"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 3] = datareader["CostType"].ToString();
                                        oRng = oSheet.get_Range("C" + intRow.ToString(), "C" + intRow.ToString());
                                        oRng.Font.Name = "Times New Roman";
                                        oRng.Font.Size = 12;
                                    }
                                    if (datareader["CostTypeDesc"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 6] = datareader["CostTypeDesc"].ToString();
                                        oRng = oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString());
                                        oRng.Font.Name = "Times New Roman";
                                        oRng.Font.Size = 12;
                                    }
                                    if (datareader["Tot_Amount"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 8] = datareader["Tot_Amount"].ToString();
                                        oRng = oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString());
                                        oRng.Font.Name = "Times New Roman";
                                        oRng.Font.Size = 12;

                                        oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        total = total + Convert.ToDouble(datareader["Tot_Amount"].ToString());
                                    }
                                    intRow = intRow + 1;
                                }
                            }
                        }
                        oSheet.Cells[intRow, 8] = total.ToString();
                        oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        oRng = oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString());
                        oRng.Font.Name = "Times New Roman";
                        oRng.Font.Size = 12;

                        string temp18 = "H" + intRow.ToString();
                        Excel.Border border18 = oSheet.get_Range(temp18, temp18).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border18.LineStyle = Excel.XlLineStyle.xlDouble;
                        Excel.Border border19 = oSheet.get_Range(temp18, temp18).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border19.LineStyle = Excel.XlLineStyle.xlContinuous;

                        intRow = intRow + 2;
                        StartRow = true;
                        total = 0;

                        SqlCommand command3 = new SqlCommand("sp_AC03_Detail_ReportCodeSummary", connection);
                        command3.CommandType = CommandType.StoredProcedure;
                        command3.Parameters.Add(new SqlParameter("@FDocEntry", Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "")));
                        command3.Parameters.Add(new SqlParameter("@TDocEntry", Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "")));

                        command3.Parameters.Add(new SqlParameter("@CostCodeFr", CostCodeFr.Text));
                        command3.Parameters.Add(new SqlParameter("@CostCodeTo", CostCodeTo.Text));
                        command3.Parameters.Add(new SqlParameter("@PrdChoice", Convert.ToInt32(PrdChoice.Text)));
                        command3.Parameters.Add(new SqlParameter("@PeriodFr", PeriodFr.Text));
                        command3.Parameters.Add(new SqlParameter("@PeriodTo", PeriodTo.Text));


                        command3.Connection = connection;
                        command3.CommandTimeout = 60000;

                        int secondbeginrow = 0;
                        using (SqlDataReader datareader = command3.ExecuteReader())
                        {
                            while (datareader.Read())
                            {
                                if (datareader.HasRows == true)
                                {
                                    if (StartRow == true)
                                    {
                                        StartRow = false;
                                        secondbeginrow = intRow;
                                        for (int i = 0; i < datareader.FieldCount; i++)
                                        {
                                            if (datareader.GetName(i) == "ReportCode")
                                            {
                                                oSheet.Cells[intRow, 3] = "Report Code";

                                                //string temp = "C" + intRow.ToString();
                                                //Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                //border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                            }
                                            if (datareader.GetName(i) == "ReportCodeName")
                                            {
                                                oSheet.Cells[intRow, 6] = "Report Code Description";

                                                //string temp = "G" + intRow.ToString();
                                                //Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                //border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                            }
                                            if (datareader.GetName(i) == "Tot_Amount")
                                            {
                                                oSheet.Cells[intRow, 8] = "Amount";
                                                oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                                                //oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                                                //string temp = "H" + intRow.ToString();


                                                oSheet.Cells[intRow, 11] = "Reference";

                                                oRng = oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString());
                                                oRng.Font.Name = "Times New Roman";
                                                oRng.Font.Size = 12;

                                                Excel.Border border = oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                border = oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                border = oSheet.get_Range("C" + intRow.ToString(), "C" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                border = oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                oRng = oSheet.get_Range("C" + intRow.ToString(), "H" + intRow.ToString());
                                                oRng.Font.Name = "Times New Roman";
                                                oRng.Font.Size = 12;

                                                intRow = intRow + 1;
                                            }
                                        }
                                    }
                                    if (datareader["ReportCode"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 3] = datareader["ReportCode"].ToString();
                                        oRng = oSheet.get_Range("C" + intRow.ToString(), "C" + intRow.ToString());
                                        oRng.Font.Name = "Times New Roman";
                                        oRng.Font.Size = 12;
                                    }
                                    if (datareader["ReportCodeName"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 6] = datareader["ReportCodeName"].ToString();
                                        oRng = oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString());
                                        oRng.Font.Name = "Times New Roman";
                                        oRng.Font.Size = 12;
                                    }
                                    if (datareader["Tot_Amount"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 8] = datareader["Tot_Amount"].ToString();
                                        oRng = oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString());
                                        oRng.Font.Name = "Times New Roman";
                                        oRng.Font.Size = 12;
                                        oRng = oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString());
                                        oRng.Font.Name = "Times New Roman";
                                        oRng.Font.Size = 12;

                                        oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                                        oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).Formula = "=+SUMIF($C$5:$C$" + BeforeTotalRow.ToString() + ",C" + intRow.ToString() + ",$K$5:$K$" + BeforeTotalRow.ToString() + ")";

                                        oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                                        oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00);-";
                                        total = total + Convert.ToDouble(datareader["Tot_Amount"].ToString());
                                    }
                                    intRow = intRow + 1;
                                }
                            }
                        }
                        oSheet.Cells[intRow, 8] = total.ToString();
                        oRng = oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString());
                        oRng.Font.Name = "Times New Roman";
                        oRng.Font.Size = 12;

                        oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                        oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).Formula = "=SUM($K$" + (secondbeginrow + 1).ToString() + ":$K$" + (intRow-1).ToString() + ")";

                        oRng = oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString());
                        oRng.Font.Name = "Times New Roman";
                        oRng.Font.Size = 12;

                        Excel.Border borderfinalsum2 = oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        borderfinalsum2.LineStyle = Excel.XlLineStyle.xlDouble;
                        borderfinalsum2 = oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        borderfinalsum2.LineStyle = Excel.XlLineStyle.xlContinuous;

                        oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        string temp11 = "H" + intRow.ToString();
                        Excel.Border border20 = oSheet.get_Range(temp11, temp11).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border20.LineStyle = Excel.XlLineStyle.xlDouble;
                        Excel.Border border21 = oSheet.get_Range(temp11, temp11).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border21.LineStyle = Excel.XlLineStyle.xlContinuous;

                        command_a.Dispose();
                        command_b.Dispose();
                        command_c.Dispose();
                        command_d.Dispose();
                        command2.Dispose();
                        command3.Dispose();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        connection.Close();
                    }


                    intRow = intRow + 3;
                    oSheet.Cells[intRow, 1] = "End of Report";
                    intRow = intRow + 1;
                    //oSheet.Cells[intRow, 1] = "User Name : " + Request.QueryString["UserName"].ToString() + " Page -1 of " + oSheet.PageSetup.Pages.Count.ToString() + " " + " Print Date : " + DateTime.Now.ToString();
                    oSheet.Cells[intRow, 1] = "User Name : " + Request.QueryString["UserName"].ToString().Replace("_", " ") + " Print Date : " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");

                    //oSheet.PageSetup.CenterFooter = "Page &[Page] of $[Pages]";

                    ((Excel.Range)oSheet.Columns["A", Missing]).ColumnWidth = 8.5;
                    ((Excel.Range)oSheet.Columns["B", Missing]).ColumnWidth = 18.5;
                    ((Excel.Range)oSheet.Columns["C", Missing]).ColumnWidth = 5.7;
                    ((Excel.Range)oSheet.Columns["D", Missing]).ColumnWidth = 3.3;
                    ((Excel.Range)oSheet.Columns["E", Missing]).ColumnWidth = 1.3;
                    ((Excel.Range)oSheet.Columns["F", Missing]).ColumnWidth = 13.2;
                    ((Excel.Range)oSheet.Columns["G", Missing]).ColumnWidth = 9.5;
                    ((Excel.Range)oSheet.Columns["H", Missing]).ColumnWidth = 26.2;
                    ((Excel.Range)oSheet.Columns["I", Missing]).ColumnWidth = 18.3;
                    ((Excel.Range)oSheet.Columns["J", Missing]).ColumnWidth = 11.2;
                    ((Excel.Range)oSheet.Columns["K", Missing]).ColumnWidth = 17;

                    /*
                    if (oSheet.Cells[3, 1] == "Project Name")
                    {
                        oRng = oSheet.get_Range("A1", "A65536");
                        oRng.ColumnWidth = 114;
                    }
                    */
                    oRng = oSheet.get_Range("A1", "A65536");
                    //oXL.Visible = true;
                }
                oXL.Visible = true;

                //File.AppendAllText(@"E:\MartinDebugAC03.txt", "before save");

                //File.AppendAllText((@"E:\MartinExcelDebug.txt", "before save Excel");
                string fname = Server.MapPath("..\\ReportsFile\\AC03ExportFile" + number.ToString() + ".xls");
                if (System.IO.File.Exists(fname))
                {
                    System.IO.File.Delete(fname);
                    oWB.SaveCopyAs(fname);
                }
                else
                    oWB.SaveCopyAs(fname);

                //File.AppendAllText(@"E:\MartinDebugAC03.txt", "after save");
                oWB.Close(null, null, null);
                oXL.Workbooks.Close();
                oXL.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(oXL);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oWB);
                //Console.WriteLine("Finsihed");
            }
            catch (Exception ex)
            {
                //File.AppendAllText(@"E:\MartinDebugAC03.txt", ex.Message.ToString());
                Console.WriteLine(ex.Message.ToString());
                //Error_Label.Text = "No file generated! " + " Error Occur - Generation of Excel " + ex.Message;
                oWB.Close(null, null, null);
                oXL.Workbooks.Close();
                oXL.Quit();
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(oRng);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oXL);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oWB);
                oSheet = null;
                oWB = null;
                oXL = null;
                GC.Collect();
            }
            //this.Close();
            //Console.ReadKey();

            //File.AppendAllText((@"E:\MartinExcelDebug.txt", "before download");
            string path = Server.MapPath("..").Replace("\\", "/");
            SimpleWebUtils.DownloadFile(Page.Response, path + "/ReportsFile/AC03ExportFile" + number.ToString() + ".xls");
            //File.AppendAllText((@"E:\MartinExcelDebug.txt", "after download");
        }
    }
}
