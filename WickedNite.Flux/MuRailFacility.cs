using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel;
using Castle.Core;
using Castle.MicroKernel.Registration;

namespace WickedNite.Commons.MuRail
{    
    public class MuRailFacility : IFacility
    {
        public IKernel Kernel { get; set; }

        #region IFacility Members

        public void Init(IKernel kernel, Castle.Core.Configuration.IConfiguration facilityConfig)
        {
            Kernel = kernel;

            Kernel.Register(
                Component
                    .For(typeof(ViewAdapter<>))
                    .LifeStyle
                        .Singleton
            );

            Kernel.Register(
                Component
                    .For(typeof(ControllerAdapter<,>))
                    .LifeStyle
                        .Singleton
            );

            Kernel.ComponentModelCreated += new ComponentModelDelegate(Kernel_ComponentModelCreated);
            Kernel.ComponentRegistered += new ComponentDataDelegate(Kernel_ComponentRegistered);
            Kernel.ComponentCreated += new ComponentInstanceDelegate(Kernel_ComponentCreated);
        }

        void Kernel_ComponentCreated(ComponentModel model, object instance)
        {
            if (typeof(IController).IsAssignableFrom(model.Implementation))
                HandleControllerComponentCreated(model, instance);
        }

        private void HandleControllerComponentCreated(ComponentModel model, object instance)
        {
            var types = model.Implementation
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IController<,>))
                .Select(i => i.GetGenericArguments())
                .First();

            var controllerAdapter = Kernel.Resolve(
                typeof(ControllerAdapter<,>)
                    .MakeGenericType(
                        types.ElementAt(0),
                        types.ElementAt(1)
                    )
            ) as IControllerAdapter;

            var viewAdapter = Kernel.Resolve(
                typeof(ViewAdapter<>)
                    .MakeGenericType(
                        types.ElementAt(1)
                    )
            ) as IViewAdapter;

            if (controllerAdapter != null && viewAdapter != null)
            {
                var view = controllerAdapter.GetView(instance);
                var propertyBag = controllerAdapter.GetPropertyBag(instance);

                viewAdapter.SetPropertyBag(view, propertyBag);

                var manager = Kernel.Resolve<ICommandManager>();
                if (manager != null)
                    manager.Bind(view, instance);
            }
        }

        void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            if (typeof(IController).IsAssignableFrom(handler.Service))
                HandleControllerComponentRegistered(key, handler);
        }

        private void HandleControllerComponentRegistered(string key, IHandler handler)
        {
            if (handler.Service.IsGenericType && handler.Service.GetGenericTypeDefinition() == typeof(IController<,>))
                return;

            var controller = handler.ComponentModel.Implementation.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IController<,>))
                .FirstOrDefault();

            if (controller != null) Kernel.RegisterHandlerForwarding(controller, key);
        }

        void Kernel_ComponentModelCreated(ComponentModel model)
        {
            if (typeof(IView).IsAssignableFrom(model.Implementation))
                HandleViewComponentModelCreated(model);
        }

        private void HandleViewComponentModelCreated(ComponentModel model)
        {
            model.Constructors.Clear();
            model.CustomComponentActivator = typeof(ViewComponentActivator);

            var propertyBagType = model.Implementation
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IView<>))
                .Select(i => i.GetGenericArguments().First())
                .First();

            var dependency = model.Dependencies
                .Where(d => d.TargetType == propertyBagType)
                .FirstOrDefault();

            if (dependency != null)
                model.Dependencies.Remove(dependency);

            var property = model.Properties
                .Where(p => p.Dependency.TargetType == propertyBagType)
                .FirstOrDefault();

            if (property != null)
                model.Properties.Remove(property);
        }

        public void Terminate()
        {
            Kernel.ComponentModelCreated -= new ComponentModelDelegate(Kernel_ComponentModelCreated);
            Kernel.ComponentRegistered -= new ComponentDataDelegate(Kernel_ComponentRegistered);
            Kernel.ComponentCreated -= new ComponentInstanceDelegate(Kernel_ComponentCreated);
        }

        #endregion
    }
}
