using BackupLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SqlBackUpperLib
{
    public class SqlBackUpperLibConfig : BackupLibrary.Config
    {
        public const string SERVER_TYPE_ELEMENT_TAG = "ServerType";
        public const string CONNECTION_ELEMENT_TAG = "Connection";
        // main
        public const string BACKUP_NAME_MASK_TAG = "BackupNameMask";

        public const string DEF_BACKUP_NAME_MASK = "backup_{0:dd-MM-yyyy_HH-mm-ss}.zip";
        public string ConnectionMask = "Data Source={0};Initial Catalog={1};User ID={2};Password={3};"
            + "Integrated Security={4};Persist Security Info={5};Trusted_Connection={6};";
        public string ConnectionGroupMask = "Data Source={0};User ID={1};Password={2};"
            + "Integrated Security={3};Persist Security Info={4};Trusted_Connection={5};";
        public string SqlQueryMask = "BACKUP DATABASE [{0}] TO  DISK = N'{1}\\{2}'"
            + "WITH NOFORMAT, INIT,  NAME = N'{3} - Full backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
        public int Timeout = 30;
        public bool UniteSameInst = false;

        public string BackupNameMask { get; set; }

        public List<Connection> Connections = new List<Connection>();
        public List<ServerType> ServerTypes = new List<ServerType>();

        /// <summary>
        /// 
        /// </summary>
        //public Config(string configFileFullName)
        //    : base()
        //{
        //    Init(configFileFullName);
        //}

        //public Config(string[] args)
        //    : base()
        //{
        //    Init(args);
        //}

        public SqlBackUpperLibConfig(string assemblyName)
            : base(assemblyName, false)
        {
            Init();
        }

        public SqlBackUpperLibConfig(string assemblyName, string configFileFullName)
            : base(assemblyName, configFileFullName, false)
        {
            //Init(configFileName);
            Init();
        }

        public SqlBackUpperLibConfig(string assemblyName, string[] args)
            : base(assemblyName, args, false)
        {
            //Init(args);
            Init();
        }

        void Init()
        {
            //BackupNameMask = DEF_BACKUP_NAME_MASK;
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool ReadConfig(string fileName)
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
                        // base
                        if (reader.Name.Equals(CONFIG_ELEMENT_TAG))
                        {
                            while (reader.MoveToNextAttribute())
                            {
                                string name = reader.Name;
                                string value = reader.Value;
                                switch (name)
                                {
                                    case BACKUP_NAME_MASK_TAG: BackupNameMask = value; break;
                                }
                                //
                                ReadBaseAttributes(name, value);
                            }
                        }
                        // connection
                        else if (reader.Name.Equals(CONNECTION_ELEMENT_TAG))
                        {
                            Connection conn = new Connection();
                            while (reader.MoveToNextAttribute())
                            {
                                string value = reader.Value;
                                switch (reader.Name)
                                {
                                    case Connection.SERVER_TAG: conn.Server = value; break;
                                    case Connection.SERVER_TYPE_ID_TAG: conn.ServerTypeId = Int32.Parse(value); break;
                                    case Connection.DATABASE_TAG: conn.Database = value; break;
                                    case Connection.USER_TAG: conn.User = value; break;
                                    case Connection.PASSWORD_TAG: conn.Password = value; break;
                                    case Connection.BACKUP_PATH_TAG: conn.BackupPath = value; break;
                                    case Connection.MAX_BACKUPS_TAG: conn.MaxBackups = Int32.Parse(value); break;
                                }
                            }
                            this.Connections.Add(conn);
                        }
                        // server type
                        else if (reader.Name.Equals(SERVER_TYPE_ELEMENT_TAG))
                        {
                            ServerType serverType = new ServerType();
                            while (reader.MoveToNextAttribute())
                            {
                                string value = reader.Value;
                                switch (reader.Name)
                                {
                                    case ServerType.ID_TAG: serverType.Id = Int32.Parse(value); break;
                                    case ServerType.NAME_TAG: serverType.Name = value; break;
                                    case ServerType.CONNECTION_STRING_MASK_TAG: serverType.ConnectionStringMask = value; break;
                                    case ServerType.SQL_QUERY_MASK_TAG: serverType.SqlQueryMask = value; break;
                                    //case ServerType.TIMEOUT_TAG: conn.Timeout = Int32.Parse(value); break;
                                    //case ServerType.UNITE_SAME_SERVER_TAG: conn.UniteSameServer = Boolean.Parse(value); break;
                                }
                            }
                            this.ServerTypes.Add(serverType);
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

        /// <summary>
        /// 
        /// </summary>
        public bool WriteConfig()
        {
            return WriteConfig(this.ConfigFilePath);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool WriteConfig(string fileName)
        {
            XmlTextWriter writer = null;
            try
            {
                writer = new XmlTextWriter(fileName, Encoding.Unicode);
                writer.Formatting = Formatting.Indented;

                writer.WriteStartElement("Config");

                // base
                WriteBaseAttributes(writer);
                writer.WriteAttributeString(BACKUP_NAME_MASK_TAG, BackupNameMask);

                // server types
                writer.WriteStartElement("ServerTypes");
                foreach (var type in this.ServerTypes)
                {
                    writer.WriteStartElement("ServerType");
                    writer.WriteAttributeString(ServerType.ID_TAG, type.Id.ToString());
                    writer.WriteAttributeString(ServerType.NAME_TAG, type.Name);
                    writer.WriteAttributeString(ServerType.CONNECTION_STRING_MASK_TAG, type.ConnectionStringMask);
                    writer.WriteAttributeString(ServerType.SQL_QUERY_MASK_TAG, type.SqlQueryMask);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                // connections
                writer.WriteStartElement("Connections");
                foreach (var conn in this.Connections)
                {
                    writer.WriteStartElement("Connection");
                    writer.WriteAttributeString(Connection.SERVER_TAG, conn.Server);
                    writer.WriteAttributeString(Connection.SERVER_TYPE_ID_TAG, conn.ServerTypeId.ToString());
                    writer.WriteAttributeString(Connection.DATABASE_TAG, conn.Database);
                    writer.WriteAttributeString(Connection.USER_TAG, conn.User);
                    writer.WriteAttributeString(Connection.PASSWORD_TAG, conn.Password);
                    writer.WriteAttributeString(Connection.BACKUP_PATH_TAG, conn.BackupPath);
                    writer.WriteAttributeString(Connection.MAX_BACKUPS_TAG, conn.MaxBackups.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.Flush();
                writer.Close();
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
