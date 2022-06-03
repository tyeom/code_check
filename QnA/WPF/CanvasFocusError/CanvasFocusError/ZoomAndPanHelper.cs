using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms.Design.Behavior;
using System.Windows.Input;
using System.Windows.Media;

namespace CanvasFocusError
{
    public class ZoomAndPanHelper : Behavior<ScrollViewer>
    {

        public Key PanningKey
        {
            get { return (Key)GetValue( PanningKeyProperty ); }
            set { SetValue( PanningKeyProperty, value ); }
        }
        public static readonly DependencyProperty PanningKeyProperty = DependencyProperty.Register( "PanningKey", typeof( Key ), typeof( ZoomAndPanHelper ), new UIPropertyMetadata( Key.None ) );
        public Key ZoomingKey
        {
            get { return (Key)GetValue( ZoomingKeyProperty ); }
            set { SetValue( ZoomingKeyProperty, value ); }
        }
        public static readonly DependencyProperty ZoomingKeyProperty = DependencyProperty.Register( "ZoomingKey", typeof( Key ), typeof( ZoomAndPanHelper ), new UIPropertyMetadata( Key.None ) );
        public Cursor PanningCursor
        {
            get { return (Cursor)GetValue( PanningCursorProperty ); }
            set { SetValue( PanningCursorProperty, value ); }
        }
        public static readonly DependencyProperty PanningCursorProperty = DependencyProperty.Register( "PanningCursor", typeof( Cursor ), typeof( ZoomAndPanHelper ), new UIPropertyMetadata( Cursors.SizeAll ) );


        public double ZoomRate
        {
            get { return (double)GetValue( ZoomRateProperty ); }
            set { SetValue( ZoomRateProperty, value ); }
        }
        public static readonly DependencyProperty ZoomRateProperty = DependencyProperty.Register( "ZoomRate", typeof( double ), typeof( ZoomAndPanHelper ), new UIPropertyMetadata( 1.0, null, CoerceZoomRate ) );
        public double MinimumZoomRate
        {
            get { return (double)GetValue( MinimumZoomRateProperty ); }
            set { SetValue( MinimumZoomRateProperty, value ); }
        }
        public static readonly DependencyProperty MinimumZoomRateProperty = DependencyProperty.Register( "MinimumZoomRate", typeof( double ), typeof( ZoomAndPanHelper ), new UIPropertyMetadata( 0.1 ) );
        public double MaximumZoomRate
        {
            get { return (double)GetValue( MaximumZoomRateProperty ); }
            set { SetValue( MaximumZoomRateProperty, value ); }
        }
        public static readonly DependencyProperty MaximumZoomRateProperty = DependencyProperty.Register( "MaximumZoomRate", typeof( double ), typeof( ZoomAndPanHelper ), new UIPropertyMetadata( 10.0 ) );

        private static object CoerceZoomRate( DependencyObject Target, object Value )
        {
            if (Value is double == false || Target is ZoomAndPanHelper == false) return Value;
            ZoomAndPanHelper ZoomAndPanning = Target as ZoomAndPanHelper;
            return ZoomAndPanning.GetCoerceZoomRateValue( (double)Value );
        }
        private double GetCoerceZoomRateValue( double ZoomRate )
        {
            if (ZoomRate < MinimumZoomRate) ZoomRate = MinimumZoomRate;
            else if (ZoomRate > MaximumZoomRate) ZoomRate = MaximumZoomRate;
            return ZoomRate;
        }
        protected override void OnPropertyChanged( DependencyPropertyChangedEventArgs e )
        {
            base.OnPropertyChanged( e );
            if (e.Property == MaximumZoomRateProperty || e.Property == MinimumZoomRateProperty)
            {
                ZoomRate = GetCoerceZoomRateValue( ZoomRate );
            }
        }

        private Point? LastCenterOfViewport { get; set; }
        private Point? LastContentMousePosition { get; set; }
        private Point? LastDragPoint { get; set; }
        protected bool CanPadding { get { return PanningKey == Key.None || Keyboard.IsKeyDown( PanningKey ); } }
        protected bool CanZooming { get { return PanningKey == Key.None || Keyboard.IsKeyDown( ZoomingKey ); } }
        protected FrameworkElement ContentElement { get { return AssociatedObject.Content as FrameworkElement; } }

        protected override void OnAttached()
        {
            AssociatedObject.ScrollChanged += OnScrollViewerScrollChanged;
            AssociatedObject.MouseLeftButtonUp += OnMouseLeftButtonUp;
            AssociatedObject.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
            AssociatedObject.PreviewMouseWheel += OnPreviewMouseWheel;
            AssociatedObject.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            AssociatedObject.MouseMove += OnMouseMove;

            DependencyPropertyDescriptor DPD = DependencyPropertyDescriptor.FromProperty( ScrollViewer.ContentProperty, typeof( ScrollViewer ) );
            DPD.AddValueChanged( AssociatedObject, ( s, e ) => OnContentChanged() );
            if (AssociatedObject.Content != null) OnContentChanged();

            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.ScrollChanged -= OnScrollViewerScrollChanged;
            AssociatedObject.MouseLeftButtonUp -= OnMouseLeftButtonUp;
            AssociatedObject.PreviewMouseLeftButtonUp -= OnMouseLeftButtonUp;
            AssociatedObject.PreviewMouseWheel -= OnPreviewMouseWheel;
            AssociatedObject.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
            AssociatedObject.MouseMove -= OnMouseMove;

            base.OnDetaching();
        }

