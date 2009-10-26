using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;

namespace WickedNite.Commons.MuRail.Registration
{
    public static class AutoRegistration
    {
        public static void From(params Assembly[] assemblies)
        {
            var registration = ServiceLocator.Current.GetInstance<IAutoRegistration>();
            registration.From(assemblies);
        }
    }
}
