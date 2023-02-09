using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Transparent_Window
{
    /// <summary>
    /// DrawWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DrawWindow : Window
    {
        private Point _currentPoint = new Point();

        public DrawWindow()
        {
            InitializeComponent();
            
            this.Loaded += this.DrawWindow_Loaded;
            this.xCanvas.PreviewMouseLeftButtonDown += this.Canvas_PreviewMouseLeftButtonDown;
            this.xCanvas.PreviewMouseRightButtonDown += this.Canvas_PreviewMouseRightButtonDown;
            this.xCanvas.PreviewMouseMove += this.Canvas_PreviewMouseMove;
        }

        private void DrawWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AppSetting.Instance.IsShowDrawWindow = true;
        }

        private void Canvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                _currentPoint = e.GetPosition(this);
        }

        private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Line line = new Line();
                line.StrokeThickness = 3.5d;
                line.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFB6C1"));
                line.X1 = _currentPoint.X;
                line.Y1 = _currentPoint.Y;
                line.X2 = e.GetPosition(this).X;
                line.Y2 = e.GetPosition(this).Y;

                _currentPoint = e.GetPosition(this);

                this.xCanvas.Children.Add(line);
            }
        }

        private void Canvas_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            AppSetting.Instance.IsMouseEventMessagePass = true;

            this.SetWindowExTransparent();
        }

        public void SetWindowExTransparent()
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            WindowsServices.SetWindowExTransparent(hwnd);
        }

        public void Dispose()
        {
            this.Loaded -= this.DrawWindow_Loaded;
            this.xCanvas.PreviewMouseLeftButtonDown -= this.Canvas_PreviewMouseLeftButtonDown;
            this.xCanvas.PreviewMouseRightButtonDown += this.Canvas_PreviewMouseRightButtonDown;
            this.xCanvas.PreviewMouseMove -= this.Canvas_PreviewMouseMove;

            AppSetting.Instance.IsShowDrawWindow = false;
        }
    }
}
