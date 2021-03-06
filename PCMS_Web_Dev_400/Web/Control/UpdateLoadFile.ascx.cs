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
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data.Common;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;



public partial class UpdateLoadFile : System.Web.UI.UserControl
{
    System.Web.HttpFileCollection hifColl = null;

    public HttpFileCollection UploadFile
    {
        get { return hifColl; }
    }

    public string NeedDeleteRecordID
    {
        get { return txtNeedDeleteRecord.Text; }
    }

    #region include for Gen
    protected string _ControlsIndex = "1";
    public string ControlsIndex
    {
        get { return _ControlsIndex; }
        set { _ControlsIndex = value; }
    }
    protected string _MyInpuTable = "MyInpuTable";
    public string MyInpuTable
    {
        get { return this.ID.ToString() + _MyInpuTable; }
    }
    protected string _MyHtmpInputFiles = "MyHtmpInputFiles";
    public string MyHtmpInputFiles
    {
        get { return this.ID.ToString() + _MyHtmpInputFiles; }
    }
    protected string _NeedInsertIndexsFiles = "NeedInsertIndexsFiles";
    public string NeedInsertIndexsFiles
    {
        get { return this.ID.ToString() + _NeedInsertIndexsFiles; }
    }
    #endregion

    string RecordID = "0";


    #region Resource
    ClientScriptManager _cs = null;
    Type _t = typeof(UpdateLoadFile);
    #endregion Resource

    #region  UserDefineFuncName
    protected string _FuncName = String.Empty;
    public string UserDefineFuncName
    {
        get { return _FuncName; }
        set { _FuncName = value; }
    }
    #endregion UserDefinePID

    #region  UserDefineRecordID
    protected string _RecordID = String.Empty;
    public string UserDefineRecordID
    {
        get { return _RecordID; }
        set { _RecordID = value; }
    }
    #endregion UserDefineRecordID

    #region UserDefineisDisabled
    protected bool _isDisabled = false;
    [Category("Behavior"), DefaultValue("false"), Description("????UserDefineisDisabled")]
    public bool UserDefineisDisabled
    {
        get { return _isDisabled; }
        set { _isDisabled = value; }
    }
    #endregion RegisterClientVariable

    #region RegisterClientVariable
    protected bool _RegisterClientVariable = false;
    [Category("Behavior"), DefaultValue("false"),
    Description("???????????????????????????? Register a client script variable , add by jawance")]
    public bool RegisterClientVariable
    {
        get { return _RegisterClientVariable; }
        set { _RegisterClientVariable = value; }
    }
    #endregion RegisterClientVariable

    #region  UserDefineFileTable
    protected object _FileTable;
    public object UserDefineFileTable
    {
        get { return _FileTable; }
        set { _FileTable = value; }
    }
    #endregion UserDefineFileTable

    protected PCTable _table = null;
    protected PCTable Table
    {
        get
        {
            if (_table == null) _table = new PCTable("CM_SessionFiles");
            _table.EnableLog = false;
            return _table;
        }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _cs = Page.ClientScript;
        this.RegisterClientScripts();
    }

    protected override void Render(HtmlTextWriter writer)
    {
        UpdateLoadTable.Attributes.Add("UserDefineFuncName", UserDefineFuncName);
        UpdateLoadTable.Attributes.Add("UserDefineRecordID", UserDefineRecordID);
        UpdateLoadTable.Attributes.Add("UserDefineisDisabled", UserDefineisDisabled.ToString());

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(UpdateLoadFile));
        if (Page.Request != null && Page.Request.QueryString != null && Page.Request.QueryString["ID"] != null)
        {
            RecordID = Page.Request.QueryString["ID"];
        }
        //????FunctionName
        if (String.IsNullOrEmpty(UserDefineFuncName))
            UserDefineFuncName = Sc.GetFunctionCode(Page.Request.Path);

        string AddInputFunctionName = "";
        int isAllowMulti = SessionInfo.IsBatchUpload ? 1 : 0;
        if (UserDefineFuncName == "qsi40" || UserDefineFuncName == "qsi04" || UserDefineFuncName == "qsi12")
            AddInputFunctionName = "AddNewInputFileMultiple";
        else
            AddInputFunctionName = "AddNewInputFile";

