using System;
using System.Data;
using System.Text;
using SimpleControls.SimpleDatabase;
using SimpleControls.SimpleCrypto;
using System.Web;
using System.Web.SessionState;
using System.Collections.Specialized;
using SimpleControls;
using System.Net.Mail;

namespace PCCore
{
    public class Sc
    {
        public enum FunctionStates
        {
            // the order is very important, Denied > Allowed > None
            None,
            Allowed,
            Denied
        }

        public enum PublishTypes
        {
            // don't change the order
            Public,
            Private, // private select not implemented currently
            PrivateUpdate
        }

        

        public static FunctionStates ConvertToFunctionState(object o)
        {
            int i = Convert.ToInt32(o);
            FunctionStates fs = (FunctionStates)Enum.ToObject(typeof(FunctionStates), i);
            return fs;
        }

        public static PublishTypes ConvertToPublishType(object o)
        {
            int i = Convert.ToInt32(o);
            PublishTypes op = (PublishTypes)Enum.ToObject(typeof(PublishTypes), i);
            return op;
        }


        public static FunctionStates ConvertAvailableStateToFunctionState(object o)
        {
            int i = Convert.ToInt32(o);
            FunctionStates fs = (i == 0 ? FunctionStates.Denied : FunctionStates.None);
            return fs;
        }

        public static bool ConvertIntegerToBoolean(object o)
        {
            int i = Convert.ToInt32(o);
            return i != 0;
        }

        public static string Encrypt(string password)
        {
            CryptoHash md5 = new CryptoHash(CryptoHash.Hashes.MD5);
            string hash = md5.GetHashInString(password);
            return hash;
        }

        public static bool Login(string loginName, string password)
        {
            //InitSessionSystemParameters();

            string hash = Encrypt(password);

            //Modified by Michael, begin
            string sql = String.Format(
        "select id,supervisor,reverse,FULLNAME,ACCESSPROJECTID,ExpireDate,BATCHUPLOAD from v_sc_users where loginname='{0}' COLLATE SQL_Latin1_General_CP1_CS_AS and passwd='{1}' COLLATE SQL_Latin1_General_CP1_CS_AS and locked=0"
                , loginName, hash);
            //Modified by Michael,end

            DataTable dt = PCDb.Db.ExecQuery(sql);
            DataRow dr;
            NameValueCollection nvc = HttpContext.Current.Request.ServerVariables;

            if (dt == null || dt.Rows.Count != 1)
            {
                return false;
            }
            dr = dt.Rows[0];
            //Todo: if ExipreDate <= today ,then not allow login in 2007-10-19
            if (dr["ExpireDate"] != DBNull.Value && Convert.ToDateTime(dr["ExpireDate"]) <= DateTime.Now)
            {
                // throw new ApplicationException("Not Allow Expire User Login In.");
                return false;
            }
            

            PCCore.Common.HRLog.RecordLog("Login");
            SessionInfo.IsLogin = true;
            PCCore.Common.HRLog.RecordLog("SessionInfo IsLogin: " + SessionInfo.IsLogin.ToString());
            SessionInfo.UserId = dr["ID"].ToString();
            SessionInfo.LoginName = loginName;
            SessionInfo.UserPassword = password;
            SessionInfo.UserName = dr["FULLNAME"].ToString();
            SessionInfo.IsSupervisor = (Convert.ToInt32(dr["SUPERVISOR"]) == 1 ? true : false);

            //add by Michael, begin
            SessionInfo.IsReverse = ((Convert.ToInt32(dr["SUPERVISOR"]) == 1 || Convert.ToInt32(dr["REVERSE"]) == 1) ? true : false);
            //add by Michael, end

            //add by Ken, begin
            SessionInfo.IsBatchUpload = ((Convert.ToInt32(dr["SUPERVISOR"]) == 1 || Convert.ToInt32(dr["BATCHUPLOAD"]) == 1) ? true : false);
            //add by Ken, end

            // Insert Login Log Table
            PCCore.PCMS.UserLog.Login(Convert.ToInt32(SessionInfo.UserId));
            //SessionInfo.AccessLevel = dr["ACCESSLEVELID"].ToString();
            

            SessionInfo.ProjectID = String.IsNullOrEmpty(dr["ACCESSPROJECTID"].ToString()) ? "-1" : dr["ACCESSPROJECTID"].ToString();

            dt = PCDb.Db.ExecQuery(string.Format(@"select b.accessprojectid,a.groupid
                                                    from sc_groupuser a
                                                    left join sc_group b on b.id=a.groupid
                                                    where a.userid={0}",SessionInfo.UserId));
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SessionInfo.ProjectID = SessionInfo.ProjectID + "," + dt.Rows[i]["accessprojectid"].ToString();
                }
            }

            SessionInfo.DataFilter = string.Format(" and (projectCode in('{0}') or (projectCode IS null and uploadbyid='{1}') )", SessionInfo.ProjectID.Replace(",", "','"), SessionInfo.UserId);

            SessionInfo.RemoteAddr = nvc["REMOTE_ADDR"];
            SessionInfo.RemoteHost = nvc["REMOTE_HOST"];
            SessionInfo.RemoteHost = nvc["HTTP_HOST"];
            SessionInfo.RemoteUser = nvc["REMOTE_USER"];

            // Host Name Solution
            //SessionInfo.RemoteHost = HttpContext.Current.Request.UserHostName;
            //System.Net.IPHostEntry objIPHostEntry = System.Net.Dns.GetHostEntry(nvc["REMOTE_ADDR"]);
            //SessionInfo.RemoteHost = objIPHostEntry.HostName;

            
  

            ScLog.Insert(ScLog.Actions.Login);


            return true;
        }

