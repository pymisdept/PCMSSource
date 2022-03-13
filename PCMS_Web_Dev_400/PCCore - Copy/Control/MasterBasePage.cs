using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SimpleControls;

namespace PCCore
{
    //Note: nested master page is not recommended here, there will be problems.

    public class MasterBasePage : System.Web.UI.MasterPage, IErrorHandler
    {
        const string FIRST_ENTER = "FirstEnter";

        #region Resource
        Type _t = typeof(MasterBasePage);
        #endregion Resource

        public bool IsFirstEnter
        {
            get { return SimpleUtils.IfNull(ViewState[FIRST_ENTER], true); }
            set { ViewState[FIRST_ENTER] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Page.Header.Title = String.Format("{0} {1}", Consts.AppName,
                String.IsNullOrEmpty(SessionInfo.Database) ? "" : String.Format("({0})", SessionInfo.Database));

            RegisterClientScripts();
        }

        private ScInfo _scInfo = null;
        
        public ScInfo SecurityInfo
        {
            
            get
            {
                if (_scInfo == null)
                {
                    InitSecurity();
                }
                return _scInfo;
            }
        }

        private void InitSecurity()
        {
            if (this.ContentPlaceHolders.Count < 1) return;
            string id = this.ContentPlaceHolders[0].ToString();
            object o = this.FindControl(id);
            if (o == null) return;
            ContentPlaceHolder cph = o as ContentPlaceHolder;
            BasePage bp = cph.Page as BasePage;
            if (bp == null) return;
            _scInfo = bp.SecurityInfo;
            
            //if (_scInfo == null) return;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e); // this trigger all children's page load event

            if (IsFirstEnter)
            {
                IsFirstEnter = false;
            }
        }

        protected void RegisterClientScripts()
        {


            //页面加载完后执行的代码,
            StringBuilder sbJaveXcript = new StringBuilder();
            sbJaveXcript.Append("document.onreadystatechange=MasterXXXToBeExecute;");
            sbJaveXcript.Append(" "
              + "  function MasterXXXToBeExecute() "
              + " {  "
              + "    if (document.readyState==\"complete\") "
              + "    { "
                // + "      // if (Menu != null) "
              + "       if ((typeof(Menu) !=\"undefined\") && (Menu != null)) "
              + "       { "
              + "         Menu.style.display=\"\"; "
              + "       } "
              + "      if (typeof(MasterBeExecute)==\"function\") "
              + "      { "
              + "            MasterBeExecute(); "
              + "      } "
              + "    } "
              + "  } ");



            Page.ClientScript.RegisterStartupScript(_t, this.ClientID,
                    sbJaveXcript.ToString(), true);


        }


        PCCore.Title _Title = null;
        public virtual PCCore.Title MasterTitle
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
            }
        }

        public virtual void ClearError()
        {

        }

        public virtual void HandleError(Exception ex)
        {

        }

        public virtual void ShowMessage(string message)
        {
            ShowMessage(message, null);
        }

        public virtual void ShowMessage(string message, string tooltip)
        {

        }

        public virtual void ShowWarning(string warning)
        {
            ShowWarning(warning, null);
        }

        public virtual void ShowWarning(string warning, string tooltip)
        {

        }

        public delegate void CheckDeletePrerequisite(PCTable table, Hashtable row);

        public virtual void DeleteRecord(string tableName, PCCore.GridView gridView, CheckDeletePrerequisite checkPrerequisite)
        {
        }
        public virtual void DeleteRecord(string tableName, PCCore.GridView gridView, CheckDeletePrerequisite checkPrerequisite, SimpleControls.Web.SimpleNote pNote)
        {
        }

        #region Personal
        protected bool sShowRightInformation = true;
        public virtual bool ShowRightInformation
        {
            get
            {
                return sShowRightInformation;
            }
            set
            {
                sShowRightInformation = value;
            }
        }
        #endregion

        #region Staff Login
        string _MasterIsShowNavMenu = "true";

        public string MasterIsShowNavMenus
        {
            set { _MasterIsShowNavMenu = value; }
            get { return _MasterIsShowNavMenu; }
        }

        bool _MasterIsShowBanner = true;
        public bool MasterIsShowBanners
        {
            set { _MasterIsShowBanner = value; }
            get { return _MasterIsShowBanner; }
        }

        string _MasterIsShowTitleBar = "true";
        public string MasterIsShowTitleBars
        {
            set { _MasterIsShowTitleBar = value; }
            get { return _MasterIsShowTitleBar; }
        }

        bool _isShowBottomFormButton = false;
        public bool ShowBottomFormButtons
        {
            get
            {
                return _isShowBottomFormButton;
            }
            set
            {
                _isShowBottomFormButton = value;
            }
        }
        

        #endregion

        public enum FormModes
        {
            New,
            Edit,
            Display,
            None
        }

        protected FormModes _formMode;
        public FormModes FormMode
        {
            get
            {
                return _formMode;
            }
            set
            {
                _formMode = value;
            }
        }


        protected string _StaffName = String.Empty;
        public virtual string StaffName
        {
            get
            {
                return _StaffName;
            }
            set
            {
                _StaffName = value;
            }
        }

        /// <summary>
        /// 显示模式，Normal = no tab.
        /// </summary>
        protected string _MasterDisplayModule = "Normal";
        public virtual string MasterDisplayModule
        {
            get { return _MasterDisplayModule; }
            set { _MasterDisplayModule = value; }

        }

        /// <summary>
        /// Clear TextBox input .
        /// </summary>
        public virtual void ClearForm()
        {

        }

        protected string _id;
        public string RecordID
        {
            get
            {
                return _id;
            }
            protected set
            {
                _id = value;
            }
        }

        protected string _pid;
        public string RecordPID
        {
            get
            {
                return _pid;
            }
            set
            {
                _pid = value;
            }
        }


        protected bool _formbuttonsaveenable = true;
        public virtual bool FormButtonSaveEnable
        {
            get
            {
                return _formbuttonsaveenable;
            }
            set
            {
                _formbuttonsaveenable = value;
            }
        }



        protected bool _ShowFormButton = true;
        public virtual bool ShowFormButton
        {
            get
            {
                return _ShowFormButton;
            }
            set
            {
                _ShowFormButton = value;
            }
        }

        public virtual void DisableForm()
        {
        }


        bool isShowlblStaffName = true;
        /// <summary>
        /// if (IsShowLblStaffName )
        /// {
        ///         lblStaffID.Visible = true;
        ///         lblStaffID.Text = SessionInfo.CurrentStaffName;// +SessionInfo.ChristianName;
        ///  }
        /// </summary>
        public bool IsShowLblStaffName
        {
            get
            {
                return isShowlblStaffName;
            }
            set
            {
                isShowlblStaffName = value;
            }
        }

    } //end of class
}
