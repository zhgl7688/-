using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Web.Models
{
    /// <summary>
    /// 单个树节点信息
    /// </summary>
    public class TreeItem<TKey>
    {

        [Newtonsoft.Json.JsonProperty("parent_id")]
        public TKey ParentId { get; set; }

        [Newtonsoft.Json.JsonProperty("id")]
        public TKey Id { get; set; }

        [Newtonsoft.Json.JsonProperty("text")]
        public string Text { get; set; }
    }

    public class TreeItem : TreeItem<string>
    { }
}
