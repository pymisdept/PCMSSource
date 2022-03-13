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

            //sqlstr = "select prjcode as [Project Code], prjname as [Project Name], [MRNum] as [SMR No.], [SMR Line No.], MRDate as [SMR Date], ";
            //sqlstr = sqlstr + "ponum as [PO No.], CardName as [Supplier Name], [Description], Unit, Quantity, rate as [Unit Rate], polinetotal as [Total], ";
            //sqlstr = sqlstr + "supinvnum as [Invoice No.], certlinetotal as [Invoice Amount], noofpayment as [No. of Payment], certnum as [Payment Cert Number], ";
            //sqlstr = sqlstr + "costcode as [Cost Code], details as [Remarks] ";
            //sqlstr = sqlstr + "from PU06_Data(" + Request.QueryString["ProjectCode"] + "," + Request.QueryString["ProjectCode"] + ") order by mrnum, mrdate, 'smr line no.'";

            sqlstr = "select prjcode, prjname, [MRNum], [SMR Line No.], MRDate, ";
            sqlstr += "ponum, CardName, [Description], Unit, Quantity, rate, polinetotal, ";
            sqlstr += "supinvnum, certlinetotal, noofpayment, certnum, ";
            sqlstr += "costcode, details ";
            sqlstr += "from PU06_Data(" + Request.QueryString["ProjectCode"] + "," + Request.QueryString["ProjectCode"] + ") order by mrnum, mrdate, 'smr line no.'";

            //start //////
            string _connectionString = ConfigurationManager.ConnectionStrings["SAP2"].ConnectionString.ToString();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sqlstr, conn);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    da.Fill(dt);

                    //Random nRandom = new Random(DateTime.Now.Millisecond);
                    //string fname = Server.MapPath("..\\Temp\\t" + nRandom.Next().ToString() + ".csv");
                    string fname = "PU06.csv";
                    string output = "";
                    output += "Project Code, Project Name, SMR No., SMR Line No., SMR Date, PO No., Supplier Name, Description, Unit, Quantity, ";
                    output += "Unit Rate, Total, Invoice Number, Invoice Amount, No. of Payment, Payment Cert No. Item Code, Cost Code, Remarks" + "\n";

                    foreach (DataRow row in dt.Rows)
                    {
                        output += row["prjcode"].ToString() + ",";
                        output += (row["prjname"].ToString()).Replace(",", ";") + ",";
                        output += row["MRNum"].ToString() + ",";
                        output += row["SMR Line No."].ToString() + ",";
                        output += ((DateTime)row["MRDate"]).ToString("dd-MMM-yyyy") +",";
                        output += row["ponum"].ToString() + ",";
                        output += (row["CardName"].ToString()).Replace(",", ";") + ",";
                        output += (row["Description"].ToString()).Replace(",", ";") + ",";
                        output += row["Unit"].ToString() + ",";
                        output += row["Quantity"].ToString() + ",";
                        output += row["rate"].ToString() + ",";
                        output += row["polinetotal"].ToString() + ",";
                        output += (row["supinvnum"].ToString()).Replace(",", ";") + ",";
                        output += row["certlinetotal"].ToString() + ",";
                        output += row["noofpayment"].ToString() + ",";
                        output += row["certnum"].ToString() + ",";
                        output += row["costcode"].ToString() + ",";
                        output += (row["details"].ToString()).Replace(",", ";") + ",";
                        output += "\n";
                    }

                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + fname);
                    Response.ContentType = "text/csv";
                    //Response.ContentType = "application/octet-stream";
                    Response.Charset = Encoding.UTF8.WebName;
                    Response.ContentEncoding = Encoding.UTF8;
                    Response.BinaryWrite(Encoding.UTF8.GetPreamble());
                    Response.Write(output);
                    //
                    //UTF8Encoding utf8BOM = new UTF8Encoding(true); // 指定encoding实例encoderShouldEmitUTF8Identifier
                    //System.IO.StreamWriter sw = new System.IO.StreamWriter(Response.OutputStream, utf8BOM);
                    //sw.Write(output);
                    //sw.Close();

                    Response.End();

                    

                }
                catch (Exception ex)
                {
                    Error_Label.Text = ex.Message.ToString();
                    //conn.Close();
                }
                finally
                {
                    conn.Close();
                }
            }

            

            //end ///////



            //Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            //if (xlApp == null)
            //{
            //    Error_Label.Text = "EXCEL could not be started. Check that your office installation and project references are correct.";
            //    return;
            //}
            //xlApp.Visible = false;

            //Workbook wb = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            //Worksheet ws = (Worksheet)wb.Worksheets[1];

            //if (ws == null)
            //{
            //    Error_Label.Text = "Worksheet could not be created. Check that your office installation and project references are correct.";
            //}

            //string _connectionString = ConfigurationManager.ConnectionStrings["SAP2"].ConnectionString.ToString();
            //using (SqlConnection conn = new SqlConnection(_connectionString))
            //{
            //    try
            //    {
            //        conn.Open();
            //        SqlDataAdapter da = new SqlDataAdapter(sqlstr, conn);
            //        System.Data.DataTable dt = new System.Data.DataTable();
            //        da.Fill(dt);
            //        DataColumnCollection dcCollection = dt.Columns;
            //        for (int i = 1; i < dt.Rows.Count + 1; i++)
            //        {

            //            for (int j = 1; j < dt.Columns.Count + 1; j++)
            //            {

            //                if (i == 1)
            //                {
            //                    xlApp.Cells[i, j] = dcCollection[j - 1].ToString();
            //                    xlApp.Cells[i + 1, j] = dt.Rows[i - 1][j - 1].ToString();
            //                }
            //                else

            //                    xlApp.Cells[i + 1, j] = dt.Rows[i - 1][j - 1].ToString();

            //            }

            //        }

            //        wb.Saved = true;

            //        Random nRandom = new Random(DateTime.Now.Millisecond);

            //        string fname = Server.MapPath("..\\Temp\\t" + nRandom.Next().ToString() + ".xls");
            //        if (System.IO.File.Exists(fname))
            //        {
            //            System.IO.File.Delete(fname);
            //            wb.SaveCopyAs(fname);
            //        }
            //        else
            //            wb.SaveCopyAs(fname);

            //        wb.Close(null, null, null);

            //        FileInfo fileDet = new System.IO.FileInfo(fname);
            //        Response.Clear();
            //        Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileDet.Name));
            //        Response.AddHeader("Content-Length", fileDet.Length.ToString());
            //        Response.ContentType = "application/ms-excel";
            //        Response.WriteFile(fileDet.FullName);
            //        Response.End();

            //    }
            //    catch (Exception ex)
            //    {
            //        Error_Label.Text = ex.Message.ToString();
            //        conn.Close();
            //    }
            //    finally
            //    {
            //        xlApp.Workbooks.Close();
            //        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(wb);
            //        xlApp.Quit();
            //        System.Runtime.InteropServices.Marshal.FinalReleaseComObject(xlApp);
            //        ws = null;
            //        wb = null;
            //        xlApp = null;
            //        GC.Collect();
            //    }
            //}
        }
    }
}
