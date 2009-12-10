using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace WickedNite.Flux
{
    public interface IAutoRegistration
    {
        void From(params Assembly[] assemblies);        
    }
}
