using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

namespace PCCore.PCMS
{
    public class PMReportTable
    {
        public static string PMR_FD_Action = "Action";
        public static string PMR_SecInfo_TableName = "tablename";
        public static string PMR_SecInfo_RowCount = "dftRow";
        public static string PMR_SecInfo_RowByColumn = "RowByColumn";
        public static string PMR_SecInfo_allowAdd = "allowAdd";
        public static string PMR_SecInfo_allowDelete = "allowDelete";
        public static string PMR_SecInfo_HiddenField = "HiddenField";

        // PM's Report Datatable
        // Use for session
        // New Table
        public const string PMR_ProgressSummary = "CPSPMA";
        
        // New Table
        public const string PMR_Instruction = "CPSPMB";
        
        public const string PMR_CostClaimsStatus = "CPSPMC";
        
        public const string PMR_DiffReason = "CPSPMD";
        
        public const string PMR_EOTClaims = "CPSPME";
        
         // New Table
        public const string PMR_Information = "CPSPMF";
        //New Table
        public const string PMR_VarStatus = "CPSPMG";
        // New Table
        
        public const string PMR_EOTDesc = "CPSPMH";
        
        public const string PMR_Images = "CPSPMI";
        
        // New Table
        public const string PMR_Plant = "CPSPMJ";
        public const string PMR_QtyAssIssues = "CPSPMK";
          public const string PMR_LD = "CPSPML";
        public const string PMR_Mstr = "CPSPMM";
         // New Table
        public const string PMR_ManPower = "CPSPMN";
        public const string PMR_OverallProgress = "CPSPMO";
        public const string PMR_PRJPARTICULAR = "CPSPMP";
         // New Table
        public const string PMR_ResDesc = "CPSPMQ";
         // New Table
        public const string PMR_LDCmmt = "CPSPMR";
         // New Table
        public const string PMR_CostClaimsCmmt = "CPSPMT";
         // New Table
        public const string PMR_Accident = "CPSPMU";
        public const string PMR_EnvIssues = "CPSPMV";
        public const string PMR_ScopeOfWorks = "CPSPMW";
        public const string PMR_EXECSUMMARY = "CPSPMX";
         // New Table 
        public const string PMR_Penalty = "CPSPMY";
        // New Table
        public const string PMR_AccidentCmmt = "CPSPMZ";
        // Removed in new version 
        public const string PMR_KeyMilestoneDates = "CPSPMK";
       
        // Split in new version
        public const string PMR_ManpowerPlant = "CPSPMM";
        // Remove in New Version
        public const string PMR_SafetyIssues = "CPSPMS";

        public const int conTableCount = 25;
        public int TableCount;
        public Hashtable ht_PMRPT;
        public Hashtable ht_PMRPT_FD;

        /* Field Structure of PM's Report Table */
        /* Master Field */

        public const string PMR_MSTR_PCMS_DOCNUM = "PCMSDocNum";
        
        public const string PMR_MSTR_Version_Number = "Version";
        public const string PMR_MSTR_Project_Code = "PrjCode";
        public const string PMR_MSTR_Period_From = "PeriodFrom";
        public const string PMR_MSTR_Period_to = "PeriodTo";
        public const string PMR_MSTR_Create_Date = "CreateDate";
        public const string PMR_MSTR_Update_Date = "UpdateDate";
        public const string PMR_MSTR_User_Sign1 = "UserSign";
        public const string PMR_MSTR_User_Sign2 = "UserSign2";
        public const string PMR_MSTR_Document_Status = "DocStatus";
        public const string PMR_MSTR_DocumentDate = "DocDate";
        public const string PMR_MSTR_DocuemntDueDate = "DocDueDate";
        public const string PMR_MSTR_Period = "Period";
        
        
        public const string PMR_COMM_Action = "Action";

        /* Common Field*/
        public const string PMR_COMM_ID = "DocEntry";
        public const string PMR_COMM_Description = "Note";
        public const string PMR_COMM_Line_Number = "LineNum";
        public const string PMR_COMM_Visual_Order = "VisOrder";

