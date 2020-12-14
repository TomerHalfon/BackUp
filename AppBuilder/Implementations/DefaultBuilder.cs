using AppBuilder.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppBuilder.Implementations
{
    public class DefaultBuilder<TIConfig,TConfig> : IBuilder where TConfig:TIConfig where TIConfig:IConfig
    {
        private IApp _app;
        public IApp Build()
        {
            //run startUp
            _app.Startup.ConfigureDependencies(_app.IOC);
            _app.Configuration = _app.IOC.GetInstance<TIConfig>();
            _app.Startup.ConfigureStaticData(_app.Configuration);
            return _app;
        }

        public IBuilder IOC<TInversionOfControl>() where TInversionOfControl : IIocContainer, new()
        {
            //load default dependencies
            _app.IOC = new TInversionOfControl();
            //Must add singleton for IConfiguration
            _app.IOC.AddSingleton<TIConfig, TConfig>();
            return this;
        }

        public IBuilder StartUp<TStartup>() where TStartup : IStartup, new()
        {
            _app.Startup = new TStartup();
            return this;
        }
    }
}
