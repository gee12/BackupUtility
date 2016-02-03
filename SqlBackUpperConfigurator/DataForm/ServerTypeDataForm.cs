using BackupLibrary;
using SqlBackUpperLib;
using System.Windows;
using System.Windows.Input;

namespace SBUConfigurator
{
    public class ServerTypeDataForm : BaseDataForm<ServerType, ServerTypeDataControl>
    {
        public ServerTypeDataForm()
        {
            Init();
        }

        private void Init()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void SetControls(ServerType entity)
        {
            ContentControls.tbName.Focus();
        }

        public override bool IsValid()
        {
            return !(string.IsNullOrWhiteSpace(ContentControls.tbName.Value)
                || string.IsNullOrWhiteSpace(ContentControls.tbConnectionStringMask.Value)
                || string.IsNullOrWhiteSpace(ContentControls.tbSqlQueryMask.Value));
        }

    }
}
