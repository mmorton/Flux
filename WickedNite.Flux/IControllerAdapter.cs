using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedNite.Flux
{
    public interface IControllerAdapter
    {
        void Build(IController controller, IView view);
    }
}
