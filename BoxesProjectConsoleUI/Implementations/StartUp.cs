using BoxesPojectShared.Entities;
using BoxesPojectShared.Interfaces;
using BoxesProjectData.Repositories;
using BoxesProjectLogic;
using BoxesProjectLogic.DataModels;
using DataStructures;
using DataStructures.Abstractions;
using DependencyInjection.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BoxesProjectConsoleUI.Implementations
{
    class StartUp : IStartup
    {
        public void ConfigureDependencies(IInversionOfControl container)
        {
            container.AddSingleton<IConfiguration, Configurations>()
                     .AddSingleton<IBinarySearchTree<BottomSizeTreeDataModel>, BinarySearchTree<BottomSizeTreeDataModel>>()
                     .AddSingleton<ILinked_List<TimeListDataModel>, Linked_List<TimeListDataModel>>()
                     .AddSingleton<IRepository<Box>, BoxesRepository>()
                     .RegisterType<ILogger, ConsoleLogger>()
                     .RegisterType<IUserInteractions, UserInteractions>()
                     .AddSingleton<IManager, Manager>();
        }

        public void ConfigureStaticData(IConfiguration configuration)
        {
            configuration
                .SetMaxSplits(10)
                .SetMaxSizeDiff(5)
                .SetDaysToStockRefresh(1)
                .SetMaxCount(1000);
        }
    }
}
