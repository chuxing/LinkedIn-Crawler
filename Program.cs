using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
namespace SqlServer
{
    public class SqlServerOperation
    {

        public static string connectionString = "Server = cia-sh-03; Database = LinkedIn; Integrated Security = True;";
        private static SqlConnection objSqlConn = null;
        //connection sqlserver
        private static void GetConnection()
        {
            try
            {
                if (objSqlConn == null)
                {
                    objSqlConn = new SqlConnection(connectionString);
                    objSqlConn.Open();
                }
            }
            catch (Exception ex)
            {
                //Output writer = new Output();
                //writer.writetofile(ex);
                throw ex;
            }
        }
        //select operation
        public static DataTable GetDataTable(string strSelectSql)
        {
            try
            {
                GetConnection();
                DataTable dtTarget = new DataTable();
                using (SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter(strSelectSql, objSqlConn))
                {
                    objSqlDataAdapter.Fill(dtTarget);
                }
                return dtTarget;
            }
            catch (Exception ex)
            {
                //Output writer = new Output();
                //writer.writetofile(ex);
                throw ex;
            }
            finally
            {
                if (objSqlConn != null)
                {
                    objSqlConn.Close();
                    objSqlConn.Dispose();
                    objSqlConn = null;
                }
            }
        }
        public static DataSet GetDataSet(string[] strSelectSqlSet)
        {
            try
            {
                GetConnection();
                DataSet dsTarget = new DataSet();
                if (strSelectSqlSet.Length > 0)
                {
                    for (int i = 0; i < strSelectSqlSet.Length; i++)
                    {
                        dsTarget.Tables.Add(GetDataTable(strSelectSqlSet[i]));
                        dsTarget.Tables[i].TableName = "DT" + i.ToString();
                    }
                }
                return dsTarget;
            }
            catch (Exception ex)
            {
                //Output writer = new Output();
                //writer.writetofile(ex);
                throw ex;
            }
            finally
            {
                if (objSqlConn != null)
                {
                    objSqlConn.Close();
                    objSqlConn.Dispose();
                    objSqlConn = null;
                }
            }
        }
        //update
        public static bool UpdateDataSource(DataTable dtResult, string strDestinationTableName)
        {
            try
            {
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.AppendLine("TRUNCATE TABLE [dbo].[" + strDestinationTableName + "]");
                ExecuteNonQuery(sbSQL.ToString());
                ExecuteSqlBulkCopy(dtResult, strDestinationTableName);
                return true;
            }
            catch (Exception ex)
            {
                //Output writer = new Output();
                //writer.writetofile(ex);
                throw ex;
            }
        }
        //execute
        public static int ExecuteNonQuery(string strSql)
        {
            //Console.WriteLine(strSql);
            try
            {
                GetConnection();
                int intResult = 0;
                using (SqlCommand objSqlCmd = new SqlCommand(strSql, objSqlConn))
                {
                    intResult = objSqlCmd.ExecuteNonQuery();
                }
                return intResult;
            }
            catch (Exception ex)
            {
                //Output writer = new Output();
                //writer.writetofile(ex);
                throw ex;
            }
            finally
            {
                if (objSqlConn != null)
                {
                    objSqlConn.Close();
                    objSqlConn.Dispose();
                    objSqlConn = null;
                }
            }
        }
        //<param name = "dtSource">数据源DataTable</param>
        //<param name = "strDestinationTableName">目标表名称</param>
        public static void ExecuteSqlBulkCopy(DataTable dtSource, string strDestinationTableName)
        {
            try
            {
                GetConnection();
                using (SqlBulkCopy objSqlBulkCopy = new SqlBulkCopy(objSqlConn))
                {
                    objSqlBulkCopy.DestinationTableName = strDestinationTableName;
                    for (int i = 0; i < dtSource.Columns.Count; i++)
                    {
                        try
                        {
                            objSqlBulkCopy.ColumnMappings.Add(i, i);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            //Output writer = new Output();
                            //writer.writetofile(ex);
                        }

                    }
                    objSqlBulkCopy.WriteToServer(dtSource);
                }
                //return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Output writer = new Output();
                //writer.writetofile("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
                //writer.writetofile(ex);
                //throw ex;
            }
            finally
            {
                if (objSqlConn != null)
                {
                    objSqlConn.Close();
                    objSqlConn.Dispose();
                    objSqlConn = null;
                }
            }
        }
        //store
        public static bool ExecuteStoreProcedure(string strStoreProcName, string[] strParametersName, string[] strParameterValue)
        {
            try
            {
                GetConnection();
                using (SqlCommand objSqlCmd = new SqlCommand(strStoreProcName, objSqlConn))
                {
                    objSqlCmd.CommandType = CommandType.StoredProcedure;
                    int intParaCount = strParametersName.Length;
                    for (int i = 0; i < intParaCount; i++)
                    {
                        objSqlCmd.Parameters.Add(strParametersName[i], SqlDbType.NVarChar);
                        objSqlCmd.Parameters[strParametersName[i]].Value = strParameterValue[i];
                    }
                    objSqlCmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                //Output writer = new Output();
                //writer.writetofile(ex);
                throw ex;
            }
            finally
            {
                if (objSqlConn != null)
                {
                    objSqlConn.Close();
                    objSqlConn.Dispose();
                    objSqlConn = null;
                }
            }
        }
    }
}
