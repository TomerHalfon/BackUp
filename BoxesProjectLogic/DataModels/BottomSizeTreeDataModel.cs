using DataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesProjectLogic.DataModels
{
    /// <summary>
    /// The data model to be used in the main bts based on the x 
    /// </summary>
    public class BottomSizeTreeDataModel : IComparable
    {

        /// <summary>
        /// The Width and Length of the box
        /// </summary>
        internal double X { get; set; }

        /// <summary>
        /// The inner <c>BinarySearchTree</c> containing <c>HeightSizeTreeDataModel</c> data model
        /// </summary>
        internal BinarySearchTree<HeightSizeTreeDataModel> InnerTree { get; set; }

        #region Constructors

        /// <summary>
        /// Used for dummy data (for search terms)
        /// </summary>
        public BottomSizeTreeDataModel() { }
        /// <summary>
        /// Main Contstructor
        /// </summary>
        /// <param name="x"></param>
        public BottomSizeTreeDataModel(double x)
        {
            X = x;
            InnerTree = new BinarySearchTree<HeightSizeTreeDataModel>();
        } 
        #endregion


        //The Comparison
        public int CompareTo(object obj)
        {
            if (obj is BottomSizeTreeDataModel bottomSizeTreeData)
                return X.CompareTo(bottomSizeTreeData.X);
            else throw new ArgumentException($"Invalid {obj.GetType()} not {X.GetType()}");
        }
    }
}
