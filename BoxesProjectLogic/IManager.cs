using BoxesPojectShared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoxesProjectLogic
{
    public interface IManager
    {
        event EventHandler<IEnumerable<Box>> OldBoxesDeletionEvent;
        DateTime DeletingAt { get; }
        DateTime DeletingOlderThan { get; }
        void AddToStock(double x, double y, int count);
        Box BuyBoxForPresent(double x, double y);
        bool BuyExactBoxSize(double x, double y, int count);
        void BuyMultipleBoxesForPresent(double x, double y, int count);
        Box GetBox(double x, double y);
        IEnumerable<Box> GetStock();
        void DeletionTimerCallBack(object state);
    }
}