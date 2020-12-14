using System;

namespace BoxesProjectConsoleUI.UIMenus
{
    internal class ManualDiscardOfOldBoxesUI : IUIMenu
    {
        public void Show()
        {
            var oldCount = UIDataManager.AutomaticallyDiscardedBoxes.Count;
            Console.WriteLine("\nDiscarding old Boxes...");
            UIDataManager.Manager.DeletionTimerCallBack(null);
            if (UIDataManager.AutomaticallyDiscardedBoxes.Count != oldCount)
                Console.WriteLine("Finished Discarding old boxes, Go to \"Show Auto Discarded Boxes\" to see which boxes were discarded");
            else Console.WriteLine($"No Boxes older than {UIDataManager.Manager.DeletingOlderThan.Date.ToShortDateString()}");
            Console.WriteLine();
            DisplayHelper.HoldForAction();
        }
    }
}