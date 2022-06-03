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
using System.Windows.Threading;

namespace WpfApp14
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        private string _imgInfoText;
        public string ImgInfoText
        {
            get => _imgInfoText;
            set
            {
                _imgInfoText = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _imageSource;
        public BitmapImage ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand<Object> _renderCompletedCommand;
        public ICommand RenderCompletedCommand
        {
            get
            {
                return _renderCompletedCommand ??
                    (_renderCompletedCommand = new RelayCommand<Object>(
                        param => {
                            ImgInfoText = $"Width : {this.xImage.ActualWidth} / Height : {this.xImage.ActualHeight}";
                        }));
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
                            //ImageSource = new BitmapImage(new Uri("/WpfApp14;component/Images/birthday_img_placeholder.png", UriKind.RelativeOrAbsolute));

                            //this.xImage.Dispatcher.Invoke(
                            //    (System.Threading.ThreadStart)(() => { }), DispatcherPriority.ApplicationIdle);

                            //this.xImgInfoText.Text =
                            //$"Width : {this.xImage.ActualWidth} / Height : {this.xImage.ActualHeight}";



                            //this.xImage.Dispatcher.BeginInvoke(
                            //    (System.Threading.ThreadStart)(() => {
                            //        ImgInfoText = $"Width : {this.xImage.ActualWidth} / Height : {this.xImage.ActualHeight}";
                            //    }), DispatcherPriority.ApplicationIdle);

                            ImageSource = new BitmapImage(new Uri("/WpfApp14;component/Images/birthday_img_placeholder.png", UriKind.RelativeOrAbsolute));
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

        private void xImage_TargetUpdated(object sender, DataTransferEventArgs e)   
        {
            this.xImgInfoText.Text =
                $"Width : {this.xImage.ActualWidth} / Height : {this.xImage.ActualHeight}";
        }

        private void xImage_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            this.xImgInfoText.Text =
                $"Width : {this.xImage.ActualWidth} / Height : {this.xImage.ActualHeight}";
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
