using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedNite.Flux
{
    public class AutoRegistrationDiscoveryInfo
    {
        public List<Pair<Type, Type>> Controllers { get; set; }
        public List<Pair<Type, Type>> Views { get; set; } /* left = interface, right = type */
        public List<Pair<Type, Type>> ViewModels { get; set; } /* left = interface, right = type */
        public List<Type> ViewModelInterfaces { get; set; }

        public AutoRegistrationDiscoveryInfo()
        {
            Controllers = new List<Pair<Type, Type>>();
            Views = new List<Pair<Type, Type>>();
            ViewModels = new List<Pair<Type, Type>>();
            ViewModelInterfaces = new List<Type>();
        }
    }
}
