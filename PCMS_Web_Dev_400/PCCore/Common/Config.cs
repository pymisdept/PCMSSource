
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Hosting;
using SimpleControls;

namespace PCCore
{
    public class Config
    {
        protected static string _Database = String.Empty;
        protected static string _Debug = ConfigurationManager.AppSettings["Debug"];
        protected static string _LogException = ConfigurationManager.AppSettings["LogException"];
        protected static string _LogDebug = ConfigurationManager.AppSettings["LogDebug"];

        protected static string _SessionDirectory = ConfigurationManager.AppSettings["SessionDirectory"];
        protected static string _XMLDirectory = _SessionDirectory + "\\" + ConfigurationManager.AppSettings["XMLDirectory"];
        protected static string _ImageDirectory = ConfigurationManager.AppSettings["ImageDirectory"];
        protected static ConnectionStringSettings _defaultConnection = ConfigurationManager.ConnectionStrings["Default"];
        // SAP Connection String
        protected static ConnectionStringSettings _SAPConnection = ConfigurationManager.ConnectionStrings["SAP"];
        protected static string _homePage = ConfigurationManager.AppSettings["homePage"];
        protected static string _smtpServer = ConfigurationManager.AppSettings["smtpServer"];
        //protected static string _supportEmail = ConfigurationManager.AppSettings["supportEmail"];
        protected static string _fromMail = ConfigurationManager.AppSettings["fromMail"];
        protected static string _fromName = ConfigurationManager.AppSettings["fromName"];
        protected static string _fromDatabase = ConfigurationManager.AppSettings["fromDatabase"];
        protected static string _license = ConfigurationManager.AppSettings["PCLicense"];
        protected static string _license2 = ConfigurationManager.AppSettings["PCLicense2"];
        protected static string _license3 = ConfigurationManager.AppSettings["PCLicense3"];
        protected static string _license4 = ConfigurationManager.AppSettings["PCLicense4"];
        protected static string _voucherPrinter = ConfigurationManager.AppSettings["VoucherPrinter"];
        protected static string _smtpUserName = ConfigurationManager.AppSettings["smtpUserName"];
        protected static string _smtpPassword = ConfigurationManager.AppSettings["smtpPassword"];
        protected static string _sessiontimeout = ConfigurationManager.AppSettings["sessiontimeout"];
        protected static string _winScrollWidth = ConfigurationManager.AppSettings["winScrollWidth"];
        protected static string _winScrollHeight = ConfigurationManager.AppSettings["winScrollHeight"];
        protected static string _SAPServer = ConfigurationManager.AppSettings["SAPServer"];
        protected static string _BatchPRUploadLimit = ConfigurationManager.AppSettings["BatchPRUploadLimit"];
        protected static string _BatchSCUploadLimit = ConfigurationManager.AppSettings["BatchSCUploadLimit"];
        /// <summary>
        /// Use in Leave Note Email Signature
        /// </summary>
        protected static string _fromSignature = ConfigurationManager.AppSettings["fromSignature"];
        protected static string _datafilterModuleID = "1,2,3,4";
        // Attachment Path
        protected static string _attachpath = ConfigurationManager.AppSettings["AttachmentPath"];
        public static bool isLogDebug
        {
            get
            {
                try
                {
                    return (_LogDebug == "Y");
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public static string AttachmentPath
        {
            get
            {
                try
                {
                    return _attachpath;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }
        }
        public static bool isLogException
        {
            get
            {
                try
                {
                    return (_LogException == "Y");
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public static bool isDebug
        {
            get
            {
                try

                {
                    return (_Debug == "Y");
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        
        }

        public static string SAPDatabase
        {
            get
            {
                PCCore.Common.HRLog.RecordLog("SAPDatabase:" + Config.Database.ToString());
                return ConfigurationManager.AppSettings[SessionInfo.Database].ToString();
            }
        }

        public static string SAPServer
        {
            get
            {
                return _SAPServer;
            }
        }

        public static string UploadFolder
        {
            get
            {
                if (Convert.ToString(ConfigurationManager.AppSettings["UploadFolder"]) != null)
                    return Convert.ToString(ConfigurationManager.AppSettings["UploadFolder"]);
                else
                    return "";
            }
        }
        
        public static int UploadWaitSecond
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["UploadWait"]);
                }
                catch (Exception ex)
                {
                    // Default 60 seconds
                    return 1000;
                }
            }
        }

        public static int DlnWaitSecond
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["DownloadRequestWait"]);
                }
                catch (Exception ex)
                {
                    // Default 60 seconds
                    return 60;
                }
            }
        }
        public static int SessionTimeout
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["SessionTimeout"]);
                }
                catch (Exception ex)
                {
                    // Default 60 seconds
                    return 30;
                }
            }
        }

        public static string DefaultProviderName
        {
            get { return _defaultConnection == null ? String.Empty : _defaultConnection.ProviderName; }
        }

        public static string DefaultConnectionString
        {
            get
            {
                    return (_defaultConnection == null || SessionInfo.Database == String.Empty) ?
                      String.Empty : String.Format(_defaultConnection.ConnectionString, SessionInfo.Database);
                
            }
            //get { return _defaultConnection == null ? String.Empty :_defaultConnection.ConnectionString; }           
        }
        
