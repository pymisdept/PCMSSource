using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using PCCore;

using PCCore.PCMS;
using SimpleControls.Web;
using System.Data.SqlClient;


public partial class MAI01_01 : BasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        PCCore.Common.HRLog.RecordLog("Page_Init");
        if (!IsPostBack)
        {
            
            //Change the label of the header tab <button>
            tab01_Visbtn.Text = "1. Executive" + '\r' + "Summary";
            tab02_Visbtn.Text = "2. Production" + '\r' + "Report";
            tab03_Visbtn.Text = "3. Financial" + '\r' + "Report";
            tab04_Visbtn.Text = "4. Manpower and" + '\r' + "Plant";
            tab05_Visbtn.Text = "5. Safety, health" + '\r' + "Environmental";
            tab06_Visbtn.Text = "6. Quaity" + '\r' + "Assurance";
            tab07_Visbtn.Text = "7. Site Photo" + '\r' + "Upload";
            //Start up the connection
            SqlConnection mConn = SqlConnect();
            //Open the connection
            mConn.Open();

            //Get Request.QueryString
            if (txtDocEntry.Text == "")
            {
                String DocEntry = Request.QueryString["DocEntry"];
                txtDocEntry.Text = DocEntry;
            }
            else
            {
                //Return 'Bad Parameter'
                Response.Write("<Script language='javascript'>alert('Bad Parameter');</script>");
                //Close this form
                Response.Write("<Script language='javascript'>window.close;</script>");
            }

            // check if new create 
            bool isNewCreate = false;
            try
            {
                isNewCreate = (Request.QueryString["isNew"] == "Y");
                
            }
            catch (Exception ex)
            {
            }
            if (isNewCreate)
            {
                btnPost.Enabled = false;
            }
            else
            {
                btnPost.Enabled = true;
            }
            //Check if this is a new docentry
            String mNew = "EXEC sp_MAI01_NEWData " + txtDocEntry.Text;
            SqlCommand mSQL;
            mSQL = new SqlCommand();
            mSQL.CommandText = mNew;
            mSQL.CommandType = CommandType.Text;
            mSQL.Connection = mConn;
            PCCore.Common.HRLog.RecordLog("ConnectionString: " + mConn.ConnectionString);
            SqlDataReader mDR = mSQL.ExecuteReader();
            PCCore.Common.HRLog.RecordLog(mDR.HasRows.ToString());
            if (mDR.HasRows == true)
            {
                while (mDR.Read())
                {
                    //Check if avaliable for download by Result type and File exists
                    PCCore.Common.HRLog.RecordLog("A1");
                    PCCore.Common.HRLog.RecordLog(mDR[0].ToString());
                    String Result = (String)mDR[0];
                    PCCore.Common.HRLog.RecordLog("A2");
                    PCCore.Common.HRLog.RecordLog(mDR[1].ToString());
                    String FileLocate = (String)mDR[1];
                    PCCore.Common.HRLog.RecordLog("A3");
                    if (Result == "POSTED" & FileLocate != "")
                    {
                        //Download program
                        PCCore.Common.HRLog.RecordLog("A4");
                        String FileOutput = ConfigurationManager.AppSettings["PMDirectory_Word"] + FileLocate;
                        //Response.Write("<Script language='javascript'>alert('" + FileOutput + "');</script>");
                        PCCore.Common.HRLog.RecordLog(FileOutput);
                        DownloadFile(FileOutput, true);
                        Response.End();
                    }
                }
                mDR.Close();
            }
            //Load title content above
            PCCore.Common.HRLog.RecordLog("A5");
            try
            {
                titleload(mConn);
                Page.Title = txtProject.Text + " (" + tab01_Visbtn.Text + ")";
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("", ex);
            }
            //Create Grid in the 6 folders
            try
            {
                PCCore.Common.HRLog.RecordLog("1");
                tab01_content(mConn);
                PCCore.Common.HRLog.RecordLog("2");
                tab02_content(mConn);
                PCCore.Common.HRLog.RecordLog("3");
                tab03_content(mConn);
                PCCore.Common.HRLog.RecordLog("4");
                tab04_content(mConn);
                PCCore.Common.HRLog.RecordLog("5");
                tab05_content(mConn);
                PCCore.Common.HRLog.RecordLog("6");
                tab06_content(mConn);
                PCCore.Common.HRLog.RecordLog("7");
                tab07_content(mConn);
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("", ex);
            }
            //Close Connection
            mConn.Close();
        }
    }
    
    #region "TAB 01"

    void tab01_content(SqlConnection mConn)
    {
        tab01_creation(mConn);
    }
    private void tab01_creation( SqlConnection mConn)
    {
        Tab01_01_DataBind(mConn);
    }

    protected void Tab01_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRowTab1 = "";
        mSQL_AddRowTab1 += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",1,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRowTab1, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab01_01_Save(mConn, -1);
        //Rebind the updated datasource
        Tab01_01_DataBind(mConn);
        mConn.Close();
    }

    public void Tab01_01_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image
        
        if (e.CommandName == "Tab01_01_03")
        {
            Tab01_01_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab01_01.Rows[mRow].FindControl("Tab01_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab01_01_DataBind(mConn);
        }
        else
        {
            Tab01_01_Save(mConn, mRow);
        }

        mConn.Close();
    }
       
    private void Tab01_01_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab01_01.Rows.Count - 1; i++)
            {
                Tab01_01_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab01_01_DBUpdate(mConn, pRow);
        }
        
    }

    private void Tab01_01_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1;
        System.Web.UI.WebControls.TextBox txt2;
        txt1 = (System.Web.UI.WebControls.TextBox)Tab01_01.Rows[pRow].FindControl("Tab01_DocEntry");
        txt2 = (System.Web.UI.WebControls.TextBox)Tab01_01.Rows[pRow].FindControl("Tab01_Detail");

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";

        PCCore.Common.HRLog.RecordLog(mSaveEntry);
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab01_01_DataBind(SqlConnection mConn)
    {
        try
        {
            //Use SQLCommand to insert value from SQL to GridView Tab01_01
            String mSQL_Tab01_01 = "SELECT DocEntry, Text1 AS Detail FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ",1,'T')";
            SqlCommand mSQL;
            mSQL = new SqlCommand();
            mSQL.CommandText = mSQL_Tab01_01;
            mSQL.CommandType = CommandType.Text;
            mSQL.Connection = mConn;
            PCCore.Common.HRLog.RecordLog("Msql_Tab_01: Query: " + mSQL_Tab01_01);
            SqlDataReader mSQLDr_01_01 = mSQL.ExecuteReader();

            Tab01_01.DataSource = mSQLDr_01_01;
            Tab01_01.DataBind();
            mSQLDr_01_01.Close();
        }
        catch (Exception ex)
        {
            PCCore.Common.HRLog.RecordException("1", ex);
        }
    }

    
    #endregion

    
    void tab02_content(SqlConnection mConn)
    {
        try
        {
            tab02_01_creation(mConn);                           // Done in Update
            tab02_02_creation(mConn);                           // Done in Update                  
            tab02_03_creation(mConn);                           // Done in Add / Update / Delete
            tab02_04_creation(mConn);                           // Done in Add / Update / Delete               
            tab02_05_creation(mConn);                           // Done in Add / Update / Delete
            tab02_06_creation(mConn);                           // Done in Add / Update / Delete
        }
        catch (Exception ex)
        {
            PCCore.Common.HRLog.RecordException("2", ex);
        }
    }
    #region "Tab 02 - 01"

    private void tab02_01_creation(SqlConnection mConn)
    {
        Tab02_01_DataBind(mConn);
    }

    private void Tab02_01_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab02_01.Rows.Count - 1; i++)
            {
                Tab02_01_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab02_01_DBUpdate(mConn, pRow);
        }

    }

    private void Tab02_01_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1;
        System.Web.UI.WebControls.TextBox txt2;
        System.Web.UI.WebControls.TextBox txt3;
        System.Web.UI.WebControls.TextBox txt4;
        txt1 = (System.Web.UI.WebControls.TextBox)Tab02_01.Rows[pRow].FindControl("Tab02_01_DocEntry");
        txt2 = (System.Web.UI.WebControls.TextBox)Tab02_01.Rows[pRow].FindControl("Tab02_01_Label");
        txt3 = (System.Web.UI.WebControls.TextBox)Tab02_01.Rows[pRow].FindControl("Tab02_01_Detail");
        txt4 = (System.Web.UI.WebControls.TextBox)Tab02_01.Rows[pRow].FindControl("Tab02_01_UOM");

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', " 
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    protected void Tab02_01_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRowTab1 = "";
        mSQL_AddRowTab1 += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",2.11,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRowTab1, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab02_01_Save(mConn, -1);
        //Rebind the updated datasource
        Tab02_01_DataBind(mConn);
        mConn.Close();
    }

    public void Tab02_01_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab02_01_03")
        {
            Tab02_01_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab02_01.Rows[mRow].FindControl("Tab02_01_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab02_01_DataBind(mConn);
        }
        else
        {
            Tab02_01_Save(mConn, mRow);
        }

        mConn.Close();
    }
    
    private void Tab02_01_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab02_01 = "SELECT DocEntry, Text1 AS TableLabel, Text2 AS TableContent, Text3 AS UOM FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 2.11,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab02_01;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_02_01 = mSQL.ExecuteReader();

        Tab02_01.DataSource = mSQLDr_02_01;
        Tab02_01.DataBind();

        mSQLDr_02_01.Close();
    }

    #endregion
    #region "Tab 02 - 02"
    private void tab02_02_creation(SqlConnection mConn)
    {
        Tab02_02_DataBind(mConn);
    }
    private void Tab02_02_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab02_02 = "SELECT Cast(DocEntry AS NVARCHAR(50)) AS DocEntry, Text1 AS ScopeOfWork FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 2.2,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab02_02;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_02_02 = mSQL.ExecuteReader();

        while (mSQLDr_02_02.Read())
        {
            Tab02_02_DocEntry.Text = (String)mSQLDr_02_02[0];
            Tab02_02_ScopeOfWork.Text = (String)mSQLDr_02_02[1];
        }
        

        mSQLDr_02_02.Close();
    }

    private void Tab02_02_DBUpdate(SqlConnection mConn, int pRow)
    {
        String mDocEntry, mScopeOfWork;
        mDocEntry = Tab02_02_DocEntry.Text;
        mScopeOfWork = Tab02_02_ScopeOfWork.Text;

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + mDocEntry + ",0,0,"
            + "'" + mScopeOfWork.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }
    #endregion
    #region "Tab 02 - 03"

    private void tab02_03_creation(SqlConnection mConn)
    {
        Tab02_03_DataBind(mConn);
    }
   
    protected void Tab02_03_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",2.3,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab02_03_Save(mConn, -1);
        //Rebind the updated datasource
        Tab02_03_DataBind(mConn);
        mConn.Close();
    }

    public void Tab02_03_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab02_03_03")
        {
            Tab02_03_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab02_03.Rows[mRow].FindControl("Tab02_03_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab02_03_DataBind(mConn);
        }
        else
        {
            Tab02_03_Save(mConn, mRow);
        }

        mConn.Close();
    }

    private void Tab02_03_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab02_03.Rows.Count - 1; i++)
            {
                Tab02_03_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab02_03_DBUpdate(mConn, pRow);
        }

    }

    private void Tab02_03_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1;
        System.Web.UI.WebControls.TextBox txt2;
        txt1 = (System.Web.UI.WebControls.TextBox)Tab02_03.Rows[pRow].FindControl("Tab02_03_DocEntry");
        txt2 = (System.Web.UI.WebControls.TextBox)Tab02_03.Rows[pRow].FindControl("Tab02_03_Detail");

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab02_03_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab02_03 = "SELECT DocEntry, Text1 AS Detail FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 2.3,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab02_03;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_02_03 = mSQL.ExecuteReader();

        Tab02_03.DataSource = mSQLDr_02_03;
        Tab02_03.DataBind();

        mSQLDr_02_03.Close();
    }
    #endregion
    #region "Tab 02 - 04"

    private void tab02_04_creation(SqlConnection mConn)
    {
        Tab02_04_DataBind(mConn);
    }
    
    protected void Tab02_04_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",2.41,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab02_04_Save(mConn, -1);
        //Rebind the updated datasource
        Tab02_04_DataBind(mConn);
        mConn.Close();
    }
    //
    public void Tab02_04_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab02_04_03")
        {
            Tab02_04_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab02_04.Rows[mRow].FindControl("Tab02_04_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab02_04_DataBind(mConn);
        }
        else
        {
            Tab02_04_Save(mConn, mRow);
        }

        mConn.Close();
    }

    private void Tab02_04_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab02_04.Rows.Count - 1; i++)
            {
                Tab02_04_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab02_04_DBUpdate(mConn, pRow);
        }

    }

    private void Tab02_04_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1;
        System.Web.UI.WebControls.TextBox txt2;
        System.Web.UI.WebControls.TextBox txt3;
        System.Web.UI.WebControls.TextBox txt4;
        System.Web.UI.WebControls.TextBox txt5;
        System.Web.UI.WebControls.TextBox txt6;
        System.Web.UI.WebControls.TextBox txt7;
        System.Web.UI.WebControls.TextBox txt8;
        System.Web.UI.WebControls.TextBox txt9;
        txt1 = (System.Web.UI.WebControls.TextBox)Tab02_04.Rows[pRow].FindControl("Tab02_04_DocEntry");
        txt2 = (System.Web.UI.WebControls.TextBox)Tab02_04.Rows[pRow].FindControl("Tab02_04_LineNum");
        txt3 = (System.Web.UI.WebControls.TextBox)Tab02_04.Rows[pRow].FindControl("Tab02_04_Type");
        txt4 = (System.Web.UI.WebControls.TextBox)Tab02_04.Rows[pRow].FindControl("Tab02_04_CommenceDate");
        txt5 = (System.Web.UI.WebControls.TextBox)Tab02_04.Rows[pRow].FindControl("Tab02_04_CompletionDate");
        txt6 = (System.Web.UI.WebControls.TextBox)Tab02_04.Rows[pRow].FindControl("Tab02_04_ContractPeriod");
        txt7 = (System.Web.UI.WebControls.TextBox)Tab02_04.Rows[pRow].FindControl("Tab02_04_CompletionPercent");
        txt8 = (System.Web.UI.WebControls.TextBox)Tab02_04.Rows[pRow].FindControl("Tab02_04_ACompletionDate");
        txt9 = (System.Web.UI.WebControls.TextBox)Tab02_04.Rows[pRow].FindControl("Tab02_04_Days");

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'" + txt5.Text.Replace("'", "''") + "',"
            + "'" + txt6.Text.Replace("'", "''") + "',"
            + "'" + txt7.Text.Replace("'", "''") + "',"
            + "'" + txt8.Text.Replace("'", "''") + "',"
            + "'" + txt9.Text.Replace("'", "''") + "',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        
        mSQL.ExecuteNonQuery();
    }
    
    private void Tab02_04_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab02_04;
        mSQL_Tab02_04 = "";
        mSQL_Tab02_04 += "SELECT DocEntry, Text1 AS LineNum, Text2 AS Type, ";
        mSQL_Tab02_04 += " Text3 AS CommenceDate, Text4 AS CompletionDate, Text5 AS ContractPeriod,  ";
        mSQL_Tab02_04 += " Text6 AS CompletionPercent, Text7 AS ACompletionDate, Text8 AS Days ";
        mSQL_Tab02_04 += " FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 2.41,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab02_04;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_02_04 = mSQL.ExecuteReader();

        Tab02_04.DataSource = mSQLDr_02_04;
        Tab02_04.DataBind();

        mSQLDr_02_04.Close();
    }

    
    #endregion
    #region "Tab 02 - 05"

    private void tab02_05_creation(SqlConnection mConn)
    {
        Tab02_05_DataBind(mConn);
    }

    public void Tab02_05_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab02_05_03")
        {
            Tab02_05_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab02_05.Rows[mRow].FindControl("Tab02_05_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab02_05_DataBind(mConn);
        }
        else
        {
            Tab02_05_Save(mConn, mRow);
        }

        mConn.Close();
    }

    protected void Tab02_05_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",2.51,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab02_05_Save(mConn, -1);
        //Rebind the updated datasource
        Tab02_05_DataBind(mConn);
        mConn.Close();
    }

    private void Tab02_05_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab02_05.Rows.Count - 1; i++)
            {
                Tab02_05_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab02_05_DBUpdate(mConn, pRow);
        }

    }

    private void Tab02_05_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1;
        System.Web.UI.WebControls.TextBox txt2;
        System.Web.UI.WebControls.TextBox txt3;
        System.Web.UI.WebControls.TextBox txt4;
        System.Web.UI.WebControls.TextBox txt5;
        txt1 = (System.Web.UI.WebControls.TextBox)Tab02_05.Rows[pRow].FindControl("Tab02_05_DocEntry");
        txt2 = (System.Web.UI.WebControls.TextBox)Tab02_05.Rows[pRow].FindControl("Tab02_05_LineNum");
        txt3 = (System.Web.UI.WebControls.TextBox)Tab02_05.Rows[pRow].FindControl("Tab02_05_Type");
        txt4 = (System.Web.UI.WebControls.TextBox)Tab02_05.Rows[pRow].FindControl("Tab02_05_InRec");
        txt5 = (System.Web.UI.WebControls.TextBox)Tab02_05.Rows[pRow].FindControl("Tab02_05_InRep");

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'" + txt5.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab02_05_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab02_05;
        mSQL_Tab02_05 = "";
        mSQL_Tab02_05 += "SELECT DocEntry, Text1 AS LineNum, Text2 AS Type, ";
        mSQL_Tab02_05 += " Text3 AS InRec, Text4 AS InRep  ";
        mSQL_Tab02_05 += " FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 2.51,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab02_05;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_02_05 = mSQL.ExecuteReader();

        Tab02_05.DataSource = mSQLDr_02_05;
        Tab02_05.DataBind();

        mSQLDr_02_05.Close();
    }
    #endregion
    #region "Tab 02 - 06"
    private void tab02_06_creation(SqlConnection mConn)
    {
        Tab02_06_DataBind(mConn);
    }

    public void Tab02_06_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab02_06_03")
        {
            Tab02_06_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab02_06.Rows[mRow].FindControl("Tab02_06_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab02_06_DataBind(mConn);
        }
        else
        {
            Tab02_06_Save(mConn, mRow);
        }

        mConn.Close();
    }

    protected void Tab02_06_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",2.61,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab02_06_Save(mConn, -1);
        //Rebind the updated datasource
        Tab02_06_DataBind(mConn);
        mConn.Close();
    }

    private void Tab02_06_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab02_06.Rows.Count - 1; i++)
            {
                Tab02_06_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab02_06_DBUpdate(mConn, pRow);
        }

    }

    private void Tab02_06_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1;
        System.Web.UI.WebControls.TextBox txt2;
        System.Web.UI.WebControls.TextBox txt3;
        System.Web.UI.WebControls.TextBox txt4;
        System.Web.UI.WebControls.TextBox txt5;
        txt1 = (System.Web.UI.WebControls.TextBox)Tab02_06.Rows[pRow].FindControl("Tab02_06_DocEntry");
        txt2 = (System.Web.UI.WebControls.TextBox)Tab02_06.Rows[pRow].FindControl("Tab02_06_LineNum");
        txt3 = (System.Web.UI.WebControls.TextBox)Tab02_06.Rows[pRow].FindControl("Tab02_06_Type");
        txt4 = (System.Web.UI.WebControls.TextBox)Tab02_06.Rows[pRow].FindControl("Tab02_06_InRec");
        txt5 = (System.Web.UI.WebControls.TextBox)Tab02_06.Rows[pRow].FindControl("Tab02_06_InRep");

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'" + txt5.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab02_06_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab02_06;
        mSQL_Tab02_06 = "";
        mSQL_Tab02_06 += "SELECT DocEntry, Text1 AS LineNum, Text2 AS Type, ";
        mSQL_Tab02_06 += " Text3 AS InRec, Text4 AS InRep  ";
        mSQL_Tab02_06 += " FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 2.61,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab02_06;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_02_06 = mSQL.ExecuteReader();

        Tab02_06.DataSource = mSQLDr_02_06;
        Tab02_06.DataBind();

        mSQLDr_02_06.Close();
    }
    #endregion

    void tab03_content(SqlConnection mConn)
    {
        try
        {
            tab03_01_creation(mConn);                           // Done in Update
            tab03_02_creation(mConn);                           // Done in Update
            tab03_A2_creation(mConn);
            tab03_03_creation(mConn);
            tab03_04_creation(mConn);
            tab03_05_creation(mConn);
            tab03_06_creation(mConn);
        }
        catch (Exception ex)
        {
            PCCore.Common.HRLog.RecordException("3", ex);
        }
    }
    #region "Tab 03 - 01"
    private void tab03_01_creation(SqlConnection mConn)
    {
        Tab03_01_DataBind(mConn);
    }

    private void Tab03_01_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab03_01.Rows.Count - 1; i++)
            {
                Tab03_01_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab03_01_DBUpdate(mConn, pRow);
        }

    }

    private void Tab03_01_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1;
        System.Web.UI.WebControls.TextBox txt2;
        System.Web.UI.WebControls.TextBox txt3;
        System.Web.UI.WebControls.TextBox txt4;
        System.Web.UI.WebControls.TextBox txt5;
        System.Web.UI.WebControls.TextBox txt6;
        txt1 = (System.Web.UI.WebControls.TextBox)Tab03_01.Rows[pRow].FindControl("Tab03_01_DocEntry");
        txt2 = (System.Web.UI.WebControls.TextBox)Tab03_01.Rows[pRow].FindControl("Tab03_01_LineNum");
        txt3 = (System.Web.UI.WebControls.TextBox)Tab03_01.Rows[pRow].FindControl("Tab03_01_Description");
        txt4 = (System.Web.UI.WebControls.TextBox)Tab03_01.Rows[pRow].FindControl("Tab03_01_LastMonth");
        txt5 = (System.Web.UI.WebControls.TextBox)Tab03_01.Rows[pRow].FindControl("Tab03_01_ThisMonth");
        txt6 = (System.Web.UI.WebControls.TextBox)Tab03_01.Rows[pRow].FindControl("Tab03_01_NextMonth");


        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'" + txt5.Text.Replace("'", "''") + "',"
            + "'" + txt6.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab03_01_DataBind(SqlConnection mConn)
    {
        String mSQL_Tab03_01_Hdr;
        mSQL_Tab03_01_Hdr = "SELECT isNull(Alias3,'') as Alias3, isNull(Alias4,'') as Alias4, isNull(Alias5,'') as Alias5 FROM CPSPMRH WHERE BaseEntry = " + txtDocEntry.Text + " AND SubEntry = '3.11' ";
        PCCore.Common.HRLog.RecordLog("Query on Tab03_01_DataBind: " + mSQL_Tab03_01_Hdr);
        SqlCommand mSQL_Hdr;
        mSQL_Hdr = new SqlCommand();
        mSQL_Hdr.CommandText = mSQL_Tab03_01_Hdr;
        mSQL_Hdr.CommandType = CommandType.Text;
        mSQL_Hdr.Connection = mConn;
        SqlDataReader mSQLDr_03_01_Hdrs = mSQL_Hdr.ExecuteReader();
        
        while (mSQLDr_03_01_Hdrs.Read())
        {
            Tab03_01.Columns[3].HeaderText = (String)mSQLDr_03_01_Hdrs[0];
            Tab03_01.Columns[4].HeaderText = (String)mSQLDr_03_01_Hdrs[1];
            Tab03_01.Columns[5].HeaderText = (String)mSQLDr_03_01_Hdrs[2];
        }

        mSQLDr_03_01_Hdrs.Close();

        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab03_01;
        mSQL_Tab03_01 = "";
        mSQL_Tab03_01 += "SELECT DocEntry, Text1 AS LineNum, Text2 AS Description, ";
        mSQL_Tab03_01 += " Text3 AS LastMonth, Text4 AS ThisMonth, Text5 AS NextMonth  ";
        mSQL_Tab03_01 += " FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 3.11,'T')";
        SqlCommand mSQL;

        PCCore.Common.HRLog.RecordLog("mSQL_Tab03_01 Databind Query: " + mSQL_Tab03_01);

        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab03_01;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_03_01 = mSQL.ExecuteReader();

        Tab03_01.DataSource = mSQLDr_03_01;
        Tab03_01.DataBind();

        mSQLDr_03_01.Close();
    }

    #endregion
    #region "Tab 03 - 02"
    private void tab03_02_creation(SqlConnection mConn)
    {
        Tab03_02_DataBind(mConn);
    }

    private void Tab03_02_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab03_02.Rows.Count - 1; i++)
            {
                Tab03_02_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab03_02_DBUpdate(mConn, pRow);
        }

    }

    private void Tab03_02_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1;
        System.Web.UI.WebControls.TextBox txt2;
        System.Web.UI.WebControls.TextBox txt3;
        System.Web.UI.WebControls.TextBox txt4;
        System.Web.UI.WebControls.TextBox txt5;
        System.Web.UI.WebControls.TextBox txt6;
        txt1 = (System.Web.UI.WebControls.TextBox)Tab03_02.Rows[pRow].FindControl("Tab03_02_DocEntry");
        txt2 = (System.Web.UI.WebControls.TextBox)Tab03_02.Rows[pRow].FindControl("Tab03_02_LineNum");
        txt3 = (System.Web.UI.WebControls.TextBox)Tab03_02.Rows[pRow].FindControl("Tab03_02_Description");
        txt4 = (System.Web.UI.WebControls.TextBox)Tab03_02.Rows[pRow].FindControl("Tab03_02_LastMonth");
        txt5 = (System.Web.UI.WebControls.TextBox)Tab03_02.Rows[pRow].FindControl("Tab03_02_ThisMonth");
        txt6 = (System.Web.UI.WebControls.TextBox)Tab03_02.Rows[pRow].FindControl("Tab03_02_NextMonth");


        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'" + txt5.Text.Replace("'", "''") + "',"
            + "'" + txt6.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab03_02_DataBind(SqlConnection mConn)
    {
        String mSQL_Tab03_02_Hdr;
        mSQL_Tab03_02_Hdr = "SELECT ISNULL(Alias3,''), ISNULL(Alias4,''), ISNULL(Alias5,'') FROM CPSPMRH WHERE BaseEntry = " + txtDocEntry.Text + " AND SubEntry = '3.21' ";
        SqlCommand mSQL_Hdr;
        mSQL_Hdr = new SqlCommand();
        mSQL_Hdr.CommandText = mSQL_Tab03_02_Hdr;
        mSQL_Hdr.CommandType = CommandType.Text;
        mSQL_Hdr.Connection = mConn;
        SqlDataReader mSQLDr_03_02_Hdrs = mSQL_Hdr.ExecuteReader();
        while (mSQLDr_03_02_Hdrs.Read())
        {
            Tab03_02.Columns[3].HeaderText = (String)mSQLDr_03_02_Hdrs[0];
            Tab03_02.Columns[4].HeaderText = (String)mSQLDr_03_02_Hdrs[1];
            Tab03_02.Columns[5].HeaderText = (String)mSQLDr_03_02_Hdrs[2];
        }
        mSQLDr_03_02_Hdrs.Close();

        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab03_02;
        mSQL_Tab03_02 = "";
        mSQL_Tab03_02 += "SELECT DocEntry, Text1 AS LineNum, Text2 AS Description, ";
        mSQL_Tab03_02 += " Text3 AS LastMonth, Text4 AS ThisMonth, Text5 AS NextMonth  ";
        mSQL_Tab03_02 += " FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 3.21,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab03_02;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_03_02 = mSQL.ExecuteReader();

        Tab03_02.DataSource = mSQLDr_03_02;
        Tab03_02.DataBind();

        mSQLDr_03_02.Close();
    }
    #endregion
    #region "Tab 03 - A2"

    private void tab03_A2_creation(SqlConnection mConn)
    {
        Tab03_A2_DataBind(mConn);
    }

    protected void Tab03_A2_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",3.22,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab03_A2_Save(mConn, -1);
        //Rebind the updated datasource
        Tab03_A2_DataBind(mConn);
        mConn.Close();
    }

    public void Tab03_A2_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab03_A2_03")
        {
            Tab03_A2_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab03_A2.Rows[mRow].FindControl("Tab03_A2_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab03_A2_DataBind(mConn);
        }
        else
        {
            Tab03_A2_Save(mConn, mRow);
        }

        mConn.Close();
    }

    private void Tab03_A2_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab03_A2.Rows.Count - 1; i++)
            {
                Tab03_A2_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab03_A2_DBUpdate(mConn, pRow);
        }

    }

    private void Tab03_A2_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1;
        System.Web.UI.WebControls.TextBox txt2;
        txt1 = (System.Web.UI.WebControls.TextBox)Tab03_A2.Rows[pRow].FindControl("Tab03_A2_DocEntry");
        txt2 = (System.Web.UI.WebControls.TextBox)Tab03_A2.Rows[pRow].FindControl("Tab03_A2_Detail");

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab03_A2_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab03_A2 = "SELECT DocEntry, Text1 AS Detail FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 3.22,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab03_A2;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_03_A2 = mSQL.ExecuteReader();

        Tab03_A2.DataSource = mSQLDr_03_A2;
        Tab03_A2.DataBind();

        mSQLDr_03_A2.Close();
    }
    #endregion

    #region "Tab 03 - 03"
    private void tab03_03_creation(SqlConnection mConn)
    {
        Tab03_03_DataBind(mConn);
    }

    private void Tab03_03_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab03_03.Rows.Count - 1; i++)
            {
                Tab03_03_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab03_03_DBUpdate(mConn, pRow);
        }

    }

    private void Tab03_03_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1;
        System.Web.UI.WebControls.TextBox txt2;
        System.Web.UI.WebControls.TextBox txt3;
        System.Web.UI.WebControls.TextBox txt4;
        System.Web.UI.WebControls.TextBox txt5;
        txt1 = (System.Web.UI.WebControls.TextBox)Tab03_03.Rows[pRow].FindControl("Tab03_03_DocEntry");
        txt2 = (System.Web.UI.WebControls.TextBox)Tab03_03.Rows[pRow].FindControl("Tab03_03_LineNum");
        txt3 = (System.Web.UI.WebControls.TextBox)Tab03_03.Rows[pRow].FindControl("Tab03_03_Description");
        txt4 = (System.Web.UI.WebControls.TextBox)Tab03_03.Rows[pRow].FindControl("Tab03_03_No");
        txt5 = (System.Web.UI.WebControls.TextBox)Tab03_03.Rows[pRow].FindControl("Tab03_03_Amount");

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'" + txt5.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab03_03_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab03_03;
        mSQL_Tab03_03 = "";
        mSQL_Tab03_03 += "SELECT DocEntry, Text1 AS LineNum, Text2 AS Description, ";
        mSQL_Tab03_03 += " Text3 AS No, Text4 AS Amount  ";
        mSQL_Tab03_03 += " FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 3.31,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab03_03;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_03_03 = mSQL.ExecuteReader();

        Tab03_03.DataSource = mSQLDr_03_03;
        Tab03_03.DataBind();

        mSQLDr_03_03.Close();
    }
    #endregion
    #region "Tab 03 - 04"

    private void tab03_04_creation(SqlConnection mConn)
    {
        Tab03_04_DataBind(mConn);
        Tab03_04_02_DataBind(mConn);
    }

    public void Tab03_04_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab03_04_03")
        {
            Tab03_04_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab03_04.Rows[mRow].FindControl("Tab03_04_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab03_04_DataBind(mConn);
        }
        else
        {
            Tab03_04_Save(mConn, mRow);
        }

        mConn.Close();
    }

    protected void Tab03_04_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",3.41,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab03_04_Save(mConn, -1);
        //Rebind the updated datasource
        Tab03_04_DataBind(mConn);
        mConn.Close();
    }

    private void Tab03_04_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab03_04.Rows.Count - 1; i++)
            {
                Tab03_04_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab03_04_DBUpdate(mConn, pRow);
        }

    }
    private void Tab03_04_02_Save(SqlConnection mConn, int pRow)
    {
        
        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + Tab03_04_02_DocEntry.Text + ",0,0,"
            + "'" + Tab03_04_02_Remark.Text + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        PCCore.Common.HRLog.RecordLog("Tab03_04_02 Save Query", mSaveEntry);
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();

    }
    private void Tab03_05_02_Save(SqlConnection mConn, int pRow)
    {

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + Tab03_05_02_DocEntry.Text + ",0,0,"
            + "'" + Tab03_05_02_Remark.Text + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();

    }
    private void Tab03_06_02_Save(SqlConnection mConn, int pRow)
    {

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + Tab03_06_02_DocEntry.Text + ",0,0,"
            + "'" + Tab03_06_02_Remark.Text + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();

    }

    private void Tab03_04_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1 = (System.Web.UI.WebControls.TextBox)Tab03_04.Rows[pRow].FindControl("Tab03_04_DocEntry");
        System.Web.UI.WebControls.TextBox txt2 = (System.Web.UI.WebControls.TextBox)Tab03_04.Rows[pRow].FindControl("Tab03_04_LineNum");
        System.Web.UI.WebControls.TextBox txt3 = (System.Web.UI.WebControls.TextBox)Tab03_04.Rows[pRow].FindControl("Tab03_04_Section");
        System.Web.UI.WebControls.TextBox txt4 = (System.Web.UI.WebControls.TextBox)Tab03_04.Rows[pRow].FindControl("Tab03_04_ContractPeriod");
        System.Web.UI.WebControls.TextBox txt5 = (System.Web.UI.WebControls.TextBox)Tab03_04.Rows[pRow].FindControl("Tab03_04_ACompletionPeriod");
        System.Web.UI.WebControls.TextBox txt6 = (System.Web.UI.WebControls.TextBox)Tab03_04.Rows[pRow].FindControl("Tab03_04_EOTRequired");
        System.Web.UI.WebControls.TextBox txt7 = (System.Web.UI.WebControls.TextBox)Tab03_04.Rows[pRow].FindControl("Tab03_04_EOTSubmitted");
        System.Web.UI.WebControls.TextBox txt8 = (System.Web.UI.WebControls.TextBox)Tab03_04.Rows[pRow].FindControl("Tab03_04_EOTGranted");
        System.Web.UI.WebControls.TextBox txt9 = (System.Web.UI.WebControls.TextBox)Tab03_04.Rows[pRow].FindControl("Tab03_04_AEOTGranted");
        System.Web.UI.WebControls.TextBox txt10 = (System.Web.UI.WebControls.TextBox)Tab03_04.Rows[pRow].FindControl("Tab03_04_LDExposure");

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'" + txt5.Text.Replace("'", "''") + "',"
            + "'" + txt6.Text.Replace("'", "''") + "',"
            + "'" + txt7.Text.Replace("'", "''") + "',"
            + "'" + txt8.Text.Replace("'", "''") + "',"
            + "'" + txt9.Text.Replace("'", "''") + "',"
            + "'" + txt10.Text.Replace("'", "''") + "',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab03_04_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab03_04;
        mSQL_Tab03_04 = "";
        mSQL_Tab03_04 += "SELECT DocEntry, Text1 AS LineNum, Text2 AS Section, ";
        mSQL_Tab03_04 += " Text3 AS ContractPeriod, Text4 AS ACompletionPeriod,  ";
        mSQL_Tab03_04 += " Text5 AS EOTRequired, Text6 AS EOTSubmitted,  ";
        mSQL_Tab03_04 += " Text7 AS EOTGranted, Text8 AS AEOTGranted,  ";
        mSQL_Tab03_04 += " Text9 AS LDExposure ";
        mSQL_Tab03_04 += " FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 3.41,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab03_04;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_03_04 = mSQL.ExecuteReader();

        Tab03_04.DataSource = mSQLDr_03_04;
        Tab03_04.DataBind();

        mSQLDr_03_04.Close();
    }

    private void Tab03_04_02_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab03_04_02 = "SELECT Cast(DocEntry AS NVARCHAR(50)) AS DocEntry, Text1 AS Remark FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 3.42,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab03_04_02;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_03_04_02 = mSQL.ExecuteReader();

        while (mSQLDr_03_04_02.Read())
        {
            Tab03_04_02_DocEntry.Text = (String)mSQLDr_03_04_02[0];
            Tab03_04_02_Remark.Text = (String)mSQLDr_03_04_02[1];
        }


        mSQLDr_03_04_02.Close();
    }

    private void Tab03_04_02_DBUpdate(SqlConnection mConn, int pRow)
    {
        String mDocEntry, mScopeOfWork;
        mDocEntry = Tab03_04_02_DocEntry.Text;
        mScopeOfWork = Tab03_04_02_Remark.Text;

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + mDocEntry + ",0,0,"
            + "'" + mScopeOfWork.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    #endregion
    #region "Tab 03 - 05"

    private void tab03_05_creation(SqlConnection mConn)
    {
        Tab03_05_DataBind(mConn);
        Tab03_05_02_DataBind(mConn);
    }

    public void Tab03_05_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab03_05_03")
        {
            Tab03_05_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab03_05.Rows[mRow].FindControl("Tab03_05_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab03_05_DataBind(mConn);
        }
        else
        {
            Tab03_05_Save(mConn, mRow);
        }

        mConn.Close();
    }

    protected void Tab03_05_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",3.51,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab03_05_Save(mConn, -1);
        //Rebind the updated datasource
        Tab03_05_DataBind(mConn);
        mConn.Close();
    }

    private void Tab03_05_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab03_05.Rows.Count - 1; i++)
            {
                Tab03_05_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab03_05_DBUpdate(mConn, pRow);
        }

    }

    private void Tab03_05_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1 = (System.Web.UI.WebControls.TextBox)Tab03_05.Rows[pRow].FindControl("Tab03_05_DocEntry");
        System.Web.UI.WebControls.TextBox txt2 = (System.Web.UI.WebControls.TextBox)Tab03_05.Rows[pRow].FindControl("Tab03_05_LineNum");
        System.Web.UI.WebControls.TextBox txt3 = (System.Web.UI.WebControls.TextBox)Tab03_05.Rows[pRow].FindControl("Tab03_05_Section");
        System.Web.UI.WebControls.TextBox txt4 = (System.Web.UI.WebControls.TextBox)Tab03_05.Rows[pRow].FindControl("Tab03_05_LDDays");
        System.Web.UI.WebControls.TextBox txt5 = (System.Web.UI.WebControls.TextBox)Tab03_05.Rows[pRow].FindControl("Tab03_05_LDExposure");
        System.Web.UI.WebControls.TextBox txt6 = (System.Web.UI.WebControls.TextBox)Tab03_05.Rows[pRow].FindControl("Tab03_05_LDPotential");
        System.Web.UI.WebControls.TextBox txt7 = (System.Web.UI.WebControls.TextBox)Tab03_05.Rows[pRow].FindControl("Tab03_05_LDDeducted");
        System.Web.UI.WebControls.TextBox txt8 = (System.Web.UI.WebControls.TextBox)Tab03_05.Rows[pRow].FindControl("Tab03_05_ARecoverable");
        System.Web.UI.WebControls.TextBox txt9 = (System.Web.UI.WebControls.TextBox)Tab03_05.Rows[pRow].FindControl("Tab03_05_Potential");

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'" + txt5.Text.Replace("'", "''") + "',"
            + "'" + txt6.Text.Replace("'", "''") + "',"
            + "'" + txt7.Text.Replace("'", "''") + "',"
            + "'" + txt8.Text.Replace("'", "''") + "',"
            + "'" + txt9.Text.Replace("'", "''") + "',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab03_05_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab03_05;
        mSQL_Tab03_05 = "";
        mSQL_Tab03_05 += "SELECT DocEntry, Text1 AS LineNum, Text2 AS Section, ";
        mSQL_Tab03_05 += " Text3 AS LDDays, Text4 AS LDExposure,  ";
        mSQL_Tab03_05 += " Text5 AS LDPotential, Text6 AS LDDeucted,  ";
        mSQL_Tab03_05 += " Text7 AS ARecoverable, Text8 AS Potential  ";
        mSQL_Tab03_05 += " FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 3.51,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab03_05;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_03_05 = mSQL.ExecuteReader();

        Tab03_05.DataSource = mSQLDr_03_05;
        Tab03_05.DataBind();

        mSQLDr_03_05.Close();
    }

    private void Tab03_05_02_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab03_05_02 = "SELECT Cast(DocEntry AS NVARCHAR(50)) AS DocEntry, Text1 AS Remark FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 3.52,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab03_05_02;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_03_05_02 = mSQL.ExecuteReader();

        while (mSQLDr_03_05_02.Read())
        {
            Tab03_05_02_DocEntry.Text = (String)mSQLDr_03_05_02[0];
            Tab03_05_02_Remark.Text = (String)mSQLDr_03_05_02[1];
        }


        mSQLDr_03_05_02.Close();
    }

    private void Tab03_05_02_DBUpdate(SqlConnection mConn, int pRow)
    {
        String mDocEntry, mRemark;
        mDocEntry = Tab03_05_02_DocEntry.Text;
        mRemark = Tab03_05_02_Remark.Text;

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + mDocEntry + ",0,0,"
            + "'" + mRemark.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    #endregion
    #region "Tab 03 - 06"

    private void tab03_06_creation(SqlConnection mConn)
    {
        Tab03_06_DataBind(mConn);
        Tab03_06_02_DataBind(mConn);
    }

    private void Tab03_06_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab03_06.Rows.Count - 1; i++)
            {
                Tab03_06_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab03_06_DBUpdate(mConn, pRow);
        }

    }

    private void Tab03_06_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1;
        System.Web.UI.WebControls.TextBox txt2;
        System.Web.UI.WebControls.TextBox txt3;
        System.Web.UI.WebControls.TextBox txt4;
        System.Web.UI.WebControls.TextBox txt5;
        txt1 = (System.Web.UI.WebControls.TextBox)Tab03_06.Rows[pRow].FindControl("Tab03_06_DocEntry");
        txt2 = (System.Web.UI.WebControls.TextBox)Tab03_06.Rows[pRow].FindControl("Tab03_06_LineNum");
        txt3 = (System.Web.UI.WebControls.TextBox)Tab03_06.Rows[pRow].FindControl("Tab03_06_Description");
        txt4 = (System.Web.UI.WebControls.TextBox)Tab03_06.Rows[pRow].FindControl("Tab03_06_No");
        txt5 = (System.Web.UI.WebControls.TextBox)Tab03_06.Rows[pRow].FindControl("Tab03_06_Amt");


        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'" + txt5.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab03_06_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab03_06;
        mSQL_Tab03_06 = "";
        mSQL_Tab03_06 += "SELECT DocEntry, Text1 AS LineNum, Text2 AS Description, ";
        mSQL_Tab03_06 += " Text3 AS No, Text4 AS Amt  ";
        mSQL_Tab03_06 += " FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 3.61,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab03_06;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_03_06 = mSQL.ExecuteReader();

        Tab03_06.DataSource = mSQLDr_03_06;
        Tab03_06.DataBind();

        mSQLDr_03_06.Close();
    }

    private void Tab03_06_02_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab03_06_02 = "SELECT Cast(DocEntry AS NVARCHAR(50)) AS DocEntry, Text1 AS Remark FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 3.62,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab03_06_02;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_03_06_02 = mSQL.ExecuteReader();

        while (mSQLDr_03_06_02.Read())
        {
            Tab03_06_02_DocEntry.Text = (String)mSQLDr_03_06_02[0];
            Tab03_06_02_Remark.Text = (String)mSQLDr_03_06_02[1];
        }
        mSQLDr_03_06_02.Close();
    }

    private void Tab03_06_02_DBUpdate(SqlConnection mConn, int pRow)
    {
        String mDocEntry, mRemark;
        mDocEntry = Tab03_06_02_DocEntry.Text;
        mRemark = Tab03_06_02_Remark.Text;

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + mDocEntry + ",0,0,"
            + "'" + mRemark.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }
    #endregion

    void tab04_content(SqlConnection mConn)
    {
        try
        {
            tab04_01_creation(mConn);
            tab04_02_creation(mConn);
        }
        catch (Exception ex)
        {
            PCCore.Common.HRLog.RecordException("4", ex);
        }
    }

    #region "Tab 04 - 01"

    private void tab04_01_creation(SqlConnection mConn)
    {
        Tab04_01_DataBind(mConn);
    }

    public void Tab04_01_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab04_01_03")
        {
            Tab04_01_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab04_01.Rows[mRow].FindControl("Tab04_01_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab04_01_DataBind(mConn);
        }
        else
        {
            Tab04_01_Save(mConn, mRow);
        }

        mConn.Close();
    }

    protected void Tab04_01_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",4.11,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab04_01_Save(mConn, -1);
        //Rebind the updated datasource
        Tab04_01_DataBind(mConn);
        mConn.Close();
    }

    private void Tab04_01_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab04_01.Rows.Count - 1; i++)
            {
                Tab04_01_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab04_01_DBUpdate(mConn, pRow);
        }

    }

    private void Tab04_01_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1 = (System.Web.UI.WebControls.TextBox)Tab04_01.Rows[pRow].FindControl("Tab04_01_DocEntry");
        System.Web.UI.WebControls.TextBox txt2 = (System.Web.UI.WebControls.TextBox)Tab04_01.Rows[pRow].FindControl("Tab04_01_LineNum");
        System.Web.UI.WebControls.TextBox txt3 = (System.Web.UI.WebControls.TextBox)Tab04_01.Rows[pRow].FindControl("Tab04_01_Position");
        System.Web.UI.WebControls.TextBox txt4 = (System.Web.UI.WebControls.TextBox)Tab04_01.Rows[pRow].FindControl("Tab04_01_Planned");
        System.Web.UI.WebControls.TextBox txt5 = (System.Web.UI.WebControls.TextBox)Tab04_01.Rows[pRow].FindControl("Tab04_01_Actual");
        System.Web.UI.WebControls.TextBox txt6 = (System.Web.UI.WebControls.TextBox)Tab04_01.Rows[pRow].FindControl("Tab04_01_Differential");

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'" + txt5.Text.Replace("'", "''") + "',"
            + "'" + txt6.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        PCCore.Common.HRLog.RecordLog("Tab4_01 SaveEntry: " + mSaveEntry);
        mSQL.ExecuteNonQuery();
    }

    private void Tab04_01_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab04_01;
        mSQL_Tab04_01 = "";
        mSQL_Tab04_01 += "SELECT DocEntry, Text1 AS LineNum, Text2 AS Position, ";
        mSQL_Tab04_01 += " Text3 AS Planned, Text4 AS Actual, Text5 AS Differential  ";
        mSQL_Tab04_01 += " FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 4.11,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab04_01;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_04_01 = mSQL.ExecuteReader();

        Tab04_01.DataSource = mSQLDr_04_01;
        Tab04_01.DataBind();

        mSQLDr_04_01.Close();
    }
    #endregion
    #region "Tab 04 - 02"
    private void tab04_02_creation(SqlConnection mConn)
    {
        Tab04_02_DataBind(mConn);
        Tab04_02_02_DataBind(mConn);
    }

    public void Tab04_02_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab04_02_03")
        {
            Tab04_02_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab04_02.Rows[mRow].FindControl("Tab04_02_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab04_02_DataBind(mConn);
        }
        else
        {
            Tab04_02_Save(mConn, mRow);
        }

        mConn.Close();
    }

    protected void Tab04_02_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",4.21,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab04_02_Save(mConn, -1);
        //Rebind the updated datasource
        Tab04_02_DataBind(mConn);
        mConn.Close();
    }

    private void Tab04_02_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab04_02.Rows.Count - 1; i++)
            {
                Tab04_02_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab04_02_DBUpdate(mConn, pRow);
        }

    }

    private void Tab04_02_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1 = (System.Web.UI.WebControls.TextBox)Tab04_02.Rows[pRow].FindControl("Tab04_02_DocEntry");
        System.Web.UI.WebControls.TextBox txt2 = (System.Web.UI.WebControls.TextBox)Tab04_02.Rows[pRow].FindControl("Tab04_02_LineNum");
        System.Web.UI.WebControls.TextBox txt3 = (System.Web.UI.WebControls.TextBox)Tab04_02.Rows[pRow].FindControl("Tab04_02_Plant");
        System.Web.UI.WebControls.TextBox txt4 = (System.Web.UI.WebControls.TextBox)Tab04_02.Rows[pRow].FindControl("Tab04_02_Planned");
        System.Web.UI.WebControls.TextBox txt5 = (System.Web.UI.WebControls.TextBox)Tab04_02.Rows[pRow].FindControl("Tab04_02_Actual");
        System.Web.UI.WebControls.TextBox txt6 = (System.Web.UI.WebControls.TextBox)Tab04_02.Rows[pRow].FindControl("Tab04_02_Differential");

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'" + txt5.Text.Replace("'", "''") + "',"
            + "'" + txt6.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab04_02_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab04_02;
        mSQL_Tab04_02 = "";
        mSQL_Tab04_02 += "SELECT DocEntry, Text1 AS LineNum, Text2 AS Plant, ";
        mSQL_Tab04_02 += " Text3 AS Planned, Text4 AS Actual, Text5 AS Differential  ";
        mSQL_Tab04_02 += " FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 4.21,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab04_02;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_04_02 = mSQL.ExecuteReader();

        Tab04_02.DataSource = mSQLDr_04_02;
        Tab04_02.DataBind();

        mSQLDr_04_02.Close();
    }
    private void Tab04_02_02_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab04_02_02 = "SELECT Cast(DocEntry AS NVARCHAR(50)) AS DocEntry, Text1 AS Remark FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 4.22,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab04_02_02;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_04_02_02 = mSQL.ExecuteReader();

        while (mSQLDr_04_02_02.Read())
        {
            Tab04_02_02_DocEntry.Text = (String)mSQLDr_04_02_02[0];
            Tab04_02_02_Remark.Text = (String)mSQLDr_04_02_02[1];
        }
        mSQLDr_04_02_02.Close();
    }

    private void Tab04_02_02_DBUpdate(SqlConnection mConn, int pRow)
    {
        String mDocEntry, mRemark;
        mDocEntry = Tab04_02_02_DocEntry.Text;
        mRemark = Tab04_02_02_Remark.Text;

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + mDocEntry + ",0,0,"
            + "'" + mRemark.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        PCCore.Common.HRLog.RecordLog("Tab4_02_02 SaveEntry: " + mSaveEntry);
        mSQL.ExecuteNonQuery();
    }

    private void Tab04_02_02_Save(SqlConnection mConn, int pRow)
    {
            Tab04_02_02_DBUpdate(mConn, 0);
    }
    #endregion

    void tab05_content(SqlConnection mConn)
    {
        try
        {

            tab05_01_creation(mConn);
            tab05_02_creation(mConn);
        }
        catch (Exception ex)
        {
            PCCore.Common.HRLog.RecordException("5", ex);
        }
    }
    #region "Tab 05 - 01"

    private void tab05_01_creation(SqlConnection mConn)
    {
        Tab05_01_DataBind(mConn);
    }

    private void Tab05_01_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab05_01.Rows.Count - 1; i++)
            {
                Tab05_01_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab05_01_DBUpdate(mConn, pRow);
        }

    }

    private void Tab05_01_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1;
        System.Web.UI.WebControls.TextBox txt2;
        System.Web.UI.WebControls.TextBox txt3;
        System.Web.UI.WebControls.TextBox txt4;
        System.Web.UI.WebControls.TextBox txt5;
        txt1 = (System.Web.UI.WebControls.TextBox)Tab05_01.Rows[pRow].FindControl("Tab05_01_DocEntry");
        txt2 = (System.Web.UI.WebControls.TextBox)Tab05_01.Rows[pRow].FindControl("Tab05_01_LineNum");
        txt3 = (System.Web.UI.WebControls.TextBox)Tab05_01.Rows[pRow].FindControl("Tab05_01_Description");
        txt4 = (System.Web.UI.WebControls.TextBox)Tab05_01.Rows[pRow].FindControl("Tab05_01_ThisMonth");
        txt5 = (System.Web.UI.WebControls.TextBox)Tab05_01.Rows[pRow].FindControl("Tab05_01_Accumulated");


        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'" + txt5.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab05_01_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab05_01;
        mSQL_Tab05_01 = "";
        mSQL_Tab05_01 += "SELECT DocEntry, Text1 AS LineNum, Text2 AS Description, ";
        mSQL_Tab05_01 += " Text3 AS ThisMonth, Text4 AS Accumulated ";
        mSQL_Tab05_01 += " FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 5.11,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab05_01;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_05_01 = mSQL.ExecuteReader();

        Tab05_01.DataSource = mSQLDr_05_01;
        Tab05_01.DataBind();

        mSQLDr_05_01.Close();
    }

    #endregion
    #region "Tab 05 - 02"

    
    private void tab05_02_creation(SqlConnection mConn)
    {
        Tab05_02_DataBind(mConn);
        Tab05_02_02_DataBind(mConn);
    }

    public void Tab05_02_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab05_02_03")
        {
            Tab05_02_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab05_02.Rows[mRow].FindControl("Tab05_02_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab05_02_DataBind(mConn);
        }
        else
        {
            Tab05_02_Save(mConn, mRow);
        }

        mConn.Close();
    }

    protected void Tab05_02_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",5.21,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab05_02_Save(mConn, -1);
        //Rebind the updated datasource
        Tab05_02_DataBind(mConn);
        mConn.Close();
    }


    private void Tab05_02_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab05_02.Rows.Count - 1; i++)
            {
                Tab05_02_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab05_02_DBUpdate(mConn, pRow);
        }

    }
    private void Tab05_02_02_Save(SqlConnection mConn, int pRow)
    {

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + Tab05_02_02_DocEntry.Text + ",0,0,"
            + "'" + Tab05_02_02_Remark.Text + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();

    }
    private void Tab05_02_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1 = (System.Web.UI.WebControls.TextBox)Tab05_02.Rows[pRow].FindControl("Tab05_02_DocEntry");
        System.Web.UI.WebControls.TextBox txt2 = (System.Web.UI.WebControls.TextBox)Tab05_02.Rows[pRow].FindControl("Tab05_02_LineNum");
        System.Web.UI.WebControls.TextBox txt3 = (System.Web.UI.WebControls.TextBox)Tab05_02.Rows[pRow].FindControl("Tab05_02_Description");
        System.Web.UI.WebControls.TextBox txt4 = (System.Web.UI.WebControls.TextBox)Tab05_02.Rows[pRow].FindControl("Tab05_02_No");
        System.Web.UI.WebControls.TextBox txt5 = (System.Web.UI.WebControls.TextBox)Tab05_02.Rows[pRow].FindControl("Tab05_02_Amt");
        System.Web.UI.WebControls.TextBox txt6 = (System.Web.UI.WebControls.TextBox)Tab05_02.Rows[pRow].FindControl("Tab05_02_CumNo");
        System.Web.UI.WebControls.TextBox txt7 = (System.Web.UI.WebControls.TextBox)Tab05_02.Rows[pRow].FindControl("Tab05_02_CumAmt");

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'" + txt5.Text.Replace("'", "''") + "',"
            + "'" + txt6.Text.Replace("'", "''") + "',"
            + "'" + txt7.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab05_02_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab05_02;
        mSQL_Tab05_02 = "";
        mSQL_Tab05_02 += "SELECT DocEntry, Text1 AS LineNum, Text2 AS Description, ";
        mSQL_Tab05_02 += " Text3 AS No, Text4 AS Amt, ";
        mSQL_Tab05_02 += " Text5 AS CumNo, Text6 AS CumAmt ";
        mSQL_Tab05_02 += " FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 5.21,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab05_02;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_05_02 = mSQL.ExecuteReader();

        Tab05_02.DataSource = mSQLDr_05_02;
        Tab05_02.DataBind();

        mSQLDr_05_02.Close();
    }

    private void Tab05_02_02_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab05_02_02 = "SELECT Cast(DocEntry AS NVARCHAR(50)) AS DocEntry, Text1 AS Remark FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ", 5.22,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab05_02_02;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_05_02_02 = mSQL.ExecuteReader();

        while (mSQLDr_05_02_02.Read())
        {
            Tab05_02_02_DocEntry.Text = (String)mSQLDr_05_02_02[0];
            Tab05_02_02_Remark.Text = (String)mSQLDr_05_02_02[1];
        }
        mSQLDr_05_02_02.Close();
    }

    private void Tab05_02_02_DBUpdate(SqlConnection mConn, int pRow)
    {
        String mDocEntry, mRemark;
        mDocEntry = Tab05_02_02_DocEntry.Text;
        mRemark = Tab05_02_02_Remark.Text;

        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + mDocEntry + ",0,0,"
            + "'" + mRemark.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    #endregion

    void tab06_content(SqlConnection mConn)
    {
        try
        {
            tab06_01_creation(mConn);
            tab06_02_creation(mConn);
            tab06_03_creation(mConn);
            Tab06_04_creation(mConn);
            Tab06_05_creation(mConn);
            Tab06_06_creation(mConn);
        }
        catch (Exception ex)
        {
            PCCore.Common.HRLog.RecordException("6", ex);
        }
    }
    #region "TAB 06 - 01"

    private void tab06_01_creation(SqlConnection mConn)
    {
        Tab06_01_DataBind(mConn);
    }

    public void Tab06_01_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab06_01_03")
        {
            Tab06_01_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab06_01.Rows[mRow].FindControl("Tab06_01_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab06_01_DataBind(mConn);
        }
        else
        {
            Tab06_01_Save(mConn, mRow);
        }

        mConn.Close();
    }

    protected void Tab06_01_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",6.1,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab06_01_Save(mConn, -1);
        //Rebind the updated datasource
        Tab06_01_DataBind(mConn);
        mConn.Close();
    }

    private void Tab06_01_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab06_01.Rows.Count - 1; i++)
            {
                Tab06_01_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab06_01_DBUpdate(mConn, pRow);
        }

    }

    private void Tab06_01_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1 = (System.Web.UI.WebControls.TextBox)Tab06_01.Rows[pRow].FindControl("Tab06_01_DocEntry");
        System.Web.UI.WebControls.TextBox txt2 = (System.Web.UI.WebControls.TextBox)Tab06_01.Rows[pRow].FindControl("Tab06_Detail");
        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab06_01_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab06_01 = "SELECT DocEntry, Text1 AS Detail,'' AS 'Delete' FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ",6.1,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab06_01;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_06_01 = mSQL.ExecuteReader();

        Tab06_01.DataSource = mSQLDr_06_01;
        Tab06_01.DataBind();

        mSQLDr_06_01.Close();
    }
    #endregion    
    #region "TAB 06 - 02"

    private void tab06_02_creation(SqlConnection mConn)
    {
        Tab06_02_DataBind(mConn);
    }

    public void Tab06_02_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab06_02_03")
        {
            Tab06_02_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab06_02.Rows[mRow].FindControl("Tab06_02_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab06_02_DataBind(mConn);
        }
        else
        {
            Tab06_02_Save(mConn, mRow);
        }

        mConn.Close();
    }

    protected void Tab06_02_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",6.21,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab06_02_Save(mConn, -1);
        //Rebind the updated datasource
        Tab06_02_DataBind(mConn);
        mConn.Close();
    }

    private void Tab06_02_Save(SqlConnection mConn, int pRow)
    {
        PCCore.Common.HRLog.RecordLog("Tab06_02_Save: " + Tab06_02.Rows.Count);
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab06_02.Rows.Count - 1; i++)
            {
                Tab06_02_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab06_02_DBUpdate(mConn, pRow);
        }

    }

    private void Tab06_02_DBUpdate(SqlConnection mConn, int pRow)
    {
        PCCore.Common.HRLog.RecordLog("Tab06_02_DBUpdate");
        System.Web.UI.WebControls.TextBox txt1 = (System.Web.UI.WebControls.TextBox)Tab06_02.Rows[pRow].FindControl("Tab06_02_DocEntry");
        System.Web.UI.WebControls.TextBox txt2 = (System.Web.UI.WebControls.TextBox)Tab06_02.Rows[pRow].FindControl("Tab06_02_Type");
        System.Web.UI.WebControls.TextBox txt3 = (System.Web.UI.WebControls.TextBox)Tab06_02.Rows[pRow].FindControl("Tab06_02_R_Period");
        System.Web.UI.WebControls.TextBox txt4 = (System.Web.UI.WebControls.TextBox)Tab06_02.Rows[pRow].FindControl("Tab06_02_R_ToDate");
        System.Web.UI.WebControls.TextBox txt5 = (System.Web.UI.WebControls.TextBox)Tab06_02.Rows[pRow].FindControl("Tab06_02_C_Period");
        System.Web.UI.WebControls.TextBox txt6 = (System.Web.UI.WebControls.TextBox)Tab06_02.Rows[pRow].FindControl("Tab06_02_C_ToDate");
        System.Web.UI.WebControls.TextBox txt7 = (System.Web.UI.WebControls.TextBox)Tab06_02.Rows[pRow].FindControl("Tab06_02_O_Period");
        System.Web.UI.WebControls.TextBox txt8 = (System.Web.UI.WebControls.TextBox)Tab06_02.Rows[pRow].FindControl("Tab06_02_O_ToDate");
        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'" + txt5.Text.Replace("'", "''") + "',"
            + "'" + txt6.Text.Replace("'", "''") + "',"
            + "'" + txt7.Text.Replace("'", "''") + "',"
            + "'" + txt8.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }



    

    private void Tab06_02_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab06_02 = "SELECT DocEntry, Text1 AS Type, Text2 as R_Period, Text3 AS R_ToDate, Text4 AS C_Period, Text5 AS C_ToDate, Text6 AS O_Period, Text7 AS O_ToDate,'' AS 'Delete' FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ",6.21,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab06_02;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_06_02 = mSQL.ExecuteReader();

        Tab06_02.DataSource = mSQLDr_06_02;
        Tab06_02.DataBind();

        mSQLDr_06_02.Close();
    }
    #endregion    

    #region "TAB 06 - 03"

    private void tab06_03_creation(SqlConnection mConn)
    {
        Tab06_03_DataBind(mConn);
        Tab06_A3_DataBind(mConn);
        Tab06_B3_DataBind(mConn);
    }

    private void Tab06_03_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab06_03.Rows.Count - 1; i++)
            {
                Tab06_03_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab06_03_DBUpdate(mConn, pRow);
        }

    }

    private void Tab06_03_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1 = (System.Web.UI.WebControls.TextBox)Tab06_03.Rows[pRow].FindControl("Tab06_03_DocEntry");
        PCCore.Common.HRLog.RecordLog("txt1 value: " + txt1.Text);
        System.Web.UI.WebControls.TextBox txt2 = (System.Web.UI.WebControls.TextBox)Tab06_03.Rows[pRow].FindControl("Tab06_03_Num");
        PCCore.Common.HRLog.RecordLog("txt2 value: " + txt2.Text);
        System.Web.UI.WebControls.TextBox txt3 = (System.Web.UI.WebControls.TextBox)Tab06_03.Rows[pRow].FindControl("Tab06_03_NoObj");
        PCCore.Common.HRLog.RecordLog("txt3 value: " + txt3.Text);
        System.Web.UI.WebControls.TextBox txt4 = (System.Web.UI.WebControls.TextBox)Tab06_03.Rows[pRow].FindControl("Tab06_03_NoObjSub");
        PCCore.Common.HRLog.RecordLog("txt4 value: " + txt4.Text);
        System.Web.UI.WebControls.TextBox txt5 = (System.Web.UI.WebControls.TextBox)Tab06_03.Rows[pRow].FindControl("Tab06_03_Rej");
        PCCore.Common.HRLog.RecordLog("txt5 value: " + txt5.Text);
        System.Web.UI.WebControls.TextBox txt6 = (System.Web.UI.WebControls.TextBox)Tab06_03.Rows[pRow].FindControl("Tab06_03_NotYet");
        PCCore.Common.HRLog.RecordLog("txt6 value: " + txt6.Text);
        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'" + txt5.Text.Replace("'", "''") + "',"
            + "'" + txt6.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab06_03_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab06_03 = "SELECT DocEntry, Text1 AS Num, Text2 as NoObj, Text3 AS NoObjSub, Text4 AS Rej, Text5 AS NotYet,'' AS 'Delete' FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ",6.31,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab06_03;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_06_03 = mSQL.ExecuteReader();

        Tab06_03.DataSource = mSQLDr_06_03;
        Tab06_03.DataBind();

        mSQLDr_06_03.Close();
    }
    #endregion  
    #region "TAB 06 - A3"

    private void Tab06_A3_creation(SqlConnection mConn)
    {
        Tab06_A3_DataBind(mConn);
    }

    public void Tab06_A3_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab06_A3_03")
        {
            Tab06_A3_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab06_A3.Rows[mRow].FindControl("Tab06_A3_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab06_A3_DataBind(mConn);
        }
        else
        {
            Tab06_A3_Save(mConn, mRow);
        }

        mConn.Close();
    }

    protected void Tab06_A3_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",6.32,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab06_A3_Save(mConn, -1);
        //Rebind the updated datasource
        Tab06_A3_DataBind(mConn);
        mConn.Close();
    }

    private void Tab06_A3_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab06_A3.Rows.Count - 1; i++)
            {
                Tab06_A3_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab06_A3_DBUpdate(mConn, pRow);
        }

    }

    private void Tab06_A3_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1 = (System.Web.UI.WebControls.TextBox)Tab06_A3.Rows[pRow].FindControl("Tab06_A3_DocEntry");
        System.Web.UI.WebControls.TextBox txt2 = (System.Web.UI.WebControls.TextBox)Tab06_A3.Rows[pRow].FindControl("Tab06_A3_LS");
        System.Web.UI.WebControls.TextBox txt3 = (System.Web.UI.WebControls.TextBox)Tab06_A3.Rows[pRow].FindControl("Tab06_A3_OS");
        System.Web.UI.WebControls.TextBox txt4 = (System.Web.UI.WebControls.TextBox)Tab06_A3.Rows[pRow].FindControl("Tab06_A3_RS");
        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab06_A3_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab06_A3 = "SELECT DocEntry, Text1 AS LS, Text2 as OS, Text3 AS RS,'' AS 'Delete' FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ",6.32,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab06_A3;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_06_03 = mSQL.ExecuteReader();

        Tab06_A3.DataSource = mSQLDr_06_03;
        Tab06_A3.DataBind();

        mSQLDr_06_03.Close();
    }
    #endregion  
    #region "TAB 06 - B3"

    private void Tab06_B3_creation(SqlConnection mConn)
    {
        Tab06_B3_DataBind(mConn);
    }

    public void Tab06_B3_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab06_B3_03")
        {
            Tab06_B3_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab06_B3.Rows[mRow].FindControl("Tab06_B3_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab06_B3_DataBind(mConn);
        }
        else
        {
            Tab06_B3_Save(mConn, mRow);
        }

        mConn.Close();
    }

    protected void Tab06_B3_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",6.33,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab06_B3_Save(mConn, -1);
        //Rebind the updated datasource
        Tab06_B3_DataBind(mConn);
        mConn.Close();
    }

    private void Tab06_B3_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab06_B3.Rows.Count - 1; i++)
            {
                Tab06_B3_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab06_B3_DBUpdate(mConn, pRow);
        }

    }

    private void Tab06_B3_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1 = (System.Web.UI.WebControls.TextBox)Tab06_B3.Rows[pRow].FindControl("Tab06_B3_DocEntry");
        System.Web.UI.WebControls.TextBox txt2 = (System.Web.UI.WebControls.TextBox)Tab06_B3.Rows[pRow].FindControl("Tab06_B3_SP");
        System.Web.UI.WebControls.TextBox txt3 = (System.Web.UI.WebControls.TextBox)Tab06_B3.Rows[pRow].FindControl("Tab06_B3_PS");
        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab06_B3_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab06_B3 = "SELECT DocEntry, Text1 AS SP, Text2 as PS,'' AS 'Delete' FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ",6.33,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab06_B3;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_06_03 = mSQL.ExecuteReader();

        Tab06_B3.DataSource = mSQLDr_06_03;
        Tab06_B3.DataBind();

        mSQLDr_06_03.Close();


        String mSQL_Tab06_B3_Hdr;
        mSQL_Tab06_B3_Hdr = "SELECT ISNULL(Alias1,'') FROM CPSPMRH WHERE BaseEntry = " + txtDocEntry.Text + " AND SubEntry = '6.33' ";
        SqlCommand mSQL_Hdr;
        mSQL_Hdr = new SqlCommand();
        mSQL_Hdr.CommandText = mSQL_Tab06_B3_Hdr;
        mSQL_Hdr.CommandType = CommandType.Text;
        mSQL_Hdr.Connection = mConn;
        SqlDataReader mSQLDr_06_B3_Hdrs = mSQL_Hdr.ExecuteReader();
        while (mSQLDr_06_B3_Hdrs.Read())
        {
            Tab06_B3.Columns[1].HeaderText = (String)mSQLDr_06_B3_Hdrs[0];
        }
        mSQLDr_06_B3_Hdrs.Close();

        
    }
    #endregion  

    #region "TAB 06 - 04"

    private void Tab06_04_creation(SqlConnection mConn)
    {
        Tab06_04_DataBind(mConn);
        Tab06_A4_DataBind(mConn);
        Tab06_B4_DataBind(mConn);
    }

    private void Tab06_04_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab06_04.Rows.Count - 1; i++)
            {
                Tab06_04_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab06_04_DBUpdate(mConn, pRow);
        }

    }

    private void Tab06_04_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1 = (System.Web.UI.WebControls.TextBox)Tab06_04.Rows[pRow].FindControl("Tab06_04_DocEntry");
        System.Web.UI.WebControls.TextBox txt2 = (System.Web.UI.WebControls.TextBox)Tab06_04.Rows[pRow].FindControl("Tab06_04_Num");
        System.Web.UI.WebControls.TextBox txt3 = (System.Web.UI.WebControls.TextBox)Tab06_04.Rows[pRow].FindControl("Tab06_04_NoObj");
        System.Web.UI.WebControls.TextBox txt4 = (System.Web.UI.WebControls.TextBox)Tab06_04.Rows[pRow].FindControl("Tab06_04_NoObjSub");
        System.Web.UI.WebControls.TextBox txt5 = (System.Web.UI.WebControls.TextBox)Tab06_04.Rows[pRow].FindControl("Tab06_04_Rej");
        System.Web.UI.WebControls.TextBox txt6 = (System.Web.UI.WebControls.TextBox)Tab06_04.Rows[pRow].FindControl("Tab06_04_NotYet");
        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'" + txt5.Text.Replace("'", "''") + "',"
            + "'" + txt6.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab06_04_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab06_04 = "SELECT DocEntry, Text1 AS Num, Text2 as NoObj, Text3 AS NoObjSub, Text4 AS Rej, Text5 AS NotYet,'' AS 'Delete' FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ",6.41,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab06_04;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_06_03 = mSQL.ExecuteReader();

        Tab06_04.DataSource = mSQLDr_06_03;
        Tab06_04.DataBind();

        mSQLDr_06_03.Close();
    }
    #endregion
    #region "TAB 06 - A4"

    private void Tab06_A4_creation(SqlConnection mConn)
    {
        Tab06_A4_DataBind(mConn);
    }

    public void Tab06_A4_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab06_A4_03")
        {
            Tab06_A4_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab06_A4.Rows[mRow].FindControl("Tab06_A4_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab06_A4_DataBind(mConn);
        }
        else
        {
            Tab06_A4_Save(mConn, mRow);
        }

        mConn.Close();
    }

    protected void Tab06_A4_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",6.42,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab06_A4_Save(mConn, -1);
        //Rebind the updated datasource
        Tab06_A4_DataBind(mConn);
        mConn.Close();
    }

    private void Tab06_A4_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab06_A4.Rows.Count - 1; i++)
            {
                Tab06_A4_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab06_A4_DBUpdate(mConn, pRow);
        }

    }

    private void Tab06_A4_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1 = (System.Web.UI.WebControls.TextBox)Tab06_A4.Rows[pRow].FindControl("Tab06_A4_DocEntry");
        System.Web.UI.WebControls.TextBox txt2 = (System.Web.UI.WebControls.TextBox)Tab06_A4.Rows[pRow].FindControl("Tab06_A4_LS");
        System.Web.UI.WebControls.TextBox txt3 = (System.Web.UI.WebControls.TextBox)Tab06_A4.Rows[pRow].FindControl("Tab06_A4_OS");
        System.Web.UI.WebControls.TextBox txt4 = (System.Web.UI.WebControls.TextBox)Tab06_A4.Rows[pRow].FindControl("Tab06_A4_RS");
        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab06_A4_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab06_A4 = "SELECT DocEntry, Text1 AS LS, Text2 as OS, Text3 AS RS,'' AS 'Delete' FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ",6.42,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab06_A4;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_06_03 = mSQL.ExecuteReader();

        Tab06_A4.DataSource = mSQLDr_06_03;
        Tab06_A4.DataBind();

        mSQLDr_06_03.Close();
    }
    #endregion
    #region "TAB 06 - B4"

    private void Tab06_B4_creation(SqlConnection mConn)
    {
        Tab06_B4_DataBind(mConn);
    }

    public void Tab06_B4_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab06_B4_03")
        {
            Tab06_B4_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab06_B4.Rows[mRow].FindControl("Tab06_B4_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab06_B4_DataBind(mConn);
        }
        else
        {
            Tab06_B4_Save(mConn, mRow);
        }

        mConn.Close();
    }

    protected void Tab06_B4_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",6.43,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab06_B4_Save(mConn, -1);
        //Rebind the updated datasource
        Tab06_B4_DataBind(mConn);
        mConn.Close();
    }

    private void Tab06_B4_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab06_B4.Rows.Count - 1; i++)
            {
                Tab06_B4_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab06_B4_DBUpdate(mConn, pRow);
        }

    }

    private void Tab06_B4_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1 = (System.Web.UI.WebControls.TextBox)Tab06_B4.Rows[pRow].FindControl("Tab06_B4_DocEntry");
        System.Web.UI.WebControls.TextBox txt2 = (System.Web.UI.WebControls.TextBox)Tab06_B4.Rows[pRow].FindControl("Tab06_B4_SP");
        System.Web.UI.WebControls.TextBox txt3 = (System.Web.UI.WebControls.TextBox)Tab06_B4.Rows[pRow].FindControl("Tab06_B4_PS");
        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab06_B4_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab06_B4 = "SELECT DocEntry, Text1 AS SP, Text2 as PS,'' AS 'Delete' FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ",6.43,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab06_B4;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_06_03 = mSQL.ExecuteReader();

        Tab06_B4.DataSource = mSQLDr_06_03;
        Tab06_B4.DataBind();

        mSQLDr_06_03.Close();

        String mSQL_Tab06_B4_Hdr;
        mSQL_Tab06_B4_Hdr = "SELECT ISNULL(Alias1,'') FROM CPSPMRH WHERE BaseEntry = " + txtDocEntry.Text + " AND SubEntry = '6.43' ";
        SqlCommand mSQL_Hdr;
        mSQL_Hdr = new SqlCommand();
        mSQL_Hdr.CommandText = mSQL_Tab06_B4_Hdr;
        mSQL_Hdr.CommandType = CommandType.Text;
        mSQL_Hdr.Connection = mConn;
        SqlDataReader mSQLDr_06_B4_Hdrs = mSQL_Hdr.ExecuteReader();
        while (mSQLDr_06_B4_Hdrs.Read())
        {
            Tab06_B4.Columns[1].HeaderText = (String)mSQLDr_06_B4_Hdrs[0];
        }
        mSQLDr_06_B4_Hdrs.Close();
    }
    #endregion  

    #region "TAB 06 - 05"

    private void Tab06_05_creation(SqlConnection mConn)
    {
        Tab06_05_DataBind(mConn);
        Tab06_A5_DataBind(mConn);
        Tab06_B5_DataBind(mConn);
    }

    private void Tab06_05_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab06_05.Rows.Count - 1; i++)
            {
                Tab06_05_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab06_05_DBUpdate(mConn, pRow);
        }

    }

    private void Tab06_05_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1 = (System.Web.UI.WebControls.TextBox)Tab06_05.Rows[pRow].FindControl("Tab06_05_DocEntry");
        System.Web.UI.WebControls.TextBox txt2 = (System.Web.UI.WebControls.TextBox)Tab06_05.Rows[pRow].FindControl("Tab06_05_Num");
        System.Web.UI.WebControls.TextBox txt3 = (System.Web.UI.WebControls.TextBox)Tab06_05.Rows[pRow].FindControl("Tab06_05_NoObj");
        System.Web.UI.WebControls.TextBox txt4 = (System.Web.UI.WebControls.TextBox)Tab06_05.Rows[pRow].FindControl("Tab06_05_NoObjSub");
        System.Web.UI.WebControls.TextBox txt5 = (System.Web.UI.WebControls.TextBox)Tab06_05.Rows[pRow].FindControl("Tab06_05_Rej");
        System.Web.UI.WebControls.TextBox txt6 = (System.Web.UI.WebControls.TextBox)Tab06_05.Rows[pRow].FindControl("Tab06_05_NotYet");
        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'" + txt5.Text.Replace("'", "''") + "',"
            + "'" + txt6.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab06_05_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab06_05 = "SELECT DocEntry, Text1 AS Num, Text2 as NoObj, Text3 AS NoObjSub, Text4 AS Rej, Text5 AS NotYet,'' AS 'Delete' FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ",6.51,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab06_05;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_06_03 = mSQL.ExecuteReader();

        Tab06_05.DataSource = mSQLDr_06_03;
        Tab06_05.DataBind();

        mSQLDr_06_03.Close();
    }
    #endregion
    #region "TAB 06 - A5"

    private void Tab06_A5_creation(SqlConnection mConn)
    {
        Tab06_A5_DataBind(mConn);
    }

    public void Tab06_A5_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab06_A5_03")
        {
            Tab06_A5_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab06_A5.Rows[mRow].FindControl("Tab06_A5_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab06_A5_DataBind(mConn);
        }
        else
        {
            Tab06_A5_Save(mConn, mRow);
        }

        mConn.Close();
    }

    protected void Tab06_A5_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",6.52,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab06_A5_Save(mConn, -1);
        //Rebind the updated datasource
        Tab06_A5_DataBind(mConn);
        mConn.Close();
    }

    private void Tab06_A5_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab06_A5.Rows.Count - 1; i++)
            {
                Tab06_A5_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab06_A5_DBUpdate(mConn, pRow);
        }

    }

    private void Tab06_A5_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1 = (System.Web.UI.WebControls.TextBox)Tab06_A5.Rows[pRow].FindControl("Tab06_A5_DocEntry");
        System.Web.UI.WebControls.TextBox txt2 = (System.Web.UI.WebControls.TextBox)Tab06_A5.Rows[pRow].FindControl("Tab06_A5_LS");
        System.Web.UI.WebControls.TextBox txt3 = (System.Web.UI.WebControls.TextBox)Tab06_A5.Rows[pRow].FindControl("Tab06_A5_OS");
        System.Web.UI.WebControls.TextBox txt4 = (System.Web.UI.WebControls.TextBox)Tab06_A5.Rows[pRow].FindControl("Tab06_A5_RS");
        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab06_A5_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab06_A5 = "SELECT DocEntry, Text1 AS LS, Text2 as OS, Text3 AS RS,'' AS 'Delete' FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ",6.52,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab06_A5;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_06_03 = mSQL.ExecuteReader();

        Tab06_A5.DataSource = mSQLDr_06_03;
        Tab06_A5.DataBind();

        mSQLDr_06_03.Close();
    }
    #endregion
    #region "TAB 06 - B5"

    private void Tab06_B5_creation(SqlConnection mConn)
    {
        Tab06_B5_DataBind(mConn);
    }

    public void Tab06_B5_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab06_B5_03")
        {
            Tab06_B5_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab06_B5.Rows[mRow].FindControl("Tab06_B5_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab06_B5_DataBind(mConn);
        }
        else
        {
            Tab06_B5_Save(mConn, mRow);
        }

        mConn.Close();
    }

    protected void Tab06_B5_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",6.53,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab06_B5_Save(mConn, -1);
        //Rebind the updated datasource
        Tab06_B5_DataBind(mConn);
        mConn.Close();
    }

    private void Tab06_B5_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab06_B5.Rows.Count - 1; i++)
            {
                Tab06_B5_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab06_B5_DBUpdate(mConn, pRow);
        }

    }

    private void Tab06_B5_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1 = (System.Web.UI.WebControls.TextBox)Tab06_B5.Rows[pRow].FindControl("Tab06_B5_DocEntry");
        System.Web.UI.WebControls.TextBox txt2 = (System.Web.UI.WebControls.TextBox)Tab06_B5.Rows[pRow].FindControl("Tab06_B5_SP");
        System.Web.UI.WebControls.TextBox txt3 = (System.Web.UI.WebControls.TextBox)Tab06_B5.Rows[pRow].FindControl("Tab06_B5_PS");
        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab06_B5_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab06_B5 = "SELECT DocEntry, Text1 AS SP, Text2 as PS,'' AS 'Delete' FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ",6.53,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab06_B5;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_06_03 = mSQL.ExecuteReader();

        Tab06_B5.DataSource = mSQLDr_06_03;
        Tab06_B5.DataBind();

        mSQLDr_06_03.Close();

        String mSQL_Tab06_B5_Hdr;
        mSQL_Tab06_B5_Hdr = "SELECT ISNULL(Alias1,'') FROM CPSPMRH WHERE BaseEntry = " + txtDocEntry.Text + " AND SubEntry = '6.53' ";
        SqlCommand mSQL_Hdr;
        mSQL_Hdr = new SqlCommand();
        mSQL_Hdr.CommandText = mSQL_Tab06_B5_Hdr;
        mSQL_Hdr.CommandType = CommandType.Text;
        mSQL_Hdr.Connection = mConn;
        SqlDataReader mSQLDr_06_B5_Hdrs = mSQL_Hdr.ExecuteReader();
        while (mSQLDr_06_B5_Hdrs.Read())
        {
            Tab06_B5.Columns[1].HeaderText = (String)mSQLDr_06_B5_Hdrs[0];
        }
        mSQLDr_06_B5_Hdrs.Close();
    }
    #endregion  

    #region "TAB 06 - 06"

    private void Tab06_06_creation(SqlConnection mConn)
    {
        Tab06_06_DataBind(mConn);
        Tab06_A6_DataBind(mConn);
        Tab06_B6_DataBind(mConn);
    }

    private void Tab06_06_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab06_06.Rows.Count - 1; i++)
            {
                Tab06_06_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab06_06_DBUpdate(mConn, pRow);
        }

    }

    private void Tab06_06_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1 = (System.Web.UI.WebControls.TextBox)Tab06_06.Rows[pRow].FindControl("Tab06_06_DocEntry");
        System.Web.UI.WebControls.TextBox txt2 = (System.Web.UI.WebControls.TextBox)Tab06_06.Rows[pRow].FindControl("Tab06_06_Num");
        System.Web.UI.WebControls.TextBox txt3 = (System.Web.UI.WebControls.TextBox)Tab06_06.Rows[pRow].FindControl("Tab06_06_NoObj");
        System.Web.UI.WebControls.TextBox txt4 = (System.Web.UI.WebControls.TextBox)Tab06_06.Rows[pRow].FindControl("Tab06_06_NoObjSub");
        System.Web.UI.WebControls.TextBox txt5 = (System.Web.UI.WebControls.TextBox)Tab06_06.Rows[pRow].FindControl("Tab06_06_Rej");
        System.Web.UI.WebControls.TextBox txt6 = (System.Web.UI.WebControls.TextBox)Tab06_06.Rows[pRow].FindControl("Tab06_06_NotYet");
        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'" + txt5.Text.Replace("'", "''") + "',"
            + "'" + txt6.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab06_06_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab06_06 = "SELECT DocEntry, Text1 AS Num, Text2 as NoObj, Text3 AS NoObjSub, Text4 AS Rej, Text5 AS NotYet,'' AS 'Delete' FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ",6.61,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab06_06;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_06_03 = mSQL.ExecuteReader();

        Tab06_06.DataSource = mSQLDr_06_03;
        Tab06_06.DataBind();

        mSQLDr_06_03.Close();
    }
    #endregion
    #region "TAB 06 - A6"

    private void Tab06_A6_creation(SqlConnection mConn)
    {
        Tab06_A6_DataBind(mConn);
    }

    public void Tab06_A6_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab06_A6_03")
        {
            Tab06_A6_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab06_A6.Rows[mRow].FindControl("Tab06_A6_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab06_A6_DataBind(mConn);
        }
        else
        {
            Tab06_A6_Save(mConn, mRow);
        }

        mConn.Close();
    }

    protected void Tab06_A6_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",6.62,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab06_A6_Save(mConn, -1);
        //Rebind the updated datasource
        Tab06_A6_DataBind(mConn);
        mConn.Close();
    }

    private void Tab06_A6_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab06_A6.Rows.Count - 1; i++)
            {
                Tab06_A6_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab06_A6_DBUpdate(mConn, pRow);
        }

    }

    private void Tab06_A6_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1 = (System.Web.UI.WebControls.TextBox)Tab06_A6.Rows[pRow].FindControl("Tab06_A6_DocEntry");
        System.Web.UI.WebControls.TextBox txt2 = (System.Web.UI.WebControls.TextBox)Tab06_A6.Rows[pRow].FindControl("Tab06_A6_LS");
        System.Web.UI.WebControls.TextBox txt3 = (System.Web.UI.WebControls.TextBox)Tab06_A6.Rows[pRow].FindControl("Tab06_A6_OS");
        System.Web.UI.WebControls.TextBox txt4 = (System.Web.UI.WebControls.TextBox)Tab06_A6.Rows[pRow].FindControl("Tab06_A6_RS");
        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'" + txt4.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab06_A6_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab06_A6 = "SELECT DocEntry, Text1 AS LS, Text2 as OS, Text3 AS RS,'' AS 'Delete' FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ",6.62,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab06_A6;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_06_03 = mSQL.ExecuteReader();

        Tab06_A6.DataSource = mSQLDr_06_03;
        Tab06_A6.DataBind();

        mSQLDr_06_03.Close();
    }
    #endregion
    #region "TAB 06 - B6"

    private void Tab06_B6_creation(SqlConnection mConn)
    {
        Tab06_B6_DataBind(mConn);
    }

    public void Tab06_B6_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab06_B6_03")
        {
            Tab06_B6_Save(mConn, -1);
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab06_B6.Rows[mRow].FindControl("Tab06_B6_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            Tab06_B6_DataBind(mConn);
        }
        else
        {
            Tab06_B6_Save(mConn, mRow);
        }

        mConn.Close();
    }

    protected void Tab06_B6_AddBtn_Click(object sender, EventArgs e)
    {
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Create a blank entry in database
        String mSQL_AddRow = "";
        mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",6.63,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSQL_AddRow, mConn);
        mSQL.ExecuteNonQuery();
        //Save current value in list to database
        Tab06_B6_Save(mConn, -1);
        //Rebind the updated datasource
        Tab06_B6_DataBind(mConn);
        mConn.Close();
    }

    private void Tab06_B6_Save(SqlConnection mConn, int pRow)
    {
        if (pRow == -1)
        {
            for (int i = 0; i <= Tab06_B6.Rows.Count - 1; i++)
            {
                Tab06_B6_DBUpdate(mConn, i);
            }
        }
        else
        {
            Tab06_B6_DBUpdate(mConn, pRow);
        }

    }

    private void Tab06_B6_DBUpdate(SqlConnection mConn, int pRow)
    {
        System.Web.UI.WebControls.TextBox txt1 = (System.Web.UI.WebControls.TextBox)Tab06_B6.Rows[pRow].FindControl("Tab06_B6_DocEntry");
        System.Web.UI.WebControls.TextBox txt2 = (System.Web.UI.WebControls.TextBox)Tab06_B6.Rows[pRow].FindControl("Tab06_B6_SP");
        System.Web.UI.WebControls.TextBox txt3 = (System.Web.UI.WebControls.TextBox)Tab06_B6.Rows[pRow].FindControl("Tab06_B6_PS");
        String mSaveEntry = "EXEC sp_MAI01_Save 'U', "
            + txt1.Text + ",0,0,"
            + "'" + txt2.Text.Replace("'", "''") + "',"
            + "'" + txt3.Text.Replace("'", "''") + "',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "'',"
            + "''";
        SqlCommand mSQL;
        mSQL = new SqlCommand(mSaveEntry, mConn);
        mSQL.ExecuteNonQuery();
    }

    private void Tab06_B6_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab06_B6 = "SELECT DocEntry, Text1 AS SP, Text2 as PS,'' AS 'Delete' FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ",6.63,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab06_B6;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_06_03 = mSQL.ExecuteReader();

        Tab06_B6.DataSource = mSQLDr_06_03;
        Tab06_B6.DataBind();

        mSQLDr_06_03.Close();

        String mSQL_Tab06_B6_Hdr;
        mSQL_Tab06_B6_Hdr = "SELECT ISNULL(Alias1,'') FROM CPSPMRH WHERE BaseEntry = " + txtDocEntry.Text + " AND SubEntry = '6.63' ";
        SqlCommand mSQL_Hdr;
        mSQL_Hdr = new SqlCommand();
        mSQL_Hdr.CommandText = mSQL_Tab06_B6_Hdr;
        mSQL_Hdr.CommandType = CommandType.Text;
        mSQL_Hdr.Connection = mConn;
        SqlDataReader mSQLDr_06_B6_Hdrs = mSQL_Hdr.ExecuteReader();
        while (mSQLDr_06_B6_Hdrs.Read())
        {
            Tab06_B6.Columns[1].HeaderText = (String)mSQLDr_06_B6_Hdrs[0];
        }
        mSQLDr_06_B6_Hdrs.Close();
    }
    #endregion  

    #region "Tab07_01"
    void tab07_content(SqlConnection mConn)
    {
        try
        {
            tab07_01_creation(mConn);
        }
        catch (Exception ex)
        {
            PCCore.Common.HRLog.RecordException("7", ex);
        }
    }

    public void Tab07_01_RowCommand(Object Sender, GridViewCommandEventArgs e)
    {
        //Retrieve Current Row
        int mRow = Convert.ToInt32(e.CommandArgument);
        //Create SQL Connection
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        //Save Current Image

        if (e.CommandName == "Tab07_01_03")
        {
            //Retrieve Docentry in the specific row
            System.Web.UI.WebControls.TextBox txt1;
            txt1 = (System.Web.UI.WebControls.TextBox)Tab07_01.Rows[mRow].FindControl("Tab07_01_DocEntry");
            //Delete the selected row
            String mCreateEntry = "EXEC sp_MAI01_Save 'D', " + txt1.Text + ",0,0,'','','','','','','','','',''";
            SqlCommand mSQL;
            mSQL = new SqlCommand(mCreateEntry, mConn);
            mSQL.ExecuteNonQuery();
            //Rebind the updated datasource
            //Tab07_01_DataBind(mConn);
        }
        Tab07_01_DataBind(mConn);
        mConn.Close();

        
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {

        if (FileUpload.HasFile)
        {
            SqlConnection mConn = SqlConnect();
            String mPrjCode = "";
            String mPeriod = "";
            String FileFullName = Server.HtmlEncode(FileUpload.FileName);
            String extension = System.IO.Path.GetExtension(FileFullName);
            String mTitleContent = "";
            mTitleContent += "SELECT LEFT(DocNum,Len(DocNum) - 4),  CAST(Right(DocNum,3) AS NVARCHAR(50)) ";
            mTitleContent += "FROM DocumentProperty ";
            mTitleContent += "WHERE ID = " + txtDocEntry.Text;

            mConn.Open();
            SqlCommand mSQL;
            mSQL = new SqlCommand();
            mSQL.CommandText = mTitleContent;
            mSQL.CommandType = CommandType.Text;
            mSQL.Connection = mConn;
            SqlDataReader mdr = mSQL.ExecuteReader();
            if (mdr.HasRows == true)
            {
                while (mdr.Read())
                {
                    mPrjCode = (String)mdr[0];
                    mPeriod = (String)mdr[1];

                }
            }
            mdr.Close();
            //String mPath = HttpContext.Current.Request.MapPath("~/"); 
            
            String mPath_webpath = (String)ConfigurationManager.AppSettings["PMDirectory"] + "/";
            String mPath = (String)ConfigurationManager.AppSettings["PMDirectory_physical"] + "\\";
            String mFileName = mPrjCode + "_" + mPeriod + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;

            FileUpload.SaveAs(mPath + mFileName);

            //Create a blank entry in database
            String mSQL_AddRow = "";
            mSQL_AddRow += "EXEC sp_MAI01_Save 'A'," + txtDocEntry.Text + ",7.1,0,'" + mPath + mFileName + "','" + mPath_webpath + mFileName + "','" + mFileName + "','','','','','','',''";
            mSQL = new SqlCommand(mSQL_AddRow, mConn);
            mSQL.ExecuteNonQuery(); 
            //Rebind the updated datasource
            Tab07_01_DataBind(mConn);
            mConn.Close();  
        }
    }
    
    private void tab07_01_creation(SqlConnection mConn)
    {
        Tab07_01_DataBind(mConn);
    }

    private void Tab07_01_DataBind(SqlConnection mConn)
    {
        //Use SQLCommand to insert value from SQL to GridView Tab01_01
        String mSQL_Tab07_01 = "SELECT DocEntry, Text2 AS FileName, '' AS 'Delete' FROM CPS_Function_MAI01_01_Detail(" + txtDocEntry.Text + ",7.1,'T')";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_Tab07_01;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mSQLDr_07_01 = mSQL.ExecuteReader();

        Tab07_01.DataSource = mSQLDr_07_01;
        Tab07_01.DataBind();
        mSQLDr_07_01.Close();
    }
    #endregion

    #region "Panel Display"
    void paneldisplay(int tabnum)
    {
        String TabName = "";
        if (tabnum == 1)
        {
            Folder01.Visible = true;
            TabName = tab01_Visbtn.Text;
        }
        else
            Folder01.Visible = false;

        if (tabnum == 2)
        {
            Folder02.Visible = true;
            TabName = tab02_Visbtn.Text;
        }
        else
            Folder02.Visible = false;

        if (tabnum == 3)
        {
            Folder03.Visible = true;
            TabName = tab03_Visbtn.Text;
        }
        else
            Folder03.Visible = false;

        if (tabnum == 4)
        {
            Folder04.Visible = true;
            TabName = tab04_Visbtn.Text;
        }
        else
            Folder04.Visible = false;

        if (tabnum == 5)
        {
            Folder05.Visible = true;
            TabName = tab05_Visbtn.Text;
        }
        else
            Folder05.Visible = false;

        if (tabnum == 6)
        {
            Folder06.Visible = true;
            TabName = tab06_Visbtn.Text;
        }
        else
            Folder06.Visible = false;

        if (tabnum == 7)
        {
            Folder07.Visible = true;
            TabName = tab07_Visbtn.Text;
        }
        else
            Folder07.Visible = false;
        
        txtCurrentTab.Text = (String)Convert.ToString(tabnum);
        Page.Title = txtProject.Text + " (" + TabName + ")";
    }

    protected void formsave()
    {
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        Tab01_01_Save(mConn, -1);
        Tab02_01_Save(mConn, -1);
        Tab02_02_DBUpdate(mConn, -1);
        Tab02_03_Save(mConn, -1);
        Tab02_04_Save(mConn, -1);
        Tab02_05_Save(mConn, -1);
        Tab02_06_Save(mConn, -1);
        Tab03_01_Save(mConn, -1);
        Tab03_02_Save(mConn, -1);
        Tab03_03_Save(mConn, -1);
        Tab03_04_Save(mConn, -1);
        Tab03_05_Save(mConn, -1);
        Tab03_06_Save(mConn, -1);
        Tab03_A2_Save(mConn, -1);
        Tab03_04_02_Save(mConn, -1);
        Tab03_05_02_Save(mConn, -1);
        Tab03_06_02_Save(mConn, -1);
        Tab04_01_Save(mConn, -1);
        Tab04_02_Save(mConn, -1);
        Tab04_02_02_Save(mConn, -1);
        Tab05_01_Save(mConn, -1);
        Tab05_02_Save(mConn, -1);
        Tab05_02_02_Save(mConn, -1);
        Tab06_01_Save(mConn, -1);

        Tab06_02_Save(mConn, -1);
        Tab06_03_Save(mConn, -1);
        Tab06_04_Save(mConn, -1);
        Tab06_05_Save(mConn, -1);
        Tab06_06_Save(mConn, -1);
        Tab06_A3_Save(mConn, -1);
        Tab06_B3_Save(mConn, -1);
        Tab06_A4_Save(mConn, -1);
        Tab06_B4_Save(mConn, -1);
        Tab06_A5_Save(mConn, -1);
        Tab06_B5_Save(mConn, -1);
        Tab06_A6_Save(mConn, -1);
        Tab06_B6_Save(mConn, -1);

        mConn.Close();
    }

    protected void tab01_Visbtn_Click(object sender, EventArgs e)
    {
        paneldisplay(1);
    }

    protected void tab02_Visbtn_Click(object sender, EventArgs e)
    {
        paneldisplay(2);
    }
    protected void tab03_Visbtn_Click(object sender, EventArgs e)
    {
        paneldisplay(3);
    }
    protected void tab04_Visbtn_Click(object sender, EventArgs e)
    {
        paneldisplay(4);
    }
    protected void tab05_Visbtn_Click(object sender, EventArgs e)
    {
        paneldisplay(5);
    }
    protected void tab06_Visbtn_Click(object sender, EventArgs e)
    {
        paneldisplay(6);
    }
    protected void tab07_Visbtn_Click(object sender, EventArgs e)
    {
        paneldisplay(7);
    }

 
    #endregion
    #region "Common Func"
    SqlConnection SqlConnect()
    {
        SqlConnection mConn;
        mConn = new SqlConnection();
        mConn.ConnectionString = PCCore.Config.DefaultConnectionString;
        return mConn;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        // Validate Integer or Decimal Fields
        
        formsave();
        Response.Write("<Script language='javascript'>alert('Document Saved'); window.close();</script>");
    }
    protected void btnPost_Click(object sender, EventArgs e)
    {
        SqlConnection mConn = SqlConnect();
        mConn.Open();
        String mSQL_PendPrint;
        mSQL_PendPrint = "EXEC sp_MAI01_Save 'PPPS'," + txtDocEntry.Text + ",0,0,'','','','','','','','','',''";
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mSQL_PendPrint;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        mSQL.ExecuteNonQuery();
        mConn.Close();
        //Response.Write("<Script Language='Javascript'>href='MAI01_01.aspx?DocEntry=" + txtDocEntry.Text + "';</Script>");
        Response.Write("<Script language='javascript'>alert('Document Posted'); window.close();</script>");
    }

    protected void titleload(SqlConnection mConn)
    {
        String mPrjCode = "";
        String  mPeriod = "";
        String mTitleContent = "";
        mTitleContent += "SELECT LEFT(DocNum,Len(DocNum) - 4),  CAST(ISNULL(CAST(Right(DocNum,3) AS NUMERIC(13,0)),1) AS NVARCHAR(50)), CASE WHEN DocStatus = 'PPPS' THEN 'Pending to Print' ELSE '' END ";
        mTitleContent += "FROM DocumentProperty ";
        mTitleContent += "WHERE ID = " + txtDocEntry.Text;

        PCCore.Common.HRLog.RecordLog(mTitleContent);
        SqlCommand mSQL;
        mSQL = new SqlCommand();
        mSQL.CommandText = mTitleContent;
        mSQL.CommandType = CommandType.Text;
        mSQL.Connection = mConn;
        SqlDataReader mdr = mSQL.ExecuteReader();
        if (mdr.HasRows == true)
        {
            while (mdr.Read() )
            {
                try
                {
                    mPrjCode = (String)mdr[0];
                    mPeriod = (String)mdr[1];
                    txtProjectStatus.Text = (String)mdr[2];
                }
                catch (Exception ex)
                {
                    PCCore.Common.HRLog.RecordException("", ex);
                }
            }
        }
        mdr.Close();

        if (mPrjCode != "")
        {
            String mProjectInfo = "";
            mProjectInfo = "SELECT 'Project Code: ' + PrjCode + ' (Period:' + Cast(Period AS NVARCHAR(50)) + ')','Project Name: ' + PrjFulName FROM CPS_Function_ProjectInfo('" + mPrjCode + "','" + mPeriod + "')";
            PCCore.Common.HRLog.RecordLog(mProjectInfo);
            mSQL = new SqlCommand();
            mSQL.CommandText = mProjectInfo;
            mSQL.CommandType = CommandType.Text;
            mSQL.Connection = mConn;
            mdr = mSQL.ExecuteReader();
            if (mdr.HasRows == true)
            {
                while (mdr.Read())
                {
                    try
                    {
                        txtProject.Text = (String)mdr[0];
                        txtProjectName.Text = (String)mdr[1];
                    }
                    catch (Exception ex)
                    {
                        PCCore.Common.HRLog.RecordException("", ex);
                    }
                }
            }
            mdr.Close();
        }
    }
        
    #endregion

    #region "Download"
    private void DownloadFile(string fname, bool forceDownload)
    {
        string path = MapPath(fname);
        string name = System.IO.Path.GetFileName(path);
        string ext = System.IO.Path.GetExtension(path);
        string type = "";
        // set known types based on file extension  
        if (ext != null)
        {
            switch (ext.ToLower())
            {
                case ".htm":
                case ".html":
                    type = "text/HTML";
                    break;

                case ".txt":
                    type = "text/plain";
                    break;

                case ".doc":
                case ".rtf":
                    type = "Application/msword";
                    break;
            }
        }
        if (forceDownload)
        {
            Response.AppendHeader("content-disposition",
                "attachment; filename=" + name);
        }
        if (type != "")
            Response.ContentType = type;
        Response.WriteFile(path);
        Response.End();
    }
    #endregion

    private Boolean isDecimal(PCCore.TextBox txt)
    {
        string s = txt.Text;
        string chkValue = "";
        Boolean isNegativeValue = false;
        if (s.Length > 0)
        {
            
            if (s.Substring(0, 1) == "(")
            {
                if (s.IndexOf(")") == s.Length)
                {
                    isNegativeValue = true;
                }
                else
                {
                    return false;
                }
                
            }
            if (isNegativeValue)

                chkValue = s.Substring(1, s.Length - 2);

            else
                chkValue = s;
            try
            {
                decimal d = Convert.ToDecimal(s);
            }
            catch (Exception ex)
            {
                // Fail
                return false;
            }
            return true;
        }
        else
        {
            txt.Text = "0";
            return true;    
        }

    }
}

