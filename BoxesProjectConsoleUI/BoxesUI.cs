using BoxesPojectShared.Interfaces;
using BoxesProjectConsoleUI.Implementations;
using BoxesProjectConsoleUI.UIMenus;
using BoxesProjectLogic;
using DependencyInjection.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxesProjectConsoleUI
{
    class BoxesUI
    {
        readonly IStartup _startUp;
        readonly IInversionOfControl _ioc;

        public BoxesUI(IStartup startUp, IInversionOfControl ioc)
        {
            _startUp = startUp;
            _ioc = ioc;
            Init();
        }
        void Init()
        {
            _startUp.ConfigureDependencies(_ioc);
            _startUp.ConfigureStaticData(_ioc.GetInstance<IConfiguration>());
            UIDataManager.Config = _ioc.GetInstance<IConfiguration>();
            UIDataManager.Manager = _ioc.GetInstance<IManager>();

            UIDataManager.Manager.OldBoxesDeletionEvent += (s, deletedBoxes) => 
            { 
                UIDataManager.AutomaticallyDiscardedBoxes.AddRange(deletedBoxes);
            };
        }
        public void Run() => new MainMenuUI().Show();
    }
}
