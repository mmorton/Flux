using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;

namespace WickedNite.Flux
{
    public static class FluxAutoRegistration
    {
        public static void From(params Assembly[] assemblies)
        {
            var locator = ServiceLocator.Current;
            if (locator == null) return;

            var registration = locator.GetInstance<IAutoRegistration>();
            if (registration != null)
                registration.From(assemblies);
        }
    } 
}
