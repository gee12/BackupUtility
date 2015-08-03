using System;
using System.Collections.Generic;
using System.Text;

namespace SqlBackUpper
{
    public class ConnectionGroup
    {
        public List<Connection> Connections;
        public string UserName = "";
        public string Password = "";
        public string InstName = "";

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
            InstName = conn.InstName;
            UserName = conn.UserName;
            Password = conn.Password;
            Connections = new List<Connection>();
            Connections.Add(conn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        public void Add(Connection conn)
        {
            Connections.Add(conn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connections"></param>
        /// <returns></returns>
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
                    if (group.InstName.Equals(conn.InstName))
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
