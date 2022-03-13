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

public partial class AnnouncementView : BasePage
{
    const string TABLE_NAME = "DocumentProperty";
    const string FILE_TYPE = "2002";
    
    string msgid = string.Empty;
    string msgtype = string.Empty;
    string msguserid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!SessionInfo.IsLogin)
            return;

        Master.ClearError();

        if (Page.IsPostBack == false)
        {
            try
            {
                msgid = Request.QueryString["msgid"].ToString();
            }
            catch (Exception ex)
            { }
            try
            {
                msgtype = Request.QueryString["msgtype"].ToString();
            }
            catch (Exception ex)
            { }
            try
            {
                msguserid = Request.QueryString["msguserid"].ToString();
            }
            catch (Exception ex)
            { }

        }
        if (this.CurrentCommand != Consts.ButtonExport)
            SetDataSource();


    }//end of Page_Load

    protected void SetDataSource()
    {
        
        DataRow _dr;
        
        try
        {
            //_dtAnnouncement = PCDb.Db.ExecQuery(sql);
            _dr = PCCore.Database.SC_Announcements.getRecord(msgid, msgtype, msguserid);

            if (_dr != null)
            {
                try
                {
                    txtbody.Text = Convert.ToString(_dr["BODY"]);
                    txtProject.Text = Convert.ToString(_dr["PRJCODE"]);
                    txtSender.Text = Convert.ToString(_dr["SenderName"]);
                    txtTitle.Text = Convert.ToString(_dr["Title"]);
                    // Update Record to read
                    switch (msgtype)
                    {
                        case "S": //System
                            PCCore.Database.SC_Announcements.ReadAnnouncement(msgid, msgtype);
                            break;
                        case "U": // User
                            PCCore.Database.SC_Announcements.ReadAnnouncement(msgid, msguserid);
                            break;
                        case "F": // Function
                            PCCore.Database.SC_Announcements.ReadAnnouncement(msgid, msgtype);
                            break;

                    }
                }
                catch (Exception ex)
                {
                    PCCore.Common.HRLog.RecordException("AnnouncementView", ex);
                }
            }
            else
            {
                PCCore.Common.HRLog.RecordException("View Announcement", new Exception("No Record found"));
            }
        }
        catch (Exception ex)
        {
            PCCore.Common.HRLog.RecordException("View Announcement", ex);
        }
        //StringBuilder sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,b.filesize,a.description,c.loginname,convert(varchar(20),b.uploadtime,120) as uploadtime,a.projectCode from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
        //StringBuilder sb = new StringBuilder(string.Format("select * from CPS_View_DocumentMessage a where a.ID={0} order by {1}",ID,"LineNum asc,xlWkSheet asc,xlLineNum asc"));
        
        

        
        
    }

    

    protected void btnClose_Click(object sender, EventArgs e)
    {

    }
}
