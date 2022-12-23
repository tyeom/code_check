using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TPL_DataFlowBroadCasting_Example;

public interface IAsyncCommand<T> : ICommand
{
    Task ExecuteAsync(T parameter);
    bool CanExecute(T parameter);
}

public class AsyncCommand<T> : IAsyncCommand<T>
{
    public event EventHandler CanExecuteChanged;

    private bool _isExecuting;
    private readonly Func<T, Task> _execute;
    private readonly Func<T, bool> _canExecute;

    public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(T parameter)
    {
        return !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);
    }

    public async Task ExecuteAsync(T parameter)
    {
        if (CanExecute(parameter))
        {
            try
            {
                _isExecuting = true;
                await _execute(parameter);
            }
            finally
            {
                _isExecuting = false;
            }
        }

        RaiseCanExecuteChanged();
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    #region IAsyncCommand<T> 구현
    bool ICommand.CanExecute(object parameter)
    {
        return CanExecute((T)parameter);
    }

    async void ICommand.Execute(object parameter)
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

        await ExecuteAsync(param);
    }
    #endregion  // IAsyncCommand<T> 구현
}