        public static void Logout()
        {
            // Insert Logout Log Table
            PCCore.PCMS.UserLog.Logout(Convert.ToInt32(SessionInfo.UserId), System.Web.HttpContext.Current.Session.SessionID);

            ScLog.Insert(ScLog.Actions.Logout);
            PCCore.Common.HRLog.RecordLog("Logout");
            SessionInfo.IsLogin = false;
            SessionInfo.UserId = String.Empty;
            SessionInfo.LoginName = String.Empty;
            SessionInfo.UserName = String.Empty;
            SessionInfo.IsSupervisor = false;
            //add by Michael, begin
            SessionInfo.IsReverse = false;
            //add by Michael, end
            //add by Ken, begin
            SessionInfo.IsBatchUpload  = false;
            //add by Ken, end
        }

        public static string GetFunctionCode(string requestPath)
        {
            string func = requestPath.ToLower();
            func = func.Substring(func.LastIndexOf("/") + 1).Replace(".aspx", "");
            int p = func.IndexOf("_");
            if (p > 0)
            {
                func = func.Substring(0, p);
            }
            return func;
        }

        public static string GetFullFunctionCode(string requestPath)
        {
            string func = requestPath.ToLower();
            func = func.Substring(func.LastIndexOf("/") + 1).Replace(".aspx", "");

            return func;
        }

        public static string Encode(string text)
        {
            byte[] bytes = System.Text.ASCIIEncoding.Default.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }

        public static string Decode(string text)
        {
            byte[] bytes = Convert.FromBase64String(text);
            string s = System.Text.ASCIIEncoding.Default.GetString(bytes);
            return s;
        }

