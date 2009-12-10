using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedNite.Flux
{
    public class ControllerAccessor<TView, TPropertyBag> : IControllerAccessor
        where TView : class, IView<TPropertyBag>
        where TPropertyBag : class
    {
        #region IControllerAdapter Members

        public IView GetView(IController instance)
        {
            return (instance != null) ? ((IController<TView, TPropertyBag>)instance).View : null;
        }

        public void SetView(IController instance, IView value)
        {
            if (instance != null) ((IController<TView, TPropertyBag>)instance).View = (TView)instance;
        }

        public object GetPropertyBag(IController instance)
        {
            return (instance != null) ? ((IController<TView, TPropertyBag>)instance).PropertyBag : null;
        }

        public void SetPropertyBag(IController instance, object value)
        {
            if (instance != null) ((IController<TView, TPropertyBag>)instance).PropertyBag = (TPropertyBag)instance;
        }

        #endregion
    }
}
