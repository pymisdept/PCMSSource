using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Threading;
using System.Data;
using System.Data.Common;
using SimpleControls.SimpleDatabase;
using SimpleControls.Web;
using SimpleControls;
using PCCore.PCMS;

namespace PCCore.Database
{

    public class ValidationList
    {
        //public const string project_sql = "SELECT PRJCODE,PRJNAME FROM OPRJ WHERE ISNULL(LOCKED,'N') = 'N'";
        //public const string project_sql = "SELECT PRJCODE,ISNULL(PRJCODE,'') + ' - ' + ISNULL(PRJNAME,'') AS PRJNAME FROM OPRJ WHERE ISNULL(LOCKED,'N') = 'N'";
        //public const string project_sql = "SELECT PRJCODE,ISNULL(PRJCODE,'') + ' - ' + ISNULL(PRJNAME,'') AS PRJName FROM OPRJ ORDER BY PRJCODE";
        public const string project_sql = "SELECT PRJCODE, PRJDesc FROM CPS_View_ProjectList ORDER BY PRJCODE";
        
        #region PurchaseAgreement
        /*
        public static void PACodeList(DropDownList ddlpacode, string projcode)
        {
            SAPDb.InitDropDownList(ddlpacode, "CPS_View_OSPurchaseAgreement", "PCMSDOCNUM", "PCMSDOCNUM", 0, "", string.Format("PrjCode = '{0}'", projcode));
        }*/

        public static void PACodeList(DropDownList ddlpacode, string projcode, int _dft, string flow)
        {
            SAPDb.InitDropDownList(ddlpacode, "CPS_View_OSPurchaseAgreement", "PCMSDOCNUM", "PCMSDOCNUM", _dft, null, string.Format("PrjCode = '{0}' and flow = '{1}'", projcode, flow));
        }

        /*
        public static void PACodeList(DropDownList ddlpacode)
        {
            SAPDb.InitDropDownList(ddlpacode, "CPS_View_OSPurchaseAgreement", "PCMSDOCNUM", "PCMSDOCNUM", 0, "", string.Format(" PRJCODE IN ('{0}','{1}')", "-1","-2"));
        }*/

        public static void PACodeList(DropDownList ddlpacode, string projcode, string flow)
        {
            PACodeList(ddlpacode, projcode, Consts.DropDownOptionNone, flow);
        }

        #endregion
        #region PurchaseAgreement
        /*
        public static void SMRList(DropDownList ddlmrno, string projectcode, int _dft)
        {
            SAPDb.InitDropDownList(ddlmrno, "CPS_View_PrjSMRList", "PCMSDocNum", "PCMSDocNum", _dft, null, string.Format(" prjCode in('{0}')", projectcode));
        }*/
        public static void SMRList(DropDownList ddlmrno, string projectcode, int _dft, string flow)
        {
            SAPDb.InitDropDownList(ddlmrno, "CPS_View_PrjSMRList", "PCMSDocNum", "PCMSDocNum", _dft, null, string.Format(" prjCode in('{0}') and flow = '{1}'", projectcode, flow));
        }
        public static void SMRList(DropDownList ddlmrno, string projectcode, string flow)
        {
            SMRList(ddlmrno, projectcode, Consts.DropDownOptionNone, flow);
        }


        public static void POList(DropDownList ddlmrno, string projectcode, int _dft)
        {
            SAPDb.InitDropDownList(ddlmrno, "CPS_View_OSPurchaseOrder", "PCMSDocNum", "PCMSDocNum", _dft, null, string.Format(" prjCode in('{0}')", projectcode));
        }
        public static void POList(DropDownList ddlmrno, string projectcode)
        {
            POList(ddlmrno, projectcode, Consts.DropDownOptionNone);
        }


        public static void OpenVendorList(DropDownList ddlmrno, string projectcode, int _dft, string flow)
        {
            SAPDb.InitDropDownList(ddlmrno, "CPS_View_OpenVendorList", "CardCode", "CardName", _dft, null, string.Format(" prjCode in('{0}') and sourcetype = '{1}'", projectcode, flow));
        }
        public static void OpenVendorList(DropDownList ddlmrno, string projectcode, string flow)
        {
            OpenVendorList(ddlmrno, projectcode, Consts.DropDownOptionNone, flow);
        }



        public static void COList(DropDownList ddlmrno, string projectcode, int _dft)
        {
            SAPDb.InitDropDownList(ddlmrno, "CPS_View_OSConfirmationOrder", "PCMSDocNum", "PCMSDocNum", _dft, null, string.Format(" prjCode in('{0}')", projectcode));
        }
        public static void COList(DropDownList ddlmrno, string projectcode)
        {
            COList(ddlmrno, projectcode, Consts.DropDownOptionNone);
        }
        

