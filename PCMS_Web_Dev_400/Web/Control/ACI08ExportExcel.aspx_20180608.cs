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
            PrdChoice.Text = "2";
            PeriodFr.Text = "201102";
            //PeriodTo.Text = "200912";
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
        object Missing = System.Type.Missing;
        Excel.Range oRng = null;
        string temp = Convert.ToChar(col+Convert.ToChar('A')+1) + row.ToString();
        if (type.ToUpper() == "N")
        {
            string decimalstring = "";
            for (int i = 1; i <= number_of_decimal; i++)
                decimalstring = decimalstring + "0";
            if(number_of_decimal > 0)
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
        if( IsBold == true)
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
        if(halignment.ToUpper() == "R")
            oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
        else if(halignment.ToUpper() == "L")
            oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
        else if(halignment.ToUpper() == "C")
            oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

        oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(colour);
    }
    protected void ExportButton_Click(object sender, EventArgs e)
    {
        Error_Label.Text = "";
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
        if (IsNumeric(PrdChoice.Text) == false)
        {
            Error_Label.Text = "PrdChoice should be numeric";
            return;
        }
        if (PeriodFr.Text.Length == 6)
        {
            DateTime result = DateTime.Now;
            if (DateTime.TryParse("20/" + PeriodFr.Text.Substring(4, 2) + "/" + PeriodFr.Text.Substring(0, 4), out result) == false)
            {
                Error_Label.Text = "Period From is not correct date format";
                return;
            }
        }
        if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["ACI03CrystalReportPath"]) == true)
        {
            Error_Label.Text = "configuration file ACI03CrystalReportPath can not be blank";
            return;
        }
        if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["BackEndServer"]) == true)
        {
            Error_Label.Text = "configuration file BackEndServer can not be blank";
            return;
        }
        if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["BackEndServerUser"]) == true)
        {
            Error_Label.Text = "configuration file BackEndServerUser can not be blank";
            return;
        }
        if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["BackEndServerPassword"]) == true)
        {
            Error_Label.Text = "configuration file BackEndServerPassword can not be blank";
            return;
        }
        if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["PCMS800"]) == true)
        {
            Error_Label.Text = "configuration file PCMS800 can not be blank";
            return;
        }
        if (!String.IsNullOrEmpty(Request.QueryString["ProjectCode"]) && !String.IsNullOrEmpty(Request.QueryString["UserID"])
           && !String.IsNullOrEmpty(CostCodeFr.Text)
           && !String.IsNullOrEmpty(CostCodeTo.Text)
           && !String.IsNullOrEmpty(PrdChoice.Text)
           && !String.IsNullOrEmpty(PeriodFr.Text)
           )
        {
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


            int CostCode_Row = 3;
            int Period_Row = 3;
            int Project_Row = 2;
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
                String temp = "";
                long OpeningBalanceTotalRow = 0;
                double OpeningBalanceTotal = 0;
                double Period1Total = 0;
                double Period2Total = 0;
                double Period3Total = 0;
                double Period4Total = 0;
                double Period5Total = 0;
                double Period6Total = 0;
                double Period7Total = 0;
                double Period8Total = 0;
                double Period9Total = 0;
                double Period10Total = 0;
                double Period11Total = 0;
                double Period12Total = 0;
                double EndingBalanceTotal = 0;

                //File.AppendAllText((@"E:\MartinExcelDebug.txt", "SAP2");
                string _connectionString = ConfigurationManager.ConnectionStrings["SAP2"].ConnectionString.ToString();
                //File.AppendAllText((@"E:\MartinExcelDebug.txt", "after SAP2");
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        Excel.Border border = oSheet.get_Range("A1", "A1").Borders[Excel.XlBordersIndex.xlEdgeBottom];

                        
                        SqlCommand command = new SqlCommand("sp_AC03_Detail_Cost_Summary", connection);
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("FPrjCode", Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "")));
                        //command.Parameters.Add(new SqlParameter("@TDocEntry", Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "")));

                        command.Parameters.Add(new SqlParameter("@CostCodeFr", CostCodeFr.Text));
                        command.Parameters.Add(new SqlParameter("@CostCodeTo", CostCodeTo.Text));
                        command.Parameters.Add(new SqlParameter("@PrdChoice", Convert.ToInt32(PrdChoice.Text)));
                        command.Parameters.Add(new SqlParameter("@PeriodFr", PeriodFr.Text));

                        connection.Open();
                        command.Connection = connection;
                        command.CommandTimeout = 60000;
                                        
                        intRow = 1;
                        //oSheet.Cells[intRow, 3] = "Print Date and Time:" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                        //intRow = intRow + 1;
                        oSheet.Cells[intRow, 1] = "PROJECT COST SUMMARY BY MONTH";
                        //oSheet.Cells[intRow, 2] = "(BY W/D DATE / VOC. DATE)";

                        oSheet.Cells[Project_Row, 1] = "Project :";
                        oSheet.Cells[Project_Row, 11] = "Base Currency: HKD";

                        oSheet.Cells[CostCode_Row, 1] = "Cost Code From:";
                        oSheet.Cells[CostCode_Row, 2] = "'" + CostCodeFr.Text;
                        oSheet.Cells[CostCode_Row, 3] = "To";
                        oSheet.Cells[CostCode_Row, 4] = "'" + CostCodeTo.Text;
                        oSheet.Cells[Period_Row, 5] = "Filter Condition : ";
                        oSheet.Cells[Period_Row, 6] = PrdChoice.Text;

                        oSheet.Cells[Period_Row, 7] = "Current Period";
                        
                        oSheet.Cells[Period_Row, 9] = "-";

                        oSheet.Cells[Period_Row, 11] = "# Indicates Foreign Currency transaction";

                        intRow = Period_Row + 2;

                        using (SqlDataReader datareader = command.ExecuteReader())
                        {
                            while (datareader.Read())
                            {
                                if (datareader.HasRows == true)
                                {
                                    if (StartRow == true)
                                    {
                                        StartRow = false;
                                        OpeningBalanceTotalRow = intRow + 1;
                                        for (int i = 0; i < datareader.FieldCount; i++)
                                        {
                                            if (datareader.GetName(i) == "U_ReportCode")
                                            {
                                                oSheet.Cells[intRow, 1] = "Report Code";
                                                oSheet.get_Range("A5", "A5").HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                            }
                                            if (datareader.GetName(i) == "CostCode")
                                            {
                                                oSheet.Cells[intRow, 2] = "Cost Code";
                                                oSheet.get_Range("B5", "B5").HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                            }
                                            if (datareader.GetName(i) == "CostCodeDesc")
                                            {
                                                oSheet.Cells[intRow, 3] = "Description";
                                                oSheet.get_Range("C5", "C5").HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                            }
                                            
                                            if (datareader.GetName(i) == "opening_balance")
                                            {
                                                oSheet.Cells[intRow, 4] = "Opening Balance";
                                            }

                                            if (datareader["Period1"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period1"].ToString().Substring(4, 2) + "/" + datareader["Period1"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 5] = "'" + headerdate.ToString("MMM") + " " + datareader["Period1"].ToString().Substring(0, 4);

                                                //DateTime fromdate = DateTime.Now;
                                                //DateTime.TryParse("20/" + PeriodFr.Text.Substring(4, 2) + "/" + PeriodFr.Text.Substring(0, 4), out fromdate);

                                                oSheet.Cells[Period_Row, 8] = "'" + headerdate.ToString("MMM") + " " + datareader["Period1"].ToString().Substring(0, 4);
                                            }
                                            //***************
                                            if (datareader["Period2"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period2"].ToString().Substring(4, 2) + "/" + datareader["Period2"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 6] = "'" + headerdate.ToString("MMM") + " " + datareader["Period2"].ToString().Substring(0, 4);
                                            }

                                            if (datareader["Period3"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period3"].ToString().Substring(4, 2) + "/" + datareader["Period3"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 7] = "'" + headerdate.ToString("MMM") + " " + datareader["Period3"].ToString().Substring(0, 4);
                                            }
                                            if (datareader["Period4"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period4"].ToString().Substring(4, 2) + "/" + datareader["Period4"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 8] = "'" + headerdate.ToString("MMM") + " " + datareader["Period4"].ToString().Substring(0, 4);
                                            }

                                            if (datareader["Period5"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period5"].ToString().Substring(4, 2) + "/" + datareader["Period5"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 9] = "'" + headerdate.ToString("MMM") + " " + datareader["Period5"].ToString().Substring(0, 4);
                                            }
                                            if (datareader["Period6"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period6"].ToString().Substring(4, 2) + "/" + datareader["Period6"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 10] = "'" + headerdate.ToString("MMM") + " " + datareader["Period6"].ToString().Substring(0, 4);
                                            }

                                            if (datareader["Period7"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period7"].ToString().Substring(4, 2) + "/" + datareader["Period7"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 11] = "'" + headerdate.ToString("MMM") + " " + datareader["Period7"].ToString().Substring(0, 4);
                                            }
                                            if (datareader["Period8"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period8"].ToString().Substring(4, 2) + "/" + datareader["Period8"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 12] = "'" + headerdate.ToString("MMM") + " " + datareader["Period8"].ToString().Substring(0, 4);

                                                temp = "L" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            if (datareader["Period9"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period9"].ToString().Substring(4, 2) + "/" + datareader["Period9"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 13] = "'" + headerdate.ToString("MMM") + " " + datareader["Period9"].ToString().Substring(0, 4);
                                            }
                                            if (datareader["Period10"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period10"].ToString().Substring(4, 2) + "/" + datareader["Period10"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 14] = "'" + headerdate.ToString("MMM") + " " + datareader["Period10"].ToString().Substring(0, 4);
                                            }
                                            if (datareader["Period11"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period11"].ToString().Substring(4, 2) + "/" + datareader["Period11"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 15] = "'" + headerdate.ToString("MMM") + " " + datareader["Period11"].ToString().Substring(0, 4);
                                            }
                                            if (datareader["Period12"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period12"].ToString().Substring(4, 2) + "/" + datareader["Period12"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 16] = "'" + headerdate.ToString("MMM") + " " + datareader["Period12"].ToString().Substring(0, 4);


                                                //DateTime todate = DateTime.Now;
                                                //DateTime.TryParse("20/" + PeriodTo.Text.Substring(4, 2) + "/" + PeriodTo.Text.Substring(0, 4), out todate);

                                                oSheet.Cells[Period_Row, 10] = "'" + headerdate.ToString("MMM") + " " + datareader["Period12"].ToString().Substring(0, 4);                                                
                                            }
                                            
                                            if (datareader["PrjCode"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[Project_Row, 2] = datareader["PrjCode"].ToString();
                                            }
                                            if (datareader["PrjName"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[Project_Row, 3] = datareader["PrjName"].ToString();
                                            }
                                            if (datareader.GetName(i) == "ending_balance")
                                            {
                                                oSheet.Cells[intRow, 17] = "Ending Balance";

                                                temp = "Q" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            //***********************
                                        }
                                        oSheet.get_Range("A" + intRow.ToString(), "Q" + intRow.ToString()).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                                        oSheet.get_Range("D5", "Q5").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                                        border = oSheet.get_Range("A" + intRow.ToString(), "P" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                        border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                        //oSheet.get_Range("A" + intRow.ToString(), "P" + intRow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                                        intRow = intRow + 1;
                                    }
                                    if(datareader["U_ReportCode"] != System.DBNull.Value)
                                        oSheet.Cells[intRow, 1] = "'" + datareader["U_ReportCode"].ToString();
                                    if (datareader["CostCode"] != System.DBNull.Value)
                                        oSheet.Cells[intRow, 2] = "'" + datareader["CostCode"].ToString();
                                    if (datareader["CostCodeDesc"] != System.DBNull.Value)
                                    {
                                        //Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "")
                                        //select CardName from opor where u_pcmsdocnum = '20064B8H/SC002' and CANCELED = 'N'
                                        if( datareader["CostCodeDesc"].ToString().IndexOf("SC") > 0 )
                                        {
                                            String findcardname = "select CardName from opor where u_pcmsdocnum = '" + datareader["PrjCode"].ToString() + "/" + datareader["CostCodeDesc"].ToString().Substring(datareader["CostCodeDesc"].ToString().IndexOf("SC"), 5) + "' and CANCELED = 'N'";
                                            SqlCommand command_j = new SqlCommand(findcardname, connection);
                                            command_j.Connection = connection;
                                            command_j.CommandTimeout = 60000;
                                            using (SqlDataReader card_name_datareader = command_j.ExecuteReader())
                                            {
                                                while (card_name_datareader.Read())
                                                {
                                                    if (card_name_datareader.HasRows == true)
                                                    {
                                                        if (card_name_datareader["CardName"] != System.DBNull.Value)
                                                        {
                                                            oSheet.Cells[intRow, 3] = "'" + datareader["CostCodeDesc"].ToString().Replace(datareader["CostCodeDesc"].ToString().Substring(datareader["CostCodeDesc"].ToString().IndexOf("SC"), 5), card_name_datareader["CardName"].ToString());
                                                        }
                                                        else
                                                        {
                                                            oSheet.Cells[intRow, 3] = "'" + datareader["CostCodeDesc"].ToString();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                            oSheet.Cells[intRow, 3] = "'" + datareader["CostCodeDesc"].ToString();
                                    }

                                    if (datareader["opening_balance"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 4] = datareader["opening_balance"].ToString();
                                        oSheet.get_Range("D" + intRow.ToString(), "D" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        OpeningBalanceTotal = OpeningBalanceTotal + Convert.ToDouble(datareader["opening_balance"].ToString());
                                    }
                                    if (datareader["Month1"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 5] = datareader["Month1"].ToString();
                                        oSheet.get_Range("E" + intRow.ToString(), "E" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period1Total = Period1Total + Convert.ToDouble(datareader["Month1"].ToString());
                                    }
                                    if (datareader["Month2"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 6] = datareader["Month2"].ToString();
                                        oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period2Total = Period2Total + Convert.ToDouble(datareader["Month2"].ToString());
                                    }
                                    if (datareader["Month3"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 7] = datareader["Month3"].ToString();
                                        oSheet.get_Range("G" + intRow.ToString(), "G" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period3Total = Period3Total + Convert.ToDouble(datareader["Month3"].ToString());
                                    }
                                    if (datareader["Month4"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 8] =  datareader["Month4"].ToString();
                                        oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period4Total = Period4Total + Convert.ToDouble(datareader["Month4"].ToString());
                                    }
                                    if (datareader["Month5"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 9] = datareader["Month5"].ToString();
                                        oSheet.get_Range("I" + intRow.ToString(), "I" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period5Total = Period5Total + Convert.ToDouble(datareader["Month5"].ToString());
                                    }
                                    if (datareader["Month6"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 10] = datareader["Month6"].ToString();
                                        oSheet.get_Range("J" + intRow.ToString(), "J" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period6Total = Period6Total + Convert.ToDouble(datareader["Month6"].ToString());
                                    }
                                    if (datareader["Month7"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 11] = datareader["Month7"].ToString();
                                        oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period7Total = Period7Total + Convert.ToDouble(datareader["Month7"].ToString());
                                    }
                                    if (datareader["Month8"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 12] = datareader["Month8"].ToString();
                                        oSheet.get_Range("L" + intRow.ToString(), "L" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period8Total = Period8Total + Convert.ToDouble(datareader["Month8"].ToString());
                                    }
                                    if (datareader["Month9"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 13] = datareader["Month9"].ToString();
                                        oSheet.get_Range("M" + intRow.ToString(), "M" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period9Total = Period9Total + Convert.ToDouble(datareader["Month9"].ToString());
                                    }
                                    if (datareader["Month10"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 14] = datareader["Month10"].ToString();
                                        oSheet.get_Range("N" + intRow.ToString(), "N" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period10Total = Period10Total + Convert.ToDouble(datareader["Month10"].ToString());
                                    }
                                    if (datareader["Month11"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 15] = datareader["Month11"].ToString();
                                        oSheet.get_Range("O" + intRow.ToString(), "O" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period11Total = Period11Total + Convert.ToDouble(datareader["Month11"].ToString());
                                    }
                                    if (datareader["Month12"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 16] = datareader["Month12"].ToString();
                                        oSheet.get_Range("P" + intRow.ToString(), "P" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period12Total = Period12Total + Convert.ToDouble(datareader["Month12"].ToString());
                                    }
                                    if (datareader["ending_balance"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 17] = datareader["ending_balance"].ToString();
                                        oSheet.get_Range("Q" + intRow.ToString(), "Q" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        EndingBalanceTotal = EndingBalanceTotal + Convert.ToDouble(datareader["ending_balance"].ToString());
                                    }
                                    intRow = intRow + 1;
                                }
                            }
                        }
                        //sp_AC03_Detail_Cost_Summary_CostTypeSummary
                        //intRow = intRow + 1;

                        //Total
                        /*
                        oSheet.Cells[intRow, 4] = OpeningBalanceTotal.ToString();
                        oSheet.get_Range("D" + intRow.ToString(), "D" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "D" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "D" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        oSheet.Cells[intRow, 5] = Period1Total.ToString();
                        oSheet.get_Range("E" + intRow.ToString(), "E" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "E" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "E" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        oSheet.Cells[intRow, 6] = Period2Total.ToString();
                        oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "F" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "F" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        oSheet.Cells[intRow, 7] = Period3Total.ToString();
                        oSheet.get_Range("G" + intRow.ToString(), "G" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "G" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "G" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        oSheet.Cells[intRow, 8] = Period4Total.ToString();
                        oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "H" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "H" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        oSheet.Cells[intRow, 9] = Period5Total.ToString();
                        oSheet.get_Range("I" + intRow.ToString(), "I" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "I" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "I" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        oSheet.Cells[intRow, 10] = Period6Total.ToString();
                        oSheet.get_Range("J" + intRow.ToString(), "J" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "J" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "J" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        oSheet.Cells[intRow, 11] = Period7Total.ToString();
                        oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "K" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "K" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        oSheet.Cells[intRow, 12] = Period8Total.ToString();
                        oSheet.get_Range("L" + intRow.ToString(), "L" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "L" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "L" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        oSheet.Cells[intRow, 13] = Period9Total.ToString();
                        oSheet.get_Range("M" + intRow.ToString(), "M" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "M" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "M" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        oSheet.Cells[intRow, 14] = Period10Total.ToString();
                        oSheet.get_Range("N" + intRow.ToString(), "N" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "N" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "N" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        oSheet.Cells[intRow, 15] = Period11Total.ToString();
                        oSheet.get_Range("O" + intRow.ToString(), "O" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "O" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "O" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        oSheet.Cells[intRow, 16] = Period12Total.ToString();
                        oSheet.get_Range("P" + intRow.ToString(), "P" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "P" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "P" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        oSheet.Cells[intRow, 17] = EndingBalanceTotal.ToString();
                        oSheet.get_Range("Q" + intRow.ToString(), "Q" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "Q" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "Q" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;
                        */

                        oSheet.get_Range("D" + intRow.ToString(), "Q" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                        border = oSheet.get_Range("D" + intRow.ToString(), "Q" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;
                        border = oSheet.get_Range("D" + intRow.ToString(), "Q" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        oSheet.get_Range("D" + intRow.ToString(), "D" + intRow.ToString()).Formula = "=SUBTOTAL(9,D" + OpeningBalanceTotalRow.ToString() + ":D" + (intRow - 1).ToString() + ")";
                        oSheet.get_Range("E" + intRow.ToString(), "E" + intRow.ToString()).Formula = "=SUBTOTAL(9,E" + OpeningBalanceTotalRow.ToString() + ":E" + (intRow - 1).ToString() + ")";
                        oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString()).Formula = "=SUBTOTAL(9,F" + OpeningBalanceTotalRow.ToString() + ":F" + (intRow - 1).ToString() + ")";
                        oSheet.get_Range("G" + intRow.ToString(), "G" + intRow.ToString()).Formula = "=SUBTOTAL(9,G" + OpeningBalanceTotalRow.ToString() + ":G" + (intRow - 1).ToString() + ")";
                        oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).Formula = "=SUBTOTAL(9,H" + OpeningBalanceTotalRow.ToString() + ":H" + (intRow - 1).ToString() + ")";
                        oSheet.get_Range("I" + intRow.ToString(), "I" + intRow.ToString()).Formula = "=SUBTOTAL(9,I" + OpeningBalanceTotalRow.ToString() + ":I" + (intRow - 1).ToString() + ")";
                        oSheet.get_Range("J" + intRow.ToString(), "J" + intRow.ToString()).Formula = "=SUBTOTAL(9,J" + OpeningBalanceTotalRow.ToString() + ":J" + (intRow - 1).ToString() + ")";
                        oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).Formula = "=SUBTOTAL(9,K" + OpeningBalanceTotalRow.ToString() + ":K" + (intRow - 1).ToString() + ")";
                        oSheet.get_Range("L" + intRow.ToString(), "L" + intRow.ToString()).Formula = "=SUBTOTAL(9,L" + OpeningBalanceTotalRow.ToString() + ":L" + (intRow - 1).ToString() + ")";
                        oSheet.get_Range("M" + intRow.ToString(), "M" + intRow.ToString()).Formula = "=SUBTOTAL(9,M" + OpeningBalanceTotalRow.ToString() + ":M" + (intRow - 1).ToString() + ")";
                        oSheet.get_Range("N" + intRow.ToString(), "N" + intRow.ToString()).Formula = "=SUBTOTAL(9,N" + OpeningBalanceTotalRow.ToString() + ":N" + (intRow - 1).ToString() + ")";
                        oSheet.get_Range("O" + intRow.ToString(), "O" + intRow.ToString()).Formula = "=SUBTOTAL(9,O" + OpeningBalanceTotalRow.ToString() + ":O" + (intRow - 1).ToString() + ")";
                        oSheet.get_Range("P" + intRow.ToString(), "P" + intRow.ToString()).Formula = "=SUBTOTAL(9,P" + OpeningBalanceTotalRow.ToString() + ":P" + (intRow - 1).ToString() + ")";
                        oSheet.get_Range("Q" + intRow.ToString(), "Q" + intRow.ToString()).Formula = "=SUBTOTAL(9,Q" + OpeningBalanceTotalRow.ToString() + ":Q" + (intRow - 1).ToString() + ")";

                        oSheet.get_Range("A5", "Q" + (intRow - 1).ToString()).AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                        intRow = intRow + 2;

                        OpeningBalanceTotal = 0;
                        Period1Total = 0;
                        Period2Total = 0;
                        Period3Total = 0;
                        Period4Total = 0;
                        Period5Total = 0;
                        Period6Total = 0;
                        Period7Total = 0;
                        Period8Total = 0;
                        Period9Total = 0;
                        Period10Total = 0;
                        Period11Total = 0;
                        Period12Total = 0;
                        EndingBalanceTotal = 0;

                        StartRow = true;
                        SqlCommand command_a = new SqlCommand("sp_AC03_Detail_Cost_Summary_CostTypeSummary", connection);
                        command_a.CommandType = CommandType.StoredProcedure;

                        command_a.Parameters.Add(new SqlParameter("@FDocEntry", Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "")));
                        command_a.Parameters.Add(new SqlParameter("@CostCodeFr", CostCodeFr.Text));
                        command_a.Parameters.Add(new SqlParameter("@CostCodeTo", CostCodeTo.Text));
                        command_a.Parameters.Add(new SqlParameter("@PrdChoice", Convert.ToInt32(PrdChoice.Text)));
                        command_a.Parameters.Add(new SqlParameter("@PeriodFr", PeriodFr.Text));

                        command_a.Connection = connection;
                        command_a.CommandTimeout = 60000;


                        using (SqlDataReader datareader = command_a.ExecuteReader())
                        {
                            while (datareader.Read())
                            {
                                if (datareader.HasRows == true)
                                {
                                    if (StartRow == true)
                                    {
                                        StartRow = false;
                                        for (int i = 0; i < datareader.FieldCount; i++)
                                        {   /*
                                            if (datareader.GetName(i) == "U_ReportCode")
                                            {
                                                oSheet.Cells[intRow, 1] = "Report Code";

                                                temp = "A" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                            }*/
                                            if (datareader.GetName(i) == "CostType")
                                            {
                                                oSheet.Cells[intRow, 2] = "Cost Type";

                                                temp = "B" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                oSheet.get_Range("B" + intRow.ToString(), "B" + intRow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                            }
                                            if (datareader.GetName(i) == "CostTypeDesc")
                                            {
                                                oSheet.Cells[intRow, 3] = "Cost Type Description";

                                                temp = "C" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                oSheet.get_Range("C" + intRow.ToString(), "C" + intRow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                            }

                                            if (datareader.GetName(i) == "opening_balance")
                                            {
                                                oSheet.Cells[intRow, 4] = "Opening Balance";

                                                temp = "D" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                                                
                                            }

                                            if (datareader["Period1"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period1"].ToString().Substring(4, 2) + "/" + datareader["Period1"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 5] = "'" + headerdate.ToString("MMM") + " " + datareader["Period1"].ToString().Substring(0, 4);

                                                //DateTime fromdate = DateTime.Now;
                                                //DateTime.TryParse("20/" + PeriodFr.Text.Substring(4, 2) + "/" + PeriodFr.Text.Substring(0, 4), out fromdate);

                                                //oSheet.Cells[Period_Row, 6] = "'" + headerdate.ToString("MMM") + "/" + datareader["Period1"].ToString().Substring(0, 4);

                                                temp = "E" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            //***************
                                            if (datareader["Period2"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period2"].ToString().Substring(4, 2) + "/" + datareader["Period2"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 6] = "'" + headerdate.ToString("MMM") + " " + datareader["Period2"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 6] = "Period2";

                                                temp = "F" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }

                                            if (datareader["Period3"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period3"].ToString().Substring(4, 2) + "/" + datareader["Period3"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 7] = "'" + headerdate.ToString("MMM") + " " + datareader["Period3"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 7] = "Period3";

                                                temp = "G" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            if (datareader["Period4"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period4"].ToString().Substring(4, 2) + "/" + datareader["Period4"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 8] = "'" + headerdate.ToString("MMM") + " " + datareader["Period4"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 8] = "Period4";

                                                temp = "H" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }

                                            if (datareader["Period5"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period5"].ToString().Substring(4, 2) + "/" + datareader["Period5"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 9] = "'" + headerdate.ToString("MMM") + " " + datareader["Period5"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 9] = "Period5";

                                                temp = "I" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            if (datareader["Period6"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period6"].ToString().Substring(4, 2) + "/" + datareader["Period6"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 10] = "'" + headerdate.ToString("MMM") + " " + datareader["Period6"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 10] = "Period6";

                                                temp = "J" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }

                                            if (datareader["Period7"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period7"].ToString().Substring(4, 2) + "/" + datareader["Period7"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 11] = "'" + headerdate.ToString("MMM") + " " + datareader["Period7"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 11] = "Period7";

                                                temp = "K" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            if (datareader["Period8"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period8"].ToString().Substring(4, 2) + "/" + datareader["Period8"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 12] = "'" + headerdate.ToString("MMM") + " " + datareader["Period8"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 12] = "Period8";

                                                temp = "L" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            if (datareader["Period9"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period9"].ToString().Substring(4, 2) + "/" + datareader["Period9"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 13] = "'" + headerdate.ToString("MMM") + " " + datareader["Period9"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 13] = "Period9";

                                                temp = "M" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            if (datareader["Period10"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period10"].ToString().Substring(4, 2) + "/" + datareader["Period10"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 14] = "'" + headerdate.ToString("MMM") + " " + datareader["Period10"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 14] = "Period10";

                                                temp = "N" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            if (datareader["Period11"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period11"].ToString().Substring(4, 2) + "/" + datareader["Period11"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 15] = "'" + headerdate.ToString("MMM") + " " + datareader["Period11"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 15] = "Period11";

                                                temp = "O" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            if (datareader["Period12"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period12"].ToString().Substring(4, 2) + "/" + datareader["Period12"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 16] = "'" + headerdate.ToString("MMM") + " " + datareader["Period12"].ToString().Substring(0, 4);


                                                //DateTime todate = DateTime.Now;
                                                //DateTime.TryParse("20/" + PeriodTo.Text.Substring(4, 2) + "/" + PeriodTo.Text.Substring(0, 4), out todate);

                                                //oSheet.Cells[Period_Row, 8] = "'" + headerdate.ToString("MMM") + "/" + datareader["Period12"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 16] = "Period12";

                                                temp = "P" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            if (datareader["PrjCode"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[Project_Row, 2] = datareader["PrjCode"].ToString();
                                            }
                                            if (datareader.GetName(i) == "ending_balance")
                                            {
                                                oSheet.Cells[intRow, 17] = "Ending Balance";

                                                temp = "Q" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            //***********************
                                        }
                                        oSheet.get_Range("D" + intRow.ToString(), "Q" + intRow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                        intRow = intRow + 1;
                                    }
                                    //if (datareader["U_ReportCode"] != System.DBNull.Value)
                                        //oSheet.Cells[intRow, 1] = "'" + datareader["U_ReportCode"].ToString();
                                    if (datareader["CostType"] != System.DBNull.Value)
                                        oSheet.Cells[intRow, 2] = "'" + datareader["CostType"].ToString();
                                    if (datareader["CostTypeDesc"] != System.DBNull.Value)
                                        oSheet.Cells[intRow, 3] = "'" + datareader["CostTypeDesc"].ToString();

                                    if (datareader["opening_balance"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 4] = datareader["opening_balance"].ToString();
                                        oSheet.get_Range("D" + intRow.ToString(), "D" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        OpeningBalanceTotal = OpeningBalanceTotal + Convert.ToDouble(datareader["opening_balance"].ToString());
                                    }
                                    if (datareader["Month1"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 5] = datareader["Month1"].ToString();
                                        oSheet.get_Range("E" + intRow.ToString(), "E" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period1Total = Period1Total + Convert.ToDouble(datareader["Month1"].ToString());
                                    }
                                    if (datareader["Month2"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 6] = datareader["Month2"].ToString();
                                        oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period2Total = Period2Total + Convert.ToDouble(datareader["Month2"].ToString());
                                    }
                                    if (datareader["Month3"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 7] = datareader["Month3"].ToString();
                                        oSheet.get_Range("G" + intRow.ToString(), "G" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period3Total = Period3Total + Convert.ToDouble(datareader["Month3"].ToString());
                                    }
                                    if (datareader["Month4"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 8] = datareader["Month4"].ToString();
                                        oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period4Total = Period4Total + Convert.ToDouble(datareader["Month4"].ToString());
                                    }
                                    if (datareader["Month5"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 9] = datareader["Month5"].ToString();
                                        oSheet.get_Range("I" + intRow.ToString(), "I" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period5Total = Period5Total + Convert.ToDouble(datareader["Month5"].ToString());
                                    }
                                    if (datareader["Month6"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 10] = datareader["Month6"].ToString();
                                        oSheet.get_Range("J" + intRow.ToString(), "J" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period6Total = Period6Total + Convert.ToDouble(datareader["Month6"].ToString());
                                    }
                                    if (datareader["Month7"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 11] = datareader["Month7"].ToString();
                                        oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period7Total = Period7Total + Convert.ToDouble(datareader["Month7"].ToString());
                                    }
                                    if (datareader["Month8"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 12] = datareader["Month8"].ToString();
                                        oSheet.get_Range("L" + intRow.ToString(), "L" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period8Total = Period8Total + Convert.ToDouble(datareader["Month8"].ToString());
                                    }
                                    if (datareader["Month9"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 13] = datareader["Month9"].ToString();
                                        oSheet.get_Range("M" + intRow.ToString(), "M" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period9Total = Period9Total + Convert.ToDouble(datareader["Month9"].ToString());
                                    }
                                    if (datareader["Month10"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 14] = datareader["Month10"].ToString();
                                        oSheet.get_Range("N" + intRow.ToString(), "N" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period10Total = Period10Total + Convert.ToDouble(datareader["Month10"].ToString());
                                    }
                                    if (datareader["Month11"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 15] = datareader["Month11"].ToString();
                                        oSheet.get_Range("O" + intRow.ToString(), "O" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period11Total = Period11Total + Convert.ToDouble(datareader["Month11"].ToString());
                                    }
                                    if (datareader["Month12"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 16] = datareader["Month12"].ToString();
                                        oSheet.get_Range("P" + intRow.ToString(), "P" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period12Total = Period12Total + Convert.ToDouble(datareader["Month12"].ToString());
                                    }
                                    if (datareader["ending_balance"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 17] = datareader["ending_balance"].ToString();
                                        oSheet.get_Range("Q" + intRow.ToString(), "Q" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        EndingBalanceTotal = EndingBalanceTotal + Convert.ToDouble(datareader["ending_balance"].ToString());
                                    }
                                    intRow = intRow + 1;
                                }
                            }
                        }
                        //Total
                        oSheet.Cells[intRow, 4] = OpeningBalanceTotal.ToString();
                        oSheet.Cells[intRow, 5] = Period1Total.ToString();
                        oSheet.Cells[intRow, 6] = Period2Total.ToString();
                        oSheet.Cells[intRow, 7] = Period3Total.ToString();
                        oSheet.Cells[intRow, 8] = Period4Total.ToString();
                        oSheet.Cells[intRow, 9] = Period5Total.ToString();
                        oSheet.Cells[intRow, 10] = Period6Total.ToString();
                        oSheet.Cells[intRow, 11] = Period7Total.ToString();
                        oSheet.Cells[intRow, 12] = Period8Total.ToString();
                        oSheet.Cells[intRow, 13] = Period9Total.ToString();
                        oSheet.Cells[intRow, 14] = Period10Total.ToString();
                        oSheet.Cells[intRow, 15] = Period11Total.ToString();
                        oSheet.Cells[intRow, 16] = Period12Total.ToString();
                        oSheet.Cells[intRow, 17] = EndingBalanceTotal.ToString();

                        oSheet.get_Range("D" + intRow.ToString(), "Q" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                        border = oSheet.get_Range("D" + intRow.ToString(), "Q" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlDouble;
                        border = oSheet.get_Range("D" + intRow.ToString(), "Q" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;
                        /*
                        oSheet.get_Range("D" + intRow.ToString(), "D" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "D" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "D" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        oSheet.get_Range("E" + intRow.ToString(), "E" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "E" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "E" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        
                        oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "F" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "F" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        
                        oSheet.get_Range("G" + intRow.ToString(), "G" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "G" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "G" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        
                        oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "H" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "H" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        
                        oSheet.get_Range("I" + intRow.ToString(), "I" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "I" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "I" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        
                        oSheet.get_Range("J" + intRow.ToString(), "J" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "J" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "J" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        
                        oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "K" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "K" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        
                        oSheet.get_Range("L" + intRow.ToString(), "L" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "L" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "L" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        
                        oSheet.get_Range("M" + intRow.ToString(), "M" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "M" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "M" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        oSheet.get_Range("N" + intRow.ToString(), "N" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "N" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "N" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        
                        oSheet.get_Range("O" + intRow.ToString(), "O" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "O" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "O" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        
                        oSheet.get_Range("P" + intRow.ToString(), "P" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "P" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "P" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        
                        oSheet.get_Range("Q" + intRow.ToString(), "Q" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "Q" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "Q" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;
                        */
                        
                        intRow = intRow + 2;

                        OpeningBalanceTotal = 0;
                        Period1Total = 0;
                        Period2Total = 0;
                        Period3Total = 0;
                        Period4Total = 0;
                        Period5Total = 0;
                        Period6Total = 0;
                        Period7Total = 0;
                        Period8Total = 0;
                        Period9Total = 0;
                        Period10Total = 0;
                        Period11Total = 0;
                        Period12Total = 0;
                        EndingBalanceTotal = 0;

                        StartRow = true;
                        SqlCommand command_b = new SqlCommand("sp_AC03_Detail_Cost_Summary_ReportCodeSummary", connection);
                        command_b.CommandType = CommandType.StoredProcedure;

                        command_b.Parameters.Add(new SqlParameter("@FDocEntry", Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "")));
                        command_b.Parameters.Add(new SqlParameter("@CostCodeFr", CostCodeFr.Text));
                        command_b.Parameters.Add(new SqlParameter("@CostCodeTo", CostCodeTo.Text));
                        command_b.Parameters.Add(new SqlParameter("@PrdChoice", Convert.ToInt32(PrdChoice.Text)));
                        command_b.Parameters.Add(new SqlParameter("@PeriodFr", PeriodFr.Text));

                        command_b.Connection = connection;
                        command_b.CommandTimeout = 60000;

                        using (SqlDataReader datareader = command_b.ExecuteReader())
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
                                            if (datareader.GetName(i) == "ReportCode")
                                            {
                                                oSheet.Cells[intRow, 2] = "Report Code";

                                                temp = "B" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                oSheet.get_Range("B" + intRow.ToString(), "B" + intRow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                            }
                                            /*
                                            if (datareader.GetName(i) == "CostCode")
                                            {
                                                oSheet.Cells[intRow, 2] = "Cost Code";

                                                temp = "B" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                            }*/
                                            if (datareader.GetName(i) == "ReportCodeName")
                                            {
                                                oSheet.Cells[intRow, 3] = "Report Code Name";

                                                temp = "C" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                oSheet.get_Range("C" + intRow.ToString(), "C" + intRow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                            }

                                            if (datareader.GetName(i) == "opening_balance")
                                            {
                                                oSheet.Cells[intRow, 4] = "Opening Balance";

                                                temp = "D" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }

                                            if (datareader["Period1"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period1"].ToString().Substring(4, 2) + "/" + datareader["Period1"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 5] = "'" + headerdate.ToString("MMM") + " " + datareader["Period1"].ToString().Substring(0, 4);

                                                //DateTime fromdate = DateTime.Now;
                                                //DateTime.TryParse("20/" + PeriodFr.Text.Substring(4, 2) + "/" + PeriodFr.Text.Substring(0, 4), out fromdate);

                                                //oSheet.Cells[Period_Row, 6] = "'" + headerdate.ToString("MMM") + "/" + datareader["Period1"].ToString().Substring(0, 4);

                                                temp = "E" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            //***************
                                            if (datareader["Period2"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period2"].ToString().Substring(4, 2) + "/" + datareader["Period2"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 6] = "'" + headerdate.ToString("MMM") + " " + datareader["Period2"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 6] = "Period2";

                                                temp = "F" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }

                                            if (datareader["Period3"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period3"].ToString().Substring(4, 2) + "/" + datareader["Period3"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 7] = "'" + headerdate.ToString("MMM") + " " + datareader["Period3"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 7] = "Period3";

                                                temp = "G" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            if (datareader["Period4"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period4"].ToString().Substring(4, 2) + "/" + datareader["Period4"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 8] = "'" + headerdate.ToString("MMM") + " " + datareader["Period4"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 8] = "Period4";

                                                temp = "H" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }

                                            if (datareader["Period5"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period5"].ToString().Substring(4, 2) + "/" + datareader["Period5"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 9] = "'" + headerdate.ToString("MMM") + " " + datareader["Period5"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 9] = "Period5";

                                                temp = "I" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            if (datareader["Period6"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period6"].ToString().Substring(4, 2) + "/" + datareader["Period6"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 10] = "'" + headerdate.ToString("MMM") + " " + datareader["Period6"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 10] = "Period6";

                                                temp = "J" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }

                                            if (datareader["Period7"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period7"].ToString().Substring(4, 2) + "/" + datareader["Period7"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 11] = "'" + headerdate.ToString("MMM") + " " + datareader["Period7"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 11] = "Period7";

                                                temp = "K" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            if (datareader["Period8"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period8"].ToString().Substring(4, 2) + "/" + datareader["Period8"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 12] = "'" + headerdate.ToString("MMM") + " " + datareader["Period8"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 12] = "Period8";

                                                temp = "L" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            if (datareader["Period9"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period9"].ToString().Substring(4, 2) + "/" + datareader["Period9"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 13] = "'" + headerdate.ToString("MMM") + " " + datareader["Period9"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 13] = "Period9";

                                                temp = "M" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            if (datareader["Period10"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period10"].ToString().Substring(4, 2) + "/" + datareader["Period10"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 14] = "'" + headerdate.ToString("MMM") + " " + datareader["Period10"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 14] = "Period10";

                                                temp = "N" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            if (datareader["Period11"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period11"].ToString().Substring(4, 2) + "/" + datareader["Period11"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 15] = "'" + headerdate.ToString("MMM") + " " + datareader["Period11"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 15] = "Period11";

                                                temp = "O" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            if (datareader["Period12"] != System.DBNull.Value)
                                            {
                                                DateTime headerdate = DateTime.Now;
                                                DateTime.TryParse("20/" + datareader["Period12"].ToString().Substring(4, 2) + "/" + datareader["Period12"].ToString().Substring(0, 4), out headerdate);
                                                oSheet.Cells[intRow, 16] = "'" + headerdate.ToString("MMM") + " " + datareader["Period12"].ToString().Substring(0, 4);


                                                //DateTime todate = DateTime.Now;
                                                //DateTime.TryParse("20/" + PeriodTo.Text.Substring(4, 2) + "/" + PeriodTo.Text.Substring(0, 4), out todate);

                                                //oSheet.Cells[Period_Row, 8] = "'" + headerdate.ToString("MMM") + "/" + datareader["Period12"].ToString().Substring(0, 4);

                                                //oSheet.Cells[intRow, 16] = "Period12";

                                                temp = "P" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            if (datareader["PrjCode"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[Project_Row, 2] = datareader["PrjCode"].ToString();
                                            }
                                            if (datareader.GetName(i) == "ending_balance")
                                            {
                                                oSheet.Cells[intRow, 17] = "Ending Balance";

                                                temp = "Q" + intRow.ToString();
                                                border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                                            }
                                            //***********************
                                        }
                                        oSheet.get_Range("D" + intRow.ToString(), "Q" + intRow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                        intRow = intRow + 1;
                                    }
                                    //if (datareader["U_ReportCode"] != System.DBNull.Value)
                                        //oSheet.Cells[intRow, 1] = "'" + datareader["U_ReportCode"].ToString();
                                    if (datareader["ReportCode"] != System.DBNull.Value)
                                        oSheet.Cells[intRow, 2] = "'" + datareader["ReportCode"].ToString();
                                    if (datareader["ReportCodeName"] != System.DBNull.Value)
                                        oSheet.Cells[intRow, 3] = "'" + datareader["ReportCodeName"].ToString();

                                    if (datareader["opening_balance"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 4] = datareader["opening_balance"].ToString();
                                        oSheet.get_Range("D" + intRow.ToString(), "D" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        OpeningBalanceTotal = OpeningBalanceTotal + Convert.ToDouble(datareader["opening_balance"].ToString());
                                    }
                                    if (datareader["Month1"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 5] = datareader["Month1"].ToString();
                                        oSheet.get_Range("E" + intRow.ToString(), "E" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period1Total = Period1Total + Convert.ToDouble(datareader["Month1"].ToString());
                                    }
                                    if (datareader["Month2"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 6] = datareader["Month2"].ToString();
                                        oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period2Total = Period2Total + Convert.ToDouble(datareader["Month2"].ToString());
                                    }
                                    if (datareader["Month3"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 7] = datareader["Month3"].ToString();
                                        oSheet.get_Range("G" + intRow.ToString(), "G" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period3Total = Period3Total + Convert.ToDouble(datareader["Month3"].ToString());
                                    }
                                    if (datareader["Month4"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 8] = datareader["Month4"].ToString();
                                        oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period4Total = Period4Total + Convert.ToDouble(datareader["Month4"].ToString());
                                    }
                                    if (datareader["Month5"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 9] = datareader["Month5"].ToString();
                                        oSheet.get_Range("I" + intRow.ToString(), "I" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period5Total = Period5Total + Convert.ToDouble(datareader["Month5"].ToString());
                                    }
                                    if (datareader["Month6"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 10] = datareader["Month6"].ToString();
                                        oSheet.get_Range("J" + intRow.ToString(), "J" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period6Total = Period6Total + Convert.ToDouble(datareader["Month6"].ToString());
                                    }
                                    if (datareader["Month7"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 11] = datareader["Month7"].ToString();
                                        oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period7Total = Period7Total + Convert.ToDouble(datareader["Month7"].ToString());
                                    }
                                    if (datareader["Month8"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 12] = datareader["Month8"].ToString();
                                        oSheet.get_Range("L" + intRow.ToString(), "L" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period8Total = Period8Total + Convert.ToDouble(datareader["Month8"].ToString());
                                    }
                                    if (datareader["Month9"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 13] = datareader["Month9"].ToString();
                                        oSheet.get_Range("M" + intRow.ToString(), "M" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period9Total = Period9Total + Convert.ToDouble(datareader["Month9"].ToString());
                                    }
                                    if (datareader["Month10"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 14] = datareader["Month10"].ToString();
                                        oSheet.get_Range("N" + intRow.ToString(), "N" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period10Total = Period10Total + Convert.ToDouble(datareader["Month10"].ToString());
                                    }
                                    if (datareader["Month11"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 15] = datareader["Month11"].ToString();
                                        oSheet.get_Range("O" + intRow.ToString(), "O" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period11Total = Period11Total + Convert.ToDouble(datareader["Month11"].ToString());
                                    }
                                    if (datareader["Month12"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 16] = datareader["Month12"].ToString();
                                        oSheet.get_Range("P" + intRow.ToString(), "P" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        Period12Total = Period12Total + Convert.ToDouble(datareader["Month12"].ToString());
                                    }
                                    if (datareader["ending_balance"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[intRow, 17] = datareader["ending_balance"].ToString();
                                        oSheet.get_Range("Q" + intRow.ToString(), "Q" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                        EndingBalanceTotal = EndingBalanceTotal + Convert.ToDouble(datareader["ending_balance"].ToString());
                                    }
                                    intRow = intRow + 1;
                                }
                            }
                        }
                        //Total
                        oSheet.Cells[intRow, 4] = OpeningBalanceTotal.ToString();
                        oSheet.Cells[intRow, 5] = Period1Total.ToString();
                        oSheet.Cells[intRow, 6] = Period2Total.ToString();
                        oSheet.Cells[intRow, 7] = Period3Total.ToString();
                        oSheet.Cells[intRow, 8] = Period4Total.ToString();
                        oSheet.Cells[intRow, 9] = Period5Total.ToString();
                        oSheet.Cells[intRow, 10] = Period6Total.ToString();
                        oSheet.Cells[intRow, 11] = Period7Total.ToString();
                        oSheet.Cells[intRow, 12] = Period8Total.ToString();
                        oSheet.Cells[intRow, 13] = Period9Total.ToString();
                        oSheet.Cells[intRow, 14] = Period10Total.ToString();
                        oSheet.Cells[intRow, 15] = Period11Total.ToString();
                        oSheet.Cells[intRow, 16] = Period12Total.ToString();
                        oSheet.Cells[intRow, 17] = EndingBalanceTotal.ToString();

                        oSheet.get_Range("D" + intRow.ToString(), "Q" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                        border = oSheet.get_Range("D" + intRow.ToString(), "Q" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlDouble;
                        border = oSheet.get_Range("D" + intRow.ToString(), "Q" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        /*
                        oSheet.get_Range("D" + intRow.ToString(), "D" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "D" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "D" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;


                        oSheet.get_Range("E" + intRow.ToString(), "E" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "E" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "E" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;


                        oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "F" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "F" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;


                        oSheet.get_Range("G" + intRow.ToString(), "G" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "G" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "G" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;


                        oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "H" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "H" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        
                        oSheet.get_Range("I" + intRow.ToString(), "I" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "I" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "I" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        
                        oSheet.get_Range("J" + intRow.ToString(), "J" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "J" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "J" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "K" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "K" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        
                        oSheet.get_Range("L" + intRow.ToString(), "L" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "L" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "L" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        
                        oSheet.get_Range("M" + intRow.ToString(), "M" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "M" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "M" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        
                        oSheet.get_Range("N" + intRow.ToString(), "N" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "N" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "N" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        
                        oSheet.get_Range("O" + intRow.ToString(), "O" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "O" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "O" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        
                        oSheet.get_Range("P" + intRow.ToString(), "P" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "P" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "P" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        
                        oSheet.get_Range("Q" + intRow.ToString(), "Q" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        temp = "Q" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;

                        temp = "Q" + intRow.ToString();
                        border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                        border.LineStyle = Excel.XlLineStyle.xlContinuous;
                        */
                        command.Dispose();
                        command_a.Dispose();
                        command_b.Dispose();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        connection.Close();
                    }
                    oRng = oSheet.get_Range("A1", "C65536");
                    oRng.EntireColumn.AutoFit();

                    //oSheet.Columns.HorizontalAlignment = 1;
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
                    //oSheet.Columns.Font.ColorIndex = -4105;
                    /*
                    if (oSheet.Cells[3, 1] == "Project Name")
                    {
                        oRng = oSheet.get_Range("A1", "A65536");
                        oRng.ColumnWidth = 114;
                    }
                    */
                    intRow = intRow + 3;
                    oSheet.Cells[intRow, 1] = "End of Report";
                    intRow = intRow + 1;
                    oSheet.Cells[intRow, 1] = "User Name : " + Request.QueryString["UserName"].ToString() + " Print Date : " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");

                    oRng = oSheet.get_Range("A1", "A65536");

                    ((Excel.Range)oSheet.Columns["A", Missing]).ColumnWidth = 7.6;
                    ((Excel.Range)oSheet.Columns["B", Missing]).ColumnWidth = 11.3;
                    ((Excel.Range)oSheet.Columns["C", Missing]).ColumnWidth = 33.6;

                    ((Excel.Range)oSheet.Columns["E", Missing]).ColumnWidth = 15;
                    ((Excel.Range)oSheet.Columns["F", Missing]).ColumnWidth = 15;
                    ((Excel.Range)oSheet.Columns["G", Missing]).ColumnWidth = 15;
                    ((Excel.Range)oSheet.Columns["H", Missing]).ColumnWidth = 15;
                    ((Excel.Range)oSheet.Columns["I", Missing]).ColumnWidth = 15;
                    ((Excel.Range)oSheet.Columns["J", Missing]).ColumnWidth = 15;
                    ((Excel.Range)oSheet.Columns["K", Missing]).ColumnWidth = 15;
                    ((Excel.Range)oSheet.Columns["L", Missing]).ColumnWidth = 15;
                    ((Excel.Range)oSheet.Columns["M", Missing]).ColumnWidth = 15;
                    ((Excel.Range)oSheet.Columns["N", Missing]).ColumnWidth = 15;
                    ((Excel.Range)oSheet.Columns["O", Missing]).ColumnWidth = 15;
                    ((Excel.Range)oSheet.Columns["P", Missing]).ColumnWidth = 15;
                    ((Excel.Range)oSheet.Columns["Q", Missing]).ColumnWidth = 15;

                    
                    //oXL.Visible = true;
                }
                oXL.Visible = true;

                //File.AppendAllText((@"E:\MartinExcelDebug.txt", "before save Excel");
                string fname = Server.MapPath("..\\ReportsFile\\AC08ExportFile" + number.ToString() + ".xls");
                if (System.IO.File.Exists(fname))
                {
                    System.IO.File.Delete(fname);
                    oWB.SaveCopyAs(fname);
                }
                else
                    oWB.SaveCopyAs(fname);

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
            SimpleWebUtils.DownloadFile(Page.Response, path + "/ReportsFile/AC08ExportFile" + number.ToString() + ".xls");
            //File.AppendAllText((@"E:\MartinExcelDebug.txt", "after download");
        }
    }
}
