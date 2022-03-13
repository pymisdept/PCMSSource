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
using PCCore;
using SimpleControls.Web;

public partial class NavMenu : System.Web.UI.UserControl
{
    protected override void OnInit(EventArgs e)
    {
        //SLB.DefaultItemImageUrl = "../App_Themes/Default/images/arr_li.gif";
        string cssLink = null;
        switch (SessionInfo.CurrentLanguage.ToUpper())
        {
            case "EN-US":
            case "JA-JP":
                cssLink = SimpleWebUtils.GetCssLink(Config.AppBaseUrl + "/control/navmenu.css");
                break;
            default:
                cssLink = SimpleWebUtils.GetCssLink(Config.AppBaseUrl + "/control/navmenu-zh.css");
                break;
        }
        LiteralControl css = new LiteralControl(cssLink);
        css.ID = "navcssID";
        Page.Header.Controls.Add(css);
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        ModuleEnable();

        // Karrson: TreeView Menu
        //if (!IsPostBack)
        //{
        string sql = string.Format("SELECT * FROM SC_FUNCTION func inner join SC_Module mo on mo.id = func.mid and mid = {0}", SessionInfo.CurrentModuleID);

        //foreach (DataRow dr1 in dt.Rows)
        //{

        //    if (Convert.ToString(dr1["Father"]).Equals(""))
        //    {
        //        string c1 = Convert.ToString(dr1["Code"]);
        //        TreeNode groupNode = new TreeNode(c1);
        //        groupNode.ShowCheckBox = false;
        //        loop(dt, c1, groupNode);
        //        treeMain.Nodes.Add(groupNode);
        //        treeMain.CollapseAll();
        //        //DataRow [] dt1  = dt.Select("Father ='' and Code ='"+c1+"'");



        //        //groupNode.ShowCheckBox = false;
        //        //foreach (DataRow dr2 in dt1)
        //        //{
        //        //    string c2 = Convert.ToString (dr2["Code"]);

        //        //    DataRow[] dt2 = dt.Select("Father ='"+c2+"'");
        //        //    if (dt2.Length  > 0)
        //        //    {
        //        //        TreeNode subheadingNode = new TreeNode(c2);
        //        //        subheadingNode.ShowCheckBox = false;
        //        //        foreach (DataRow dr3 in dt2)
        //        //        {

        //        //            string c3 = Convert.ToString(dr3["Code"]);
        //        //            TreeNode productNode = new TreeNode(c3);
        //        //            productNode.NavigateUrl = "http://google.com";
        //        //            productNode.ShowCheckBox = false;

        //        //            subheadingNode.ChildNodes.Add(productNode);


        //        //        }
        //        //        groupNode.ChildNodes.Add(subheadingNode);
        //        //    }



        //        //}

        //        //treeMain.Nodes.Add(groupNode);
        //        //treeMain.CollapseAll();





        //    }
        //}
                
            
  
//}   
        
    }

    protected void ModuleEnable()
    {
        //SLB.Visible = false;
    }

    void setNavMenu()
    {
        DataTable _dtNavMenu = PCCore.Database.ValidationList.FunctionList(Convert.ToInt32(SessionInfo.CurrentModuleID));
        if (_dtNavMenu != null)
        {
            // Function Name by Multi-Language
            foreach (DataRow _dr in _dtNavMenu.Rows)
            {
                // Point: Multi-Language 
                switch (SessionInfo.CurrentLanguage)
                {
                    case "en-us":
                        _dr["Title"] = GetGlobalResourceObject(Consts.ResourcesLabels, _dr["Code"].ToString());
                        break;
                    case "zh-tw":
                        _dr["Title"] = GetGlobalResourceObject(Consts.ResourcesLabels, _dr["Code"].ToString());
                        break;
                    case "zh-cn":
                        _dr["Title"] = GetGlobalResourceObject(Consts.ResourcesLabels, _dr["Code"].ToString());
                        break;

                }
                
                
            }
        }

    }
}
