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

public partial class ctlTreel : System.Web.UI.UserControl
{
    public DataTable dt;


    
    protected void Page_Load(object sender, EventArgs e)
    {
       // TreeView treeMain = new TreeView();
      
        


        if (!IsPostBack)
        {


            foreach (DataRow dr1 in dt.Rows)
            {

                if (Convert.ToString(dr1["Father"]).Equals(""))
                {
                    string c1 = Convert.ToString(dr1["Code"]);
                    TreeNode groupNode = new TreeNode(c1);
                    groupNode.ShowCheckBox = false;
                    loop(dt, c1, groupNode);
                    treeMain.Nodes.Add(groupNode);
                    treeMain.CollapseAll();
                    //DataRow [] dt1  = dt.Select("Father ='' and Code ='"+c1+"'");



                    //groupNode.ShowCheckBox = false;
                    //foreach (DataRow dr2 in dt1)
                    //{
                    //    string c2 = Convert.ToString (dr2["Code"]);

                    //    DataRow[] dt2 = dt.Select("Father ='"+c2+"'");
                    //    if (dt2.Length  > 0)
                    //    {
                    //        TreeNode subheadingNode = new TreeNode(c2);
                    //        subheadingNode.ShowCheckBox = false;
                    //        foreach (DataRow dr3 in dt2)
                    //        {

                    //            string c3 = Convert.ToString(dr3["Code"]);
                    //            TreeNode productNode = new TreeNode(c3);
                    //            productNode.NavigateUrl = "http://google.com";
                    //            productNode.ShowCheckBox = false;

                    //            subheadingNode.ChildNodes.Add(productNode);


                    //        }
                    //        groupNode.ChildNodes.Add(subheadingNode);
                    //    }



                    //}

                    //treeMain.Nodes.Add(groupNode);
                    //treeMain.CollapseAll();







                }
            }

        }

    }

    public TreeNode loop(DataTable dt, string code, TreeNode groupNode)
    {
        DataRow[] dtSub = dt.Select("Father ='" + code + "'");
        if (dtSub.Length > 0)
        {

            TreeNode subheadingNode = new TreeNode(code);
            subheadingNode.ShowCheckBox = false;

            foreach (DataRow dr in dtSub)
            {
                string son = Convert.ToString(dr["Code"]);
                string url = Convert.ToString(dr["URL"]);

                TreeNode productNode = new TreeNode(son);
                if (!url.Equals(""))
                    productNode.NavigateUrl = url;
                productNode.ShowCheckBox = false;

                subheadingNode.ChildNodes.Add(productNode);
                loop(dt, son, productNode);
                groupNode.ChildNodes.Add(productNode);
            }


        }
        return groupNode;
    }
}
