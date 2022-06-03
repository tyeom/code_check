using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace WpfApp1
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            this.DataContext = new WindowViewModel();
        }
    }

    public class WindowViewModel : INotifyPropertyChanged
    {
        private string _sampleText = "메모리 누수";

        ~WindowViewModel()
        {
            Console.WriteLine("파괴!");
        }

        //public string SampleText
        //{
        //    get => _sampleText;
        //    set => _sampleText = value;
        //}

        public string SampleText
        {
            get => _sampleText;
            set
            {
                _sampleText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SampleText)));
            }
        }

        private ICommand _click;
        public ICommand Click
        {
            get
            {
                return _click ?? (_click = new Command(() => { SampleText = null; }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Command : ICommand {
        private readonly Action _execute;
        public event EventHandler CanExecuteChanged;

        public Command(Action execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }
}
