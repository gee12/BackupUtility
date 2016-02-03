using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace SqlBackUpperLib
{
    public class Connection : IRecord<Connection>
    {
        public const int DEF_MAX_BACKUPS = 5;

        //public const string INST_NAME_TAG = "InstName";
        //public const string BASE_NAME_TAG = "BaseName";
        //public const string USER_NAME = "UserName";
        //public const string PASSWORD = "Password";
        //public const string BACKUP_PATH_NAME = "BackupPath";
        //public const string MAX_BACKUPS_TAG = "MaxBackups";
        public const string SERVER_TAG = "Server";
        public const string SERVER_TYPE_ID_TAG = "ServerTypeId";
        public const string DATABASE_TAG = "Database";
        public const string USER_TAG = "User";
        public const string PASSWORD_TAG = "Password";
        public const string BACKUP_PATH_TAG = "BackupPath";
        public const string MAX_BACKUPS_TAG = "MaxBackups";

        private string server;
        private string database;
        private string user;
        private string password;
        private string backupPath;
        private int maxBackups;
        private int serverTypeId;
        private ServerType serverType;
 
        public Connection()
        {
            maxBackups = DEF_MAX_BACKUPS;
            serverTypeId = 0;
            serverType = default(ServerType);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void AttachServerTypeObjects(List<Connection> connections, List<ServerType> serverTypes)
        {
            foreach (var conn in connections)
            {
                foreach (var st in serverTypes)
                {
                    if (conn.serverTypeId == st.Id)
                    {
                        conn.serverType = st;
                    }
                }
            }
        }

        public static void AttachServerTypeObjects(Connection conn, List<ServerType> serverTypes)
        {
            AttachServerTypeObjects(new List<Connection>() { conn }, serverTypes);
        }

        public override string GetReadableTypeName()
        {
            return "Соединение";
        }

        /// <summary>
        /// 
        /// </summary>
        public string Server
        {
            get
            {
                return server;
            }
            set
            {
                SendPropertyChanging();
                server = value;
                SendPropertyChanged("Server");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ServerType ServerType
        {
            get
            {
                return serverType;
            }
            set
            {
                SendPropertyChanging();
                serverType = value;
                SendPropertyChanged("ServerType");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Database
        {
            get
            {
                return database;
            }
            set
            {
                SendPropertyChanging();
                database = value;
                SendPropertyChanged("Database");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string User
        {
            get
            {
                return user;
            }
            set
            {
                SendPropertyChanging();
                user = value;
                SendPropertyChanged("User");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                SendPropertyChanging();
                password = value;
                SendPropertyChanged("Password");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string BackupPath
        {
            get
            {
                return backupPath;
            }
            set
            {
                SendPropertyChanging();
                backupPath = value;
                SendPropertyChanged("BackupPath");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MaxBackups
        {
            get
            {
                return maxBackups;
            }
            set
            {
                SendPropertyChanging();
                maxBackups = value;
                SendPropertyChanged("MaxBackups");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ServerTypeId
        {
            get
            {
                return serverTypeId;
            }
            set
            {
                SendPropertyChanging();
                serverTypeId = value;
                SendPropertyChanged("ServerTypeId");
            }
        }
    }
}
