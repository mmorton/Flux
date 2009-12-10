using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedNite.Flux
{
    public class ViewAccessor<TPropertyBag> : IViewAccessor
        where TPropertyBag : class
    {
        #region IViewAdapter Members

        public object GetPropertyBag(IView instance)
        {
            return (instance != null) ? ((IView<TPropertyBag>)instance).PropertyBag : null;
        }

        public void SetPropertyBag(IView instance, object value)
        {
            if (instance != null) ((IView<TPropertyBag>)instance).PropertyBag = (TPropertyBag)value;
        }

        #endregion
    }
}
