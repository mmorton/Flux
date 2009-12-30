using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Media;

namespace WickedNite.Flux
{
    public class DefaultControllerAdapter : IControllerAdapter
    {
        public IControllerLifecycleManager LifecycleManager { get; set; }
        public ICommandProvider CommandProvider { get; set; }
        public IActionInvoker ActionInvoker { get; set; }

        public DefaultControllerAdapter(IControllerLifecycleManager lifecycleManager, ICommandProvider commandProvider, IActionInvoker actionInvoker)
        {
            LifecycleManager = lifecycleManager;
            CommandProvider = commandProvider;
            ActionInvoker = actionInvoker;
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
                                method
                            )
                        ));

                        el.CommandBindings.Add(new CommandBinding(
                            commandByType,
                            CreateActionDelegate(
                                controller,
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
                            method
                        )
                    ));

                    el.CommandBindings.Add(new CommandBinding(
                        commandByType,
                        CreateActionDelegate(
                            controller,
                            method
                        )
                    ));
                }
            }            
            
            var frameworkEl = view as FrameworkElement;
            if (frameworkEl != null)            
                frameworkEl.Loaded += CreateLoadedDelegate(controller);                      
        }

        private static T AncestorOfType<T>(DependencyObject element) where T : class
        {
            while (element != null && (element is T) == false) element = VisualTreeHelper.GetParent(element);

            return element as T;
        }

        public RoutedEventHandler CreateLoadedDelegate(IController controller)
        {
            var manager = LifecycleManager;
            var ready = false;            

            return delegate(object sender, RoutedEventArgs args)
            {
                if (ready == false)
                {
                    var notify = controller as IViewAware;

                    var frameworkEl = sender as FrameworkElement;

                    var container = AncestorOfType<IViewContainer>(frameworkEl);
                    if (container != null)
                    {
                        container.ViewClosed += (s, e) =>
                        {
                            // if the closed event is firing for this view
                            if (e.View == sender)
                            {
                                if (notify != null)
                                    notify.Closed();

                                if (manager != null)
                                    manager.Release(controller);
                            }
                        };
                    }
                    else
                    {                        
                        var window = AncestorOfType<Window>(frameworkEl);
                        if (window != null)
                            window.Closed += (s, e) =>
                            {
                                if (notify != null)
                                    notify.Closed();

                                if (manager != null)
                                    manager.Release(controller);
                            };                        
                    }
                    
                    if (notify != null)
                        notify.Ready();

                    ready = true;
                }
            };
        }     

        public ExecutedRoutedEventHandler CreateActionDelegate(IController controller, MethodInfo method)
        {
            var invoker = ActionInvoker;

            return delegate(object sender, ExecutedRoutedEventArgs e)
            {
                var parameters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                parameters["sender"] = sender;
                parameters["e"] = e;
                parameters["parameter"] = e.Parameter;

                invoker.Invoke(controller, method, parameters);
            };
        }       

        #endregion
    }
}
