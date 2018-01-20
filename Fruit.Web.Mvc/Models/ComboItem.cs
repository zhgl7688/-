using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Web.Models
{
    /// <summary>
    /// 为 Combobox 提供项描述
    /// </summary>
    public class ComboItem<TValue>
    {
        [Newtonsoft.Json.JsonProperty("text")]
        public string Text { get; set; }

        [Newtonsoft.Json.JsonProperty("value")]
        public TValue Value { get; set; }
    }

    /// <summary>
    /// 为 Combobox 提供项描述
    /// </summary>
    public class ComboItem : ComboItem<String>
    {
    }
}
