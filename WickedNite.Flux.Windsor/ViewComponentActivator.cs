using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.ComponentActivator;
using Castle.Core;
using Castle.MicroKernel;

namespace WickedNite.Flux.Windsor
{
    public class ViewComponentActivator : DefaultComponentActivator
    {
        public Type PropertyBagType { get; set; }
        public Type ViewType { get; set; }

        public ViewComponentActivator(ComponentModel model, IKernel kernel, ComponentInstanceDelegate onCreation, ComponentInstanceDelegate onDestruction)
            : base(model, kernel, onCreation, onDestruction)
        {
            ViewType = Model.Implementation;
            PropertyBagType = Model.Implementation
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IView<>))
                .Select(i => i.GetGenericArguments().First())
                .First();
        }

        protected override object Instantiate(CreationContext context)
        {
            if (typeof(IController).IsAssignableFrom(context.Handler.Service)) return base.Instantiate(context);

            var controller = Kernel.Resolve(typeof(IController<,>).MakeGenericType(ViewType, PropertyBagType));
            if (controller == null) return null;

            var accessor = Kernel.Resolve(typeof(ControllerAccessor<,>).MakeGenericType(ViewType, PropertyBagType)) as IControllerAccessor;
            if (accessor == null) return null;

            return accessor.GetView((IController)controller);
        }
    }
}
