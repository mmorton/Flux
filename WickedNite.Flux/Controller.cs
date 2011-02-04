using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace WickedNite.Flux
{
    public abstract class Controller<TView, TViewModel> : IController<TView, TViewModel>
        where TView : IView<TViewModel>
    {
        public TView View { get; set; }
        public TViewModel ViewModel { get; set; }        
    }

    
}
