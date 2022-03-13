using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;

namespace PCCore.PCMS
{
    public class Menu
    {
        string mid = "";
        string subFolderClass = "folder";
        string FileClass = "file";
        string strlink = "";

        public Menu(string _mid)
        {
            mid = _mid;
        }

        DataRow getFunction(string funcid)
        {
            DataTable _dt;
            string sql = string.Format("SELECT * FROM {0} WHERE FID = {1}","CPS_View_Menu",funcid);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt.Rows.Count > 0)
                    return _dt.Rows[0];
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("getFunction", ex);

            }
            return null;
        }

        TreeNode getTreeNode(string pid)
        {
            TreeNode _tn = new TreeNode();
            string sql = string.Format("SELECT * FROM {0} WHERE pid = {1} order by visible asc","CPS_View_Menu",pid);
            PCCore.Common.HRLog.RecordLog(sql);
            DataTable _dt;
            DataRow dr = getFunction(pid);
            if (dr != null)
            {
                
                _tn.Text = Convert.ToString(System.Web.HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, Convert.ToString(dr["code"])));
            }
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt.Rows.Count > 0)
                {
                    foreach (DataRow _dr in _dt.Rows)
                    {
                        if (Convert.ToInt32(_dr["submenu"]) == 1)
                        {
                            _tn.ChildNodes.Add(getTreeNode(Convert.ToString(_dr["fid"])));
                        }
                        else
                        {
                            TreeNode __tn = new TreeNode();
                            string webUrl = ""; // SubFolder + Function Code.aspx
                            __tn.Text = System.Web.HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, Convert.ToString(_dr["code"])).ToString();
                            __tn.ImageUrl = "";
                            __tn.NavigateUrl = webUrl;
                            __tn.Value = Convert.ToString(_dr["code"]);

                            _tn.ChildNodes.Add(__tn);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("getTreeNode: Parent Functon: " + pid ,ex);
            }
            return _tn;
        }
        public void getTreeView(TreeView _tv)
        {
            // Get First Level Menu of Current Module
            DataTable _dt;
            string sql = string.Format("SELECT * FROM {0} m where m.mmid = {1} and isNull(pid,0) = 0 order by visible asc ","CPS_View_Menu", mid);
            PCCore.Common.HRLog.RecordLog(sql);
            TreeNode _tn = new TreeNode();
            
            //TreeView _tv = new TreeView();

            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt.Rows.Count > 0)
                {
                    foreach (DataRow _dr in _dt.Rows)
                    {
                        // Check Sub Menu
                        if (Convert.ToInt32(_dr["submenu"]) == 1)
                        {
                            
                            _tv.Nodes.Add(getTreeNode(Convert.ToString(_dr["fid"])));
                        }
                        else
                        {
                            TreeNode __tn = new TreeNode();
                            string webUrl = ""; // SubFolder + Function Code.aspx
                            __tn.Text = System.Web.HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, Convert.ToString(_dr["code"])).ToString();
                            __tn.ImageUrl = "";
                            __tn.NavigateUrl = webUrl;
                            __tn.Value = Convert.ToString(_dr["code"]);
                            _tv.Nodes.Add(__tn);
                            //_tn.ChildNodes.Add(__tn);
                            

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Get Top Level Function", ex);
            }
            
            
        }

        public string getNavMenu(string mid)
        {
            string navHtml = "";
            navHtml += "<ul id='browser' class='filetree'>";

            // Get First Level Menu of Current Module
            DataTable _dt;
            string sql = string.Format("SELECT * FROM {0} m where m.mmid = {1} and isNull(pid,0) = 0 and isnull(show,0) = 1 order by visible asc ", "CPS_View_Menu", mid);
            PCCore.Common.HRLog.RecordLog(sql);
            
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt.Rows.Count > 0)
                {
                    foreach (DataRow _dr in _dt.Rows)
                    {
                        // Check Sub Menu
                        if (Convert.ToInt32(_dr["submenu"]) == 1)
                        {

                            navHtml = getSubMenu(navHtml, Convert.ToString(_dr["fid"]));
                            
                        }
                        else
                        {
                            strlink = Config.AppBaseUrl + Convert.ToString(_dr["subfolder"]) + "/" + Convert.ToString(_dr["code"]).Trim() + ".aspx";
                            navHtml += string.Format("<li><span class='{0}'><a style='color:Black;text-decoration: none' href='{1}'>{2}</a></span></li>", FileClass, strlink, Convert.ToString(System.Web.HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, Convert.ToString(_dr["code"]))));
                            
                        }
                    }
                    navHtml += "</ul>";
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Get Top Level Function", ex);
            }
            PCCore.Common.HRLog.RecordLog(navHtml);
            return navHtml;
        }

        string getSubMenu(string navHtml,string pid)
        {

            string sql = string.Format("SELECT * FROM {0} WHERE pid = {1} and isnull(show,0) = 1 order by visible asc", "CPS_View_Menu", pid);
            PCCore.Common.HRLog.RecordLog(sql);
            DataTable _dt;
            DataRow dr = getFunction(pid);
            if (dr != null)
            {
                // Write Sub Folder
                //_tn.Text = Convert.ToString(System.Web.HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, Convert.ToString(dr["code"])));
                PCCore.Common.HRLog.RecordLog("Folder Name:" + Convert.ToString(dr["title_code"]));
                navHtml += string.Format("<li><span class='{0}'>{1}</span>",subFolderClass,Convert.ToString(System.Web.HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels,Convert.ToString(dr["title_code"]))));
			
            }
            
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt.Rows.Count > 0)
                {
                    navHtml += "<ul>";
                    foreach (DataRow _dr in _dt.Rows)
                    {
                        if (Convert.ToInt32(_dr["submenu"]) == 1)
                        {
                            //_tn.ChildNodes.Add(getTreeNode(Convert.ToString(_dr["fid"])));
                            navHtml += getSubMenu(navHtml, Convert.ToString(_dr["fid"]));
                        }
                        else
                        {
                            strlink = Config.AppBaseUrl + Convert.ToString(_dr["subfolder"]) + "/" + Convert.ToString(_dr["code"]).Trim() + ".aspx";
                            navHtml += string.Format("<li><span class='{0}'><a style='color:Black;text-decoration: none' href='{1}'>{2}</a></span></li>", FileClass, strlink, Convert.ToString(System.Web.HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, Convert.ToString(_dr["code"]))));
                        }
                    }
                    navHtml += "</ul>";
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("getTreeNode: Parent Functon: " + pid, ex);
            }
            
            return navHtml;
        }
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
        
            }
}
