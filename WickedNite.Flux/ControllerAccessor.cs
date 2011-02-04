using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedNite.Flux
{
    public class ControllerAccessor<TView, ViewModel> : IControllerAccessor
        where TView : class, IView<ViewModel>
        where ViewModel : class
    {
        #region IControllerAdapter Members

        public IView GetView(IController instance)
        {
            return (instance != null) ? ((IController<TView, ViewModel>)instance).View : null;
        }

        public void SetView(IController instance, IView value)
        {
            if (instance != null) ((IController<TView, ViewModel>)instance).View = (TView)instance;
        }

        public object GetViewModel(IController instance)
        {
            return (instance != null) ? ((IController<TView, ViewModel>)instance).ViewModel : null;
        }

        public void SetViewModel(IController instance, object value)
        {
            if (instance != null) ((IController<TView, ViewModel>)instance).ViewModel = (ViewModel)instance;
        }

        #endregion
    }
}
