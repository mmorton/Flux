using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Reflection;
using System.ComponentModel;

namespace WickedNite.Flux
{
    public class DefaultControllerAdapter : IControllerAdapter
    {
        public IControllerLifecycleManager LifecycleManager { get; set; }
        public ICommandProvider CommandProvider { get; set; }        

        public DefaultControllerAdapter(IControllerLifecycleManager lifecycleManager, ICommandProvider commandProvider)
        {
            LifecycleManager = lifecycleManager;
            CommandProvider = commandProvider;
        }

        #region IControllerAdapter Members

        public void Build(IController controller, IView view)
        {
            Bind(controller, view);
        }

        private void Bind(IController controller, IView view)
        {
            var el = view as UIElement;
            if (el == null) return;

            var type = controller.GetType();

            foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
				// filter out disposable
                if (typeof(IDisposable).IsAssignableFrom(type) && method.Name == "Dispose") continue;

                // filter out property accessors
                if (method.IsSpecialName) continue;
				
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
                                view,
                                commandByName,
                                method
                            )
                        ));

                        el.CommandBindings.Add(new CommandBinding(
                            commandByType,
                            CreateActionDelegate(
                                controller,
                                view,
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
                            view,
                            commandByName,
                            method
                        )
                    ));

                    el.CommandBindings.Add(new CommandBinding(
                        commandByType,
                        CreateActionDelegate(
                            controller,
                            view,
                            commandByType,
                            method
                        )
                    ));
                }
            }            
            
            var frameworkEl = view as FrameworkElement;
            if (frameworkEl != null)
            {
                frameworkEl.Loaded += CreateLoadedDelegate(controller);
                frameworkEl.Unloaded += CreateUnloadedDelegate(controller);
            }
        }

        public RoutedEventHandler CreateLoadedDelegate(IController controller)
        {
            return delegate(object sender, RoutedEventArgs e)
            {
                var notify = controller as INotifyViewReady;
                if (notify != null)
                    notify.Ready();
            };
        }

        public RoutedEventHandler CreateUnloadedDelegate(IController controller)
        {
            var manager = LifecycleManager;
            return delegate(object sender, RoutedEventArgs e)
            {
                if (manager != null)
                    manager.Release(controller);
            };
        }

        public ExecutedRoutedEventHandler CreateActionDelegate(IController controller, IView view, ICommand command, MethodInfo method)
        {
            return delegate(object sender, ExecutedRoutedEventArgs e)
            {
                var parameters = method.GetParameters();
                if (parameters.Length > 0)
                {
                    var values = new object[parameters.Length];
                    if (e.Parameter != null)
                    {
                        var converter = TypeDescriptor.GetConverter(parameters[0].ParameterType);
                        if (converter != null && converter.CanConvertFrom(e.Parameter.GetType()))
                            values[0] = converter.ConvertFrom(e.Parameter);
                        else
                            values[0] = parameters[0].RawDefaultValue;
                    }

                    for (var i = 1; i < parameters.Length; i++)
                        values[i] = parameters[i].RawDefaultValue;

                    var result = method.Invoke(controller, values);  
                    // todo: what to do with result?
                }
                else
                    method.Invoke(controller, new object[] { });
            };
        }

        #endregion
    }
}
