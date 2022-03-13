using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web;
using System.Configuration;
using System.Data.Common;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using PCCore.Common;
using PCCore;

//using System.Diagnostics;

namespace PCCore.PCMS
{

    public class DlnRequest
    {
        PCTable table;
        Hashtable _htParameter;
        string strProjCode;
        string strFunctionID;
        string strParameter;
        string strSpearate = ",";
         

        public DlnRequest()
        {
            table = new PCTable(Consts.TableDownloadManager);
        }


        /*********************************************************/
        /*prepare request download paramater to write to PCMS    */
        /*********************************************************/
        public DlnRequest(string strProjCode,string strFunctionID,Hashtable _htParameter)
        {
            this._htParameter = _htParameter;
            table = new PCTable(Consts.TableDownloadManager);
            this.strProjCode = strProjCode;
            this.strFunctionID = strFunctionID;
            // Covert Hashtable Parameter to String
            strParameter = "";
            bool isFirst = true;
            if (_htParameter != null)
            {
                //SubContractor=-2,FunctionCode=QSI18,PrjCode=08019,FunctionID=1018
                if (_htParameter["PrjCode"] != null)
                    strParameter += _htParameter["PrjCode"].ToString();
                if (_htParameter["FunctionID"] != null)
                    strParameter += "," + _htParameter["FunctionID"].ToString();
                if (_htParameter["PANum"] != null)
                    strParameter += "," + _htParameter["PANum"].ToString();
                if (_htParameter["CertType"] != null)
                    strParameter += "," + _htParameter["CertType"].ToString();
                if (_htParameter["SubContractor"] != null)
                    strParameter += "," + _htParameter["SubContractor"].ToString();
                if (_htParameter["SubContract"] != null)
                    strParameter += "," + _htParameter["SubContract"].ToString();
                if (_htParameter["InputType"] != null)
                    strParameter += "," + _htParameter["InputType"].ToString();
                if (_htParameter["MRNO"] != null)
                    strParameter += "," + _htParameter["MRNO"].ToString();
                if (_htParameter["Vendor"] != null)
                    strParameter += "," + _htParameter["Vendor"].ToString();
                if (_htParameter["SectionCode"] != null)
                    strParameter += "," + _htParameter["SectionCode"].ToString();
                if (_htParameter["Manual"] != null)
                    strParameter += "," + _htParameter["Manual"].ToString();
                if (_htParameter["CutOffDate"] != null)
                    strParameter += "," + _htParameter["CutOffDate"].ToString();
                if (_htParameter["CustCode"] != null)
                    strParameter += "," + _htParameter["CustCode"].ToString().Replace("_","&");

                //Debug.WriteLine("Martin");
                //Debug.WriteLine(strParameter);
                
                //foreach (object o in _htParameter.Keys)
                //{
                //    if (!isFirst)
                //    {
                //         strParameter = strParameter + strSpearate;
                        
                //    } else
                //    {
                //        isFirst = false;
                //    }
                //    strParameter = strParameter + o.ToString() + "=" + _htParameter[o.ToString()].ToString();
                //}
            }
        }
        public Boolean Create()
        {
            
            try
            {
                //this.Insert(setRow());
                
                PCDb.Db.ExecUpdate(this.setQuery());

                return true;
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("DlnRequest.Create", ex);

                throw ex;
                return false;
            }
        }

        public string setQuery()
        {
            return string.Format("INSERT INTO {0} (id,Parameter,AllowDownload,FileType,CreateDate,CreateUser,DocStatus,Project,IPAddr) Values ({1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", Consts.TableDownloadManager, PCDb.GetNextDlnMgrID(), strParameter, "N", strFunctionID, DateTime.Now.ToString(), SessionInfo.UserId, Consts.STATUS_OPEN, strProjCode,SessionInfo.RemoteHost.ToString());
        }
        public Hashtable setRow()
        {
            Hashtable _ht = new Hashtable();

            _ht.Add("id", PCDb.GetNextDlnMgrID());
            _ht.Add("Parameter",strParameter);
            _ht.Add("Allowdownload","N");
            //_ht.Add("FileData",null);
            _ht.Add("FileType",strFunctionID);
            _ht.Add("CreateDate",DateTime.Now.ToString());
            _ht.Add("CreateUser",SessionInfo.UserId);
            _ht.Add("DocStatus",Consts.STATUS_OPEN);
            _ht.Add("Project", strProjCode);

            return _ht;
        }
        public bool Close(string id)
        {
            string sql = string.Format("UPDATE {0} SET {1} = '{2}' WHERE {3} = {4}",table.TableName,"DocStatus",Consts.STATUS_CLOSE,Consts.FieldID,id);
            try
            {
                PCDb.Db.ExecUpdate(sql);
                
                return true;
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Close Status", ex);
                return false;
            }
            //if (!table.IsTransactionReady)
            //    table.BeginTransaction();
            //System.Data.DataRow _dr = table.GetRow(id);
            //if (_dr != null)
            //{
            //    _dr["DocStatus"] = Consts.STATUS_CLOSE;
            //    table.Update(_dr);
            //    return true;
            //}
            //else
            //{
            //    PCCore.Common.HRLog.RecordLog("DlnRequest.Close: No Rows Select");
            //    return false;
            //}
            //table.CommitTransaction();
        }
    }

}
