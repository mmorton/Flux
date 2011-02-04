using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WickedNite.Flux
{
    public interface IController
    {
                
    }

    public interface IController<TView, TViewModel> : IController where TView : IView<TViewModel>
    {
        TView View { get; set; }
        TViewModel ViewModel { get; set; }
    }
}
