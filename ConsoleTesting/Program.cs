using BoxesProjectLogic;
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
        enum Actions
        {
            AddToStock = 1,
            BuyExactBox,
            FindPresent,
            PrintStock,
            Clear
        }
        static Manager mng = new Manager();
        static void Main(string[] args)
        {
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
                    case Actions.FindPresent:
                       FindPreasent("Box X => ".Get<double>(), "Box Y => ".Get<double>());
                        break;
                    default:
                        break;
                }
                Console.WriteLine();
            }
        }

        private static void FindPreasent(double x, double y)
        {
            var res = mng.FindPreasent(x, y);
            if(res is null)
            {
                Console.WriteLine("No Present found");
            }
            else
            {
                Console.WriteLine($"Found Present!\nX => {res.X}, Y => {res.Y}, In Stock => {res.Count}");
            }
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
