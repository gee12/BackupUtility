using BackupLibrary;
using SqlBackUpperLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace SqlBackUpper
{
    public class ConnectionGroup
    {
        public List<Connection> Connections;
        public string User = "";
        public string Password = "";
        public string Server = "";

        /// <summary>
        /// 
        /// </summary>
        public ConnectionGroup()
        {
            Connections = new List<Connection>();
        }

        /// <summary>
        /// 
        /// </summary>
        public ConnectionGroup(Connection conn)
        {
            Server = conn.Server;
            User = conn.User;
            Password = conn.Password;
            Connections = new List<Connection>();
            Connections.Add(conn);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Add(Connection conn)
        {
            Connections.Add(conn);
        }

        /// <summary>
        /// 
        /// </summary>
        public static List<ConnectionGroup> SpritToGroups(List<Connection> connections)
        {
            if (connections == null) return null;
            List<ConnectionGroup> groups = new List<ConnectionGroup>();

            foreach(Connection conn in connections)
            {
                bool isUniqueConn = true;
                // find right group
                foreach(ConnectionGroup group in groups)
                {
                    if (group.Server.Equals(conn.Server))
                    {
                        group.Add(conn);
                        isUniqueConn = false;
                        break;
                    }
                }
                // create new group
                if (isUniqueConn)
                {
                    ConnectionGroup group = new ConnectionGroup(conn);
                    groups.Add(group);
                }
            }
            return groups;
        }
    }
}
