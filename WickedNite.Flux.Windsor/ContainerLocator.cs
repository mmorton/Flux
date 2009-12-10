using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using System.Windows;

namespace WickedNite.Flux.Windsor
{
    public delegate IWindsorContainer ContainerProvider();

    public class ContainerLocator
    {
        public static ContainerProvider _provider;

        public static IWindsorContainer Container
        {
            get { return _provider(); }
        }

        static ContainerLocator()
        {
            _provider = () =>
            {
                var accessor = Application.Current as IContainerAccessor;
                if (accessor == null) return null;

                return accessor.Container;
            };
        }
    }
}
