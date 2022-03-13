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
    public class MultiLineTextBox: PCCore.TextBox
    {
        public MultiLineTextBox(string id) : this(id, 100) { }
        public MultiLineTextBox(string id,double width)
        {
            PCCore.Common.HRLog.RecordLog("MultiLineTextbox");
            this.DataType = DataTypes.String;
            
            this.TextMode = TextBoxMode.MultiLine;
            this.Rows = 3;
            this.ID = id;
            this.Width = Unit.Percentage(width);
        }
    }//end of class

   

}

