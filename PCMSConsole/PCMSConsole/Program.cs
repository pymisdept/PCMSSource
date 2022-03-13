using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace PCMSConsole
{
    class Program
    {
        const string PCMS_FE_SERVER = "10.1.1.125";
        const string PCMS_BE_SERVER = "10.1.1.126";
        const string PMIS_SERVER = @"10.1.1.118\EISDB";
        const string FLEX_SERVER = "10.1.1.101";

        static void Main(string[] args)
        {
            checkPMISRejectStatus();
            checkPCMSRejectStatus();
            checkPCMSReverseStatus();
        }

        static void checkPMISRejectStatus()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = string.Format("Server={0};Database={1};User Id={2};Password={3};", PMIS_SERVER, "SPMMLPYE", "sa", "Spm01dbsa");

            string queryString = "select cmdocument, ISNULL(quantity2, 0) as quantity2 from cmdocument where cmdocumentdefn = 543 and project = 52 and flag5 = 0 and cmdocumentstatus = 58";

            SqlCommand command = new SqlCommand(queryString, conn); 
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<int> _pmisIDList = new List<int>();
            List<int> _pcmsIDList = new List<int>();

            while (reader.Read())
            {
                var thisPMISID = reader.GetInt32(0);
                var thisPCMSID = (int)reader.GetDecimal(1);
               
                if (thisPCMSID > 0)
                    _pcmsIDList.Add(thisPCMSID);

                _pmisIDList.Add(thisPMISID);
            }

            reader.Close();
            conn.Close();

            if(_pcmsIDList.Count > 0)
            {
                updatePCMSStatus(string.Join(",", _pcmsIDList));
                updatePCMSStatus_2(string.Join(",", _pcmsIDList));
            }

            updatePMISFlag(_pmisIDList);
        }

        static void checkPCMSRejectStatus()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = string.Format("Server={0};Database={1};User Id={2};Password={3};", PMIS_SERVER, "SPMMLPYE", "sa", "Spm01dbsa");

            string queryString = "select ISNULL(cmdocument,0) from cmdocument d, PCMS_FE.PCMS800.dbo.DocumentProperty p where d.cmdocumentdefn = 543 and d.project = 52 and d.cmdocumentstatus <> 58 and d.quantity2 = p.ID and p.DocStatus = 'PRBD'";
            
            SqlCommand command = new SqlCommand(queryString, conn);
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<int> _pmisIDList = new List<int>();

            while (reader.Read())
            {
                var thisPMISID = reader.GetInt32(0);

                _pmisIDList.Add(thisPMISID);
            }

            reader.Close();
            conn.Close();

            updatePMISStatus(_pmisIDList, 58);
        }

        static void updatePMISFlag(List<int> PMISID)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = string.Format("Server={0};Database={1};User Id={2};Password={3};", PMIS_SERVER, "SPMMLPYE", "sa", "Spm01dbsa");

            string updateString = "update cmdocument set flag5 = 1 where cmdocumentdefn = 543 and project = 52 and flag5 = 0 and cmdocument = @id";

            conn.Open();

            for (int i = 0; i < PMISID.Count; i++)
            {

                SqlCommand command = new SqlCommand(updateString, conn);
                SqlParameter p = new SqlParameter("@id", SqlDbType.Int);
                p.Value = PMISID[i];
                command.Parameters.Add(p);
                command.ExecuteNonQuery();
            }

            conn.Close();
        }

        static void updatePMISStatus(List<int> PMISID, int status)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = string.Format("Server={0};Database={1};User Id={2};Password={3};", PMIS_SERVER, "SPMMLPYE", "sa", "Spm01dbsa");

            string updateString = "update cmdocument set cmdocumentstatus = @status where cmdocumentdefn = 543 and project = 52 and cmdocument = @id";

            conn.Open();

            for (int i = 0; i < PMISID.Count; i++)
            {

                SqlCommand command = new SqlCommand(updateString, conn);
                SqlParameter p = new SqlParameter("@id", SqlDbType.Int);
                SqlParameter p2 = new SqlParameter("@status", SqlDbType.Int);
                p.Value = PMISID[i];
                p2.Value = status;
                command.Parameters.Add(p);
                command.Parameters.Add(p2);
                command.ExecuteNonQuery();
            }

            conn.Close();
        }

        static void updatePCMSStatus(string PCMSID)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = string.Format("Server={0};Database={1};User Id={2};Password={3};", PCMS_FE_SERVER, "PCMS800", "sa", "compass2009");

            string updateString = "update DocumentProperty set DocStatus = 'PRDR' where Type = 1012 and ID in ({0})";
            updateString = string.Format(updateString, PCMSID);
            SqlCommand command = new SqlCommand(updateString, conn);

            conn.Open();

            command.ExecuteNonQuery();

            conn.Close();
        }

        static void updatePCMSStatus_2(string PCMSID)
        {
            SqlConnection conn2 = new SqlConnection();
            conn2.ConnectionString = string.Format("Server={0};Database={1};User Id={2};Password={3};", PCMS_BE_SERVER, "PAY800", "sa", "compass2008");

            string updateString2 = "INSERT INTO PCMSConsole_Reject (PCMSConsole_IDList) VALUES (@id)";

            SqlCommand command2 = new SqlCommand(updateString2, conn2);
            SqlParameter p = new SqlParameter("@id", SqlDbType.NVarChar);
            p.Value = PCMSID;
            command2.Parameters.Add(p);

            conn2.Open();

            command2.ExecuteNonQuery();

            conn2.Close();
        }

        static void checkPCMSReverseStatus()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = string.Format("Server={0};Database={1};User Id={2};Password={3};", PMIS_SERVER, "SPMMLPYE", "sa", "Spm01dbsa");

            string queryString = "select ISNULL(cmdocument,0) from cmdocument d, FE_PCMS.PCMS800.dbo.DocumentProperty p where d.cmdocumentdefn = 543 and d.project = 52 and d.cmdocumentstatus <> 144 and d.quantity2 = p.ID and p.DocStatus = 'R'";

            SqlCommand command = new SqlCommand(queryString, conn);
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<int> _pmisIDList = new List<int>();

            while (reader.Read())
            {
                var thisPMISID = reader.GetInt32(0);

                _pmisIDList.Add(thisPMISID); 
            }

            reader.Close();
            conn.Close();

            updatePMISStatus(_pmisIDList, 144); // 141 ==> production
        }
    }
}
