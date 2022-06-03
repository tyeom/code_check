namespace SimpleMVVMWpf.ViewModels
{
    using SimpleMVVMWpf.Base;
    using SimpleMVVMWpf.Common;
    using SimpleMVVMWpf.Models;
    using SimpleMVVMWpf.Services;
    using System;
    using System.Windows;
    using System.Windows.Input;

    public class DataEditPageViewModel : BaseViewModel
    {
        private readonly IDBService _dbService;
        private bool _ageValidation = false;

        public DataEditPageViewModel(IDBService dbService)
        {
            _dbService = dbService;
        }

        public SampleModel EditSampleModel
        {
            get => (App.Current.Resources["ViewModelLocator"] as ViewModelLocator).MainViewModel.EditSampleModel;
        }

        public bool AgeValidation
        {
            get => _ageValidation;
            set
            {
                _ageValidation = value;
                OnPropertyChanged();
            }
        }

        public bool IsValidationSuccess
        {
            get;
            set;
        }

        public Func<bool> AgeValidationFunc
        {
            get => this.AgeVali;
        }

        #region Command Methods
        protected override void ExecuteUpdateCommand(object param)
        {
            AgeValidation = true;
        }
        #endregion // Command Methods

        private bool AgeVali()
        {
            if(EditSampleModel.Age <= 0)
            {
                MessageBox.Show("0보다 크게 입력해주세요!");
                return false;
            }
            else
            {
                (App.Current.Resources["ViewModelLocator"] as ViewModelLocator).MainViewModel.PopUpCloseCommand.Execute(null);
                return true;
            }
        }
    }
}
