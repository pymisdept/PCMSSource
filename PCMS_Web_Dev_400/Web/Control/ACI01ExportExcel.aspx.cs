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

using System.Collections.Generic;
using System.ComponentModel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

using SimpleControls;
using SimpleControls.Web;

public partial class Control_ACI01ExportExcel : System.Web.UI.Page
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

                int intRow = 1;
                object Missing = System.Type.Missing;
                Excel._Workbook oWB = null;
                Excel._Worksheet oSheet = null;
                Excel.Range oRng = null;
                Excel.Application oXL = new Excel.Application();

                List<long> headerRowList = new List<long>();
                int NumberOfRecords = 0;
                int SubContractor_Row = 4;
                int Project_Row = 3;
                try
                {
                    oXL.Visible = false;
                    oXL.DisplayAlerts = false;
                    Boolean StartRow = true;
                    //Get a new workbook.
                    oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing));
                    oSheet = (Excel._Worksheet)oWB.ActiveSheet;
                    double Grant_Total_Of_InvAmt = 0;
                    double Grant_Total_Of_AmtDeduct = 0;
                    double Grant_Total_Of_DIFF = 0;
                    String previous_TempAC = "";
                    String TempStoreStr = "";

                    double Grant_Total_Of_Ledger = 0;
                    double Grant_Total_Of_Cert = 0;
                    double Grant_Total_Of_DIFF_LedgerCert = 0;

                    long Grant_Total_Of_Ledger_StartRow = 0;
                    
                    int counter = 0;

                    String temp5 = "";
                    Excel.Border border5 = oSheet.get_Range("A1", "A1").Borders[Excel.XlBordersIndex.xlEdgeBottom];

                    Dictionary<int, String> prjentry_list = new Dictionary<int, String>();

                    //File.AppendAllText((@"E:\MartinExcelDebug.txt", "SAP2");
                    string _connectionString = ConfigurationManager.ConnectionStrings["SAP2"].ConnectionString.ToString();
                    //File.AppendAllText((@"E:\MartinExcelDebug.txt", "after SAP2");
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        try
                        {
                            TempStoreStr = "CREATE TABLE #TR_Rpt (";
                            TempStoreStr = TempStoreStr + "SubsiName		nvarchar(max),";
                            TempStoreStr = TempStoreStr + "prjentry        int,";
	                        TempStoreStr = TempStoreStr + "ProjectCode		nvarchar(8),";
                            TempStoreStr = TempStoreStr + "Deduce_Indicator	nvarchar(1),";
	                        TempStoreStr = TempStoreStr + "SubCon				nvarchar(15),";
	                        TempStoreStr = TempStoreStr + "SubConName		nvarchar(100),";
                            TempStoreStr = TempStoreStr + "AcctCode        nvarchar(15), ";
	                        TempStoreStr = TempStoreStr + "PrjName					nvarchar(150),";
	                        TempStoreStr = TempStoreStr + "TempAC			nvarchar(24),";
	                        TempStoreStr = TempStoreStr + "VOCDate			datetime,";
	                        TempStoreStr = TempStoreStr + "Particular			nvarchar(254), ";
	                        TempStoreStr = TempStoreStr + "Nature				nvarchar(30),";
	                        TempStoreStr = TempStoreStr + "InvAmt				numeric(19,6),";
	                        TempStoreStr = TempStoreStr + "AmtDeduct		numeric(19,6),";
	                        TempStoreStr = TempStoreStr + "CertNo				nvarchar(100),";
	                        TempStoreStr = TempStoreStr + "VOCNo				nvarchar(255),";
	                        TempStoreStr = TempStoreStr + "docentry			int,";
                            TempStoreStr = TempStoreStr + "DRef                nvarchar(100),";
                            TempStoreStr = TempStoreStr + "AC_PERIOD		nvarchar(255))";

                            SqlCommand command_a = new SqlCommand(TempStoreStr, connection);
                            connection.Open();

                            command_a.Connection = connection;
                            command_a.CommandTimeout = 60000;
                            command_a.ExecuteNonQuery();

                            TempStoreStr = "INSERT INTO #TR_Rpt exec sp_AC01_Detail '" + Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "");
                            TempStoreStr = TempStoreStr + "', '" + Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "") + "'";

                            SqlCommand command_b = new SqlCommand(TempStoreStr, connection);
                            command_b.Connection = connection;
                            command_b.CommandTimeout = 60000;
                            command_b.ExecuteNonQuery();

                            TempStoreStr = "select count(*) as NumberOfRecords ";
                            TempStoreStr = TempStoreStr + " from ( ";
                            TempStoreStr = TempStoreStr + " select 1 as orderitem, TempAC, SubsiName, prjentry, ProjectCode, Deduce_Indicator, SubCon, SubConName, AcctCode, PrjName, VOCDate, Particular, Nature, InvAmt, AmtDeduct, CertNo, VOCNo, docentry, DRef, 0 as diffamt, AC_PERIOD";
                            TempStoreStr = TempStoreStr + " FROM #TR_Rpt group by TempAC, SubsiName, prjentry, ProjectCode, Deduce_Indicator, SubCon, SubConName, AcctCode, PrjName, VOCDate, Particular, Nature, InvAmt, AmtDeduct, CertNo, VOCNo, docentry, DRef, AC_PERIOD";
                            TempStoreStr = TempStoreStr + " union all ";
                            TempStoreStr = TempStoreStr + " select 2 as orderitem, TempAC, NULL as SubsiName, NULL as prjentry, NULL as ProjectCode, NULL as Deduce_Indicator, NULL as SubCon, NULL as SubConName, NULL as AcctCode, NULL as PrjName, NULL as VOCDate, NULL as Particular, NULL as Nature, Sum(InvAmt) as InvAmt, Sum(AmtDeduct) as AmtDeduct, NULL as CertNo, NULL as VOCNo, NULL as docentry, NULL as DRef, Sum(InvAmt+AmtDeduct) as diffamt, NULL as AC_PERIOD";
                            TempStoreStr = TempStoreStr + " FROM #TR_Rpt group by TempAC";
                            TempStoreStr = TempStoreStr + " ) as a ";

                            SqlCommand command_z = new SqlCommand(TempStoreStr, connection);
                            command_z.Connection = connection;
                            command_z.CommandTimeout = 60000;
                            using (SqlDataReader datareader = command_z.ExecuteReader())
                            {
                                while (datareader.Read())
                                {
                                    if (datareader.HasRows == true)
                                    {
                                        NumberOfRecords = Convert.ToInt32(datareader["NumberOfRecords"]);
                                    }
                                }
                            }
                            TempStoreStr = "select z.TempAC, z.SubsiName, z.prjentry, z.ProjectCode, z.Deduce_Indicator, z.SubCon, z.SubConName, z.AcctCode, z.PrjName, z.VOCDate, z.Particular, z.Nature, z.InvAmt, z.AmtDeduct, z.CertNo, z.VOCNo, z.docentry, z.DRef, z.diffamt, z.AC_PERIOD, z.orderitem";
                            TempStoreStr = TempStoreStr + " from ( ";
                            TempStoreStr = TempStoreStr + " select 1 as orderitem, TempAC, SubsiName, prjentry, ProjectCode, Deduce_Indicator, SubCon, SubConName, AcctCode, PrjName, VOCDate, Particular, Nature, InvAmt, AmtDeduct, CertNo, VOCNo, docentry, DRef, 0 as diffamt, AC_PERIOD";
                            TempStoreStr = TempStoreStr + " FROM #TR_Rpt group by TempAC, SubsiName, prjentry, ProjectCode, Deduce_Indicator, SubCon, SubConName, AcctCode, PrjName, VOCDate, Particular, Nature, InvAmt, AmtDeduct, CertNo, VOCNo, docentry, DRef, AC_PERIOD";
                            TempStoreStr = TempStoreStr + " union all ";
                            TempStoreStr = TempStoreStr + " select 2 as orderitem, TempAC, SubsiName, prjentry, ProjectCode, NULL as Deduce_Indicator, SubCon, SubConName, AcctCode, PrjName, NULL as VOCDate, NULL as Particular, NULL as Nature, Sum(InvAmt) as InvAmt, Sum(AmtDeduct) as AmtDeduct, NULL as CertNo, NULL as VOCNo, NULL as docentry, NULL as DRef, Sum(InvAmt+AmtDeduct) as diffamt, NULL as AC_PERIOD ";
                            TempStoreStr = TempStoreStr + " FROM #TR_Rpt group by TempAC,SubsiName, prjentry, ProjectCode,SubCon, SubConName, AcctCode, PrjName ";
                            TempStoreStr = TempStoreStr + " ) as z order by  z.SubConName, z.TempAC, z.orderitem, z.VOCDate";

                            SqlCommand command_c = new SqlCommand(TempStoreStr, connection);
                            command_c.Connection = connection;
                            command_c.CommandTimeout = 60000;
                            /*
                            SqlCommand command = new SqlCommand("sp_AC01_Detail", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Add(new SqlParameter("@FDocEntry", Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "")));
                            command.Parameters.Add(new SqlParameter("@TDocEntry", Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "")));

                            //File.AppendAllText((@"E:\MartinExcelDebug.txt", "before connection open");
                            connection.Open();
                            command.Connection = connection;
                            command.CommandTimeout = 12000;
                            */
                            //File.AppendAllText((@"E:\MartinExcelDebug.txt", "after connection open");
                            intRow = 1;
                            oSheet.Cells[intRow, 1] = "PAUL Y. GENERAL CONTRACTORS LIMITED";
                            oSheet.Cells[intRow, 5] = "Print Date and Time:" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                            intRow = intRow + 1;
                            oSheet.Cells[intRow, 1] = "CONTRA CHARGES - MATERIALS & EXPENSES PAID ON BEHALF";
                            //oSheet.Cells[SubContractor_Row, 1] = "SUBCONTRACTOR :";
                            oSheet.Cells[Project_Row, 1] = "SITE/PROJECT :";
                            intRow = Project_Row + 3;


                            //File.AppendAllText(@"E:\MartinExcelDebug.txt", "before command execute reader");
                            using (SqlDataReader datareader = command_c.ExecuteReader())
                            {
                                while (datareader.Read())
                                {
                                    if (datareader.HasRows == true)
                                    {
                                        //File.AppendAllText(@"E:\MartinExcelDebug.txt", "in loop");
                                        if (StartRow == true)
                                        {
                                            StartRow = false;

                                            if (datareader["orderitem"].ToString() == "2")
                                                intRow = intRow + 1;
                                            if (datareader["TempAC"] != System.DBNull.Value)
                                            {
                                                //oSheet.Cells[intRow - 1, 8] = "TEMP A/C : " + datareader["TempAC"].ToString();

                                                if (previous_TempAC != datareader["TempAC"].ToString() || String.IsNullOrEmpty(previous_TempAC))
                                                {
                                                    Grant_Total_Of_Ledger_StartRow = intRow+1;
                                                }

                                                previous_TempAC = datareader["TempAC"].ToString();
                                            }
                                            //intRow = intRow + 1;
                                            oSheet.Cells[intRow, 1] = "S/C";

                                            for (int i = 0; i < datareader.FieldCount; i++)
                                            {
                                                if (datareader.GetName(i) == "VOCDate")
                                                {
                                                    oSheet.Cells[intRow, 2] = "W/D DATE/VOC Date";
                                                }
                                                if (datareader.GetName(i) == "AC_PERIOD")
                                                {
                                                    oSheet.Cells[intRow, 3] = "ACCOUNT PERIOD";
                                                }
                                                if (datareader.GetName(i) == "Particular")
                                                {
                                                    oSheet.Cells[intRow, 4] = "PARTICULARS";
                                                }
                                                if (datareader.GetName(i) == "Nature")
                                                {
                                                    oSheet.Cells[intRow, 5] = "NATURE";
                                                }
                                                if (datareader.GetName(i) == "InvAmt")
                                                {
                                                    oSheet.Cells[intRow, 6] = "INV AMOUNT (a)";
                                                }
                                                if (datareader.GetName(i) == "AmtDeduct")
                                                {
                                                    oSheet.Cells[intRow, 7] = "AMT DEDUCTED BY QS* (b)";
                                                }
                                                //oSheet.Cells[intRow - 1, 1] = "SUBCONTRACTOR :";
                                                //if (datareader["SubConName"] != System.DBNull.Value)
                                                //{
                                                    //oSheet.Cells[intRow - 1, 2] = "'" + datareader["SubConName"].ToString();
                                                    //oSheet.get_Range("B" + (intRow - 1).ToString(), "B" + (intRow - 1).ToString()).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                                //}

                                                oSheet.Cells[intRow, 8] = "DIFF (a)+(b)";
                                                oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                                                if (datareader.GetName(i) == "CertNo")
                                                {
                                                    oSheet.Cells[intRow, 9] = "CERT NO./PCMS No.";
                                                }
                                                
                                                
                                                if (datareader.GetName(i) == "VOCNo")
                                                {
                                                    oSheet.Cells[intRow, 10] = "VOC #";
                                                }
                                            }

                                            oSheet.get_Range("A" + intRow.ToString(), "K" + intRow.ToString()).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);

                                            oSheet.Cells[intRow, 11] = "Temp A/C Code";
                                            Excel.Border border = oSheet.get_Range("A" + intRow.ToString(), "K" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                            border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                            headerRowList.Add(intRow);
                                            
                                            intRow = intRow + 1;
                                        }
                                        // Data
                                        if (datareader["SubConName"] != System.DBNull.Value)
                                        {
                                            oSheet.Cells[intRow, 1] = "'" + datareader["SubConName"].ToString();

                                            if (intRow == 7)
                                                oSheet.get_Range("A" + intRow.ToString(), "A" + intRow.ToString()).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                        }
                                        else
                                        {
                                            if( datareader["TempAC"].ToString().IndexOf("CC") < 0)
                                                oSheet.Cells[intRow, 1] = "'" + datareader["TempAC"].ToString().Substring(0, datareader["TempAC"].ToString().Length);
                                            else
                                                oSheet.Cells[intRow, 1] = "'" + datareader["TempAC"].ToString().Substring(datareader["TempAC"].ToString().IndexOf("CC"), datareader["TempAC"].ToString().Length- datareader["TempAC"].ToString().IndexOf("CC"));
                                            if (intRow == 7)
                                                oSheet.get_Range("A" + intRow.ToString(), "A" + intRow.ToString()).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                        }
                                        if (datareader["TempAC"] != System.DBNull.Value && previous_TempAC != datareader["TempAC"].ToString())
                                        {
                                            if (previous_TempAC != datareader["TempAC"].ToString() || String.IsNullOrEmpty(previous_TempAC))
                                            {
                                                Grant_Total_Of_Ledger_StartRow = intRow;

                                                oSheet.get_Range("A" + intRow.ToString(), "A" + intRow.ToString()).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            }
                                            previous_TempAC = datareader["TempAC"].ToString();
                                        }
                                        if (datareader["TempAC"] != System.DBNull.Value)
                                        {
                                            oSheet.Cells[intRow, 11] = "TEMP A/C : " + datareader["TempAC"].ToString();
                                        }
                                        if (datareader["prjentry"] != System.DBNull.Value)
                                            if (prjentry_list.ContainsValue(datareader["prjentry"].ToString()) == false)
                                                prjentry_list.Add(prjentry_list.Count, datareader["prjentry"].ToString());

                                        
                                        if (datareader["VOCDate"] != System.DBNull.Value)
                                        {
                                            //oSheet.Cells[intRow, 1] = Convert.ToDateTime(datareader["VOCDate"]);
                                            oRng = oSheet.get_Range("B" + intRow.ToString(), "B" + intRow.ToString());
                                            oRng.set_Value(Missing, datareader["VOCDate"]);
                                            oSheet.get_Range("B" + intRow.ToString(), "B" + intRow.ToString()).NumberFormat = "dd/MM/yyyy";

                                            oSheet.get_Range("B" + intRow.ToString(), "B" + intRow.ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                        }
                                        if (datareader["AC_PERIOD"] != System.DBNull.Value)
                                        {
                                            oSheet.Cells[intRow, 3] = "'" + Convert.ToDateTime(datareader["AC_PERIOD"]).ToString("MMM") + "/" + Convert.ToDateTime(datareader["AC_PERIOD"]).Year.ToString();
                                            oSheet.get_Range("C" + intRow.ToString(), "C" + intRow.ToString()).Font.Bold = true;
                                            oSheet.get_Range("C" + intRow.ToString(), "C" + intRow.ToString()).Font.Name = "Courier New";
                                            //if ("'May/2012" == "'" + Convert.ToDateTime(datareader["AC_PERIOD"]).ToString("MMM") + "/" + Convert.ToDateTime(datareader["AC_PERIOD"]).Year.ToString())
                                            //{
                                                //Console.WriteLine("");
                                            //}
                                        }
                                        if (datareader["Particular"] != System.DBNull.Value)
                                            oSheet.Cells[intRow, 4] = "'" + datareader["Particular"].ToString();
                                        if (datareader["Nature"] != System.DBNull.Value)
                                            oSheet.Cells[intRow, 5] = "'" + datareader["Nature"].ToString();
                                        if (datareader["InvAmt"] != System.DBNull.Value)
                                        {
                                            oSheet.Cells[intRow, 6] = datareader["InvAmt"].ToString();

                                            if (datareader["orderitem"].ToString() != "2")
                                                Grant_Total_Of_InvAmt = Grant_Total_Of_InvAmt + Convert.ToDouble(datareader["InvAmt"].ToString());

                                            oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                                        }
                                        if (datareader["AmtDeduct"] != System.DBNull.Value)
                                        {
                                            oSheet.Cells[intRow, 7] = datareader["AmtDeduct"].ToString();
                                            if (datareader["orderitem"].ToString() != "2")
                                                Grant_Total_Of_AmtDeduct = Grant_Total_Of_AmtDeduct + Convert.ToDouble(datareader["AmtDeduct"].ToString());

                                            oSheet.get_Range("G" + intRow.ToString(), "G" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                                        }
                                        if (datareader["InvAmt"] != System.DBNull.Value && datareader["AmtDeduct"] != System.DBNull.Value)
                                        {
                                            oSheet.Cells[intRow, 8] = (Convert.ToDouble(datareader["InvAmt"]) + Convert.ToDouble(datareader["AmtDeduct"])).ToString();
                                            oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                            if (datareader["orderitem"].ToString() != "2")
                                                Grant_Total_Of_DIFF = Grant_Total_Of_DIFF + (Convert.ToDouble(datareader["InvAmt"]) + Convert.ToDouble(datareader["AmtDeduct"]));
                                        }
                                        if (datareader["CertNo"] != System.DBNull.Value)
                                            oSheet.Cells[intRow, 9] = "'" + datareader["CertNo"].ToString();
                                        
                                        if (datareader["VOCNo"] != System.DBNull.Value)
                                            oSheet.Cells[intRow, 10] = "'" + datareader["VOCNo"].ToString();

                                        //if (datareader["SubCon"] != System.DBNull.Value)
                                            //oSheet.Cells[SubContractor_Row, 2] = "'" + datareader["SubCon"].ToString();
                                        if (datareader["PrjName"] != System.DBNull.Value)
                                            oSheet.Cells[Project_Row, 3] = "'" + datareader["ProjectCode"].ToString() + " " + datareader["PrjName"].ToString();
                   
                                        intRow = intRow + 1;
                                        counter = counter + 1;

                                        if (datareader["orderitem"].ToString() == "2")
                                        {
                                            intRow = intRow - 1;
                                            oSheet.Cells[intRow, 5] = "Grand Total";

                                            oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString()).Formula = "=SUBTOTAL(9,F" + Grant_Total_Of_Ledger_StartRow.ToString() + ":F" + (intRow-1).ToString() + ")";
                                            oSheet.get_Range("G" + intRow.ToString(), "G" + intRow.ToString()).Formula = "=SUBTOTAL(9,G" + Grant_Total_Of_Ledger_StartRow.ToString() + ":G" + (intRow - 1).ToString() + ")";
                                            oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).Formula = "=SUBTOTAL(9,H" + Grant_Total_Of_Ledger_StartRow.ToString() + ":H" + (intRow - 1).ToString() + ")";

                                            Excel.Border border20 = oSheet.get_Range( "F" + intRow.ToString(), "H" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                            border20.LineStyle = Excel.XlLineStyle.xlContinuous;

                                            //border20 = oSheet.get_Range("I" + intRow.ToString(), "I" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                            //border20.LineStyle = Excel.XlLineStyle.xlContinuous;
                                            
                                            border20 = oSheet.get_Range("F" + (intRow - 1).ToString(), "H" + (intRow - 1).ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                            border20.LineStyle = Excel.XlLineStyle.xlContinuous;
                                            //previous_TempAC = "";
                                            intRow = intRow + 2;

                                            StartRow = true;
                                            foreach (int key in prjentry_list.Keys)
                                            {
                                                string entry = "";
                                                prjentry_list.TryGetValue(key, out entry);
                                                //SqlCommand command2 = new SqlCommand("select * from AC01_NatureSummary(" + entry + "," + entry + ") where TempAC ='" + previous_TempAC+"'", connection);

                                                TempStoreStr = "CREATE TABLE #TR_Rpt2 (";
                                                TempStoreStr = TempStoreStr + "PrjCode		nvarchar(8),";
                                                TempStoreStr = TempStoreStr + "prjentry    int,";
                                                TempStoreStr = TempStoreStr + "SubCon  		nvarchar(15),";
                                                TempStoreStr = TempStoreStr + "TempAC			nvarchar(24),";
                                                TempStoreStr = TempStoreStr + "Nature				nvarchar(100),";
                                                TempStoreStr = TempStoreStr + "NatureDesc		nvarchar(100),";
                                                TempStoreStr = TempStoreStr + "Ledger				numeric(19,6),";
                                                TempStoreStr = TempStoreStr + "Cert			numeric(19,6))";

                                                SqlCommand command_e = new SqlCommand(TempStoreStr, connection);
                                                //connection.Open();

                                                command_e.Connection = connection;
                                                command_e.CommandTimeout = 60000;
                                                command_e.ExecuteNonQuery();

                                                TempStoreStr = "INSERT INTO #TR_Rpt2 exec sp_AC01_NatureSummary '" + entry;
                                                TempStoreStr = TempStoreStr + "', '" + entry + "'";

                                                SqlCommand command_f = new SqlCommand(TempStoreStr, connection);
                                                command_f.Connection = connection;
                                                command_f.CommandTimeout = 60000;
                                                command_f.ExecuteNonQuery();

                                                TempStoreStr = "select z.PrjCode,z.prjentry,z.SubCon,z.TempAC,z.Nature,z.NatureDesc,z.Ledger,z.Cert,z.diffamt, z.orderitem";
                                                TempStoreStr = TempStoreStr + " from ( ";
                                                TempStoreStr = TempStoreStr + "select PrjCode,prjentry,SubCon,TempAC,Nature,NatureDesc,Ledger,Cert,(Ledger + Cert) as diffamt, 1 as orderitem";
                                                TempStoreStr = TempStoreStr + " FROM #TR_Rpt2 where TempAC = '" + previous_TempAC + "' ";
                                                TempStoreStr = TempStoreStr + " union all ";
                                                TempStoreStr = TempStoreStr + "select NULL as PrjCode,NULL as prjentry,NULL as SubCon,NULL as TempAC,NULL as Nature,NULL as NatureDesc,sum(Ledger),sum(Cert), sum(Ledger + Cert) as diffamt, 2 as orderitem";
                                                TempStoreStr = TempStoreStr + " FROM #TR_Rpt2 where TempAC = '" + previous_TempAC + "' ";
                                                TempStoreStr = TempStoreStr + " ) as z order by z.orderitem ";

                                                SqlCommand command_g = new SqlCommand(TempStoreStr, connection);
                                                command_g.Connection = connection;
                                                command_g.CommandTimeout = 60000;

                                                using (SqlDataReader datareader2 = command_g.ExecuteReader())
                                                {
                                                    while (datareader2.Read())
                                                    {
                                                        if (datareader2.HasRows == true)
                                                        {
                                                            if (StartRow == true)
                                                            {
                                                                StartRow = false;
                                                                /*
                                                                for (int i = 0; i < datareader2.FieldCount; i++)
                                                                {
                                                                    if (datareader2.GetName(i) == "Nature")
                                                                    {
                                                                        oSheet.Cells[intRow, 4] = "Nature";
                                                                    }
                                                                    if (datareader2.GetName(i) == "NatureDesc")
                                                                    {
                                                                        oSheet.Cells[intRow, 5] = "Description";
                                                                    }
                                                                    if (datareader2.GetName(i) == "Ledger")
                                                                    {
                                                                        oSheet.Cells[intRow, 6] = "Ledger";
                                                                    }
                                                                    if (datareader2.GetName(i) == "Cert")
                                                                    {
                                                                        oSheet.Cells[intRow, 7] = "Cert";
                                                                    }
                                                                    if (datareader2.GetName(i) == "diffamt")
                                                                    {
                                                                        oSheet.Cells[intRow, 8] = "Diff";
                                                                    }
                                                                    Excel.Border border2 = oSheet.get_Range("F" + intRow.ToString(), "H" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                                    border2.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                                }
                                                                intRow = intRow + 1;
                                                                */
                                                            }
                                                            if (datareader2["Nature"] != System.DBNull.Value)
                                                                oSheet.Cells[intRow, 5] = datareader2["Nature"].ToString();
                                                            //if (datareader2["NatureDesc"] != System.DBNull.Value)
                                                                //oSheet.Cells[intRow, 5] = datareader2["NatureDesc"].ToString();
                                                            if (datareader2["Ledger"] != System.DBNull.Value)
                                                            {
                                                                oSheet.Cells[intRow, 6] = datareader2["Ledger"].ToString();
                                                                //Grant_Total_Of_Ledger = Grant_Total_Of_Ledger + Convert.ToDouble(datareader2["Ledger"].ToString());

                                                                oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                                                            }
                                                            if (datareader2["Cert"] != System.DBNull.Value)
                                                            {
                                                                oSheet.Cells[intRow, 7] = datareader2["Cert"].ToString();
                                                                //Grant_Total_Of_Cert = Grant_Total_Of_Cert + Convert.ToDouble(datareader2["Cert"].ToString());

                                                                oSheet.get_Range("G" + intRow.ToString(), "G" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                                                            }
                                                            if (datareader2["diffamt"] != System.DBNull.Value)
                                                            {
                                                                oSheet.Cells[intRow, 8] = Convert.ToDouble(datareader2["diffamt"]);
                                                                //Grant_Total_Of_DIFF_LedgerCert = Grant_Total_Of_DIFF_LedgerCert + (Convert.ToDouble(datareader2["Ledger"]) + Convert.ToDouble(datareader2["Cert"]));

                                                                oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                                                            }
                                                            if (datareader2["orderitem"].ToString() == "2")
                                                            {
                                                                oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                                                                oSheet.get_Range("G" + intRow.ToString(), "G" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                                                                oSheet.get_Range("H" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                                                                Excel.Border border3 = oSheet.get_Range("F" + intRow.ToString(), "H" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                                border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                                border3 = oSheet.get_Range("F" + (intRow - 1).ToString(), "H" + (intRow - 1).ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                                border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                            }
                                                            oSheet.get_Range("E" + intRow.ToString(), "H" + intRow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightYellow);
                                                            intRow = intRow + 1;
                                                        }
                                                    }
                                                }
                                                TempStoreStr = "drop table #TR_Rpt2";
                                                SqlCommand command_i = new SqlCommand(TempStoreStr, connection);
                                                command_i.Connection = connection;
                                                command_i.CommandTimeout = 60000;
                                                command_i.ExecuteNonQuery();
                                            }
                                            prjentry_list.Clear();
                                            //intRow = intRow + 1;

                                            StartRow = false;
                                            /*
                                            if (counter < NumberOfRecords)
                                            {
                                                if (datareader["orderitem"].ToString() == "2")
                                                {
                                                    intRow = intRow + 1;
                                                    
                                                }
                                                
                                                //intRow = intRow + 1;
                                                for (int i = 0; i < datareader.FieldCount; i++)
                                                {
                                                    if (datareader.GetName(i) == "VOCDate")
                                                    {
                                                        oSheet.Cells[intRow, 1] = "W/D DATE/VOC Date";
                                                    }
                                                    if (datareader.GetName(i) == "AC_PERIOD")
                                                    {
                                                        oSheet.Cells[intRow, 2] = "ACCOUNT PERIOD";
                                                    }
                                                    if (datareader.GetName(i) == "Particular")
                                                    {
                                                        oSheet.Cells[intRow, 3] = "PARTICULARS";
                                                    }
                                                    if (datareader.GetName(i) == "Nature")
                                                    {
                                                        oSheet.Cells[intRow, 4] = "NATURE";
                                                    }
                                                    if (datareader.GetName(i) == "InvAmt")
                                                    {
                                                        oSheet.Cells[intRow, 5] = "INV AMOUNT (a)";
                                                    }
                                                    if (datareader.GetName(i) == "AmtDeduct")
                                                    {
                                                        oSheet.Cells[intRow, 6] = "AMT DEDUCTED BY QS* (b)";
                                                    }
                                                    if (datareader.GetName(i) == "CertNo")
                                                    {
                                                        oSheet.Cells[intRow, 7] = "CERT NO./PCMS No.";
                                                    }
                                                    oSheet.Cells[intRow-1, 1] = "SUBCONTRACTOR :";
                                                    if (datareader["SubConName"] != System.DBNull.Value)
                                                        oSheet.Cells[intRow - 1, 2] = "'" + datareader["SubConName"].ToString();

                                                    oSheet.Cells[intRow, 8] = "DIFF (a)+(b)";
                                                   
                                                    if (datareader.GetName(i) == "VOCNo")
                                                    {
                                                        oSheet.Cells[intRow, 9] = "VOC #";
                                                    }
                                                   
                                                }
                                                Excel.Border border = oSheet.get_Range("A" + intRow.ToString(), "I" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                            }*/
                                            //headerRowList.Add(intRow);
                                            intRow = intRow + 1;
                                        }
                                        StartRow = false;
                                    }
                                }
                                /*
                                oSheet.Cells[intRow, 4] = "Grand Total";
                                //oSheet.Cells[intRow, 5] = Grant_Total_Of_InvAmt.ToString();
                                //oSheet.Cells[intRow, 6] = Grant_Total_Of_AmtDeduct.ToString();
                                //oSheet.Cells[intRow, 8] = Grant_Total_Of_DIFF.ToString();

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

                                
                                temp3 = "E" + (intRow - 1).ToString();
                                border3 = oSheet.get_Range(temp3, temp3).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                temp3 = "F" + (intRow - 1).ToString();
                                border3 = oSheet.get_Range(temp3, temp3).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                temp3 = "H" + (intRow - 1).ToString();
                                border3 = oSheet.get_Range(temp3, temp3).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                border3.LineStyle = Excel.XlLineStyle.xlContinuous;
                                
                                */
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

                            StartRow = false;
                            foreach (long headerRow in headerRowList)
                            {
                                if (StartRow == false) // run the first only
                                {
                                    oSheet.get_Range("A" + headerRow.ToString(), "K65535").AutoFilter(headerRow, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                                    StartRow = true;
                                }
                            }

                            StartRow = true;
                            
                            intRow = intRow + 1;
                            //Grant_Total_Of_Ledger = 0;
                            //Grant_Total_Of_Cert = 0;
                            //Grant_Total_Of_DIFF_LedgerCert = 0;

                            foreach (int key in prjentry_list.Keys)
                            {
                                string entry = "";
                                prjentry_list.TryGetValue(key, out entry);
                                //SqlCommand command2 = new SqlCommand("select * from AC01_NatureSummary(" + entry + "," + entry + ") where TempAC ='" + previous_TempAC+"'", connection);

                                TempStoreStr = "CREATE TABLE #TR_Rpt2 (";
                                TempStoreStr = TempStoreStr + "PrjCode		nvarchar(8),";
                                TempStoreStr = TempStoreStr + "prjentry    int,";
                                TempStoreStr = TempStoreStr + "SubCon  		nvarchar(15),";
                                TempStoreStr = TempStoreStr + "TempAC			nvarchar(24),";
                                TempStoreStr = TempStoreStr + "Nature				nvarchar(100),";
                                TempStoreStr = TempStoreStr + "NatureDesc		nvarchar(100),";
                                TempStoreStr = TempStoreStr + "Ledger				numeric(19,6),";
                                TempStoreStr = TempStoreStr + "Cert			numeric(19,6))";


                                SqlCommand command_e = new SqlCommand(TempStoreStr, connection);
                                //connection.Open();

                                command_e.Connection = connection;
                                command_e.CommandTimeout = 60000;
                                command_e.ExecuteNonQuery();

                                TempStoreStr = "INSERT INTO #TR_Rpt2 exec sp_AC01_NatureSummary '" + entry;
                                TempStoreStr = TempStoreStr + "', '" + entry + "'";

                                SqlCommand command_f = new SqlCommand(TempStoreStr, connection);
                                command_f.Connection = connection;
                                command_f.CommandTimeout = 60000;
                                command_f.ExecuteNonQuery();

                                TempStoreStr = "select z.PrjCode,z.prjentry,z.SubCon,z.TempAC,z.Nature,z.NatureDesc,z.Ledger,z.Cert,z.diffamt, z.orderitem";
                                TempStoreStr = TempStoreStr + " from ( ";
                                TempStoreStr = TempStoreStr + "select PrjCode,prjentry,SubCon,TempAC,Nature,NatureDesc,Ledger,Cert,(Ledger + Cert) as diffamt, 1 as orderitem";
                                TempStoreStr = TempStoreStr + " FROM #TR_Rpt2 where TempAC = '" + previous_TempAC + "' ";
                                TempStoreStr = TempStoreStr + " union all ";
                                TempStoreStr = TempStoreStr + "select NULL as PrjCode,NULL as prjentry,NULL as SubCon,NULL as TempAC,NULL as Nature,NULL as NatureDesc,sum(Ledger),sum(Cert), sum(Ledger + Cert) as diffamt, 2 as orderitem";
                                TempStoreStr = TempStoreStr + " FROM #TR_Rpt2 where TempAC = '" + previous_TempAC + "' ";
                                TempStoreStr = TempStoreStr + " ) as z order by z.orderitem ";

                                SqlCommand command_g = new SqlCommand(TempStoreStr, connection);
                                command_g.Connection = connection;
                                command_g.CommandTimeout = 60000;

                                //SqlCommand command2 = new SqlCommand("AC01_NatureSummary", connection);
                                //command2.CommandType = CommandType.StoredProcedure;

                                //command2.Parameters.Add(new SqlParameter("@FDocEntry", Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "")));
                                //command2.Parameters.Add(new SqlParameter("@TDocEntry", Request.QueryString["ProjectCode"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", "")));
                                
                                //command2.Connection = connection;
                                //command2.CommandTimeout = 60000;

                                using (SqlDataReader datareader2 = command_g.ExecuteReader())
                                {
                                    while (datareader2.Read())
                                    {
                                        if (datareader2.HasRows == true)
                                        {
                                            if (StartRow == true)
                                            {
                                                StartRow = false;
                                                /*
                                                for (int i = 0; i < datareader2.FieldCount; i++)
                                                {
                                                    if (datareader2.GetName(i) == "Nature")
                                                    {
                                                        oSheet.Cells[intRow, 3] = "Nature";
                                                    }
                                                    if (datareader2.GetName(i) == "NatureDesc")
                                                    {
                                                        oSheet.Cells[intRow, 4] = "Description";
                                                    }
                                                    if (datareader2.GetName(i) == "Ledger")
                                                    {
                                                        oSheet.Cells[intRow, 5] = "Ledger";
                                                    }
                                                    if (datareader2.GetName(i) == "Cert")
                                                    {
                                                        oSheet.Cells[intRow, 6] = "Cert";
                                                    }
                                                    oSheet.Cells[intRow, 7] = "Diff";
                                                }
                                                Excel.Border border2 = oSheet.get_Range("D" + intRow.ToString(), "H" + intRow.ToString()).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                                                border2.LineStyle = Excel.XlLineStyle.xlContinuous;
                                                intRow = intRow + 1;*/
                                            }
                                            if (datareader2["Nature"] != System.DBNull.Value)
                                                oSheet.Cells[intRow, 3] = datareader2["Nature"].ToString();
                                            if (datareader2["NatureDesc"] != System.DBNull.Value)
                                                oSheet.Cells[intRow, 4] = datareader2["NatureDesc"].ToString();
                                            if (datareader2["Ledger"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[intRow, 5] = datareader2["Ledger"].ToString();
                                                Grant_Total_Of_Ledger = Grant_Total_Of_Ledger + Convert.ToDouble(datareader2["Ledger"].ToString());

                                                oSheet.get_Range("E" + intRow.ToString(), "E" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                                            }
                                            if (datareader2["Cert"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[intRow, 6] = datareader2["Cert"].ToString();
                                                Grant_Total_Of_Cert = Grant_Total_Of_Cert + Convert.ToDouble(datareader2["Cert"].ToString());

                                                oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                                            }
                                            if (datareader2["Ledger"] != System.DBNull.Value && datareader2["Cert"] != System.DBNull.Value)
                                            {
                                                oSheet.Cells[intRow, 7] = (Convert.ToDouble(datareader2["Ledger"]) + Convert.ToDouble(datareader2["Cert"])).ToString();
                                                Grant_Total_Of_DIFF_LedgerCert = Grant_Total_Of_DIFF_LedgerCert + (Convert.ToDouble(datareader2["Ledger"]) + Convert.ToDouble(datareader2["Cert"]));

                                                oSheet.get_Range("G" + intRow.ToString(), "G" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                                            }

                                            oSheet.get_Range("E" + intRow.ToString(), "G" + intRow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightYellow);
                                            intRow = intRow + 1;
                                        }
                                    }
                                }
                                TempStoreStr = "drop table #TR_Rpt2";
                                SqlCommand command_i = new SqlCommand(TempStoreStr, connection);
                                command_i.Connection = connection;
                                command_i.CommandTimeout = 60000;
                                command_i.ExecuteNonQuery();

                                command_e.Dispose();
                                command_f.Dispose();
                                command_g.Dispose();
                                command_i.Dispose();
                            }
                            /*
                            oSheet.Cells[intRow, 5] = Grant_Total_Of_Ledger.ToString();
                            oSheet.Cells[intRow, 6] = Grant_Total_Of_Cert.ToString();
                            oSheet.Cells[intRow, 7] = Grant_Total_Of_DIFF_LedgerCert.ToString();

                            oSheet.get_Range("E" + intRow.ToString(), "E" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                            oSheet.get_Range("F" + intRow.ToString(), "F" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";
                            oSheet.get_Range("G" + intRow.ToString(), "G" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                            temp5 = "E" + intRow.ToString();
                            border5 = oSheet.get_Range(temp5, temp5).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                            border5.LineStyle = Excel.XlLineStyle.xlContinuous;
                            temp5 = "F" + intRow.ToString();
                            border5 = oSheet.get_Range(temp5, temp5).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                            border5.LineStyle = Excel.XlLineStyle.xlContinuous;
                            temp5 = "G" + intRow.ToString();
                            border5 = oSheet.get_Range(temp5, temp5).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                            border5.LineStyle = Excel.XlLineStyle.xlContinuous;

                            temp5 = "E" + (intRow - 1).ToString();
                            border5 = oSheet.get_Range(temp5, temp5).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                            border5.LineStyle = Excel.XlLineStyle.xlContinuous;
                            temp5 = "F" + (intRow - 1).ToString();
                            border5 = oSheet.get_Range(temp5, temp5).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                            border5.LineStyle = Excel.XlLineStyle.xlContinuous;
                            temp5 = "G" + (intRow - 1).ToString();
                            border5 = oSheet.get_Range(temp5, temp5).Borders[Excel.XlBordersIndex.xlEdgeBottom];
                            border5.LineStyle = Excel.XlLineStyle.xlContinuous;
                            */
                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                            //File.AppendAllText(@"E:\MartinExcelDebug.txt", ex.Message.ToString());
                            Console.WriteLine(ex.Message.ToString());
                            connection.Close();
                        }
                        /*
                        oRng = oSheet.get_Range("A1", "Z65536");
                        oRng.EntireColumn.AutoFit();

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
                        //oSheet.Columns.Font.ColorIndex = -4105;
                        */

                        oSheet.Cells[intRow, 5] = "Report Total";
                        oSheet.Cells[intRow, 6] = Grant_Total_Of_InvAmt.ToString();
                        oSheet.Cells[intRow, 7] = Grant_Total_Of_AmtDeduct.ToString();
                        oSheet.Cells[intRow, 8] = Grant_Total_Of_DIFF.ToString();

                        oSheet.get_Range("E" + intRow.ToString(), "H" + intRow.ToString()).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                        oSheet.get_Range("F" + intRow.ToString(), "H" + intRow.ToString()).NumberFormat = "#,##0.00_);(#,##0.00)";

                        ((Excel.Range)oSheet.Columns["A", Missing]).ColumnWidth = 18;
                        ((Excel.Range)oSheet.Columns["B", Missing]).ColumnWidth = 10.6;
                        ((Excel.Range)oSheet.Columns["C", Missing]).ColumnWidth = 11.3;
                        ((Excel.Range)oSheet.Columns["D", Missing]).ColumnWidth = 40;
                        ((Excel.Range)oSheet.Columns["E", Missing]).ColumnWidth = 10.5;
                        ((Excel.Range)oSheet.Columns["F", Missing]).ColumnWidth = 17;
                        ((Excel.Range)oSheet.Columns["G", Missing]).ColumnWidth = 17;
                        ((Excel.Range)oSheet.Columns["H", Missing]).ColumnWidth = 17;
                        ((Excel.Range)oSheet.Columns["I", Missing]).ColumnWidth = 18.5;
                        ((Excel.Range)oSheet.Columns["J", Missing]).ColumnWidth = 11.2;
                        ((Excel.Range)oSheet.Columns["K", Missing]).ColumnWidth = 30;

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
                    string fname = Server.MapPath("..\\ReportsFile\\AC01ExportFile" + number.ToString() + ".xls");
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
                SimpleWebUtils.DownloadFile(Page.Response, path + "/ReportsFile/AC01ExportFile" + number.ToString() + ".xls");
                //File.AppendAllText((@"E:\MartinExcelDebug.txt", "after download");
            }
        }
    }
}
