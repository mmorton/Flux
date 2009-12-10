using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WickedNite.Flux
{
    public interface IActionInvoker
    {
        void Invoke(object presenter, MethodInfo method, IDictionary<string, object> parameters);
    }
}
