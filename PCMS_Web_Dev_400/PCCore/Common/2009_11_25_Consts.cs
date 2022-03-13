using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;


namespace PCCore
{

    /// <summary>
    /// Summary description for Consts
    /// </summary>

    [Serializable]
    public class Consts
    {

        public enum ReportMode{New,Edit,View};
        // Project All Code Value
        public const string ProjectAll = "All Project";

        
        
        // karrson: Add Filter Const
        public const string Filter_Function_List = "(isNull(FALL,0) = 1 or isNull(FNEW,0) = 1 or isNull(FEDT,0) = 1 or isNull(FDEL,0) = 1)";

        //karrson: Add Const field
        public const string Function_DownloadManager = "DownloadManager";
        public const string Function_RequestDownload = "REQUESTDOWNLOAD";
        public const string Confirm = "confirm";
        public const string Save = "save";
        public const string Add = "add";
        public const string SaveDB = "savedb";
        public const string Submit = "submit";
        public const string Cancel = "cancel";
        public const string Delete = "delete";
        public const string Redirect = "RedirectUrl";
        public const string AppName = "Project Cost Management System";

        // Prject Status Field
        public const string PROJECT_OPEN = "O";
        public const string PROJECT_CANCEL = "X";
        public const string PROJECT_CLOSE = "C";

        // Status Field 
        public const string STATUS_OPEN = "O";
        public const string STATUS_CANCEL = "X";
        public const string STATUS_CLOSE = "C";

        // Document Status
        public const string DOCUMENT_CLOSED = "C";
        public const string DOCUMENT_DRAFT = "D";
        public const string DOCUMENT_DRAFT_PENDING = "DP";
        public const string DOCUMENT_DRAFT_REJECTED = "DR";
        public const string DOCUMENT_POSTED = "P";
        public const string DOCUMENT_POST_PENDING_DA = "PPDA";
        public const string DOCUMENT_POST_PENDING_FA = "PPFA";
        public const string DOCUMENT_POST_PENDING_SUBMIT = "PPPS";
        public const string DOCUMENT_POST_REJECTED_BD = "PRBD";
        public const string DOCUMENT_POST_REJECTED_DR = "PRDR";
        public const string DOCUMENT_CANCELED = "X";
        /// <summary>
        /// 最大的软件比较使用日期 
        /// Defalut = 3000-01-01
        /// </summary>
        public const string SystemCompareMaxUseDate= "3000-01-01";

        /// <summary>
        /// 系统开始使用的默认时间
        /// Default  = 2000-01-01
        /// </summary>
        public const string SystemStartUseDate = "2000-01-01";


        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public const string DateFormat = "yyyy-MM-dd";
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        public const string DateLongFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// #,###,##0.00
        /// </summary>
        public const string CurrencyFormat = "#,###,##0.00";

        /// <summary>
        /// #,###,###.##
        /// </summary>
        public const string DecimalFormat = "#,###,###.##";

        /// <summary>
        /// 用于时间显示的格式 #,###,##0.00
        /// </summary>
        public const string HoursDisplayFormat = "#,###,##0.00";

        /// <summary>
        /// 用于时间显示的格式 #,###,##0.000
        /// </summary>
        public const string TakenDaysDisplayFormat = "#,###,##0.000";

        /// <summary>
        /// 用于天数显示的格式 ##0.00
        /// 有半天0.50,最大4位数
        /// </summary>
        public const string DaysDisplayFormat = "##0.00";

        /// <summary>
        /// 用于Int类型的格式
        /// </summary>
        public const string IntDisplayFormat = "#0";

        /// <summary>
        /// HH:mm:ss -- for sql
        /// </summary>
        public const string TimeFormat = "HH:mm:ss";

        /// <summary>
        /// H:mm:ss
        /// </summary>
        public const string TimeFormat12 = "H:mm:ss";

        /// <summary>
        /// HH:mm
        /// </summary>
        public const string ShortTimeFormat = "HH:mm";
        public const string DateTimeFormat = DateFormat + " " + TimeFormat;
        public const string DateTimeFormat12 = DateFormat + " " + TimeFormat12;
        public const string DateShortTimeFormat = DateFormat + " " + ShortTimeFormat;
        //public const string StringSeperator = "#;";
        //public const string BaanTablePreSpace = "         ";

        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public const string DbDateFormat = DateFormat;

        /// <summary>
        /// yyyy-MM-dd  HH:mm:ss
        /// </summary>
        public const string DbDateTimeFormat = DateLongFormat;

        DateTime DefaultConstsSys1901Time = new DateTime(1901, 1, 1, 0, 0, 0);
        /// <summary>
        /// 1901-01-01 00:00:00 
        /// </summary>
        public const string DefaultSysDBtime = "1901-01-01 00:00:00 ";

        /// <summary>
        /// DateTimeFormat12 + " tt";
        /// </summary>
        public const string DefaultDateTimeFormat = DateTimeFormat12 + " tt";
        public const string MaxTime = " 23:59:59";

        public const string Day = "D";
        public const string Hour = "H";
        public const string Minute = "M";
        public const string Second = "S";
        public const string Year = "Y";
        public const string Month = "MM";

        public const string ButtonDetail = "detail";
        public const string ButtonBack = "back";
        public const string ButtonSearch = "search";
        public const string ButtonRefresh = "refresh";
        public const string ButtonAddNew = "new";
        public const string ButtonEdit = "edit";
        public const string ButtonSave = "save";
        public const string ButtonSubmit = "submit";
        public const string ButtonExport = "export";
        public const string ButtonDelete = "delete";
        public const string ButtonPrint = "print";
        public const string ButtonCancel = "cancel";
        public const string ButtonImport = "import";
        public const string ButtonApply = "apply";

