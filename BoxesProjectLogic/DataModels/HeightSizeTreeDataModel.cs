using DataStructures;
using System;
using System.Configuration;

namespace BoxesProjectLogic.DataModels
{
    /// <summary>
    /// The data model to be used in the inner bts inside the x node
    /// </summary>
   public class HeightSizeTreeDataModel : IComparable
    {
        

        /// <summary>
        /// The Max Count of an item
        /// </summary>
        internal static int MaxCount { get; set; }


        /// <summary>
        /// The Height of the Box
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// The Box ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Count of the boxes for this size
        /// </summary>
        public int Count
        {
            get => _count;
            set =>
                //keep below max
                _count = Math.Min(value, MaxCount);
        }

        private int _count;

        /// <summary>
        /// A reference to the Date node
        /// </summary>
        public Linked_List<TimeListDataModel>.Node DateNode { get; set; }

        #region Constructors
        /// <summary>
        /// For Dummy Data
        /// </summary>
        /// <param name="y"></param>
        public HeightSizeTreeDataModel(double y)
        {
            Y = y;
        }

        /// <summary>
        /// Main Constructor
        /// </summary>
        /// <param name="y"></param>
        /// <param name="count"></param>
        public HeightSizeTreeDataModel(double y, int count) : this(y)
        {
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