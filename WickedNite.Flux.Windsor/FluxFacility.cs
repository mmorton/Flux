using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel;
using Castle.Core;
using Castle.MicroKernel.Registration;
using WickedNite.Flux;

namespace WickedNite.Flux.Windsor
{    
    public class FluxFacility : IFacility
    {
        public IKernel Kernel { get; set; }

        #region IFacility Members

        public void Init(IKernel kernel, Castle.Core.Configuration.IConfiguration facilityConfig)
        {
            Kernel = kernel;

            if (Kernel.HasComponent(typeof(ViewAccessor<>)) == false)
                Kernel.Register(Component.For(typeof(ViewAccessor<>)).LifeStyle.Singleton);

            if (Kernel.HasComponent(typeof(ControllerAccessor<,>)) == false)
                Kernel.Register(Component.For(typeof(ControllerAccessor<,>)).LifeStyle.Singleton);

            if (Kernel.HasComponent<IActionInvoker>() == false)
                Kernel.Register(Component.For<IActionInvoker>().ImplementedBy<DefaultActionInvoker>().LifeStyle.Singleton);

            if (Kernel.HasComponent<ICommandProvider>() == false)
                Kernel.Register(Component.For<ICommandProvider>().ImplementedBy<DefaultCommandProvider>().LifeStyle.Singleton);            

            if (Kernel.HasComponent<IControllerAdapter>() == false)
                Kernel.Register(Component.For<IControllerAdapter>().ImplementedBy<DefaultControllerAdapter>().LifeStyle.Singleton);

            if (Kernel.HasComponent<IControllerLifecycleManager>() == false)
                Kernel.Register(Component.For<IControllerLifecycleManager>().ImplementedBy<MicroKernelControllerLifecycleManager>().LifeStyle.Singleton);

            if (Kernel.HasComponent<IAutoRegistration>() == false)
                Kernel.Register(Component.For<IAutoRegistration>().ImplementedBy<MicroKernelAutoRegistration>().LifeStyle.Singleton);

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

            var controllerAccessor = Kernel.Resolve(
                typeof(ControllerAccessor<,>)
                    .MakeGenericType(
                        types.ElementAt(0),
                        types.ElementAt(1)
                    )
            ) as IControllerAccessor;

            var viewAccessor = Kernel.Resolve(
                typeof(ViewAccessor<>)
                    .MakeGenericType(
                        types.ElementAt(1)
                    )
            ) as IViewAccessor;

            if (controllerAccessor != null && viewAccessor != null)
            {
                var view = controllerAccessor.GetView((IController)instance);
                var viewModel = controllerAccessor.GetViewModel((IController)instance);

                viewAccessor.SetViewModel(view, viewModel);

                var manager = Kernel.Resolve<IControllerAdapter>();
                if (manager != null)
                    manager.Build((IController)instance, (IView)view);
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

            var viewModelType = model.Implementation
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IView<>))
                .Select(i => i.GetGenericArguments().First())
                .First();

            var dependency = model.Dependencies
                .Where(d => d.TargetType == viewModelType)
                .FirstOrDefault();

            if (dependency != null)
                model.Dependencies.Remove(dependency);

            var property = model.Properties
                .Where(p => p.Dependency.TargetType == viewModelType)
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
