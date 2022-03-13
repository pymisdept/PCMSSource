using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using PCCore;
using SimpleControls.Web;
using SimpleControls.SimpleDatabase;
using System.Data.SqlClient;
using System.Data.Common;
using SimpleControls;

//using System.Diagnostics;

/// <summary>
/// DownloadTemplate: Procedure of receive excel template from PCMS
/// </summary>
//public partial class RequestDownload : BasePage
public partial class RequestDownload : Page
{

    //protected override void OnInit(EventArgs e)
    protected void OnInit(EventArgs e)
    {
        //ShowWebMenu = false;
        
        //base.OnInit(e);
    }

    // Request Parameter Mandatory
    string FunctionID = "";
    string FunctionCode = "";
    string ProjectCode = "";
    // Request Parameter Optional
    string CertType = "";
    string SubContractor = "";
    string SubContract = "";
    string PANum = "";
    string InputType = "";
    string MRNO = "";
    string VendorCode = "";
    string SectionCode = "";
    // Added by Ken, 20151111, begin
    string CustCode = "";
    // Added by Ken, 20151111, end

    // Template Info
    string StrPath = "";
    string StrFileName = "";
    string FullName = "";
    string _strParameter;
    string strParameter = "";
    Hashtable _htParameter;
    string Manual;
    string CutOffDate;
    
    
    protected void Page_Load(object sender, EventArgs e)
    {
        string AllownDln = "Y";

        StrPath = Resources.TemplateInfo.TEMPLATEPATH;
        
        
        if (!Page.IsPostBack)
        {
            PCCore.Common.HRLog.RecordLog("SessionInfo.isLogin," + SessionInfo.IsLogin);
            if (SessionInfo.IsLogin)
            {
                /*********************************************************/
                /*get request download paramater from [MasterForm.master]*/
                /*********************************************************/
                if (Page.Request.QueryString != null && Page.Request.QueryString.Count > 0)
                {
                    FunctionCode = Page.Request.QueryString["FunctionCode"] as string;
                    FunctionID = Page.Request.QueryString["FunctionID"] as string;
                    ProjectCode = Page.Request.QueryString["PrjCode"] as string;

                    CertType = Page.Request.QueryString["CertType"] as string;
                    SubContractor = Page.Request.QueryString["SubContractor"] as string;
                    SubContract = Page.Request.QueryString["SubContractNo"] as string;
                    PANum = Page.Request.QueryString["PA"] as string;
                    MRNO = Page.Request.QueryString["MRNO"] as string;
                    VendorCode = Page.Request.QueryString["Vendor"] as string;
                    InputType = Page.Request.QueryString["InputType"] as string;
                    SectionCode = Page.Request.QueryString["SectionCode"] as string;
                    this.CutOffDate = Page.Request.QueryString["CutOffDate"] as string;
                    // Added by Ken, 20151111, begin
                    CustCode = Page.Request.QueryString["CustCode"] as string;
                    // Added by Ken, 20151111, end

                    if (!string.IsNullOrEmpty(Page.Request.QueryString["Manual"]))
                    {
                        Manual = Page.Request.QueryString["Manual"] as string;
                        //Debug.WriteLine("have manual " + Manual);
                    }
                    else
                    {
                        //Debug.WriteLine("No manual");
                        Manual = "";
                    }
                    
                    //if (FunctionCode != "PUI05")
                    //    StrFileName = GetGlobalResourceObject("TemplateInfo", FunctionCode + "_Template").ToString();
                    //else
                    //{
                    //    try
                    //    {
                    //        string s_filename = Resources.TemplateInfo.PUI05_Template;
                    //        string s_file = s_filename.Substring(0, s_filename.LastIndexOf("."));
                    //        string s_extend = s_filename.Substring(s_filename.LastIndexOf("."));
                    //        StrFileName = s_file + "_" + extend.Trim() + s_extend.Trim();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        throw ex;
                    //    }
                    //}

                    //**
                    // if download Payment Application, not allow download if last payment cert not post.
                    //**
                    //if (FunctionID == "1017")
                    //{
                    //    string _connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

                    //    string[] dbs = null;
                    //    dbs = Config.Database.Split(new char[] { ',' });

                    //    _connectionString = string.Format(_connectionString, dbs[0]);
                    //    string queryString = "select count(*) from documentproperty where type=1018 and projectcode='" + ProjectCode + "' and docstatus='PPFA'";

                    //    using (SqlConnection connection = new SqlConnection(_connectionString))
                    //    {
                    //        SqlCommand command = connection.CreateCommand();
                    //        command.CommandText = queryString;
                    //        connection.Open();

                    //        int rCount = (int)command.ExecuteScalar();
                    //        if (rCount > 0)
                    //        {
                    //            AllownDln = "N";
                    //        }

                    //        connection.Close();
                    //    }
                    //}

                    if (AllownDln == "Y")
                    {

                        /*********************************************************/
                        /*prepare request download paramater to [DlnRequest.cs]  */
                        /*********************************************************/
                        PCCore.Common.HRLog.RecordLog("Prepare request download");
                        if (FunctionID != "" && ProjectCode != "" && FunctionID != "")
                        {
                            _htParameter = new Hashtable();
                            _htParameter.Add("PrjCode", ProjectCode);
                            _htParameter.Add("FunctionCode", FunctionCode.ToUpper());
                            _htParameter.Add("FunctionID", FunctionID);
                            if (Manual != null && Manual != String.Empty)
                            {
                                if (Manual == "M")
                                    _htParameter.Add("Manual", "M");
                                else
                                    _htParameter.Add("Manual", "N");
                            }
                            if (CutOffDate != null && CutOffDate != String.Empty)
                                _htParameter.Add("CutOffDate", CutOffDate);
                            if (CertType != null && CertType != String.Empty)
                                _htParameter.Add("CertType", CertType);
                            if (SubContractor != null && SubContractor != String.Empty)
                                _htParameter.Add("SubContractor", SubContractor);
                            if (SubContract != null && SubContract != String.Empty)
                                _htParameter.Add("SubContract", SubContract);
                            if (PANum != null && PANum != String.Empty)
                                _htParameter.Add("PANum", PANum);
                            if (InputType != null && InputType != String.Empty)
                                _htParameter.Add("InputType", InputType);
                            if (MRNO != null && MRNO != string.Empty)
                                _htParameter.Add("MRNO", MRNO);
                            if (VendorCode != null && VendorCode != String.Empty)
                                _htParameter.Add("Vendor", VendorCode);
                            if (SectionCode != null && SectionCode != String.Empty)
                                _htParameter.Add("SectionCode", SectionCode);
                            // Added by Ken, 20151111, begin
                            if (CustCode != null && SectionCode != String.Empty)
                                _htParameter.Add("CustCode", CustCode);
                            // Added by Ken, 20151111, end


                            PCCore.Common.HRLog.RecordLog("Create Download Request");
                            PCCore.PCMS.DlnRequest _dlnRequest = new PCCore.PCMS.DlnRequest(ProjectCode, FunctionID, _htParameter);
                            if (!_dlnRequest.Create())
                            {

                            }
                            else
                            {
                                Page.RegisterStartupScript("CloseForm", "<script language='javascript'>{self.close();}</script>");
                            }

                        }
                    }
                    else
                    {
                        //Error Log 
                        PCCore.Common.HRLog.RecordLog("CloseForm");
                        //Page.RegisterStartupScript("returnValue", "<script language='javascript'>{window.returnValue='error';}</script>");
                        //Page.RegisterStartupScript("AlertSession", "<script language='javascript'>{alert('Session Timeout. Please login again.');}</script>");
                        Page.RegisterStartupScript("CloseForm", "<script language='javascript'>{alert('Previous Payment Certificate not yet post, not allow download Payment Application'); window.returnValue='error'; self.close();}</script>");
                    }
                    //FullName = Server.MapPath(StrPath + "\\" + StrFileName);

                    //try
                    //{
                    //    SimpleWebUtils.DownloadFile(Page.Response, FullName);
                    //}
                    //catch (Exception Ex)
                    //{
                    //    //Log: Fail to Download Excel Template
                    //    throw Ex;
                    //}

                }
            }
            else
            {
                PCCore.Common.HRLog.RecordLog("CloseForm");
                //Page.RegisterStartupScript("returnValue", "<script language='javascript'>{window.returnValue='error';}</script>");
                //Page.RegisterStartupScript("AlertSession", "<script language='javascript'>{alert('Session Timeout. Please login again.');}</script>");
                Page.RegisterStartupScript("CloseForm", "<script language='javascript'>{self.close();}</script>");
            }
        }
        else
        {

        }

    }


}

