using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TPL_DataFlowBroadCasting_Example.Models;

public class StockInfoModel
{
    public string? StockName
    {
        get; set;
    }

    public double StartPrice
    {
        get; set;
    }

    public double CurrentPrice
    {
        get;set;
    }
}