/// <summary>
/// SAPConnectionString: get Connection String of SAP B1
/// </summary>
/// 
        public static string SAPConnectionString
        {
            get
            {
                //return (_SAPConnection == null || SessionInfo.Database == String.Empty) ?
                //  String.Empty : String.Format(_SAPConnection.ConnectionString, ConfigurationManager.AppSettings["SAPDB"]);
                return (_SAPConnection == null || SessionInfo.Database == String.Empty) ?
                  String.Empty : String.Format(_SAPConnection.ConnectionString, ConfigurationManager.AppSettings[SessionInfo.Database]).Split(',')[0].ToString();

            }
            //get { return _defaultConnection == null ? String.Empty :_defaultConnection.ConnectionString; }           
        }

        public static string NoDbConnectionString
        {
            get
            {
                return _defaultConnection.ConnectionString;
            }
        }

        public static string DefaultDatabase
        {
            get { return _Database; }
            set { _Database = value; }
        }

        protected const string ThemesDirectory = "App_Themes";
        protected const string TempDirectory = "Temp";

        public static string AppBaseUrl
        {
            get { return HostingEnvironment.ApplicationVirtualPath; }
        }
        public static string AppphysicsUrl
        {
            get { return HostingEnvironment.ApplicationPhysicalPath; }
        }
        public static string ThemesBaseUrl
        {
            get { return String.Format("{0}/{1}", AppBaseUrl, ThemesDirectory); }
        }
        public static string GetThemeBaseUrl(string theme)
        {
            return String.Format("{0}/{1}", ThemesBaseUrl, theme);
        }
        public static string GetImageBaseUrl(string theme)
        {
            return String.Format("{0}/{1}/images", ThemesBaseUrl, theme);
        }
        public static string TempBaseUrl
        {
            get { return String.Format("{0}/{1}", AppBaseUrl, TempDirectory); }
        }
        public static string AppHomeUrl
        {
            get { return String.Format("{0}/{1}", AppBaseUrl, _homePage); }
        }
        public static string ServerIPAddress
        {
            get
            {
                try
                {
                    return HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"].ToString();
                }
                catch
                {
                    return String.Empty;
                }

            }
        }
        public static string SMTPServer
        {
            get { return _smtpServer; }
        }

        public static string SMTPUserName
        {
            get
            {
                return _smtpUserName;
            }
        }

        public static string SMTPPassword
        {
            get
            {
                return _smtpPassword;
            }
        }

        //public static string SupportEmail
        //{
        //    get { return _supportEmail; }
        //}

        public static string FromMail
        {
            get { return _fromMail; }
        }

        /// <summary>
        /// Use in ReminderBase.cs Config.FromName
        /// </summary>
        public static string FromName
        {
            get { return _fromName; }
        }

        /// <summary>
        /// Use in Note Email
        /// </summary>
        public static string FromSignature
        {
            get { return _fromSignature; }
        }

        public static string Database
        {
            get { return _fromDatabase; }
        }

        public static string License
        {
            get { return _license; }
        }
        public static string License2
        {
            get { return _license2; }
        }
        public static string License3
        {
            get { return _license3; }
        }
        public static string License4
        {
            get { return _license4; }
        }

        public static string VoucherPrinter
        {
            get
            {
                return _voucherPrinter;
            }
        }

        public static bool DataFilterModule(string moduleid)
        {
            if (moduleid == "9") return false;
            string[] filterid = _datafilterModuleID.Split(',');
            for (int i = 0; i < filterid.Length; i++)
            {
                if (filterid[i] == moduleid) return true;
            }
            return false;
        }

        public static int BatchPRUploadLimit
        {
            get
            {
                if (String.IsNullOrEmpty(_BatchPRUploadLimit))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(_BatchPRUploadLimit);
                }
            }
        }

        public static int BatchSCUploadLimit
        {
            get
            {
                if (String.IsNullOrEmpty(_BatchSCUploadLimit))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(_BatchSCUploadLimit);
                }
            }
        }

        /// <summary>
        /// Max Work Space Height
        /// </summary>
        public static int winScrollHeight
        {
            get
            {
                if (String.IsNullOrEmpty(_winScrollHeight))
                {
                    return 0;//768-> 738; (Status Bar Height = 30)
                }
                else
                {
                    return Convert.ToInt32(_winScrollHeight);
                }
            }
        }


        /// <summary>
        /// Max Work Space Width
        /// </summary>
        public static int winScrollWidth
        {
            get
            {
                if (String.IsNullOrEmpty(_winScrollWidth))
                {
                    return 0;//1024;
                }
                else
                {
                    return Convert.ToInt32(_winScrollWidth);
                }
            }
        }
        public static string ImageDirectory
        {
            get
            { return _ImageDirectory; }
            set
            { _ImageDirectory = value; }

        }
        public static string SessionDirectory
        {
            get
            {
                return _SessionDirectory;
            }
        }

        public static string XMLDirectory
        {
            get
            {
                return _XMLDirectory;
            }
        }

    }//end of class
}
