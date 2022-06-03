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
using System.Windows.Threading;

namespace WpfApp3
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            this.DataContext = new Window1ViewModel();
        }
    }

    public class Window1ViewModel : INotifyPropertyChanged
    {
        public Window1ViewModel()
        {
            DispatcherTimer tmr = new DispatcherTimer();
            tmr.Interval = new TimeSpan(0, 0, 3);
            tmr.Tick += (_, __) =>
            {
                tmr.Stop();
                IsClose = true;
            };
            tmr.Start();
        }

        private bool _isClose = false;
        public bool IsClose
        {
            get => _isClose;
            set
            {
                _isClose = value;
                OnPropertyChanged("IsClose");
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
}
