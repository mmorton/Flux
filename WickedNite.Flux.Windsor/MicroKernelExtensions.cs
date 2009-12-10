using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel;

namespace WickedNite.Flux.Windsor
{
    public static class MicroKernelExtensions
    {
        public static bool HasComponent<T>(this IKernel kernel)
        {
            return kernel.HasComponent(typeof(T));
        }
    }
}
