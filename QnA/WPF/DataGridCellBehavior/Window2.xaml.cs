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
using System.Windows.Shapes;

namespace WpfApp5
{
    /// <summary>
    /// Window2.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Window2 : Window, INotifyPropertyChanged
    {
        private string _row_column = null;

        public Window2()
        {
            InitializeComponent();

            this.DataContext = this;

            List<AModel> aList = new List<AModel>();
            aList.Add(new AModel() { Name = "test1", Age = 11 });
            aList.Add(new AModel() { Name = "test2", Age = 22 });
            aList.Add(new AModel() { Name = "test3", Age = 33 });

            List<BModel> bList = new List<BModel>();
            bList.Add(new BModel() { Status = "Nomal", IsCheck = true });
            bList.Add(new BModel() { Status = "Hard", IsCheck = false });
            bList.Add(new BModel() { Status = "Mid", IsCheck = true });

            AList = aList;
            BList = bList;
        }

        public string Row_column
        {
            get => _row_column;
            set
            {
                _row_column = value;
                OnPropertyChanged();
            }

        }

        public List<AModel> AList
        {
            get;set;
        }

        public List<BModel> BList
        {
            get; set;
        }

        private RelayCommand<string> _selectedCellsChangedCommand;
        public ICommand SelectedCellsChangedCommand
        {
            get
            {
                return _selectedCellsChangedCommand ??
                    (_selectedCellsChangedCommand = new RelayCommand<string>(
                        param => this.ExecuteSelectedCellsChanged(param)));
            }
        }

        private void ExecuteSelectedCellsChanged(string param)
        {
            Row_column = param;
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

    public class AModel
    {
        public string Name
        {
            get;set;
        }

        public int Age
        {
            get; set;
        }
    }

    public class BModel
    {
        public string Status
        {
            get; set;
        }

        public bool IsCheck
        {
            get; set;
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
