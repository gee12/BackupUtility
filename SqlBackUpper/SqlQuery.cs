using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using BackupLibrary;
using Npgsql;
using SqlBackUpperLib;

namespace SqlBackUpper
{
    class SqlQuery
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Execute(Connection connElem, string connectionMask, int timeout, string sqlQueryMask, string backupName)
        {
            if (connElem == null) return;

            FileSystem.CreateIfMissing(connElem.BackupPath);

            string connString = String.Format(connectionMask,
                connElem.Server, connElem.Database, connElem.User, connElem.Password, "False", "False", "True");
            SqlConnection conn = new SqlConnection(connString);

            string backupQueryText = String.Format(sqlQueryMask, connElem.Database, connElem.BackupPath, backupName, connElem.Database);

            // connecting to server
            try
            {
                Log.Add("Connection to server [{0}]...", connElem.Server);
                conn.Open();
                Log.Add("Connection was successfull", connElem.Server);
                // executing sql query to database
                Log.Add("Backup from [{0}] to [{1}]...", connElem.Database, backupName);
                SqlCommand sqlCommand = new SqlCommand(backupQueryText, conn);
                sqlCommand.CommandTimeout = timeout;
                sqlCommand.Prepare();
                sqlCommand.ExecuteNonQuery();
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
        public static void ExecutePostgreSQL(Connection connElem, string connectionMask, int timeout, string sqlQueryMask, string backupName)
        {
            // !!
            string connString = String.Format(connectionMask,
                connElem.Server, connElem.Database, connElem.User, connElem.Password, "False", "False", "True");
            NpgsqlConnection conn = new NpgsqlConnection(connString);

            // !!
            string backupQueryText = String.Format(sqlQueryMask, connElem.Database, connElem.BackupPath, backupName, connElem.Database);

            try
            {
                Log.Add("Connection to server [{0}]...", connElem.Server);
                conn.Open();
                Log.Add("Connection was successfull", connElem.Server);

                // executing sql query to database
                Log.Add("Backup from [{0}] to [{1}]...", connElem.Database, backupName);

                using (var sqlCommand = new NpgsqlCommand(backupQueryText, conn))
                {
                    sqlCommand.CommandTimeout = timeout;
                    // ?
                    sqlCommand.Prepare();
                    sqlCommand.ExecuteNonQuery();
                    Log.Add("Query executed. Result code: {0}", sqlCommand.UpdatedRowSource);
                }
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
        public static void ExecuteGroup(ConnectionGroup group, string connGroupMask, int timeout, string sqlQueryMask, string backupNameMask)
        {
            if (group == null) return;

            SqlConnection conn = new SqlConnection(String.Format(connGroupMask,
                group.Server, group.User, group.Password, "False", "False", "True"));

            // connecting to group server
            try
            {
                Log.Add("Connection to server [{0}]...", group.Server);
                conn.Open();
                Log.Add("Connection was successfull", group.Server);

                foreach (Connection connElem in group.Connections)
                {
                    FileSystem.CreateIfMissing(connElem.BackupPath);

                    string backupName = connElem.Database + String.Format(backupNameMask, DateTime.Now);
                    string backupQueryText = String.Format(sqlQueryMask, connElem.Database, connElem.BackupPath, backupName, connElem.Database);
                    // executing sql query to database
                    try
                    {
                        Log.Add("Backup from [{0}] to [{1}]...", connElem.Database, backupName);
                        SqlCommand sqlCommand = new SqlCommand(backupQueryText, conn);
                        sqlCommand.CommandTimeout = timeout;
                        sqlCommand.Prepare();
                        sqlCommand.ExecuteNonQuery();
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
