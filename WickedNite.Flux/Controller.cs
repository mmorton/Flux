using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using Castle.Windsor;
using Castle.Core;

namespace WickedNite.Commons.MuRail
{
    public abstract class Controller<TView, TPropertyBag> : IController<TView, TPropertyBag>
        where TView : IView<TPropertyBag>
    {
        public TView View { get; set; }
        public TPropertyBag PropertyBag { get; set; }        
    }

    
}
