using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TPL_DataFlowBroadCasting_Example.Models;

namespace TPL_DataFlowBroadCasting_Example;

public class StockInfoBroadCast
{
    private readonly BroadcastBlock<List<StockInfoModel>> _stockInfoBroadCaster;

    public StockInfoBroadCast(BroadcastBlock<List<StockInfoModel>> broadcaster)
    {
        _stockInfoBroadCaster = broadcaster;
    }

    public async Task RealTimeData()
    {
        var random1 = new Random();
        var random2 = new Random();

        StockInfoModel stockInfo1 = new()
        {
            StockName = "삼성전자",
            StartPrice = random1.Next(1000, 10000)
        };

        StockInfoModel stockInfo2 = new()
        {
            StockName = "삼성전자우",
            StartPrice = random2.Next(1000, 10000)
        };

        for (int i = 0; i < 100; i++)
        {
            await Task.Delay(1200);
            stockInfo1.CurrentPrice = random1.Next(1000, 10000);
            stockInfo2.CurrentPrice = random2.Next(1000, 10000);
            //_stockInfoBroadCaster.Post(new() { stockInfo1, stockInfo2 });
            await _stockInfoBroadCaster.SendAsync(new() { stockInfo1, stockInfo2 });
        }
        _stockInfoBroadCaster.Complete();
        try
        {
            _stockInfoBroadCaster.Completion.Wait();
        }
        catch (Exception ex)
        {
            // 데이터흐름 완료 오류
        }
    }
}