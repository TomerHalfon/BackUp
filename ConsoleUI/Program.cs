using BoxesProjectLogic;
using ConsoleUI.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjection.Abstractions;
using DependencyInjection.Implementations;
using BoxesPojectShared.Interfaces;

namespace ConsoleUI
{
    class Program
    {
        static List<BoxesPojectShared.Entities.Box> TestData => new List<BoxesPojectShared.Entities.Box> {
            new BoxesPojectShared.Entities.Box { X = 2, Y = 2, Count = 2 , TimeLastPurchase = DateTime.Now},
            new BoxesPojectShared.Entities.Box { X = 2, Y = 1, Count = 2 , TimeLastPurchase = DateTime.Now},
            new BoxesPojectShared.Entities.Box { X = 2, Y = 4, Count = 2, TimeLastPurchase = DateTime.Now.Subtract(TimeSpan.FromDays(1)) },
            new BoxesPojectShared.Entities.Box { X = 1, Y = 2, Count = 2 , TimeLastPurchase = DateTime.Now},
            new BoxesPojectShared.Entities.Box { X = 1, Y = 3, Count = 2, TimeLastPurchase = DateTime.Now.Subtract(TimeSpan.FromDays(3)) },
            new BoxesPojectShared.Entities.Box { X = 1, Y = 4, Count = 2 , TimeLastPurchase = DateTime.Now},
            new BoxesPojectShared.Entities.Box { X = 66, Y = 32, Count = 2, TimeLastPurchase = DateTime.Now.Subtract(TimeSpan.FromDays(2)) },
            new BoxesPojectShared.Entities.Box { X = 66, Y = 12, Count = 2 , TimeLastPurchase = DateTime.Now},
            new BoxesPojectShared.Entities.Box { X = 44, Y = 4, Count = 2, TimeLastPurchase = DateTime.Now},
            new BoxesPojectShared.Entities.Box { X = 44, Y = 2, Count = 2 , TimeLastPurchase = DateTime.Now},
            new BoxesPojectShared.Entities.Box { X = 44, Y = 44, Count = 2 , TimeLastPurchase = DateTime.Now},
            new BoxesPojectShared.Entities.Box { X = 2, Y = 22, Count = 2, TimeLastPurchase = DateTime.Now.Subtract(TimeSpan.FromDays(10)) },
            new BoxesPojectShared.Entities.Box { X = 76, Y = 99, Count = 2 , TimeLastPurchase = DateTime.Now},
            new BoxesPojectShared.Entities.Box { X = 22, Y = 2, Count = 2, TimeLastPurchase = DateTime.Now.Subtract(TimeSpan.FromDays(15)) },
        };


