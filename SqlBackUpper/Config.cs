using System;
using System.Configuration;
using System.Reflection;
using BackupLibrary;
using System.Diagnostics;
using System.Collections.Generic;
using System.Xml;

namespace SqlBackUpper
{
    class Config : BackupLibrary.Config
    {
        public const string CONNECTION_ELEMENT_TAG = "Connection";
        public const string BACKUP_NAME_MASK_TAG = "BackupNameMask";
        public const string SQL_QUERY_MASK_TAG = "SqlQueryMask";
        public const string CONNECTION_MASK_TAG = "ConnectionMask";
        public const string TIMEOUT_TAG = "Timeout";
        public const string UNITE_SAME_INST_TAG = "UniteSameInst";

        public string BackupNameMask = "backup_{0:dd-MM-yyyy_HH-mm-ss}.zip";
        public string ConnectionMask = "Data Source={0};Initial Catalog={1};User ID={2};Password={3};"
            + "Integrated Security={4};Persist Security Info={5};Trusted_Connection={6};";
        public string ConnectionGroupMask = "Data Source={0};User ID={1};Password={2};"
            + "Integrated Security={3};Persist Security Info={4};Trusted_Connection={5};";
        public string SqlQueryMask = "BACKUP DATABASE [{0}] TO  DISK = N'{1}\\{2}'"
            + "WITH NOFORMAT, INIT,  NAME = N'{3} - Full backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
        public int Timeout = 30;
        public bool UniteSameInst = false;

        public List<Connection> Connections = new List<Connection>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configFileName"></param>
        public Config(string configFileName)
            : base(configFileName)
        {
            Init(configFileName);
        }

        public Config(string[] args)
            : base(args)
        {
            Init(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        override public bool ReadConfig(string fileName)
        {
            XmlTextReader reader = null;
            try
            {
                reader = new XmlTextReader(fileName);
                reader.WhitespaceHandling = WhitespaceHandling.None;

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name.Equals(CONFIG_ELEMENT_TAG))
                        {
                            while (reader.MoveToNextAttribute())
                            {
                                string name = reader.Name;
                                string value = reader.Value;
                                switch (name)
                                {
                                    case BACKUP_NAME_MASK_TAG: BackupNameMask = value; break;
                                    case SQL_QUERY_MASK_TAG: SqlQueryMask = value; break;
                                    case CONNECTION_MASK_TAG: ConnectionMask = value; break;
                                    case TIMEOUT_TAG: Timeout = Int32.Parse(value); break;
                                    case UNITE_SAME_INST_TAG: UniteSameInst = Boolean.Parse(value); break;
                                }
                                //
                                ReadBaseAttributes(name, value);
                            }
                        }
                        else if (reader.Name.Equals(CONNECTION_ELEMENT_TAG))
                        {
                            Connection conn = new Connection();
                            while (reader.MoveToNextAttribute())
                            {
                                string value = reader.Value;
                                switch (reader.Name)
                                {
                                    case Connection.INST_NAME_TAG: conn.InstName = value; break;
                                    case Connection.BASE_NAME_TAG: conn.BaseName = value; break;
                                    case Connection.USER_NAME: conn.UserName = value; break;
                                    case Connection.PASSWORD: conn.Password = value; break;
                                    case Connection.BACKUP_PATH_NAME: conn.BackupPath = value; break;
                                    case Connection.MAX_BACKUPS_TAG: conn.MaxBackups = Int32.Parse(value); break;
                                }
                            }
                            Connections.Add(conn);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Add(ex.Message);
                return false;
            }
            return true;
        }
    }
}
