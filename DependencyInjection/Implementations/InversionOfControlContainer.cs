using DependencyInjection.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DependencyInjection.Implementations
{
   public class InversionOfControlContainer:IInversionOfControl
    {

        //A dictionary to house the registerd types
        private readonly Dictionary<Type, Func<object>> _registeredTypes = new Dictionary<Type, Func<object>>();

        //Register a regular intrface's implementation
        public IInversionOfControl RegisterType<TInterface, TImplementation>() where TImplementation : TInterface
        {
            if (_registeredTypes.TryGetValue(typeof(TInterface), out _))
            {
                throw new ArgumentException($"Type {typeof(TInterface)} can only be registerd once");
            }
            //register the type
            _registeredTypes.Add(typeof(TInterface), () => GetInstance(typeof(TImplementation)));
            return this;
        }
        public IInversionOfControl AddSingleton<TInterface, TImplementation>() where TImplementation : TInterface
        {
            if (_registeredTypes.TryGetValue(typeof(TInterface), out _))
            {
                throw new ArgumentException("Singleton can only be registerd once");
            }

            //register the singleton

            var instance = GetInstance(typeof(TImplementation));

            _registeredTypes.Add(typeof(TInterface), () => instance);
            return this;
        }
        // a generic way of requesting an instance
        public T GetInstance<T>()
        {
            return (T)GetInstance(typeof(T));
        }
        /// <summary>
        /// Will return an instace of this type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object GetInstance(Type type)
        {
            //if the type was registerd
            if (_registeredTypes.TryGetValue(type, out Func<object> value))
            {
                return value();
            }
            //get largest ctor
            var constructor = type.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .First();
            //create instances for each of the params
            var args = constructor.GetParameters()
                .Select(param => GetInstance(param.ParameterType))
                .ToArray();

            //activate the ctor with the args
            return Activator.CreateInstance(type, args);
        }
    }
}
