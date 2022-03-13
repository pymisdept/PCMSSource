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
using SimpleControls;
using System.Collections;



namespace PCCore.Control.PmsReport
{
    public class NumericBox: PCCore.TextBox
    {
        public NumericBox(string id)
            : this(id, 100)
        {

        }
        public NumericBox(string id,double d)
        {
            this.DataType = DataTypes.Double;
            this.TextMode = TextBoxMode.SingleLine;
            //this.Rows = 3;
            
            this.ID = id;
            this.Width = Unit.Percentage(d);
        }
    }//end of class

   

}

