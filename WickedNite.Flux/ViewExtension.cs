﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows;
using Microsoft.Practices.ServiceLocation;
using System.ComponentModel;

namespace WickedNite.Flux
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
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return null;

            var locator = ServiceLocator.Current;
            if (locator == null) return null;

            return locator.GetInstance(Type);
        }
    }
}
