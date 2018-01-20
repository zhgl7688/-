using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit
{
    /// <summary>
    /// 提供把字符串转换为常用简单类型的功能
    /// </summary>
    public static class StringToConverterExt
    {
        public static TModel To<TModel>(this string text)
        {
            return (TModel)To(text, typeof(TModel));
        }

        public static object To(this string text, Type type)
        {
            bool nullabe = type.Name == "Nullable`1";
            type = type.GenericTypeArguments[0];

            if (type == typeof(string))
                return text;

            if (type == typeof(int))
                return int.Parse(text);

            throw new Exception("无法将字符串转换为 " + type.FullName + " 类型！");
        }
    }
}
