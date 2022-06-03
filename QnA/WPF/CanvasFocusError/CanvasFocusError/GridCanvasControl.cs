namespace CanvasFocusError
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    [TemplatePart(Name = CanvasPartName, Type = typeof(Canvas))]
    internal class GridCanvasControl : ContentControl
    {
        private const string CanvasPartName = "PART_Canvas";
        private Canvas _canvasPart;

        public GridCanvasControl()
        {
            this.DefaultStyleKey = typeof(GridCanvasControl);
        }

        public bool CanvasVisibility
        {
            get { return (bool)base.GetValue(CanvasVisibilityProperty); }
            set { base.SetValue(CanvasVisibilityProperty, value); }
        }

        public static readonly DependencyProperty CanvasVisibilityProperty =
          DependencyProperty.Register("CanvasVisibility", typeof(bool), typeof(GridCanvasControl), new UIPropertyMetadata(true));

        public VisualBrush CanvasBackground
        {
            get { return (VisualBrush)base.GetValue(CanvasBackgroundProperty); }
            set { base.SetValue(CanvasBackgroundProperty, value); }
        }

        public static readonly DependencyProperty CanvasBackgroundProperty =
          DependencyProperty.Register("CanvasBackground", typeof(VisualBrush), typeof(GridCanvasControl), new UIPropertyMetadata(null));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _canvasPart = GetTemplateChild(CanvasPartName) as Canvas;

            if (_canvasPart is null) return;
            _canvasPart.MouseDown += this.CanvasPart_MouseDown;
        }

        private void CanvasPart_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var cdn = Mouse.GetPosition(_canvasPart);
        }
    }
}
