
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Size = System.Windows.Size;

namespace SBUConfigurator
{
	public class GridView : DataGrid
    {
        public const ListSortDirection ASC = ListSortDirection.Ascending;
        public const ListSortDirection DESC = ListSortDirection.Descending;

		public GridView() :
			base()
		{
		    this.AutoGenerateColumns = false;
            this.SelectionMode = DataGridSelectionMode.Single;
            this.SelectionUnit = DataGridSelectionUnit.FullRow;
            this.Margin = new Thickness(2);
            this.IsReadOnly = true;
            this.GridLinesVisibility = DataGridGridLinesVisibility.All;
            this.VerticalGridLinesBrush = Brushes.LightGray;
            this.HorizontalGridLinesBrush = Brushes.LightGray;
            this.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataGridRow GetSelectedDataGridRow()
        {
            int index = SelectedIndex;
            if (index == -1)
            {
                if (Items.Count != 0)
                    index = 0;
                else return null;
            }
            var current = Items[index];
            SelectedItem = current;
            SelectedIndex = index;

            DataGridRow row = GetDataGridRow(index);
            if (row == null)
            {
                ScrollIntoView(current);
                row = GetDataGridRow(index);
            }
            return row;
        }

        public DataGridRow GetDataGridRow(int index)
        {
            return (DataGridRow)ItemContainerGenerator.ContainerFromIndex(index);
        }

        public DataGridRow GetDataGridRow<T>(T item)
        {
            return (DataGridRow)ItemContainerGenerator.ContainerFromItem(item);
        }

        public bool IsClickedOnDataGridCell(MouseButtonEventArgs e)
        {
            var depObj = (DependencyObject)e.OriginalSource;
            while ((depObj != null) && !(depObj is DataGridCell))
            {
                depObj = VisualTreeHelper.GetParent(depObj);
            }
            return (depObj != null);
        }

        public bool IsClickedOnParentWithType<T>(MouseButtonEventArgs e)
        {
            var depObj = (DependencyObject)e.OriginalSource;
            while ((depObj != null) && !(depObj is T))
            {
                depObj = VisualTreeHelper.GetParent(depObj);
            }
            return (depObj != null);
        }

        public DataGridCell GetDataGridCell(MouseButtonEventArgs e)
        {
            var depObj = (DependencyObject)e.OriginalSource;
            while ((depObj != null) && !(depObj is DataGridCell))
            {
                depObj = VisualTreeHelper.GetParent(depObj);
            }
            return (DataGridCell)depObj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public DataGridCell GetCell(DataGridRow row, int columnIndex = 0)
        {
            if (row == null) return null;

            var presenter = GetVisualChild<DataGridCellsPresenter>(row);
            if (presenter == null) return null;

            var cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
            if (cell != null) return cell;

            // today try to bring into view and retreive the cell
            ScrollIntoView(row, Columns[columnIndex]);
            cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);

            return cell;
        }

        public DataGridCell GetCell(int row, int column)
        {
            DataGridRow rowContainer = GetRow(row);
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);
                if (presenter == null)
                {
                    ScrollIntoView(rowContainer, Columns[column]);
                    presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);
                }
                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                return cell;
            }
            return null;
        }

        public DataGridRow GetRow(int index)
        {
            DataGridRow row = (DataGridRow)ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                UpdateLayout();
                ScrollIntoView(Items[index]);
                row = (DataGridRow)ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            if (parent == null) return null;
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <returns></returns>
        public static T GetVisualParent<T>(Visual child) where T : Visual
        {
            if (child == null) return null;
            T parent = default(T);
            Visual v = (Visual)VisualTreeHelper.GetParent(child);
            if (v == null)
                return null;
            parent = v as T;
            if (parent == null)
            {
                parent = GetVisualParent<T>(v);
            }
            return parent;

        }


        /// <summary>
        /// 
        /// </summary>
        public void RefreshItems()
        {
            Items.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        public void FocusOnDataGrid()
        {
            var row = GetSelectedDataGridRow();
            if (row != null)
            {
                row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
            }
        }

        public static DataGridCell GetDataGridCell(DataGridCellInfo cellInfo)
        {
            if (cellInfo == null || cellInfo.Column == null) return null;
            var cellContent = cellInfo.Column.GetCellContent(cellInfo.Item);
            if (cellContent != null)
                return (DataGridCell)cellContent.Parent;

            return null;
        }

        //private DataTemplate CreateCellTemplate()
        //{
        //    DataTemplate template = new DataTemplate();
        //    FrameworkElementFactory gridFactory = new FrameworkElementFactory(typeof(Grid));
        //    // rows
        //    var row1 = new FrameworkElementFactory(typeof(RowDefinition));
        //    var row2 = new FrameworkElementFactory(typeof(RowDefinition));
        //    gridFactory.AppendChild(row1);
        //    gridFactory.AppendChild(row2);
        //    // columns
        //    var col1 = new FrameworkElementFactory(typeof(ColumnDefinition));
        //    col1.SetValue(ColumnDefinition.WidthProperty, new GridLength(100, GridUnitType.Pixel));
        //    var col2 = new FrameworkElementFactory(typeof(ColumnDefinition));
        //    col2.SetValue(ColumnDefinition.WidthProperty, new GridLength(100, GridUnitType.Pixel));
        //    gridFactory.AppendChild(col1);
        //    gridFactory.AppendChild(col2);

        //    template.VisualTree = gridFactory;
        //    return template;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public int GetRowIndex(DataGridCell cell)
        {
            // Use reflection to get DataGridCell.RowDataItem property value.
            PropertyInfo rowDataItemProperty = cell.GetType().
                GetProperty("RowDataItem", BindingFlags.Instance | BindingFlags.NonPublic);

            return Items.IndexOf(rowDataItemProperty.GetValue(cell, null));
        }
	}
}