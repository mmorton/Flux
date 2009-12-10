using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace WickedNite.Flux
{
    public class FluxStarter
    {
        public static void Initialize()
        {
            var configuration = ConfigurationManager.GetSection("flux") as FluxConfiguration;
            if (configuration != null)
            {
                foreach (var moduleConfiguration in configuration.Modules)
                {
                    if (typeof(IModule).IsAssignableFrom(moduleConfiguration.Type))
                    {
                        var module = Activator.CreateInstance(moduleConfiguration.Type) as IModule;
                        if (module != null)
                            module.Initialize();
                    }
                }
            }
        }
    }
}
