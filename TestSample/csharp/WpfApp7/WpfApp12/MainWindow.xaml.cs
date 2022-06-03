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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp12
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var lineViewModel = this.xLineView.DataContext as LineViewModel;

            var linePointsViewModel = new LinePointsViewModel() { LinePoints = new PointCollection(new Point[] { new Point(100, 350), new Point(200, 300), new Point(300, 400), new Point(400, 500) }) };
            lineViewModel.Lines.Add(linePointsViewModel);
        }
    }

    public class File : INotifyPropertyChanged
    {
        public string FileName { get; set; }
        public string Modified { get; set; }
        public string OriginUri { get; set; }
        public string Size { get; set; } //Double
        private int statusFilesize;
        public int StatusFilesize
        {
            get { return statusFilesize; }
            set
            {
                statusFilesize = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            List<File> selectedfiles = new List<File>();
            selectedfiles.Add(new File() { FileName = "aaa", StatusFilesize = 0 });
            selectedfiles.Add(new File() { FileName = "bbb", StatusFilesize = 0 });
            Selectedfiles = selectedfiles;
        }

        private List<File> _selectedfiles;
        public List<File> Selectedfiles
        {
            get => _selectedfiles;
            set
            {
                _selectedfiles = value;
                OnPropertyChanged();
            }
        }


        private RelayCommand<Object> _testCommand;

        public ICommand TestCommand
        {
            get
            {
                return _testCommand ??
                    (_testCommand = new RelayCommand<Object>(
                        param => {
                            // 방법 1.
                            //Selectedfiles = null;

                            //List<File> selectedfiles = new List<File>();
                            //selectedfiles.Add(new File() { FileName = "aaa", StatusFilesize = 0 });
                            //selectedfiles.Add(new File() { FileName = "bbb", StatusFilesize = 1 });

                            //Selectedfiles = selectedfiles;

                            // 방법 2.
                            Selectedfiles[1].StatusFilesize = 100;
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