        public static void GRList(DropDownList ddlmrno, string projectcode, int _dft)
        {
            SAPDb.InitDropDownList(ddlmrno, "CPS_View_OSGoodsReceived", "PCMSDocNum", "PCMSDocNum", _dft, null, string.Format(" prjCode in('{0}')", projectcode));
        }
        public static void GRList(DropDownList ddlmrno, string projectcode)
        {
            GRList(ddlmrno, projectcode, Consts.DropDownOptionNone);
        }
        
        public static void ProjectList(DropDownList ddlProject)
        {
            ProjectList(ddlProject, Consts.DropDownOptionAll);
        }

        public static void ProjectList(DropDownList ddlProject, int _dft)
        {
            //SAPDb.InitDropDownList(ddlProject, "CPS_View_ProjectList", "PrjCode", "PrjName", _dft, null, string.Format("isnull(U_STATUS,'{1}') = '{1}' and prjCode in('{0}')", SessionInfo.ProjectID.Replace(",", "','"),Consts.PROJECT_OPEN));
            //SAPDb.InitDropDownList(ddlProject, "CPS_View_ProjectList", "PrjCode", "PrjName", _dft, null, string.Format("isnull(DocStatus,'{1}') = '{1}' and prjCode in('{0}')", SessionInfo.ProjectID.Replace(",", "','"), Consts.PROJECT_OPEN));

            //SAPDb.InitDropDownList(ddlProject, "CPS_View_ProjectList", "PrjCode", "PrjDesc", _dft, null, string.Format("isnull(DocStatus,'{1}') = '{1}' and prjCode in('{0}')", SessionInfo.ProjectID.Replace(",", "','"), Consts.PROJECT_OPEN));
            SAPDb.InitDropDownList(ddlProject, "CPS_View_ProjectList", "PrjCode", "PrjDesc", _dft, null, string.Format("isnull(DocStatus,'{0}') = '{0}'", Consts.PROJECT_OPEN));
            
        }

        public static void ReportProjectList(DropDownList ddlProject)
        {
            ReportProjectList(ddlProject, Consts.DropDownOptionAll);
        }

        public static void ReportProjectList(DropDownList ddlProject, int _dft)
        {
            PCCore.Common.HRLog.RecordLog("SAPDB:" + SAPDb.Db.ConnectionString);
            //SAPDb.InitDropDownList(ddlProject, "CPS_View_ProjectList", "PrjCode", "PrjName", _dft, null, string.Format("isnull(U_STATUS,'{1}') = '{1}' and prjCode in('{0}')", SessionInfo.ProjectID.Replace(",", "','"),Consts.PROJECT_OPEN));
            if (SessionInfo.IsSupervisor)
            {
                SAPDb.InitDropDownList(ddlProject, "CPS_View_ProjectList", "DocEntry", "PrjDesc", _dft, null, null);
            }
            else
            {
                SAPDb.InitDropDownList(ddlProject, "CPS_View_ProjectList", "DocEntry", "PrjDesc", _dft, null, string.Format(" prjCode in('{0}')", SessionInfo.ProjectID.Replace(",", "','"), Consts.PROJECT_OPEN));
            }
            
            //SAPDb.InitDropDownList(ddlProject, "CPS_View_ProjectList", "DocEntry", "PrjCode", _dft, null, string.Format(" prjCode in('{0}')", SessionInfo.ProjectID.Replace(",", "','"), Consts.PROJECT_OPEN));
        }
        #endregion


        public static void PMRptPeriodList(DropDownList ddlPeriod, int _dft, string prjcode)
        {
            PCDb.InitDropDownList(ddlPeriod, "v_PMRPTList", PMReportTable.PMR_COMM_ID, "PERIOD", _dft, null, string.Format("prjCode in('{0}')", prjcode));
        }
        public static void SubContractorList(DropDownList ddl, string projectcode)
        {
            SubContractorList(ddl, projectcode, Consts.DropDownOptionAll);
        }
        public static void SubContractorList(DropDownList ddl)
        {
            SubContractorList(ddl, Consts.DropDownOptionAll);
        }
        public static void SubContractorList(DropDownList ddl, int _dft)
        {
            SAPDb.InitDropDownList(ddl, "CPS_View_SubContractorList", "CARDCODE", "CARDNAME", _dft, null);
        }

        // Added by Ken, 20151111, begin
        public static void CustomerList(DropDownList ddl, int _dft)
        {
            SAPDb.InitDropDownList(ddl, "CPS_View_ClientList", "CARDCODE", "CARDNAME", _dft, null);
        }
        // Added by Ken, 20151111, end

        public static void VendorList(DropDownList ddl)
        {
            VendorList(ddl, Consts.DropDownOptionAll);
        }
        public static void VendorList(DropDownList ddl, int _dft)
        {
            SAPDb.InitDropDownList(ddl, "CPS_View_MaterialSupplierList", "CARDCODE", "CARDNAME", _dft, null);
        }

