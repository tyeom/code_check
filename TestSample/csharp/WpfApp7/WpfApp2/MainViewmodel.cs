namespace WpfApp2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows.Input;

    internal class MainViewmodel : INotifyPropertyChanged
    {
        private bool _showMessagePopup;
        private string _selectedMenuDesc;

        public MainViewmodel()
        {
            this.Init();
        }

        public List<MenuModel> MenuList
        {
            get;
            set;
        }

        public string SelectedMenuDesc
        {
            get => _selectedMenuDesc;
            set
            {
                if(_selectedMenuDesc != value)
                {
                    _selectedMenuDesc = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ShowMessagePopup
        {
            get => _showMessagePopup;
            set
            {
                if (_showMessagePopup != value)
                {
                    _showMessagePopup = value;
                    OnPropertyChanged();
                }
            }
        }

        private RelayCommand<string> _showDescMsgPopup;

        public ICommand ShowDescMsgPopup
        {
            get
            {
                return _showDescMsgPopup ??
                    (_showDescMsgPopup = new RelayCommand<string>(
                        param => {
                            SelectedMenuDesc = param;
                            ShowMessagePopup = true;
                        }));
            }
        }

        private async void Init()
        {
            MenuModel menuModel = new MenuModel();
            MenuList = await menuModel.GetDataAsync();
            OnPropertyChanged("MenuList");
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
