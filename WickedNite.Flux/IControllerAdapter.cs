using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedNite.Commons.MuRail
{
    public interface IControllerAdapter
    {
        object GetView(object instance);
        void SetView(object instance, object value);
        object GetPropertyBag(object instance);
        void SetPropertyBag(object instance, object value);
    }
}
