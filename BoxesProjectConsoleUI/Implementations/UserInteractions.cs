using BoxesPojectShared.Entities;
using BoxesPojectShared.Interfaces;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxesProjectConsoleUI.Implementations
{
    class UserInteractions : IUserInteractions
    {

        public bool RequestConfirmation(List<Box> order)
        {
            Console.WriteLine("\nPlease Confirm Order\n");
            DisplayHelper.PrintBoxes(order);
            Console.Write("Do you approve this transaction ? Y/N => ");
            string response = string.Empty;
            while (string.IsNullOrEmpty(response))
            {
                switch (Console.ReadLine().ToUpper())
                {
                    case "Y":
                        return true;
                    case "N":
                        return false;
                    default:
                        Console.ForegroundColor = Design.Colors("erorr");
                        Console.WriteLine("Invalid answer");
                        Console.ResetColor();
                        break;
                }
            }
            return true;
        }
    }
}
