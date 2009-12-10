using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WickedNite.Flux
{
    public interface ICommandProvider
    {
        ICommand GetCommand(string name);
        ICommand GetCommand(string name, Type type);
    }
}
