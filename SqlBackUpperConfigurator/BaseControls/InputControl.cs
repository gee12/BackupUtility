using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SBUConfigurator
{
	public class InputControl : UserControl
	{
		private readonly Grid _layoutGrid;
		private readonly TextBlock _labelTextBlock;
		private readonly Grid _internalContentGrid;

		public InputControl()
		{
			base.IsTabStop = false;

            this._layoutGrid = new Grid() { /*Margin = new Thickness(2)*/};
			this.Content = _layoutGrid;

			this._labelTextBlock = new TextBlock() { Margin = new Thickness(0,0,0,2) };
			_layoutGrid.Children.Add(this._labelTextBlock);

            this._internalContentGrid = new Grid() { /*Margin = new Thickness(2)*/};
			_layoutGrid.Children.Add(this._internalContentGrid);

            //LineHeight = 25;

			this.Loaded += InputControl_Loaded;
		}

        public void InputControl_Loaded(object sender, RoutedEventArgs e)
		{
			this.UpdateControlLayout();
		}

		public new virtual void Focus()
		{
			var control = this.InternalContent as Control;
			if (control != null)
				control.Focus();
			else
				base.Focus();
		}

        public virtual void UpdateControlState()
		{
			var bindingExpression = this.GetBindingExpression(EditableItemProperty);
			if (bindingExpression != null)
			{
				if (this.EditableItem != null)
				{
					this.InternalIsEnabled = true;
					this.InternalIsReadOnly = false;
					this.InternalIsTabStop = this.IsTabStop;
				}
				else
				{
					this.InternalIsEnabled = false;
					this.InternalIsReadOnly = true;
					this.InternalIsTabStop = false;
				}
			}
			else
			{
				this.InternalIsEnabled = true;
				this.InternalIsReadOnly = false;
				this.InternalIsTabStop = this.IsTabStop;
			}

			var visibility = this.IsVisible ? Visibility.Visible : Visibility.Collapsed;

			this.SetValue(VisibilityProperty, visibility);
		}

        public virtual void UpdateControlLayout()
		{
			if (this._layoutGrid == null)
				return;

			this._layoutGrid.ColumnDefinitions.Clear();
			this._layoutGrid.RowDefinitions.Clear();

            this._labelTextBlock.HorizontalAlignment = this.LabelHorizontalAlignment;

			switch (this.LabelPosition)
			{
				case LabelPosition.Left:
					this._layoutGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = this.LabelSize });
					this._layoutGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = this.ControlSize });
					this._layoutGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(this.LineHeight * this.LinesNumber) });

					Grid.SetRow(this._labelTextBlock, 0);
					Grid.SetColumn(this._labelTextBlock, 0);
					this._labelTextBlock.VerticalAlignment = VerticalAlignment.Center;

					Grid.SetRow(this._internalContentGrid, 0);
					Grid.SetColumn(this._internalContentGrid, 1);
					break;

				case LabelPosition.Right:
					this._layoutGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = this.ControlSize });
					this._layoutGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = this.LabelSize });
					this._layoutGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(this.LineHeight * this.LinesNumber) });

					Grid.SetRow(this._labelTextBlock, 0);
					Grid.SetColumn(this._labelTextBlock, 1);
					this._labelTextBlock.VerticalAlignment = VerticalAlignment.Center;

					Grid.SetRow(this._internalContentGrid, 0);
					Grid.SetColumn(this._internalContentGrid, 0);
                    break;

                case LabelPosition.Top:
                	this._layoutGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = this.ControlSize });
                	this._layoutGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(this.LineHeight) });
                	this._layoutGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(this.LineHeight * this.LinesNumber) });
                
                	Grid.SetRow(this._labelTextBlock, 0);
                	Grid.SetColumn(this._labelTextBlock, 0);
                	this._labelTextBlock.VerticalAlignment = VerticalAlignment.Bottom;
                
                	Grid.SetRow(this._internalContentGrid, 1);
                	Grid.SetColumn(this._internalContentGrid, 0);
                	break;
                
                case LabelPosition.Bottom:
                	this._layoutGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = this.ControlSize });
                	this._layoutGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(this.LineHeight * this.LinesNumber) });
                	this._layoutGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(this.LineHeight) });
                
                	Grid.SetRow(this._labelTextBlock, 1);
                	Grid.SetColumn(this._labelTextBlock, 0);
                	this._labelTextBlock.VerticalAlignment = VerticalAlignment.Top;
                
                	Grid.SetRow(this._internalContentGrid, 0);
                	Grid.SetColumn(this._internalContentGrid, 0);
                	break;
			}
		}

        public void SetupInternalBinding(DependencyProperty publicProperty, FrameworkElement internalControl, DependencyProperty internalProperty)
		{
			var bindingExpression = this.GetBindingExpression(publicProperty);

			if (bindingExpression != null)
			{
				var binding = this.CreateInternalBinding(bindingExpression.DataItem, bindingExpression.ParentBinding.Path.Path, bindingExpression);
				internalControl.SetBinding(internalProperty, binding);
			}
		}

        public Binding CreateInternalBinding(object source, string path, BindingExpression bindingExpression)
		{
			var binding = new Binding(path)
			{
				Source = source,
				Mode = BindingMode.TwoWay,
				ValidatesOnDataErrors = true,
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
			};

			if (bindingExpression == null) 
				return binding;

			binding.Converter = bindingExpression.ParentBinding.Converter;
			binding.ConverterCulture = bindingExpression.ParentBinding.ConverterCulture;
			binding.ConverterParameter = bindingExpression.ParentBinding.ConverterParameter;

			return binding;
		}

		#region props

        /// <summary>
        /// 
        /// </summary>
		public object InternalContent
		{
			get { return GetValue(InternalContentProperty); }
			set { SetValue(InternalContentProperty, value); }
		}

		public static readonly DependencyProperty InternalContentProperty =
			DependencyProperty.Register("InternalContent", typeof(object), typeof(InputControl), new PropertyMetadata((d, e) =>
			{
				((InputControl)d)._internalContentGrid.Children.Add((UIElement)e.NewValue);
			}));

        /// <summary>
        /// 
        /// </summary>
		public string Label
		{
			get { return (string)GetValue(LabelProperty); }
			set { SetValue(LabelProperty, value); }
		}

		public static readonly DependencyProperty LabelProperty =
			DependencyProperty.Register("Label", typeof(string), typeof(InputControl), new PropertyMetadata((d, e) =>
			{
				((InputControl)d)._labelTextBlock.Text = e.NewValue as string;
			}));

        /// <summary>
        /// 
        /// </summary>
		public GridLength LabelSize
		{
			get { return (GridLength)GetValue(LabelSizeProperty); }
			set { SetValue(LabelSizeProperty, value); }
		}

		public static readonly DependencyProperty LabelSizeProperty =
			DependencyProperty.Register("LabelSize", typeof(GridLength), typeof(InputControl), new PropertyMetadata(GridLength.Auto, LayoutChangedCallback));

        /// <summary>
        /// 
        /// </summary>
		public LabelPosition LabelPosition
		{
			get { return (LabelPosition)GetValue(LabelPositionProperty); }
			set { SetValue(LabelPositionProperty, value); }
		}

		public static readonly DependencyProperty LabelPositionProperty =
			DependencyProperty.Register("LabelPosition", typeof(LabelPosition), typeof(InputControl), new PropertyMetadata(LabelPosition.Left, LayoutChangedCallback));

        /// <summary>
        /// 
        /// </summary>
        public HorizontalAlignment LabelHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(LabelHorizontalAlignmentProperty); }
            set { SetValue(LabelHorizontalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty LabelHorizontalAlignmentProperty =
            DependencyProperty.Register("LabelHorizontalAlignment", typeof(HorizontalAlignment), typeof(InputControl), new PropertyMetadata(HorizontalAlignment.Left, LayoutChangedCallback));

        /// <summary>
        /// 
        /// </summary>
		public GridLength ControlSize
		{
			get { return (GridLength)GetValue(ControlSizeProperty); }
			set { SetValue(ControlSizeProperty, value); }
		}

		public static readonly DependencyProperty ControlSizeProperty =
			DependencyProperty.Register("ControlSize", typeof(GridLength), typeof(InputControl), new PropertyMetadata(GridLength.Auto, LayoutChangedCallback));

        /// <summary>
        /// 
        /// </summary>
        //internal Double LineHeight
        //{
        //    get { return 26; }
        //}
        public Double LineHeight
        {
            get { return (Double)GetValue(LineHeightProperty); }
            set { SetValue(LineHeightProperty, value); }
        }

        public static readonly DependencyProperty LineHeightProperty =
            DependencyProperty.Register("LineHeight", typeof(Double), typeof(InputControl), new PropertyMetadata(25d, LayoutChangedCallback));

        /// <summary>
        /// 
        /// </summary>
		public int LinesNumber
		{
			get { return (int)GetValue(LinesNumberProperty); }
			set { SetValue(LinesNumberProperty, value); }
		}

		public static readonly DependencyProperty LinesNumberProperty =
			DependencyProperty.Register("LinesNumber", typeof(int), typeof(InputControl), new PropertyMetadata(1, LayoutChangedCallback));

		private static void LayoutChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((InputControl)d).UpdateControlLayout();
		}

        /// <summary>
        /// 
        /// </summary>
		public bool InternalIsEnabled
		{
			get { return (bool)GetValue(InternalIsEnabledProperty); }
			set { SetValue(InternalIsEnabledProperty, value); }
		}

		public static readonly DependencyProperty InternalIsEnabledProperty =
			DependencyProperty.Register("InternalIsEnabled", typeof(bool), typeof(InputControl), new PropertyMetadata(true));

        /// <summary>
        /// 
        /// </summary>
		public bool InternalIsReadOnly
		{
			get { return (bool)GetValue(InternalIsReadOnlyProperty); }
			set { SetValue(InternalIsReadOnlyProperty, value); }
		}

		public static readonly DependencyProperty InternalIsReadOnlyProperty =
			DependencyProperty.Register("InternalIsReadOnly", typeof(bool), typeof(InputControl), new PropertyMetadata(false));

        /// <summary>
        /// 
        /// </summary>
		public bool InternalIsTabStop
		{
			get { return (bool)GetValue(InternalIsTabStopProperty); }
			set { SetValue(InternalIsTabStopProperty, value); }
		}

		public static readonly DependencyProperty InternalIsTabStopProperty =
			DependencyProperty.Register("InternalIsTabStop", typeof(bool), typeof(InputControl), new PropertyMetadata(true));

        /// <summary>
        /// 
        /// </summary>
		public object EditableItem
		{
			get { return GetValue(EditableItemProperty); }
			set { SetValue(EditableItemProperty, value); }
		}

		public static readonly DependencyProperty EditableItemProperty =
			DependencyProperty.Register("EditableItem", typeof(object), typeof(InputControl), new PropertyMetadata(null, EditableItemChangedCallback));

		private static void EditableItemChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var inputControl = d as InputControl;
			if (inputControl != null)
				inputControl.UpdateControlState();
		}

		#endregion
	}

	public enum LabelPosition
	{
		None,
		Left,
		Right,
        Top,
        Bottom
	}
}