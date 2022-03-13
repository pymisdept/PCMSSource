using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Collections;
using System.Data;

namespace PCCore.Common
{
    public class File
    {
        System.IO.FileInfo fi;
        public Boolean isExists;

        public File(String _file)
        {
            try
            {
                fi = new FileInfo(_file);
                isExists = fi.Exists;
            } catch (Exception ex)
            {
                isExists = false;
            }
        }
        
        public File(String _path, String _file)
        {
            try
            {
                fi = new FileInfo(_path + "\\" + _file);
                isExists = fi.Exists;
            }
            catch (Exception ex)
            {
                isExists = false;
            }
        }

       
        public Boolean Upload(String dir,Boolean bol_CreateProjectFolder)
        {
            
            FileInfo _fi = null;
            DirectoryInfo _dr = null;

            try
            {
                _dr = new DirectoryInfo(dir + "\\" + SessionInfo.ProjectID);
                if (!_dr.Exists)
                {
                    _dr.Create();
                }
                _fi = new FileInfo(_dr.FullName + "\\" + fi.Name);
                fi.CopyTo(_fi.FullName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
  
    }
}