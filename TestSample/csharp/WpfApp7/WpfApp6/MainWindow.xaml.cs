namespace WpfApp6
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    public enum ETest
    {
        A,
        B,
        C,
        D,
        E
    }

    public class EnumBindingSourceExtension : MarkupExtension
    {
        private Type _enumType;

        /// <summary>
        /// 기본 생성자
        /// </summary>
        public EnumBindingSourceExtension() { }

        /// <summary>
        /// enum 타입 파라메터 생성자
        /// </summary>
        /// <param name="enumType"></param>
        public EnumBindingSourceExtension(Type enumType)
        {
            this.EnumType = enumType;
        }
                
        public Type EnumType
        {
            get { return this._enumType; }
            private set
            {
                if (value != this._enumType)
                {
                    if (null != value)
                    {
                        // null 타입 체크
                        Type enumType = Nullable.GetUnderlyingType(value) ?? value;
                        if (!enumType.IsEnum)
                            throw new ArgumentException("Enum 타입이 아닙니다!");
                    }

                    this._enumType = value;
                }
            }
        }

        public string Fillter
        {
            get; set;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (null == this._enumType)
                throw new InvalidOperationException("Enum 타입이 아닙니다!");

            Type enumType = Nullable.GetUnderlyingType(this._enumType) ?? this._enumType;
            Array enumValues = Enum.GetValues(enumType);

            if (enumType == this._enumType)
            {
                if (string.IsNullOrWhiteSpace(Fillter))
                {
                    return enumValues;
                }
                else
                {
                    var fillterArr = Fillter.ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    return enumValues.Cast<Enum>().Where(p => fillterArr.Contains(p.ToString()));
                }
            }

            {
                Array enumValArr = Array.CreateInstance(enumType, enumValues.Length + 1);
                enumValues.CopyTo(enumValArr, 1);

                if (string.IsNullOrWhiteSpace(Fillter))
                {
                    return enumValues;
                }
                else
                {
                    var fillterArr = Fillter.ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    return enumValues.Cast<Enum>().Where(p => fillterArr.Contains(p.ToString()));
                }
            }
        }
    }

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        

        private string _keyLog = null;
        private ETest _selectedETest = ETest.A;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        public ETest SelectedETest
        {
            get => _selectedETest;
            set
            {
                _selectedETest = value;
                OnPropertyChanged();
            }
        }

        public string KeyLog
        {
            get => _keyLog;
            set
            {
                _keyLog = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand<Key> _keyDownCommand;
        public ICommand KeyDownCommand
        {
            get
            {
                return _keyDownCommand ??
                    (_keyDownCommand = new RelayCommand<Key>(
                        param => {
                            KeyLog += ($"Key down event발생, key : {param.ToString()}\r\n");
                        }));
            }
        }


        private RelayCommand<Key> _keyUpCommand;
        public ICommand KeyUpCommand
        {
            get
            {
                return _keyUpCommand ??
                    (_keyUpCommand = new RelayCommand<Key>(
                        param => {
                            KeyLog += ($"Key up event발생, key : {param.ToString()}\r\n");
                        }));
            }
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

        private void UserControl1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            //e.Handled = true;
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
}
