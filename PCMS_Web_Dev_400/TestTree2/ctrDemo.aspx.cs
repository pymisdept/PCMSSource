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

public partial class ctrDemo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //CtlTreel abc = new ctlTreel();
        DataTable dt = new DataTable();
        dt.Columns.Add("Father");
        dt.Columns.Add("Code");
        dt.Columns.Add("URL");


        DataRow dr = dt.NewRow();
        dr["Father"] = "";
        dr["Code"] = "Peter";
        dr["URL"] = "";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["Father"] = "Peter";
        dr["Code"] = "Paul";
        dr["URL"] = "";
        dt.Rows.Add(dr);


        dr = dt.NewRow();
        dr["Father"] = "Peter";
        dr["Code"] = "Mary";
        dr["URL"] = "";
        dt.Rows.Add(dr);

        //" ", "Categories", ""
        dr = dt.NewRow();
        dr["Father"] = "";
        dr["Code"] = "Categories";
        dr["URL"] = "";
        dt.Rows.Add(dr);



        //"Categories", "Graphics", ""
        dr = dt.NewRow();
        dr["Father"] = "Categories";
        dr["Code"] = "Graphics";
        dr["URL"] = "";
        dt.Rows.Add(dr);

        //"Categories", "Internet", ""
        dr = dt.NewRow();
        dr["Father"] = "Categories";
        dr["Code"] = "Internet";
        dr["URL"] = "";
        dt.Rows.Add(dr);

        //"Internet", "HTML", ""
        dr = dt.NewRow();
        dr["Father"] = "Internet";
        dr["Code"] = "HTML";
        dr["URL"] = "google.com";
        dt.Rows.Add(dr);

        //"Internet", "CSS", ""
        dr = dt.NewRow();
        dr["Father"] = "Internet";
        dr["Code"] = "CSS";
        dr["URL"] = "google.com";
        dt.Rows.Add(dr);


        CtlTreel1.dt = dt;
      

    }
}
