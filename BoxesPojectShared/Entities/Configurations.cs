using BoxesPojectShared.Interfaces;

namespace BoxesPojectShared.Entities
{
    public class Configurations : IConfiguration
    {
        const int MAX_SPLITS = 3;
        const int MAX_COUNT = 100;
        const int MAX_SIZEDIFF = 2;
        const double SEARCH_INCREMENT = 0.1;
        const int MAX_DAYS_TO_STOCK_REFRESH = 3;
        const int NOTIFY_COUNT_THRESHOLD = 10;

        [Symbol("Units")]
        public int MaxCount => _maxCount;
        public int MaxSplits => _maxSplits;
        /// <summary>
        /// In Precentage
        /// </summary>
        [Symbol("%")]
        public double MaxSizeDiff => (_maxSizeDiff < 1 && _maxSizeDiff > 0 ? 1 + _maxSizeDiff : _maxSizeDiff) * 100;
        public double SearchIncrement => _searchIncrement;
        [Symbol("Days")]
        public int DaysToStockRefresh => _daysToStockRefresh;
        [Symbol("Units")]
        public int NotifyCountThreshold => _notifyCountThreshold;

        private int _maxCount = MAX_COUNT;
        private int _maxSplits = MAX_SPLITS;
        private double _maxSizeDiff = MAX_SIZEDIFF;
        private double _searchIncrement = SEARCH_INCREMENT;
        private int _daysToStockRefresh = MAX_DAYS_TO_STOCK_REFRESH;
        private int _notifyCountThreshold = NOTIFY_COUNT_THRESHOLD;

        public IConfiguration SetMaxSplits(int maxSplits)
        {
            _maxSplits = maxSplits;
            return this;
        }

        public IConfiguration SetMaxSizeDiff(double maxSizeDiff)
        {
            _maxSizeDiff = maxSizeDiff;
            return this;
        }

        public IConfiguration SetDaysToStockRefresh(int daysToStockRefresh)
        {
            _daysToStockRefresh = daysToStockRefresh;
            return this;
        }

        public IConfiguration SetSerchIncrement(double searchIncrement)
        {
            _searchIncrement = searchIncrement;
            return this;
        }

        public IConfiguration SetMaxCount(int maxCount)
        {
            _maxCount = maxCount;
            return this;
        }

        public IConfiguration SetNotifyCountThreshold(int notifyCountThreshold)
        {
            _notifyCountThreshold = notifyCountThreshold;
            return this;
        }
    }
}
