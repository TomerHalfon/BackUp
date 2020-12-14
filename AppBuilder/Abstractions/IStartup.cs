using System;
using System.Collections.Generic;
using System.Text;

namespace AppBuilder.Abstractions
{
    public interface IStartup
    {
        void ConfigureDependencies(IIocContainer container);
        void ConfigureStaticData(IConfiguration configuration);
    }
}
