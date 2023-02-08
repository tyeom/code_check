using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TPL_DataFlowBroadCasting_Example.Models;

namespace TPL_DataFlowBroadCasting_Example.ViewModels;

public class StockInfoViewModel : INotifyPropertyChanged
{
    private StockInfoModel? _stockInfo;
    private double _percentChange;

    public StockInfoViewModel()
    {

    }

    public StockInfoModel? StockInfo
    {
        get => _stockInfo;
        set
        {
            _stockInfo = value;
            OnPropertyChanged();
        }
    }

    public double PercentChange
    {
        get => _percentChange;
        set
        {
            _percentChange = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 이전 현재가
    /// </summary>
    public double PreviousPrice
    {
        get;set;
    }

    public void UpdateStockInfo(StockInfoModel stockInfo)
    {
        StockInfo = stockInfo;
    }

    #region INotifyPropertyChange Implementation
    public event PropertyChangedEventHandler? PropertyChanged = delegate { };
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChangedEventHandler? handler = this.PropertyChanged;

        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    #endregion INotifyPropertyChange Implementation
}