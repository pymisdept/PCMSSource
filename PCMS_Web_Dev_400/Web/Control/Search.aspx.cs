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

public partial class _Search : SearchBasePage
{
    

    protected void Page_Init(object sender, EventArgs e)
    {
        PCCore.Common.HRLog.RecordLog("Page_Init");
        //ClientScriptManager cs = this.ClientScript;
        //Type t = this.GetType();
        //SimpleJS.RegisterClientScripts(this.Page, SimpleJS.JSTypes.Common);
        //cs.RegisterClientScriptInclude(t, "common", Config.AppBaseUrl + "/common.js");

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PCCore.Common.HRLog.RecordLog("Page_Load");
        if (Page.IsPostBack)
        {
            
            //switch (cmd)
            //{
            //    case Consts.ButtonDelete:
            //        DeleteRecord();
            //        break;
            //    case Consts.Confirm:
            //        ConfirmRecord();
            //        break;

            //}
        } else {
        }
        
    }//end of Page_Load

    
}
