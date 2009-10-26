using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Input;
using System.Windows;
using Microsoft.Practices.ServiceLocation;
using Castle.Windsor;

namespace WickedNite.Commons.MuRail
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

        /*
        // commented out due to bug with Visual Studio 2008 (see: http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=369812)
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
            var accessor = Application.Current as IContainerAccessor;
            if (accessor == null) return null;

            var commandProvider = accessor.Container.Resolve<ICommandProvider>();
            if (commandProvider == null) return null;

            return Target != null
                ? commandProvider.GetCommand(Name, Target)
                : commandProvider.GetCommand(Name);
        }
    }
}
