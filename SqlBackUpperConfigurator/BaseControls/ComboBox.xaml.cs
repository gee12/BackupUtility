using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SBUConfigurator
{
	public partial class ComboBox : InputControl
	{
        public ComboBox()
		{
			InitializeComponent();
		}

		public override void Focus()
		{
            this.InternalComboBox.Focus();
		}

	    public bool IsSelectedFirst()
	    {
	        return (InternalComboBox.SelectedIndex == 0);
	    }

	    public T GetSelectedItemWithEmptyFirst<T>()
	    {
            return (IsSelectedFirst()) ? default(T) : (T)SelectedItem;
	    }

        public static T GetSelectedItemWithEmptyFirst<T>(System.Windows.Controls.ComboBox comboBox)
        {
            return (comboBox.SelectedIndex == 0) ? default(T) : (T)comboBox.SelectedItem;
        }

        public string GetText()
        {
            return (string.IsNullOrWhiteSpace(Text)) ? null : Text;
        }

        public string GetTextWithEmptyFirst()
        {
            //return (string.IsNullOrWhiteSpace(Text) || IsSelectedFirst()) ? null : Text;
            return (string.IsNullOrWhiteSpace(Text)) ? null : Text;
        }

        public void SetSelectedItem<T>(T item)
        {
            SelectedItem = item;
        }

        /// <summary>
        /// Установить выбранным item.
        /// Если item == null, установить выбранным первое значение из ItemSource
        /// </summary>
        public void SetSelectedItemFirstIsNull(object item)
        {
            SetSelectedItemFirstIfValueNull(InternalComboBox, item);
        }

        public static void SetSelectedItemFirstIfValueNull(System.Windows.Controls.ComboBox comboBox, object item)
        {
            if (item == null)
            {
                comboBox.SelectedIndex = 0;
            }
            else comboBox.SelectedItem = item;
        }

        public void SetItemsSource(IEnumerable items)
        {
            ItemsSource = items;
        }

        public void SetItemsSourceWithEmptyFirst<T>(IEnumerable<T> items)
            where T : new()
        {
            SetItemsSourceWithEmptyFirst(this, items);
        }

        public static void SetItemsSourceWithEmptyFirst<T>(System.Windows.Controls.ComboBox comboBox, IEnumerable<T> items)
            where T : new()
        {
            comboBox.ItemsSource = Utils.AddEmptyAtFirst(items);
        }

        public static void SetItemsSourceWithEmptyFirst<T>(ComboBox comboBox, IEnumerable<T> items)
            where T : new()
        {
            comboBox.ItemsSource = Utils.AddEmptyAtFirst(items);
        }

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ComboBox), new PropertyMetadata((d, e) =>
            {
                var comboBox = d as ComboBox;
                if (comboBox != null)
                    comboBox.SetupInternalBinding(ItemsSourceProperty, comboBox.InternalComboBox, ItemsControl.ItemsSourceProperty);
            }));

        /// <summary>
        /// 
        /// </summary>
        //public int SelectedIndex
        //{
        //    get { return (int)GetValue(SelectedIndexProperty); }
        //    set { SetValue(SelectedIndexProperty, value); }
        //}
        //public static readonly DependencyProperty SelectedIndexProperty =
        //    DependencyProperty.Register("SelectedIndex", typeof(object), typeof(ComboBox), new PropertyMetadata((d, e) =>
        //    {
        //        var comboBox = d as ComboBox;
        //        if (comboBox != null)
        //            comboBox.SetupInternalBinding(SelectedIndexProperty, comboBox.InternalComboBox, Selector.SelectedIndexProperty);
        //    }));

        /// <summary>
        /// 
        /// </summary>
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(ComboBox), new PropertyMetadata((d, e) =>
            {
                var comboBox = d as ComboBox;
                if (comboBox != null)
                    comboBox.SetupInternalBinding(SelectedItemProperty, comboBox.InternalComboBox, Selector.SelectedItemProperty);
            }));

        /// <summary>
        /// 
        /// </summary>
        public object SelectedValue
        {
            get { return (object)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }
        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(object), typeof(ComboBox), new PropertyMetadata((d, e) =>
            {
                var comboBox = d as ComboBox;
                if (comboBox != null)
                    comboBox.SetupInternalBinding(SelectedValueProperty, comboBox.InternalComboBox, Selector.SelectedValueProperty);
            }));

        /// <summary>
        /// 
        /// </summary>
        public string SelectedValuePath
        {
            get { return (string)GetValue(SelectedValuePathProperty); }
            set { SetValue(SelectedValuePathProperty, value); }
        }
        public static readonly DependencyProperty SelectedValuePathProperty =
            DependencyProperty.Register("SelectedValuePath", typeof(string), typeof(ComboBox), new PropertyMetadata((d, e) =>
            {
                var comboBox = d as ComboBox;
                if (comboBox != null)
                    comboBox.SetupInternalBinding(SelectedValuePathProperty, comboBox.InternalComboBox, Selector.SelectedValuePathProperty);
            }));

        /// <summary>
        /// 
        /// </summary>
        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }
        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(ComboBox), new PropertyMetadata((d, e) =>
            {
                var comboBox = d as ComboBox;
                if (comboBox != null)
                    comboBox.SetupInternalBinding(DisplayMemberPathProperty, comboBox.InternalComboBox, System.Windows.Controls.ItemsControl.DisplayMemberPathProperty);
            }));

        /// <summary>
        /// 
        /// </summary>
        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }
        public static readonly DependencyProperty IsEditableProperty =
            DependencyProperty.Register("IsEditable", typeof(bool), typeof(ComboBox), new PropertyMetadata((d, e) =>
            {
                var comboBox = d as ComboBox;
                if (comboBox != null)
                    comboBox.SetupInternalBinding(IsEditableProperty, comboBox.InternalComboBox, System.Windows.Controls.ComboBox.IsEditableProperty);
            }));

        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ComboBox), new PropertyMetadata((d, e) =>
            {
                var comboBox = d as ComboBox;
                if (comboBox != null)
                    comboBox.SetupInternalBinding(TextProperty, comboBox.InternalComboBox, System.Windows.Controls.ComboBox.TextProperty);
            }));

        /*
         *ublic TextCompositionEventHandler MyPreviewTextInput
        {
            get { return (TextCompositionEventHandler)GetValue(MyPreviewTextInputProperty); }
            set { SetValue(MyPreviewTextInputProperty, value); }
        }

        public static readonly DependencyProperty MyPreviewTextInputProperty =
            DependencyProperty.Register("MyPreviewTextInput", typeof(TextCompositionEventHandler), typeof(TextBox),
	     new PropertyMetadata());
         * */
        #endregion
    }
}