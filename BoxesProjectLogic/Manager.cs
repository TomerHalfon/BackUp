using BoxesProjectLogic.DataModels;
using DataStructures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using BoxesProjectData;
using BoxesPojectShared.Entities;
using BoxesPojectShared.Interfaces;
using DataStructures.Abstractions;
using System.Threading;

namespace BoxesProjectLogic
{
    public class Manager : IManager
    {
        public event EventHandler<IEnumerable<Box>> OldBoxesDeletionEvent;


        readonly ILogger _logger;
        readonly IUserInteractions _userInteractions;
        readonly IConfiguration _config;
        readonly IRepository<Box> _boxesRepository;
        /// <summary>
        /// Search the sizes based on the X dimention and than by the Y dimention.
        /// allows for Log(n) operations
        /// </summary>
        readonly IBinarySearchTree<BottomSizeTreeDataModel> _mainTree;
        /// <summary>
        /// This List is sorted by the time of the last perchuess for each box size.
        /// you can pull from a box size from here based on the last time a customer bought this size
        /// </summary>
        readonly ILinked_List<TimeListDataModel> _dataByTime;
        Timer _timer;
        public DateTime DeletingAt { get; private set; }
        public DateTime DeletingOlderThan => DeletingAt.Subtract(TimeSpan.FromDays(_config.DaysToStockRefresh));

        #region Constructor
        public Manager(ILogger logger, IUserInteractions userInteractions,
            IConfiguration configuration, IBinarySearchTree<BottomSizeTreeDataModel> mainTree,
            ILinked_List<TimeListDataModel> linkedList, IRepository<Box> boxesRepository)
        {
            //Recive Implementations
            _logger = logger;
            _userInteractions = userInteractions;
            _config = configuration;
            _boxesRepository = boxesRepository;
            _mainTree = mainTree;
            _dataByTime = linkedList;

            //Init The Manager
            Initialize();
        }
        #endregion
        #region Public Methods

        /// <summary>
        /// Add to stock from outside (UI)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="count"></param>
        public void AddToStock(double x, double y, int count)
        {
            AddToStock(x, y, count, null, true, null);
        }

        /// <summary>
        /// Get the exact <c>Box</c> by the arguments
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns><c>null</c> if no match</returns>
        public Box GetBox(double x, double y)
        {
            //Search the main tree
            var mainTreeSearchTerm = new BottomSizeTreeDataModel(x);
            if (_mainTree.FindExact(mainTreeSearchTerm, out BottomSizeTreeDataModel mainTreeData) == false) return null;

            //search the inner tree
            var innerTreeSearchTerm = new HeightSizeTreeDataModel(y);
            if (mainTreeData.InnerTree.FindExact(innerTreeSearchTerm, out HeightSizeTreeDataModel innerTreeData) == false) return null;

            //return the box
            return new Box
            {
                Id = innerTreeData.Id,
                X = mainTreeData.X,
                Y = innerTreeData.Y,
                Count = innerTreeData.Count,
                TimeLastPurchase = innerTreeData.DateNode.Data.TimeLastPurchase
            };
        }
        /// <summary>
        /// Get a string of the Stock by a type of order
        /// </summary>
        /// <param name="order">by what comperiosn to sort</param>
        /// <returns></returns>
        public IEnumerable<Box> GetStock()
        {
            List<Box> stock = new List<Box>();
            _mainTree.TraverseInOrder(x => x.InnerTree.TraverseInOrder(y => stock.Add(new Box {Id = y.Id, X = x.X, Y = y.Y, Count = y.Count, TimeLastPurchase = y.DateNode.Data.TimeLastPurchase})));
            return stock;
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
            var mainSearchTerm = new BottomSizeTreeDataModel(x);
            if (_mainTree.FindExact(mainSearchTerm, out BottomSizeTreeDataModel mainTreeData) == false)
            {
                //if no data found with the x value
                _logger.Log(new LogData($"Can't find a box with a X value of: {x}", false));
                return false;
            }

            var innerSearchTerm = new HeightSizeTreeDataModel(y);
            if (mainTreeData.InnerTree.FindExact(innerSearchTerm, out HeightSizeTreeDataModel innerTreeData) == false)
            {
                //if no data found for this x value with this y value
                _logger.Log(new LogData($"Can't find a box with a X value of: {x} and a Y value of {y}", false));
                return false;
            }

            //if found the spesific box.
            //check stock avilability, don't allow to perchues more than what you have
            if (innerTreeData.Count < count)
            {
                _logger.Log(new LogData($"Not enugh boxes of this size in stock, in stock: {innerTreeData.Count}", new Box { X = x, Y = y, Count = count },false));
                return false;
            }

            BuyHelper(mainTreeData, innerTreeData, count);
            //Save the changes in the db
            //SaveToDB(x, y, innerTreeData.Count, DateTime.Now);
            return true;
        }

