using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Collections;
using System.Data;

namespace PCCore.Common
{
    // Modified: 2008-3-26 9:37:44 by Jesse Zhang
    public class HRLog
    {
        //private static string RecordFilePath = Config.TempBaseUrl;
        private const int FontSize = 3;

        private static string RecordFilePath
        {
            get
            {
                //string url = "~/Temp";
                string url = string.Format("~/Temp/{0}/{1}",SessionInfo.LoginName,HttpContext.Current.Session.SessionID.ToString());
                string fPath = string.Format("{0}\\",HttpContext.Current.Server.MapPath(url));
                System.IO.DirectoryInfo _di = new DirectoryInfo(fPath);
                try
                {
                    if (_di.Exists == false)
                    {
                        _di.Create();
                    }
                } catch (Exception ex)
                {

                }
                return fPath;
            }
        }
        
        #region WebPageName

        public static string WebPageName
        {
            get
            {
                string pageName = string.Empty;
                
                try
                {
                    pageName = HttpContext.Current.Request.ServerVariables[29].ToString();
                }
                catch
                {
                    pageName = ".Unknown";
                }
                
                int mIndex = 0;
                for ( int i = 0; i < pageName.Length; i++ )
                {
                    int n = pageName.IndexOf ( "/", i );
                    if ( n > 0 )
                    {
                        mIndex = n;
                    }
                }
                
                int eIndex = pageName.IndexOf ( "," );
                if ( eIndex == -1)
                {
                    eIndex = pageName.Length;
                    
                    if ( pageName.ToLower().EndsWith("aspx"))
                    {
                        eIndex = eIndex - 5;
                    }    
                }

                string fileName = pageName.Substring ( mIndex + 1, eIndex - mIndex -1 );

                return fileName;
            }
        }
        #endregion

        #region Record LOG

        public static void RecordLog( string title, object logInfo, bool createNewFile )
        {
            string fileName = string.Format ( "{0}.htm", WebPageName );
            RecordLog ( fileName, title, logInfo.ToString (), createNewFile );
        }

        public static void RecordLog( string title, object logInfo )
        {
            string fileName = string.Format ( "{0}.htm", WebPageName );
            RecordLog ( fileName, title, logInfo.ToString (), false );
        }

        public static void RecordLog( object logInfo )
        {
            string fileName = string.Format ( "{0}.htm", WebPageName );
            RecordLog ( fileName, string.Empty, logInfo.ToString (), false );
        }
        #endregion

