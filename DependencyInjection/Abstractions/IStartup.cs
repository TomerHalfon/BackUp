using BoxesPojectShared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjection.Abstractions
{
    public interface IStartup
    {
        void ConfigureDependencies(IInversionOfControl container);
        void ConfigureStaticData(IConfiguration configuration);
    }
}
