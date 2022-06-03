namespace SimpleMVVMWpf.ViewModels
{
    using SimpleMVVMWpf.Base;
    using SimpleMVVMWpf.Common;
    using SimpleMVVMWpf.Models;
    using SimpleMVVMWpf.Services;
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;

    public class MainViewModel : BaseViewModel
    {
        public enum EPopupPage
        {
            NONE,
            [ExtensionEnum("Views\\DataEditPage.xaml")]
            DataEditPagePopUp,
            [ExtensionEnum("Views\\APage.xaml")]
            APagePopUp,
            [ExtensionEnum("Views\\BPage.xaml")]
            BPagePopUp,
            [ExtensionEnum("Views\\CPage.xaml")]
            CPagePopUp,
            [ExtensionEnum("Views\\DPage.xaml")]
            DPagePopUp,
        }

        private readonly IDBService _dbService;
        private SampleModelList _dataList;
        private bool _isDefaultStyle = true;
        private bool _isMainPopUpOpen;
        private bool _isMsgPopup = false;
        private EPopupPage _popupPage = EPopupPage.NONE;

        public MainViewModel(IDBService dbService)
        {
            _dbService = dbService;
        }

        public EPopupPage PopupPage
        {
            get
            {
                return _popupPage;
            }
            set
            {
                _popupPage = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsMsgPopup
        {
            get
            {
                return _isMsgPopup;
            }
            set
            {
                _isMsgPopup = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// 서버 데이터 리스트 View 바인딩
        /// </summary>
        public SampleModelList DataList
        {
            get
            {
                if (_dataList == null)
                {
                    _dataList = new SampleModelList(_dbService);
                    Dispatcher dispatcher = null;
                    if (Application.Current != null)
                        dispatcher = Application.Current.Dispatcher;

                    dispatcher.InvokeAsync(async () =>
                    {
                        await _dataList.FillData();
                    });
                }

                return _dataList;
            }
        }

        public bool IsDefaultStyle
        {
            get => _isDefaultStyle;
            set
            {
                _isDefaultStyle = value;
                OnPropertyChanged();
            }
        }

        public bool IsMainPopUpOpen
        {
            get => _isMainPopUpOpen;
            set
            {
                _isMainPopUpOpen = value;
                this.OnPropertyChanged();
            }
        }

        public SampleModel EditSampleModel
        {
            get;
            private set;
        }

        #region Commands

        private DelegateCommand<Object> _styleChangeCommand;
        public ICommand StyleChangeCommand
        {
            get
            {
                return _styleChangeCommand ??
                    (_styleChangeCommand = new DelegateCommand<Object>(
                        param => this.ExecuteStyleChangeCommand(),
                        null));
            }
        }

        private DelegateCommand<Object> _popUpCloseCommand;
        public ICommand PopUpCloseCommand
        {
            get
            {
                return _popUpCloseCommand ??
                    (_popUpCloseCommand = new DelegateCommand<Object>(
                        param =>
                        {
                            if (IsMainPopUpOpen)
                            {
                                IsMainPopUpOpen = false;
                                PopupPage = EPopupPage.NONE;
                            }
                        },
                        null));
            }
        }

        private DelegateCommand<Object> _aPageCommand;
        public ICommand APageCommand
        {
            get
            {
                return _aPageCommand ??
                    (_aPageCommand = new DelegateCommand<Object>(
                        param => this.ExecuteAPageCommand(),
                        null));
            }
        }

        private DelegateCommand<Object> _bPageCommand;
        public ICommand BPageCommand
        {
            get
            {
                return _bPageCommand ??
                    (_bPageCommand = new DelegateCommand<Object>(
                        param => this.ExecuteBPageCommand(),
                        null));
            }
        }

        private DelegateCommand<Object> _cPageCommand;
        public ICommand CPageCommand
        {
            get
            {
                return _cPageCommand ??
                    (_cPageCommand = new DelegateCommand<Object>(
                        param => this.ExecuteCPageCommand(),
                        null));
            }
        }

        private DelegateCommand<Object> _dPageCommand;
        public ICommand DPageCommand
        {
            get
            {
                return _dPageCommand ??
                    (_dPageCommand = new DelegateCommand<Object>(
                        param => this.ExecuteDPageCommand(),
                        null));
            }
        }

        private DelegateCommand<Object> _showMsgCommand;
        public ICommand ShowMsgCommand
        {
            get
            {
                return _showMsgCommand ??
                    (_showMsgCommand = new DelegateCommand<Object>(
                        param =>
                        {
                            IsMsgPopup = true;
                        },
                        null));
            }
        }
        #endregion  // Commands

        #region Command Methods
        private void ExecuteStyleChangeCommand()
        {
            IsDefaultStyle = !IsDefaultStyle;
        }

        private void ExecuteAPageCommand()
        {
            PopupPage = EPopupPage.APagePopUp;
            IsMainPopUpOpen = true;
        }

        private void ExecuteBPageCommand()
        {
            PopupPage = EPopupPage.BPagePopUp;
            IsMainPopUpOpen = true;
        }

        private void ExecuteCPageCommand()
        {
            PopupPage = EPopupPage.CPagePopUp;
            IsMainPopUpOpen = true;
        }

        private void ExecuteDPageCommand()
        {
            PopupPage = EPopupPage.DPagePopUp;
            IsMainPopUpOpen = true;
        }

        protected override void ExecuteEditCommand(object param)
        {
            SampleModel sampleModel = param as SampleModel;
            EditSampleModel = sampleModel;

            PopupPage = EPopupPage.DataEditPagePopUp;
            IsMainPopUpOpen = true;
        }
        #endregion // Command Methods
    }
}