        private void OnContentChanged()
        {
            if (ContentElement == null) return;

            ScaleTransform Transform = new ScaleTransform();
            BindingOperations.SetBinding( Transform, ScaleTransform.ScaleXProperty, new Binding( "ZoomRate" ) { Source = this } );
            BindingOperations.SetBinding( Transform, ScaleTransform.ScaleYProperty, new Binding( "ZoomRate" ) { Source = this } );
            ContentElement.LayoutTransform = Transform;
        }


        void OnMouseMove( object sender, MouseEventArgs e )
        {
            //if (ContentElement == null) return;
            if (/*CanPadding &&*/ LastDragPoint.HasValue)
            {
                Point Current = e.GetPosition( AssociatedObject );

                double dX = Current.X - LastDragPoint.Value.X;
                double dY = Current.Y - LastDragPoint.Value.Y;

                LastDragPoint = Current;

                AssociatedObject.ScrollToHorizontalOffset( AssociatedObject.HorizontalOffset - dX );
                AssociatedObject.ScrollToVerticalOffset( AssociatedObject.VerticalOffset - dY );
            }
        }

        void OnMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            if (ContentElement == null) return;
            if (CanPadding == false) return;

            Point Current = e.GetPosition( AssociatedObject );
            if (Current.X <= AssociatedObject.ViewportWidth && Current.Y < AssociatedObject.ViewportHeight) //make sure we still can use the scrollbars
            {
                AssociatedObject.Cursor = PanningCursor;
                LastDragPoint = Current;
                Mouse.Capture( AssociatedObject );
            }
        }
        void OnMouseLeftButtonUp( object sender, MouseButtonEventArgs e )
        {
            if (ContentElement == null) return;
            AssociatedObject.Cursor = Cursors.Arrow;
            AssociatedObject.ReleaseMouseCapture();
            LastDragPoint = null;
        }

        private Point GetContentMousePosition()
        {
            if (ContentElement == null) return new Point();
            Point ResultPoint = Mouse.GetPosition( ContentElement );
            return ResultPoint;
        }
        private Point GetCenterOfViewport()
        {
            if (ContentElement == null) return new Point();
            var centerOfViewport = new Point( AssociatedObject.ViewportWidth / 2, AssociatedObject.ViewportHeight / 2 );
            Point ResultPoint = AssociatedObject.TranslatePoint( centerOfViewport, ContentElement );
            return ResultPoint;
        }

        void OnPreviewMouseWheel( object sender, MouseWheelEventArgs e )
        {
            if (ContentElement == null) return;
            if (CanZooming == false) return;

            LastContentMousePosition = GetContentMousePosition();

            double ResultZoomRate = ZoomRate;

            double ScaleRate = (ZoomRate * 0.1) * (e.Delta < 0 ? -1 : 1);
            ResultZoomRate += ScaleRate;
            if (ResultZoomRate < 0.1) ResultZoomRate = 0.1;
            else if (ResultZoomRate > 10) ResultZoomRate = 10;

            ZoomRate = ResultZoomRate;

            LastCenterOfViewport = GetCenterOfViewport();

            e.Handled = true;
        }
        void OnScrollViewerScrollChanged( object sender, ScrollChangedEventArgs e )
        {
            if (ContentElement == null) return;

            if (e.ViewportWidthChange != 0 || e.ViewportHeightChange != 0)
            {
                LastCenterOfViewport = GetCenterOfViewport();
            }

            if (e.ExtentHeightChange != 0 || e.ExtentWidthChange != 0)
            {
                Point? Before = null;
                Point? Current = null;

                if (!LastContentMousePosition.HasValue)
                {
                    if (LastCenterOfViewport.HasValue)
                    {
                        Before = LastCenterOfViewport;
                        Current = GetCenterOfViewport();
                    }
                }
                else
                {
                    Before = LastContentMousePosition;
                    Current = GetContentMousePosition();

                    LastContentMousePosition = null;
                }

                if (Before.HasValue)
                {
                    double dXInTargetPixels = Current.Value.X - Before.Value.X;
                    double dYInTargetPixels = Current.Value.Y - Before.Value.Y;

                    double multiplicatorX = e.ExtentWidth / ContentElement.ActualWidth;
                    double multiplicatorY = e.ExtentHeight / ContentElement.ActualHeight;

                    double newOffsetX = AssociatedObject.HorizontalOffset - dXInTargetPixels * multiplicatorX;
                    double newOffsetY = AssociatedObject.VerticalOffset - dYInTargetPixels * multiplicatorY;

                    if (double.IsNaN( newOffsetX ) || double.IsNaN( newOffsetY )) return;

                    AssociatedObject.ScrollToHorizontalOffset( newOffsetX );
                    AssociatedObject.ScrollToVerticalOffset( newOffsetY );
                }
            }
            else
            {
                LastCenterOfViewport = GetCenterOfViewport();
            }
        }
    }
}
