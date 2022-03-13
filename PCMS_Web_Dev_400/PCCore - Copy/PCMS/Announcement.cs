using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PCCore;
using System.Collections.Specialized;
using System.Data;
namespace PCCore.PCMS
{
    public class Announcement
    {
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        //public static HtmlTable GetHtmlTable()
        public static void GetHtmlTable(HtmlTable _htmlt)
        {
            string lang = SessionInfo.CurrentLanguage;
            
            string _CusMessage = string.Empty;
            bool isShowAnnouncement = false;

            #region "DataSource"
            DataTable _dtAnnouncement = null;
            //HtmlTable _htmlt = new HtmlTable();
            try
            {
                _dtAnnouncement = PCDb.Db.ExecQuery(String.Format("select id,title,body,expirydate from {0} where 1=1 and effectivedate<='{1}' and (expirydate>'{1}' or expirydate is null)", "SC_ANNOUNCEMENT", DateTime.Today.ToString(Consts.DateFormat)));

            #endregion


                // width="100%" border="0" cellspacing="0" cellpadding="3"
                _htmlt.Width = "100%";
                _htmlt.Border = 0;
                _htmlt.CellSpacing = 0;
                _htmlt.CellPadding = 3;

                HtmlTableRow _trow = null;
                HtmlTableCell _tcellA = null;


                //_IsShowCusMessage = false;

                isShowAnnouncement = true;
                PCCore.Common.HRLog.RecordTable("Announcement", _dtAnnouncement);
                if (isShowAnnouncement)
                {
                    if (_dtAnnouncement != null && _dtAnnouncement.Rows.Count > 0)
                    {
                        for (int i = 0; i < _dtAnnouncement.Rows.Count; i++)
                        {
                            _trow = new HtmlTableRow();
                            _tcellA = new HtmlTableCell();
                            _tcellA.Attributes.Add("style", "font-weight:bold;");
                            _tcellA.InnerHtml = "<IMG SRC='" + Config.ThemesBaseUrl + "/Default/Images/Larr.gif'  width=\"9\" height=\"12\" align=\"absmiddle\" /> " + _dtAnnouncement.Rows[i]["title"].ToString() + "<hr size=1/>";
                            _trow.Cells.Add(_tcellA);
                            _htmlt.Rows.Add(_trow);

                            _trow = new HtmlTableRow();
                            _tcellA = new HtmlTableCell();
                            _tcellA.Attributes.Add("style", "padding-bottom:20px;");
                            _tcellA.InnerHtml = _dtAnnouncement.Rows[i]["body"].ToString().Replace("\n", "<br/>");
                            _trow.Cells.Add(_tcellA);
                            _htmlt.Rows.Add(_trow);
                        }
                    }
                    //return _htmlt;
                }
            }
            catch
            {
            }

            //return _htmlt;
        }

        public static void GetHtmlTableByFunction(HtmlTable _htmlt,string func)
        {
            string lang = SessionInfo.CurrentLanguage;
            string _CusMessage = string.Empty;
            bool isShowAnnouncement = true;

            #region "DataSource"
            DataTable _dtAnnouncement = null;
            //System.Web.UI.HtmlControls.HtmlTable _htmlt = new System.Web.UI.HtmlControls.HtmlTable();
            try
            {

                //_dtAnnouncement = PCDb.Db.ExecQuery(String.Format("select id,title,body,expirydate from {0} where 1=1 and effectivedate<='{1}' and isNull(isPublic,0) = 1 and  (expirydate>'{1}' or expirydate is null)", "SC_ANNOUNCEMENT", DateTime.Today.ToString(Consts.DateFormat)));
                PCCore.Common.HRLog.RecordLog(String.Format("select id,title,body,expirydate from CPS_View_FunctionAnnouncement where FuncCode = '{0}'", func));
                _dtAnnouncement = PCDb.Db.ExecQuery(String.Format("select id,title,body,expirydate from CPS_View_FunctionAnnouncement where FuncCode = '{0}'", func));


            #endregion


                // width="100%" border="0" cellspacing="0" cellpadding="3"
                _htmlt.Width = "100%";
                _htmlt.Border = 0;
                _htmlt.CellSpacing = 0;
                _htmlt.CellPadding = 3;

                System.Web.UI.HtmlControls.HtmlTableRow _trow = null;
                System.Web.UI.HtmlControls.HtmlTableCell _tcellA = null;


                //_IsShowCusMessage = false;

                isShowAnnouncement = true;
                if (isShowAnnouncement)
                {
                    if (_dtAnnouncement != null && _dtAnnouncement.Rows.Count > 0)
                    {
                        for (int i = 0; i < _dtAnnouncement.Rows.Count; i++)
                        {
                            _trow = new System.Web.UI.HtmlControls.HtmlTableRow();
                            _tcellA = new System.Web.UI.HtmlControls.HtmlTableCell();
                            _tcellA.Attributes.Add("style", "font-weight:bold;");
                            _tcellA.InnerHtml = "<IMG SRC='" + Config.ThemesBaseUrl + "/Default/Images/Larr.gif'  width=\"9\" height=\"12\" align=\"absmiddle\" /> " + _dtAnnouncement.Rows[i]["title"].ToString() + "<hr size=1/>";
                            _trow.Cells.Add(_tcellA);
                            _htmlt.Rows.Add(_trow);

                            _trow = new System.Web.UI.HtmlControls.HtmlTableRow();
                            _tcellA = new System.Web.UI.HtmlControls.HtmlTableCell();
                            _tcellA.Attributes.Add("style", "padding-bottom:20px;");
                            _tcellA.InnerHtml = _dtAnnouncement.Rows[i]["body"].ToString().Replace("\n", "<br/>");
                            _trow.Cells.Add(_tcellA);
                            _htmlt.Rows.Add(_trow);
                        }
                    }
                    //return _htmlt;
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("", ex);
            }

            //return _htmlt;
        }

        

    }
    
}
