namespace BoxesProjectConsoleUI.UIMenus
{
    internal class BuyBoxForPresentUI : IUIMenu
    {
        public void Show()
        {
            System.Console.WriteLine();
            var (x, y) = "Please enter the size of the present you want to pack => ".GetSize();
            var count = "How many present do you want to pack => ".GetCount();
            System.Console.WriteLine();
            if (count == 1)
            {
                var box = UIDataManager.Manager.BuyBoxForPresent(x, y);
                if(box != null)
                    DisplayHelper.PrintBox(box);
            }
            else UIDataManager.Manager.BuyMultipleBoxesForPresent(x, y, count);
            System.Console.WriteLine();
            DisplayHelper.HoldForAction();

        }
    }
}