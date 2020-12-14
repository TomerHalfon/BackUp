using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxesProjectConsoleUI.UIMenus
{
    /// <summary>
    /// Handle the stock
    /// </summary>
    class StockManagementUI : IUIMenu
    {
        enum StockMnagementActions
        {
            Add_To_Stock = 1,
            Display_Stock,
            Box_Lookup,
            Show_Auto_Discarded_Boxes,
            Manual_Discard_Of_Old_Boxes,
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
                StockMnagementActions selectedAction = DisplayHelper.GetAction<StockMnagementActions, StockManagementUI>();
                switch (selectedAction)
                {
                    case StockMnagementActions.Add_To_Stock:
                        activeMenu = new AddToSockUI();
                        break;
                    case StockMnagementActions.Display_Stock:
                        activeMenu = new DisplayStockUI();
                        break;
                    case StockMnagementActions.Box_Lookup:
                        activeMenu = new BoxLookupUI();
                        break;
                    case StockMnagementActions.Back:
                        DisplayHelper.Close();
                        return;
                    case StockMnagementActions.Show_Auto_Discarded_Boxes:
                        activeMenu = new AutoDiscardedBoxesUI();
                        break;
                    case StockMnagementActions.Manual_Discard_Of_Old_Boxes:
                        activeMenu = new ManualDiscardOfOldBoxesUI();
                        break;
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
