using BackupLibrary;
using SqlBackUpperLib;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            AppCommands.SetCommandBinding(CommandBindings, AppCommands.TestCommand, OnCommadExecute, OnConnectCommadExecute);
            ContentControls.bBackupPathShowDialog.Click += bBackupPathShowDialog;
            ContentControls.tbMaxBackups.PreviewTextInput += Utils.OnUnsignedIntPreviewTextInput;
        }

        /// <summary>
        /// 
        /// </summary>
        void OnCommadExecute(object sender, ExecutedRoutedEventArgs e)
        {
            var command = e.Command;
            if (AppCommands.TestCommand.Equals(command))
            {
                Test();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void OnConnectCommadExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsServerValid();
        }

        /// <summary>
        /// 
        /// </summary>
        void Test()
        {
            ServerType st = ContentControls.cbServerType.SelectedItem as ServerType;
            if (st == null) return;

            string connString = string.Format(st.ConnectionStringMask,
                ContentControls.tbServer.Value, ContentControls.tbDatabase.Value, ContentControls.tbUser.Value, ContentControls.tbPassword.Value, 
                "False", "False", "True");

            ContentControls.gMain.IsEnabled = false;

            try
            {
                SqlConnection conn = new SqlConnection(connString);
                conn.Open();
                var state = conn.State;
                conn.Close();

                bool res = (state == System.Data.ConnectionState.Open);
                MessageBoxes.ServerTest(this, ContentControls.tbServer.Value, res);
            }
            catch (SqlException sqlex)
            {
                MessageBoxes.Error(this, "Невозможно соединиться с сервером:\n" + sqlex.Message);
            }
            catch (Exception ex)
            {
                MessageBoxes.Error(this, "Ошибка в строке соединения:\n" + ex.Message);
            }

            ContentControls.gMain.IsEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        void bBackupPathShowDialog(object sender, RoutedEventArgs e)
        {
            var fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.SelectedPath = ContentControls.tbBackupPath.Value;
            fbd.ShowNewFolderButton = true;
            fbd.Description = "Выберите каталог";

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                ContentControls.tbBackupPath.Value = fbd.SelectedPath;
        }

        public short ShowModal(Window owner, List<ServerType> serverTypes)
        {
            ContentControls.cbServerType.ItemsSource = serverTypes;
            return ShowModal(owner);
        }

        public short ShowModal(Window owner, Connection conn, List<ServerType> serverTypes)
        {
            ContentControls.cbServerType.ItemsSource = serverTypes;
            return ShowModal(owner, conn);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void SetControls(Connection entity)
        {
            ContentControls.tbServer.Focus();
        }

        public override bool IsValid()
        {
            return !(!IsServerValid()
                || string.IsNullOrWhiteSpace(ContentControls.tbBackupPath.Value));
        }

        public bool IsServerValid()
        {
            return !(string.IsNullOrWhiteSpace(ContentControls.tbServer.Value)
                || string.IsNullOrWhiteSpace(ContentControls.tbDatabase.Value)
                || ContentControls.cbServerType.InternalComboBox.SelectedIndex == -1);
        }

    }
}
