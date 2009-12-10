using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedNite.Flux
{
    public interface IViewAccessor
    {
        object GetPropertyBag(IView instance);
        void SetPropertyBag(IView instance, object value);
    }
}