        //public const string PMR_KEY_Description = "Description";
        
        
        /* Project Particular */
        public const string PMR_PART_ClientName = "ClientName";
        public const string PMR_PART_ArchiName = "ArchiName";
        public const string PMR_PART_DesignName = "DesignName";
        public const string PMR_PART_StructName = "StructName";
        public const string PMR_PART_BSName = "BSName";
        public const string PMR_PART_QSName = "QSName";
        public const string PMR_PART_LandscapeArchiName = "LandscapeArchiName";
        public const string PMR_PART_StartDate = "StartDate";
        public const string PMR_PART_CompDate = "CompDate";
        public const string PMR_PART_ContractDuration = "ContractDuration";
        public const string PMR_PART_TIMEELAPSED = "TimeElapsed";
        public const string PMR_PART_EOTGRANTED = "EOTGranted";
        public const string PMR_PART_ExtCompDate = "ExtCompDate";
        //public const string PMR_PART_ORG_AMT = "ContVal";
        public const string PMR_PART_FORECAST_AMT = "ForecastAmt";
        //public const string PMR_PART_CERT_AMT = "CertedAmt";

        /* Progress Summary */
        public const string PMR_PROGSUM_Code = "Code";
        public const string PMR_PROGSUM_Commerce_Date = "CommDate";
        public const string PMR_PROGSUM_Planned_Finish_Date = "PlanFinishDate";
        public const string PMR_PROGSUM_Period = "Period";
        public const string PMR_PROGSUM_Completeion = "Completeion";
        public const string PMR_PROGSUM_Anti_Plan_AntiFinish_date = "AntiFinishDate";
        public const string PMR_PROGSUM_Different_Day = "DiffDays";

        /* Instruction */
        public const string PMR_INST_Type = "Type";
        public const string PMR_INST_Receive_Cum = "RecvCumTotal";
        public const string PMR_INST_Receive_Curr = "RecvCumCurr";

        /* Information */
        public const string PMR_INFO_Type = "Type";
        public const string PMR_INFO_Receive_Cum = "RecvCumTotal";
        public const string PMR_INFO_Receive_Curr = "RecvCumCurr";
        
        /* Cost Claim */
        public const string PMR_COSTCLAIM_Value = "Value";
        public const string PMR_COSTCLAIM_Amount = "Amount";

        /* Variation Status */
        public const string PMR_VAR_Value = "Value";
        public const string PMR_VAR_Amount = "Amount";

        /* EOT Claims */
        public const string PMR_EOTC_Section = "Section";
        public const string PMR_EOTC_Contract_Day = "ContractDays";
        public const string PMR_EOTC_R_Days = "EOTRDays";
        public const string PMR_EOTC_S_Days = "EOTSDays";
        public const string PMR_EOTC_G_Days = "EOTGDays";
        public const string PMR_EOTC_Anti_G_Days = "AntiGDays";



        // Removed
        /**************************************************/
        public const string PMR_KEY_F_ORGINAL = "F_ORGINAL";
        public const string PMR_KEY_F_REVISED = "F_REVISED";
        public const string PMR_KEY_F_VARIANCE = "F_VARIANCE";

        public const string PMR_KEY_S_ORGINAL = "S_ORGINAL";
        public const string PMR_KEY_S_REVISED = "S_REVISED";
        public const string PMR_KEY_S_VARIANCE = "S_VARIANCE";
        /**************************************************/

        public const string PMR_IMG_PATH = "IMGPATH";
        public const string PMR_IMG_DESCRIPTION = "Note";
        public const string PMR_IMG_FILENAME = "FileName";

        /* Plant and ManPower */
        public const string PMR_PLANT_NAME = "Name";
        public const string PMR_PLANT_Planned_Quantity = "PlannedQty";
        public const string PMR_PLANT_ActualQty = "ActualQty";

        public const string PMR_MPW_NAME = "Position";
        public const string PMR_MPW_Planned_Quantity = "PlannedQty";
        public const string PMR_MPW_ActualQty = "ActualQty";

        /* Liqulid and Damage */
        public const string PMR_LD_Section = "Section";
        public const string PMR_LD_Days = "Days";
        public const string PMR_LD_Exposure_Amount = "ExposureAmt";
        public const string PMR_LD_Deduction_Date = "DeductDate";
        public const string PMR_LD_Anti_Amount = "AntiLDAmt";

        /* Accident */
        public const string PMR_ACCIDENT_Current_Quantity = "CurrQty";
        public const string PMR_ACCIDNET_Total_Quantity = "QtyTotal";