        //Part 1
        /// <summary>
        /// Buy a single box for a preasent cant be smaller than the preasent size
        /// </summary>
        /// <param name="x">present width and length </param>
        /// <param name="y">present height</param>
        /// <param name="wasLast"> was this box the last one in stock</param>
        /// <returns>the box that was bought, null if no such box exist in stock</returns>
        public Box BuyBoxForPresent(double x, double y)
        {
            var (actualX, actualY) = FindSingleBoxForPresent(x, y);

            if (actualX is null || actualY is null)
            {
                _logger.Log(new LogData($"No Box for this size [{x}, {y}]", false));
                return null;
            }

            BuyHelper(actualX, actualY, 1);

            //output
            var box =  new Box()
            {
                Id = actualY.Id,
                X = actualX.X,
                Y = actualY.Y,
                Count = actualY.Count,
                TimeLastPurchase = actualY.DateNode.Data.TimeLastPurchase
            };
            return box;
        }

        //Part 2
        /// <summary>
        /// Buy multiple boxes, 
        /// if not enugh of this size, will try to compleate from other larger sizes
        /// will need user to confirm the order
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="count"></param>
        public void BuyMultipleBoxesForPresent(double x, double y, int count)
        {
            var order = CompileOrder(x, y, count);
            if(order.Count == 0)
            {
                _logger.Log(new LogData("No Boxes Found for this search",new Box { X = x, Y = y, Count = count},false));
                return;
            }
            var confirmation = _userInteractions
                .RequestConfirmation(order
                .Select(b => new Box
                {
                    Id = b.actualY.Id,
                    X = b.actualX.X,
                    Y = b.actualY.Y,
                    Count = b.count,
                    TimeLastPurchase = b.actualY.DateNode.Data.TimeLastPurchase.Date
                }).ToList());

            if (confirmation)
            {
                order.ForEach(b => BuyHelper(b.actualX, b.actualY, count));
            }
            else
            {
                
                //Cancel
                _logger.Log(new LogData("User Canceled Request, No action taken"));
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Initialize components
        /// </summary>
        void Initialize()
        {
            //Load relevent config data
            LoadConfigData();

            //Load data from db
            LoadDataFromDB();

            //Start timer
            LoadTimer();

        }

        /// <summary>
        /// Load Info from config file
        /// </summary>
        void LoadConfigData()
        {
            HeightSizeTreeDataModel.MaxCount = _config.MaxCount;
        }
        /// <summary>
        /// Load Data from db to run time
        /// </summary>
        void LoadDataFromDB() =>
            //load data from db
            _boxesRepository
            .Get()
            .ToList()
            .ForEach(d => AddToStock(d.X, d.Y, d.Count, d.TimeLastPurchase, false, d.Id));
        /// <summary>
        /// Sets the deletion event timer
        /// </summary>
        void LoadTimer()
        {
            //00:00:00 the next day
            var when = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddDays(1);
            DeletingAt = when;
            // when to start
            //from now to 00:00:00
            var dueTime = when.Subtract(DateTime.Now);
            //diff between call backs
            //every day
            var period = TimeSpan.FromDays(1);
            //will invoke the deletionEventHandler at 00:00:00 every day
            _timer = new Timer(DeletionTimerCallBack, null, dueTime, period);
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
        void AddToStock(double x, double y, int count, DateTime? timeLastPurchase = null, bool saveToDB = true, int? id = null)
        {
            bool newBox = false;
            //Get the mainTreeData for the search term from the main tree
            var mainTreeSearchTerm = new BottomSizeTreeDataModel(x);
            _mainTree.FindAndUpdate(mainTreeSearchTerm, out BottomSizeTreeDataModel mainTreeData);

            //Get the innerTreeData for the searchTerm in the inner tree
            var innerTreeSearchTerm = new HeightSizeTreeDataModel(y);
            if (mainTreeData.InnerTree is null) mainTreeData.InnerTree = new BinarySearchTree<HeightSizeTreeDataModel>();

            //if the inner TreeData is new
            if (mainTreeData.InnerTree.FindAndUpdate(innerTreeSearchTerm, out HeightSizeTreeDataModel innerTreeData) == false)
            {
                //if there isn't time data, than will get the current date and time
                if (timeLastPurchase.HasValue == false)
                    innerTreeData.DateNode = _dataByTime.Add(new TimeListDataModel() { X = x, Y = y, TimeLastPurchase = DateTime.Now });
                //if there is a time last purchase it means this should be added to a diffrent position.
                //will never be null here
                else
                    innerTreeData.DateNode = _dataByTime.InsertByValue(new TimeListDataModel { X = x, Y = y, TimeLastPurchase = timeLastPurchase.Value });

                newBox = true;
            }
            //if the inner tree data is old
            else
            {
                //update the data
                innerTreeData.DateNode.Data.TimeLastPurchase = DateTime.Now;
                //Shift the node to the end of the list
                _dataByTime.ShiftNodeToEnd(innerTreeData.DateNode);

            }

            //Set the Properties
            innerTreeData.Count += count;
            //if there is no value than id will be 0 by default(int)
            if (id.HasValue)
                innerTreeData.Id = id.Value;

            //the box of this size
            var box = new Box
            {
                Id = innerTreeData.Id,
                X = mainTreeData.X,
                Y = innerTreeData.Y,
                Count = innerTreeData.Count,
                TimeLastPurchase = innerTreeData.DateNode.Data.TimeLastPurchase
            };

            //Add Data To DB
            if (saveToDB)
            {
                _logger.Log(new LogData("Updated Stock", true));
                //save the id 
                innerTreeData.Id = SaveToDB(box, newBox).Id;
            }
        }
        /// <summary>
        /// Will Get a box in the exact or slightly larger
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>an obect with the refrences of this size</returns>
        (BottomSizeTreeDataModel actualX, HeightSizeTreeDataModel actualY) FindSingleBoxForPresent(double x, double y)
        {

            var actualX = default(BottomSizeTreeDataModel);
            var actualY = default(HeightSizeTreeDataModel);
            var mainTreeSearchTerm = new BottomSizeTreeDataModel(x);
            var innerTreeSearchTerm = new HeightSizeTreeDataModel(y);

            while (actualY is null)
            {
                //get the actual x
                actualX = _mainTree.FindClosestOrExact(mainTreeSearchTerm);

                //get the actual y
                actualY = actualX?.InnerTree.FindClosestOrExact(innerTreeSearchTerm);

                if (actualX is null && actualY is null)
                {
                    break;
                }
                mainTreeSearchTerm.X = actualX.X + _config.SearchIncrement;
                if (!ValidateSize(mainTreeSearchTerm.X, x))
                {
                    break;
                }
            }
            return (actualX, actualY);
        }
        /// <summary>
        /// Compiles an order given a size and an amount
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<(BottomSizeTreeDataModel actualX, HeightSizeTreeDataModel actualY, int count)> CompileOrder(double x, double y, int count)
        {
            var order = new List<(BottomSizeTreeDataModel x, HeightSizeTreeDataModel y, int count)>();
            var xSearchTerm = new BottomSizeTreeDataModel(x);
            int countSoFar = 0;
            int splits = 0;

            var actualX = _mainTree.FindClosestOrExact(xSearchTerm);
            while (actualX != null)
            {
                //SearchTerm
                var ySearchTerm = new HeightSizeTreeDataModel(y);

                //Get all of the boxes for this order of this base
                var boxes = FindBoxesForX(actualX, ySearchTerm, count - countSoFar, ref splits);

                //add to order
                order.AddRange(boxes);

                //increse the counter
                countSoFar += boxes.Sum(b => b.count);

                //if the order is now complete
                if (count <= countSoFar) break;
                if (splits >= _config.MaxSplits) break;
                //Progress the loop
                xSearchTerm.X = actualX.X + _config.SearchIncrement;
                actualX = _mainTree.FindClosestOrExact(xSearchTerm);

                if (ValidateSize(xSearchTerm.X,x) == false)
                {
                    _logger.Log(new LogData($"Reached max size allowed in config X: {actualX.X}"));
                    break;
                }
            }
            if (order.Count <= 0)
            {
                _logger.Log(new LogData("Can't complete order",false));
            }
            if (countSoFar < count)
                _logger.Log(new LogData($"Incomplete order missing: {count - countSoFar}",false));

            return order;
        }
        /// <summary>
        /// Find all boxes for this X size, using the search term and splits counter
        /// </summary>
        /// <param name="x"></param>
        /// <param name="searchTerm"></param>
        /// <param name="count"></param>
        /// <param name="splitsSoFar"></param>
        /// <returns></returns>
        IEnumerable<(BottomSizeTreeDataModel x, HeightSizeTreeDataModel y, int count)> FindBoxesForX(BottomSizeTreeDataModel x, HeightSizeTreeDataModel searchTerm, int count, ref int splitsSoFar)
        {
            var actualData = new List<(BottomSizeTreeDataModel, HeightSizeTreeDataModel, int)>();
            int countSoFar = 0;

            var actualY = x.InnerTree.FindClosestOrExact(searchTerm);
            while (actualY != null)
            {
                //Found a size
                //not a valid size
                if (ValidateSize(actualY.Y,searchTerm.Y) == false)
                {
                    _logger.Log(new LogData($"Reached max size allowed in config Y: {actualY.Y}"));
                    break;
                }
                splitsSoFar++;
                //valid Size
                int fromThisSize = Math.Min(actualY.Count, count - countSoFar);
                actualData.Add((x, actualY, fromThisSize));
                countSoFar += fromThisSize;

                //if finished all of the count needed
                if (count <= countSoFar) break;
                if (splitsSoFar >= _config.MaxSplits)
                {
                    _logger.Log(new LogData("Max splits reached", false));
                    break;
                }
                //progress the loop
                searchTerm.Y = actualY.Y + _config.SearchIncrement;
                actualY = x.InnerTree.FindClosestOrExact(searchTerm);
            }
            return actualData;
        }
        /// <summary>
        /// Recives size models and an amount of this size,
        /// processes the purchase action
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="count"></param>
        void BuyHelper(BottomSizeTreeDataModel x, HeightSizeTreeDataModel y, int count)
        {
            //Get the box from the db
            var boxInDB = _boxesRepository.GetById(y.Id);
            //null check
            if (boxInDB is null) throw new Exception("Box was not found in db while trying to buy");

            //update purchase time
            y.DateNode.Data.TimeLastPurchase = DateTime.Now;

            //Update the count
            y.Count -= count;


            //if no more of this size
            if (y.Count <= 0)
            {
                //delete from inner tree
                x.InnerTree.Delete(y, out _);
                _logger.Log(new LogData($"Deleted {y.Y} from inner tree",true));

                //if now the inner tree has no nodes
                if (x.InnerTree.IsEmpty)
                {
                    //delete the mainTree node
                    _mainTree.Delete(x, out _);
                    _logger.Log(new LogData($"Deleted {x.X} from main tree",true));
                }

                //Delete fome Date Linked List.
                _dataByTime.DeleteNode(y.DateNode);
                _logger.Log(new LogData("Deleted node in time list",true));

                //delete in db
                _boxesRepository.Delete(boxInDB.Id);
                _logger.Log(new LogData("Deleted box from db", boxInDB, true));
            }
            else
            {
                //if there are more of this size
                //update the date data
                if (_dataByTime.ShiftNodeToEnd(y.DateNode))
                    _logger.Log(new LogData("Node Shifted",true));
                else
                    _logger.Log(new LogData("Node was allready the latest"));

                //update the db
                boxInDB.Count = y.Count;
                boxInDB.TimeLastPurchase = y.DateNode.Data.TimeLastPurchase;

                boxInDB = _boxesRepository.Update(boxInDB);
                _logger.Log(new LogData("Updated box", boxInDB, true));
                if (y.Count <= _config.NotifyCountThreshold) _logger.Log(new LogData("This box count is low", boxInDB,false));

            }
        }
        /// <summary>
        /// Validates a size
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        bool ValidateSize(double size, double originalSize) => size <= originalSize * (_config.MaxSizeDiff / 100);

        /// <summary>
        /// Takes All Data and saves a new entry
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="count"></param>
        /// <param name="timeLastPurchase"></param>
        Box SaveToDB(Box box, bool isNewBox)
        {
            _logger.Log(new LogData("Updated DataBase", box, true));
            if (isNewBox) return _boxesRepository.Create(box);
            else
            {
                box.Id = _boxesRepository.Get()
                    .SingleOrDefault(b => b.X.Equals(box.X) && b.Y.Equals(box.Y))?.Id ?? throw new ArgumentException("cant update a non existing box");
                return _boxesRepository.Update(box);
            }
        }
        #endregion

        /// <summary>
        /// The deletion Event
        /// </summary>
        /// <param name="state"></param>
        public void DeletionTimerCallBack(object state)
        {
            var deleted = new List<Box>();
            //Get the oldest date that wont be deleted
            var oldestAllowed = DateTime.Now.Subtract(TimeSpan.FromDays(_config.DaysToStockRefresh));

            //Disconnect all old data from the list
            var boxes = _dataByTime.DeleteNodesSmallerThan(new TimeListDataModel { TimeLastPurchase = oldestAllowed });

            //loop through all data needed to be removed
            foreach (var box in boxes)
            {
                //delete from trees
                if (_mainTree.FindExact(new BottomSizeTreeDataModel(box.X), out BottomSizeTreeDataModel foundX))
                {
                    //delete the Y
                    foundX.InnerTree.Delete(new HeightSizeTreeDataModel(box.Y), out HeightSizeTreeDataModel deletedYData);

                    //delete the x
                    if (foundX.InnerTree.IsEmpty) _mainTree.Delete(foundX, out _);

                    //delete from DB
                    _boxesRepository.Delete(deletedYData.Id);

                    deleted.Add(new Box { Id = deletedYData.Id, X = box.X, Y = box.Y, Count = deletedYData.Count, TimeLastPurchase = box.TimeLastPurchase });
                }
            }
            OldBoxesDeletionEvent?.Invoke(this, deleted);
        }
    }
}