namespace SimpleMVVMWpf.Common.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    public class MessagePopUpBox : UserControl
    {
        public enum EMessagePopUpBoxType
        {
            OK,
            YesNo,
            ConfirmDelete
        }

        public event RoutedEventHandler OKClick;
        public event RoutedEventHandler CancelClick;
        public event RoutedEventHandler DeleteClick;

        public MessagePopUpBox()
        {
            DefaultStyleKey = typeof(MessagePopUpBox);
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(MessagePopUpBox), new FrameworkPropertyMetadata(typeof(MessagePopUpBox)));
        }

        #region Control Dependency Properties
        public static readonly DependencyProperty MessagePopUpBoxTypeProperty =
          DependencyProperty.Register("MessagePopUpBoxType", typeof(EMessagePopUpBoxType), typeof(MessagePopUpBox));
        public EMessagePopUpBoxType MessagePopUpBoxType
        {
            get { return (EMessagePopUpBoxType)this.GetValue(MessagePopUpBoxTypeProperty); }
            set { this.SetValue(MessagePopUpBoxTypeProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
          DependencyProperty.Register("IsOpen", typeof(bool), typeof(MessagePopUpBox));
        public bool IsOpen
        {
            get { return (bool)this.GetValue(IsOpenProperty); }
            set { this.SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsBackgroundDisableProperty =
          DependencyProperty.Register("IsBackgroundDisable", typeof(bool), typeof(MessagePopUpBox), new PropertyMetadata(false));
        public bool IsBackgroundDisable
        {
            get { return (bool)this.GetValue(IsBackgroundDisableProperty); }
            set { this.SetValue(IsBackgroundDisableProperty, value); }
        }

        public static readonly DependencyProperty MsgBoxPlacementTargetProperty =
            DependencyProperty.Register(
                "MsgBoxPlacementTarget",
                typeof(UIElement),
                typeof(MessagePopUpBox),
                new FrameworkPropertyMetadata(
                    null, OnMsgBoxPlacementTargetPropertyChanged
                    ));
        public UIElement MsgBoxPlacementTarget
        {
            get { return (UIElement)this.GetValue(MsgBoxPlacementTargetProperty); }
            set { this.SetValue(MsgBoxPlacementTargetProperty, (object)value); }
        }

        public static readonly DependencyProperty VAlignmentProperty =
          DependencyProperty.Register("VAlignment", typeof(VerticalAlignment), typeof(MessagePopUpBox), new PropertyMetadata(VerticalAlignment.Bottom));
        public VerticalAlignment VAlignment
        {
            get { return (VerticalAlignment)this.GetValue(VAlignmentProperty); }
            set { this.SetValue(VAlignmentProperty, value); }
        }

        public static readonly DependencyProperty HAlignmentProperty =
          DependencyProperty.Register("HAlignment", typeof(HorizontalAlignment), typeof(MessagePopUpBox), new PropertyMetadata(HorizontalAlignment.Right));
        public HorizontalAlignment HAlignment
        {
            get { return (HorizontalAlignment)this.GetValue(HAlignmentProperty); }
            set { this.SetValue(HAlignmentProperty, value); }
        }
        #endregion  // Control Dependency Properties

        #region Command attached properties
        public ICommand CancelCommand
        {
            get
            {
                return (ICommand)GetValue(CancelCommandProperty);
            }
            set
            {
                SetValue(CancelCommandProperty, value);
            }
        }

        public object CancelCommandParameter
        {
            get { return (object)this.GetValue(CancelCommandParameterProperty); }
            set { this.SetValue(CancelCommandParameterProperty, value); }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return (ICommand)GetValue(DeleteCommandProperty);
            }
            set
            {
                SetValue(DeleteCommandProperty, value);
            }
        }

        public object DeleteCommandParameter
        {
            get { return (object)this.GetValue(DeleteCommandParameterProperty); }
            set { this.SetValue(DeleteCommandParameterProperty, value); }
        }

        public ICommand OKCommand
        {
            get
            {
                return (ICommand)GetValue(OKCommandProperty);
            }
            set
            {
                SetValue(OKCommandProperty, value);
            }
        }

        public object OKCommandParameter
        {
            get { return (object)this.GetValue(OKCommandParameterProperty); }
            set { this.SetValue(OKCommandParameterProperty, value); }
        }

        #endregion  // Command attached properties

        #region Command Dependency Properties

        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register(
                "CancelCommand",
                typeof(ICommand),
                typeof(MessagePopUpBox),
                new UIPropertyMetadata(null));

        public static readonly DependencyProperty CancelCommandParameterProperty =
            DependencyProperty.RegisterAttached(
                "CancelCommandParameter",
                typeof(object),
                typeof(MessagePopUpBox),
                new UIPropertyMetadata(null));

        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register(
                "DeleteCommand",
                typeof(ICommand),
                typeof(MessagePopUpBox),
                new UIPropertyMetadata(null));

        public static readonly DependencyProperty DeleteCommandParameterProperty =
            DependencyProperty.RegisterAttached(
                "DeleteCommandParameter",
                typeof(object),
                typeof(MessagePopUpBox),
                new UIPropertyMetadata(null));

        public static readonly DependencyProperty OKCommandProperty =
            DependencyProperty.Register(
                "OKCommand",
                typeof(ICommand),
                typeof(MessagePopUpBox),
                new UIPropertyMetadata(null));

        public static readonly DependencyProperty OKCommandParameterProperty =
            DependencyProperty.RegisterAttached(
                "OKCommandParameter",
                typeof(object),
                typeof(MessagePopUpBox),
                new UIPropertyMetadata(null));

        #endregion  // Command Dependency Properties

        private static void OnMsgBoxPlacementTargetPropertyChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MessagePopUpBox messagePopUpBox = d as MessagePopUpBox;
            messagePopUpBox.MsgBoxPlacementTarget.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(messagePopUpBox.OnClick), true);

            //MouseDevice mouseDevice = Mouse.PrimaryDevice;
            //MouseButtonEventArgs mouseButtonEventArgs = new MouseButtonEventArgs(mouseDevice, 0, MouseButton.Left);
            //mouseButtonEventArgs.RoutedEvent = Mouse.MouseDownEvent;
            //mouseButtonEventArgs.Source = messagePopUpBox.MsgBoxPlacementTarget;
            //messagePopUpBox.MsgBoxPlacementTarget.RaiseEvent(mouseButtonEventArgs);
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            // IsOpen = true;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Button cancelBtn = GetTemplateChild("xCancelBtn") as Button;
            cancelBtn.Click += new RoutedEventHandler(delegate (Object s, RoutedEventArgs e)
            {
                IsOpen = false;
                if (CancelClick != null)
                    CancelClick(this, new RoutedEventArgs());
            });

            Button deleteBtn = GetTemplateChild("xDeleteBtn") as Button;
            deleteBtn.Click += new RoutedEventHandler(delegate (Object s, RoutedEventArgs e)
            {
                IsOpen = false;
                if (DeleteClick != null)
                    DeleteClick(this, new RoutedEventArgs());
            });

            Button okBtn = GetTemplateChild("xOKBtn") as Button;
            okBtn.Click += new RoutedEventHandler(delegate (Object s, RoutedEventArgs e)
            {
                IsOpen = false;
                if (OKClick != null)
                    OKClick(this, new RoutedEventArgs());
            });

            Button yesBtn = GetTemplateChild("xYesBtn") as Button;
            yesBtn.Click += new RoutedEventHandler(delegate (Object s, RoutedEventArgs e)
            {
                IsOpen = false;
                if (OKClick != null)
                    OKClick(this, new RoutedEventArgs());
            });

            Button noBtn = GetTemplateChild("xNoBtn") as Button;
            noBtn.Click += new RoutedEventHandler(delegate (Object s, RoutedEventArgs e)
            {
                IsOpen = false;
                if (CancelClick != null)
                    CancelClick(this, new RoutedEventArgs());
            });
        }
    }
}
