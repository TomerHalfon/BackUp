using System;
using System.Collections.Generic;
using System.Text;

namespace AppBuilder.Abstractions
{
    public interface IBuilder
    {
        IBuilder IOC<TInversionOfControl>() where TInversionOfControl : IIocContainer, new();
        IBuilder StartUp<TStartup>() where TStartup : IStartup, new();
        IApp Build();
    }
}
