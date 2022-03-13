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
using Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;

using SimpleControls;
using SimpleControls.Web;

public partial class Control_PU07ExportExcel : System.Web.UI.Page
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

            //sqlstr = "select * ";
            sqlstr = "select PrjCode as [Project Code], prjname as [Project], cardname as [Supplier Name], [PA No.], [Description], dnnum as [DN No.],";
		    sqlstr = sqlstr + "Unit, Quantity, Rate as [Unit Rate], dnlinetotal as [Total], supinvnum as [Invoice No.], certlinetotal as [Invoice Amount],";
		    sqlstr = sqlstr + "NoOfPayment as [No. of Payment], certnum as [Cert No.], concharge as [Contra Charge], itemcode as [Item Code],";
            sqlstr = sqlstr + "costcode as [Cost Code], details as [Remarks]";
            sqlstr = sqlstr + "from PU07_Data(" + Request.QueryString["ProjectCode"] + "," + Request.QueryString["ProjectCode"] + ") order by cardname, 'PA No.'";

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
                    for (int i = 1; i < dt.Rows.Count + 1; i++)
                    {

                        for (int j = 1; j < dt.Columns.Count + 1; j++)
                        {

                            if (i == 1)
                            {
                                xlApp.Cells[i, j] = dcCollection[j - 1].ToString();
                                xlApp.Cells[i + 1, j] = dt.Rows[i - 1][j - 1].ToString();
                            }
                            else

                                xlApp.Cells[i+1, j] = dt.Rows[i - 1][j - 1].ToString();

                        }

                    }

                    wb.Saved = true;

                    Random nRandom = new Random(DateTime.Now.Millisecond);

                    string fname = Server.MapPath("..\\Temp\\t" + nRandom.Next().ToString() + ".xls");
                    if (System.IO.File.Exists(fname))
                    {
                        System.IO.File.Delete(fname);
                        wb.SaveCopyAs(fname);
                    }
                    else
                        wb.SaveCopyAs(fname);

                    wb.Close(null, null, null);
                    //xlApp.Workbooks.Close();
                    //xlApp.Quit();

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
                    //GC.WaitForPendingFinalizers();
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
