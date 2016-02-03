using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace SBUConfigurator
{
	public partial class TextBox : InputControl
	{
		public TextBox()
		{
			InitializeComponent();

            //InternalTextBoxHeight = 25;
		}

		public override void Focus()
		{
            this.InternalTextBox.Focus();
            //this.InternalTextBox.ScrollToEnd();
		}

        public void SetCaretIndex(int index)
        {
             this.InternalTextBox.CaretIndex = index;
             this.InternalTextBox.ScrollToEnd();
        }

        string oldValue = null;

        /// <summary>
        /// 
        /// </summary>
		public string Value
		{
			get { return (string)GetValue(ValueProperty); }
			set
            {
                oldValue = Value;
                SetValue(ValueProperty, value);
                //if (value != null)
                //    SetCaretIndex(value.Length);
            }
		}

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(string), typeof(TextBox), new PropertyMetadata((d, e) =>
			{
				var textBox = d as TextBox;
                if (textBox != null)
                {
                    textBox.SetupInternalBinding(ValueProperty, textBox.InternalTextBox, System.Windows.Controls.TextBox.TextProperty);
                    if (textBox.oldValue == null && textBox.Value != null)
                        textBox.SetCaretIndex(textBox.Value.Length);
                    textBox.oldValue = textBox.Value;
                }
            }));

        /// <summary>
        /// 
        /// </summary>
        public double InternalMaxLength
        {
            get { return (double)GetValue(InternalMaxLengthProperty); }
            set { SetValue(InternalMaxLengthProperty, value); }
        }

        public static readonly DependencyProperty InternalMaxLengthProperty =
            DependencyProperty.Register("InternalMaxLength", typeof(double), typeof(TextBox), new PropertyMetadata((d, e) =>
            {
                var textBox = d as TextBox;
                if (textBox != null)
                    textBox.SetupInternalBinding(InternalMaxLengthProperty, textBox.InternalTextBox, System.Windows.Controls.TextBox.MaxLengthProperty);
            }));

        /// <summary>
        /// 
        /// </summary>
        //public int InternalTextBoxHeight
        //{
        //    get { return (int)GetValue(InternalTextBoxHeightProperty); }
        //    set { SetValue(InternalTextBoxHeightProperty, value); }
        //}

        //public static readonly DependencyProperty InternalTextBoxHeightProperty =
        //    DependencyProperty.Register("InternalTextBoxHeight", typeof(int), typeof(TextBox), new PropertyMetadata((d, e) =>
        //    {
        //        var textBox = d as TextBox;
        //        if (textBox != null)
        //            textBox.SetupInternalBinding(InternalTextBoxHeightProperty, textBox.InternalTextBox, System.Windows.Controls.TextBox.HeightProperty);
        //    }));

        /// <summary>
        /// 
        /// </summary>
        public ScrollBarVisibility VertScrollVisibility
        {
            get { return (ScrollBarVisibility)GetValue(VertScrollVisibilityProperty); }
            set { SetValue(VertScrollVisibilityProperty, value); }
        }

        public static readonly DependencyProperty VertScrollVisibilityProperty =
            DependencyProperty.Register("VertScrollVisibility", typeof(ScrollBarVisibility), typeof(TextBox), new PropertyMetadata((d, e) =>
            {
                var textBox = d as TextBox;
                if (textBox != null)
                    textBox.SetupInternalBinding(VertScrollVisibilityProperty, textBox.InternalTextBox, TextBoxBase.VerticalScrollBarVisibilityProperty);
            }));

        /// <summary>
        /// 
        /// </summary>
        public ScrollBarVisibility HorizScrollVisibility
        {
            get { return (ScrollBarVisibility)GetValue(HorizScrollVisibilityProperty); }
            set { SetValue(HorizScrollVisibilityProperty, value); }
        }

        public static readonly DependencyProperty HorizScrollVisibilityProperty =
            DependencyProperty.Register("HorizScrollVisibility", typeof(ScrollBarVisibility), typeof(TextBox), new PropertyMetadata((d, e) =>
            {
                var textBox = d as TextBox;
                if (textBox != null)
                    textBox.SetupInternalBinding(HorizScrollVisibilityProperty, textBox.InternalTextBox, TextBoxBase.HorizontalScrollBarVisibilityProperty);
            }));

        /// <summary>
        /// 
        /// </summary>
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(TextBox), new PropertyMetadata((d, e) =>
            {
                var textBox = d as TextBox;
                if (textBox != null)
                    textBox.SetupInternalBinding(TextWrappingProperty, textBox.InternalTextBox, System.Windows.Controls.TextBox.TextWrappingProperty);
            }));

        /// <summary>
        /// 
        /// </summary>
        public bool AcceptsReturn
        {
            get { return (bool)GetValue(AcceptsReturnProperty); }
            set { SetValue(AcceptsReturnProperty, value); }
        }

        public static readonly DependencyProperty AcceptsReturnProperty =
            DependencyProperty.Register("AcceptsReturn", typeof(bool), typeof(TextBox), new PropertyMetadata((d, e) =>
            {
                var textBox = d as TextBox;
                if (textBox != null)
                    textBox.SetupInternalBinding(AcceptsReturnProperty, textBox.InternalTextBox, System.Windows.Controls.TextBox.AcceptsReturnProperty);
            }));

//        /// <summary>
//        /// 
//        /// </summary>
//        public TextCompositionEventHandler MyPreviewTextInput
//        {
//            get { return (TextCompositionEventHandler)GetValue(MyPreviewTextInputProperty); }
//            set { SetValue(MyPreviewTextInputProperty, value); }
//        }

//        public static readonly DependencyProperty MyPreviewTextInputProperty =
//            DependencyProperty.Register("MyPreviewTextInput", typeof(TextCompositionEventHandler), typeof(TextBox),
//         new PropertyMetadata());//(d, e) =>
////            {
////                var textBox = d as TextBox;
////                if (textBox != null)
////                    textBox.SetupInternalBinding(PreviewTextInputProperty, textBox.InternalTextBox, System.Windows.UIElement.AllowDropProperty);
////            }));

//        private void InternalTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
//        {
//            //InternalTextBox.ScrollToEnd();
//        }

//        private void InternalTextBox_GotFocus(object sender, RoutedEventArgs e)
//        {
//            //InternalTextBox.ScrollToEnd();
//        }
	}
}