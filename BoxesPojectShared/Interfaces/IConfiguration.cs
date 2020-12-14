using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesPojectShared.Interfaces
{
    public interface IConfiguration
    {
        int MaxCount { get; }
        int MaxSplits { get; }
        double MaxSizeDiff { get; }
        double SearchIncrement { get; }
        int DaysToStockRefresh { get; }
        int NotifyCountThreshold { get; }

        IConfiguration SetMaxCount(int maxCount);
        IConfiguration SetMaxSplits(int maxSplits);
        IConfiguration SetMaxSizeDiff(double maxSizeDiff);
        IConfiguration SetSerchIncrement(double searchIncrement);
        IConfiguration SetDaysToStockRefresh(int daysToStockRefresh);
        IConfiguration SetNotifyCountThreshold(int notifyCountThreshold);
    }
}