        public static void SubContractorList(DropDownList ddl, string projectcode, int _dft)
        {
            SAPDb.InitDropDownList(ddl, "CPS_View_OSPrjSubContractor", "CARDCODE", "CARDNAME", _dft, null, string.Format("Project = '{0}'", projectcode));
        }
        public static void SectionList(DropDownList ddl)
        {
            SectionList(ddl, Consts.DropDownOptionAll);
        }
        public static void SectionList(DropDownList ddl, int _dft)
        {
            //SAPDb.InitDropDownList(ddl, "CPS_View_SectionList", "SectionCode", "SectionName", _dft, null);
            SAPDb.InitDropDownList(ddl, "CPS_View_WorksList", "WorksCode", "WorksName", _dft, null);
        }
        public static void FiscalSectionList(DropDownList ddl, int _dft)
        {
            //SAPDb.InitDropDownList(ddl, "CPS_View_SectionList", "SectionCode", "SectionName", _dft, null);
            SAPDb.InitDropDownList(ddl, "CPS_View_FiscalSectionList", "WorksCode", "WorksName", _dft, null);
        }

        public static void QSSectionList(DropDownList ddl, int _dft)
        {
            SAPDb.InitDropDownList(ddl, "CPS_View_QSSectionList", "SectionCode", "SectionName", _dft, null);
        }

        public static void SubContractorNoList(DropDownList ddl, string projectcode, string cardcode)
        {
            SubContractorNoList(ddl, projectcode, cardcode, Consts.DropDownOptionAll);
        }

        public static void SubContractorNoList(DropDownList ddl, string projectcode, string cardcode, int _dft)
        {
            //SAPDb.InitDropDownList(ddl, "CPS_View_OSSubContract", "DOCNUM", "CONTRACTNAME", _dft, null, string.Format("PROJECT = '{0}' AND CARDCODE = '{1}'", projectcode, cardcode));
            //SAPDb.InitDropDownList(ddl, "CPS_View_OSSubContract", "PCMSDocNum", "PCMSDocNum", _dft, null, string.Format("PROJECT = '{0}' AND CARDCODE = '{1}'", projectcode, cardcode));
            SAPDb.InitDropDownList(ddl, "CPS_View_OSSubContract", "PCMSDocNum", "PCMSDocNum", _dft, null, string.Format("PrjCode = '{0}' AND CARDCODE = '{1}'", projectcode, cardcode));
        }
        public static void UserProjectList(DropDownList ddl, string userid)
        {
            
            PCDb.InitDropDownList(ddl, "CPS_View_UserProject", "PrjCode", "PrjName", 0, "", string.Format("Userid = {0}", userid));
        }
        public static void UserProjectList(DropDownList ddl, string userid, int opt)
        {
            PCDb.InitDropDownList(ddl, "CPS_View_UserProject", "PrjCode", "PrjName", opt, "", string.Format("Userid = {0}", userid));
        }
        public static void GroupProjectList(DropDownList ddl, string groupid)
        {
            PCDb.InitDropDownList(ddl,"CPS_View_GroupProject","PrjCode","PrjName",Consts.DropDownOptionAll,"",string.Format("ID = {0}",groupid));
        }
        public static string FunctionFileType(string funtionid)
        {
            string strsql = string.Format("SELECT FILETYPE FROM {0} WHERE ID = {1}", "v_sc_functions", funtionid);
            PCCore.Common.HRLog.RecordLog(strsql);
            DataTable _dt;
            try
            {
                _dt = PCDb.Db.ExecQuery(strsql);
                if (_dt.Rows.Count > 0)
                    return (_dt.Rows[0][0]).ToString();
                else
                    return funtionid;
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("FunctionFileType", ex);
                return funtionid;
            }

        }

        public static DataTable FunctionList(int mid)
        {
            DataTable _dt = null;
            try
            {
                _dt = PCDb.Db.ExecQuery(string.Format("SELECT '' as Link,'' as Title,* FROM (0) where Mid = {1} and {2} ", "CPS_View_SCFunctionInNav", mid, " 1 = 1"));
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Function List", ex);
            }
            return _dt;

        }
        public static void TemplateList(DropDownList dll, string mid)
        {
            if (mid != "" && mid != null)
                PCDb.InitDropDownList(dll, "CPS_View_UserTemplateList", "FID", "FCODE", Consts.DropDownOptionNone, null, string.Format(" userid = {0} and isTemplateFunction = 1 and showInNav = 1 and {1} and mid = {2}", SessionInfo.UserId, Consts.Filter_Function_List, mid), true);
            else
                PCDb.InitDropDownList(dll, "CPS_View_UserTemplateList", "FID", "FCODE", Consts.DropDownOptionNone, null, string.Format(" userid = {0} and isTemplateFunction = 1 and showInNav = 1 and {1} ", SessionInfo.UserId, Consts.Filter_Function_List), true);
        }
        public static void TemplateList(DropDownList dll)
        {
            PCDb.InitDropDownList(dll, "CPS_View_UserTemplateList", "FID", "FCODE", Consts.DropDownOptionNone, null, string.Format(" userid = {0} and isTemplateFunction = 1 and showInNav = 1 and {1}", SessionInfo.UserId, Consts.Filter_Function_List), true);
        }

