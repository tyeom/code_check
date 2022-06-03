namespace WpfApp13
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PopViewModel : ViewModelBase
    {
        private ViewModelBase _popupVM;

        public ViewModelBase PopupVM
        {
            get => _popupVM;
            set => this.SetProperty(ref _popupVM, value);
        }
    }
}
