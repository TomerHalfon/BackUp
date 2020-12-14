using BoxesPojectShared.Entities;
using BoxesPojectShared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxesProjectConsoleUI.Implementations
{
    /// <summary>
    /// This Hijacks the logging function and displays it on screen insted of loggin to file
    /// </summary>
    class ConsoleLogger : ILogger
    {
        public void Log(LogData log)
        {

            if (log.Status.HasValue)
                Console.ForegroundColor = log.Status.Value ? Design.Colors("confirmation") : Design.Colors("erorr");
            else Console.ForegroundColor = Design.Colors("neutral");
            if (log.Box != null)
            {
                var table = Design.GetStarterBoxesTable;
                table.Columns.Remove("Id");
                table.Columns.Remove("Last bought at");
                table.AddRow(log.Box.X, log.Box.Y, log.Box.Count);
                table.Write(Design.Formats("box"));
            }
            Console.WriteLine(log.Message);
            Console.ResetColor();
        }
    }
}
