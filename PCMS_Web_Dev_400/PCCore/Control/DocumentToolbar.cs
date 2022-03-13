using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace PCCore
{
    [ToolboxData("<{0}:ToolBar runat=server></{0}:ToolBar>")]
    [ToolboxBitmap(typeof(Button))]
    public class DocumentToolBar : WebControl
    {
        
        protected bool _buttonRefreshVisible = true;
        //protected bool _buttonSearchVisible = true;
        protected bool _buttonExportVisible = true;
        protected bool _buttonNewVisible = true;
        protected bool _buttonEditVisible = true;
        protected bool _buttonDeleteVisible = true;
        //protected bool _buttonSaveVisible = false;
        protected bool _buttonSaveVisible = true;

        protected bool _buttonBackVisible = false;
        protected bool _buttonPrintVisible = false;
        protected bool _buttonBatchVisible = false;
        protected bool _buttonViewVisible = false;
        protected bool _buttonSubmitVisible = false;
        protected bool _buttonDetailVisible = false;
        protected bool _buttonImportVisible = false;

        ImageButton _ibDetail = null;
        ImageButton _ibRefresh = null;
        ImageButton _ibNew = null;
        ImageButton _ibEdit = null;
        ImageButton _ibDelete = null;
        ImageButton _ibSave = null;
        ImageButton _ibPrint = null;//print
        ImageButton _ibExport = null;
        ImageButton _ibBack = null;
        ImageButton _ibRoster = null;
        ImageButton _ibView = null;
        ImageButton _ibSubmit = null;
        ImageButton _ibImport = null;

        public DocumentToolBar()
        {

        }

        [Bindable(true), Category("Appearance"), DefaultValue(true),
        Description("Gets or sets Visible of Print button")]
        public bool ButtonPrintVisible
        {
            get { return _buttonPrintVisible; }
            set { _buttonPrintVisible = value; }
        }

        [Bindable(true), Category("Appearance"), DefaultValue(true),
        Description("Gets or sets Visible of Detail button")]
        public bool ButtonDetailVisible
        {
            get { return _buttonDetailVisible; }
            set { _buttonDetailVisible = value; }
        }

        [Bindable(true), Category("Appearance"), DefaultValue(true),
        Description("Gets or sets Visible of Back button")]
        public bool ButtonBackVisible
        {
            get { return _buttonBackVisible; }
            set { _buttonBackVisible = value; }
        }

        [Bindable(true), Category("Appearance"), DefaultValue(true),
        Description("Gets or sets Visible of Save button")]
        public bool ButtonSaveVisible
        {
            get { return _buttonSaveVisible; }
            set { _buttonSaveVisible = value; }
        }

        [Bindable(true), Category("Appearance"), DefaultValue(true),
        Description("Gets or sets Visible of Delete button")]
        public bool ButtonDeleteVisible
        {
            get { return _buttonDeleteVisible; }
            set { _buttonDeleteVisible = value; }
        }

        [Bindable(true), Category("Appearance"), DefaultValue(true),
        Description("Gets or sets Visible of Edit button")]
        public bool ButtonEditVisible
        {
            get { return _buttonEditVisible; }
            set { _buttonEditVisible = value; }
        }

        [Bindable(true), Category("Appearance"), DefaultValue(true),
        Description("Gets or sets Visible of New button")]
        public bool ButtonNewVisible
        {
            get { return _buttonNewVisible; }
            set { _buttonNewVisible = value; }
        }

        [Bindable(true), Category("Appearance"), DefaultValue(true),
        Description("Gets or sets Visible of New button")]
        public bool ButtonExportVisible
        {
            get { return _buttonExportVisible; }
            set { _buttonExportVisible = value; }
        }

        [Bindable(true), Category("Appearance"), DefaultValue(true),
        Description("Gets or sets Visible of Refresh button")]
        public bool ButtonRefreshVisible
        {
            get { return _buttonRefreshVisible; }
            set { _buttonRefreshVisible = value; }
        }
        [Bindable(true), Category("Appearance"), DefaultValue(true),
        Description("Gets or sets Visible of RosterBatch button")]
        public bool ButtonBatchVisible
        {
            get { return _buttonBatchVisible; }
            set { _buttonBatchVisible = value; }
        }
        [Bindable(true), Category("Appearance"), DefaultValue(true),
        Description("Gets or sets Visible of ViewDetail button")]
        public bool ButtonViewVisible
        {
            get { return _buttonViewVisible; }
            set { _buttonViewVisible = value; }
        }
        [Bindable(true), Category("Appearance"), DefaultValue(true),
        Description("Gets or sets Visible of ViewDetail button")]
        public bool ButtonSubmitVisible
        {
            get { return _buttonSubmitVisible; }
            set { _buttonSubmitVisible = value; }
        }
        [Bindable(true), Category("Appearance"), DefaultValue(true),
       Description("Gets or sets Visible of ViewImport button")]
        public bool ButtonImportVisible
        {
            get { return _buttonImportVisible; }
            set { _buttonImportVisible = value; }
        }

        public ImageButton SubmitButton
        {
            get { return _ibSubmit; }
        }
        //[Bindable(true), Category("Appearance"), DefaultValue(true),
        //Description("Gets or sets Visible of Search button")]
        //public bool ButtonSearchVisible
        //{
        //    get { return _buttonSearchVisible; }
        //    set { _buttonSearchVisible = value; }
        //}

        string _imageBaseUrl = null;
        protected string GetButton(string function, string title, string image)
        {
            if(_imageBaseUrl==null) 
                _imageBaseUrl = Config.GetImageBaseUrl(Page.Theme);

            return String.Format("<A onclick=\"{0}\" href=\"#\"><IMG class='ToolbarImage' title=\"{1}\" src=\"{2}/{3}\"></A>", function, title, _imageBaseUrl, image);
        }



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _imageBaseUrl = Config.GetImageBaseUrl(Page.Theme);

            _ibDetail = new ImageButton();
            _ibDetail.CausesValidation = false;            
            _ibDetail.ID = Consts.ButtonDetail;
            _ibDetail.ImageUrl = _imageBaseUrl + "/detail.gif";
            _ibDetail.ToolTip = HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "Detail").ToString();
            _ibDetail.CssClass = "ToolbarImage";
            _ibDetail.OnClientClick = "if(!Detail()) return false;";
            _ibDetail.Visible = false;

            _ibRefresh = new ImageButton();
            _ibRefresh.CausesValidation = false;
            _ibRefresh.ID = Consts.ButtonRefresh;
            _ibRefresh.ImageUrl = _imageBaseUrl + "/refresh.gif";
            _ibRefresh.ToolTip = HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "Refresh").ToString();
            _ibRefresh.CssClass = "ToolbarImage";
            _ibRefresh.OnClientClick = "if(!Refresh()) return false;";
            _ibRefresh.Visible = true;

            _ibNew = new ImageButton();
            _ibNew.CausesValidation = false;
            _ibNew.ID = Consts.ButtonAddNew;
            //_ibNew.ImageUrl = _imageBaseUrl + "/new.gif";
            _ibNew.ImageUrl = _imageBaseUrl + "/add-icon.jpg";
            //_ibNew.ToolTip = HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "AddNew").ToString();
            _ibNew.ToolTip = HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "New").ToString();
            _ibNew.CssClass = "ToolbarImage";
            _ibNew.OnClientClick = "if(!AddNew()) return false;";
            //_ibNew.OnClientClick = "alert(" + "\"a\"" + "); ";
            _ibNew.Visible = false;

            _ibEdit = new ImageButton();
            _ibEdit.CausesValidation = false;
            _ibEdit.ID = Consts.ButtonEdit;
            _ibEdit.ImageUrl = _imageBaseUrl + "/edit.gif";
            _ibEdit.ToolTip = HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "Edit").ToString();
            _ibEdit.CssClass = "ToolbarImage";
            _ibEdit.OnClientClick = "if(!Edit()) return false;";
            _ibEdit.Visible = false;
            

            _ibDelete = new ImageButton();
            _ibDelete.CausesValidation = false;
            _ibDelete.ID = Consts.ButtonDelete;
            _ibDelete.ImageUrl = _imageBaseUrl + "/delete.gif";
            _ibDelete.ToolTip = HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "Delete").ToString();
            _ibDelete.CssClass = "ToolbarImage";
            _ibDelete.OnClientClick = "if(!Delete()) return false;";
            _ibDelete.Visible = false;
            

            _ibSave = new ImageButton();
            _ibSave.CausesValidation = true;
            _ibSave.ID = Consts.ButtonSave;
            _ibSave.ImageUrl = _imageBaseUrl + "/save.gif";
            _ibSave.ToolTip = HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "Save").ToString();
            _ibSave.CssClass = "ToolbarImage";
            _ibSave.OnClientClick = "if (typeof(Save)=='function'){if(!Save()) return false; }else{ return false;}";
            _ibSave.Visible = false;
            

            _ibPrint = new ImageButton();
            _ibPrint.CausesValidation = false;
            _ibPrint.ID = Consts.ButtonPrint;
            _ibPrint.ImageUrl = _imageBaseUrl + "/print.gif";
            _ibPrint.ToolTip = HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "Print").ToString();
            _ibPrint.CssClass = "ToolbarImage";
            _ibPrint.OnClientClick = "if(!Print()) return false;";
            _ibPrint.Visible = false;


            _ibExport = new ImageButton();
            _ibExport.CausesValidation = false;
            _ibExport.ID = Consts.ButtonExport;
            //_ibExport.ImageUrl = _imageBaseUrl + "/export.gif";
            _ibExport.ImageUrl = _imageBaseUrl + "/ICXLS.GIF";
            _ibExport.ToolTip = HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "Export").ToString();
            _ibExport.CssClass = "ToolbarImage";
            _ibExport.OnClientClick = "if(!Export()) return false;";
            _ibExport.Visible = true;

            _ibBack = new ImageButton();
            _ibBack.CausesValidation = false;
            _ibBack.ID = Consts.ButtonBack;
            _ibBack.ImageUrl = _imageBaseUrl + "/back.gif";
            _ibBack.ToolTip = HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "Back").ToString();
            _ibBack.CssClass = "ToolbarImage";
            _ibBack.OnClientClick = "if(!Back()) return false;";
            _ibBack.Visible = false;

            _ibRoster = new ImageButton();
            _ibRoster.CausesValidation = false;
            _ibRoster.ID = "ibtnRosterBatch";
            _ibRoster.ImageUrl = _imageBaseUrl + "/ico_Roster_Batch.gif";
            _ibRoster.ToolTip = HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "BatchSetting").ToString();
            _ibRoster.CssClass = "ToolbarImage";
            _ibRoster.OnClientClick = "if(!BatchSetting()) return false;";
            _ibRoster.Visible = false;

            _ibView = new ImageButton();
            _ibView.CausesValidation = false;
            _ibView.ID = "ibtntbView";
            _ibView.ImageUrl = _imageBaseUrl + "/View.gif";
            _ibView.ToolTip = HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "View").ToString();
            _ibView.CssClass = "ToolbarImage";
            _ibView.OnClientClick = "if(!tbView()) return false;";
            _ibView.Visible = false;

            _ibSubmit = new ImageButton();
            _ibSubmit.CausesValidation = false;
            _ibSubmit.ID = "ibtntbSubmit";// ImageButton + ToolBar  ==>> ibtn +tb + same name
            _ibSubmit.ImageUrl = _imageBaseUrl + "/SubmitICO.gif";
            _ibSubmit.ToolTip = HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "Submit").ToString();
            _ibSubmit.CssClass = "ToolbarImage";
            _ibSubmit.OnClientClick = "if(!tbSubmit()) return false;";
            _ibSubmit.Visible = false;

            _ibImport = new ImageButton();
            _ibImport.CausesValidation = false;
            _ibImport.ID = "ibtntbImport";// ImageButton + ToolBar  ==>> ibtn +tb + same name
            _ibImport.ImageUrl = _imageBaseUrl + "/import.gif";
            _ibImport.ToolTip = HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "Import").ToString();
            _ibImport.CssClass = "ToolbarImage";
            _ibImport.OnClientClick = "if(!Import()) return false;";
            _ibImport.Visible = false;


            Controls.Add(_ibDetail);
            Controls.Add(_ibRefresh);
            Controls.Add(_ibNew);
            Controls.Add(_ibEdit);
            Controls.Add(_ibDelete);
            Controls.Add(_ibSave);
            Controls.Add(_ibPrint);
            Controls.Add(_ibExport);
            Controls.Add(_ibBack);
            Controls.Add(_ibRoster);
            Controls.Add(_ibView);
            Controls.Add(_ibSubmit);
            Controls.Add(_ibImport);

        }

        protected override void Render(HtmlTextWriter writer)
        {
            ScInfo scInfo = null;
            BasePage bp = this.Page as BasePage;
            if(bp != null) 
                scInfo = bp.SecurityInfo;

            #region Old
            //StringBuilder sb = new StringBuilder();

            //if (scInfo == null || scInfo.CanQuery)
            //{
            //    if (_buttonRefreshVisible)
            //        sb.Append(GetButton("Refresh();", HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "Refresh").ToString(), "refresh.gif"));
            //    //if (_buttonSearchVisible)
            //    //    sb.Append(GetButton("Search();", "Search", "search.gif"));
            //}
            //if (scInfo == null || scInfo.CanNew)
            //{
            //    if (_buttonNewVisible)
            //        sb.Append(GetButton("AddNew();", HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "AddNew").ToString(), "new.gif"));
            //}
            //if (scInfo == null || scInfo.CanEdit)
            //{
            //    if (_buttonEditVisible)
            //        sb.Append(GetButton("Edit();", HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon,"Edit").ToString(), "edit.gif"));
            //}
            //if (scInfo == null || scInfo.CanDelete)
            //{
            //    if (_buttonDeleteVisible)
            //        sb.Append(GetButton("Delete();", HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon,"Delete").ToString(), "delete.gif"));
            //}
            //if (scInfo == null || scInfo.CanEdit || scInfo.CanDelete) // why not scInfo.CanSave? cause New not need to save in grid now
            //{
            //    if (_buttonSaveVisible)
            //    {
            //        //_ibSave.Visible = false;
            //    }
            //        //sb.Append(GetButton("Save();", HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "Save").ToString(), "save.gif"));
            //}
            //if (scInfo == null || scInfo.CanQuery)
            //{
            //    if (_buttonExportVisible)
            //        sb.Append(GetButton("Export();", HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "Export").ToString(), "export.gif"));
            //}

            //writer.Write(sb.ToString());
            #endregion Old

            
            if (scInfo == null || scInfo.CanDetail)
            {
                if (_buttonDetailVisible)
                {
                    _ibDetail.Visible = true;
                }
                else
                {
                    _ibDetail.Visible = false;
                }

            }

            if (scInfo == null || scInfo.CanQuery)
            {
                if (_buttonRefreshVisible)
                {
                    _ibRefresh.Visible = true;

                }
                else 
                {
                    _ibRefresh.Visible = false;
                } 
            }

            if (scInfo == null || scInfo.CanNew)
            {
                if (_buttonNewVisible)
                {
                    _ibNew.Visible = true;
                }
                else
                {
                    _ibNew.Visible = false;
                }
            }

            if (scInfo == null || scInfo.CanEdit)
            {
                if (_buttonEditVisible)
                {
                    _ibEdit.Visible = true;
                }
                else
                {
                    _ibEdit.Visible = false;
                }
            }

            if (scInfo == null || scInfo.CanDelete)
            {
                if (_buttonDeleteVisible)
                {
                    _ibDelete.Visible = true;
                }
                else
                {
                    _ibDelete.Visible = false;
                }
            }
            //// karrson: Can Save Re-Use for save the data to draft
            //if (scInfo == null || scInfo.CanSave)
            //{
            //    if (_buttonSaveVisible)
            //    {
            //        _ibSave.Visible = true;
            //    }
            //    else
            //    {
            //        _ibSave.Visible = false;
            //    }
            //}
            if (scInfo == null || scInfo.CanEdit) // why not scInfo.CanSave? cause New not need to save in grid now
            {
                if (_buttonSaveVisible)
                {
                    _ibSave.Visible = true;
                }
                else
                {
                    _ibSave.Visible = false;
                }
            }

            if (scInfo == null || scInfo.CanQuery)
            {
                if (_buttonPrintVisible)
                {
                    _ibPrint.Visible = true;

                }
                else
                {
                    _ibPrint.Visible = false;
                }
            }

            if (scInfo == null || scInfo.CanQuery)
            {
                if (_buttonExportVisible)
                {
                    _ibExport.Visible = true;
                }
                else
                {
                    _ibExport.Visible = false;
                }
            }

            if (scInfo == null || scInfo.CanBack)
            {
                if (_buttonBackVisible)
                {
                    _ibBack.Visible = true;
                }
                else
                {
                    _ibBack.Visible = false;
                }

            }

            if (scInfo == null || scInfo.CanQuery)
            {
                if (_buttonBatchVisible)
                {
                    _ibRoster.Visible = true;
                }
                else
                {
                    _ibRoster.Visible = false;
                }
            }

            if (scInfo == null || scInfo.CanQuery)
            {
                if (_buttonViewVisible)
                {
                    _ibView.Visible = true;
                }
                else
                {
                    _ibView.Visible = false;
                }
            }

            if (scInfo == null || scInfo.CanQuery)
            {
                if (_buttonSubmitVisible)
                {
                    _ibSubmit.Visible = true;
                }
                else
                {
                    _ibSubmit.Visible = false;
                }
            }
            if (scInfo == null || scInfo.CanQuery)
            {
                if (_buttonImportVisible)
                {
                    _ibImport.Visible = true;
                }
                else
                {
                    _ibImport.Visible = false;
                }
            }
            // force to disable delete, save and edit button
            _ibDelete.Visible = false;
            _ibEdit.Visible = false;
            if (SessionInfo.CurrentModuleID != "6")
                _ibSave.Visible = false;
            else
                _ibSave.Visible = true;
            
            //_ibSave.Visible = false;
            //_ibNew.Visible = false;

            if (!PCCore.PCMS.Authorization.PageAllowAdd(SessionInfo.CurrentFunctionID))
                _ibNew.Visible = false;
            else
                _ibNew.Visible = true;

            this.RenderChildren(writer);
        }

    }
}