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



namespace PCCore.PCMS
{
    public class Upload
    {
        
        FileUpload aspFileUpload;
        Hashtable row;
        string FuncId;
        System.Web.HttpFileCollection hifColl;
        public Upload(FileUpload _aspFileUpload,Hashtable _row,string _FuncId)
        {
            aspFileUpload = _aspFileUpload;
            row = _row;
            _FuncId = FuncId;

        }


        public void SaveFiles(decimal descID, DbCommand cmd)
        {
           
            SaveToDateBase(descID, cmd);
        }
    void UpLoadBigFile(Hashtable row, int index, DbCommand cmd)
        {
            if (row["ATTENFILES"] == null) return;


            string sql = String.Format("Insert into CM_SessionFiles (ID ,FunctionName ,RecordID ,FileName,FilePath,AttenFiles,Filetype,ContextType ,isHadUploaded,Controlsindex,FileSize,UploadByID,UploadTime ) " +
            " values ({0},'{1}',{2},'{10}','{11}',@images{12} ,'{3}','{4}',{5}, {6},{7},{8},'{9}')",
             row[Consts.FieldID], row["FUNCTIONNAME"], row["RECORDID"], row["FILETYPE"].ToString(), row["CONTEXTTYPE"].ToString(), row["ISHADUPLOADED"], row["CONTROLSINDEX"], row["FILESIZE"], SessionInfo.UserId, DateTime.Now.ToString(Consts.DateTimeFormat), row["FILENAME"], row["FILEPATH"], index);
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

            SavePath = System.Web.HttpContext.Current.Server.MapPath(System.Web.HttpContext.GetGlobalResourceObject(Consts.ResourcesTemplateInfo, Consts.ResourcesUPLOADPATH).ToString()) + FuncPath;
            
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
            
            //System.Web.HttpPostedFile HIFs = hifColl[hifCon];
             System.Web.HttpPostedFile HIFs = aspFileUpload.PostedFile;
            FullName = HIFs.FileName;
            FileName = FullName.Substring(FullName.LastIndexOf("\\") + 1);
            FilePath = FullName.Substring(0, FullName.LastIndexOf("\\"));
            FileExt = FullName.Substring(FullName.LastIndexOf("."));
            //PCCore.Common.FileUpload _file = new PCCore.Common.FileUpload(null, FileName, "");
            SaveName = StrKeyPrefix + FileName;


            //SaveName = 
            try
            {
                HIFs.SaveAs(SavePath + "\\" + SaveName);
            }
            catch (Exception e)
            {
                PCCore.Common.HRLog.RecordException("SavetoFile", e);
                throw e;

            }
        }
    
    }


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
        /// ÅÐ¶ÏÍêÕûÂ¼ÈëµÄÎÄ¼þÂ·¾¶ÊÇ·ñºÏ·¨,µ«Ã»ÓÐ¼ì²â¸ÃÎÄ¼þÊÇ·ñ´æÔÚ
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        private bool IsInvalidFilePath(string fullPath)
        {
            bool S_isInvalid = false;

            //²»ÄÜÎª¿Õ
            if (string.IsNullOrEmpty(fullPath))
            {
                return S_isInvalid = true;
            }
            //×îÆðÂëÓÐÒ»¸ö \
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
        ///  ¨
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        private bool IsInvalidFileName(string fullname)
        {
            bool S_isInvalid = false;
            string S_FileName = string.Empty;

            //²»ÄÜÎª¿Õ
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

            //½ØÈ¡ºóµÄfilename²»ÄÜÎª¿Õ
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
        public void SaveToDateBase(decimal descID, DbCommand cmd)
        {
            HttpPostedFile HIFs = aspFileUpload.PostedFile;
            string _objID = "";
            PCCore.Database.SC_Function _scFunc = new PCCore.Database.SC_Function(FuncId);
            
                try
                {
                    int len = (int) aspFileUpload.FileContent.Length;
                    byte[] content = new byte[len];
                    aspFileUpload.FileContent.Read(content, 0, len);
                    
                    Hashtable BigFileHs = new Hashtable();
                    
                    BigFileHs.Add("ID", PCDb.GetNextSessionFileID());
                    
                    BigFileHs.Add("FUNCTIONNAME", _scFunc.FunctionName());

                    //BigFileHs.Add("RECORDID", Convert.ToDecimal(_functionRecordID));
                    BigFileHs.Add("RECORDID", descID);
                    string _filePathAndName = aspFileUpload.FileName;
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
                    //BigFileHs.Add("CONTEXTTYPE", HIFs.ContentType);
                    BigFileHs.Add("CONTEXTTYPE",aspFileUpload.PostedFile.ContentType);
                    BigFileHs.Add("ATTENFILES", content);
                    BigFileHs.Add("ISHADUPLOADED", 1);
                    // BigFileHs.Add("CONTROLSINDEX", DBNull.Value);
                    BigFileHs.Add("CONTROLSINDEX", Convert.ToDecimal(1));

                    //this.Table.BeginTransaction();
                    this.UpLoadBigFile(BigFileHs, 1, cmd);
                    //this.Table.CommitTransaction();
                }
                catch (Exception err)
                {
                    throw err;
                }


            

        }

        #region DeleteRecord
        public void DeleteRecord(decimal descID, string _NeedDeleteRecordCollection, DbCommand cmd)
        {
            string[] _RecordID;
            _RecordID = _NeedDeleteRecordCollection.Split(',');
            for (int i = 0; i < _RecordID.Length; i++)
            {
                if (_RecordID[i] != string.Empty)
                {
                    PCDb.Db.ExecScalar(string.Format("delete from CM_SessionFiles where ID='{0}' and recordid={1} ", _RecordID[i], descID), cmd);
                }
            }
        }
        #endregion

        
    }
}

    



