namespace SimpleMVVMWpf.Base
{
    using SimpleMVVMWpf.Common;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Constructor
        public BaseViewModel() { }
        #endregion  // Constructor

        private DelegateCommand<Object> _addCommand;
        public DelegateCommand<Object> AddCommand
        {
            get
            {
                return _addCommand ??
                       (_addCommand =
                        new DelegateCommand<Object>(
                            param => this.ExecuteAddCommand(param),
                            param => this.CanExecuteAddCommand()));
            }
        }

        private DelegateCommand<Object> _editCommand;
        public DelegateCommand<Object> EditCommand
        {
            get
            {
                return _editCommand ??
                       (_editCommand =
                        new DelegateCommand<Object>(
                            param => this.ExecuteEditCommand(param),
                            param => this.CanExecuteEditCommand()));
            }
        }

        private DelegateCommand<Object> _updateCommand;
        public DelegateCommand<Object> UpdateCommand
        {
            get
            {
                return _updateCommand ??
                       (_updateCommand =
                        new DelegateCommand<Object>(
                            param => this.ExecuteUpdateCommand(param),
                            param => this.CanExecuteUpdateCommand()));
            }
        }

        protected virtual void ExecuteAddCommand(object param)
        {
            //
        }

        protected virtual bool CanExecuteAddCommand()
        {
            return true;
        }

        protected virtual void ExecuteEditCommand(object param)
        {
            //
        }

        protected virtual bool CanExecuteEditCommand()
        {
            return true;
        }

        protected virtual void ExecuteUpdateCommand(object param)
        {
            //
        }

        protected virtual bool CanExecuteUpdateCommand()
        {
            return true;
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
