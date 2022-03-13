using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using PCCore;

public partial class Control_loading : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string appUrl = Config.AppBaseUrl;
        string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
        string strShowMth = Convert.ToString(Request.QueryString["ShowMth"]);
        img1.ImageUrl = themeUrl + "/images/loading.gif";
        if (!Page.IsPostBack)
        {
            switch (strShowMth)
            {
                case "upload":
                    this.lbl1.Text = Convert.ToString(GetGlobalResourceObject(Consts.ResourcesLabels, "UploadProcessingMessage"));
                    break;
                case "download":
                    this.lbl1.Text = Convert.ToString(GetGlobalResourceObject(Consts.ResourcesLabels, "DownloadProcessingMessage"));
                    break;

            }
        }
    }
}
