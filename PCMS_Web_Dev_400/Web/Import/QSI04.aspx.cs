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

public partial class QSI04 : BasePage
{
   


    protected void Page_Load(object sender, EventArgs e)
    {
        
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
