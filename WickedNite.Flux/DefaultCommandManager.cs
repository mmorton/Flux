using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Reflection;
using System.Windows.Input;

namespace WickedNite.Commons.MuRail
{
    public class DefaultCommandManager : ICommandManager
    {
        public ICommandProvider CommandProvider { get; set; }

        public DefaultCommandManager(ICommandProvider provider)
        {
            CommandProvider = provider;        
        }

        #region ICommandManager Members

        public void Bind(object view, object controller)
        {
            var el = view as UIElement;
            if (el == null) return;

            var type = controller.GetType();

            foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                var attributes = type.GetCustomAttributes(typeof(ActionAttribute), false)
                    .OfType<ActionAttribute>()
                    .ToList();             

                if (attributes.Count > 0)
                {
                    foreach (var attribute in attributes)
                    {
                        var commandByName = CommandProvider.GetCommand(attribute.Name ?? method.Name);
                        var commandByType = CommandProvider.GetCommand(attribute.Name ?? method.Name, type);

                        el.CommandBindings.Add(new CommandBinding(
                            commandByName,
                            CreateActionDelegate(
                                controller,
                                commandByName,
                                method
                            )
                        ));

                        el.CommandBindings.Add(new CommandBinding(
                            commandByType,
                            CreateActionDelegate(
                                controller,
                                commandByType,
                                method
                            )
                        ));
                    }
                }
                else
                {
                    var commandByName = CommandProvider.GetCommand(method.Name);
                    var commandByType = CommandProvider.GetCommand(method.Name, type);

                    el.CommandBindings.Add(new CommandBinding(
                        commandByName,
                        CreateActionDelegate(
                            controller,
                            commandByName,
                            method
                        )
                    ));

                    el.CommandBindings.Add(new CommandBinding(
                        commandByType,
                        CreateActionDelegate(
                            controller,
                            commandByType,
                            method
                        )
                    ));
                }
            }            
            
            var frameworkEl = view as FrameworkElement;
            if (frameworkEl != null)
                frameworkEl.Loaded += CreateLoadedDelegate(controller);
        }

        public RoutedEventHandler CreateLoadedDelegate(object controller)
        {
            return delegate(object sender, RoutedEventArgs e)
            {
                var notify = controller as INotifyViewReady;
                if (notify != null)
                    notify.Ready();
            };
        }

        public ExecutedRoutedEventHandler CreateActionDelegate(object controller, ICommand command, MethodInfo method)
        {
            return delegate(object sender, ExecutedRoutedEventArgs e)
            {
                method.Invoke(controller, new object[] { });
            };
        }

        #endregion
    }
}
