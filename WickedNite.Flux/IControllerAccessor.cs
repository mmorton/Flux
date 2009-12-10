using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedNite.Flux
{
    //TODO: change to IControllerAccessor
    public interface IControllerAccessor
    {
        IView GetView(IController instance);
        void SetView(IController instance, IView value);
        object GetPropertyBag(IController instance);
        void SetPropertyBag(IController instance, object value);
    }
}
