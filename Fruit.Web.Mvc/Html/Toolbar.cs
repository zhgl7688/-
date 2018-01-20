using Fruit.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Fruit.Web
{
    public class ToolbarBuilder : BaseBuilder<ToolbarBuilder>
    {
        public ToolbarBuilder(HtmlHelper htmlHelper)
            : base("div")
        {
            Tag.Attributes.Add("class", "z-toolbar");
        }

        /// <summary>
        /// 加入一个工具栏按钮
        /// </summary>
        /// <param name="id">按钮 id</param>
        /// <param name="icon">图标</param>
        /// <param name="title">标题</param>
        /// <param name="clickHandler">回调处理</param>
        /// <returns></returns>
        public ToolbarBuilder AddButton(string id, string icon, string title, string clickHandler)
        {
            AddChild(new LinkButtonBuilder().Id(id).Icon(icon).Plain().Title(title).Data("bind", "click:'"+clickHandler + "'"));
            return this;
        }
    }
}
