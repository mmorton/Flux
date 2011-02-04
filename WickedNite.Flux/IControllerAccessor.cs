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
        object GetViewModel(IController instance);
        void SetViewModel(IController instance, object value);
    }
}
