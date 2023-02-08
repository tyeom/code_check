using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace WpfApp15
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = new MainViewModel();
        }
    }

    public class TestModel
    {
        public string A { get; set; }
        public int B { get; set; }
    }

    public class MainViewModel
    {
        TaskService _taskService = new TaskService();
        private CancellationTokenSource _cancelToken;

        public MainViewModel()
        {
            // 초기화
            ModelList = new ObservableCollection<TestModel>()
            {
                new TestModel() {
                    A = System.IO.Path.GetRandomFileName(), B = new Random(DateTime.Now.Second).Next(10)
                }
            };
        }

        public ObservableCollection<TestModel> ModelList { get; set; }

        private RelayCommand<string> _taskStartCommand;
        public ICommand TaskStartCommand
        {
            get
            {
                return _taskStartCommand ??
                    (_taskStartCommand = new RelayCommand<string>(
                        async param =>
                        {
                            _cancelToken = new CancellationTokenSource();
                            var result = await Task.Run(() => _taskService.DoWork(_cancelToken));
                            foreach (var item in result)
                            {
                                ModelList.Add(item);

                                System.Windows.Application.Current.Dispatcher.Invoke(
                                    (System.Threading.ThreadStart)(() => { }), DispatcherPriority.ApplicationIdle);
                            }
                        }));
            }
        }

        private RelayCommand<string> _taskCancelCommand;
        public ICommand TaskCancelCommand
        {
            get
            {
                return _taskCancelCommand ??
                    (_taskCancelCommand = new RelayCommand<string>(
                        param =>
                        {
                            _cancelToken.Cancel();
                        }));
            }
        }
    }

    public class TaskService
    {
        public IEnumerable<TestModel> DoWork(CancellationTokenSource token)
        {
            while (token.IsCancellationRequested == false)
            {
                Thread.Sleep(2000);

                yield return new TestModel()
                {
                    A = System.IO.Path.GetRandomFileName(),
                    B = new Random(DateTime.Now.Second).Next(10)
                };
            }
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
