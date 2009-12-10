using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Castle.MicroKernel;
using Castle.Windsor;
using System.Windows;

namespace WickedNite.Flux.Windsor
{
    public class WindsorModule : IModule
    {
        public void Initialize()
        {
            var container = ContainerLocator.Container;
            if (container == null) return;

            container.Kernel.AddFacility<FluxFacility>();
        }
    }
}