        public const string HiddenInputCommand = "_HRCommand"; // store current command
        public const string HiddenInputSave = "_HRSave"; // whether save clicked
        public const string HiddenInputChange = "_HRChange"; // whether data changed
        public const string HiddenInputWinRetVal = "_HRWinRetVal"; // store window.returnValue


        public static string DropDownAll
        {
            get
            {
                return HttpContext.GetGlobalResourceObject(ResourcesLabels, "ALLL").ToString();
            }
        }// "-All-";
        public static string DropDownNone
        {
            get
            {
                return HttpContext.GetGlobalResourceObject(ResourcesLabels, "None").ToString();
            }
        }//"-None-";
        public const string DropDownAllValue = "-1";
        public const string DropDownNoneValue = "-2";
        public const int DropDownOptionAll = 0x1;
        public const int DropDownOptionNone = 0x2;
        public const int DropDownOptionSpecial = 0x4;

        public const string BasicModule = "BASIC";
        public const string PersonModule = "PERSON";
        public const string AttendanceModule = "ATTENDANCE";
        public const string PayrollModule = "PAYROLL";
        public const string LeaveModule = "LEAVE";
        public const string ReportModule = "REPORT";
        public const string SecurityModule = "SECURITY";
        public const string StatisticsModule = "STATISTICS";
        public const string LinkModule = "LINK";
        public const string Staff = "STAFF";
        public const string StaffInformation = "STAFFINFORMATION";
        public const string TrainingModule = "TRAINING";

        public const string ToolsModule = "TOOLS";


        public const string BasicModuleID = "3";
        public const string PersonModuleID = "5";
        public const string AttendanceModuleID = "6";
        public const string PayrollModuleID = "2";
        public const string LeaveModuleID = "7";
        public const string ReportModuleID = "8";
        public const string SecurityModuleID = "4";
        public const string StatisticsModuleID = "10";
        public const string StaffModuleID = "13";
        public const string TrainingModuleID = "14";


        //Table Start
        //Common
        public const string TableID = "CM_ID";

               //Security
        public const string TableScGroup = "SC_GROUP";
        public const string TableScGroupUser = "SC_GROUPUSER";
        public const string TableScAuditLog = "SC_LOG";
        public const string TableScRight = "SC_RIGHT";
        public const string TableScFunction = "SC_FUNCTION";
        public const string TableScUser = "SC_USER";
        public const string TableScModule = "SC_MODULE";              
        public const string TableScUserAccessLevel = "SC_USERACCESSLEVEL";
        public const string TableScRole = "SC_ROLE";
        public const string TableCustomizingField = "BS_CUSTOMIZINGFIELD";
        public const string TableCustomizingData = "BS_CUSTOMIZINGDATA";

        public const string TableDownloadManager = "DlnMgr";
        //Table End

        //View Start
        public const string ViewScUserAccessLevel = "V_SC_USERACCESSLEVEL";
        public const string ViewScRight = "V_SC_RIGHT";
        
        //View End


        public const string ScDropdownSetting = "SCDROPDOWNSETTING";
        public const string ScMandatorySetting = "SCMANDATORYSETTING";
        public const string ScFunctionFunc = "SCFUNCTION";
        public const string ScGroupFunc = "SCGROUP";
        public const string ScGroupRightFunc = "SCGROUPRIGHT";
        public const string ScLogFunc = "SCACCESSLOG";
        public const string ScModuleFunc = "SCMODULE";
        public const string ScUserFunc = "SCUSER";
        public const string ScUserRightFunc = "SCUSERRIGHT";
        public const string ScGroupFilterFunc = "SCGROUPFILTER";
        public const string ScUserFilterFunc = "SCUSERFILTER";
        public const string ScAuditTrailFunc = "SCAUDITLOG";
        public const string ScSystemParametersFunc = "SCSYSTEMPARA";
        public const string ScBackupPathFunc = "SCBACKUPPATH";
        public const string ScHousekeepingFunc = "SCHOUSEKEEPING";
        public const string ScHardCodedItemFunc = "SCHARDCODEDITEMLIST";
        public const string ScUserFunctionFunc = "SCUSERFUNCTION";

        public const string FieldID = "ID";
        public const string ModuleSecurityID = "6";
        public const string FieldPID = "PID";
        public const string FieldCode = "CODE";
        public const string FieldDescription = "DESCRIPTION";
        public const string FieldName = "NAME";
        public const string FieldCreated = "CREATED";
        public const string FieldCreatedBy = "CREATEDBY";
        public const string FieldModified = "MODIFIED";
        public const string FieldModifiedBy = "MODIFIEDBY";
        public const string FieldSearchProj = "PrjCode";
        public const string FieldProject = "PrjCode";
        //public const string FieldProjectName = "PrjName1";
        public const string FieldProjectName = "PrjName";
        public const string FieldProjectDesc = "PrjDesc";

        public const string QueryStringID = "id";
        public const string QueryStringPID = "pid";
        public const string QueryStringMode = "mode";

        public const string ResourcesCommon = "Common";
        public const string ResourcesLabels = "Labels";
        public const string ResourcesMessages = "Messages";
        public const string ResourcesReportLabels = "ReportLabels";

        public const string ResourcesTemplateInfo = "TemplateInfo";
        public const string ResourcesUPLOADPATH = "UPLOADPATH";

        public const string Table_DocumentProperty = "DocumentProperty";
        public const string Table_DocumentMessage = "DocumentMessage";
        public const string Table_SCFunction = "SC_FUNCTION";
        public const string Table_SCRole = "SC_Role";




    }//end of class
}