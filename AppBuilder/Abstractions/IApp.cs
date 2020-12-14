using System;
using System.Collections.Generic;
using System.Text;

namespace AppBuilder.Abstractions
{
   public interface IApp
    {
        IIocContainer IOC { get; set; }
        IStartup Startup { get; set; }
        IConfig Configuration { get; set; }

        void Run();
    }
}
