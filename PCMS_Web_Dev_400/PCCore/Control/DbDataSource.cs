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
using SimpleControls.Web;

namespace PCCore
{
    [ToolboxData("<{0}:DbDataSource runat=server></{0}:DbDataSource>")]
    [ToolboxBitmap(typeof(Button))]
    public class DbDataSource : System.Web.UI.WebControls.SqlDataSource
    {
        public DbDataSource()
        {
            base.ProviderName = Config.DefaultProviderName;
            base.ConnectionString = Config.DefaultConnectionString;
            base.Selected += new SqlDataSourceStatusEventHandler(DbDataSource_Selected);
        }

        void DbDataSource_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (_errorHandler != null)
            {
                if (e.Exception != null)
                {
                    _errorHandler.HandleError(e.Exception);
                    e.ExceptionHandled = true;
                }
                //else
                //{
                //    _errorHandler.ClearError();
                //}
            }
        }

        #region ErrorHandler
        protected IErrorHandler _errorHandler = null;
        public IErrorHandler ErrorHandler
        {
            get { return _errorHandler; }
            set { _errorHandler = value; }
        }
        #endregion

        private bool _autoRestoreSelectCommand = true;
        [DefaultValue(true)]
        public bool AutoRestoreSelectCommand
        {
            get { return _autoRestoreSelectCommand; }
            set { _autoRestoreSelectCommand = value; }
        }

        private string GetSelectCommandClientID()
        {
            return this.ID + "_s";
        }

        protected void RegisterSelectCommand()
        {
            if (Page != null && !String.IsNullOrEmpty(this.SelectCommand))
            {
                Page.ClientScript.RegisterHiddenField(GetSelectCommandClientID(), Sc.Encode(this.SelectCommand));
            }
        }

        protected void RestoreSelectCommand()
        {
            if (Page != null && Page.IsPostBack && !String.IsNullOrEmpty(Page.Request.Form[GetSelectCommandClientID()]))
            {
                this.SelectCommand = Sc.Decode(Page.Request.Form[GetSelectCommandClientID()]);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (AutoRestoreSelectCommand)
                RestoreSelectCommand();
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            base.RenderControl(writer);

            RegisterSelectCommand();
        }

    } //end of class

}