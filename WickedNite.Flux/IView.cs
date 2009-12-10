using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;

namespace WickedNite.Flux
{    
    public interface IView
    {

    }

    public interface IView<TPropertyBag> : IView
    {
        TPropertyBag PropertyBag { get; set; }
    }    
}
