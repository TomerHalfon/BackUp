using System;

namespace BoxesProjectConsoleUI.UIMenus
{
    internal class AddToSockUI : IUIMenu
    {
        public void Show()
        {
            Console.WriteLine();
            var (X, Y) = "Please enter the size of the box".GetSize();
            var count = "Please enter the amount to add to this size".GetCount();
            Console.WriteLine();
            UIDataManager.Manager.AddToStock(X, Y, count);
            Console.WriteLine();
            DisplayHelper.HoldForAction();
        }
    }
}