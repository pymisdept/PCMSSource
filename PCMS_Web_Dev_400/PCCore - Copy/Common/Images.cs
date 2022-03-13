using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace PCCore.Common
{
    public class PMRptImages
    {
        public string projcode;
        public string period;
        public string _ImagePath;
        public string _sessionImagePath;
        public System.Web.UI.WebControls.FileUpload _uploadImg;
        public static string SESSION = "session";
        public static string PIC = "pic";
        public FileInfo _imgFile;

        public PMRptImages(string _imgFile):this(new FileInfo(_imgFile))
        {
            
        }
        public PMRptImages(System.Web.UI.WebControls.FileUpload _uploadImg)
        {
            this._uploadImg = _uploadImg;
            InitImagePath();
            if (_uploadImg.HasFile)
                _imgFile = new FileInfo(_uploadImg.FileName);

        }
        public PMRptImages(FileInfo _imgFile)
        {

            InitImagePath();
        }
        
        public string Save(string target)
        {
            string returnName = "";
            
            if (!_uploadImg.HasFile)
                
                return returnName;
            try
            {
                if (target == SESSION)
                {
                    returnName = _sessionImagePath + "\\" + _uploadImg.FileName.Substring(_uploadImg.FileName.LastIndexOf("\\") + 1);
                    _uploadImg.SaveAs(returnName);
                    
                }
                else
                {
                    returnName = _ImagePath + "\\" + _uploadImg.FileName.Substring(_uploadImg.FileName.LastIndexOf("\\") + 1);
                    _uploadImg.SaveAs(returnName);
                    
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Save Image", ex);
                return returnName;
            }
            return returnName;
        }

        public string Copy(string target, bool delete)
        {
            string returnName = "";
            if (_imgFile != null)
            {
                if (target == SESSION)
                {
                    if (delete)

                        _imgFile.MoveTo(_sessionImagePath);
                    else
                        _imgFile.CopyTo(_sessionImagePath);
                    returnName = _sessionImagePath + "\\" + _imgFile.Name;
                }
                else
                {
                    if (delete)
                        _imgFile.MoveTo(_ImagePath);
                    else
                        _imgFile.CopyTo(_ImagePath);
                    returnName = _ImagePath + "\\" + _imgFile.Name;
                }
            }
            else
            {
                returnName = "";
            }
            return returnName;
        }
        
        public void InitImagePath()
        {
            _sessionImagePath = HttpContext.Current.Server.MapPath("~\\" + Config.XMLDirectory + "\\" + HttpContext.Current.Session.SessionID + "\\" + SessionInfo.PMRptProject + "\\" + SessionInfo.PMRptPeriod + "\\PMRPT\\Images");
            _ImagePath = HttpContext.Current.Server.MapPath("~\\" + Config.ImageDirectory + "\\" + SessionInfo.PMRptProject + "\\" + SessionInfo.PMRptPeriod);
            DirectoryInfo _imagedir = new DirectoryInfo(_ImagePath);
            DirectoryInfo _sessiondir = new DirectoryInfo(_sessionImagePath);
            
            if (!_imagedir.Exists)
            {
                try
                {
                    _imagedir.Create();
                }
                catch (Exception ex)
                { PCCore.Common.HRLog.RecordException("Create ImagePath", ex); }
            }
            if (!_sessiondir.Exists)
            {
                try
                {
                    _sessiondir.Create();
                }
                catch (Exception ex)
                { PCCore.Common.HRLog.RecordException("Create SessionImagePath", ex); }
            }
        }

        public void Delete()
        {
            try
            {
                if (_imgFile != null)
                    _imgFile.Delete();
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Delete File", ex);
            }
        }
    }


}
