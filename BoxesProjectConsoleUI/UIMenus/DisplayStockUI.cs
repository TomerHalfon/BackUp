namespace BoxesProjectConsoleUI.UIMenus
{
    internal class DisplayStockUI : IUIMenu
    {
        
        public void Show()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("The Stock:");
            var stock = UIDataManager.Manager.GetStock();
            DisplayHelper.PrintBoxes(stock);
            System.Console.WriteLine();
            DisplayHelper.HoldForAction();

        }
    }
}