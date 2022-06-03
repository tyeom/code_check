namespace SimpleMVVMWpf.Common
{
    using System;
    using System.Windows.Input;

    public class DelegateCommand<T> : ICommand
    {
        #region Member Fields
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;
        #endregion

        #region Construct
        public DelegateCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public DelegateCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion

        #region ICommand 구현
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

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            T param;
            if (parameter == null)
            {
                param = default(T);
            }
            else if (typeof(T).Name == "Object")
            {
                param = (T)parameter;
            }
            else if (parameter is T)
            {
                param = (T)Convert.ChangeType(parameter, typeof(T));
            }
            else
            {
                param = (T)parameter;
            }
            return _canExecute(param);

            //return _canExecute((parameter == null) ?
            //    default(T) : (T)Convert.ChangeType(parameter, typeof(T)));
        }

        //public void Execute(object parameter)
        //{
        //    _execute((parameter == null) ? default(T) : (T)Convert.ChangeType(parameter, typeof(T)));
        //}

        public void Execute(object parameter)
        {
            T param;
            if (parameter == null)
            {
                param = default(T);
            }
            else if (typeof(T).Name == "Object")
            {
                param = (T)parameter;
            }
            else if (parameter is T)
            {
                param = (T)Convert.ChangeType(parameter, typeof(T));
            }
            else
            {
                param = (T)parameter;
            }
            _execute(param);
            //_execute((parameter == null) ? default(T) : (T)Convert.ChangeType(parameter, typeof(T)));
        }
        #endregion
    }
}
