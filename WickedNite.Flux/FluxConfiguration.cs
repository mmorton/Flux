using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WickedNite.Flux
{
    [XmlRoot("flux")]
    public class FluxConfiguration
    {
        public class Module
        {
            [XmlIgnore]
            public Type Type { get; set; }

            [XmlAttribute("type")]
            public string TypeName
            {
                get { return Type.AssemblyQualifiedName; }
                set { Type = value != null ? Type.GetType(value, false) : null; }
            }
        }

        [XmlArray("modules")]
        [XmlArrayItem("module")]
        public List<Module> Modules { get; set; }

        public FluxConfiguration()
        {
            Modules = new List<Module>();
        }
    }
}
