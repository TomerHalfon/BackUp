using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjection.Abstractions
{
    public interface IInversionOfControl
    {
        IInversionOfControl AddSingleton<TInterface, TImplementation>() where TImplementation : TInterface;
        IInversionOfControl RegisterType<TInterface, TImplementation>() where TImplementation : TInterface;
        T GetInstance<T>();
    }
}
