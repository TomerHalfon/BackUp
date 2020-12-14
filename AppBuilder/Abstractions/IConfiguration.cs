using System;
using System.Collections.Generic;
using System.Text;

namespace AppBuilder.Abstractions
{
    public interface IConfiguration: IConfig
    {
        int MaxCount { get; }
        int MaxSplits { get; }
        int MaxSizeDiff { get; }
        double SearchIncrement { get; }
        int DaysToStockRefresh { get; }

        IConfiguration SetMaxCount(int maxCount);
        IConfiguration SetMaxSplits(int maxSplits);
        IConfiguration SetMaxSizeDiff(int maxSizeDiff);
        IConfiguration SetSerchIncrement(double searchIncrement);
        IConfiguration SetDaysToStockRefresh(int daysToStockRefresh);
    }
}
