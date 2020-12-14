using BoxesPojectShared.Entities;
using BoxesProjectConsoleUI.UIMenus;
using System;
using System.Collections.Generic;
using System.Linq;
using static BoxesProjectConsoleUI.BoxesUI;

namespace BoxesProjectConsoleUI
{
    internal static class DisplayHelper
    {
        internal static void Greet()
        {
            string greet = " Hello, Welcome To BOXESn'STUFF* ";

            Console.BackgroundColor = Design.Colors("logobackground");
            Console.ForegroundColor = Design.Colors("logo");

            Console.WriteLine(new string('─', greet.Length));
            Console.WriteLine(greet);
            Console.WriteLine(new string('─', greet.Length));
            Console.WriteLine("*no actual stuff");
            Console.WriteLine();
            Console.ResetColor();
        }

        internal static void HoldForAction()
        {
            Console.Write("Press any key to continue... ");
           _ = Console.ReadKey();
            //Remove 2 lines here
        }
        internal static void PrintBoxes(IEnumerable<Box> stock)
        {
            var table = Design.GetStarterBoxesTable;
            stock.ToList().ForEach(box => table.AddRow(box.Id, box.X, box.Y, box.Count, box.TimeLastPurchase.ToShortDateString()));
            table.Write(Design.Formats("Boxes"));
        }

        internal static void Close()
        {
            Console.WriteLine("Closing");
        }

        internal static void PrintBox(Box box)
        {
            Console.WriteLine($"Box => Id: {box.Id} | X: {box.X}, Y: {box.Y}");
            Console.WriteLine($"In stock => {box.Count}| Last bought at {box.TimeLastPurchase.Date}");
        }

        internal static void Invalid()
        {
            Console.WriteLine("Invalid Action");
        }

        internal static void ClearBackTo((int CursorLeft, int CursorTop) ReturnTo)
        {
            var cursorTop =  Console.CursorTop;
            for (int i = cursorTop; i >= ReturnTo.CursorTop; i--)
            {
                //Clear this row
                Console.Write(new string(' ', Console.BufferWidth));
                //Move the cursor
                Console.SetCursorPosition(0, i);
            }
            Console.SetCursorPosition(ReturnTo.CursorLeft, ReturnTo.CursorTop);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(ReturnTo.CursorLeft, ReturnTo.CursorTop);
        }

        internal static TActions GetAction<TActions,UIMenu>() where TActions : struct, Enum where UIMenu: IUIMenu
        {
            
            Console.ForegroundColor = Design.Colors<UIMenu>();

            if (typeof(TActions).IsEnum == false) throw new ArgumentException("Enums Only");

            //print options and request a choise
            Console.WriteLine($"  {typeof(TActions).Name}:");

            var table = new ConsoleTables.ConsoleTable("#", "Option");
            table.Options.EnableCount = false;

            //I know it's kinda dirty but :)
            var values = typeof(TActions).GetEnumValues();
            var firstAction = values.GetValue(0);
            var lastAction = values.GetValue(values.Length - 1);

            int i = Convert.ToInt32(firstAction);
            foreach (var item in values)
            {
                table.AddRow(i++, item.ToString().Replace('_', ' '));
            }
            //Enum.GetNames(typeof(TActions)).ToList().ForEach(name => table.AddRow(i++, name));

            table.Write(Design.Formats("Menu"));
            Console.Write("Enter an options => ");

            Console.ResetColor();
            TActions res;
            var cursorY = Console.CursorTop;
            var cursorX = Console.CursorLeft;
            var input = Console.ReadLine();
            while (!Enum.TryParse(input,true, out res) || res.CompareTo(firstAction) < 0 || res.CompareTo(lastAction) > 0)
            {
                Invalid();
                input = Console.ReadLine();
            }
            FormatResualt(cursorY, cursorX, res.ToString().Replace('_',' '));
            return res;
        }

        private static void FormatResualt(int cursorY, int cursorX, string res)
        {
            ClearBackTo((cursorX, cursorY));
            Console.WriteLine(res);
        }

        internal static (double X, double Y) GetSize(this string msg)
        {
            Console.WriteLine(msg);
            return (GetDouble("X => "), GetDouble("Y => "));
        }
        internal static int GetCount(this string msg)
        {
            Console.WriteLine(msg);
            return GetInt("Count => ");
        }
        static double GetDouble(string msg)
        {
            var cursurTop = Console.CursorTop;
            var cursurLeft = Console.CursorLeft;
            while (true)
            {
                Console.Write(msg);
                var input = Console.ReadLine();
                if (double.TryParse(input, out double num))
                {
                    FormatResualt(cursurTop, cursurLeft, msg + num.ToString());
                    return num;
                }
            }
        }
        static int GetInt(string msg)
        {
            var cursurTop = Console.CursorTop;
            var cursurLeft = Console.CursorLeft;
            while (true)
            {
                Console.Write(msg);
                var input = Console.ReadLine();
                if (int.TryParse(input, out int num))
                {
                    FormatResualt(cursurTop, cursurLeft, msg + num.ToString());
                    return num;
                }
            }
        }

        internal static void Spacing()
        {
            Console.WriteLine();
            Console.WriteLine(new string('═', Console.WindowWidth));
            Console.ForegroundColor = Design.Colors("special");
            Console.WriteLine(new string('☼', Console.WindowWidth));
            Console.ResetColor();
            Console.WriteLine(new string('═', Console.WindowWidth));
            Console.WriteLine();
        }
    }
}