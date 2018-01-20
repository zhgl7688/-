using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fruit.Data
{
    /// <summary>
    /// 提供表达式处理器
    /// </summary>
    public static class FruitExpression
    {
        private static List<IFruitExpressionReplacer> replacers = new List<IFruitExpressionReplacer>();

        static FruitExpression()
        {
            foreach(var type in typeof(IFruitExpressionReplacer).FindAllImplements())
            {
                replacers.Add((IFruitExpressionReplacer)Activator.CreateInstance(type));
            }
        }


        /// <summary>
        /// 替换表达式中所有变量引用 {name} 元素
        /// </summary>
        public static string Replace(string expression, object context = null)
        {
            Regex r = new Regex(@"\{([^\}]*)\}", RegexOptions.Compiled);
            return r.Replace(expression, (match) =>
            {
                var name = match.Groups[1].Value;
                foreach (var replacer in replacers)
                {
                    if (replacer.CanHandle(name, context))
                    {
                        return replacer.Replace(name, context);
                    }
                }
                return match.Value;
            });
        }
    }
}
