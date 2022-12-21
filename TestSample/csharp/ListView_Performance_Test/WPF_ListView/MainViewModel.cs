namespace Performance_Test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class MainViewModel : INotifyPropertyChanged
    {
        private NotifyTaskCompletion<List<DataModel>?>? _dataList;

        public NotifyTaskCompletion<List<DataModel>?>? DataList
        {
            get => _dataList;
            set
            {
                _dataList = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand<Object>? _fetchDataCommand;
        public ICommand FetchDataCommand
        {
            get
            {
                return _fetchDataCommand ??
                    (_fetchDataCommand = new RelayCommand<Object>(this.FetchDataExecute));
            }
        }

        private void FetchDataExecute(object? param)
        {
            DataList = new(Task<List<DataModel>?>.Run(async () =>
            {
                var fetchDataList = await this.FetchData();
                //Thread.Sleep(5000);
                if(fetchDataList != null)
                {
                    List<DataModel> dataList = new(1000000);
                    // 데이터 10만개
                    for (int i = 0; i < 10000; i ++)
                    {
                        dataList.AddRange(fetchDataList);
                    }

                    return dataList;
                }
                else
                {
                    return null;
                }
            }), null);
        }

        private async Task<List<DataModel>?> FetchData()
        {
            HttpClient http = new();
            var dataList = await http.GetFromJsonAsync<List<DataModel>?>("http://arong.info:7003/posts/");

            return dataList;
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
