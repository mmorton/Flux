using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WickedNite.Commons.MuRail
{
    public interface ICommandProvider
    {
        ICommand GetCommand(string name);
        ICommand GetCommand(string name, Type type);
    }
}
