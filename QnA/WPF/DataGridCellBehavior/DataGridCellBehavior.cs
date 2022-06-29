namespace WpfApp5
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

    internal class DataGridCellBehavior : DependencyObject
    {
        public static readonly DependencyProperty IsCellFocusProperty =
            DependencyProperty.RegisterAttached("IsCellFocus",
                typeof(bool),
                typeof(DataGridCellBehavior),
                new UIPropertyMetadata(false, (d, e) =>
                {
                    DataGrid dataGrid = d as DataGrid;
                    if (dataGrid == null)
                    {
                        throw new ArgumentException("This property may only be used on DataGrid");
                    }

                    if (((bool)e.NewValue) is true)
                    {
                        dataGrid.SelectedCellsChanged += DataGrid_SelectedCellsChanged;
                    }
                    else
                    {
                        dataGrid.SelectedCellsChanged -= DataGrid_SelectedCellsChanged;
                    }
                }));

        private static void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid != null && e.AddedCells != null && e.AddedCells.Count > 0)
            {
                var cell = e.AddedCells[0];
                if (!cell.IsValid)
                    return;

                var generator = dataGrid.ItemContainerGenerator;
                int columnIndex = cell.Column.DisplayIndex;
                int rowIndex = generator.IndexFromContainer(generator.ContainerFromItem(cell.Item));

                ICommand selectedCellsChangedCommand = GetSelectedCellsChangedCommand(dataGrid);
                selectedCellsChangedCommand.Execute($"{rowIndex},{columnIndex}");
            }
        }

        public static bool GetIsCellFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsCellFocusProperty);
        }

        public static void SetIsCellFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(IsCellFocusProperty, value);
        }

        public static ICommand GetSelectedCellsChangedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(SelectedCellsChangedCommandProperty);
        }

        public static void SetSelectedCellsChangedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(SelectedCellsChangedCommandProperty, value);
        }

        public static readonly DependencyProperty SelectedCellsChangedCommandProperty =
            DependencyProperty.RegisterAttached(
                "SelectedCellsChangedCommand",
                typeof(ICommand),
                typeof(DataGridCellBehavior),
                new UIPropertyMetadata(null)
            );


        public static readonly DependencyProperty CellFocusProperty =
            DependencyProperty.RegisterAttached("CellFocus",
                typeof(string),
                typeof(DataGridCellBehavior),
                new UIPropertyMetadata(null, OnCellFocusPropertyChanged));

        public static string GetCellFocus(DependencyObject obj)
        {
            return (string)obj.GetValue(CellFocusProperty);
        }

        public static void SetCellFocus(DependencyObject obj, string value)
        {
            obj.SetValue(CellFocusProperty, value);
        }

        private static void OnCellFocusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue == null) return;

            DataGrid dataGrid = (DataGrid)d;
            if (dataGrid.SelectionUnit != DataGridSelectionUnit.Cell)
                throw new ArgumentException("데이터그리드 SelectionUnit 속성이 Cell 모드가 아닙니다!");

            var row_cell_token = e.NewValue.ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if(row_cell_token.Length != 2)
                throw new ArgumentException("데이터그리드 row, cell index 문자열 정보가 잘못 되었습니다!");

            int rowIndex = int.Parse(row_cell_token[0]);
            int columnIndex = int.Parse(row_cell_token[1]);

            if (rowIndex < 0 || rowIndex > (dataGrid.Items.Count - 1))
                throw new ArgumentException("유효하지 않은 Row Index!");

            if (columnIndex < 0 || columnIndex > (dataGrid.Columns.Count - 1))
                throw new ArgumentException("유효하지 않은 Column Index!");

            dataGrid.SelectedCells.Clear();

            object item = dataGrid.Items[rowIndex];
            DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            if (row == null)
            {
                dataGrid.ScrollIntoView(item);
                row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            }
            if (row != null)
            {
                DataGridCell cell = GetCell(dataGrid, row, columnIndex);
                if (cell != null)
                {
                    DataGridCellInfo dataGridCellInfo = new DataGridCellInfo(cell);
                    dataGrid.SelectedCells.Add(dataGridCellInfo);
                    cell.Focus();
                }
            }
        }

        public static DataGridCell GetCell(DataGrid dataGrid, DataGridRow rowContainer, int column)
        {
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                if (presenter == null)
                {
                    // CellsPresenter가 null인 경우 (아직 렌더링 전으로 추측?)
                    // DataGridRow의 ApplyTemplate()를 호출해줌으로써 비주얼트리가 적용되도록 처리
                    rowContainer.ApplyTemplate();
                    presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                }
                if (presenter != null)
                {
                    DataGridCell cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    if (cell == null)
                    {
                        // 해당 Cell이 데이터그리드 스크롤영역 밖으로 보이지 않은 경우
                        // 해당 Cell로 스크롤이동
                        dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);
                        cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    }
                    return cell;
                }
            }
            return null;
        }

        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
}
