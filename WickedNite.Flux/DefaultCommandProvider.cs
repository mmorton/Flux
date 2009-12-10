using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;

namespace WickedNite.Flux
{    
    public class DefaultCommandProvider : ICommandProvider
    {
        protected Dictionary<string, ICommand> NamedCommands { get; set; }
        protected object NamedCommandsSync { get; set; }

        protected Dictionary<Type, Dictionary<string, ICommand>> TypedCommands { get; set; }
        protected object TypedCommandsSync { get; set; }

        public DefaultCommandProvider()
        {
            NamedCommands = new Dictionary<string, ICommand>();
            NamedCommandsSync = new object();

            TypedCommands = new Dictionary<Type, Dictionary<string, ICommand>>();
            TypedCommandsSync = new object();
        }

        #region ICommandProvider Members

        public ICommand GetCommand(string name)
        {
            lock (NamedCommandsSync)
            {
                ICommand command;
                if (NamedCommands.TryGetValue(name, out command)) return command;

                command = new RoutedCommand(name, typeof(ICommandProvider));

                return (NamedCommands[name] = command);
            }
        }

        public ICommand GetCommand(string name, Type type)
        {
            if (type == null) return GetCommand(name);

            lock (TypedCommandsSync)
            {
                var commandsForType = TypedCommands.ContainsKey(type)
                    ? (TypedCommands[type])
                    : (TypedCommands[type] = new Dictionary<string, ICommand>());

                ICommand command;
                if (commandsForType.TryGetValue(name, out command)) return command;

                command = new RoutedCommand(name, type);

                return (commandsForType[name] = command);
            }
        }

        #endregion
    }
}
