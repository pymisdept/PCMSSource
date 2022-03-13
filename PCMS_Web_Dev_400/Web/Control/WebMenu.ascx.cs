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
using System.Drawing;
using SimpleControls.Web;
using HRMenu;


public partial class WebMenu : System.Web.UI.UserControl
{
    #region Resource
    Type _t = typeof(WebMenu);
    #endregion Resource

    protected override void OnInit(EventArgs e)
    {
        HRMenu.DynamicMenuStyle.BackColor = Color.Black;
        //HRMenu.Attributes["onclick"] = "javascript:alert('Hello World!')";
        
        
        base.OnInit(e);
        this.RegisterClientScripts();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
            if ((((PCCore.BasePage)(Page)).ShowWebMenu) && SessionInfo.IsLogin)
            {
                HRMenu.Visible = true;

                ModuleEnable();
            }
            HRMenu.Attributes["style"] = "display:none;";

            if (SessionInfo.CurrentLanguage == "en-us" || SessionInfo.CurrentLanguage == "ja-JP")
            {
                this.HRMenu.StaticMenuItemStyle.Font.Size = FontUnit.Point(9);
            }
            else
            {
                this.HRMenu.StaticMenuItemStyle.Font.Size = FontUnit.Point(11);
            }
        }
    }
    protected void RegisterClientScripts()
    {

        Page.ClientScript.RegisterStartupScript(_t, this.ClientID,
                String.Format("var {0}=document.getElementById('{1}');", this.ID, HRMenu.ClientID),
                true);

    }
    protected void ModuleEnable()
    {

        //if (!License.ModuleEnable(Consts.TrainingModuleID))
        //{
        //    //HRMenu.Items.RemoveAt(5);
        //}
        
    }


    protected void FunctionEnable()
    {
        //HRMenu.Items[0].ChildItems.Clear();

        //WebMenu_Helper Helper = new WebMenu_Helper();
        //Helper.SetMenu(3, HRMenu.Items);

    }


}


namespace HRMenu
{

    public class WebMenu_Helper
    {
        DataTable _dt = null;

        string _Text = string.Empty;
        string _Value = string.Empty;
        string _Mid = "0";
        string _NavigateUrl = string.Empty;
        string _ImageUrl = string.Format(@"/App_Themes/Default/images/arrow00.gif");
        string _Target = string.Format("0");


        private MenuItem GetMenuItem(string pText, string pImgeUrl, string pNavigateUrl)
        {
            MenuItem sMenuItem;
            sMenuItem = new MenuItem(pText, string.Empty, pImgeUrl, pNavigateUrl);
            return sMenuItem;
        }

        public void SetMenu(Int32 pMID, MenuItemCollection pMenu)
        {
            initDateSoure();

            DataView sDv = _dt.DefaultView;
            MenuItem sM = null;

            switch (pMID)
            {
                case 3://Basic

                    sDv.RowFilter = string.Format("mid ={0}", pMID);
                    for (int i = 0; i < sDv.Count; i++)
                    {
                        //Set Basic->Employment Group -> detail
                        if (i == 0)
                        {
                            _Text = Resources.Labels.Employment;
                            _NavigateUrl = string.Format(@"/basic/{0}.aspx?_slbg={1}&_slbi={2}", sDv[i]["code"], _Mid, i);
                            sM = GetMenuItem(_Text, _ImageUrl, _NavigateUrl);
                            pMenu[0].ChildItems.Add(sM);
                        }

                        _Text = ComFunction2.GetGlobalResourceObject_ByStringID(Consts.ResourcesLabels, sDv[i]["name"].ToString());
                        _NavigateUrl = string.Format(@"/basic/{0}.aspx?_slbg={1}&_slbi={2}", sDv[i]["code"], _Mid, i);
                        sM = GetMenuItem(_Text, _ImageUrl, _NavigateUrl);
                        pMenu[0].ChildItems[0].ChildItems.Add(sM);
                    }
                    break;
            }

        }

        private void initDateSoure()
        {
            //if SimplecCache no this object than init it
            if (_dt == null)
            {
                string sql = string.Format("select id,code,name,mid,FMenuIndex from {0} where FMenu <> 1 and FMenuIndex is not null order by FMenuIndex "
                    , Consts.TableScFunction);
                _dt = PCDb.Db.ExecQuery(sql);

                if (_dt != null && _dt.Rows.Count > 0)
                {
                    //Insert to Cache

                }

            }
        }

    }

    public class MenuHashTable : Hashtable
    {
        private string _objKeyRecordID = "RecordID";
        private string _objKeyMenuIndex = "FMenuIndex";

        public Int32 RecordID
        {
            get
            {
                return Convert.ToInt32(this[_objKeyRecordID]);
            }
            set
            {
                this[_objKeyRecordID] = value;
            }
        }

        public Int32 MenuIndex
        {
            get
            {
                return Convert.ToInt32(this[_objKeyMenuIndex]);
            }
            set
            {
                this[_objKeyMenuIndex] = value;
            }
        }
    }
}
