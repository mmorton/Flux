using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WickedNite.Commons.MuRail
{
    public interface ICommandManager
    {
        void Bind(object view, object controller);
    }
}
