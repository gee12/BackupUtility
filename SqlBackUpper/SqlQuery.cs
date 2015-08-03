using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using BackupLibrary;

namespace SqlBackUpper
{
    class SqlQuery
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="backupNameMask"></param>
        /// <returns></returns>
        public static void Execute(Connection connElem, string connectionMask, int timeout, string sqlQueryMask, string backupName)
        {
            if (connElem == null) return;

            FileSystem.CreateIfMissing(connElem.BackupPath);

            SqlConnection conn = new SqlConnection(String.Format(connectionMask,
                connElem.InstName, connElem.BaseName, connElem.UserName, connElem.Password, "False", "False", "True"));

            string backupQueryText = String.Format(sqlQueryMask, connElem.BaseName, connElem.BackupPath, backupName, connElem.BaseName);

            // connecting to server
            try
            {
                Log.Add("Connection to server [{0}]...", connElem.InstName);
                conn.Open();
                Log.Add("Connection was successfull", connElem.InstName);
                // executing sql query to database
                Log.Add("Backup from [{0}] to [{1}]...", connElem.BaseName, backupName);
                SqlCommand sqlCommand = new SqlCommand(backupQueryText, conn);
                sqlCommand.Prepare();
                sqlCommand.ExecuteNonQuery();
                sqlCommand.CommandTimeout = timeout;
                Log.Add("Query executed. Result code: {0}", sqlCommand.UpdatedRowSource);
            }
            catch (SqlException ex)
            {
                Log.Add("Error connection to server. Code: [{0}], Message: [{1}]", ex.ErrorCode, ex.Message);
            }
            finally
            {
                conn.Close();
                Log.Add("Connection closed");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="connGroupMask"></param>
        /// <param name="timeout"></param>
        /// <param name="sqlQueryMask"></param>
        /// <param name="backupName"></param>
        public static void ExecuteGroup(ConnectionGroup group, string connGroupMask, int timeout, string sqlQueryMask, string backupNameMask)
        {
            if (group == null) return;

            SqlConnection conn = new SqlConnection(String.Format(connGroupMask,
                group.InstName, group.UserName, group.Password, "False", "False", "True"));

            // connecting to group server
            try
            {
                Log.Add("Connection to server [{0}]...", group.InstName);
                conn.Open();
                Log.Add("Connection was successfull", group.InstName);

                foreach (Connection connElem in group.Connections)
                {
                    FileSystem.CreateIfMissing(connElem.BackupPath);

                    string backupName = connElem.BaseName + String.Format(backupNameMask, DateTime.Now);
                    string backupQueryText = String.Format(sqlQueryMask, connElem.BaseName, connElem.BackupPath, backupName, connElem.BaseName);
                    // executing sql query to database
                    try
                    {
                        Log.Add("Backup from [{0}] to [{1}]...", connElem.BaseName, backupName);
                        SqlCommand sqlCommand = new SqlCommand(backupQueryText, conn);
                        sqlCommand.Prepare();
                        sqlCommand.ExecuteNonQuery();
                        sqlCommand.CommandTimeout = timeout;
                        Log.Add("Query executed. Result code: {0}", sqlCommand.UpdatedRowSource);
                    }
                    catch(SqlException ex)
                    {
                        Log.Add("Error with query executing. Code: [{0}], Message: [{1}]", ex.ErrorCode, ex.Message);
                    }
                }
            }
            catch (SqlException ex)
            {
                Log.Add("Error with connection to server. Code: [{0}], Message: [{1}]", ex.ErrorCode, ex.Message);
            }
            finally
            {
                conn.Close();
                Log.Add("Connection closed");
            }
        }
    }
}
