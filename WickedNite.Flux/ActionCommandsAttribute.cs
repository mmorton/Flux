using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Reflection;

namespace WickedNite.Commons.MuRail
{
    public class ActionCommandsAttribute : Attribute, IActionCommandsProvider
    {
        public Type Type { get; set; }

        public ActionCommandsAttribute() { }
        public ActionCommandsAttribute(Type t)
        {
            Type = t;
        }
    
        #region IActionCommandsProvider Members

        public IDictionary<string, ICommand>  GetCommands(Type controllerType)
        {
 	        var fields = Type.GetFields(BindingFlags.Public | BindingFlags.Static);
            var commands = new Dictionary<string, ICommand>();
            foreach (var field in fields)
                if (typeof(ICommand).IsAssignableFrom(field.FieldType))
                    commands[field.Name] = (ICommand)field.GetValue(null);

            return commands;
        }

        #endregion
    }  
}
