using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedNite.Flux
{
    public class ViewAccessor<TViewModel> : IViewAccessor
        where TViewModel : class
    {
        #region IViewAdapter Members

        public object GetViewModel(IView instance)
        {
            return (instance != null) ? ((IView<TViewModel>)instance).ViewModel : null;
        }

        public void SetViewModel(IView instance, object value)
        {
            if (instance != null) ((IView<TViewModel>)instance).ViewModel = (TViewModel)value;
        }

        #endregion
    }
}
