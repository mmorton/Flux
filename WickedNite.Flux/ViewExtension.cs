using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows;
using Castle.Windsor;

namespace WickedNite.Commons.MuRail
{
    public class ViewExtension : MarkupExtension
    {
        public Type Type { get; set; }

        public ViewExtension(Type type)
        {
            Type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var accessor = Application.Current as IContainerAccessor;
            if (accessor == null) return null;

            return accessor.Container.Resolve(Type);
        }
    }
}
