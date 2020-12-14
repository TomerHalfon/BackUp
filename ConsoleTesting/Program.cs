using BoxesPojectShared.Interfaces;
using BoxesProjectLogic;
using ConsoleUI.Implementations;
using DataStructures;
using ExtentionsByTomer.ConsoleUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTesting
{
    class Program
    {
        static List<BoxesPojectShared.Entities.Box> TestData => new List<BoxesPojectShared.Entities.Box> {
            new BoxesPojectShared.Entities.Box { X = 2, Y = 2, Count = 2 },
            new BoxesPojectShared.Entities.Box { X = 2, Y = 1, Count = 2 },
            new BoxesPojectShared.Entities.Box { X = 2, Y = 4, Count = 2, TimeLastPurchase = DateTime.Now.Subtract(TimeSpan.FromDays(1)) },
            new BoxesPojectShared.Entities.Box { X = 1, Y = 2, Count = 2 },
            new BoxesPojectShared.Entities.Box { X = 1, Y = 3, Count = 2, TimeLastPurchase = DateTime.Now.Subtract(TimeSpan.FromDays(3)) },
            new BoxesPojectShared.Entities.Box { X = 1, Y = 4, Count = 2 },
            new BoxesPojectShared.Entities.Box { X = 66, Y = 32, Count = 2, TimeLastPurchase = DateTime.Now.Subtract(TimeSpan.FromDays(2)) },
            new BoxesPojectShared.Entities.Box { X = 66, Y = 12, Count = 2 },
            new BoxesPojectShared.Entities.Box { X = 44, Y = 4, Count = 2},
            new BoxesPojectShared.Entities.Box { X = 44, Y = 2, Count = 2 },
            new BoxesPojectShared.Entities.Box { X = 44, Y = 44, Count = 2 },
            new BoxesPojectShared.Entities.Box { X = 2, Y = 22, Count = 2, TimeLastPurchase = DateTime.Now.Subtract(TimeSpan.FromDays(10)) },
            new BoxesPojectShared.Entities.Box { X = 76, Y = 99, Count = 2 },
            new BoxesPojectShared.Entities.Box { X = 22, Y = 2, Count = 2, TimeLastPurchase = DateTime.Now.Subtract(TimeSpan.FromDays(15)) },
        };

        enum Actions
        {
            AddToStock = 1,
            ShowBoxData,
            BuyExactBox,
            BuyBoxForPresent,
            BuyBoxesForPresent,
            PrintStock,
            Clear
        }
        static Manager mng = new Manager(new ConsoleLogger(), new UserInteractions());
        static void Main(string[] args)
        {
            LoadTestData();
            ConfigureManager();
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                PrintActions();

                Console.ForegroundColor = ConsoleColor.Green;
                Actions selected = (Actions)"Action => ".Get<int>();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();

                switch (selected)
                {
                    case Actions.AddToStock:
                        AddToStock("Box X => ".Get<double>(), "Box Y => ".Get<double>(), "Count => ".Get<int>());
                        break;
                    case Actions.PrintStock:
                        PrintStock();
                        break;
                    case Actions.Clear:
                        Clear();
                        continue;
                    case Actions.BuyExactBox:
                        var success = BuyExactBox("Box X => ".Get<double>(), "Box Y => ".Get<double>(), "Count => ".Get<int>());
                        Console.WriteLine($"Transaction => Success Status => {success}");
                        break;
                    case Actions.BuyBoxForPresent:
                        BuyBoxForPresent("Box X => ".Get<double>(), "Box Y => ".Get<double>());
                        break;
                    case Actions.BuyBoxesForPresent:
                        BuyBoxesForPresent("Box X => ".Get<double>(), "Box Y => ".Get<double>(), "Count => ".Get<int>());
                        break;
                    case Actions.ShowBoxData:
                        GetBox("Box X => ".Get<double>(), "Box Y => ".Get<double>());
                        break;
                    default:
                        break;
                }
                Console.WriteLine();
            }
        }

        private static void ConfigureManager()
        {
            //IConfiguration config;
            //config.SetMax

            //mng = new Manager(config);
        }

        private static void LoadTestData()
        {
            TestData.ForEach(box => mng.AddToStock(box.X, box.Y, box.Count, box.TimeLastPurchase));
        }

        private static void GetBox(double x, double y)
        {
            //try to get a box
            var box = mng.GetBox(x, y);
            //if no box returned
            if (box is null) Console.WriteLine($"No match for size X => {x}, Y => {y}");
            //if box was returned
            else Console.WriteLine($"Found match for size X => {box.X}, Y => {box.Y} Count => {box.Count}, Last Bought at => {box.TimeLastPurchase}");
        }

        private static void BuyBoxForPresent(double x, double y)
        {
            var res = mng.BuyBoxForPresent(x, y);
            if (res != null)
            {
                Console.WriteLine($"Bought Present!\nX => {res.X}, Y => {res.Y}, Now In Stock => {res.Count}");
            }
        }
        private static void BuyBoxesForPresent(double x, double y, int count)
        {
            mng.BuyMultipleBoxesForPresent(x, y, count);
        }
        private static bool BuyExactBox(double x, double y, int count)
        {
            return mng.BuyExactBoxSize(x, y, count);
        }
        private static void Clear() => Console.Clear();
        private static void AddToStock(double x, double y, int count) => mng.AddToStock(x, y, count);
        private static async void PrintStock() => Console.Write(await mng.PrintStock(Manager.PrintOrder.BySizeAsc));
        private static void PrintActions()
        {
            int actionNum = 1;
            Enum.GetNames(typeof(Actions))
                .ToList()
                .ForEach(a =>
                {
                    string action = $"{actionNum++} | {a}";
                    Console.WriteLine($"{action}\n{new string('-', action.Length)}");
                });
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
