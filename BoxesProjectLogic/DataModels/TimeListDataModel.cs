using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesProjectLogic.DataModels
{
    /// <summary>
    /// A data model that compares by the last time a customer perchuesd this size
    /// </summary>
    class TimeListDataModel : IComparable
    {
        /// <summary>
        /// The bottom of the box
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// The Height of the box
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// The last time this size was perchused
        /// </summary>
        public DateTime TimeLastPurchase { get; set; }
        public int CompareTo(object obj)
        {
            if (obj is TimeListDataModel comp)
            {
                return TimeLastPurchase.CompareTo(comp.TimeLastPurchase);
            }
            throw new ArgumentException($"Invalid obj");
        }
    }
}