        /* Penalty*/
        public const string PMR_PENALTY_Current_Quantity = "CaseQty";
        public const string PMR_PENALTY_Current_Amount = "CaseAmt";
        public const string PMR_PENALTY_Summary_Quantity = "CaseSum";
        public const string PMR_PENALTY_Summary_Amount = "CaseAmtSum";
        

        public const string PMR_OPEN_STATUS = "O";
        public const string PMR_CLOSE_STATUS = "C";
        public const string PMR_CANCEL_STATUS = "X";
        public const string PMR_START_DATE = "PeriodStartDate";

        public DataTable SE_DT_PMR_ProgressSummary;
        public DataTable SE_DT_PMR_Instruction;
        public DataTable SE_DT_PMR_CostClaimsStatus;
        public DataTable SE_DT_PMR_DiffReason;
        public DataTable SE_DT_PMR_EOTClaims;
        public DataTable SE_DT_PMR_Information;
        public DataTable SE_DT_PMR_VarStatus;
        public DataTable SE_DT_PMR_EOTDesc;
        public DataTable SE_DT_PMR_Images;
        public DataTable SE_DT_PMR_Plant;
        public DataTable SE_DT_PMR_QtyAssIssues;
        public DataTable SE_DT_PMR_LD ;
        public DataTable SE_DT_PMR_Mstr;
        public DataTable SE_DT_PMR_ManPower;
        public DataTable SE_DT_PMR_OverallProgress;
        public DataTable SE_DT_PMR_PRJPARTICULAR;
        public DataTable SE_DT_PMR_ResDesc;
        public DataTable SE_DT_PMR_LDCmmt;
        public DataTable SE_DT_PMR_CostClaimsCmmt;
        public DataTable SE_DT_PMR_Accident;
        public DataTable SE_DT_PMR_EnvIssues;
        public DataTable SE_DT_PMR_ScopeOfWorks;
        public DataTable SE_DT_PMR_EXECSUMMARY;
        public DataTable SE_DT_PMR_Penalty;
        public DataTable SE_DT_PMR_AccidentCmmt;
        // Removed
        public DataTable SE_DT_PMR_KeyMilestoneDates;
        public DataTable SE_DT_PMR_ManpowerPlant;
        public DataTable SE_DT_PMR_SafetyIssues;
        
