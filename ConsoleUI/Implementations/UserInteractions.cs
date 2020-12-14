using BoxesPojectShared.Interfaces;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.Implementations
{
    public class UserInteractions : IUserInteractions
    {
        public bool RequestConfirmation(List<BoxesPojectShared.Entities.Box> order)
        {
            Console.WriteLine("Please Confirm Order");
            var col = order.First().GetType().GetProperties().Select(p => p.Name).ToArray();
            var table = new ConsoleTable(col);

            //foreach (var b in order)
            //{
            //    var row = b.GetType().GetProperties().Select(p => p.GetValue(b)).ToArray();
            //}
            order.ForEach(b => table.AddRow(b.GetType().GetProperties().Select(p => p.GetValue(b)).ToArray()));
            table.Write(Format.Minimal);
            Console.Write("Do you approve this transaction ? Y/N => ");
            string response = "";

            while (string.IsNullOrEmpty(response))
            {
                switch (Console.ReadLine().ToUpper())
                {
                    case "Y":
                        return true;
                    case "N":
                        return false;
                    default:
                        Console.WriteLine("Invalid answer");
                        break;
                }
            }
            return true;
        }
    }
}
