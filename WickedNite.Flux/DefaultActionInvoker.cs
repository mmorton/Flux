using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WickedNite.Flux
{
    public class DefaultActionInvoker : IActionInvoker
    {
        protected object ToCompatibleValue(Type targetType, object value)
        {
            var targetDefaultValue = targetType.IsValueType
                ? Activator.CreateInstance(targetType)
                : null;

            if (value == null) 
                return targetDefaultValue;                      
            
            if (targetType.IsAssignableFrom(value.GetType())) 
                return value;

            var converter = TypeDescriptor.GetConverter(targetType);
            if (converter != null && converter.CanConvertFrom(value.GetType()))
                return converter.ConvertFrom(value);

            return targetDefaultValue;
        }

        public void Invoke(object presenter, MethodInfo method, IDictionary<string, object> properties)
        { 
            var parameters = method.GetParameters();
            var args = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                if (properties.ContainsKey(parameter.Name) && properties[parameter.Name] != null)                
                    args[i] = ToCompatibleValue(parameter.ParameterType, properties[parameter.Name]);
                else
                    args[i] = parameter.ParameterType.IsValueType
                        ? Activator.CreateInstance(parameter.ParameterType)
                        : null;
            }

            var result = method.Invoke(presenter, args);
        }
    }
}
