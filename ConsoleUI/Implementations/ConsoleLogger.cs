using BoxesPojectShared.Entities;
using BoxesPojectShared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.Implementations
{
    public class ConsoleLogger : ILogger
    {
        public void Log(LogData log)
        {
            Console.WriteLine("Results");
            if(log.Box != null)
                PrintBox(log.Box);
            Console.WriteLine(log.Message);
            if(log.Status != null)
            {
                Console.WriteLine($"Status: {log.Status}");
            }
        }
        void PrintBox(Box box)
        {
            Console.WriteLine($"X => {box.X} Y => {box.Y} Count => {box.Count} Last bought at {box.TimeLastPurchase.ToShortDateString()}");
        }
    }
}
