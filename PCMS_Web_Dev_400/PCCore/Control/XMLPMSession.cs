using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace PCCore.Common
{
    public class XMLPMSession
    {
        public XMLPMSession()
        {

        }
        public static Boolean Write(Hashtable htValue)
        {
            foreach (object o in htValue.Keys)
            {
                XMLPMSession.Write(o.ToString(), ((System.Collections.Hashtable)htValue[o.ToString()]));
            }
            return true;
        }
        public static Boolean Write(string name,Hashtable htValue)
        {
            PCCore.Common.HRLog.RecordLog("Write: " + name); 
            XMLPMSession.Delete(name);
            string strFile = XMLPath() + "\\" + name + ".xml";    
            XmlTextWriter _xtw = new XmlTextWriter(strFile,Encoding.Unicode);
            _xtw.WriteStartDocument();
            _xtw.WriteStartElement("Row");
            foreach (object o in htValue.Keys)
            {
                _xtw.WriteStartElement("Detail");
                _xtw.WriteStartElement("Value");
                _xtw.WriteValue(o.ToString());
                _xtw.WriteEndElement();
                try
                {
                    Hashtable _htColumn = (Hashtable) htValue[o];
                    int _i = 1;
                    _xtw.WriteStartElement("Column");
                    foreach (object _o in _htColumn.Keys)
                    {
                        _xtw.WriteStartElement(_o.ToString());
                        _xtw.WriteValue(_htColumn[_o.ToString()].ToString());
                        _xtw.WriteEndElement();
                        _i++;
                    }
                    _xtw.WriteEndElement();
                    
                } catch (Exception ex)
                {
                    PCCore.Common.HRLog.RecordException("ArrayList in WriteXML",ex);
                }
                _xtw.WriteEndElement();

            }
            _xtw.WriteEndElement();
            _xtw.WriteEndDocument();
            try
            {
                _xtw.Close();
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Close Xml Writer", ex);
            }
            return true;

        }
        public static Hashtable Read(string name)
        {
            try
            {
                return XMLPMSession.Read(new System.IO.FileInfo(XMLPMSession.XMLPath() + "\\" + name + ".xml"));
            }
            catch (Exception ex)
            {
                return new Hashtable();
            }
        }

        public static Hashtable Read(System.IO.FileInfo fi)
        {
            PCCore.Common.HRLog.RecordLog("Read");
            PCCore.Common.HRLog.RecordLog(fi.FullName);
            Hashtable _htValue = null;

            XmlDocument xmldoc = new XmlDocument();
            XmlNodeList nodes;
            
            ArrayList _al = null;
            if (fi.Exists)
            {
                PCCore.Common.HRLog.RecordLog("File Exists");
                try
                {
                    _htValue = new Hashtable();
                    //string strfile = XMLPath() + "\\" + name + ".xml";
                    xmldoc.Load(fi.FullName);
                    // Test to write data to log file
                    System.IO.StreamReader _sw = new System.IO.StreamReader(fi.FullName);
                    PCCore.Common.HRLog.RecordLog("File Content");
                    PCCore.Common.HRLog.RecordLog(_sw.ReadToEnd());
                    _sw.Close();
                    

                    nodes = xmldoc.SelectNodes("//Detail");
                    
                    string key;
                    foreach (XmlNode _node in nodes)
                    {
                        PCCore.Common.HRLog.RecordLog("Loop Detail Node");
                        XmlNodeList _nodeCols = _node.LastChild.ChildNodes;
                        PCCore.Common.HRLog.RecordLog(_node.InnerXml);
                        XmlNode __nodekey = _node.FirstChild;
                        
                        //XmlNode __nodecols = _node.LastChild;

                        Hashtable _htColumn = new Hashtable();
                        
                        foreach (XmlNode __nodecol in _nodeCols)
                        {

                            try
                            {
                                _htColumn.Add(__nodecol.Name, __nodecol.LastChild.Value);
                            }
                            catch (Exception ex)
                            {
                                // Assign Empty when no Value;
                                _htColumn.Add(__nodecol.Name, String.Empty);
                            }
                        }
                        _htValue.Add(__nodekey.FirstChild.Value.ToString(), _htColumn);
                    }
                }
                catch (Exception ex)
                {
                    PCCore.Common.HRLog.RecordException("Read XML: ", ex);
                    _htValue = new Hashtable();
                }
                
            } else 
            {
                _htValue = new Hashtable();
            }
            
            return _htValue;

        }
        public static Hashtable Read()
        {
            Hashtable _htValue = new Hashtable();
            System.IO.DirectoryInfo _dir = new System.IO.DirectoryInfo(XMLPath());
            foreach (System.IO.FileInfo _fi in _dir.GetFiles())
            {
                _htValue.Add(_fi.Name.Replace(".xml", ""), XMLPMSession.Read(_fi.Name.Replace(".xml", "")));
            }
            return _htValue;

        }
        public static void Delete(int sessionid)
        {
            System.IO.DirectoryInfo _dir = new System.IO.DirectoryInfo(XMLPath()  + sessionid.ToString());
        }
        public static void Delete(string name)
        {
            string strFile = XMLPath()  + name + ".xml";
            System.IO.FileInfo _fi = new System.IO.FileInfo(strFile);
            try
            {
                if (_fi.Exists)
                {
                    try
                    {
                        _fi.Delete();
                    }
                    catch (Exception ex)
                    {
                        PCCore.Common.HRLog.RecordException("Delete XML File", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Delete File", ex);
            }
        }
        public static Boolean Exists()
        {
            System.IO.DirectoryInfo _dir = new System.IO.DirectoryInfo(XMLPMSession.XMLPath());
            if (!_dir.Exists)
                return false;
            else
            {
                if (_dir.GetFiles().Length > 0)
                    return true;
                else
                    return false;
            }
        }
        public static Boolean Exists(string name)
        {
            string strFile = XMLPath() + "\\" + name + ".xml";
            System.IO.FileInfo _fi = new System.IO.FileInfo(strFile);
            if (_fi.Exists)
            
                return true;
            else
                return false;
            
        }
        public static string XMLPath()
        {
            //string strPath = HttpContext.Current.Server.MapPath(Config.XMLDirectory);
            string strPath = HttpContext.Current.Server.MapPath("~\\" + Config.XMLDirectory + "\\" + HttpContext.Current.Session.SessionID + "\\" + SessionInfo.PMRptProject + "\\" + SessionInfo.PMRptPeriod + "\\PMRPT");
            //string strPath = ("c:\\" + Config.XMLDirectory + "\\" + HttpContext.Current.Session.SessionID + "\\PMRPT");
            System.IO.DirectoryInfo _dir = new System.IO.DirectoryInfo(strPath);
            PCCore.Common.HRLog.RecordLog(_dir.FullName);
            if (!_dir.Exists)
            {
                try
                {
                    _dir.Create();
                }
                catch (Exception ex)
                { PCCore.Common.HRLog.RecordException("Create Session Directory", ex); }
            }
            _dir = new System.IO.DirectoryInfo(strPath);

            return strPath;
        }
        
    }
}
