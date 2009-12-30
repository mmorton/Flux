using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Reflection;

namespace WickedNite.Flux
{
    public interface IActionInvoker
    {
        void Invoke(object controller, MethodInfo method, IDictionary<string, object> parameters);
    }
}
