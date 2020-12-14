using BoxesPojectShared.Interfaces;
using DependencyInjection.Abstractions;
using BoxesPojectShared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures.Abstractions;
using DataStructures;
using BoxesProjectLogic.DataModels;
using BoxesProjectLogic;
using BoxesProjectData.Repositories;

namespace ConsoleUI.Implementations
{
    class StartUp : IStartup
    {
        public void ConfigureDependencies(IInversionOfControl container)
        {
            container
                .AddSingleton<IConfiguration, Configurations>()
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
                .SetMaxSplits(3)
                .SetMaxSizeDiff(5)
                .SetDaysToStockRefresh(3);
        }
    }
}