        if (!Page.IsPostBack)
        {            
            if (!UserDefineisDisabled)
            {
                lblUpLabel.Attributes.Add("onclick", string.Format("{0}{1}('{0}','{2}','{3}',{4});return false;",
                          this.ID, AddInputFunctionName, UserDefineFuncName, UserDefineRecordID, isAllowMulti));

                if (Request.QueryString["mode"] != null && Request.QueryString["mode"].ToLower() == "new")
                {
                    string strClient = string.Format(@" function selfToBeExecute()
                                                {{
                                                    {0}{1}('{0}','{2}','{3}',{4});return false;
                                                }}
                                                ", this.ID, AddInputFunctionName, UserDefineFuncName, UserDefineRecordID, isAllowMulti);
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "newclick", strClient,true);
                }
            }
            else
            {
                    
                lblUpLabel.Attributes.Add("onclick", "return false;");
                HrefA.Attributes.Clear();
            }

        }
        else
        {
            string strClient = string.Format(@" function selfToBeExecute()
                                                {{
                                                    {0}{1}('{0}','{2}','{3}',{4});return false;
                                                }}
                                                ", this.ID, AddInputFunctionName, UserDefineFuncName, UserDefineRecordID, isAllowMulti);
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "newclick", strClient, true);
            hifColl =Page.Request.Files;
            //if (CheckUploadFile() == "")
            //{
            //    SaveFiles();
            //}
            //else
            //{
            //    throw new Exception(Resources.Messages.UploadFile);
            //}
        }

    }

    public string CheckUploadFile(decimal descID,DbCommand cmd)
    {
        //if (UploadFile != null && UploadFile.Count == 0)
        if (txtNeedInsertIndexs.Text.Length == 0)
        {
            if (string.IsNullOrEmpty(txtHasCount.Text.Trim()) || Convert.ToDecimal(txtHasCount.Text.Trim()) < 1)
            {
                return Resources.Messages.UploadFile;
            }
            else
            {

                if (txtNeedDeleteRecord.Text.Length > 0)
                {
                    string needDelete = txtNeedDeleteRecord.Text.Substring(0, txtNeedDeleteRecord.Text.Length - 1);
                    int hasCount = (int)PCDb.Db.ExecScalar(string.Format("select count(*) from CM_SessionFiles where id not in({0}) and recordid={1}", needDelete, descID),cmd);
                    if (hasCount < 1)
                    {
                        return Resources.Messages.UploadFile;
                    }
                }
            }
        }
        return "";

    }

    public void SaveFiles(decimal descID,DbCommand cmd)
    {
        if (CheckUploadFile(descID,cmd) != "")
        {
            throw new Exception(Resources.Messages.UploadFile);
        }
        if (String.IsNullOrEmpty(UserDefineFuncName))
            UserDefineFuncName = Sc.GetFunctionCode(Page.Request.Path);

        if (txtNeedDeleteRecord.Text != string.Empty)
        {
            DeleteRecord(descID,txtNeedDeleteRecord.Text, cmd);
        }
        //save append's file 
        SaveToDateBase(descID,cmd);
    }

    // Added by Ken, 20161107, begin
    public void SaveFiles(decimal descID, DbCommand cmd, int crow)
    {
        if (CheckUploadFile(descID, cmd) != "")
        {
            throw new Exception(Resources.Messages.UploadFile);
        }
        if (String.IsNullOrEmpty(UserDefineFuncName))
            UserDefineFuncName = Sc.GetFunctionCode(Page.Request.Path);

        if (txtNeedDeleteRecord.Text != string.Empty)
        {
            DeleteRecord(descID, txtNeedDeleteRecord.Text, cmd);
        }
        //save append's file 
        SaveToDateBase(descID, cmd, crow);
    }
    // Added by Ken, 20161107, end
    #region RegisterClientScripts
    protected void RegisterClientScripts()
    {
        //SimpleJS.RegisterClientScripts(Page, SimpleJS.JSTypes.Common);


        if (this.RegisterClientVariable)
        {
            string _Rstr = "";
            _Rstr = String.Format("var {0}=document.getElementById('{1}'); var {2}= document.getElementById('{3}');",
                this.ID, this.UpdateLoadTable.ClientID, this.ID + this.FileTable.ID, this.FileTable.ClientID);
            //reg display Function
            //_Rstr += "function ShowAppendFile(){ ShowUpLoadFiles('" + this.ID + "','" + UserDefineFuncName + "','" + UserDefineRecordID + "');}";
            _Rstr += string.Format("function ShowAppendFile{5}(){3} "
                              + "      if (typeof(ShowUpLoadFiles{5})==\"function\") "
                              + "      {3} "
                                           + "ShowUpLoadFiles{5}('{0}','{1}','{2}');{4}"
                              + "      {4} ",
                this.ID, UserDefineFuncName, UserDefineRecordID, "{", "}", this.ControlsIndex);


            //reg txtNeedDeleteRecord
            _Rstr += String.Format("var {0}=document.getElementById('{1}'); ",
                 this.ID + this.txtNeedDeleteRecord.ID, this.txtNeedDeleteRecord.ClientID);


            //reg txtNeedInserIndex
            _Rstr += String.Format("var {0}=document.getElementById('{1}'); ",
                 this.ID + this.txtNeedInsertIndexs.ID, this.txtNeedInsertIndexs.ClientID);

            //reg txtHasCount
            _Rstr += String.Format("var {0}=document.getElementById('{1}'); ",
                 this.ID + this.txtHasCount.ID, this.txtHasCount.ClientID);
            
            _cs.RegisterStartupScript(_t, this.ClientID, _Rstr, true);

        }

    }
    #endregion

    [AjaxPro.AjaxMethod]
    public string GetRecordCount(string FunctionName, string FunctionRecordID, string pControlsIndex)
    {
        string _sRecount =
           Convert.ToString(PCDb.Db.ExecScalar(
            string.Format("select isnull(count(id),0) from CM_SessionFiles where (FunctionName='{0}' and RecordID='{1}' and ControlsIndex='{2}' )  ",
            FunctionName, FunctionRecordID, pControlsIndex)));
        return _sRecount;
    }


    [AjaxPro.AjaxMethod]
    public HtmlTable SetPostTable(string FunctionName, string FunctionRecordID, HtmlTable objHTMLTable, string ID, string pUserDefineisDisabled,
        string pControlsID, string pControlsIndex)
    {
        #region "remove RecID"
        if (ID != string.Empty)
        {
            PCDb.Db.ExecScalar(string.Format("delete from CM_SessionFiles where ID='{0}' ", ID));
        }

        #endregion

        HtmlTable _htmlt = objHTMLTable;

        HtmlTableRow _trow = null;
        HtmlTableCell _tcellA = null;
        HtmlTableCell _tcellB = null;
        HtmlTableCell _tcellC = null;
        HtmlTableCell _tcellD = null;
        string _strFilePath = "";
        string _strFileName = "";
        char[] _strChar = { Convert.ToChar("\\") };

        DataTable _dt = null;
        _dt = PCDb.Db.ExecQuery(string.Format("select id,recordid,fileName, filepath ,Filetype,isHadUploaded from CM_SessionFiles where (FunctionName='{0}' and RecordID='{1}') and  ControlsIndex ='{2}' "
            , FunctionName, FunctionRecordID, pControlsIndex));
        if (_dt != null && _dt.Rows.Count > 0)
        {
            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                _trow = new HtmlTableRow();
                _trow.ID = Convert.ToString(i + 1);
                _tcellA = new HtmlTableCell();
                _tcellB = new HtmlTableCell();
                _tcellC = new HtmlTableCell();
                _tcellD = new HtmlTableCell();
                _strFilePath = _dt.Rows[i]["filepath"].ToString();
                _strFileName = _dt.Rows[i]["fileName"].ToString();

                _tcellA.InnerHtml = _dt.Rows[i]["id"].ToString();
                _tcellA.Attributes.Add("style", "display:none");
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(Config.ThemesBaseUrl + "/Default/Images/UploadFileControl/IC" + _dt.Rows[i]["Filetype"].ToString().Substring(1) + ".gif")))
                {
                    _tcellB.InnerHtml = "<IMG SRC='" + Config.ThemesBaseUrl + "/Default/Images/UploadFileControl/IC" + _dt.Rows[i]["Filetype"].ToString().Substring(1) + ".GIF'> ";
                }
                else
                {
                    _tcellB.InnerHtml = "<IMG SRC='" + Config.ThemesBaseUrl + "/Default/Images/UploadFileControl/ICUNKNOW.GIF'> ";
                }


                _tcellB.Attributes.Add("style", "width:16px");
                if (_dt.Rows[i]["isHadUploaded"] == DBNull.Value)
                {
                    _tcellC.InnerHtml = _strFileName;
                }
                else
                {
                    _tcellC.InnerHtml = "<a href=\"#\" onclick=\"javascript:" + pControlsID + "DownLoadThis('" + _dt.Rows[i]["id"].ToString() + "');return false;\">"
                           + _strFileName + "</a>";
                }

                if (pUserDefineisDisabled != "True")
                {
                    _tcellD.InnerHtml = "<a  href=\"#\" class=\"UpCell\" onclick=\"javascript:" + pControlsID + "RemoveLocal(this," + objHTMLTable.ClientID + ",'" + _dt.Rows[i]["id"].ToString() + "','" + Convert.ToString(i + 1) + "');\">"
                        + "<IMG SRC='" + Config.ThemesBaseUrl + "/Default/Images/UploadFileControl/smalldel.gif' align='absmiddle' border=0'>" + Resources.Common.Delete + "</a>";
                }
                _trow.Cells.Add(_tcellA);
                _trow.Cells.Add(_tcellB);
                _trow.Cells.Add(_tcellC);
                _trow.Cells.Add(_tcellD);
                _htmlt.Rows.Add(_trow);
            }
        }

        return _htmlt;
    }

    #region SaveToDataBase
    [AjaxPro.AjaxMethod]
    public void SaveFileMessageToDb(string FunctionName, string FunctionRecordID, string FileName)
    {
        string _fileName = FileName.Substring(FileName.LastIndexOf("\\") + 1);
        Hashtable row = new Hashtable();
        row.Add("ID", PCDb.GetNextID());
        row.Add("SESSIONID", HttpContext.Current.Session.SessionID);
        row.Add("FUNCTIONNAME", FunctionName);
        row.Add("RECORDID", Convert.ToDecimal(FunctionRecordID));
        row.Add("FILENAME", _fileName);


        this.Table.BeginTransaction();
        this.Table.Insert(row);
        this.Table.CommitTransaction();

    }

    private string FindFunctionRecordID(ControlCollection cc, string objID)
    {
        string fid = String.Empty;
        for (int q = 0; q < cc.Count; q++)
        {
            string id = cc[q].ID;
            string type = cc[q].GetType().ToString();
            switch (type)
            {
                case "SimpleControls.Web.SimpleTextBox":
                case "PCCore.TextBox":
                case "System.Web.UI.WebControls.TextBox":
                    if (id == objID)
                    {
                        fid = ((System.Web.UI.WebControls.TextBox)cc[q]).Text;
                    }
                    break;
                case "PCCore.Panel":
                case "System.Web.UI.WebControls.Panel":
                    fid = FindFunctionRecordID(((System.Web.UI.WebControls.Panel)cc[q]).Controls, objID);
                    break;
            }

            if (!String.IsNullOrEmpty(fid)) break;
        }
        return fid;
    }
 
    public void SaveToDateBase(decimal descID,DbCommand cmd)
    {
        string _objID = "";
        
        string _functionFunctionName = UserDefineFuncName;
        ControlCollection _tmpctl = Page.Form.Controls;
        for (int p = 0; p < _tmpctl.Count; p++)
        {
            //????????????????????????MasterControlscontext??
            ControlCollection _tmpctlq = null;
            if (_tmpctl[p].GetType().ToString() == "System.Web.UI.WebControls.ContentPlaceHolder")
            {
                _tmpctlq = _tmpctl[p].Controls;

                //Init Function Name
                for (int q = 0; q < _tmpctlq.Count; q++)
                {
                    if (_tmpctlq[q].GetType().ToString() == "ASP.control_updateloadfile_ascx")
                    {
                       
                        
                        //if (((ASP.control_updateloadfile_ascx)_tmpctlq[q]).UserDefineFuncName != string.Empty)
                        
                        if (((ASP.control_updateloadfile_ascx)_tmpctlq[q]).UserDefineFuncName != string.Empty)
                        {
                            _functionFunctionName = ((ASP.control_updateloadfile_ascx)_tmpctlq[q]).UserDefineFuncName;
                            break;
                        }
                        //no use went dynamic by cline chose
                        //if (((ASP.control_updateloadfile_ascx)_tmpctlq[q]).UserDefineRecordID != string.Empty)
                        //{
                        //    _functionRecordID = ((ASP.control_updateloadfile_ascx)_tmpctlq[q]).UserDefineRecordID;
                        //}
                    }
                }

                _objID = UserDefineRecordID; //"txtID";
                

            }

        }

        
        for (int hifCon = 0; hifCon < hifColl.Count; hifCon++)
        {
            System.Web.HttpPostedFile HIFs = hifColl[hifCon];

            if (HIFs.FileName == string.Empty)
            {
                continue;
            }
            if (txtNeedInsertIndexs.Text.IndexOf(hifCon.ToString() + ',') == -1)
            {
                continue;
            }
            try
            {
                //<1>//Todo: ????????????????????????????????????????????????????????????
                int len = HIFs.ContentLength;
                byte[] content = new byte[len];
                HIFs.InputStream.Read(content, 0, len);


                //2 ????????????
                Hashtable BigFileHs = new Hashtable();
                //BigFileHs.Add("ID", PCDb.GetNextID());
                BigFileHs.Add("ID", PCDb.GetNextSessionFileID());
                //BigFileHs.Add("SESSIONID", HttpContext.Current.Session.SessionID);
                BigFileHs.Add("FUNCTIONNAME", _functionFunctionName);

                //BigFileHs.Add("RECORDID", Convert.ToDecimal(_functionRecordID));
                BigFileHs.Add("RECORDID", descID);
                string _filePathAndName = HIFs.FileName;
                BigFileHs.Add("FILENAME", _filePathAndName.Substring(_filePathAndName.LastIndexOf("\\") + 1));

                if (_filePathAndName.Length > 100)
                {
                    BigFileHs.Add("FILEPATH", _filePathAndName.Substring(0, 100));
                }
                else
                {
                    BigFileHs.Add("FILEPATH", _filePathAndName);
                }

                BigFileHs.Add("FILESIZE", Math.Round(Convert.ToDecimal(len) / 1024, 1));

                if (_filePathAndName.LastIndexOf(".") > 0)
                {
                    BigFileHs.Add("FILETYPE", _filePathAndName.Substring(_filePathAndName.LastIndexOf(".")));
                }
                else
                {
                    BigFileHs.Add("FILETYPE", ".unknow");
                }
                BigFileHs.Add("CONTEXTTYPE", HIFs.ContentType);
                BigFileHs.Add("ATTENFILES", content);
                BigFileHs.Add("ISHADUPLOADED", 1);
                // BigFileHs.Add("CONTROLSINDEX", DBNull.Value);
                BigFileHs.Add("CONTROLSINDEX", Convert.ToDecimal(ControlsIndex));
                
                //this.Table.BeginTransaction();
                this.UpLoadBigFile(BigFileHs, hifCon,cmd);
                //this.Table.CommitTransaction();
            }
            catch (Exception err)
            {
                throw err;
            }

        }

    }
    
    // Added by Ken, 20161107, begin
    public void SaveToDateBase(decimal descID, DbCommand cmd, int crow)
    {
        string _objID = "";

        string _functionFunctionName = UserDefineFuncName;
        ControlCollection _tmpctl = Page.Form.Controls;
        for (int p = 0; p < _tmpctl.Count; p++)
        {
            //????????????????????????MasterControlscontext??
            ControlCollection _tmpctlq = null;
            if (_tmpctl[p].GetType().ToString() == "System.Web.UI.WebControls.ContentPlaceHolder")
            {
                _tmpctlq = _tmpctl[p].Controls;

                //Init Function Name
                for (int q = 0; q < _tmpctlq.Count; q++)
                {
                    if (_tmpctlq[q].GetType().ToString() == "ASP.control_updateloadfile_ascx")
                    {


                        //if (((ASP.control_updateloadfile_ascx)_tmpctlq[q]).UserDefineFuncName != string.Empty)

                        if (((ASP.control_updateloadfile_ascx)_tmpctlq[q]).UserDefineFuncName != string.Empty)
                        {
                            _functionFunctionName = ((ASP.control_updateloadfile_ascx)_tmpctlq[q]).UserDefineFuncName;
                            break;
                        }
                        //no use went dynamic by cline chose
                        //if (((ASP.control_updateloadfile_ascx)_tmpctlq[q]).UserDefineRecordID != string.Empty)
                        //{
                        //    _functionRecordID = ((ASP.control_updateloadfile_ascx)_tmpctlq[q]).UserDefineRecordID;
                        //}
                    }
                }

                _objID = UserDefineRecordID; //"txtID";


            }

        }

        System.Web.HttpPostedFile HIFs = hifColl[crow];

        if (HIFs.FileName == string.Empty)
        {
            return;
        }
        //if (txtNeedInsertIndexs.Text.IndexOf(crow.ToString() + ',') == -1)
        //{
        //    return;
        //}
        try
        {
            //<1>//Todo: ????????????????????????????????????????????????????????????
            int len = HIFs.ContentLength;
            byte[] content = new byte[len];
            HIFs.InputStream.Read(content, 0, len);


            //2 ????????????
            Hashtable BigFileHs = new Hashtable();
            //BigFileHs.Add("ID", PCDb.GetNextID());
            BigFileHs.Add("ID", PCDb.GetNextSessionFileID());
            //BigFileHs.Add("SESSIONID", HttpContext.Current.Session.SessionID);
            BigFileHs.Add("FUNCTIONNAME", _functionFunctionName);

            //BigFileHs.Add("RECORDID", Convert.ToDecimal(_functionRecordID));
            BigFileHs.Add("RECORDID", descID);
            string _filePathAndName = HIFs.FileName;
            BigFileHs.Add("FILENAME", _filePathAndName.Substring(_filePathAndName.LastIndexOf("\\") + 1));

            if (_filePathAndName.Length > 100)
            {
                BigFileHs.Add("FILEPATH", _filePathAndName.Substring(0, 100));
            }
            else
            {
                BigFileHs.Add("FILEPATH", _filePathAndName);
            }

            BigFileHs.Add("FILESIZE", Math.Round(Convert.ToDecimal(len) / 1024, 1));

            if (_filePathAndName.LastIndexOf(".") > 0)
            {
                BigFileHs.Add("FILETYPE", _filePathAndName.Substring(_filePathAndName.LastIndexOf(".")));
            }
            else
            {
                BigFileHs.Add("FILETYPE", ".unknow");
            }
            BigFileHs.Add("CONTEXTTYPE", HIFs.ContentType);
            BigFileHs.Add("ATTENFILES", content);
            BigFileHs.Add("ISHADUPLOADED", 1);
            // BigFileHs.Add("CONTROLSINDEX", DBNull.Value);
            BigFileHs.Add("CONTROLSINDEX", Convert.ToDecimal(ControlsIndex));

            //this.Table.BeginTransaction();
            this.UpLoadBigFile(BigFileHs, crow, cmd);
            //this.Table.CommitTransaction();
        }
        catch (Exception err)
        {
            throw err;
        }
    }
    // Added by Ken, 20161107, end

    void UpLoadBigFile(Hashtable row,int index, DbCommand cmd)
    {
        if (row["ATTENFILES"] == null) return;


        string sql = String.Format("Insert into CM_SessionFiles (ID ,FunctionName ,RecordID ,FileName,FilePath,AttenFiles,Filetype,ContextType ,isHadUploaded,Controlsindex,FileSize,UploadByID,UploadTime ) " +
        " values ({0},'{1}',{2},'{10}','{11}',null ,'{3}','{4}',{5}, {6},{7},{8},'{9}')",
         row[Consts.FieldID], row["FUNCTIONNAME"], row["RECORDID"], row["FILETYPE"].ToString(), row["CONTEXTTYPE"].ToString(), row["ISHADUPLOADED"], row["CONTROLSINDEX"], row["FILESIZE"], SessionInfo.UserId, DateTime.Now.ToString(Consts.DateTimeFormat), row["FILENAME"], row["FILEPATH"],index);
        SqlParameter[] para =
        {
            new SqlParameter(string.Format("@images{0}",index),row["ATTENFILES"])
            //new SqlParameter("@filename",row["FILENAME"]),
            //new SqlParameter("@filepath",row["FILEPATH"])
        };       
        
        //SqlCommand Comm = cmd as SqlCommand;

        //SqlParameter param = Comm.Parameters.Add("@images", SqlDbType.Image, ((byte[])row["ATTENFILES"]).Length);
        //SqlParameter param2 = Comm.Parameters.Add("@filename", SqlDbType.NVarChar);
        //SqlParameter param3 = Comm.Parameters.Add("@filepath", SqlDbType.NVarChar);
        //param.Value = row["ATTENFILES"];
        //param2.Value = row["FILENAME"];
        //param3.Value = row["FILEPATH"];
        
        //Comm.ExecuteNonQuery();

        PCDb.Db.ExecUpdate(sql, para as DbParameter[], cmd);
    }

    #endregion

    #region DeleteRecord
    public void DeleteRecord(decimal descID,string _NeedDeleteRecordCollection,DbCommand cmd)
    {
        string[] _RecordID;
        _RecordID = _NeedDeleteRecordCollection.Split(',');
        for (int i = 0; i < _RecordID.Length; i++)
        {
            if (_RecordID[i] != string.Empty)
            {
                PCDb.Db.ExecScalar(string.Format("delete from CM_SessionFiles where ID='{0}' and recordid={1} ", _RecordID[i],descID),cmd);
            }
        }
    }
    #endregion


    [AjaxPro.AjaxMethod]
    public string AccessExitFileFullPath(string sub)
    {
        string _iFilePath = string.Empty;

        if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(Config.ThemesBaseUrl + "/Default/Images/UploadFileControl/IC" + sub + ".gif")))
        {
            _iFilePath = Config.ThemesBaseUrl + "/Default/Images/UploadFileControl/IC" + sub + ".gif";
        }
        else
        {
            _iFilePath = Config.ThemesBaseUrl + "/Default/Images/UploadFileControl/ICUNKNOW.gif";
        }
        return _iFilePath;
    }

    [AjaxPro.AjaxMethod]
    public bool ULFCheckFile(string FilePath)
    {
        bool sIsCheckOK = true;
        if (IsInvalidFilePath(FilePath) || IsInvalidFileName(FilePath))
        {
            sIsCheckOK = false;
        }
        return sIsCheckOK;
    }

    #region UploadFile Helpper

    /// <summary>
    /// ??????????????????????????????,????????????????????????
    /// </summary>
    /// <param name="fullPath"></param>
    /// <returns></returns>
    private bool IsInvalidFilePath(string fullPath)
    {
        bool S_isInvalid = false;

        //????????
        if (string.IsNullOrEmpty(fullPath))
        {
            return S_isInvalid = true;
        }
        //???????????? \
        if (fullPath.IndexOf(Convert.ToChar(@"\")) < 0)
        {
            return S_isInvalid = true;
        }

        try
        {
            Path.GetFullPath(fullPath);
        }
        catch
        {
            return S_isInvalid = true;
        }

        // Get a list of invalid path characters.
        char[] invalidPathChars = Path.GetInvalidPathChars();

        foreach (char invalidPChar in invalidPathChars)
        {
            if (fullPath.IndexOf(invalidPChar) > 0)
            {
                return S_isInvalid = true;
            }
        }
        return S_isInvalid;
    }

    /// <summary>
    ///  ??????????????????
    /// </summary>
    /// <param name="fullname"></param>
    /// <returns></returns>
    private bool IsInvalidFileName(string fullname)
    {
        bool S_isInvalid = false;
        string S_FileName = string.Empty;

        //????????
        if (string.IsNullOrEmpty(fullname))
        {
            return S_isInvalid = true;
        }

        try
        {
            S_FileName = Path.GetFileName(fullname);
        }
        catch
        {
            return S_isInvalid = true;
        }

        //????????filename????????
        if (string.IsNullOrEmpty(S_FileName))
        {
            return S_isInvalid = true;
        }

        // Get a list of invalid path characters.
        char[] invalidPathChars = Path.GetInvalidFileNameChars();

        foreach (char invalidPChar in invalidPathChars)
        {
            if (S_FileName.IndexOf(invalidPChar) > 0)
            {
                return S_isInvalid = true;
            }
        }
        return S_isInvalid;
    }

    #endregion
    /// <summary>
    /// SaveToFile: 
    /// </summary>
    /// <param name="descID"></param>
    /// <param name="cmd"></param>
    /// <param name="FuncPath"> false</param>
    public void SaveToFile(decimal descID, DbCommand cmd, String FuncPath, String ProjectCode)
    {
        SaveToFile(descID, cmd, FuncPath, ProjectCode, false);
    }
    /// <summary>
    /// SaveToFile: Save Excel Template to Server
    /// </summary>
    /// <param name="descID"></param>
    /// <param name="cmd"></param>
    /// <param name="FuncPath"></param>
    /// <param name="ProjectCode"></param>
    /// <param name="DeleteOldRecord"></param>
    public void SaveToFile(decimal descID, DbCommand cmd, String FuncPath, String ProjectCode, Boolean DeleteOldRecord)
    {
        FileInfo _f = null;
        Boolean error = false;
        String FullName = "";
        String FileName = "";
        String FileExt = "";
        String FilePath = "";
        String SaveName = "";
        String SavePath = "";
        DirectoryInfo _di = null;
        String StrKeyPrefix = Convert.ToString(descID) + "_" + ProjectCode.Trim() + "_" + SessionInfo.UserId + "_";
        // get path from resourse file
        try
        {

            //SavePath = Server.MapPath(Config.UploadFolder + FuncPath);
            if (Config.UploadFolder != String.Empty)
                SavePath = Config.UploadFolder + FuncPath;
            else
                SavePath = Server.MapPath(Resources.TemplateInfo.UPLOADPATH + FuncPath);


        }
        catch (Exception e)
        {
            PCCore.Common.HRLog.RecordException("SavetoFile", e);
            throw e;
            error = true;
        }
        if (!error)
        {
            if (DeleteOldRecord)
            {
                try
                {
                    _di = new DirectoryInfo(SavePath);
                    if (!_di.Exists)
                    {
                        _di.Create();
                    }
                    foreach (FileInfo __fi in _di.GetFiles())
                    {
                        try
                        {
                            if (__fi.Name.Substring(0, StrKeyPrefix.Length) == StrKeyPrefix)
                            {
                                __fi.Delete();
                            }
                        }
                        catch (Exception ex)
                        {
                            PCCore.Common.HRLog.RecordException("SavetoFile", ex);
                            /* karrson: exception to check to file keypredix, assump not match, no need to delete file*/
                        }

                    }
                }
                catch (Exception e)
                {
                    PCCore.Common.HRLog.RecordException("SavetoFile", e);
                    /*throw e;*/
                }
            }
            else
            {
                _di = new DirectoryInfo(SavePath);
                if (!_di.Exists)
                {
                    _di.Create();
                }
            }
        }
        if (!error)
        {
            for (int hifCon = 0; hifCon < hifColl.Count; hifCon++)
            {
                System.Web.HttpPostedFile HIFs = hifColl[hifCon];
                PCCore.Common.HRLog.RecordLog(HIFs.FileName);

                try
                {
                    FullName = HIFs.FileName;
                    if (FullName.LastIndexOf("\\") > 0)
                    {
                        FileName = FullName.Substring(FullName.LastIndexOf("\\") + 1);
                        FilePath = FullName.Substring(0, FullName.LastIndexOf("\\"));
                    }
                    else
                    {
                        FileName = FullName;
                        FilePath = "";
                    }
                    if (FullName.LastIndexOf(".") >= 0)
                    {
                        FileExt = FullName.Substring(FullName.LastIndexOf("."));
                    }
                    else
                    {
                        FileExt = "";
                    }
                    //PCCore.Common.FileUpload _file = new PCCore.Common.FileUpload(null, FileName, "");
                    SaveName = StrKeyPrefix + FileName;
                }
                catch (Exception ex)
                {
                    PCCore.Common.HRLog.RecordException("UpdateLoad", ex);
                }

                //SaveName = 
                try
                {
                    PCCore.Common.HRLog.RecordLog("SaveFile:" + SavePath + "\\" + SaveName);
                    HIFs.SaveAs(SavePath + "\\" + SaveName);

                    if (FuncPath != "MAI01")
                    {

                        //Added by Eric
                        Excel.Application xlApp;
                        Excel.Workbook xlWorkBook;

                        object misValue = System.Reflection.Missing.Value;

                        xlApp = new Excel.ApplicationClass();

                        //HttpPostedFile file = UpdateLoad.UploadFile[0];
                        //file.SaveAs(HttpContext.Current.Server.MapPath("~/Temp") + "/temp.xls");

                        xlWorkBook = xlApp.Workbooks.Open(SavePath + "\\" + SaveName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                        string[] str1 = HttpContext.Current.Request.Url.ToString().Split('/');
                        string[] str2 = str1[str1.Length - 1].ToString().Split('_');
                        if (xlWorkBook.Keywords != "")
                        {
                            if (xlWorkBook.Keywords != str2[0].ToString().ToUpper())
                            {
                                //File.Delete(SavePath + "\\" + SaveName);
                                PCDb.Db.ExecUpdate("insert into DocumentMessage (id,idx,LineNum,xlWkSheet,ErrMessage,ErrCode) select " + descID + ",1,0,'FORM','Wrong Template', '1234' ");
                                PCDb.Db.ExecUpdate("update DocumentProperty set DocStatus = 'DR' where id = " + descID);
                            }
                        }


                        xlWorkBook.Close(false, misValue, misValue);
                        xlApp.Quit();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);

                        //Ended by Eric
                    }

                }
                catch (Exception e)
                {
                    PCCore.Common.HRLog.RecordException("SavetoFile", e);
                    throw e;

                }
            }
        }
    }

    // Added by Ken, 20161107, begin
    public void SaveToFile(decimal descID, DbCommand cmd, String FuncPath, String ProjectCode, Boolean DeleteOldRecord, int crow)
    {
        FileInfo _f = null;
        Boolean error = false;
        String FullName = "";
        String FileName = "";
        String FileExt = "";
        String FilePath = "";
        String SaveName = "";
        String SavePath = "";
        DirectoryInfo _di = null;
        String StrKeyPrefix = Convert.ToString(descID) + "_" + ProjectCode.Trim() + "_" + SessionInfo.UserId + "_";
        // get path from resourse file
        try
        {

            //SavePath = Server.MapPath(Config.UploadFolder + FuncPath);
            if (Config.UploadFolder != String.Empty)
                SavePath = Config.UploadFolder + FuncPath;
            else
                SavePath = Server.MapPath(Resources.TemplateInfo.UPLOADPATH + FuncPath);


        }
        catch (Exception e)
        {
            PCCore.Common.HRLog.RecordException("SavetoFile", e);
            throw e;
            error = true;
        }
        if (!error)
        {
            if (DeleteOldRecord)
            {
                try
                {
                    _di = new DirectoryInfo(SavePath);
                    if (!_di.Exists)
                    {
                        _di.Create();
                    }
                    foreach (FileInfo __fi in _di.GetFiles())
                    {
                        try
                        {
                            if (__fi.Name.Substring(0, StrKeyPrefix.Length) == StrKeyPrefix)
                            {
                                __fi.Delete();
                            }
                        }
                        catch (Exception ex)
                        {
                            PCCore.Common.HRLog.RecordException("SavetoFile", ex);
                            /* karrson: exception to check to file keypredix, assump not match, no need to delete file*/
                        }

                    }
                }
                catch (Exception e)
                {
                    PCCore.Common.HRLog.RecordException("SavetoFile", e);
                    /*throw e;*/
                }
            }
            else
            {
                _di = new DirectoryInfo(SavePath);
                if (!_di.Exists)
                {
                    _di.Create();
                }
            }
        }
        if (!error)
        {
            System.Web.HttpPostedFile HIFs = hifColl[crow];
            PCCore.Common.HRLog.RecordLog(HIFs.FileName);

            try
            {
                FullName = HIFs.FileName;
                if (FullName.LastIndexOf("\\") > 0)
                {
                    FileName = FullName.Substring(FullName.LastIndexOf("\\") + 1);
                    FilePath = FullName.Substring(0, FullName.LastIndexOf("\\"));
                }
                else
                {
                    FileName = FullName;
                    FilePath = "";
                }
                if (FullName.LastIndexOf(".") >= 0)
                {
                    FileExt = FullName.Substring(FullName.LastIndexOf("."));
                }
                else
                {
                    FileExt = "";
                }
                //PCCore.Common.FileUpload _file = new PCCore.Common.FileUpload(null, FileName, "");
                SaveName = StrKeyPrefix + FileName;
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("UpdateLoad", ex);
            }

            //SaveName = 
            try
            {
                PCCore.Common.HRLog.RecordLog("SaveFile:" + SavePath + "\\" + SaveName);
                HIFs.SaveAs(SavePath + "\\" + SaveName);

                if (FuncPath != "MAI01")
                {

                    //Added by Eric
                    Excel.Application xlApp;
                    Excel.Workbook xlWorkBook;

                    object misValue = System.Reflection.Missing.Value;

                    xlApp = new Excel.ApplicationClass();

                    //HttpPostedFile file = UpdateLoad.UploadFile[0];
                    //file.SaveAs(HttpContext.Current.Server.MapPath("~/Temp") + "/temp.xls");

                    xlWorkBook = xlApp.Workbooks.Open(SavePath + "\\" + SaveName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                    string[] str1 = HttpContext.Current.Request.Url.ToString().Split('/');
                    string[] str2 = str1[str1.Length - 1].ToString().Split('_');
                    if (xlWorkBook.Keywords != "")
                    {
                        if (xlWorkBook.Keywords != str2[0].ToString().ToUpper())
                        {
                            //File.Delete(SavePath + "\\" + SaveName);
                            PCDb.Db.ExecUpdate("insert into DocumentMessage (id,idx,LineNum,xlWkSheet,ErrMessage,ErrCode) select " + descID + ",1,0,'FORM','Wrong Template', '1234' ");
                            PCDb.Db.ExecUpdate("update DocumentProperty set DocStatus = 'DR' where id = " + descID);
                        }
                    }


                    xlWorkBook.Close(false, misValue, misValue);
                    xlApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);

                    //Ended by Eric
                }

            }
            catch (Exception e)
            {
                PCCore.Common.HRLog.RecordException("SavetoFile", e);
                throw e;

            }
        }
    }
    // Added by Ken, 20161107, end
}

