using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedNite.Flux
{
    public interface IViewAccessor
    {
        object GetViewModel(IView instance);
        void SetViewModel(IView instance, object value);
    }
}