        public static string TemplateFunctionID()
        {
            DataTable _dt;
            string _funcId = "";
            string sql = string.Format("select fid from CPS_ViewUserTemplateList where userid = {0}", SessionInfo.UserId);
            try
            {
                _dt = PCCore.PCDb.Db.ExecQuery(sql);
                if (_dt != null && _dt.Rows.Count > 0)
                {
                    foreach (DataRow _dr in _dt.Rows)
                    {
                        _funcId += "'" + _dr[0].ToString() + "',";
                    }
                    _funcId = _funcId.Substring(0, _funcId.Length - 1);
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("TemplateFunctionList", ex);
            }
            return _funcId;
        }

        public static void DropDownMinimumIndex(PCCore.DropDownList dll)
        {
           
        }
        public static void AllowAddDropDownProjectList(PCCore.DropDownList dll,int _dft)
        {
             //SAPDb.InitAddDocProjectList(dll, "CPS_View_ProjectList", "PrjCode", "PrjName", _dft, null, string.Format(" prjCode in('{0}')", SessionInfo.ProjectID.Replace(",", "','"), Consts.PROJECT_OPEN));
            SAPDb.InitAddDocProjectList(dll, "CPS_View_ProjectList", "PrjCode", "PrjDesc", _dft, null, string.Format(" prjCode in('{0}')", SessionInfo.ProjectID.Replace(",", "','"), Consts.PROJECT_OPEN));
        }
        public static void AllowQueryDropDownProjectList(PCCore.DropDownList dll, int _dft)
        {
            //SAPDb.InitAddDocProjectList(dll, "CPS_View_ProjectList", "PrjCode", "PrjName", _dft, null, string.Format(" prjCode in('{0}')", SessionInfo.ProjectID.Replace(",", "','"), Consts.PROJECT_OPEN));
            SAPDb.InitQueryDocProjectList(dll, "CPS_View_ProjectList", "PrjCode", "PrjDesc", _dft, null, string.Format(" prjCode in('{0}')", SessionInfo.ProjectID.Replace(",", "','"), Consts.PROJECT_OPEN));
        }

        public static void AnnouncementFunctionList(DropDownList ddl)
        {
            PCDb.InitDropDownList(ddl, "CPS_View_AnnouncementFuncList", "FUNCCODE", "FUNCNAME", Consts.DropDownOptionNone, null, null, true);
        }
        public static void SetProjectCheckBoxListItems(CheckBoxList cbl)
        {
            DataTable _dt;
            string sql = "";
            if (SessionInfo.IsSupervisor)
            {
                sql = "SELECT PrjCode,PrjDesc FROM CPS_View_ProjectList ORDER BY PRJCODE";
            }
            else
            {
                sql = string.Format("SELECT PrjCode,PrjDesc FROM CPS_View_ProjectList where prjCode in ('{0}') ORDER BY PRJCODE", SessionInfo.ProjectID.Replace(",", "','"));
            }
            try
            {
                _dt = SAPDb.Db.ExecQuery(sql);
                cbl.Items.Clear();
                if (_dt != null)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        ListItem li = new ListItem(_dt.Rows[i][1].ToString(), _dt.Rows[i][0].ToString());
                        cbl.Items.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("setProjectCheckBoxListItems", ex);
            }



        }

        public static void SourceUserList(DropDownList dll, int sel_userid)
        {
            PCDb.InitDropDownList(dll, "SC_User", "ID", "loginname", Consts.DropDownOptionNone, null, string.Format(" id <> {0} and isNull(Supervisor,0) = 0 ", sel_userid)," loginname asc");
            //ValidationList.ProjectList(dll, Consts.DropDownOptionNone);
            //PCDb.InitDropDownList(dll, "SC_User", "ID", "loginname", Consts.DropDownOptionNone, null, null);
            //SAPDb.InitAddDocProjectList(dll, "CPS_View_ProjectList", "PrjCode", "PrjDesc", Consts.DropDownOptionNone, null, string.Format(" prjCode in('{0}')", SessionInfo.ProjectID.Replace(",", "','"), Consts.PROJECT_OPEN));
        }

    }
}
    


    
    
    

    
