using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using WickedNite.Flux;
using System.Reflection;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;
using ImageView.Views;
using ImageView.Controllers;

namespace ImageView
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IContainerAccessor
    {
        private readonly WindsorContainer _container;

        public IWindsorContainer Container
        {
            get { return _container; }
        }

        public App()
        {
            _container = new WindsorContainer();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(_container));

            FluxStarter.Initialize();
            FluxAutoRegistration.From(Assembly.GetExecutingAssembly());
        }        
    }
}
