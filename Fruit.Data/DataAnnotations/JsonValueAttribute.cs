using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data.DataAnnotations
{
    /// <summary>
    /// 标记表示字段内容为 JSON 格式
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple=false, Inherited=true)]
    public class JsonValueAttribute : Attribute
    {
    }
}
