using System.Windows.Controls;
using System.Windows.Input;

namespace SBUConfigurator
{
    /// <summary>
    /// Interaction logic for ServerTypeDataControl.xaml
    /// </summary>
    public partial class ServerTypeDataControl : UserControl
    {
        public ServerTypeDataControl()
        {
            InitializeComponent();

            //AppCommands.SetCommandBinding(CommandBindings, AppCommands.ShowOnMapCommand, OnExecuteCommand, CanExecuteCommand);
        }

        //private void OnExecuteCommand(object sender, ExecutedRoutedEventArgs e)
        //{
        //    string addr = tbName.Value;
        //    string url = string.Format(Config.AppConf.MapUrlMask, addr);
        //    System.Diagnostics.Process.Start(url);
        //}

        //private void CanExecuteCommand(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    e.CanExecute = (!string.IsNullOrWhiteSpace(tbName.Value));
        //}
    }
}
