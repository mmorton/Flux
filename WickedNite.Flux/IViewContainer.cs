using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedNite.Flux
{
    public interface IViewContainer
    {
        event ViewEventHandler ViewClosed;
    }
}
