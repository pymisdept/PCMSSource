<%@ Application Language="C#" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="PCCore" %>
<%@ Import Namespace="System.Data" %>
<script runat="server">
    
   
    void Application_Start(object sender, EventArgs e) 
    {
        Application.Lock();
        Application["AppTempDir"] = Server.MapPath(Config.TempBaseUrl);
        //License.Init();
        Application.UnLock();
    }

    
    void Application_End(object sender, EventArgs e) 
    {
        string appTemp = Application["AppTempDir"].ToString();
        try
        {
            if (Directory.Exists(appTemp))
            {
                //string[] dirs = Directory.GetDirectories(appTemp, "t_*");
                string[] dirs = Directory.GetDirectories(appTemp);
                foreach (string dir in dirs)
                {
                    Directory.Delete(dir, true); ;
                }
            }
        }
        catch { }
        
        try
        {
            if (Directory.Exists(appTemp))
            {
                string[] files = Directory.GetFiles(appTemp);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
        }
        catch { }
    }

    void Application_Error(object sender, EventArgs e)
    {
        Server.Transfer(Config.AppBaseUrl + "\\ErrorPage.aspx", true);
    }

    void Session_Start(object sender, EventArgs e) 
    {
        SessionInfo.Init();
        PCCore.Common.HRLog.RecordLog("Session_Start");
        //if (!License.IsLicensed)
        //{
        //    //throw new ApplicationException("No License.");
        //    Server.Transfer(Config.AppBaseUrl + "\\License_Request.aspx", true);
        //}
        
        
        //Response.Redirect(Config.AppBaseUrl + "\\Default.aspx", false); 2007-03-16 (Base Page had declare's logic)

        try
        {
            Directory.CreateDirectory(SessionInfo.TempDir);
        }
        catch
        {
        }

    }

    void Session_End(object sender, EventArgs e) 
    {
        try
        {
            PCCore.Common.HRLog.RecordLog("Session_End");
            PCCore.Common.HRLog.RecordLog("Userid: " + SessionInfo.UserId);
            // Insert Logout Log Table
            PCCore.PCMS.UserLog.Logout(Convert.ToInt32(SessionInfo.UserId), System.Web.HttpContext.Current.Session.SessionID);
            //Directory.Delete(SessionInfo.TempDir, true);
            Directory.Delete(Session[SessionInfo.SE_TEMPDIR].ToString(), true);

            ScLog.InsertTimeout(Session[SessionInfo.SE_LOGINNAME].ToString(),
                Session[SessionInfo.SE_REMOTEADDR].ToString(),
                Session[SessionInfo.SE_REMOTEHOST].ToString(),
                Session[SessionInfo.SE_REMOTEUSER].ToString());
        }
        catch
        {            
        }
    }
       
</script>
