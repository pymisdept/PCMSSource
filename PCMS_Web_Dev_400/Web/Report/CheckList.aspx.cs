using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using PCCore;
using SimpleControls.Web;

public partial class CheckList : BasePage
{
    const string TABLE_NAME = "DocumentProperty";
    const string FILE_TYPE = "2002";
    String DocEntry = "";
    int filetype = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ClearError();

        if (Page.IsPostBack)
        {
            DocEntry = Page.Request.QueryString["ID"] as string;
            string cmd = this.CurrentCommand;
            switch (cmd)
            {
                case Consts.ButtonDelete:
                    
                    break;
            }
        }
        else
        {
            DocEntry = Page.Request.QueryString["ID"] as string;
            
        }

        gvData.HiddenFields += "ID,ErrCode";
        gvData.HeaderDescriptions = "ID,#,Worksheet,Section,Line Num,Field,Description,Value,Msg";

        PCCore.Database.DocumentProperty _doc = new PCCore.Database.DocumentProperty(DocEntry);
        PCCore.Database.SC_Function _sc_func = new PCCore.Database.SC_Function(_doc.FunctionCode());
        lblTitle.Text = GetGlobalResourceObject(Consts.ResourcesLabels, _sc_func.FunctionCode()) + " " + GetGlobalResourceObject(Consts.ResourcesLabels, "CheckList");
        if (this.CurrentCommand != Consts.ButtonExport)
            SetDataSource();


    }//end of Page_Load

    protected void SetDataSource()
    {
        
        //StringBuilder sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,b.filesize,a.description,c.loginname,convert(varchar(20),b.uploadtime,120) as uploadtime,a.projectCode from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
        //StringBuilder sb = new StringBuilder(string.Format("select * from CPS_View_DocumentMessage a where a.ID={0} order by {1}",ID,"LineNum asc,xlWkSheet asc,xlLineNum asc"));
        PCCore.Common.HRLog.RecordLog(DocEntry);
        StringBuilder sb = new StringBuilder(string.Format("select * from CPS_View_DocumentMessage a where a.ID={0}",DocEntry));
        PCCore.Common.HRLog.RecordLog(sb.ToString());
        
        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sb.ToString();
        dsGridView.ErrorHandler = this.Master;
    }


    protected void btnClose_Click(object sender, EventArgs e)
    {

    }
}
