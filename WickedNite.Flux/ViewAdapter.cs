using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedNite.Commons.MuRail
{
    public class ViewAdapter<TPropertyBag> : IViewAdapter
        where TPropertyBag : class
    {
        #region IViewAdapter Members

        public object GetPropertyBag(object instance)
        {
            return (instance != null) ? ((IView<TPropertyBag>)instance).PropertyBag : null;
        }

        public void SetPropertyBag(object instance, object value)
        {
            if (instance != null) ((IView<TPropertyBag>)instance).PropertyBag = (TPropertyBag)value;
        }

        #endregion
    }
}
