using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace WpfApp3
{
    

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainViewModel();
        }

        public async Task<int> GetNumberAsync()
        {
            await Task.Delay(5000);
            return new Random().Next(0, 100);
        }

        public int GetNumber()
        {
            Thread.Sleep(5000);
            return new Random().Next(0, 100);
        }

        //public IEnumerable<int> GetNumbers()
        //{
        //    for (int i = 0; i < 100; i++)
        //    {
        //        int getNum = GetNumber();
        //        if (i == getNum)
        //        {
        //            yield return i;
        //        }
        //    }
        //}

        public IEnumerable<int> GetNumbers()
        {
            for (int i = 0; i < 9_999_999; i++)
            {
                yield return i;
            }
        }

        //List<int> _tmp = new List<int>();
        //public IEnumerable<int> GetNumbers()
        //{
        //    for (int i = 0; i < 9_999_999; i++)
        //    {
        //        _tmp.Add(i);
        //    }
        //    return _tmp;
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var numbers = this.GetNumbers();
            foreach (var num in numbers)
            {
                this.xTxtBlock.Text = num.ToString();
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);

            //Window1 win1 = new Window1();
            //win1.ShowDialog();
        }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            var menu1 = new MenuModel() { MenuName = "메뉴1", MenuCommand = MenuCommand };
            var menu2 = new MenuModel() { MenuName = "메뉴2", MenuCommand = MenuCommand };
            var menu3 = new MenuModel() { MenuName = "메뉴3", MenuCommand = MenuCommand };

            MenuList = new List<MenuModel>();
            MenuList.Add(menu1);
            MenuList.Add(menu2);
            MenuList.Add(menu3);
        }

        public List<MenuModel> MenuList { get; set; }  // 샘플이라서 그냥 이렇게 한건데 바인딩 속성 이렇게 하면 메모리 누수 위험 있음!!!!!!!!!

        private RelayCommand<MenuModel> _menuCommand;

        public ICommand MenuCommand
        {
            get
            {
                return _menuCommand ??
                    (_menuCommand = new RelayCommand<MenuModel>(
                        param => {
                            // TODO..
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

    public class MenuModel
    {
        public string MenuName { get; set; }
        public ICommand MenuCommand { get; set; }
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