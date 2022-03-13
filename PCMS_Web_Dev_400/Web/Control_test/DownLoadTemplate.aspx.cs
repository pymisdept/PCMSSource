using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using PCCore;
using SimpleControls.Web;
using SimpleControls.SimpleDatabase;
using System.Data.SqlClient;
using System.Data.Common;
using SimpleControls;

public partial class DownLoadTemplate : BasePage
{

    protected override void OnInit(EventArgs e)
    {
        ShowWebMenu = false;
        base.OnInit(e);
    }
    string DlnID;
    Int32 byeOfLength = 0;
    //string fileName = "";
    string ProjCode = "";
    string FunctionCode = "";

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            if (Page.Request.QueryString != null && Page.Request.QueryString.Count > 0)
            {
                DlnID = Page.Request.QueryString["DlnID"] as string;
                ProjCode = Page.Request.QueryString["ProjCode"] as string;
                FunctionCode = Page.Request.QueryString["FunctionCode"] as string;

                //fileName = Page.Request.QueryString["filename"] as string;

                if (!String.IsNullOrEmpty(DlnID))
                {
                    // Update Download Manager Status to Close
                    PCCore.Common.HRLog.RecordLog("Close Status");
                    PCCore.PCMS.DlnRequest _dlnReq = new PCCore.PCMS.DlnRequest();
                    try
                    {
                        _dlnReq.Close(DlnID);
                    }
                    catch (Exception ex)
                    {
                        PCCore.Common.HRLog.RecordException("DownloadTemplate: Close Status", ex);

                    }

                    DbDataReader DBreader = null;
                   // DBreader = PCDb.Db.ExecReader(string.Format("select FileData from {0} where  id='{1}'   ",Consts.TableDownloadManager,DlnID), PCDb.Db.GetConnection());
                    DBreader = PCDb.Db.ExecReader(string.Format("select Path, FileName from {0} where  id='{1}'   ", Consts.TableDownloadManager, DlnID), PCDb.Db.GetConnection());
                    while (DBreader.Read())
                    {
                   //     byeOfLength = ((byte[])DBreader["FileData"]).Length;
                        
                        //if (byeOfLength == 0)
                        //{
                        //    litupload.Text = "File size is zero. Download completed.";
                        //    break;
                        //}

                        Response.ClearHeaders();
                        Response.ClearContent();
                        //string _filePathAndName = DBreader["FilePath"].ToString();
                        string _filePathAndName = Server.MapPath("");
                        //string _fileName = ProjCode + "_" + FunctionCode + ".xls";

                        string _fileName = "";
                        if (!String.IsNullOrEmpty(ProjCode.Trim()))
                            _fileName = ProjCode + "_";
                        _fileName += FunctionCode + ".xls";

                        //string _fileName = DBreader["FileName"].ToString();//_filePathAndName.Substring(_filePathAndName.LastIndexOf("\\") + 1);
                        //Response.ContentType = DBreader["ContextType"].ToString();
                        Response.ContentType = "application/vnd.ms-excel";
                        
                        Response.AppendHeader("Content-Disposition", String.Format("attachment;filename=\"{0}\"", HttpContext.Current.Server.UrlPathEncode(_fileName)));
                        //Response.AppendHeader("Content-Disposition", String.Format("attachment;filename=\"{0}\"",((string)DBreader[0]).ToString()));
                        //Response.AppendHeader("content-length", ((byte[])DBreader[0]).Length.ToString());
                        Response.AppendHeader("pragma", "public");
                        Response.AppendHeader("expires", "0");
                        Response.AppendHeader("cache-control", "must-revalidate, post-check=0, pre-check=0");

                        //Response.OutputStream.Write(((byte[])DBreader[0]), 0, byeOfLength);
                        Response.WriteFile(((string)DBreader[0]).ToString() + "\\" + ((string)DBreader[1]).ToString());
                        Response.Flush();
                        Response.End();

                        #region XX
                        //Response.Expires = 0;
                        //Response.Buffer = true;
                        //Response.Clear();

                        //Response.ContentType = DBreader["ContextType"].ToString();
                        //Response.BinaryWrite((byte[])DBreader["AttenFiles"]);
                        #endregion
                    }
                    
                }
                else
                {
                //    SimpleWebUtils.DownloadFile(Page.Response, String.Format("{0}\\{1}", SessionInfo.TempDir, fileName));
                }
                
                
            }
        }
        else
        {

        }

    }


}

