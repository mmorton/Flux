using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel;

namespace WickedNite.Flux.Windsor
{
    public class MicroKernelControllerLifecycleManager : IControllerLifecycleManager
    {
        public IKernel Kernel { get; set; }

        public MicroKernelControllerLifecycleManager(IKernel kernel)
        {
            Kernel = kernel;
        }

        #region IControllerLifecycleManager Members

        public void Release(IController controller)
        {
            var types = controller.GetType()
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
                var view = controllerAccessor.GetView(controller);
                var viewModel = controllerAccessor.GetViewModel(controller);

                Kernel.ReleaseComponent(view);
                Kernel.ReleaseComponent(viewModel);
            }

            Kernel.ReleaseComponent(controller);
        }

        #endregion
    }
}
