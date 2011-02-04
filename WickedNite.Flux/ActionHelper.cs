using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.ServiceLocation;
using System.Windows.Input;

namespace WickedNite.Flux
{
    public static class ActionHelper
    {
        public static void Execute(string name, Type target, object parameter, IInputElement element)
        {
            var container = ServiceLocator.Current;
            if (container == null) return;

            var provider = container.GetInstance<ICommandProvider>();
            if (provider == null) return;

            var command = target != null
                ? provider.GetCommand(name, target)
                : provider.GetCommand(name);

            if (command != null)
            {
                if (command is RoutedCommand)
                    ((RoutedCommand)command).Execute(parameter, element);
                else
                    command.Execute(parameter);
            }
        }

        public static void Execute(Type type, string name, object parameter)
        {
            Execute(name, type, parameter, null);
        }

        public static void Execute(Type type, string name, IInputElement element)
        {
            Execute(name, type, null, element);
        }

        public static void Execute(Type type, string name)
        {
            Execute(name, type);
        }

        public static void Execute(string name, object parameter, IInputElement element)
        {
            Execute(name, null, parameter, element);
        }

        public static void Execute(string name, object parameter)
        {
            Execute(name, null, parameter, null);
        }

        public static void Execute(string name, IInputElement element)
        {
            Execute(name, null, null, element);
        }

        public static void Execute(string name)
        {
            Execute(name, null, null, null);
        }

    }
}
