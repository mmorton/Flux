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

    public interface IView<TViewModel> : IView
    {
        TViewModel ViewModel { get; set; }
    }    
}
