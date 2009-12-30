using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedNite.Flux
{
    public class ViewEventArgs : EventArgs
    {
        public object View { get; set; }
    }
}
