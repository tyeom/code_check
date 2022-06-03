namespace WpfApp13
{
    using Microsoft.Toolkit.Mvvm.Input;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class MainViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        public MainViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        //public MainViewModel(IMainView view) :
        //    base(view)
        //{

        //}

        private List<FooModel> _dataList = new List<FooModel>(
            new FooModel[] {
                new FooModel() { Name = "테스트1", Description = "asdfsd" },
                new FooModel() { Name = "테스트2", Description = "sdfsdddddd" },
                new FooModel() { Name = "테스트3", Description = "5555555" }
            }
            );
        public List<FooModel> DataList
        {
            get => _dataList;
            set => _dataList = value;
        }

        
        private RelayCommand<Object> _rowClickCommand;
        public RelayCommand<Object> RowClickCommand
        {
            get
            {
                return _rowClickCommand ??
                    (_rowClickCommand = new RelayCommand<Object>(
                        (item) =>
                        {
                            
                        },
                        null));
            }
        }

        private RelayCommand _popUpCommand;

        public RelayCommand PopUpCommand
        {
            get
            {
                //return _popUpCommand ??
                //    (_popUpCommand = new RelayCommand(
                //        () =>
                //        {
                //            base.View.ShowPopupWindow();
                //        },
                //        null));


                return _popUpCommand ??
                    (_popUpCommand = new RelayCommand(
                        () =>
                        {
                            _dialogService.SetVM(new Popup1ViewModel());
                            _dialogService.Dialog.ShowDialog();
                        },
                        null));
            }
        }
    }

    public class FooModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
