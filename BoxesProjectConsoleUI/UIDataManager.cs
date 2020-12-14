using BoxesPojectShared.Interfaces;
using BoxesProjectLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxesProjectConsoleUI
{
    public static class UIDataManager
    {
        public static IManager Manager { get; set; }
        public static IConfiguration Config { get; internal set; }
        public static List<BoxesPojectShared.Entities.Box> AutomaticallyDiscardedBoxes { get; set; } = new List<BoxesPojectShared.Entities.Box>();
    }
}
