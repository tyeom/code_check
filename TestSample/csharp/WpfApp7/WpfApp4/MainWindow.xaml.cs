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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp4
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (sender, _) =>
            {
                TestModel a = new TestModel() { GroupName = "A" };
                TestModel b = new TestModel() { GroupName = "B" };
                TestModel c = new TestModel() { GroupName = "C" };

                List<TestModel> testModelList = new List<TestModel>();
                testModelList.Add(a);
                testModelList.Add(b);
                testModelList.Add(c);
                TestModelList = testModelList;

                this.DataContext = this;
            };
        }

        public string qqq1 { get; set; } = "11111";
        public string qqq2 { get; set; } = "222222";

        public List<TestModel> TestModelList { get; set; }

        public TestModel SelectedItem { get; set; }

        private RelayCommand<TestModel> _menuCommand;
        public ICommand MenuCommand
        {
            get
            {
                return _menuCommand ??
                    (_menuCommand = new RelayCommand<TestModel>(
                        param => {
                            MessageBox.Show($"현재 선택된 GroupName은 {(SelectedItem == null ? "[없음]" : SelectedItem.GroupName)}이고, 우 클릭으로 선택한 GroupName은 {param.GroupName}입니다!");
                        }));
            }
        }
    }

    public class TestModel
    {
        public string GroupName { get; set; }
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
