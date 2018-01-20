using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fruit
{
    /// <summary>
    /// 提供类相关辅助功能
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// 在当前域所有已加载的程序集中查找类的实现
        /// </summary>
        /// <param name="baseType">基类</param>
        /// <returns></returns>
        public static Type[] FindAllImplements(this Type baseType)
        {
            return FindAll(t => baseType.IsAssignableFrom(t) && !t.IsAbstract);
        }

        /// <summary>
        /// 在当前域所有已加载的程序集中查找类
        /// </summary>
        /// <param name="finder">测试是否返回类的回调函数</param>
        /// <returns>所有满足 finder 的类的数组</returns>
        public static Type[] FindAll(Func<Type, bool> finder)
        {
            List<Type> types = new List<Type>();
            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                types.AddRange(FindAll(assembly, finder));
            }
            return types.ToArray();
        }


        /// <summary>
        /// 在指定的程序集中查找类
        /// </summary>
        /// <param name="assembly">查找的程序集</param>
        /// <param name="finder">测试是否返回类的回调函数</param>
        /// <returns>所有满足 finder 的类的数组</returns>
        public static Type[] FindAll(this Assembly assembly, Func<Type, bool> finder)
        {
            List<Type> types = new List<Type>();
            try
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (finder(type))
                    {
                        types.Add(type);
                    }
                }
            }
            catch (NotSupportedException e) {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return types.ToArray();
        }
    }
}
