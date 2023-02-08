using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ICustomTypeDescriptorEx;

public class MainViewModel
{
    ObservableCollection<DataModel> _dataModelList = new ObservableCollection<DataModel>();

    public MainViewModel() {
        var data1 = this.InitDataModel();
        var data2 = this.InitDataModel();
        var data3 = this.InitDataModel();
        var data4 = this.InitDataModel();

        data1.SetPropertyValue<string>("UserID", Guid.NewGuid().ToString());
        data1.SetPropertyValue<string>("UserName", "arong");
        data1.SetPropertyValue<bool>("UserChecked", true);

        data2.SetPropertyValue<string>("UserID", Guid.NewGuid().ToString());
        data2.SetPropertyValue<string>("UserName", "ming");
        data2.SetPropertyValue<bool>("UserChecked", true);

        data3.SetPropertyValue<string>("UserID", Guid.NewGuid().ToString());
        data3.SetPropertyValue<string>("UserName", "test1");
        data3.SetPropertyValue<bool>("UserChecked", false);

        data4.SetPropertyValue<string>("UserID", Guid.NewGuid().ToString());
        data4.SetPropertyValue<string>("UserName", "test2");
        data4.SetPropertyValue<bool>("UserChecked", false);

        
        _dataModelList.Add(data1);
        _dataModelList.Add(data2);
        _dataModelList.Add(data3);
        _dataModelList.Add(data4);
    }

    public ObservableCollection<DataModel> DataModelList
    {
        get { return _dataModelList; }
    }

    private DataModel InitDataModel()
    {
        DataModel dataModel = new();
        dataModel.AddProperty<string, DataModel>("UserID");
        dataModel.AddProperty<string, DataModel>("UserName");
        dataModel.AddProperty<bool, DataModel>("UserChecked");
        return dataModel;
    }

    private RelayCommand<Object>? _updatePropertyCommand;
    public ICommand UpdatePropertyCommand
    {
        get
        {
            return _updatePropertyCommand ??
                (_updatePropertyCommand = new RelayCommand<Object>(this.UpdatePropertyExecute));
        }
    }

    private void UpdatePropertyExecute(object param)
    {
        DataModelList[1].SetPropertyValue<bool>("UserChecked", false);
    }
}

internal class RelayCommand<T> : ICommand
{
    private readonly Action<T> _execute;

    public RelayCommand(Action<T> execute)
    {
        _execute = execute;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        T param = (T)parameter;
        _execute(param);
    }

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
}