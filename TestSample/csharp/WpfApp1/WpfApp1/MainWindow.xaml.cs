using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int CellSize = 30;
        private const int InitialRows = 1;
        private const int InitialCols = 1;
        private const int MaxRows = 15;
        private const int MaxCols = 15;

        private int _currentRows = InitialRows;
        private int _currentCols = InitialCols;
        private bool _tableInserted = false;
        private Grid _insertedTable;
        private Window _tablePreviewWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TablePreviewCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_tableInserted) return;

            Point mousePos = e.GetPosition(this.xTablePreviewCanvas);

            // 마우스 위치에 따라 행과 열 수 계산
            int rows = Math.Min(Math.Max((int)(mousePos.Y / CellSize) + 1, InitialRows), MaxRows);
            int cols = Math.Min(Math.Max((int)(mousePos.X / CellSize) + 1, InitialCols), MaxCols);

            if (rows != _currentRows || cols != _currentCols)
            {
                _currentRows = rows;
                _currentCols = cols;
                DrawTablePreview(rows, cols);
                this.xSizeInfoText.Text = $"현재 크기: {rows} x {cols}";
            }
        }

        private void TablePreviewCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_tableInserted) return;

            // 테이블 삽입
            InsertTable(_currentRows, _currentCols);
            _tableInserted = true;
            this.xSizeInfoText.Text = $"테이블 삽입됨: {_currentRows} x {_currentCols}";
        }

        private void InsertTableButton_Click(object sender, RoutedEventArgs e)
        {
            this.ShowTablePreviewPopup();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // 테이블 제거 및 프리뷰 초기화
            if (_insertedTable != null)
            {
                this.xTablePreviewCanvas.Children.Remove(_insertedTable);
                _insertedTable = null;
            }

            _tableInserted = false;
            _currentRows = InitialRows;
            _currentCols = InitialCols;
        }

        private void ShowTablePreviewPopup()
        {
            // 이미 팝업이 열려있으면 닫기
            if (_tablePreviewWindow != null)
            {
                _tablePreviewWindow.Close();
            }

            // 새로운 팝업 윈도우 생성
            _tablePreviewWindow = new Window
            {
                Title = "Insert table layout",
                Width = 500,
                Height = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize
            };

            // 팝업 내용 구성
            Grid popupGrid = new Grid();
            _tablePreviewWindow.Content = popupGrid;

            // 행과 열 정의
            popupGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength() });
            popupGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            popupGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength() });

            // 안내 텍스트
            TextBlock instructionText = new TextBlock
            {
                Text = "마우스를 이동하여 테이블 크기를 조정하고 클릭하여 선택하세요.",
                Margin = new Thickness(10),
                FontSize = 14,
                TextAlignment = TextAlignment.Center
            };
            popupGrid.Children.Add(instructionText);
            Grid.SetRow(instructionText, 0);

            // 크기 정보 텍스트
            TextBlock sizeText = new TextBlock
            {
                Margin = new Thickness(10),
                FontSize = 14,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            popupGrid.Children.Add(sizeText);
            Grid.SetRow(sizeText, 1);
            Panel.SetZIndex(sizeText, 10);

            // 테이블 미리보기 캔버스
            Canvas previewCanvas = new Canvas
            {
                Background = Brushes.White,
                Margin = new Thickness(10)
            };

            Border canvasBorder = new Border
            {
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(10),
                Child = previewCanvas
            };

            popupGrid.Children.Add(canvasBorder);
            Grid.SetRow(canvasBorder, 1);

            // 버튼 영역
            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(10)
            };

            Button cancelButton = new Button
            {
                Content = "취소",
                Width = 100,
                Height = 30,
                Margin = new Thickness(5)
            };
            cancelButton.Click += (s, e) => _tablePreviewWindow.Close();
            buttonPanel.Children.Add(cancelButton);

            popupGrid.Children.Add(buttonPanel);
            Grid.SetRow(buttonPanel, 2);

            // 현재 선택된 행과 열 정보
            int popupRows = InitialRows;
            int popupCols = InitialCols;

            // 초기 테이블 미리보기 그리기
            Action<int, int> drawPreview = (rows, cols) =>
            {
                previewCanvas.Children.Clear();

                // 배경 그리기
                Rectangle background = new Rectangle
                {
                    Width = MaxCols * CellSize,
                    Height = MaxRows * CellSize,
                    Fill = Brushes.LightGray,
                    Opacity = 0.3
                };
                previewCanvas.Children.Add(background);
                Canvas.SetLeft(background, 0);
                Canvas.SetTop(background, 0);

                // 활성화된 셀 그리기
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < cols; col++)
                    {
                        Rectangle cell = new Rectangle
                        {
                            Width = CellSize - 1,
                            Height = CellSize - 1,
                            Fill = Brushes.SkyBlue,
                            Stroke = Brushes.DarkBlue,
                            StrokeThickness = 1
                        };
                        previewCanvas.Children.Add(cell);
                        Canvas.SetLeft(cell, col * CellSize);
                        Canvas.SetTop(cell, row * CellSize);
                    }
                }

                sizeText.Text = $"현재 크기: {rows} x {cols}";
            };

            // 초기 그리기
            drawPreview(popupRows, popupCols);

            // 마우스 이동 이벤트 처리
            previewCanvas.MouseMove += (s, e) =>
            {
                Point mousePos = e.GetPosition(previewCanvas);

                // 마우스 위치에 따라 행과 열 수 계산
                int rows = Math.Min(Math.Max((int)(mousePos.Y / CellSize) + 1, InitialRows), MaxRows);
                int cols = Math.Min(Math.Max((int)(mousePos.X / CellSize) + 1, InitialCols), MaxCols);

                if (rows != popupRows || cols != popupCols)
                {
                    popupRows = rows;
                    popupCols = cols;
                    drawPreview(popupRows, popupCols);
                }
            };

            // 클릭 이벤트 처리
            previewCanvas.MouseLeftButtonDown += (s, e) =>
            {
                // 메인 윈도우에 테이블 삽입
                _tableInserted = false;  // 새 테이블 삽입 허용
                _currentRows = popupRows;
                _currentCols = popupCols;
                InsertTable(_currentRows, _currentCols);
                _tableInserted = true;
                this.xSizeInfoText.Text = $"테이블 삽입됨: {_currentRows} x {_currentCols}";

                // 팝업 닫기
                _tablePreviewWindow.Close();
                _tablePreviewWindow = null;
            };

            // 팝업 표시
            _tablePreviewWindow.ShowDialog();
        }

        private void DrawTablePreview(int rows, int cols)
        {
            this.xTablePreviewCanvas.Children.Clear();

            // 배경 영역 그리기 (전체 가능 영역)
            Rectangle background = new Rectangle
            {
                Width = MaxCols * CellSize,
                Height = MaxRows * CellSize,
                Fill = Brushes.LightGray,
                Opacity = 0.3
            };
            this.xTablePreviewCanvas.Children.Add(background);
            Canvas.SetLeft(background, 0);
            Canvas.SetTop(background, 0);

            // 활성화된 셀 그리기
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Rectangle cell = new Rectangle
                    {
                        Width = CellSize - 1,
                        Height = CellSize - 1,
                        Fill = Brushes.SkyBlue,
                        Stroke = Brushes.DarkBlue,
                        StrokeThickness = 1
                    };
                    this.xTablePreviewCanvas.Children.Add(cell);
                    Canvas.SetLeft(cell, col * CellSize);
                    Canvas.SetTop(cell, row * CellSize);
                }
            }
        }

        private void InsertTable(int rows, int cols)
        {
            // 기존 테이블이 있으면 제거
            if (_insertedTable != null)
            {
                this.xTablePreviewCanvas.Children.Remove(_insertedTable);
            }

            // 새 테이블 생성
            _insertedTable = new Grid
            {
                Width = cols * CellSize,
                Height = rows * CellSize,
                Background = Brushes.White
            };

            // 행과 열 정의
            for (int i = 0; i < rows; i++)
            {
                _insertedTable.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int i = 0; i < cols; i++)
            {
                _insertedTable.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // 테이블 경계선 추가
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Border border = new Border
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1)
                    };

                    TextBlock textBlock = new TextBlock
                    {
                        Text = $"({row + 1},{col + 1})",
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = 10
                    };

                    border.Child = textBlock;
                    _insertedTable.Children.Add(border);
                    Grid.SetRow(border, row);
                    Grid.SetColumn(border, col);
                }
            }

            // 캔버스에 테이블 추가
            this.xTablePreviewCanvas.Children.Add(_insertedTable);
            Canvas.SetLeft(_insertedTable, 50); // 약간 오프셋을 주어 프리뷰와 구분되게 함
            Canvas.SetTop(_insertedTable, 50);
        }
    }
}