using System;
using System.Linq;

namespace BoxesProjectConsoleUI.UIMenus
{
    internal class AutoDiscardedBoxesUI : IUIMenu
    {
        public void Show()
        {
            System.Console.WriteLine("\nBoxes That where automatically discarded by the software\n");
            var willDeleteIn = UIDataManager.Manager.DeletingAt.Subtract(DateTime.Now);
            if (UIDataManager.AutomaticallyDiscardedBoxes.Count == 0)
                Console.WriteLine($"No Boxes where discarded yet.\nWill Automatically Discard Boxes older than {UIDataManager.Manager.DeletingOlderThan.Date.ToShortDateString()}, In {willDeleteIn.Hours} Hours and {willDeleteIn.Minutes} Minutes");
            else DisplayHelper.PrintBoxes(UIDataManager.AutomaticallyDiscardedBoxes);
            Console.WriteLine();
            Console.WriteLine("If you wish to manualy activate the deletion go to \"Manual Discard Of Old Boxes\"\n");
            DisplayHelper.HoldForAction();
        }
    }
}