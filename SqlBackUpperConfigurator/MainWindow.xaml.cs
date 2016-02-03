using SqlBackUpperLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SBUConfigurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Connection> connections;
        ObservableCollection<ServerType> serverTypes;

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //
            var config = App.CurrentConfig;
            gBase.DataContext = config;

            this.connections = new ObservableCollection<Connection>(config.Connections);
            this.serverTypes = new ObservableCollection<ServerType>(config.ServerTypes);

            dgConnections.ItemsSource = connections;
            dgServerTypes.ItemsSource = serverTypes;

        }

        /// <summary>
        /// 
        /// </summary>
        void OnCommadExecute(object sender, ExecutedRoutedEventArgs e)
        {
            var command = e.Command;
            if (AppCommands.ApplyCommand.Equals(command))
            {
                Apply();
            }
            else if (AppCommands.CancelCommand.Equals(command))
            {
                Cancel();
            }
            else if (AppCommands.AddConnectCommand.Equals(command))
            {
                AddConnection();
            }
            else if (AppCommands.EditConnectCommand.Equals(command))
            {
                EditConnection();
            }
            else if (AppCommands.DeleteConnectCommand.Equals(command))
            {
                DeleteConnection();
            }
            else if (AppCommands.AddServerTypeCommand.Equals(command))
            {
                AddServerType();
            }
            else if (AppCommands.EditServerTypeCommand.Equals(command))
            {
                EditServerType();
            }
            else if (AppCommands.DeleteServerTypeCommand.Equals(command))
            {
                DeleteServerType();
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        void OnConnectCommadExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (dgConnections != null && dgConnections.SelectedIndex != -1);
        }

        void OnServerTypeCommadExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (dgServerTypes != null && dgServerTypes.SelectedIndex != -1);
        }

        /// <summary>
        /// 
        /// </summary>
        void AddConnection()
        {
            var dataForm = new ConnectionDataForm();
            if (dataForm.ShowModal(this) == ResultCodes.Apply)
            {
                var conn = dataForm.GetEntity();
                Connection.AttachServerTypeObjects(conn, this.serverTypes.ToList());

                this.connections.Add(conn);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void EditConnection()
        {
            var conn = dgConnections.SelectedItem as Connection;

            var dataForm = new ConnectionDataForm();
            if (dataForm.ShowModal(this, conn) == ResultCodes.Apply)
            {
                Connection.AttachServerTypeObjects(conn, this.serverTypes.ToList());
                dgConnections.Items.Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void DeleteConnection()
        {
            if (MessageBoxes.DeleteRequest(this))
            {
                var entity = dgConnections.SelectedItem as Connection;
                this.connections.Remove(entity);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void AddServerType()
        {
            var dataForm = new ServerTypeDataForm();
            if (dataForm.ShowModal(this) == ResultCodes.Apply)
            {
                var type = dataForm.GetEntity();
                this.serverTypes.Add(type);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void EditServerType()
        {
            var type = dgServerTypes.SelectedItem as ServerType;

            var dataForm = new ServerTypeDataForm();
            if (dataForm.ShowModal(this, type) == ResultCodes.Apply)
            {
                //Connection.AttachServerTypeObjects(this.connections.ToList(), this.serverTypes.ToList());
                dgConnections.Items.Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void DeleteServerType()
        {
            if (MessageBoxes.DeleteRequest(this))
            {
                var entity = dgServerTypes.SelectedItem as ServerType;
                this.serverTypes.Remove(entity);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void Apply()
        {
            SqlBackUpperLibConfig config = App.CurrentConfig;

            config.Connections = this.connections.ToList();
            config.ServerTypes = this.serverTypes.ToList();

            //
            config.WriteConfig();

            Close();
        }

        /// <summary>
        /// 
        /// </summary>
        void Cancel()
        {
            Close();
        }
    }
}
