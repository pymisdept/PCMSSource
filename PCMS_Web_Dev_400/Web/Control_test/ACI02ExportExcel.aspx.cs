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

using System.ComponentModel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

using SimpleControls;
using SimpleControls.Web;
using System.IO;

public partial class Control_ACI02ExportExcel : System.Web.UI.Page
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
            if (!String.IsNullOrEmpty(Request.QueryString["ProjectCode"]) && !String.IsNullOrEmpty(Request.QueryString["UserID"]))
            {
                Random ranObj = new Random();
                int start = 1;
                int end = 100;
                int number = ranObj.Next(start, end);
                double total_payment_cur = 0;
                double total_payment_tot = 0;

                int intRow = 1;

                String Grand_Total_Subcontract_Value_F1 = "";
                String Grand_Total_WorkCertified_Current = "";
                String Grand_Total_WorkCertified_Total = "";
                String Grand_Total_RententionMoney_Current = "";
                String Grand_Total_RententionMoney_Total = "";
                String Grand_Total_Deductions_Current = "";
                String Grand_Total_Deductions_Total = "";
                String Grand_Total_Payment_Current = "";
                String Grand_Total_Payment_Total = "";

                String Temp_Store_Last_WorkCertified_Current = "";
                String Temp_Store_Last_WorkCertified_Total = "";
                String Temp_Store_Last_RententionMoney_Current = "";
                String Temp_Store_Last_RententionMoney_Total = "";
                String Temp_Store_Last_Deductions_Current = "";
                String Temp_Store_Last_Deductions_Total = "";
                String Temp_Store_Last_Payment_Current = "";
                String Temp_Store_Last_Payment_Total = "";


                object Missing = System.Type.Missing;
                Excel._Workbook oWB = null;
                Excel._Worksheet oSheet = null;
                Excel.Range oRng = null;
                Excel.Application oXL = new Excel.Application();

                int SubContractor_Row = 2;
                int Project_Row = 3;
                int Nature_of_Works_Row = 4;
                try
                {
                    oXL.Visible = false;
                    //
                    //oXL.Visible = true;
                    //
                    oXL.DisplayAlerts = false;
                    Boolean StartRow = true;
                    //Get a new workbook.
                    oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing));
                    oSheet = (Excel._Worksheet)oWB.ActiveSheet;
                    double Grant_Total_Of_InvAmt = 0;
                    double Grant_Total_Of_AmtDeduct = 0;
                    double Grant_Total_Of_DIFF = 0;
                    String TempStoreStr = "";
                    String previous_SubConName = "";
                    String previous_ProjectName = "";
                    String previous_NatureOfWorks = "";
                    String previous_PCDocNum = "";
                    String odd_PCMSDocNum = "";

                    //File.AppendAllText((@"E:\MartinExcelDebug.txt", "SAP2");
                    string _connectionString = ConfigurationManager.ConnectionStrings["SAP2"].ConnectionString.ToString();
                    //File.AppendAllText((@"E:\MartinExcelDebug.txt", "after SAP2");
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        try
                        {
                            TempStoreStr = "CREATE TABLE #TR_Rpt (SubsiCode				nvarchar(max),";
	                        TempStoreStr = TempStoreStr + "SubSiName			nvarchar(100),";
	                        TempStoreStr = TempStoreStr + "SubCon					nvarchar(15),";
	                        TempStoreStr = TempStoreStr + "SubConName		nvarchar(100),";
	                        TempStoreStr = TempStoreStr + "PrjCode					nvarchar(8),";
	                        TempStoreStr = TempStoreStr + "PrjName					nvarchar(150),";
	                        TempStoreStr = TempStoreStr + "NatureOfWork		nvarchar(max),";
	                        TempStoreStr = TempStoreStr + "PCMSDocNum		nvarchar(30),";
	                        TempStoreStr = TempStoreStr + "Ref							nvarchar(20),";
	                        TempStoreStr = TempStoreStr + "Cert_no					nvarchar(20),";
	                        TempStoreStr = TempStoreStr + "Cert_Date				datetime,";
	                        TempStoreStr = TempStoreStr + "WorksCertified_Cur	numeric(19,6),";
	                        TempStoreStr = TempStoreStr + "WorksCertified_Tot		numeric(19,6),";
	                        TempStoreStr = TempStoreStr + "RetentionMoney_Cur	numeric(19,6),";
	                        TempStoreStr = TempStoreStr + "RetentionMoney_Tot	numeric(19,6),";
	                        TempStoreStr = TempStoreStr + "Deductions_Cur	numeric(19,6),";
	                        TempStoreStr = TempStoreStr + "Deductions_Tot	numeric(19,6),";
	                        TempStoreStr = TempStoreStr + "Receipt_Date	datetime, ";
                            TempStoreStr = TempStoreStr + "ReceiptMoney_Cur numeric(19,6),";
                            TempStoreStr = TempStoreStr + "ReceiptMoney_Tot numeric(19,6),";
	                        TempStoreStr = TempStoreStr + "SubconValue      numeric(19,6),";
	                        TempStoreStr = TempStoreStr + "AC_PERIOD		nvarchar(255))";

                            SqlCommand command_a = new SqlCommand(TempStoreStr, connection);
                            connection.Open();

                            command_a.Connection = connection;
                            command_a.CommandTimeout = 60000;
                            command_a.ExecuteNonQuery();

                            TempStoreStr = "INSERT INTO #TR_Rpt exec sp_AC02_Detail '" + Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "");
                            TempStoreStr = TempStoreStr + "', '" + Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "") +"'";
                            
                            SqlCommand command_b = new SqlCommand(TempStoreStr, connection);
                            command_b.Connection = connection;
                            command_b.CommandTimeout = 60000;
                            command_b.ExecuteNonQuery();

                            TempStoreStr = "select NatureOfWork, SubsiCode,SubSiName, SubCon, SubConName, SubconValue, PrjCode, PrjName, PCMSDocNum, Cert_Date,AC_PERIOD,Cert_no,WorksCertified_Cur,WorksCertified_Tot,RetentionMoney_Cur,RetentionMoney_Tot,Deductions_Cur,Deductions_Tot,ReceiptMoney_Cur,ReceiptMoney_Tot,Receipt_Date, Ref";
                            //TempStoreStr = TempStoreStr + " FROM #TR_Rpt group by SubConName, NatureOfWork, SubsiCode,SubSiName, SubCon, SubconValue, PrjCode, PrjName, PCMSDocNum, Cert_Date,AC_PERIOD,Cert_no,WorksCertified_Cur,WorksCertified_Tot,RetentionMoney_Cur,RetentionMoney_Tot,Deductions_Cur,Deductions_Tot,ReceiptMoney_Cur,ReceiptMoney_Tot,Receipt_Date, Ref order by SubConName, Ref, Cert_Date";
                            TempStoreStr = TempStoreStr + " FROM #TR_Rpt";

                            SqlCommand command_c = new SqlCommand(TempStoreStr, connection);
                            command_c.Connection = connection;
                            command_c.CommandTimeout = 60000;
                            /*
                            SqlCommand command = new SqlCommand("sp_AC02_Detail", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Add(new SqlParameter("@FDocEntry", Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "")));
                            command.Parameters.Add(new SqlParameter("@TDocEntry", Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "")));

                            connection.Open();
                            command.Connection = connection;
                            command.CommandTimeout = 12000;
                            */
                            intRow = 1;

                            /*
                            SubsiCode
                            SubSiName
                            SubCon
                            SubConName
                            PrjCode
                            PrjName
                            NatureOfWork
                            PCMSDocNum
                            Ref
                            Cert_no
                            Cert_Date
                            WorksCertified_Cur
                            WorksCertified_Tot
                            RetentionMoney_Cur
                            RetentionMoney_Tot
                            Deductions_Cur
                            Deductions_Tot
                            Receipt_Date
                            ReceiptMoney_Cur
                            ReceiptMoney_Tot
                            SubconValue
                            AC_PERIOD
                             */

                            oSheet.get_Range("A" + intRow.ToString(), "A" + intRow.ToString()).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                            oSheet.Cells[intRow, 1] = "PAUL Y. CONSTRUCTION COMPANY, LIMITED";
                            oSheet.get_Range("A" + intRow.ToString(), "A" + intRow.ToString()).Font.Bold = true;
                            intRow = intRow + 1;
                            oSheet.Cells[intRow, 5] = "Print Date and Time:" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                            
                            oSheet.get_Range("A" + intRow.ToString(), "A" + intRow.ToString()).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                            oSheet.get_Range("A" + intRow.ToString(), "A" + intRow.ToString()).Font.Bold = true;
                            oSheet.Cells[intRow, 1] = "SUB-CONTRACTORS WORKS AND PAYMENTS RECORDS";

                            using (SqlDataReader datareader = command_c.ExecuteReader())
                            
                            
                            {
                                while (datareader.Read())
                                {
                                    if (datareader.HasRows == true)
                                    {
                                        //if (previous_SubConName == "" )
                                        if(String.IsNullOrEmpty(odd_PCMSDocNum))
                                        {
                                            //intRow = intRow + 1;
                                            SubContractor_Row = intRow + SubContractor_Row;
                                            Project_Row = intRow + Project_Row;
                                            Nature_of_Works_Row = intRow + Nature_of_Works_Row;

                                            oSheet.Cells[SubContractor_Row, 1] = "Subcontractor :";
                                            oSheet.Cells[Project_Row, 1] = "Site / Project :";
                                            oSheet.Cells[Nature_of_Works_Row, 1] = "Nature of Works :";

                                            intRow = Nature_of_Works_Row + 2;
                                            if (datareader["SubConName"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[SubContractor_Row, 2] = datareader["SubConName"].ToString();
                                                previous_SubConName = datareader["SubConName"].ToString();

                                                string temp = "B" + SubContractor_Row.ToString();

                                                oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                                                Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                            }

                                            oSheet.Cells[SubContractor_Row, 3] = "SubContract Value HK$";
                                            if (datareader["SubconValue"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[SubContractor_Row, 4] = datareader["SubconValue"].ToString();


                                                if (Grand_Total_Subcontract_Value_F1 == "")
                                                {
                                                    Grand_Total_Subcontract_Value_F1 = "=+" + "D" + SubContractor_Row.ToString();
                                                }
                                                else
                                                {
                                                    Grand_Total_Subcontract_Value_F1 = Grand_Total_Subcontract_Value_F1 + "+" + "D" + SubContractor_Row.ToString();
                                                }

                                                oSheet.get_Range("D" + SubContractor_Row.ToString(), "D" + SubContractor_Row.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                                oSheet.get_Range("D" + SubContractor_Row.ToString(), "D" + SubContractor_Row.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                                            }

                                            if (datareader["PrjCode"] != System.DBNull.Value && datareader["PrjName"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[Project_Row, 2] = datareader["PrjCode"].ToString() + " " + datareader["PrjName"].ToString();

                                                string temp = "B" + Project_Row.ToString();
                                                Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                            }

                                            if (datareader["NatureOfWork"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[Nature_of_Works_Row, 2] = datareader["NatureOfWork"].ToString();

                                                string temp = "B" + Nature_of_Works_Row.ToString();
                                                Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                            }

                                            //StartRow = false;
                                            if (datareader["PCMSDocNum"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[intRow - 2, 11] = datareader["PCMSDocNum"].ToString().Substring(0, datareader["PCMSDocNum"].ToString().IndexOf('/',11));

                                                string temp = "K" + (intRow - 2).ToString();
                                                oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                                previous_PCDocNum = datareader["PCMSDocNum"].ToString().Substring(0, datareader["PCMSDocNum"].ToString().IndexOf('/', 11));
                                            }
                                            for (int i = 0; i < datareader.FieldCount; i++)
                                            {
                                                if (datareader.GetName(i) == "Cert_Date")
                                                {
                                                    oSheet.Cells[intRow, 1] = "Date of Cert";

                                                    string temp = "A" + intRow.ToString();
                                                    Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                }
                                                if (datareader.GetName(i) == "AC_PERIOD")
                                                {
                                                    oSheet.Cells[intRow, 2] = "A/C PERIOD";

                                                    string temp = "B" + intRow.ToString();
                                                    Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                }
                                                if (datareader.GetName(i) == "Cert_no")
                                                {
                                                    oSheet.Cells[intRow, 3] = "Cert No";

                                                    string temp = "C" + intRow.ToString();
                                                    Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                }
                                                if (datareader.GetName(i) == "WorksCertified_Cur")
                                                {
                                                    oSheet.Cells[intRow, 4] = "Current";
                                                    
                                                    oSheet.Cells[intRow - 1, 4] = "Works Certified";
                                                    
                                                    int temprow = intRow - 1;

                                                    Excel.Border border = oSheet.get_Range("D" + temprow.ToString(), "D" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border1 = oSheet.get_Range("D" + temprow.ToString(), "D" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border1.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border2 = oSheet.get_Range("D" + temprow.ToString(), "D" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeLeft];
                                                    border2.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    string temp = "D" + intRow.ToString();
                                                    Excel.Border border3 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border4 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeLeft];
                                                    border4.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border5 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border5.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);
                                                }
                                                if (datareader.GetName(i) == "WorksCertified_Tot")
                                                {
                                                    oSheet.Cells[intRow, 5] = "Total";

                                                    int temprow = intRow - 1;

                                                    Excel.Border border = oSheet.get_Range("E" + temprow.ToString(), "E" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border1 = oSheet.get_Range("E" + temprow.ToString(), "E" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border1.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border2 = oSheet.get_Range("E" + temprow.ToString(), "E" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border2.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    oSheet.get_Range("D" + temprow.ToString(), "E" + temprow.ToString()).MergeCells = true;
                                                    oSheet.get_Range("D" + temprow.ToString(), "E" + temprow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                                                    oSheet.get_Range("D" + temprow.ToString(), "E" + temprow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);

                                                    string temp = "E" + intRow.ToString();
                                                    Excel.Border border3 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border4 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border4.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);
                                                }
                                                if (datareader.GetName(i) == "RetentionMoney_Cur")
                                                {
                                                    oSheet.Cells[intRow, 6] = "Current";
                                                    oSheet.Cells[intRow - 1, 6] = "Retention Money";

                                                    int temprow = intRow - 1;

                                                    Excel.Border border = oSheet.get_Range("F" + temprow.ToString(), "F" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border1 = oSheet.get_Range("F" + temprow.ToString(), "F" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border1.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    string temp = "F" + intRow.ToString();
                                                    Excel.Border border2 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border2.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border3 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border3.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);
                                                }
                                                if (datareader.GetName(i) == "RetentionMoney_Tot")
                                                {
                                                    oSheet.Cells[intRow, 7] = "Total";

                                                    int temprow = intRow - 1;

                                                    Excel.Border border = oSheet.get_Range("G" + temprow.ToString(), "G" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border1 = oSheet.get_Range("G" + temprow.ToString(), "G" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border1.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border2 = oSheet.get_Range("G" + temprow.ToString(), "G" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border2.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    oSheet.get_Range("F" + temprow.ToString(), "G" + temprow.ToString()).MergeCells = true;
                                                    oSheet.get_Range("F" + temprow.ToString(), "G" + temprow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range("F" + temprow.ToString(), "G" + temprow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);

                                                    string temp = "G" + intRow.ToString();
                                                    Excel.Border border3 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border4 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border4.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);
                                                }


                                                if (datareader.GetName(i) == "Deductions_Cur")
                                                {
                                                    oSheet.Cells[intRow, 8] = "Current";
                                                    oSheet.Cells[intRow - 1, 8] = "Deductions";


                                                    int temprow = intRow - 1;

                                                    Excel.Border border = oSheet.get_Range("H" + temprow.ToString(), "H" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border1 = oSheet.get_Range("H" + temprow.ToString(), "H" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border1.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    string temp = "H" + intRow.ToString();
                                                    Excel.Border border2 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border2.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border3 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);
                                                }
                                                if (datareader.GetName(i) == "Deductions_Tot")
                                                {
                                                    oSheet.Cells[intRow, 9] = "Total";

                                                    int temprow = intRow - 1;

                                                    Excel.Border border = oSheet.get_Range("I" + temprow.ToString(), "I" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border1 = oSheet.get_Range("I" + temprow.ToString(), "I" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border1.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border2 = oSheet.get_Range("I" + temprow.ToString(), "I" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border2.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    oSheet.get_Range("H" + temprow.ToString(), "I" + temprow.ToString()).MergeCells = true;
                                                    oSheet.get_Range("H" + temprow.ToString(), "I" + temprow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range("H" + temprow.ToString(), "I" + temprow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);

                                                    string temp = "I" + intRow.ToString();
                                                    Excel.Border border3 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border4 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border4.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);
                                                }

                                                if (datareader.GetName(i) == "ReceiptMoney_Cur")
                                                {
                                                    oSheet.Cells[intRow, 10] = "Current";
                                                    oSheet.Cells[intRow - 1, 10] = "Payment";

                                                    int temprow = intRow - 1;

                                                    Excel.Border border = oSheet.get_Range("J" + temprow.ToString(), "J" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border1 = oSheet.get_Range("J" + temprow.ToString(), "J" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border1.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    string temp = "J" + intRow.ToString();
                                                    Excel.Border border2 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border2.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border3 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);
                                                }
                                                if (datareader.GetName(i) == "ReceiptMoney_Tot")
                                                {
                                                    oSheet.Cells[intRow, 11] = "Total";

                                                    int temprow = intRow - 1;

                                                    Excel.Border border = oSheet.get_Range("K" + temprow.ToString(), "K" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border1 = oSheet.get_Range("K" + temprow.ToString(), "K" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border1.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border2 = oSheet.get_Range("K" + temprow.ToString(), "K" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border2.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    oSheet.get_Range("J" + temprow.ToString(), "K" + temprow.ToString()).MergeCells = true;
                                                    oSheet.get_Range("J" + temprow.ToString(), "K" + temprow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range("J" + temprow.ToString(), "K" + temprow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);

                                                    string temp = "K" + intRow.ToString();
                                                    Excel.Border border3 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border4 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border4.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);
                                                }
                                                if (datareader.GetName(i) == "Receipt_Date")
                                                {
                                                    oSheet.Cells[intRow, 12] = "Date";

                                                    string temp = "L" + intRow.ToString();
                                                    Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border1 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border1.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border2 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border2.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);
                                                }
                                            }
                                            intRow = intRow + 1;
                                        }
                                        
                                        odd_PCMSDocNum = "";
                                        if (datareader["PrjCode"] != System.DBNull.Value && datareader["PrjName"] != System.DBNull.Value)
                                        {
                                            if( datareader["PCMSDocNum"].ToString().Length > 0 )
                                                odd_PCMSDocNum = datareader["PCMSDocNum"].ToString().Substring(0, datareader["PCMSDocNum"].ToString().IndexOf('/', 11));
                                            else
                                                odd_PCMSDocNum = previous_PCDocNum;
                                        }
                                        else
                                        {
                                            odd_PCMSDocNum = previous_PCDocNum;
                                        }

                                        if (!String.IsNullOrEmpty(previous_PCDocNum) && previous_PCDocNum != odd_PCMSDocNum)
                                        {
                                            intRow = intRow + 2;
                                            SubContractor_Row = intRow + 1;
                                            Project_Row = intRow + 2;
                                            Nature_of_Works_Row = intRow + 3;
                                            oSheet.Cells[SubContractor_Row, 1] = "Subcontractor :";
                                            oSheet.Cells[Project_Row, 1] = "Site / Project :";
                                            oSheet.Cells[Nature_of_Works_Row, 1] = "Nature of Works :";

                                            intRow = Nature_of_Works_Row + 2;
                                            if (datareader["SubConName"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[SubContractor_Row, 2] = datareader["SubConName"].ToString();
                                                previous_SubConName = datareader["SubConName"].ToString();

                                                string temp = "B" + SubContractor_Row.ToString();
                                                oSheet.get_Range(temp,  temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                                                Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                            }

                                            oSheet.Cells[SubContractor_Row, 3] = "SubContract Value HK$";
                                            if (datareader["SubconValue"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[SubContractor_Row, 4] = datareader["SubconValue"].ToString();


                                                if (Grand_Total_Subcontract_Value_F1 == "")
                                                {
                                                    Grand_Total_Subcontract_Value_F1 = "=+" + "D" + SubContractor_Row.ToString();
                                                }
                                                else
                                                {
                                                    Grand_Total_Subcontract_Value_F1 = Grand_Total_Subcontract_Value_F1 + "+" + "D" + SubContractor_Row.ToString();
                                                }

                                                oSheet.get_Range("D" + SubContractor_Row.ToString(), "D" + SubContractor_Row.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                                oSheet.get_Range("D" + SubContractor_Row.ToString(), "D" + SubContractor_Row.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                                            }
                                            if (datareader["PrjCode"] != System.DBNull.Value && datareader["PrjName"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[Project_Row, 2] = datareader["PrjCode"].ToString() + " " + datareader["PrjName"].ToString();
                                                previous_ProjectName = datareader["PrjCode"].ToString() + " " + datareader["PrjName"].ToString();

                                                string temp = "B" + Project_Row.ToString();
                                                Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                            }
                                            if (datareader["NatureOfWork"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[Nature_of_Works_Row, 2] = datareader["NatureOfWork"].ToString();
                                                previous_NatureOfWorks = datareader["NatureOfWork"].ToString();

                                                string temp = "B" + Nature_of_Works_Row.ToString();
                                                Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                            }

                                            //StartRow = false;
                                            if (datareader["PCMSDocNum"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[intRow - 2, 11] = datareader["PCMSDocNum"].ToString().Substring(0, datareader["PCMSDocNum"].ToString().IndexOf('/', 11));

                                                oSheet.get_Range("K" + (intRow - 2).ToString(), "K" + (intRow - 2).ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                                previous_PCDocNum = datareader["PCMSDocNum"].ToString().Substring(0, datareader["PCMSDocNum"].ToString().IndexOf('/', 11));
                                            }
                                            for (int i = 0; i < datareader.FieldCount; i++)
                                            {
                                                if (datareader.GetName(i) == "Cert_Date")
                                                {
                                                    oSheet.Cells[intRow, 1] = "Date of Cert";

                                                    string temp = "A" + intRow.ToString();
                                                    Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                }
                                                if (datareader.GetName(i) == "AC_PERIOD")
                                                {
                                                    oSheet.Cells[intRow, 2] = "A/C PERIOD";

                                                    string temp = "B" + intRow.ToString();
                                                    Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                }
                                                if (datareader.GetName(i) == "Cert_no")
                                                {
                                                    oSheet.Cells[intRow, 3] = "Cert No";

                                                    string temp = "C" + intRow.ToString();
                                                    Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                }
                                                if (datareader.GetName(i) == "WorksCertified_Cur")
                                                {
                                                    oSheet.Cells[intRow, 4] = "Current";
                                                    oSheet.Cells[intRow - 1, 4] = "Works Certified";

                                                    int temprow = intRow - 1;

                                                    Excel.Border border = oSheet.get_Range("D" + temprow.ToString(), "D" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border1 = oSheet.get_Range("D" + temprow.ToString(), "D" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border1.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border2 = oSheet.get_Range("D" + temprow.ToString(), "D" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeLeft];
                                                    border2.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    string temp = "D" + intRow.ToString();
                                                    Excel.Border border3 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border4 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeLeft];
                                                    border4.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border5 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border5.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);

                                                    if (Grand_Total_WorkCertified_Current == "")
                                                    {
                                                        Grand_Total_WorkCertified_Current = "=" + Temp_Store_Last_WorkCertified_Current;
                                                    }
                                                    else
                                                    {
                                                        Grand_Total_WorkCertified_Current = Grand_Total_WorkCertified_Current + Temp_Store_Last_WorkCertified_Current;
                                                    }

                                                }
                                                if (datareader.GetName(i) == "WorksCertified_Tot")
                                                {
                                                    oSheet.Cells[intRow, 5] = "Total";


                                                    int temprow = intRow - 1;

                                                    Excel.Border border = oSheet.get_Range("E" + temprow.ToString(), "E" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border1 = oSheet.get_Range("E" + temprow.ToString(), "E" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border1.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border2 = oSheet.get_Range("E" + temprow.ToString(), "E" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border2.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    oSheet.get_Range("D" + temprow.ToString(), "E" + temprow.ToString()).MergeCells = true;
                                                    oSheet.get_Range("D" + temprow.ToString(), "E" + temprow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range("D" + temprow.ToString(), "E" + temprow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);

                                                    string temp = "E" + intRow.ToString();
                                                    Excel.Border border3 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border4 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border4.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);

                                                    if (Grand_Total_WorkCertified_Total == "")
                                                    {
                                                        Grand_Total_WorkCertified_Total = "=" + Temp_Store_Last_WorkCertified_Total;
                                                    }
                                                    else
                                                    {
                                                        Grand_Total_WorkCertified_Total = Grand_Total_WorkCertified_Total + Temp_Store_Last_WorkCertified_Total;
                                                    }
                                                }
                                                if (datareader.GetName(i) == "RetentionMoney_Cur")
                                                {
                                                    oSheet.Cells[intRow, 6] = "Current";
                                                    oSheet.Cells[intRow - 1, 6] = "Retention Money";

                                                    int temprow = intRow - 1;

                                                    Excel.Border border = oSheet.get_Range("F" + temprow.ToString(), "F" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border1 = oSheet.get_Range("F" + temprow.ToString(), "F" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border1.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    string temp = "F" + intRow.ToString();
                                                    Excel.Border border2 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border2.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border3 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border3.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);


                                                    if (Grand_Total_RententionMoney_Current == "")
                                                    {
                                                        Grand_Total_RententionMoney_Current = "=" + Temp_Store_Last_RententionMoney_Current;
                                                    }
                                                    else
                                                    {
                                                        Grand_Total_RententionMoney_Current = Grand_Total_RententionMoney_Current + Temp_Store_Last_RententionMoney_Current;
                                                    }
                                                }
                                                if (datareader.GetName(i) == "RetentionMoney_Tot")
                                                {
                                                    oSheet.Cells[intRow, 7] = "Total";

                                                    int temprow = intRow - 1;

                                                    Excel.Border border = oSheet.get_Range("G" + temprow.ToString(), "G" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border1 = oSheet.get_Range("G" + temprow.ToString(), "G" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border1.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border2 = oSheet.get_Range("G" + temprow.ToString(), "G" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border2.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    oSheet.get_Range("F" + temprow.ToString(), "G" + temprow.ToString()).MergeCells = true;
                                                    oSheet.get_Range("F" + temprow.ToString(), "G" + temprow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range("F" + temprow.ToString(), "G" + temprow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);


                                                    string temp = "G" + intRow.ToString();
                                                    Excel.Border border3 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border4 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border4.LineStyle = Excel.XlLineStyle.xlContinuous;


                                                    oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);

                                                    if (Grand_Total_RententionMoney_Total == "")
                                                    {
                                                        Grand_Total_RententionMoney_Total = "=" + Temp_Store_Last_RententionMoney_Total;
                                                    }
                                                    else
                                                    {
                                                        Grand_Total_RententionMoney_Total = Grand_Total_RententionMoney_Total + Temp_Store_Last_RententionMoney_Total;
                                                    }
                                                }


                                                if (datareader.GetName(i) == "Deductions_Cur")
                                                {
                                                    oSheet.Cells[intRow, 8] = "Current";
                                                    oSheet.Cells[intRow - 1, 8] = "Deductions";

                                                    int temprow = intRow - 1;

                                                    Excel.Border border = oSheet.get_Range("H" + temprow.ToString(), "H" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border1 = oSheet.get_Range("H" + temprow.ToString(), "H" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border1.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    string temp = "H" + intRow.ToString();
                                                    Excel.Border border2 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border2.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border3 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border3.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);

                                                    if (Grand_Total_Deductions_Current == "")
                                                    {
                                                        Grand_Total_Deductions_Current = "=" + Temp_Store_Last_Deductions_Current;
                                                    }
                                                    else
                                                    {
                                                        Grand_Total_Deductions_Current = Grand_Total_Deductions_Current + Temp_Store_Last_Deductions_Current;
                                                    }
                                                }
                                                if (datareader.GetName(i) == "Deductions_Tot")
                                                {
                                                    oSheet.Cells[intRow, 9] = "Total";

                                                    int temprow = intRow - 1;

                                                    Excel.Border border = oSheet.get_Range("I" + temprow.ToString(), "I" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border1 = oSheet.get_Range("I" + temprow.ToString(), "I" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border1.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border2 = oSheet.get_Range("I" + temprow.ToString(), "I" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border2.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    oSheet.get_Range("H" + temprow.ToString(), "I" + temprow.ToString()).MergeCells = true;
                                                    oSheet.get_Range("H" + temprow.ToString(), "I" + temprow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range("H" + temprow.ToString(), "I" + temprow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);


                                                    string temp = "I" + intRow.ToString();
                                                    Excel.Border border3 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border4 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border4.LineStyle = Excel.XlLineStyle.xlContinuous;


                                                    oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);

                                                    if (Grand_Total_Deductions_Total == "")
                                                    {
                                                        Grand_Total_Deductions_Total = "=" + Temp_Store_Last_Deductions_Total;
                                                    }
                                                    else
                                                    {
                                                        Grand_Total_Deductions_Total = Grand_Total_Deductions_Total + Temp_Store_Last_Deductions_Total;
                                                    }
                                                }

                                                if (datareader.GetName(i) == "ReceiptMoney_Cur")
                                                {
                                                    oSheet.Cells[intRow, 10] = "Current";
                                                    oSheet.Cells[intRow - 1, 10] = "Payment";

                                                    int temprow = intRow - 1;

                                                    Excel.Border border = oSheet.get_Range("J" + temprow.ToString(), "J" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border1 = oSheet.get_Range("J" + temprow.ToString(), "J" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border1.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    string temp = "J" + intRow.ToString();
                                                    Excel.Border border2 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border2.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border3 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border3.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);

                                                    if (Grand_Total_Payment_Current == "")
                                                    {
                                                        Grand_Total_Payment_Current = "=" + Temp_Store_Last_Payment_Current;
                                                    }
                                                    else
                                                    {
                                                        Grand_Total_Payment_Current = Grand_Total_Payment_Current + Temp_Store_Last_Payment_Current;
                                                    }
                                                }
                                                if (datareader.GetName(i) == "ReceiptMoney_Tot")
                                                {
                                                    oSheet.Cells[intRow, 11] = "Total";

                                                    int temprow = intRow - 1;

                                                    Excel.Border border = oSheet.get_Range("K" + temprow.ToString(), "K" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border1 = oSheet.get_Range("K" + temprow.ToString(), "K" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border1.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border2 = oSheet.get_Range("K" + temprow.ToString(), "K" + temprow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border2.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    oSheet.get_Range("J" + temprow.ToString(), "K" + temprow.ToString()).MergeCells = true;
                                                    oSheet.get_Range("J" + temprow.ToString(), "K" + temprow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range("J" + temprow.ToString(), "K" + temprow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);

                                                    string temp = "K" + intRow.ToString();
                                                    Excel.Border border3 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border4 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border4.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    oSheet.get_Range(temp, temp).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);

                                                    if (Grand_Total_Payment_Total == "")
                                                    {
                                                        Grand_Total_Payment_Total = "=" + Temp_Store_Last_Payment_Total;
                                                    }
                                                    else
                                                    {
                                                        Grand_Total_Payment_Total = Grand_Total_Payment_Total + Temp_Store_Last_Payment_Total;
                                                    }

                                                }
                                                if (datareader.GetName(i) == "Receipt_Date")
                                                {
                                                    oSheet.Cells[intRow, 12] = "Date";

                                                    string temp = "L" + intRow.ToString();
                                                    Excel.Border border = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                                    border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border1 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                    border1.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    Excel.Border border2 = oSheet.get_Range(temp, temp).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                                    border2.LineStyle = Excel.XlLineStyle.xlContinuous;

                                                    oSheet.get_Range(temp, temp).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.PaleTurquoise);
                                                }
                                            }
                                            intRow = intRow + 1;
                                        }

                                        if (datareader["Cert_Date"] != System.DBNull.Value)
                                        {
                                            //oSheet.Cells[intRow, 1] = datareader["Cert_Date"];

                                            oRng = oSheet.get_Range("A" + intRow.ToString(), "A" + intRow.ToString());
                                            oRng.set_Value(Missing, datareader["Cert_Date"]);
                                            oSheet.get_Range("A" + intRow.ToString(), "A" + intRow.ToString()).NumberFormat = "dd/MM/yyyy";
                                            oSheet.get_Range("A" + intRow.ToString(), "A" + intRow.ToString()).Font.Bold = true;
                                            oSheet.get_Range("A" + intRow.ToString(), "A" + intRow.ToString()).Font.Name = "Courier New";
                                        }
                                        total_payment_cur = 0;
                                        total_payment_tot = 0;

                                        Temp_Store_Last_WorkCertified_Current = "";
                                        Temp_Store_Last_WorkCertified_Total = "";
                                        Temp_Store_Last_RententionMoney_Current = "";
                                        Temp_Store_Last_RententionMoney_Total = "";
                                        Temp_Store_Last_Deductions_Current = "";
                                        Temp_Store_Last_Deductions_Total = "";
                                        Temp_Store_Last_Payment_Current = "";
                                        Temp_Store_Last_Payment_Total = "";
                                        if (datareader["AC_PERIOD"] != System.DBNull.Value)
                                        {
                                            oSheet.Cells[intRow, 2] = "'" + Convert.ToDateTime(datareader["AC_PERIOD"]).ToString("MMM") + "/" + Convert.ToDateTime(datareader["AC_PERIOD"]).Year.ToString();
                                            oSheet.get_Range("B" + intRow.ToString(), "B" + intRow.ToString()).Font.Bold = true;
                                            oSheet.get_Range("B" + intRow.ToString(), "B" + intRow.ToString()).Font.Name = "Courier New";
                                        }

                                        if (datareader["Cert_no"] != System.DBNull.Value)
                                            oSheet.Cells[intRow, 3] = datareader["Cert_no"].ToString();
                                        if (datareader["WorksCertified_Cur"] != System.DBNull.Value)
                                        {
                                            oSheet.Cells[intRow, 4] = Convert.ToDouble(datareader["WorksCertified_Cur"].ToString());
                                            total_payment_cur = Convert.ToDouble(datareader["WorksCertified_Cur"].ToString());
                                            oSheet.get_Range("D" + intRow.ToString(), "D" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                            Temp_Store_Last_WorkCertified_Current = "+D" + intRow.ToString();

                                        }
                                        if (datareader["WorksCertified_Tot"] != System.DBNull.Value)
                                        {
                                            oSheet.Cells[intRow, 5] = Convert.ToDouble(datareader["WorksCertified_Tot"].ToString());
                                            total_payment_tot = Convert.ToDouble(datareader["WorksCertified_Tot"].ToString());
                                            oSheet.get_Range("E" + intRow.ToString(), "E" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                            Temp_Store_Last_WorkCertified_Total = "+E" + intRow.ToString();
                                        }
                                        if (datareader["RetentionMoney_Cur"] != System.DBNull.Value)
                                        {
                                            oSheet.Cells[intRow, 6] = Convert.ToDouble(datareader["RetentionMoney_Cur"].ToString());
                                            total_payment_cur = total_payment_cur + Convert.ToDouble(datareader["RetentionMoney_Cur"].ToString());
                                            oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                            Temp_Store_Last_RententionMoney_Current = "+F" + intRow.ToString();
                                        }
                                        if (datareader["RetentionMoney_Tot"] != System.DBNull.Value)
                                        {
                                            oSheet.Cells[intRow, 7] = Convert.ToDouble(datareader["RetentionMoney_Tot"].ToString());
                                            total_payment_tot = total_payment_tot + Convert.ToDouble(datareader["RetentionMoney_Tot"].ToString());
                                            oSheet.get_Range("G" + intRow.ToString(), "G" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                            Temp_Store_Last_RententionMoney_Total = "+G" + intRow.ToString();
                                        }
                                        if (datareader["Deductions_Cur"] != System.DBNull.Value)
                                        {
                                            oSheet.Cells[intRow, 8] = Convert.ToDouble(datareader["Deductions_Cur"].ToString());
                                            total_payment_cur = total_payment_cur + Convert.ToDouble(datareader["Deductions_Cur"].ToString());
                                            oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                            Temp_Store_Last_Deductions_Current = "+H" + intRow.ToString();
                                        }
                                        if (datareader["Deductions_Tot"] != System.DBNull.Value)
                                        {
                                            oSheet.Cells[intRow, 9] = Convert.ToDouble(datareader["Deductions_Tot"].ToString());
                                            total_payment_tot = total_payment_tot + Convert.ToDouble(datareader["Deductions_Tot"].ToString());
                                            oSheet.get_Range("I" + intRow.ToString(), "I" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                            Temp_Store_Last_Deductions_Total = "+I" + intRow.ToString();
                                        }

                                        //if (datareader["NatureOfWork"].ToString() == "Rental of Forklift")
                                            //Console.WriteLine("");

                                        if (datareader["ReceiptMoney_Cur"] != System.DBNull.Value)
                                        {
                                            //oSheet.Cells[intRow, 10] = total_payment_cur;
                                            oSheet.Cells[intRow, 10] = Convert.ToDouble(datareader["ReceiptMoney_Cur"].ToString());
                                            oSheet.get_Range("J" + intRow.ToString(), "J" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                            Temp_Store_Last_Payment_Current = "+J" + intRow.ToString();
                                        }
                                        if (datareader["ReceiptMoney_Tot"] != System.DBNull.Value)
                                        {
                                            //oSheet.Cells[intRow, 11] = total_payment_tot;
                                            oSheet.Cells[intRow, 11] = Convert.ToDouble(datareader["ReceiptMoney_Tot"].ToString());
                                            oSheet.get_Range("K" + intRow.ToString(), "K" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                            Temp_Store_Last_Payment_Total = "+K" + intRow.ToString();
                                        }
                                        if (datareader["Receipt_Date"] != System.DBNull.Value)
                                        {
                                            //oSheet.Cells[intRow, 12] = Convert.ToDateTime(datareader["Receipt_Date"]);

                                            oRng = oSheet.get_Range("L" + intRow.ToString(), "L" + intRow.ToString());
                                            oRng.set_Value(Missing, datareader["Receipt_Date"]);
                                            oSheet.get_Range("L" + intRow.ToString(), "L" + intRow.ToString()).NumberFormat = "dd/MM/yyyy";
                                        }

                                        intRow = intRow + 1;
                                    }
                                }
                                /*
                                oSheet.Cells[intRow, 4] = "Grand Total";
                                oSheet.Cells[intRow, 5] = Grant_Total_Of_InvAmt.ToString();
                                oSheet.Cells[intRow, 6] = Grant_Total_Of_AmtDeduct.ToString();
                                oSheet.Cells[intRow, 8] = Grant_Total_Of_DIFF.ToString();

                                oSheet.get_Range("E" + intRow.ToString(), "E" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                                oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                                oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                            
                                string temp3 = "E" + intRow.ToString();
                                Excel.Border border3 = oSheet.get_Range(temp3, temp3).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border3.LineStyle = Excel.XlLineStyle.xlContinuous;

                                temp3 = "F" + intRow.ToString();
                                border3 = oSheet.get_Range(temp3, temp3).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border3.LineStyle = Excel.XlLineStyle.xlContinuous;

                                temp3 = "H" + intRow.ToString();
                                border3 = oSheet.get_Range(temp3, temp3).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border3.LineStyle = Excel.XlLineStyle.xlContinuous;

                                temp3 = "A" + (intRow-1).ToString();
                                border3 = oSheet.get_Range(temp3, temp3).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                temp3 = "B" + (intRow - 1).ToString();
                                border3 = oSheet.get_Range(temp3, temp3).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                temp3 = "C" + (intRow - 1).ToString();
                                border3 = oSheet.get_Range(temp3, temp3).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                temp3 = "D" + (intRow - 1).ToString();
                                border3 = oSheet.get_Range(temp3, temp3).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                temp3 = "E" + (intRow - 1).ToString();
                                border3 = oSheet.get_Range(temp3, temp3).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                temp3 = "F" + (intRow - 1).ToString();
                                border3 = oSheet.get_Range(temp3, temp3).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                temp3 = "G" + (intRow - 1).ToString();
                                border3 = oSheet.get_Range(temp3, temp3).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                temp3 = "H" + (intRow - 1).ToString();
                                border3 = oSheet.get_Range(temp3, temp3).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                temp3 = "I" + (intRow - 1).ToString();
                                border3 = oSheet.get_Range(temp3, temp3).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                            

                                //((Excel.Range)oSheet.Columns["A", Missing]).ColumnWidth = 10;
                                //((Excel.Range)oSheet.Columns["B", Missing]).ColumnWidth = 10;

                                ((Excel.Range)oSheet.Cells[1, 1]).EntireColumn.ColumnWidth = 10;
                                ((Excel.Range)oSheet.Cells[1, 2]).EntireColumn.ColumnWidth = 10;
                                */

                                if (Temp_Store_Last_WorkCertified_Current != "")
                                {
                                    if (Grand_Total_WorkCertified_Current == "")
                                    {
                                        Grand_Total_WorkCertified_Current = "=" + Temp_Store_Last_WorkCertified_Current;
                                    }
                                    else
                                    {
                                        Grand_Total_WorkCertified_Current = Grand_Total_WorkCertified_Current + Temp_Store_Last_WorkCertified_Current;
                                    }
                                }


                                if (Temp_Store_Last_WorkCertified_Total != "")
                                {
                                    if (Grand_Total_WorkCertified_Total == "")
                                    {
                                        Grand_Total_WorkCertified_Total = "=" + Temp_Store_Last_WorkCertified_Total;
                                    }
                                    else
                                    {
                                        Grand_Total_WorkCertified_Total = Grand_Total_WorkCertified_Total + Temp_Store_Last_WorkCertified_Total;
                                    }
                                }

                                if (Temp_Store_Last_RententionMoney_Current != "")
                                {
                                    if (Grand_Total_RententionMoney_Current == "")
                                    {
                                        Grand_Total_RententionMoney_Current = "=" + Temp_Store_Last_RententionMoney_Current;
                                    }
                                    else
                                    {
                                        Grand_Total_RententionMoney_Current = Grand_Total_RententionMoney_Current + Temp_Store_Last_RententionMoney_Current;
                                    }
                                }

                                if (Temp_Store_Last_RententionMoney_Total != "")
                                {
                                    if (Grand_Total_RententionMoney_Total == "")
                                    {
                                        Grand_Total_RententionMoney_Total = "=" + Temp_Store_Last_RententionMoney_Total;
                                    }
                                    else
                                    {
                                        Grand_Total_RententionMoney_Total = Grand_Total_RententionMoney_Total + Temp_Store_Last_RententionMoney_Total;
                                    }
                                }

                                if (Temp_Store_Last_Deductions_Current != "")
                                {
                                    if (Grand_Total_Deductions_Current == "")
                                    {
                                        Grand_Total_Deductions_Current = "=" + Temp_Store_Last_Deductions_Current;
                                    }
                                    else
                                    {
                                        Grand_Total_Deductions_Current = Grand_Total_Deductions_Current + Temp_Store_Last_Deductions_Current;
                                    }
                                }

                                if (Temp_Store_Last_Deductions_Total != "")
                                {
                                    if (Grand_Total_Deductions_Total == "")
                                    {
                                        Grand_Total_Deductions_Total = "=" + Temp_Store_Last_Deductions_Total;
                                    }
                                    else
                                    {
                                        Grand_Total_Deductions_Total = Grand_Total_Deductions_Total + Temp_Store_Last_Deductions_Total;
                                    }
                                }

                                if (Temp_Store_Last_Payment_Current != "")
                                {
                                    if (Grand_Total_Payment_Current == "")
                                    {
                                        Grand_Total_Payment_Current = "=" + Temp_Store_Last_Payment_Current;
                                    }
                                    else
                                    {
                                        Grand_Total_Payment_Current = Grand_Total_Payment_Current + Temp_Store_Last_Payment_Current;
                                    }
                                }

                                if (Temp_Store_Last_Payment_Total != "")
                                {
                                    if (Grand_Total_Payment_Total == "")
                                    {
                                        Grand_Total_Payment_Total = "=" + Temp_Store_Last_Payment_Total;
                                    }
                                    else
                                    {
                                        Grand_Total_Payment_Total = Grand_Total_Payment_Total + Temp_Store_Last_Payment_Total;
                                    }
                                }

                                intRow = intRow + 3;
                                oSheet.Cells[intRow, 3] = "Grand Total";

                                if (Grand_Total_Subcontract_Value_F1 != "")
                                {
                                    oSheet.Cells[intRow, 4] = Grand_Total_Subcontract_Value_F1;
                                }

                                oSheet.get_Range("C" + intRow.ToString(), "C" + intRow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
                                oSheet.get_Range("D" + intRow.ToString(), "D" + intRow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                                intRow = intRow + 2;
                                
                                int temp_prerow = intRow - 1;

                                oSheet.Cells[intRow - 1, 4] = "Works Certified";
                                oSheet.Cells[intRow - 1, 6] = "Retention Money";
                                oSheet.Cells[intRow - 1, 8] = "Deductions";
                                oSheet.Cells[intRow - 1, 10] = "Payment";

                                Excel.Border  tborder = oSheet.get_Range("D" + temp_prerow.ToString(), "D" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                tborder.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder1 = oSheet.get_Range("D" + temp_prerow.ToString(), "D" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                tborder1.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder2 = oSheet.get_Range("D" + temp_prerow.ToString(), "D" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeLeft];
                                tborder2.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder3 = oSheet.get_Range("E" + temp_prerow.ToString(), "E" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                tborder3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder4 = oSheet.get_Range("E" + temp_prerow.ToString(), "E" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                tborder4.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder5 = oSheet.get_Range("E" + temp_prerow.ToString(), "E" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                tborder5.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder6 = oSheet.get_Range("F" + temp_prerow.ToString(), "F" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                tborder6.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder7 = oSheet.get_Range("F" + temp_prerow.ToString(), "F" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                tborder7.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder8 = oSheet.get_Range("G" + temp_prerow.ToString(), "G" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                tborder8.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder9 = oSheet.get_Range("G" + temp_prerow.ToString(), "G" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                tborder9.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder10 = oSheet.get_Range("G" + temp_prerow.ToString(), "G" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                tborder10.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder11 = oSheet.get_Range("H" + temp_prerow.ToString(), "H" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                tborder11.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder12 = oSheet.get_Range("H" + temp_prerow.ToString(), "H" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                tborder12.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder13 = oSheet.get_Range("I" + temp_prerow.ToString(), "I" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                tborder13.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder14 = oSheet.get_Range("I" + temp_prerow.ToString(), "I" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                tborder14.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder15 = oSheet.get_Range("I" + temp_prerow.ToString(), "I" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                tborder15.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder16 = oSheet.get_Range("J" + temp_prerow.ToString(), "J" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                tborder16.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder17 = oSheet.get_Range("J" + temp_prerow.ToString(), "J" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                tborder17.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder18 = oSheet.get_Range("K" + temp_prerow.ToString(), "K" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeTop];
                                tborder18.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder19 = oSheet.get_Range("K" + temp_prerow.ToString(), "K" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                tborder19.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border tborder20 = oSheet.get_Range("K" + temp_prerow.ToString(), "K" + temp_prerow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                tborder20.LineStyle = Excel.XlLineStyle.xlContinuous;

                                oSheet.get_Range("D" + temp_prerow.ToString(), "E" + temp_prerow.ToString()).MergeCells = true;
                                oSheet.get_Range("D" + temp_prerow.ToString(), "E" + temp_prerow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                oSheet.get_Range("D" + temp_prerow.ToString(), "E" + temp_prerow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.SpringGreen);

                                oSheet.get_Range("F" + temp_prerow.ToString(), "G" + temp_prerow.ToString()).MergeCells = true;
                                oSheet.get_Range("F" + temp_prerow.ToString(), "G" + temp_prerow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                oSheet.get_Range("F" + temp_prerow.ToString(), "G" + temp_prerow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.SpringGreen);

                                oSheet.get_Range("H" + temp_prerow.ToString(), "I" + temp_prerow.ToString()).MergeCells = true;
                                oSheet.get_Range("H" + temp_prerow.ToString(), "I" + temp_prerow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                oSheet.get_Range("H" + temp_prerow.ToString(), "I" + temp_prerow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.SpringGreen);

                                oSheet.get_Range("J" + temp_prerow.ToString(), "K" + temp_prerow.ToString()).MergeCells = true;
                                oSheet.get_Range("J" + temp_prerow.ToString(), "K" + temp_prerow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                oSheet.get_Range("J" + temp_prerow.ToString(), "K" + temp_prerow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.SpringGreen);


                                oSheet.Cells[intRow, 4] = "Current";
                                oSheet.Cells[intRow, 5] = "Total";
                                oSheet.Cells[intRow, 6] = "Current";
                                oSheet.Cells[intRow, 7] = "Total";
                                oSheet.Cells[intRow, 8] = "Current";
                                oSheet.Cells[intRow, 9] = "Total";
                                oSheet.Cells[intRow, 10] = "Current";
                                oSheet.Cells[intRow, 11] = "Total";

                                string tempcell = "D" + intRow.ToString();
                                string tempcell1 = "E" + intRow.ToString();
                                string tempcell2 = "F" + intRow.ToString();
                                string tempcell3 = "G" + intRow.ToString();
                                string tempcell4 = "H" + intRow.ToString();
                                string tempcell5 = "I" + intRow.ToString();
                                string tempcell6 = "J" + intRow.ToString();
                                string tempcell7 = "K" + intRow.ToString();

                                Excel.Border border31 = oSheet.get_Range(tempcell, tempcell).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border31.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border border32 = oSheet.get_Range(tempcell, tempcell).Borders[Excel.XlBordersIndex.xlEdgeLeft];
                                border32.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border border33 = oSheet.get_Range(tempcell, tempcell).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                border33.LineStyle = Excel.XlLineStyle.xlContinuous;

                                Excel.Border border34 = oSheet.get_Range(tempcell1, tempcell1).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border34.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border border35 = oSheet.get_Range(tempcell1, tempcell1).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                border35.LineStyle = Excel.XlLineStyle.xlContinuous;

                                Excel.Border border36 = oSheet.get_Range(tempcell2, tempcell2).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border36.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border border37 = oSheet.get_Range(tempcell2, tempcell2).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                border37.LineStyle = Excel.XlLineStyle.xlContinuous;

                                Excel.Border border38 = oSheet.get_Range(tempcell3, tempcell3).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border38.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border border39 = oSheet.get_Range(tempcell3, tempcell3).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                border39.LineStyle = Excel.XlLineStyle.xlContinuous;


                                Excel.Border border40 = oSheet.get_Range(tempcell4, tempcell4).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border40.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border border41 = oSheet.get_Range(tempcell4, tempcell4).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                border41.LineStyle = Excel.XlLineStyle.xlContinuous;

                                Excel.Border border42 = oSheet.get_Range(tempcell5, tempcell5).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border42.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border border43 = oSheet.get_Range(tempcell5, tempcell5).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                border43.LineStyle = Excel.XlLineStyle.xlContinuous;


                                Excel.Border border44 = oSheet.get_Range(tempcell6, tempcell6).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border44.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border border45 = oSheet.get_Range(tempcell6, tempcell6).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                border45.LineStyle = Excel.XlLineStyle.xlContinuous;

                                Excel.Border border46 = oSheet.get_Range(tempcell7, tempcell7).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border46.LineStyle = Excel.XlLineStyle.xlContinuous;
                                Excel.Border border47 = oSheet.get_Range(tempcell7, tempcell7).Borders[Excel.XlBordersIndex.xlEdgeRight];
                                border47.LineStyle = Excel.XlLineStyle.xlContinuous;

                                oSheet.get_Range(tempcell, tempcell7).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.SpringGreen);

                                oSheet.get_Range(tempcell, tempcell).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                oSheet.get_Range(tempcell1, tempcell1).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                oSheet.get_Range(tempcell2, tempcell2).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                oSheet.get_Range(tempcell3, tempcell3).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                oSheet.get_Range(tempcell4, tempcell4).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                oSheet.get_Range(tempcell5, tempcell5).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                oSheet.get_Range(tempcell6, tempcell6).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                oSheet.get_Range(tempcell7, tempcell7).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                                intRow = intRow + 1;
                                if (Grand_Total_WorkCertified_Current != "")
                                {
                                    oSheet.Cells[intRow, 4] = Grand_Total_WorkCertified_Current;
                                }
                                if (Grand_Total_WorkCertified_Total != "")
                                {
                                    oSheet.Cells[intRow, 5] = Grand_Total_WorkCertified_Total;
                                }
                                if (Grand_Total_RententionMoney_Current != "")
                                {
                                    oSheet.Cells[intRow, 6] = Grand_Total_RententionMoney_Current;
                                }
                                if (Grand_Total_RententionMoney_Total != "")
                                {
                                    oSheet.Cells[intRow, 7] = Grand_Total_RententionMoney_Total;
                                }
                                if (Grand_Total_Deductions_Current != "")
                                {
                                    oSheet.Cells[intRow, 8] = Grand_Total_Deductions_Current;
                                }
                                if (Grand_Total_Deductions_Total != "")
                                {
                                    oSheet.Cells[intRow, 9] = Grand_Total_Deductions_Total;
                                }
                                if (Grand_Total_Payment_Current != "")
                                {
                                    oSheet.Cells[intRow, 10] = Grand_Total_Payment_Current;
                                }
                                if ( Grand_Total_Payment_Total != "")
                                {
                                    oSheet.Cells[intRow, 11] = Grand_Total_Payment_Total;
                                }
                            }
                            
                            TempStoreStr = "drop table #TR_Rpt";
                            SqlCommand command_d = new SqlCommand(TempStoreStr, connection);
                            command_d.Connection = connection;
                            command_d.CommandTimeout = 60000;
                            command_d.ExecuteNonQuery();

                            command_a.Dispose();
                            command_b.Dispose();
                            command_c.Dispose();
                            command_d.Dispose();
                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message.ToString());
                            connection.Close();
                        }
                        oRng = oSheet.get_Range("A1", "Z65536");
                        oRng.EntireColumn.AutoFit();

                        //oSheet.Columns.HorizontalAlignment = 1;
                        oSheet.Columns.VerticalAlignment = -4160;
                        oSheet.Columns.Orientation = 0;
                        oSheet.Columns.AddIndent = false;
                        oSheet.Columns.IndentLevel = 0;
                        oSheet.Columns.ShrinkToFit = false;
                        oSheet.Columns.ReadingOrder = -5002;
                        //oSheet.Columns.MergeCells = false;
                        //oSheet.Columns.Font.Name = "Arial";
                        //Michael oSheet.Columns.Font.FontStyle = "Normal";
                        //Michael oSheet.Columns.Font.Size = 10;
                        oSheet.Columns.Font.Strikethrough = false;
                        oSheet.Columns.Font.Superscript = false;
                        oSheet.Columns.Font.Subscript = false;
                        oSheet.Columns.Font.OutlineFont = false;
                        oSheet.Columns.Font.Shadow = false;
                        oSheet.Columns.Font.Underline = -4142;
                        //oSheet.Columns.Font.ColorIndex = -4105;

                        ((Excel.Range)oSheet.Columns["A", Missing]).ColumnWidth = 12;
                        ((Excel.Range)oSheet.Columns["B", Missing]).ColumnWidth = 15;

                        ((Excel.Range)oSheet.Columns["C", Missing]).ColumnWidth = 10;
                        ((Excel.Range)oSheet.Columns["E", Missing]).ColumnWidth = 14;
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

                    //File.AppendAllText((@"E:\MartinExcelDebug.txt", "before save Excel");
                    string fname = Server.MapPath("..\\ReportsFile\\AC02ExportFile" + number.ToString() + ".xls");
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
                SimpleWebUtils.DownloadFile(Page.Response, path + "/ReportsFile/AC02ExportFile" + number.ToString() + ".xls");
                //File.AppendAllText((@"E:\MartinExcelDebug.txt", "after download");
            }
        }
        
    }
}
