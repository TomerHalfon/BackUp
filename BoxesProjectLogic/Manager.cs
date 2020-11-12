using BoxesProjectLogic.DataModels;
using DataStructures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using BoxesProjectData;
using BoxesPojectShared.Entities;

namespace BoxesProjectLogic
{
    public class Manager
    {
        public enum PrintOrder
        {
            BySizeAsc,
            ByDateAsc
        }

        /// <summary>
        /// Search the sizes based on the X dimention and than by the Y dimention.
        /// allows for Log(n) operations
        /// </summary>
        BinarySearchTree<BottomSizeTreeDataModel> MainTree { get; set; }
        /// <summary>
        /// This List is sorted by the time of the last perchuess for each box size.
        /// you can pull from a box size from here based on the last time a customer bought this size
        /// </summary>
        DuelLinkeLinkedList<TimeListDataModel> DataByTime { get; set; }

        public Manager()
        {
            MainTree = new BinarySearchTree<BottomSizeTreeDataModel>();
            DataByTime = new DuelLinkeLinkedList<TimeListDataModel>();
            //load data
            LoadDataFromDB();
        }

        /// <summary>
        /// Add Boxes to the stock.
        /// if the box exist will only update the Count
        /// if the box doesn't exist will create a new record
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="count"></param>
        /// <param name="timeLastPurchase"></param>
        public void AddToStock(double x, double y, int count, DateTime? timeLastPurchase = null, bool updateDB = true)
        {
            //Get the mainTreeData for the search term from the main tree
            var mainTreeSearchTerm = new BottomSizeTreeDataModel() { X = x };
            MainTree.FindAndUpdate(mainTreeSearchTerm, out BottomSizeTreeDataModel mainTreeData);

            //Get the innerTreeData for the searchTerm in the inner tree
            var innerTreeSearchTerm = new HeightSizeTreeDataModel() { Y = y };
            if (mainTreeData.InnerTree is null) mainTreeData.InnerTree = new BinarySearchTree<HeightSizeTreeDataModel>();
            mainTreeData.InnerTree.FindAndUpdate(innerTreeSearchTerm, out HeightSizeTreeDataModel innerTreeData);

            //Set the Properties
            innerTreeData.Count += count;

            //Add the Data to the DataByTime, if there is no date data use date time now
            DataByTime.AddInOrder(new TimeListDataModel() { X = x, Y = y, TimeLastPurchase = timeLastPurchase ?? DateTime.Now });

            //Add Data To DB
            if(updateDB) SaveToDB(x, y, innerTreeData.Count, timeLastPurchase);
        }

        /// <summary>
        /// Takes All Data and saves a new entry
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="count"></param>
        /// <param name="timeLastPurchase"></param>
        private void SaveToDB(double x, double y, int count, DateTime? timeLastPurchase)
        {
            using (DataAccess dal = new DataAccess())
            {
                //Add the data or if exist update only
                dal.AddOrUpdatae(
                    new Box()
                    {
                        X = x,
                        Y = y,
                        Count = count,
                        TimeLastPurchase = timeLastPurchase
                    }
                    );
            }
        }

        /// <summary>
        /// Get a string of the Stock by a type of order
        /// </summary>
        /// <param name="order">by what comperiosn to sort</param>
        /// <returns></returns>
        public async Task<string> PrintStock(PrintOrder order)
        {
            StringBuilder sb = new StringBuilder($"The Stock\n{new string('-', 9)}\n");
            switch (order)
            {
                case PrintOrder.BySizeAsc:
                    MainTree.TraverseInOrder(x => x.InnerTree.TraverseInOrder(y => sb.AppendLine($"X => {x.X}, Y => {y.Y}, Count => {y.Count}")));
                    break;
                case PrintOrder.ByDateAsc:
                    DataByTime.ToList().ForEach(d => sb.AppendLine($"X => {d.X}, Y => {d.Y}, Date => {d.TimeLastPurchase}"));
                    break;
                default:
                    break;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Buy an amount of boxes of an exact size
        /// </summary>
        /// <param name="x">X dimention</param>
        /// <param name="y">Y dimention</param>
        /// <param name="count">Amount of boxes</param>
        /// <returns></returns>
        public bool BuyExactBoxSize(double x, double y, int count)
        {
            //search for the exact size
            var mainSearchTerm = new BottomSizeTreeDataModel() { X = x };
            if (MainTree.FindExact(mainSearchTerm, out BottomSizeTreeDataModel mainTreeData) == false)
            {
                //if no data found with the x value
                return false;
            }

            var innerSearchTerm = new HeightSizeTreeDataModel() { Y = y };
            if (mainTreeData.InnerTree.FindExact(innerSearchTerm, out HeightSizeTreeDataModel innerTreeData) == false)
            {
                //if no data found for this x value with this y value
                return false;
            }
            
            //if found the spesific box.
            //check stock avilability, don't allow to perchues more than what you have
            if (innerTreeData.Count < count) return false;

            //lower the stock count
            innerTreeData.Count -= count;

            //if no more stock, delete the record in both the inner tree(if no other nodes in inner tree delete the main tree node)
            //an in the date list
            if(innerTreeData.Count <= 0)
            {
                //delete the node from the inner tree
                mainTreeData.InnerTree.Delete(innerTreeData);
                //if now the inner tree has no nodes
                if (mainTreeData.InnerTree.IsEmpty)
                    //delete the mainTree node
                    MainTree.Delete(mainTreeData);
                DeleteFromDB(x, y);
                return true;
                //TODO:
                //Delete from DataByTime
            }
            //Save the changes in the db
            SaveToDB(x, y, innerTreeData.Count, DateTime.Now);
            return true;
        }

        /// <summary>
        /// Delete from the db with x and y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void DeleteFromDB(double x, double y)
        {
            using (DataAccess dal = new DataAccess())
            {
                dal.Delete(new Box
                {
                    X = x,
                    Y = y
                });
            }
        }

        /// <summary>
        /// FInd a present Q
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Box FindPreasent(double x, double y)
        {
            //find X
            var mainTreeSearchTerm = new BottomSizeTreeDataModel() { X = x };
            BottomSizeTreeDataModel actualX = MainTree.FindClosestOrExact(mainTreeSearchTerm);

            //find Y
            var innerTreeSearchTerm = new HeightSizeTreeDataModel() { Y = y };
            var innerTreeData = actualX.InnerTree.FindClosestOrExact(innerTreeSearchTerm);
            
            //output
            return new Box()
            {
                X = actualX.X,
                Y = innerTreeData.Y,
                Count = innerTreeData.Count
            };
        }

        /// <summary>
        /// Load Data from db to run time
        /// </summary>
        void LoadDataFromDB()
        {
            //load data from db
            using (DataAccess dal = new DataAccess())
            {
                dal.GetAllData().ToList().ForEach(d => AddToStock(d.X, d.Y, d.Count, d.TimeLastPurchase, false));
            }
        }
    }
}
