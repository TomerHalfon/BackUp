using System;
using System.Configuration;

namespace BoxesProjectLogic.DataModels
{
    /// <summary>
    /// The data model to be used in the inner bts inside the x node
    /// </summary>
     class HeightSizeTreeDataModel : IComparable
    {
        /// <summary>
        /// default max count
        /// </summary>
        const int DEFAULT_MAX_COUNT = 10000;
        /// <summary>
        /// The Max Count of an item
        /// </summary>
        public int MaxCount => DEFAULT_MAX_COUNT;

        /// <summary>
        /// The Height of the Box
        /// </summary>
        public double Y { get; set; }
        private int _count;
        /// <summary>
        /// The Count of the boxes for this size
        /// </summary>
        public int Count
        {
            get { return _count; }
            set
            { 
                //keep below max
                _count = Math.Min(value, MaxCount); 
            }
        }
        #region Constructors
        /// <summary>
        /// For Dummy Data
        /// </summary>
        public HeightSizeTreeDataModel()
        {
        }
        /// <summary>
        /// Main Constructor
        /// </summary>
        /// <param name="y"></param>
        /// <param name="count"></param>
        public HeightSizeTreeDataModel(double y, int count)
        {
            Y = y;
            Count = count;
        }
        #endregion
        public int CompareTo(object obj)
        {
            if (obj is HeightSizeTreeDataModel heightSizeTreeData)
                return Y.CompareTo(heightSizeTreeData.Y);
            else throw new ArgumentException($"Invalid {obj.GetType()} not {Y.GetType()}");
        }
    }
}