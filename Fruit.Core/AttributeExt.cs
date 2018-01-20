using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit
{
    public static class AttributeExt
    {
        public static TAttribute GetCustomAttribute<TAttribute>(this Type type, bool inherit = false)
            where TAttribute: Attribute
        {
            return (TAttribute)type.GetCustomAttributes(typeof(TAttribute), inherit).FirstOrDefault();
        }
    }
}
