using BoxesProjectConsoleUI.UIMenus;
using System;

namespace BoxesProjectConsoleUI.UIMenus
{
    internal class PurchaseUI : IUIMenu
    {
        enum PurchaseActions
        {
            Buy_Box = 1,
            Buy_Box_For_Present,
            Back
        }
        public void Show()
        {
            IUIMenu activeMenu;
            DisplayHelper.Spacing();
            var startingPosition = (Console.CursorLeft, Console.CursorTop);

            //run loop
            while (true)
            {
                PurchaseActions selectedAction = DisplayHelper.GetAction<PurchaseActions, PurchaseUI>();
                switch (selectedAction)
                {
                    case PurchaseActions.Buy_Box:
                        activeMenu = new BuyBoxUI();
                        break;
                    case PurchaseActions.Buy_Box_For_Present:
                        activeMenu = new BuyBoxForPresentUI();
                        break;
                    case PurchaseActions.Back:
                        DisplayHelper.Close();
                        return;
                    default:
                        DisplayHelper.Invalid();
                        continue;
                }
                activeMenu.Show();
                //Reset cursor to starting position while clearing thae path
                DisplayHelper.ClearBackTo(startingPosition);
            }
        }
    }
}