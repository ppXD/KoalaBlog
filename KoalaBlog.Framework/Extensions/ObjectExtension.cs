using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.Framework.Extensions
{
    public static class ObjectExtension
    {
        public static T ConvertTo<T>(this object from) where T : new()
        {
            if (from == null)
            {
                return default(T);
            }
            var to = Activator.CreateInstance<T>();
            return from.ConvertTo<T>(to);
        }

        public static T ConvertTo<T>(this object from, T to) where T : new()
        {
            if (from == null)
            {
                return to;
            }
            if (to == null)
            {
                to = Activator.CreateInstance<T>();
            }
            PropertyDescriptorCollection fromProperties = TypeDescriptor.GetProperties(from);
            PropertyDescriptorCollection toProperties = TypeDescriptor.GetProperties(to);
            foreach (PropertyDescriptor fromProperty in fromProperties)
            {
                PropertyDescriptor toProperty = toProperties.Find(fromProperty.Name, true);
                if (toProperty != null && !toProperty.IsReadOnly)
                {
                    bool isDirectlyAssignable = toProperty.PropertyType.IsAssignableFrom(fromProperty.PropertyType);
                    bool liftedValueType = !isDirectlyAssignable && Nullable.GetUnderlyingType(fromProperty.PropertyType) == toProperty.PropertyType;
                    if (isDirectlyAssignable || liftedValueType)
                    {
                        object fromValue = fromProperty.GetValue(from);
                        if (isDirectlyAssignable || (fromValue != null && liftedValueType))
                        {
                            toProperty.SetValue(to, fromValue);
                        }
                    }
                }
            }
            return to;
        }

    }
}
