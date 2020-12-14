namespace BoxesProjectConsoleUI.UIMenus
{
    internal class BuyBoxUI : IUIMenu
    {
        public void Show()
        {
            System.Console.WriteLine();
            var size = "Enter the size of the box => ".GetSize();
            var count = "Enter how many => ".GetCount();
            System.Console.WriteLine();
            var res = UIDataManager.Manager.BuyExactBoxSize(size.X, size.Y, count);
            if (res) System.Console.WriteLine("Success");
            else System.Console.WriteLine("Failed");
            System.Console.WriteLine();
            DisplayHelper.HoldForAction();

        }
    }
}