using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Input;
using System.Windows;
using Microsoft.Practices.ServiceLocation;

namespace WickedNite.Flux
{
    [MarkupExtensionReturnType(typeof(ICommand))]
    public class ActionExtension : MarkupExtension
    {
        public Type Target { get; set; }
        public string Name { get; set; }

        public ActionExtension() { }
        public ActionExtension(string name)
        {
            Name = name;
        }

        
        // commented out due to bug with Visual Studio 2008 (see: http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=369812)
        /*
        public ActionExtension(Type target)
        {
            Target = target;
        }

        public ActionExtension(Type target, string name)
        {
            Target = target;
            Name = name;
        }
        */

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var locator = ServiceLocator.Current;
            if (locator == null) return null;

            var provider = locator.GetInstance<ICommandProvider>();
            if (provider == null) return null;

            return Target != null
                ? provider.GetCommand(Name, Target)
                : provider.GetCommand(Name);
        }
    }
}
