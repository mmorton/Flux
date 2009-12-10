using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Core;

namespace WickedNite.Flux.Windsor
{
    public class MicroKernelAutoRegistration : AbstractAutoRegistration
    {
        public IKernel Kernel { get; set; }

        public MicroKernelAutoRegistration(IKernel kernel)
        {
            Kernel = kernel;
        }

        protected override void RegisterController(string name, Type contract, Type implementation)
        {
            Kernel.Register(
                Component.For(contract)
                    .ImplementedBy(implementation)
                    .LifeStyle.Is(LifestyleType.Transient)
                    .Named(name)
            );
        }       

        protected override void RegisterView(string name, Type contract, Type implementation)
        {
            Kernel.Register(
                Component.For(contract)
                    .ImplementedBy(implementation)
                    .LifeStyle.Is(LifestyleType.Transient)
                    .Named(name)
            );
        }

        protected override void RegisterPropertyBag(string name, Type contract, Type implementation)
        {
            if (implementation == null)
                implementation = PropertyBagMaker.Make(contract);

            Kernel.Register(
                Component.For(contract)
                    .ImplementedBy(implementation)
                    .LifeStyle.Is(LifestyleType.Transient)
                    .Named(name)
            );
        }
    }
}
