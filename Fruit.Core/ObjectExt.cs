using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fruit
{
    /// <summary>
    /// 提供基本对象扩展方法
    /// </summary>
    public static class ObjectExt
    {
        public static T GetValue<T>(this object obj, string path)
        {
            string[] pns = path.Split('.');
            string pn = pns[0];
            var members = obj.GetType().GetMember(pn, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.GetProperty);

            object result = null;
            if (members.Length > 0)
            {
                if (members[0] is FieldInfo)
                {
                    result = (members[0] as FieldInfo).GetValue(obj);
                }
                else if(members[0] is PropertyInfo)
                {
                    result = (members[0] as PropertyInfo).GetValue(obj);
                }
            }

            if (result != null)
            {
                if (pns.Length > 1)
                {
                    result = GetValue<T>(result, string.Join(".", pns.Skip(1)));
                }
                return (T)result;
            }

            return default(T);
        }
    }
}