        public static string SE_HT_FIELDS_PMR_ProgressSummary = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order + "," + PMR_PROGSUM_Code + "," + PMR_PROGSUM_Commerce_Date + "," + PMR_PROGSUM_Completeion + "," + PMR_PROGSUM_Different_Day + "," + PMR_PROGSUM_Period + "," + PMR_PROGSUM_Planned_Finish_Date;
        public static string SE_HT_FIELDS_PMR_Instruction = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order + "," + PMR_INST_Receive_Cum + "," + PMR_INST_Receive_Curr ;
        public static string SE_HT_FIELDS_PMR_CostClaimsStatus = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Description + "," + PMR_COSTCLAIM_Amount + "," + PMR_COSTCLAIM_Value;
        public static string SE_HT_FIELDS_PMR_DiffReason = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order +  "," + PMR_COMM_Description;
        public static string SE_HT_FIELDS_PMR_EOTClaims =  PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order +  "," + PMR_COMM_Description + "," + PMR_EOTC_Anti_G_Days + "," + PMR_EOTC_Contract_Day + "," + PMR_EOTC_G_Days + "," + PMR_EOTC_R_Days + "," + PMR_EOTC_S_Days + "," + PMR_EOTC_Section ;
        public static string SE_HT_FIELDS_PMR_Information = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order +  "," + PMR_INFO_Type + "," + PMR_INFO_Receive_Cum + "," + PMR_INFO_Receive_Curr ;
        public static string SE_HT_FIELDS_PMR_VarStatus = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order +  "," + PMR_COMM_Description + "," +  PMR_VAR_Amount + "," + PMR_VAR_Value;
        public static string SE_HT_FIELDS_PMR_EOTDesc = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order +  "," + PMR_COMM_Description;
        public static string SE_HT_FIELDS_PMR_Images = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order +  "," + PMR_IMG_DESCRIPTION + "," + PMR_IMG_PATH + "," + PMR_IMG_FILENAME;
        public static string SE_HT_FIELDS_PMR_Plant = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order +  "," + PMR_PLANT_ActualQty + "," + PMR_PLANT_NAME + "," + PMR_PLANT_Planned_Quantity;
        public static string SE_HT_FIELDS_PMR_QtyAssIssues= PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order +  "," + PMR_COMM_Description;
        public static string SE_HT_FIELDS_PMR_LD = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order +  "," + PMR_COMM_Description;
        public static string SE_HT_FIELDS_PMR_Mstr = PMR_COMM_ID + "," +  PMR_MSTR_Project_Code + "," + PMR_MSTR_Period_From + "," + PMR_MSTR_Period_to + "," + PMR_MSTR_Period + "," + PMR_MSTR_DocumentDate + "," + PMR_MSTR_DocuemntDueDate + "," +  PMR_MSTR_Create_Date + "," + PMR_MSTR_Update_Date + "," + PMR_MSTR_User_Sign1 + "," + PMR_MSTR_User_Sign2 + "," + PMR_MSTR_Document_Status + "," + PMR_MSTR_PCMS_DOCNUM;
        public static string SE_HT_FIELDS_PMR_ManPower = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order +  "," + PMR_MPW_ActualQty + "," + PMR_MPW_NAME + "," + PMR_MPW_Planned_Quantity;
        public static string SE_HT_FIELDS_PMR_OverallProgress = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order +  "," + PMR_COMM_Description;
        public static string SE_HT_FIELDS_PMR_PRJPARTICULAR = PMR_COMM_ID + "," + PMR_PART_ArchiName + "," + PMR_PART_BSName + "," + PMR_PART_ClientName + "," + PMR_PART_CompDate + "," + PMR_PART_ContractDuration + "," + PMR_PART_DesignName + "," + PMR_PART_EOTGRANTED + "," + PMR_PART_ExtCompDate + "," + PMR_PART_FORECAST_AMT + "," + PMR_PART_LandscapeArchiName + "," + PMR_PART_QSName + "," + PMR_PART_StartDate + "," + PMR_PART_StructName + "," + PMR_PART_TIMEELAPSED;
        public static string SE_HT_FIELDS_PMR_ResDesc = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order +  "," + PMR_COMM_Description;
        public static string SE_HT_FIELDS_PMR_LDCmmt = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order +  "," + PMR_COMM_Description;
        public static string SE_HT_FIELDS_PMR_CostClaimsCmmt = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order +  "," + PMR_COMM_Description;
        public static string SE_HT_FIELDS_PMR_Accident = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order +  "," + PMR_ACCIDENT_Current_Quantity + "," + PMR_ACCIDNET_Total_Quantity;
        public static string SE_HT_FIELDS_PMR_EnvIssues = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order  + "," + PMR_COMM_Description;
        public static string SE_HT_FIELDS_PMR_ScopeOfWorks = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order +  "," + PMR_COMM_Description;
        public static string SE_HT_FIELDS_PMR_EXECSUMMARY = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order +  "," + PMR_COMM_Description;
        public static string SE_HT_FIELDS_PMR_Penalty = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order +  "," + PMR_PENALTY_Current_Amount + "," + PMR_PENALTY_Current_Quantity + "," + PMR_PENALTY_Summary_Amount + "," + PMR_PENALTY_Summary_Quantity;
        public static string SE_HT_FIELDS_PMR_AccidentCmmt = PMR_COMM_ID + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Visual_Order +  "," + PMR_COMM_Description;
        
        
        // Removed
        public static string SE_HT_FIELDS_PMR_KeyMilestoneDates = PMR_MSTR_Project_Code + "," + PMR_COMM_ID + "," + PMR_MSTR_PCMS_DOCNUM + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Description;
        public static string SE_HT_FIELDS_PMR_ManpowerPlant = PMR_MSTR_Project_Code + "," + PMR_COMM_ID + "," + PMR_MSTR_PCMS_DOCNUM + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Description;
        public static string SE_HT_FIELDS_PMR_SafetyIssues = PMR_MSTR_Project_Code + "," + PMR_COMM_ID + "," + PMR_MSTR_PCMS_DOCNUM + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Description;
        

        public PMReportTable()
        {
            initPMSReportDT();
        }
                
