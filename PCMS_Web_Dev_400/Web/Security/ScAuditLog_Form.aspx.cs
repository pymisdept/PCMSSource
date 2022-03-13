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
using System.Text;
using PCCore;
using SimpleControls.SimpleDatabase;

public partial class ScAuditLog_Form : BasePage 
{

    protected override void OnInit(EventArgs e)
    {
        Master.ButtonSave.Visible = false;
        Master.ButtonClose.Visible = false;
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        
        if(Request.QueryString["strContent"]!=null)
            txt1.Text = Request.QueryString["strContent"].ToString().Replace("$", "\n");

        if (Request.QueryString["funcname"] != null)
        {
            sb.AppendFormat(Resources.Labels.Function + " : {0}; ",Request.QueryString["funcname"].ToString());
        }
        if (Request.QueryString["action"] != null)
        {
            sb.AppendFormat(Resources.Labels.Action + " : {0} ", Request.QueryString["action"].ToString());
        }
        Master.MasterNote.Text = sb.ToString();
    }



}//end of class
