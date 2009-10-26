﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedNite.Commons.MuRail
{
    public interface IViewAdapter
    {
        object GetPropertyBag(object instance);
        void SetPropertyBag(object instance, object value);
    }
}
