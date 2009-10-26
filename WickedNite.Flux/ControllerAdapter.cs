using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedNite.Commons.MuRail
{
    public class ControllerAdapter<TView, TPropertyBag> : IControllerAdapter
        where TView : class, IView<TPropertyBag>
        where TPropertyBag : class
    {
        #region IControllerAdapter Members

        public object GetView(object instance)
        {
            return (instance != null) ? ((IController<TView, TPropertyBag>)instance).View : null;
        }

        public void SetView(object instance, object value)
        {
            if (instance != null) ((IController<TView, TPropertyBag>)instance).View = (TView)instance;
        }

        public object GetPropertyBag(object instance)
        {
            return (instance != null) ? ((IController<TView, TPropertyBag>)instance).PropertyBag : null;
        }

        public void SetPropertyBag(object instance, object value)
        {
            if (instance != null) ((IController<TView, TPropertyBag>)instance).PropertyBag = (TPropertyBag)instance;
        }

        #endregion
    }
}
