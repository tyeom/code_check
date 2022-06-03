namespace CanvasFocusError
{
    using Prism.Commands;
    using Prism.Mvvm;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    public class MainWindowViewModel : BindableBase
    {
        private double _zoomValue;
        private DelegateCommand _clickCommand;
        private bool _visible = true;
        private VisualBrush _canvasBackground;

        public MainWindowViewModel()
        {
            this.Init();
        }

        public VisualBrush CanvasBackground
        {
            get { return _canvasBackground; }
            set { SetProperty(ref this._canvasBackground, value); }
        }

        public bool Visible
        {
            get { return _visible; }
            set { SetProperty(ref this._visible, value); }
        }

        public double ZoomValue
        {
            get { return _zoomValue; }
            set { SetProperty(ref _zoomValue, value); }
        }

        public DelegateCommand ClickCommand => _clickCommand ?? (_clickCommand = new DelegateCommand(ExecuteClickCommand));

        private void ExecuteClickCommand()
        {
            this.ZoomValue = new Random().NextDouble() * 10;
        }

        internal void Init()
        {
            CanvasBackground = new VisualBrush
            {
                TileMode = TileMode.Tile,
                Stretch = Stretch.Uniform,
                Viewport = new System.Windows.Rect(20, 20, 20, 20),
                ViewportUnits = BrushMappingMode.Absolute,
                Visual = new Rectangle { Width = 20, Height = 20, Fill = Brushes.Transparent, Stroke = Brushes.Gray, StrokeThickness = 0.1/*, Visibility = IsVisible*/ } //Visible Binding은 어떻게 해야 할까 ?
            };
        }
    }
}
