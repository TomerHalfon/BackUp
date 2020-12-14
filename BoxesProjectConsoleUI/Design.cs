using BoxesProjectConsoleUI.UIMenus;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxesProjectConsoleUI
{
    static class Design
    {
        const ConsoleColor DEFAULT_COLOR = ConsoleColor.White;
        const Format DEFAULT_FORMAT = Format.Default;

        public static ConsoleTable GetStarterBoxesTable
        {
            get
            {
                var table = new ConsoleTable("Id", "X", "Y", "Count", "Last bought at");
                table.Configure(options => options.EnableCount = false);
                return table;
            }
        }

        static Dictionary<Type, ConsoleColor> MenuColors { get; } = new Dictionary<Type, ConsoleColor>();
        static Dictionary<string, ConsoleColor> ColorDictionary { get; } = new Dictionary<string, ConsoleColor>();
        static Dictionary<string, ConsoleTables.Format> FormatDictionary { get; } = new Dictionary<string, ConsoleTables.Format>();

        static Design()
        {
            LoadColors();
            LoadFormats();
        }

        /// <summary>
        /// Get a color based on the ui
        /// </summary>
        /// <typeparam name="UIMenu"></typeparam>
        /// <returns></returns>
        public static ConsoleColor Colors<UIMenu>() where UIMenu : IUIMenu => MenuColors.TryGetValue(typeof(UIMenu), out ConsoleColor color) ? color : DEFAULT_COLOR;
        /// <summary>
        /// Get a color by key
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ConsoleColor Colors(string name) => ColorDictionary.TryGetValue(name.ToLower(), out ConsoleColor color) ? color : DEFAULT_COLOR;
        /// <summary>
        /// Get format based on name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Format Formats(string name) => FormatDictionary.TryGetValue(name.ToLower(), out Format format) ? format : DEFAULT_FORMAT;


        // Here you can load colors
        private static void LoadFormats()
        {
            FormatDictionary.Add("menu", Format.Alternative);
            FormatDictionary.Add("boxes", Format.MarkDown);
            FormatDictionary.Add("config", Format.Minimal);
        }
        private static void LoadColors()
        {
            MenuColors.Add(typeof(StockManagementUI), ConsoleColor.Yellow);
            MenuColors.Add(typeof(PurchaseUI), ConsoleColor.Green);
            MenuColors.Add(typeof(MainMenuUI), ConsoleColor.Cyan);

            ColorDictionary.Add("logo", ConsoleColor.Blue);
            ColorDictionary.Add("logobackground", ConsoleColor.White);
            ColorDictionary.Add("special", ConsoleColor.Yellow);
            ColorDictionary.Add("erorr", ConsoleColor.Red);
            ColorDictionary.Add("confirmation", ConsoleColor.Green); 
            ColorDictionary.Add("neutral", ConsoleColor.DarkYellow); 
        }
    }
}
