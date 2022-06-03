namespace WpfApp13
{
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class ViewModelBase : ObservableObject
    {
        public ViewModelBase() {}

        public ViewModelBase(IView view)
        {
            View = view;
        }

        public IView View { get; private set; }
    }

    public abstract class ViewModelBase<T> : ObservableObject where T : IView
    {
        public ViewModelBase(T view)
        {
            View = view;
        }

        public T View { get; private set; }
    }
}
