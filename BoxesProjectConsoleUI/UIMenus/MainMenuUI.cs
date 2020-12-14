using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxesProjectConsoleUI.UIMenus
{
    class MainMenuUI :IUIMenu
    {
        enum MainMenuActions
        {
            Stock_Management = 1,
            Purchase,
            Display_System_Configurations,
            Exit
        }
        public void Show()
        {
            //Greet
            IUIMenu activeMenu;
            //run loop
            while (true)
            {
                Console.Clear();
                DisplayHelper.Greet();
                MainMenuActions selectedAction = DisplayHelper.GetAction<MainMenuActions, MainMenuUI>();
                switch (selectedAction)
                {
                    case MainMenuActions.Stock_Management:
                        activeMenu = new StockManagementUI();
                        break;
                    case MainMenuActions.Purchase:
                        activeMenu = new PurchaseUI();
                        break;
                    case MainMenuActions.Display_System_Configurations:
                        activeMenu = new ConfigurationUI();
                        break;
                    case MainMenuActions.Exit:
                        DisplayHelper.Close();
                        return;
                    default:
                        DisplayHelper.Invalid();
                        continue;
                }
                activeMenu.Show();
                DisplayHelper.Spacing();
            }
        }
    }
}
