﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace WickedNite.Flux
{
    public abstract class AbstractAutoRegistration : IAutoRegistration
    {
        #region IAutoRegistration Members

        public void From(params Assembly[] assemblies)
        {
            var discovery = DoDiscovery(assemblies);

            discovery.Controllers.ForEach(p => RegisterController("controller-" + ToComponentName(p.Second), p.First, p.Second));
            discovery.Views.ForEach(p => RegisterView("view-" + ToComponentName(p.Second), p.First, p.Second));
            discovery.ViewModels.ForEach(p => RegisterViewModel("bag-" + ToComponentName(p.Second), p.First, p.Second));
            discovery.ViewModelInterfaces.ForEach(t => RegisterViewModel("bag-for-" + ToComponentName(t), t, null));
        }
        
        #endregion

        protected virtual AutoRegistrationDiscoveryInfo DoDiscovery(params Assembly[] assemblies)
        {
            var result = new AutoRegistrationDiscoveryInfo();
            var viewModelMap = new Dictionary<Type, Type>();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsInterface)
                    {
                        if (type.GetInterfaces().Any(o => typeof(IViewModel).Equals(o)))
                            viewModelMap[type] = null;

                        /* no more special interface only type handling */
                        continue;
                    }

                    if (type.IsAbstract)
                        continue;

                    if (typeof(IController).IsAssignableFrom(type))
                    {
                        var service = type.GetInterfaces()
                            .Where(
                                direct => direct.GetInterfaces()
                                    .Any(ancestor => ancestor.IsGenericType && typeof(IController<,>).Equals(ancestor.GetGenericTypeDefinition()))
                            )
                            .FirstOrDefault();

                        result.Controllers.Add(new Pair<Type, Type>(service ?? type, type));
                    }

                    if (typeof(IView).IsAssignableFrom(type))
                    {
                        var service = type.GetInterfaces()
                            .Where(
                                direct => direct.GetInterfaces()
                                    .Any(ancestor => ancestor.IsGenericType && typeof(IView<>).Equals(ancestor.GetGenericTypeDefinition()))
                            )
                            .FirstOrDefault();

                        result.Views.Add(new Pair<Type, Type>(service ?? type, type));
                    }

                    if (typeof(IViewModel).IsAssignableFrom(type))
                    {
                        var service = (
                            from o in type.GetInterfaces()
                            where o.GetInterfaces().Any(c => typeof(IViewModel).Equals(c))
                            select o
                        ).FirstOrDefault();

                        viewModelMap[service ?? type] = type;
                    }
                }
            }

            foreach (var item in viewModelMap)
                if (item.Value != null)
                    result.ViewModels.Add(new Pair<Type, Type>(item.Key, item.Value));
                else
                    result.ViewModelInterfaces.Add(item.Key);

            return result;
        }

        protected virtual string ToComponentName(Type type)
        {
            return type.FullName.Replace(".", "-").ToLower();
        } 

        /// <summary>
        /// Register a controller.  Controllers must be registered as a transient type (i.e. each request returns a new instance).
        /// </summary>
        /// <param name="name"></param>
        /// <param name="implementation"></param>
        protected abstract void RegisterController(string name, Type contract, Type implementation);
        /// <summary>
        /// Register a view for an interface.  Views must be registered as a transient type (i.e. each request returns a new instance).
        /// </summary>
        /// <param name="name"></param>
        /// <param name="contract"></param>
        /// <param name="implementation"></param>
        protected abstract void RegisterView(string name, Type contract, Type implementation);
        /// <summary>
        /// Register a property bag for interface.  Property bags must be registered as a transient type (i.e. each request returns a new instance).
        /// </summary>
        /// <param name="name"></param>
        /// <param name="contract"></param>
        /// <param name="implementation">
        /// If the implementation is null it is up to the callee to either throw an error or generate an implementation.        
        /// </param>
        protected abstract void RegisterViewModel(string name, Type contract, Type implementation);
    }
}
