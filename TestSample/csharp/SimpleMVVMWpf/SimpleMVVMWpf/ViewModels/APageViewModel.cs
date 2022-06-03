namespace SimpleMVVMWpf.ViewModels
{
    using SimpleMVVMWpf.Base;
    using SimpleMVVMWpf.Services;
    using System;

    public class APageViewModel : BaseViewModel
    {
        private readonly IDBService _dbService;

        public APageViewModel(IDBService dbService, Test111 a)
        {
            a.a = "11111";
            _dbService = dbService;
        }

        ~APageViewModel()
        {
            //
        }
    }
}
