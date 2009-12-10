using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Serialization;
using System.IO;

namespace WickedNite.Flux
{
    public class FluxSectionHandler : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler Members

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            // cheat
            using (var reader = new StringReader(section.OuterXml))            
            {
                var serializer = new XmlSerializer(typeof(FluxConfiguration));

                return serializer.Deserialize(reader);
            }           
        }

        #endregion
    }
}
