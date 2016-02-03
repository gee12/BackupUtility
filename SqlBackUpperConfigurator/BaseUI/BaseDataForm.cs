using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
//using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Windows.Threading;
using BackupLibrary;
using SqlBackUpperLib;

namespace SBUConfigurator
{
    public abstract class BaseDataForm<T, TDC> : Window
        where T : IRecord<T>, new()
        where TDC : UserControl, new()
    {
        public const short MODE_EDIT = 0;
        public const short MODE_READ = 1;
        public const short MODE_BLOCKING = 2;
        public const short MODE_CLONE = 3;
        public const short MODE_CREATE = 4;

        public const short BLOCKING_MODE_UNBLOCKED = 0;
        public const short BLOCKING_MODE_BLOCKED = 1;
        public const short BLOCKING_MODE_SELF_BLOCKED = 2;

        public short Result = ResultCodes.Cancel;
        protected BaseDataControl BaseControls;
        public TDC ContentControls { get; protected set; }
        //public BaseDataManager<T> DataManager;
        protected short mode = MODE_EDIT;
        //public short BlockingMode = BLOCKING_MODE_UNBLOCKED;

        protected BaseDataForm()
        {
            Init();
        }

        private void Init()
        {
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/ico.ico", UriKind.RelativeOrAbsolute));
            this.Closing += BaseDataForm_Closing;
                //Application.Current.Resources["appIcon"] as ImageSource; //Utils.AppIcon;

            // add BaseDataControl
            BaseControls = new BaseDataControl();
            this.Content = BaseControls;
            // add dataControls
            ContentControls = new TDC();
            BaseControls.gBase.Children.Add(ContentControls);

            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

            // add commands to BaseDataControl
            AppCommands.SetCommandBinding(CommandBindings, AppCommands.ApplyCommand, BApplyOnClick, IsValid);
            AppCommands.SetInputBinding(InputBindings, AppCommands.ApplyCommand, new KeyGesture(Key.Enter, ModifierKeys.Control));
            BaseControls.bApply.Command = AppCommands.ApplyCommand;

            AppCommands.SetCommandBinding(CommandBindings, AppCommands.CancelCommand, BCancelOnClick);
            AppCommands.SetInputBinding(InputBindings, AppCommands.CancelCommand, new KeyGesture(Key.Escape));
            BaseControls.bCancel.Command = AppCommands.CancelCommand;

            IsVisibleChanged += FormIsVisibleChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        public short ShowModal(Window owner)//, BaseDataManager<T> dataManager)
        {
            this.Owner = owner;
            return ShowModal(/*dataManager,*/ default(T), MODE_CREATE);
        }

        /// <summary>
        /// 
        /// </summary>
        public short ShowModal(Window owner, /*BaseDataManager<T> dataManager,*/ T entity = default(T), short mode = MODE_EDIT)
        {
            this.Owner = owner;
            return ShowModal(/*dataManager, */entity, mode);
        }

        /// <summary>
        /// 
        /// </summary>
        public short ShowModal(/*BaseDataManager<T> dataManager,*/ T entity = default(T), short mode = MODE_EDIT)
        {
            //this.DataManager = dataManager;

            SetTitle(mode);
            SetControls(entity);
            SetDataContext(entity);
            this.ShowDialog();
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        public void IsValid(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsValid();
        }

        public virtual bool IsValid()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void SetControls(T entity) { }

        /// <summary>
        /// 
        /// </summary>
        private void BApplyOnClick(object sender, RoutedEventArgs e)
        {
            Apply();
        }

        protected virtual void Apply() 
        {
            Close(ResultCodes.Apply);
        }

        /// <summary>
        /// 
        /// </summary>
        private void BCancelOnClick(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        protected virtual void Cancel()
        {
            Close(ResultCodes.Cancel);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void SetDataContext(T dc)
        {
            if (dc == null)
                dc = new T();
            BaseControls.gBase.DataContext = dc;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetTitle(string title)
        {
            this.Title = title;
        }

        public virtual void SetTitle(short mode)
        {
            string title = (mode == MODE_CREATE) ? "Добавление"
                : (mode == MODE_READ) ? "Просмотр"
                : (mode == MODE_CLONE) ? "Дублирование"
                : "Изменение";
            title += string.Format(" записи [{0}]", ((ITypeReadableName)new T()).GetReadableTypeName());
            this.Title = title;
        }

        void FormIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                Dispatcher.BeginInvoke(
                DispatcherPriority.ContextIdle,
                new Action(delegate()
                {
                    FormIsVisibleChanged();
                }));
            }
        }

        public virtual void FormIsVisibleChanged() {}
        
        /// <summary>
        /// 
        /// </summary>
        public T GetEntity()
        {
            return (T)BaseControls.gBase.DataContext;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Close(short result)
        {
            Result = result;
            this.Close();
        }

        void BaseDataForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FormClosing();
        }

        protected virtual void FormClosing()
        {
        }

        public TDC GetDataControl()
        {
            return ContentControls;
        }
    }
}
