namespace WpfApp13
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DialogService : IDialogService
    {
        private IDialog _popWindow;

        public DialogService()
        {
            _popWindow = App.Current.Services.GetService(typeof(PopWindow)) as IDialog;
        }

        public IDialog Dialog => _popWindow;

        public void SetVM(ViewModelBase vm)
        {
            if(_popWindow.DataContext is PopViewModel viewModel)
            {
                viewModel.PopupVM = vm;
            }
        }
    }
}
