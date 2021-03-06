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

public partial class DownLoadFile : BasePage
{

    protected override void OnInit(EventArgs e)
    {
        ShowWebMenu = false;
        base.OnInit(e);
    }
    string UpRecordID;
    Int32 byeOfLength = 0;
    string fileName = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            if (Page.Request.QueryString != null && Page.Request.QueryString.Count > 0)
            {
                UpRecordID = Page.Request.QueryString["UpRecordID"] as string;
                fileName = Page.Request.QueryString["filename"] as string;

                if (!String.IsNullOrEmpty(UpRecordID))
                {
                    DbDataReader DBreader = null;
                    //DBreader = PCDb.Db.ExecReader(string.Format("select AttenFiles,FilePath,FileName,ContextType,isHadUploaded from CM_SessionFiles where  id='{0}'   ", UpRecordID), PCDb.Db.GetConnection());
                    DBreader = PCDb.Db.ExecReader(string.Format("select SFilePath, SFileName, FilePath, FileName, ContextType, isHadUploaded from CM_SessionFiles where  id='{0}'   ", UpRecordID), PCDb.Db.GetConnection());
                    
                    while (DBreader.Read())
                    {
                        //byeOfLength = ((byte[])DBreader["AttenFiles"]).Length;
                        //if (DBreader["isHadUploaded"] == System.DBNull.Value)
                        //{
                        //    litupload.Text = "File is null. Download completed.";
                        //    break;
                        //}
                        //if (byeOfLength == 0)
                        //{
                        //    litupload.Text = "File size is zero. Download completed.";
                        //    break;
                        //}

                        Response.ClearHeaders();
                        Response.ClearContent();
                        string _filePathAndName = DBreader["FilePath"].ToString();
                        string _fileName = DBreader["FileName"].ToString();//_filePathAndName.Substring(_filePathAndName.LastIndexOf("\\") + 1);
                        Response.ContentType = DBreader["ContextType"].ToString();
                        Response.AppendHeader("Content-Disposition", String.Format("attachment;filename=\"{0}\"", HttpContext.Current.Server.UrlPathEncode(_fileName)));

                        //Response.AppendHeader("content-length", ((byte[])DBreader["AttenFiles"]).Length.ToString());
                        Response.AppendHeader("pragma", "public");
                        Response.AppendHeader("expires", "0");
                        Response.AppendHeader("cache-control", "must-revalidate, post-check=0, pre-check=0");

                        //Response.OutputStream.Write(((byte[])DBreader["AttenFiles"]), 0, byeOfLength);
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
                    SimpleWebUtils.DownloadFile(Page.Response, String.Format("{0}\\{1}", SessionInfo.TempDir, fileName));
                }
                
                
            }
        }
        else
        {

        }

    }


}

