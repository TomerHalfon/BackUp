using System;
using System.Collections.Generic;
using System.Text;

namespace AppBuilder.Abstractions
{
    public interface IIocContainer
    {
        IIocContainer AddSingleton<TInterface, TImplementation>() where TImplementation : TInterface;
        IIocContainer RegisterType<TInterface, TImplementation>() where TImplementation : TInterface;

        T GetInstance<T>();
    }
}