        #region RecordLog Function
        public static void RecordLog( string pageName, string title, string logInfo, bool createNewFile )
        {
            try
            {
                if (pageName == string.Empty)
                    return;
                string fileFullName = string.Format("{0}{1}", RecordFilePath, pageName);

                #region Delete Old File
                if (createNewFile)
                {
                    System.IO.File.Delete(fileFullName);
                }
                #endregion

                if (Config.isLogDebug)
                {
                    FileStream file = new FileStream(fileFullName, FileMode.Append);
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(file))
                        {
                            #region Create Title
                            if (title != string.Empty)
                            {
                                sw.WriteLine(string.Format("<font color='#dd0000' size='2'><strong>{0}</strong></font>", title));
                                sw.WriteLine("<br>");
                            }
                            #endregion

                            string recordInfo = string.Format("<font size='2' color='#bbbbbb'>{0} {1}</font><br>", WebPageName, DateTime.Now.ToString());
                            sw.WriteLine(recordInfo);

                            sw.WriteLine(string.Format("<font size='{0}'>{1}</font>", FontSize, logInfo));
                            sw.Write(CreateSQLLink(logInfo));
                            sw.WriteLine("<br>");
                        }
                    }
                    catch
                    {

                        file.Close();
                    }
                }
            }
            catch (Exception iex)
            {
                
            }
        }
        #endregion

        #region SQL Process

        private static string CreateSQLLink( string logInfo )
        {
            string strScript = string.Empty;
            logInfo = HttpContext.Current.Server.HtmlEncode ( logInfo.ToLower ().Trim () );
            logInfo = FormatSQL ( logInfo );
            if ( logInfo.IndexOf ( "select" ) != -1 && logInfo.IndexOf ( "from" ) != -1 )
            {
                strScript += "<img src=\"images\\phto.gif\" style=\"cursor:hand;\" alt=\"Run SQL\" ";
                strScript += string.Format ( "onclick = \"javascript:window.open('http://localhost/HRLogManager/RunSQL.aspx?SQL={0}','_blank', 'scrollbars = no,status=no,toolbar=no,width=600,height=400,top=200,left=200,resizable=yes');\" ", logInfo );
                strScript += " >";
            }

            return strScript;
        }

        private static string FormatSQL( string logInfo )
        {
            logInfo = logInfo.Replace ( "'", "(danyinhao)" );
            logInfo = logInfo.Replace ( "+", "(jiahao)" );
            logInfo = HttpContext.Current.Server.UrlEncode ( logInfo );

            return logInfo;
        }
        #endregion 

        #region Record DataTable
        public static void RecordTable( string title, DataTable dt )
        {
            try
            {
                if (dt == null)
                    return;

                string pageName = string.Format("{0}.htm", WebPageName);
                string fileFullName = string.Format("{0}{1}", RecordFilePath, pageName);
                if (Config.isLogDebug)
                {
                    FileStream file = new FileStream(fileFullName, FileMode.Append);

                    try
                    {
                        using (StreamWriter sw = new StreamWriter(file))
                        {
                            #region Create Title
                            if (title != string.Empty)
                            {
                                sw.WriteLine(string.Format("<font color='#dd0000' size='2'><strong>{0}</strong></font>", title));
                                sw.WriteLine("<br>");
                            }
                            #endregion

                            string recordInfo = string.Format("<font size='2' color='#bbbbbb'>{0} {1}</font><br>", WebPageName, DateTime.Now.ToString());
                            sw.WriteLine(recordInfo);

                            sw.WriteLine("<table border=\"1\" cellspacing=\"1\" cellpadding=\"2\" width=\"90%\">");

                            #region Table Header

                            sw.WriteLine("<tr>");
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                sw.WriteLine("<td>");
                                sw.WriteLine(dt.Columns[i].ColumnName);
                                sw.WriteLine("</td>");
                            }
                            sw.WriteLine("</tr>");
                            #endregion

                            #region Table Rows

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                sw.WriteLine("<tr>");
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    sw.WriteLine("<td>");
                                    sw.WriteLine(dt.Rows[i][j].ToString());
                                    sw.WriteLine("</td>");
                                }
                                sw.WriteLine("</tr>");
                            }
                            #endregion

                            sw.WriteLine("</table>");
                            sw.WriteLine("<br>");
                        }
                    }
                    catch
                    {
                        file.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Record Exception

        public static void RecordException(string title,Exception ex)
        {
            try
            {
                string pageName = string.Format("{0}.htm", WebPageName);
                string fileFullName = string.Format("{0}{1}", RecordFilePath, pageName);
                if (Config.isLogException)
                {
                    FileStream file = new FileStream(fileFullName, FileMode.Append);

                    try
                    {
                        using (StreamWriter sw = new StreamWriter(file))
                        {
                            #region Create Title
                            if (title != string.Empty)
                            {
                                sw.WriteLine(string.Format("<font color='#dd0000' size='2'><strong>{0}</strong></font>", title));
                                sw.WriteLine("<br>");
                            }
                            #endregion

                            string webPageName = HttpContext.Current.Request.ServerVariables[29].ToString();
                            string recordInfo = string.Format("<font size='2' color='#bbbbbb'>{0} {1}</font><br>", webPageName, DateTime.Now.ToString());
                            sw.WriteLine(recordInfo);

                            sw.WriteLine(ex.Message);
                            sw.WriteLine("<br>");
                            sw.WriteLine(ex.Source);
                            sw.WriteLine("<br>");
                            sw.WriteLine(ex.StackTrace);
                            sw.WriteLine("<br>");
                        }
                    }
                    catch
                    {
                        file.Close();
                    }
                }
            }
            catch (Exception iex)
            {
            }
        }
        #endregion

    }
}
