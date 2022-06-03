using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    //public class Pre_ActionCommandBehavior : DependencyObject
    //{
    //    public static readonly DependencyProperty CommandProperty =
    //        DependencyProperty.RegisterAttached("Command", typeof(Action<Object>), typeof(Pre_ActionCommandBehavior),
    //            new UIPropertyMetadata(true, null,
    //                (o, e) =>
    //                {
    //                    ButtonBase button = o as ButtonBase;
    //                    button. .Command = new RelayCommand<Object>(GetCommand)


    //                    return e;
    //                }));

    //    public static Action<Object> GetCommand(DependencyObject obj)
    //    {
    //        return (Action<Object>)obj.GetValue(CommandProperty);
    //    }

    //    public static void SetCommand(DependencyObject obj, Action<Object> value)
    //    {
    //        obj.SetValue(CommandProperty, value);
    //    }

    //    public static readonly DependencyProperty PreActionCommandProperty =
    //        DependencyProperty.RegisterAttached("PreActionCommand", typeof(Action), typeof(Pre_ActionCommandBehavior), null);

    //    public static Action GetStretch(DependencyObject obj)
    //    {
    //        return (Action)obj.GetValue(PreActionCommandProperty);
    //    }

    //    public static void SetStretch(DependencyObject obj, Action value)
    //    {
    //        obj.SetValue(PreActionCommandProperty, value);
    //    }
    //}


    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class MyAttribute : Attribute
    {
        private string _actionName;
        public MyAttribute(string actionName)
        {
            _actionName = actionName;
        }

        public string ActionName
        {
            get => _actionName;
        }
    }

    internal class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            T param = (T)parameter;
            _execute(param);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }

    public class ViewModelBase
    {
        private ICommand _defaultActionCommand;
        public ICommand DefaultActionCommand
        {
            get
            {
                return _defaultActionCommand ??
                    (_defaultActionCommand = new RelayCommand<Object>(this.DefaultActionExecute));
            }
        }

        protected virtual void DefaultActionExecute(Object param)
        {
            //
        }

        protected void CallDefaultAction(MethodBase method)
        {
            MyAttribute attr = (MyAttribute)method.GetCustomAttributes(typeof(MyAttribute), true)[0];
            string a = attr.ActionName;
        }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChange Implementation
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChange Implementation

        public MainViewModel()
        {
            aa = System.Windows.Media.Brushes.Red;

            System.Windows.Media.Color vvv = new Color();
            vvv.R = 255;
            vvv.G = 100;
            vvv.B = 50;

            bb = vvv;
        }

        private ICommand _defaultActionCommand;
        public ICommand DefaultActionCommand
        {
            get
            {
                return _defaultActionCommand ??
                    (_defaultActionCommand = new RelayCommand<Object>(this.DefaultActionExecute));
            }
        }

        [My("CommandPre_Action")]
        private void DefaultActionExecute(object param)
        {
            MessageBox.Show("테스트!");
        }

        public SolidColorBrush aa
        {
            get; set;
        }

        private System.Windows.Media.Color _bb;
        public System.Windows.Media.Color bb
        {
            get => _bb;
            set
            {
                _bb = value;
                OnPropertyChanged();
            }
        }

    }

    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Media.Color vvv = new System.Windows.Media.Color();
            vvv.R = 255;
            vvv.G = 100;
            vvv.B = 50;

            return vvv;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer _dispatcherTimer;
        Queue<WeakReference> _list = new Queue<WeakReference>();

        public MainWindow()
        {
            InitializeComponent();

            //_dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            //_dispatcherTimer.Tick += _dispatcherTimer_Tick;
            //_dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            //_dispatcherTimer.Start();

            this.DataContext = new MainViewModel();
        }

        #region INotifyPropertyChange Implementation
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChange Implementation
        private int aaa;
        public int AAA
        {
            get
            {
                return aaa;
            }
            set
            {
                aaa = value;
                OnPropertyChanged();
            }
        }

        private void _dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Queue<WeakReference> copyList = new Queue<WeakReference>();

            while (_list.Count > 0)
            {
                WeakReference wr = _list.Dequeue();
                if (wr.Target != null)
                {
                    copyList.Enqueue(wr);
                }
            }

            _list = copyList;
            System.Diagnostics.Trace.WriteLine(_list.Count);

            GC.Collect(2, GCCollectionMode.Forced);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1 win = new Window1();
            win.Show();

            WeakReference wr = new WeakReference(win);
            _list.Enqueue(wr);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("aaa");
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string str = new TextRange(xRichTextBox2.Document.ContentStart, xRichTextBox2.Document.ContentEnd).Text;
            this.xRichTextBox.Document.Blocks.Clear();
            this.xRichTextBox.Document.Blocks.Add(new Paragraph(new Run(str)));
        }
    }
}
