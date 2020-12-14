namespace BoxesProjectConsoleUI.UIMenus
{
    internal class BoxLookupUI : IUIMenu
    {
        public void Show()
        {
            System.Console.WriteLine();
           var searchTerm = "Enter the size of the box to lookup => ".GetSize();
            System.Console.WriteLine();
            var box = UIDataManager.Manager.GetBox(searchTerm.X, searchTerm.Y);
            if (box is null) System.Console.WriteLine("No such box");
            else DisplayHelper.PrintBox(box);
            System.Console.WriteLine();
            DisplayHelper.HoldForAction();

        }
    }
}