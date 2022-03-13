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

    [ToolboxData("<{0}:ImageButton runat=server></{0}:ImageButton>")]
    [ToolboxBitmap(typeof(Label))]
    public class ImageButton : System.Web.UI.WebControls.ImageButton, IButtonControl, IPostBackEventHandler
    {

        public ImageButton()
        {

        }

        #region UseSubmitBehavior
        protected bool _UseSubmitBehavior = true;
        [Category("Behavior"), DefaultValue("True"),
      Description("是否激活PerformValidation属性，控制WebForm_DoPostBackWithOptions  , add by jawance")]
        public bool UseSubmitBehavior
        {
            get { return _UseSubmitBehavior; }
            set { _UseSubmitBehavior = value; }
        }
        #endregion UseSubmitBehavior


        protected string _UseBeString = String.Empty;
        protected override void Render(HtmlTextWriter writer)
        {
            PostBackOptions pb = new PostBackOptions(this);
            if (_UseSubmitBehavior)
            {

                // pb.ClientSubmit = false;
                // pb.PerformValidation = false;

            }
            else
            {
                base.CausesValidation = false;
                // pb.ClientSubmit = true;
                pb.PerformValidation = true;

                base.OnClientClick += Page.ClientScript.GetPostBackEventReference(pb);
            }
 
            base.Render(writer);

        }
 

    }
}
