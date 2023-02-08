using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Input;
using TPL_DataFlowBroadCasting_Example.Models;

namespace TPL_DataFlowBroadCasting_Example.ViewModels;

public class MainViewModel
{
    private BroadcastBlock<List<StockInfoModel>> _broadcast;
    private StockInfoViewModel _stockInfo1VM = new();
    private StockInfoViewModel _stockInfo2VM = new();

    public MainViewModel()
    {
        // 브로드캐스트 생성
        _broadcast = new BroadcastBlock<List<StockInfoModel>>(null);
    }

    public StockInfoViewModel StockInfo1VM
    {
        get => _stockInfo1VM;
    }

    public StockInfoViewModel StockInfo2VM
    {
        get => _stockInfo2VM;
    }

    private AsyncCommand<Object>? _monitoringCommand;
    public ICommand MonitoringCommand
    {
        get
        {
            return _monitoringCommand ??
                (_monitoringCommand = new AsyncCommand<Object>(this.MonitoringExecute));
        }
    }

    private async Task MonitoringExecute(object param)
    {
        // 브로드캐스팅된 블록(주식정보)을 수신받아
        // 정보 표시 ViewModel에 데이터 전달 역할을 하는 블록 생성
        ActionBlock<List<StockInfoModel>> updateStockInfoStart = this.UpdateStockInfoStart();
        // 브로드캐스트에 ActionBlock 연결
        _broadcast.LinkTo(updateStockInfoStart);

        // 브로드캐스트에 추가 블록을 연결할 수 있다.
        ActionBlock<List<StockInfoModel>> foo = Foo();
        _broadcast.LinkTo(foo);

        // 브로드캐스팅으로 데이터를 수신받고
        // 가공된 데이터를 ViewModel에 전달 역할을 하는 블록 생성
        ActionBlock<Tuple<double, double>> updateChangeCalc = this.UpdateChangeCalc();
        var changeCalc = new TransformBlock<List<StockInfoModel>, Tuple<double, double>>(p =>
        {
            var stockInfo1_currPrice = p[0].CurrentPrice;
            var stockInfo2_currPrice = p[1].CurrentPrice;

            double percentChangeByItem1 = (stockInfo1_currPrice - StockInfo1VM.PreviousPrice) * 100 / StockInfo1VM.PreviousPrice;
            double percentChangeByItem2 = (stockInfo2_currPrice - StockInfo2VM.PreviousPrice) * 100 / StockInfo2VM.PreviousPrice;

            StockInfo1VM.PreviousPrice = stockInfo1_currPrice;
            StockInfo2VM.PreviousPrice = stockInfo2_currPrice;

            return new Tuple<double, double>(percentChangeByItem1, percentChangeByItem2);
        });

        // 브로드캐스트에 TransformBlock 연결
        _broadcast.LinkTo(changeCalc);
        // TransformBlock에 ActionBlock 연결
        changeCalc.LinkTo(updateChangeCalc);

        // 브로드캐스팅 시작
        StockInfoBroadCast stockInfoBroadCast = new(_broadcast);
        await stockInfoBroadCast.RealTimeData();
    }

    private ActionBlock<List<StockInfoModel>> UpdateStockInfoStart()
    {
        return new ActionBlock<List<StockInfoModel>>(p => {
            StockInfo1VM.UpdateStockInfo(p[0]);
            StockInfo2VM.UpdateStockInfo(p[1]);
        },
        new ExecutionDataflowBlockOptions()
        {
            TaskScheduler =
            TaskScheduler.FromCurrentSynchronizationContext()
        });
    }

    private ActionBlock<List<StockInfoModel>> Foo()
    {
        return new ActionBlock<List<StockInfoModel>>(p => { /* do something */ },
        new ExecutionDataflowBlockOptions()
            {
                TaskScheduler =
                TaskScheduler.FromCurrentSynchronizationContext()
            });
    }

    private ActionBlock<Tuple<double, double>> UpdateChangeCalc()
    {
        return new ActionBlock<Tuple<double, double>>(p =>
        {
            StockInfo1VM.PercentChange = p.Item1;
            StockInfo2VM.PercentChange = p.Item2;
        },
        new ExecutionDataflowBlockOptions()
        {
            TaskScheduler =
            TaskScheduler.FromCurrentSynchronizationContext()
        });
    }
}

internal class RelayCommand<T> : ICommand
{
    private readonly Action<T?> _execute;

    public RelayCommand(Action<T?> execute)
    {
        _execute = execute;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        T? param = (T?)parameter;
        _execute(param);
    }

    public event EventHandler? CanExecuteChanged
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