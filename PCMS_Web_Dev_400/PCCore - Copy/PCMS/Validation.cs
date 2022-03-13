using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
namespace PCCore.PCMS
{
    public class Validation
    {
        public static DataTable NavMenu()
        {
            return null;
            //return NavMenu(null);
        }
        public static DataTable NavMenu(int ModuleID)
        {
            return null;
            //return NavMenu(ModuleID, null);
        }
        public static DataTable NavMenu(int ModuleID,int FatherID)

        {
            string strsql = "SELECT * FROM ";
            if (FatherID == null)
            {
                PCDb.Db.ExecQuery(string.Format("SELECT * FROM SC_FUNCTIONS WHERE ISNULL(SHOWINNAV,0) = 1"));
            } else {

            }
            return null;
        }
        public static bool CheckProject(DropDownList ddlProject)
        {
            if (ddlProject.SelectedIndex <= 0)
                return false;
            else
                return true;
        }
        public static bool CheckProjectExist(string Pcode)
        {
            return PCDb.Db.ExecQuery(string.Format("SELECT PrjCode FROM PCMS_BE.{0}.DBO.OPRJ WHERE PrjCode = '{1}' AND Locked = 'N'", Config.SAPDatabase,Pcode)).Rows.Count > 0;
        }

        public static bool CheckProjectAccess(string fid, string uid, string Pcode)
        {
            return PCDb.Db.ExecQuery(string.Format("SELECT projcode from v_sc_rights where fid = '{0}' and (FALL = 1 OR FNEW = 1) and userid = {1} and projcode = '{2}'", fid, uid, Pcode)).Rows.Count > 0;
        }

        public static bool CheckVendor(DropDownList ddlVendor)
        {
            if (ddlVendor.SelectedIndex <= 0)
                return false;
            else
                return true;
        }
        public static bool CheckSubContract(DropDownList ddlSubContract)
        {
            if (ddlSubContract.SelectedIndex <= 0)
                return false;
            else
                return true;
        }
        public static bool CheckSubContractor(DropDownList ddlSubContractor)
        {
            if (ddlSubContractor.SelectedIndex <= 0)
                return false;
            else
                return true;
        }
        public static bool CheckPurchaseAgreement(DropDownList ddlPA)
        {
            if (ddlPA.SelectedIndex <= 0)
                return false;
            else
                return true;
        }
        public static bool CheckSection(DropDownList ddlSectionCode)
        {
            if (ddlSectionCode.SelectedIndex <= 0)
                return false;
            else
                return true;
        }

    }
}