        static IManager _mng;
        static void RunStartUp()
        {
            IStartup startup = new StartUp();
            IInversionOfControl ioc = new InversionOfControlContainer();

            startup.ConfigureDependencies(ioc);
            startup.ConfigureStaticData(ioc.GetInstance<IConfiguration>());

            _mng = ioc.GetInstance<IManager>();
        }
        static void Main(string[] args)
        {
            RunStartUp();
            //_mng.AddToStock(1, 1, 3);
            //_mng.AddToStock(3, 1, 3);
            //_mng.AddToStock(4, 3, 3);
            //_mng.AddToStock(4, 4, 3);
            //RunTests(_mng, T10) ;

            //TestData.ForEach(b => _mng.AddToStock(b.X, b.Y, b.Count, b.TimeLastPurchase));
            //Console.WriteLine(_mng.PrintStock(Manager.PrintOrder.BySizeAsc));
            //_mng.DeletionEventHandler(null);

            //_mng.TEST();
            //_mng.AddToStock(1, 1, 10, DateTime.Now);
            //Console.ReadLine();
            //_mng.AddToStock(1, 3, 44, DateTime.Now);
            //_mng.BuyBoxForPresent(2, 1);
            _mng.BuyMultipleBoxesForPresent(5, 5, 8);
            Console.WriteLine(_mng.PrintStock(Manager.PrintOrder.ByDateAsc));
        }
        static void RunTests(IManager mng, params Action<IManager>[] tests)
        {
            tests.ToList().ForEach(test => test.Invoke(mng));
        }
        //closest to X and exact Y
        static void T1()
        {
            //inputs
            double x = 3, y = 3;
            //Expected
            double eX = 4, eY = 3;
            //resaults
            var b = _mng.BuyBoxForPresent(x, y);
            //Assertion
            if (b.X.Equals(eX) && b.Y.Equals(eY))
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Test X=>{x},Y =>{y} Expected: X=>{eX},Y=>{eY} Actual X=>{b.X},Y=>{b.Y}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        //ExactMatch both x and y
        static void T2()
        {
            //inputs
            double x = 1, y = 1;
            //Expected
            double eX = 1, eY = 1;
            //resaults
            var b = _mng.BuyBoxForPresent(x, y);
            //Assertion
            if (b.X.Equals(eX) && b.Y.Equals(eY))
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Test X=>{x},Y =>{y} Expected: X=>{eX},Y=>{eY} Actual X=>{b.X},Y=>{b.Y}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        //closest to both x and y
        static void T3()
        {
            //inputs
            double x = 2, y = 2;
            //Expected
            double eX = 4, eY = 3;
            //resaults
            var b = _mng.BuyBoxForPresent(x, y);
            //Assertion
            if (b.X.Equals(eX) && b.Y.Equals(eY))
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Test X=>{x},Y =>{y} Expected: X=>{eX},Y=>{eY} Actual X=>{b.X},Y=>{b.Y}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        //exactMatch x closest y
        static void T4()
        {
            //inputs
            double x = 4, y = 2;
            //Expected
            double eX = 4, eY = 3;
            //resaults
            var b = _mng.BuyBoxForPresent(x, y);
            //Assertion
            if (b.X.Equals(eX) && b.Y.Equals(eY))
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Test X=>{x},Y =>{y} Expected: X=>{eX},Y=>{eY} Actual X=>{b.X},Y=>{b.Y}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        //non Existing match for both
        static void T5()
        {
            //inputs
            double x = 5, y = 5;
            //Expected
            //should return null and print to screen
            //resaults
            Console.ForegroundColor = ConsoleColor.Yellow;
            var b = _mng.BuyBoxForPresent(x, y);
            //Assertion
            if (b is null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else Console.ForegroundColor = ConsoleColor.Red;
            
            Console.WriteLine($"Test X=>{x},Y =>{y} Expected: null Actual X=>{b?.X},Y=>{b?.Y}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        //non Existing match for X only
        static void T6()
        {
            //inputs
            double x = 5, y = 1;
            //Expected
            //should return null and print to screen
            //resaults
            Console.ForegroundColor = ConsoleColor.Yellow;
            var b = _mng.BuyBoxForPresent(x, y);
            //Assertion
            if (b is null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"Test X=>{x},Y =>{y} Expected: null Actual X=>{b?.X},Y=>{b?.Y}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        //non Existing match for Y only
        static void T7()
        {
            //inputs
            double x = 1, y = 5;
            //Expected
            //should return null and print to screen
            //resaults
            Console.ForegroundColor = ConsoleColor.Yellow;
            var b = _mng.BuyBoxForPresent(x, y);
            //Assertion
            if (b is null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"Test X=>{x},Y =>{y} Expected: null Actual X=>{b?.X},Y=>{b?.Y}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        //Buy present Part B
        //Exact All
        static void T8()
        {
            //inputs
            double x = 4, y = 3;
            int count = 3;
            //Expected
            double eX = 4, eY = 3;
            //resaults
            _mng.BuyMultipleBoxesForPresent(x, y,count);
        }
        //Exact sizes but not enough in stock of exact size
        static void T9()
        {
            //inputs
            double x = 1, y = 1;
            int count = 10;
            //Expected
            double eX = 4, eY = 3;
            //resaults
            _mng.BuyMultipleBoxesForPresent(x, y, count);
        }
        //Cant fulfil because of count
        static void T10()
        {
            //inputs
            double x = 1, y = 1;
            int count = 100;
            //Expected
            double eX = 4, eY = 3;
            //resaults
            _mng.BuyMultipleBoxesForPresent(x, y, count);
        }
    }
}
