using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WickedNite.Commons.MuRail
{
    public interface IController
    {
                
    }

    public interface IController<TView, TPropertyBag> : IController where TView : IView<TPropertyBag>
    {
        TView View { get; set; }
        TPropertyBag PropertyBag { get; set; }
    }
}