        private void initPMSReportDT()
        {

            ht_PMRPT_FD = new Hashtable();
            ht_PMRPT_FD.Add(PMR_ProgressSummary,SE_HT_FIELDS_PMR_ProgressSummary);
            ht_PMRPT_FD.Add(PMR_Instruction,SE_HT_FIELDS_PMR_Instruction);
            ht_PMRPT_FD.Add(PMR_CostClaimsStatus,SE_HT_FIELDS_PMR_CostClaimsStatus);
            ht_PMRPT_FD.Add(PMR_DiffReason, SE_HT_FIELDS_PMR_DiffReason);
            ht_PMRPT_FD.Add(PMR_EOTClaims, SE_HT_FIELDS_PMR_EOTClaims);
            ht_PMRPT_FD.Add(PMR_Information, SE_HT_FIELDS_PMR_Information);
            ht_PMRPT_FD.Add(PMR_VarStatus, SE_HT_FIELDS_PMR_VarStatus);
            ht_PMRPT_FD.Add(PMR_EOTDesc, SE_HT_FIELDS_PMR_EOTDesc);
            ht_PMRPT_FD.Add(PMR_Images, SE_HT_FIELDS_PMR_Images);
            ht_PMRPT_FD.Add(PMR_Plant, SE_HT_FIELDS_PMR_Plant);
            ht_PMRPT_FD.Add(PMR_QtyAssIssues, SE_HT_FIELDS_PMR_QtyAssIssues);
            ht_PMRPT_FD.Add(PMR_LD, SE_HT_FIELDS_PMR_LD);
            ht_PMRPT_FD.Add(PMR_Mstr, SE_HT_FIELDS_PMR_Mstr);
            ht_PMRPT_FD.Add(PMR_ManPower, SE_HT_FIELDS_PMR_ManPower);
            ht_PMRPT_FD.Add(PMR_OverallProgress, SE_HT_FIELDS_PMR_OverallProgress);
            ht_PMRPT_FD.Add(PMR_PRJPARTICULAR, SE_HT_FIELDS_PMR_PRJPARTICULAR);
            ht_PMRPT_FD.Add(PMR_ResDesc, SE_HT_FIELDS_PMR_ResDesc);
            ht_PMRPT_FD.Add(PMR_LDCmmt, SE_HT_FIELDS_PMR_LDCmmt);
            ht_PMRPT_FD.Add(PMR_CostClaimsCmmt, SE_HT_FIELDS_PMR_CostClaimsCmmt);
            ht_PMRPT_FD.Add(PMR_Accident, SE_HT_FIELDS_PMR_Accident);
            ht_PMRPT_FD.Add(PMR_EnvIssues, SE_HT_FIELDS_PMR_EnvIssues);
            ht_PMRPT_FD.Add(PMR_ScopeOfWorks, SE_HT_FIELDS_PMR_ScopeOfWorks);
            ht_PMRPT_FD.Add(PMR_EXECSUMMARY, SE_HT_FIELDS_PMR_EXECSUMMARY);
            ht_PMRPT_FD.Add(PMR_Penalty, SE_HT_FIELDS_PMR_Penalty);
            ht_PMRPT_FD.Add(PMR_AccidentCmmt, SE_HT_FIELDS_PMR_AccidentCmmt);

            //ht_PMRPT_FD.Add(PMR_SafetyIssues, SE_HT_FIELDS_PMR_SafetyIssues);
            //ht_PMRPT_FD.Add(PMR_ManpowerPlant, SE_HT_FIELDS_PMR_ManpowerPlant);
            
            
            
            
            //ht_PMRPT_FD.Add(PMR_KeyMilestoneDates, SE_HT_FIELDS_PMR_KeyMilestoneDates);
            
            
            
            
            try

            {
                SE_DT_PMR_ProgressSummary = PCCore.Database.Init.InitDataTable(PMR_ProgressSummary);
                SE_DT_PMR_Instruction = PCCore.Database.Init.InitDataTable(PMR_Instruction);
                SE_DT_PMR_CostClaimsStatus = PCCore.Database.Init.InitDataTable(PMR_CostClaimsStatus);
                SE_DT_PMR_DiffReason = PCCore.Database.Init.InitDataTable(PMR_DiffReason);
                SE_DT_PMR_EOTClaims = PCCore.Database.Init.InitDataTable(PMR_EOTClaims);
                SE_DT_PMR_Information = PCCore.Database.Init.InitDataTable(PMR_Information);
                SE_DT_PMR_VarStatus = PCCore.Database.Init.InitDataTable(PMR_VarStatus);
                SE_DT_PMR_EOTDesc = PCCore.Database.Init.InitDataTable(PMR_EOTDesc);
                SE_DT_PMR_Images = PCCore.Database.Init.InitDataTable(PMR_Images);
                SE_DT_PMR_Plant = PCCore.Database.Init.InitDataTable(PMR_Plant);
                SE_DT_PMR_QtyAssIssues = PCCore.Database.Init.InitDataTable(PMR_QtyAssIssues);
                SE_DT_PMR_LD = PCCore.Database.Init.InitDataTable(PMR_LD);
                SE_DT_PMR_Mstr = PCCore.Database.Init.InitDataTable(PMR_Mstr);
                SE_DT_PMR_ManPower = PCCore.Database.Init.InitDataTable(PMR_ManPower);
                SE_DT_PMR_OverallProgress = PCCore.Database.Init.InitDataTable(PMR_OverallProgress);
                SE_DT_PMR_PRJPARTICULAR = PCCore.Database.Init.InitDataTable(PMR_PRJPARTICULAR);
                SE_DT_PMR_ResDesc = PCCore.Database.Init.InitDataTable(PMR_ResDesc);
                SE_DT_PMR_LDCmmt = PCCore.Database.Init.InitDataTable(PMR_LDCmmt);
                SE_DT_PMR_CostClaimsCmmt = PCCore.Database.Init.InitDataTable(PMR_CostClaimsCmmt);
                SE_DT_PMR_Accident = PCCore.Database.Init.InitDataTable(PMR_Accident);
                SE_DT_PMR_EnvIssues = PCCore.Database.Init.InitDataTable(PMR_EnvIssues);
                SE_DT_PMR_ScopeOfWorks = PCCore.Database.Init.InitDataTable(PMR_ScopeOfWorks);
                SE_DT_PMR_EXECSUMMARY = PCCore.Database.Init.InitDataTable(PMR_EXECSUMMARY);
                SE_DT_PMR_Penalty = PCCore.Database.Init.InitDataTable(PMR_Penalty);
                SE_DT_PMR_AccidentCmmt = PCCore.Database.Init.InitDataTable(PMR_AccidentCmmt);
                
                
                //SE_DT_PMR_KeyMilestoneDates = PCCore.Database.Init.InitDataTable(PMR_KeyMilestoneDates);
                //SE_DT_PMR_ManpowerPlant = PCCore.Database.Init.InitDataTable(PMR_ManpowerPlant);
                //SE_DT_PMR_SafetyIssues = PCCore.Database.Init.InitDataTable(PMR_SafetyIssues);
                
                
                

                ht_PMRPT = new Hashtable();

                ht_PMRPT.Add(PMR_ProgressSummary,SE_DT_PMR_ProgressSummary);
                ht_PMRPT.Add(PMR_Instruction,SE_DT_PMR_Instruction);
                ht_PMRPT.Add(PMR_CostClaimsStatus, SE_DT_PMR_CostClaimsStatus);
                ht_PMRPT.Add(PMR_DiffReason, SE_DT_PMR_DiffReason);
                ht_PMRPT.Add(PMR_EOTClaims, SE_DT_PMR_EOTClaims);
                ht_PMRPT.Add(PMR_Information, SE_DT_PMR_Information);
                ht_PMRPT.Add(PMR_VarStatus, SE_DT_PMR_VarStatus);
                ht_PMRPT.Add(PMR_EOTDesc, SE_DT_PMR_EOTDesc);
                ht_PMRPT.Add(PMR_Images, SE_DT_PMR_Images);
                ht_PMRPT.Add(PMR_QtyAssIssues, SE_DT_PMR_QtyAssIssues);
                ht_PMRPT.Add(PMR_LD, SE_DT_PMR_LD);
                ht_PMRPT.Add(PMR_Mstr, SE_DT_PMR_Mstr);
                ht_PMRPT.Add(PMR_ManPower, SE_DT_PMR_ManPower);
                ht_PMRPT.Add(PMR_OverallProgress, SE_DT_PMR_OverallProgress);
                ht_PMRPT.Add(PMR_PRJPARTICULAR, SE_DT_PMR_PRJPARTICULAR);
                ht_PMRPT.Add(PMR_ResDesc, SE_DT_PMR_ResDesc);
                ht_PMRPT.Add(PMR_LDCmmt, SE_DT_PMR_LDCmmt);
                ht_PMRPT.Add(PMR_CostClaimsCmmt, SE_DT_PMR_CostClaimsCmmt);
                ht_PMRPT.Add(PMR_Accident, SE_DT_PMR_Accident);
                ht_PMRPT.Add(PMR_EnvIssues, SE_DT_PMR_EnvIssues);
                ht_PMRPT.Add(PMR_ScopeOfWorks, SE_DT_PMR_ScopeOfWorks);                
                ht_PMRPT.Add(PMR_EXECSUMMARY, SE_DT_PMR_EXECSUMMARY);
                ht_PMRPT.Add(PMR_Penalty, SE_DT_PMR_Penalty);
                ht_PMRPT.Add(PMR_AccidentCmmt, SE_DT_PMR_AccidentCmmt);

                //ht_PMRPT.Add(PMR_SafetyIssues, SE_DT_PMR_SafetyIssues);
                //ht_PMRPT.Add(PMR_ManpowerPlant, SE_DT_PMR_ManpowerPlant);
                //ht_PMRPT.Add(PMR_KeyMilestoneDates, SE_DT_PMR_KeyMilestoneDates);
                
                TableCount = ht_PMRPT.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    #region "Convert to HashTable"
    public Hashtable toHashTale()
    {
        Hashtable _htTable = new Hashtable();
        foreach (object o in ht_PMRPT.Keys)
        {
            Hashtable _ht = null;
            string fields;
            try
            {
                _ht = toHashTable(o.ToString(),ht_PMRPT_FD[o.ToString()]);
                
                _htTable.Add(o.ToString(), _ht);

            } catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Convert to HashTable: " + o.ToString(), ex);
                throw ex;
            }
        } 
        return _htTable;

        
    }

private Hashtable toHashTable(string p,object p_2)
{
 	throw new NotImplementedException();
}
    
    public static void EmptyRow(ref Hashtable _dtControl,string fields, int count)
    {
        if (_dtControl == null)
            _dtControl = new Hashtable();

        for (int i = 0; i < count; i++)
        {
            Hashtable _htColumn = new Hashtable();
            string[] arString = fields.Split(',');
            foreach (string _s in arString)
            {
                _htColumn.Add(_s, String.Empty);

            }
            _dtControl.Add(_dtControl.Count + 1, _htColumn);
        }
    }
    public static void EmptyRow(ref Hashtable _dtControl, string fields, string key)
    {
        if (_dtControl == null)
            _dtControl = new Hashtable();

            Hashtable _htColumn = new Hashtable();
            string[] arString = fields.Split(',');
            foreach (string _s in arString)
            {
                _htColumn.Add(_s, String.Empty);

            }
            _dtControl.Add(key, _htColumn);
       
    }

    
    public Hashtable toHashTable(string tblname, string fields)
    {
        Hashtable _ht = new Hashtable();
        DataTable _dt = (DataTable)ht_PMRPT[tblname];
        if (_dt.Rows.Count > 0)
        {
            int i = 0;
            foreach (DataRow _dr in _dt.Rows)
            {
                Hashtable _htColumn = new Hashtable();
                string[] arString = fields.Split(',');
                foreach (string _s in arString)
                {
                    _htColumn.Add(_s, _dr[_s]);
                    
                }
                i++;
                _ht.Add(i, _htColumn); 
            }
        }
        return _ht;
    }
    #endregion

    public static Hashtable SectionInfo(string code)
    {
        string sql = string.Format("SELECT * FROM {0} where Code = '{1}'", "CPSPMS", code);
        Hashtable _ht = new Hashtable();
        DataTable _dt = PCDb.Db.ExecQuery(sql);
        if (_dt.Rows.Count > 0)
        {
            _ht.Add(PMR_SecInfo_allowAdd, _dt.Rows[0][PMR_SecInfo_allowAdd]);
            _ht.Add(PMR_SecInfo_allowDelete, _dt.Rows[0][PMR_SecInfo_allowDelete]);
            _ht.Add(PMR_SecInfo_HiddenField, _dt.Rows[0][PMR_SecInfo_HiddenField]);
            _ht.Add(PMR_SecInfo_RowByColumn, _dt.Rows[0][PMR_SecInfo_RowByColumn]);
            _ht.Add(PMR_SecInfo_RowCount,_dt.Rows[0][PMR_SecInfo_RowCount]);
            _ht.Add(PMR_SecInfo_TableName,_dt.Rows[0][PMR_SecInfo_TableName]);

        }
        return _ht;
    }

    }
   



}


