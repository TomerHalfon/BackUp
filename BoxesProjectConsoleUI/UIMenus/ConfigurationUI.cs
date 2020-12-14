using BoxesProjectConsoleUI.UIMenus;
using BoxesPojectShared.Entities;
using ConsoleTables;
using System.Linq;
using System.Reflection;

namespace BoxesProjectConsoleUI.UIMenus
{
    internal class ConfigurationUI : IUIMenu
    {
        public void Show()
        {
            System.Console.WriteLine();
            var table = new ConsoleTable("Key", "Value", "Type");
            typeof(Configurations)
                .GetProperties()
                .ToList()
                .ForEach(prop =>table.AddRow(prop.Name, prop.GetValue(UIDataManager.Config).ToString(), GetSymbol(prop)));
        
            table.AddRow("Deleting old data at", UIDataManager.Manager.DeletingAt.ToShortTimeString(),"");
            table.AddRow("Deleting older than", UIDataManager.Manager.DeletingOlderThan.Date.ToShortDateString(),"");
            System.Console.WriteLine("Config => ");
            table.Write(Format.Minimal);
            System.Console.WriteLine();
            DisplayHelper.HoldForAction();
        }
        string GetSymbol(PropertyInfo prop)
        {
            return (prop.GetCustomAttribute(typeof(SymbolAttribute), false) as SymbolAttribute)?.Symbol ?? "";
        }
    }
}