using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

namespace PCCore.Database
{
    public class SC_Announcements
    {
        public static string header_tbl_name = "SC_ANNOUNCEMENT";
        public static string detail_tbl_name = "SC_ANNOUNCEMENTDTL";
        public static Hashtable GetHeaderHashTable()
        {
            Hashtable row;
            DataTable _dt;
            string sql = string.Format("SELECT * FROM {0} WHERE 1 = 2", SC_Announcements.header_tbl_name);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    row = PCCore.Database.Tools.CreateHashTable(_dt);
                    return row;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("GetHeaderHashTable", ex);
                return null;
            }

        }
        public static Hashtable GetDetailHashTable()
        {
            Hashtable row;
            DataTable _dt;
            string sql = string.Format("SELECT * FROM {0} WHERE 1 = 2", SC_Announcements.detail_tbl_name);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    row = PCCore.Database.Tools.CreateHashTable(_dt);
                    return row;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("GetHeaderHashTable", ex);
                return null;
            }

        }
        public static void WriteData(Hashtable row)
        {
            
            foreach (Object o in row.Keys)
            {
                PCCore.Common.HRLog.RecordLog(o.ToString() + ":" + row[o.ToString()]);
            }
            
        }
        public static string AddHeader(Hashtable row)
        {
            //SC_Announcements.WriteData(row);
            PCCore.Common.HRLog.RecordLog(Consts.FieldID + ":&," + row[Consts.FieldID]);
            string sql = "";
            if (!PCCore.Database.SC_Announcements.isExists(Convert.ToString(row[Consts.FieldID])))
            {
                PCTable table = new PCTable(SC_Announcements.header_tbl_name,new ScInfo(SessionInfo.CurrentFunction));
                try
                {
                    

                    //table.BeginTransaction();
                    //table.Insert(row);
                    //table.CommitTransaction();
                    sql = PCCore.Database.Tools.GenerateInsertQuery(row, SC_Announcements.header_tbl_name);
                    PCDb.Db.ExecUpdate(sql);
                    ScLog.Insert(ScLog.Actions.Insert, Convert.ToInt32(SessionInfo.CurrentFunctionID), SessionInfo.CurrentFunction,new PCTable(SC_Announcements.header_tbl_name),null,row);
                }
                catch (Exception ex)
                {
                    PCCore.Common.HRLog.RecordException("SC_Announcement,ADDHeader", ex);
                    return ex.Message;
                   // table.RollbackTransaction();
                }

            }
            else
            {
                return PCCore.Database.SC_Announcements.UpdateHeader(row);
            }
            return string.Empty;
        }
        public static string AddDetail(Hashtable row)
        {
            //SC_Announcements.WriteData(row);
            PCCore.Common.HRLog.RecordLog(Consts.FieldID + ":&," + row[Consts.FieldID]);
            SC_Announcements.DeleteDetail(row);
            string sql = "";
            PCTable table = new PCTable(SC_Announcements.detail_tbl_name, new ScInfo(SessionInfo.CurrentFunction));
            try
            {


                //table.BeginTransaction();
                //table.Insert(row);
                //table.CommitTransaction();
                sql = PCCore.Database.Tools.GenerateInsertQuery(row, SC_Announcements.detail_tbl_name);
                PCDb.Db.ExecUpdate(sql);
                ScLog.Insert(ScLog.Actions.Insert, Convert.ToInt32(SessionInfo.CurrentFunctionID), SessionInfo.CurrentFunction, new PCTable(SC_Announcements.detail_tbl_name), null, row);
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("SC_Announcement,ADDDetail", ex);
                return ex.Message;
                // table.RollbackTransaction();
            }
        
            return string.Empty;
        }
        public static string UpdateHeader(Hashtable row)
        {
            string sql = "";
            if (PCCore.Database.SC_Announcements.isExists(Convert.ToString(row[Consts.FieldID])))
            {
                //PCTable table = new PCTable(SC_Announcements.tbl_name);
                try
                {
                    sql = PCCore.Database.Tools.GenerateUpdateQuery(row,SC_Announcements.header_tbl_name,string.Format(" id = {0} and userid = {1}",Convert.ToString(row[Consts.FieldID]),Convert.ToString(row["userid"])));
                    PCDb.Db.ExecUpdate(sql);
                    //table.Update(row);
                    ScLog.Insert(ScLog.Actions.Update, Convert.ToInt32(SessionInfo.CurrentFunctionID), SessionInfo.CurrentFunction, new PCTable(SC_Announcements.header_tbl_name), null, row);
                }
                catch (Exception ex)
                {
                    PCCore.Common.HRLog.RecordException("SC_Announcement,UpdateHeader", ex);
                    return ex.Message;
                }
            } else {
                return PCCore.Database.SC_Announcements.AddHeader(row);
            }
            return string.Empty;
        }

        public static int DeleteHeader(Hashtable row)

        {
            
            int ret;
            string sql = string.Format("DELETE FROM {0} WHERE ID= {1}", SC_Announcements.header_tbl_name, row[Consts.FieldID]);
            try
            {
                PCCore.Common.HRLog.RecordLog("Sql Query: " + sql);
                ret = PCDb.Db.ExecUpdate(sql);
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("SC_Announcements,DeleteHeader", ex);
                ret = 0;
            }

            return ret;
        }
        public static int DeleteDetail(Hashtable row)
        {

            int ret;
            string sql = string.Format("DELETE FROM {0} WHERE ID= {1} and Userid = {2}", SC_Announcements.detail_tbl_name, row[Consts.FieldID],row["USERID"]);
            try
            {
                PCCore.Common.HRLog.RecordLog("Sql Query: " + sql);
                ret = PCDb.Db.ExecUpdate(sql);
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("SC_Announcements,DeleteDetail", ex);
                ret = 0;
            }

            return ret;
        }
        public static Hashtable Find(string id, string userid)
        {
            string sql = string.Format("SELECT 1 FROM {0} WHERE ID= {1} AND USERID = {2}", SC_Announcements.detail_tbl_name, id, userid);
            DataTable _dt;
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        return PCCore.Database.Tools.ReadRows(_dt, 0);
                    }
                    else
                    {
                        return null;
                    }
                     
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("SC_Announcements,isExists", ex);
                return null;
            }
        }

        public static bool isExists(string id, string userid)
        {
            string sql = string.Format("SELECT 1 FROM {0} WHERE ID= {1} AND USERID = {2}", SC_Announcements.detail_tbl_name, id, userid);
            PCCore.Common.HRLog.RecordLog(sql);
            DataTable _dt;
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("SC_Announcements,isExists", ex);
                return false;
            }
            
        }
        public static bool isExists(string id)
        {
            string sql = string.Format("SELECT 1 FROM {0} WHERE ID= {1}", SC_Announcements.header_tbl_name, id);
            PCCore.Common.HRLog.RecordLog(sql);
            DataTable _dt;
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("SC_Announcements,isExists", ex);
                return false;
            }

        }

        public static string SaveByProject(Hashtable row,ArrayList user, decimal id, string prjcode)
        {
            PCCore.Common.HRLog.RecordLog("SaveProject:ID:" + id.ToString());
            string errmsg = String.Empty;
            Hashtable _headerRow = SC_Announcements.GetHeaderHashTable();
            Hashtable _detailRow = SC_Announcements.GetDetailHashTable();
            // Assign Header Value
            try
            {
                PCCore.Common.HRLog.RecordLog("Header Hashtable Count: " + _headerRow.Count.ToString());
                //row["isPublic"] = 0;
                //_headerrow["userid"] = o.ToString();
                _headerRow[Consts.FieldID] = id;
                _headerRow["ISPUBLIC"] = 0;
                _headerRow[Consts.FieldCreated] = DateTime.Now;
                _headerRow[Consts.FieldCreatedBy] = SessionInfo.LoginName;
                _headerRow["CREATEDID"] = SessionInfo.UserId;
                _headerRow["PRJCODE"] = prjcode;
                _headerRow["EFFECTIVEDATE"] = row["EFFECTIVEDATE"];
                

                _headerRow["EXPIRYDATE"] = row["EXPIRYDATE"];
                if (_headerRow["EXPIRYDATE"] == DBNull.Value)
                    _headerRow.Remove("EXPIRYDATE");
                _headerRow["TITLE"] = row["TITLE"].ToString();
                _headerRow[Consts.FieldModified] = DBNull.Value;
                try
                {
                    _headerRow.Remove(Consts.FieldModified);
                }
                catch (Exception ex)
                {
                    PCCore.Common.HRLog.RecordException("Remove Hashtable Field", ex);
                }
                
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                PCCore.Common.HRLog.RecordException("SaveByProject: HeaderRow", ex);
                return errmsg;
            }
            // Assign Row Value
            try
            {
                PCCore.Common.HRLog.RecordLog("Detail Hashtable Count: " + _detailRow.Count.ToString());
                _detailRow[Consts.FieldID] = id;
                _detailRow["TITLE"] = row["TITLE"].ToString();
                _detailRow["BODY"] = row["BODY"].ToString();
                _detailRow[Consts.FieldCreated] = DateTime.Now;
                _detailRow[Consts.FieldCreatedBy] = SessionInfo.UserName;
                _detailRow[Consts.FieldModified] = DBNull.Value;
                _detailRow["HASREAD"] = 0;
                try
                {
                    _detailRow.Remove("modified");
                }
                catch (Exception ex)
                {
                    PCCore.Common.HRLog.RecordException("Remove Hashtable Field", ex);
                }
                

            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                PCCore.Common.HRLog.RecordException("SaveByProject: DetailRow", ex);
                return errmsg;
            }

            errmsg = String.Empty;
            // Save Header
            if (!PCCore.Database.SC_Announcements.isExists(id.ToString()))
                errmsg = SC_Announcements.AddHeader(_headerRow);
            else
                errmsg = SC_Announcements.UpdateHeader(_headerRow);
            
            // Save Detail
            if (errmsg == string.Empty)
            {
                foreach (object o in user)
                {

                    _detailRow["USERID"] = o.ToString();
                    errmsg = SC_Announcements.AddDetail(_detailRow);
                    if (errmsg != string.Empty)
                        return errmsg;
                }


            }
            else
            {
                return errmsg;
            }
            return string.Empty;
            
        }
        public static string SaveSystemMessage(Hashtable row, decimal id)
        {
            string errmsg = String.Empty;
            
            if (!PCCore.Database.SC_Announcements.isExists(id.ToString()))
            {
                row["ISPUBLIC"] = 1;
                row["PRJCODE"] = String.Empty;
                //row["userid"] = -1;
                row["CREATEDID"] = SessionInfo.UserId;
                row["HASREAD"] = 0;
                //row["noticetype"] = "M";
                row[Consts.FieldCreated] = DateTime.Now;
                row[Consts.FieldCreatedBy] = SessionInfo.LoginName;
                row[Consts.FieldID] = id;
                if (row["EXPIRYDATE"] == DBNull.Value)
                    row.Remove("EXPIRYDATE");
                SC_Announcements.AddHeader(row);
            }
            return errmsg;
        }

        public static string SaveByFunction(Hashtable row, decimal id)
        {
            string errmsg = String.Empty;

            if (!PCCore.Database.SC_Announcements.isExists(id.ToString()))
            {
                row["ISPUBLIC"] = 0;
                row["PRJCODE"] = String.Empty;
                //row["userid"] = -1;
                row["CREATEDID"] = SessionInfo.UserId;
                row["HASREAD"] = 0;
                //row["noticetype"] = "M";
                row[Consts.FieldCreated] = DateTime.Now;
                row[Consts.FieldCreatedBy] = SessionInfo.LoginName;
                row[Consts.FieldID] = id;
                if (row["EXPIRYDATE"] == DBNull.Value)
                    row.Remove("EXPIRYDATE");
                SC_Announcements.AddHeader(row);
            }
            return errmsg;
        }

        public static string DeleteAnnouncement(string id) 
        {
            string errmsg = "";
            string sql = string.Format("UPDATE {0} SET CANCEL = 1 WHERE ID = {1}", SC_Announcements.header_tbl_name, id);
            try
            {
                PCDb.Db.ExecUpdate(sql);
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("DeleteAnnouncement", ex);
                errmsg = ex.Message;
            }
            return errmsg;
        }
        public static string ActiveAnnouncement(string id)
        {
            string errmsg = "";
            string sql = string.Format("UPDATE {0} SET CANCEL = 0 WHERE ID = {1}", SC_Announcements.header_tbl_name, id);
            try
            {
                PCDb.Db.ExecUpdate(sql);
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("ActiveAnnouncement", ex);
                errmsg = ex.Message;
            }
            return errmsg;
        }
        public static string ReadAnnouncement(string id, string msgtype)
        {
            return SC_Announcements.ReadAnnouncement(id, msgtype, SessionInfo.UserId);
        }
        public static string ReadAnnouncement(string id,string msgtype,string userid)
        {
            PCCore.Common.HRLog.RecordLog("MsgType: " + msgtype);
            string errmsg = "";
            string sql = "";
            if (msgtype == "S")
                sql = string.Format("UPDATE {0} SET HASREAD = 1 WHERE ID = {1}", SC_Announcements.header_tbl_name, id);
            else
                sql = string.Format("UPDATE {0} SET HASREAD = 1 WHERE ID = {1} AND USERID = {2}", SC_Announcements.detail_tbl_name, id,userid);
            try
            {
                PCDb.Db.ExecUpdate(sql);
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("ReadAnnouncement", ex);
                errmsg = ex.Message;
            }
            return errmsg;
        }
        public static string UnReadAnnouncement(string id, string msgtype)
        {
            return SC_Announcements.UnReadAnnouncement(id, msgtype, SessionInfo.UserId);
        }
        public static string UnReadAnnouncement(string id, string msgtype, string userid)
        {
            PCCore.Common.HRLog.RecordLog("MsgType: " + msgtype);
            string errmsg = "";
            string sql = "";
            if (msgtype == "S")
                sql = string.Format("UPDATE {0} SET HASREAD = 0 WHERE ID = {1}", SC_Announcements.header_tbl_name, id);
            else
                sql = string.Format("UPDATE {0} SET HASREAD = 0 WHERE ID = {1} AND USERID = {2}", SC_Announcements.detail_tbl_name, id, userid);
            try
            {
                PCDb.Db.ExecUpdate(sql);
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("ReadAnnouncement", ex);
                errmsg = ex.Message;
            }
            return errmsg;
        }

        public static DataRow getRecord(string id, string msgtype, string userid)
        {
            DataTable _dt;
            
            string sql = string.Empty;
            switch (msgtype)
            {
                case "S": // System
                    sql = string.Format("SELECT AH.ID,AH.USERID,AH.TITLE,AH.BODY,AH.EFECTIVEDATE,AH.EXPIRYDATE,AH.PRJCODE, U.FULLNAME AS SENDERNAME FROM SC_ANNOUNCEMENT AH LET JOIN SC_USER U ON U.ID = AH.CREATEDID WHERE ID = {0}", id);
                    break;
                case "U": // User
                    sql = string.Format("SELECT AD.ID,AD.USERID,AH.TITLE,AD.BODY,AH.CREATEDID,U.FULLNAME AS SENDERNAME, AH.PRJCODE FROM SC_ANNOUNCEMENT AH INNER JOIN SC_ANNOUNCEMENTDTL AD ON AD.ID = AH.ID LEFT JOIN SC_USER U ON U.ID = AH.CREATEDID WHERE AH.ID = {0} AND AD.USERID = {1} ", id, userid);
                    break;
                case "F": // Function
                    sql = string.Format("SELECT AH.ID,AH.USERID,AH.TITLE,AH.BODY,AH.EFECTIVEDATE,AH.EXPIRYDATE,AH.PRJCODE, U.FULLNAME AS SENDERNAME FROM SC_ANNOUNCEMENT AH LET JOIN SC_USER U ON U.ID = AH.CREATEDID WHERE ID = {0}", id);
                    break;

            }
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt.Rows.Count > 0)
                {
                    return _dt.Rows[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("SC_Announcement.getRecord", ex);
                return null;
            }

        }

        
    }

}
