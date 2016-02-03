using BackupLibrary;
using SqlBackUpperLib;
using System.Windows;
using System.Windows.Input;

namespace SBUConfigurator
{
    public class ConnectionDataForm : BaseDataForm<Connection, ConnectionDataControl>
    {
        public ConnectionDataForm()
        {
            Init();
        }

        private void Init()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void SetControls(Connection entity)
        {
            ContentControls.cbServerType.ItemsSource = App.CurrentConfig.ServerTypes;
            ContentControls.tbServer.Focus();
        }

        public override bool IsValid()
        {
            return !(string.IsNullOrWhiteSpace(ContentControls.tbServer.Value)
                || string.IsNullOrWhiteSpace(ContentControls.tbDatabase.Value)
                || string.IsNullOrWhiteSpace(ContentControls.tbBackupPath.Value));
        }

    }
}
