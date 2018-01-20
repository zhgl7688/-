using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    /// <summary>
    /// 向 FruitExpression 提供变量引用替换器
    /// </summary>
    public interface IFruitExpressionReplacer
    {
        /// <summary>
        /// 测试当前替换器是否可对变量名进行替换
        /// </summary>
        /// <param name="name">变量名</param>
        /// <param name="context">环境上下文</param>
        bool CanHandle(string name, object context);

        /// <summary>
        /// 替换变量名，返回当前值
        /// </summary>
        /// <param name="name">变量名</param>
        /// <param name="context">环境上下文</param>
        /// <returns>替换后的字符串值</returns>
        string Replace(string name, object context);
    }
}
