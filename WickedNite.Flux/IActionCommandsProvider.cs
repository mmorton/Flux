using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Castle.Windsor;
using Castle.Core;

namespace WickedNite.Commons.MuRail
{
    public interface IActionCommandsProvider
    {
        IDictionary<string, ICommand> GetCommands(Type controllerType);
    }
}