        public static bool RecoverPassword(string email)
        {
            string sql = String.Format("select id,loginname,fullname,staffid from v_ps_employee where email='{0}'", email.Trim().ToLower());
            DataTable dt = PCDb.Db.ExecQuery(sql);
            if (dt == null || dt.Rows.Count != 1)
            {
                throw new ApplicationException("Email address is not in our database.");
            }

            SimplePassword sp = new SimplePassword();
            sp.Minimum = 8;
            sp.Maximum = 8;
            sp.ExcludeSymbols = true;
            string password = sp.Generate();
            string hash = Encrypt(password);

            password = password.ToLower();

            DataRow dr = dt.Rows[0];
            string id = dr[0].ToString();
            string loginName = dr[1].ToString();
            string fullName = dr[2].ToString();

            /*--add by ada */
            PCCore.Common.Security _s = new PCCore.Common.Security();
            /*--end add by ada */

            //sql = String.Format("update ps_personal set password='{0}' where id={1}", hash, id);
            sql = String.Format("update sc_user set passwd='{0}', passwd2='{1}' where id={2}", Sc.Encrypt(password), _s.Encrypt(password), id);
            PCDb.Db.ExecUpdate(sql);

            string subject = String.Format("{0} - Forgotten Password", Consts.AppName);
            StringBuilder body = new StringBuilder();
            /*
            body.AppendFormat("\n{0},\n\n", fullName);
            body.Append("Your forgotten password request was received. \n\n");
            body.Append("A new password was created to you as below: \n\n");
            body.AppendFormat("Username: {0}\n", loginName);
            body.AppendFormat("Password: {0}\n\n", password);
            //body.AppendFormat("For security reasons, please change the password immediately <a href='http://{0}{1}/{2}'>here</a>\n\n", Config.ServerIPAddress, Config.AppBaseUrl, "Security/ChangePassword.aspx?from=cs");
            body.Append("Or\n\n");
            body.AppendFormat("You can login the <a href='http://{0}{1}/{2}'>PC System</a> now.\n\n", Config.ServerIPAddress, Config.AppBaseUrl, "");
            body.Append("*** If you did not send this forgotten password request, please reply ASAP to inform system administrator.\n\n");
            body.AppendFormat("Thanks, \n{0}\n", Consts.AppName);
            */

            body.AppendFormat("Dear {0},\n\n", fullName);
            //body.Append("Your forgot password request was received. \n\n");
            body.Append("A new password was created to you as below: \n\n");
            body.AppendFormat("Username: {0}\n", loginName);
            body.AppendFormat("Password: {0}\n\n", password);
            //body.AppendFormat("For security reasons, please change the password immediately <a href='http://{0}{1}/{2}'>here</a>\n\n", Config.ServerIPAddress, Config.AppBaseUrl, "Security/ChangePassword.aspx?from=cs");            
            //body.Append("*** If you did not send this forgot password request, please reply ASAP to inform system administration.\n\n");
            //body.AppendFormat("Thanks \n{0}\n", "PCMS Auto Messaging System");
            body.AppendFormat("{0}\n", Config.FromName);

            try
            {
                SmtpClient client = new SmtpClient(Config.SMTPServer);
                MailAddress from = new MailAddress(Config.FromMail);
                MailAddress to = new MailAddress(email);
                MailMessage mail = new MailMessage(from, to);
                mail.Subject = subject;
                mail.Body = body.ToString().Replace("\n", "<br/>");
                mail.IsBodyHtml = true;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                client.Send(mail);
            }
            catch
            {
            }
            finally
            {
            }

            /* remark by ada */
            /*
            SimpleMail sm = new SimpleMail(Config.SMTPServer, Config.FromMail, Config.FromName);
            sm.UserName = Config.SMTPUserName;
            sm.Password = Config.SMTPPassword;

            try
            {
                sm.Send(email, email, subject, body.ToString().Replace("\n", "<br/>"));
            }
            catch
            {
            }
            finally
            {
            }
            //sm.Send(email, email, subject, body.ToString().Replace("\n", "<br/>"), null, Config.BccMail, null);
            */
            /* --remark by ada */

            return true;
        }

        public static bool UserChangePassword(string old, string new1, string new2)
        {
            if (new1.Length < 8)
            {
                throw new ApplicationException("Minimum password length is 8.");
            }

            if (new1 != new2)
            {
                throw new ApplicationException("New Passwords do not matched.");
            }

            string hash = Encrypt(old);
            string sql = String.Format("select passwd from sc_user where loginname='{0}' and passwd='{1}'", SessionInfo.LoginName, hash);
            DataTable dt = PCDb.Db.ExecQuery(sql);
            if (dt == null || dt.Rows.Count != 1)
            {
                throw new ApplicationException("Old Passwords is not correct.");
            }

            hash = Encrypt(new1);
            sql = String.Format("update sc_user set passwd='{0}' where loginname='{1}'", hash, SessionInfo.LoginName);
            PCDb.Db.ExecUpdate(sql);
            return true;

        }

    } //end of class
}
