using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WickedNite.Flux
{
    public class ActionAttribute : Attribute
    {
        public string Name { get; set; }

        public ActionAttribute() { }
        public ActionAttribute(string name)
        {
            Name = name;
        }
    }
}
