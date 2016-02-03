using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace SqlBackUpperLib
{
    public class ServerType : IRecord<ServerType>
    {
        public const string ID_TAG = "Id";
        public const string NAME_TAG = "Name";
        public const string SQL_QUERY_MASK_TAG = "SqlQueryMask";
        public const string CONNECTION_STRING_MASK_TAG = "ConnectionStringMask";
        //public const string CONNECTION_MASK_TAG = "ConnectionMask";
        public const string TIMEOUT_TAG = "Timeout";
        public const string UNITE_SAME_SERVER_TAG = "UniteSameInst";

        private int id;
        private string name;
        private string connectionStringMask;
        private string sqlQueryMask;
        //private int tymeOut;
        //private bool uniteSame;

        public ServerType()
        {

        }

        public override string GetReadableTypeName()
        {
            return "Тип сервера";
        }

        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                SendPropertyChanging();
                id = value;
                SendPropertyChanged("Id");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                SendPropertyChanging();
                name = value;
                SendPropertyChanged("Name");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ConnectionStringMask
        {
            get
            {
                return connectionStringMask;
            }
            set
            {
                SendPropertyChanging();
                connectionStringMask = value;
                SendPropertyChanged("ConnectionString");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SqlQueryMask
        {
            get
            {
                return sqlQueryMask;
            }
            set
            {
                SendPropertyChanging();
                sqlQueryMask = value;
                SendPropertyChanged("SqlQueryMask");
            }
        }

    }
}
