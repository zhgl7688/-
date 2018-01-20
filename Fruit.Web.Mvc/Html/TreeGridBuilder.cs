using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fruit.Web
{
    public class TreeGridBuilder : DataGridBuilder
    {
        private HtmlHelper htmlHelper;

        public TreeGridBuilder()
            : this(null) 
        { }

        public TreeGridBuilder(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
            Tag.Attributes["class"] = "easyui-treegrid";
        }

        public TreeGridBuilder DataBind()
        {
            Data("bind", "treegrid");
            Tag.Attributes.Remove("class");
            return this;
        }
    }
}
