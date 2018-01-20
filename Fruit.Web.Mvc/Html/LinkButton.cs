using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Fruit.Web
{
    public class LinkButtonBuilder : BaseBuilder<LinkButtonBuilder>
    {
        public LinkButtonBuilder() : this(null) { }

        public LinkButtonBuilder(HtmlHelper htmlHelper)
            : base("a")
        {
            Tag.Attributes.Add("class", "easyui-linkbutton");
            Tag.Attributes.Add("href", "#");
        }

        public override LinkButtonBuilder Title(string title)
        {
            Tag.SetInnerText(title);
            return base.Title(title);
        }

        public LinkButtonBuilder Icon(string icon)
        {
            Tag.Attributes["icon"] = icon;
            return this;
        }

        public LinkButtonBuilder Plain(bool plain = true)
        {
            Tag.Attributes["plain"] = plain ? "true" : "false";
            return this;
        }
    }
}
