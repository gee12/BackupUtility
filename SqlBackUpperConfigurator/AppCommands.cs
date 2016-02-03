using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SBUConfigurator
{
    public class AppCommands
    {
        public static RoutedUICommand ApplyCommand { get; protected set; }
        public static RoutedUICommand CancelCommand { get; protected set; }
        public static RoutedUICommand AddServerTypeCommand { get; protected set; }
        public static RoutedUICommand EditServerTypeCommand { get; protected set; }
        public static RoutedUICommand DeleteServerTypeCommand { get; protected set; }
        public static RoutedUICommand AddConnectCommand { get; protected set; }
        public static RoutedUICommand EditConnectCommand { get; protected set; }
        public static RoutedUICommand DeleteConnectCommand { get; protected set; }
        public static RoutedUICommand CloneCommand { get; protected set; }
        public static RoutedUICommand UpCommand { get; protected set; }
        public static RoutedUICommand DownCommand { get; protected set; }
        public static RoutedUICommand TestCommand { get; protected set; }

        static AppCommands()
        {
            AddServerTypeCommand = new RoutedUICommand("Добавить новую запись", "AddServerTypeCommand", typeof(AppCommands));
            EditServerTypeCommand = new RoutedUICommand("Изменить запись", "EditServerTypeCommand", typeof(AppCommands));
            DeleteServerTypeCommand = new RoutedUICommand("Удалить запись", "DeleteServerTypeCommand", typeof(AppCommands));
            //
            AddConnectCommand = new RoutedUICommand("Добавить новую запись", "AddConnectCommand", typeof(AppCommands));
            EditConnectCommand = new RoutedUICommand("Изменить запись", "EditConnectCommand", typeof(AppCommands));
            DeleteConnectCommand = new RoutedUICommand("Удалить запись", "DeleteConnectCommand", typeof(AppCommands));
            //
            TestCommand = new RoutedUICommand("Проверить соединение", "TestCommand ", typeof(AppCommands));
            ApplyCommand = new RoutedUICommand("Сохранить", "ApplyDataFormCommand", typeof(AppCommands));
            CancelCommand = new RoutedUICommand("Закрыть окно", "CancelDataFormCommand", typeof(AppCommands));
            //
            CloneCommand = new RoutedUICommand("Создать дубликат", "CloneCommand", typeof(AppCommands));
            //
            UpCommand = new RoutedUICommand("Поднять", "UpCommand", typeof(AppCommands));
            DownCommand = new RoutedUICommand("Опустить", "DownCommand", typeof(AppCommands));
        }

        public static void SetCommandBinding(CommandBindingCollection commandCollection, ICommand command, 
            ExecutedRoutedEventHandler executed, CanExecuteRoutedEventHandler canExecute = null)
        {
            var binding = (canExecute == null)
                ? new CommandBinding(command, executed)
                : new CommandBinding(command, executed, canExecute);
            commandCollection.Remove(binding);
            commandCollection.Add(binding);
        }

        public static void SetInputBinding(InputBindingCollection inputCollection, ICommand command, InputGesture gesture)
        {
            var binding = new InputBinding(command, gesture);
            inputCollection.Remove(binding);
            inputCollection.Add(binding);
        }
    }
}
