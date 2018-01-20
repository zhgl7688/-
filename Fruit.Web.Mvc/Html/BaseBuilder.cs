using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Fruit.Web
{
    public abstract class BaseBuilder<T> : IHtmlString
        where T: BaseBuilder<T>
    {
        protected BaseBuilder(string tagName)
        {
            Tag = new TagBuilder(tagName);
        }

        public T Id(string id)
        {
            Tag.Attributes["id"] = id;
            return (T)this;
        }

        public virtual T Title(string title)
        {
            Tag.Attributes["title"] = title;
            return (T)this;
        }

        public T Data(string name, string value)
        {
            Tag.Attributes["data-"+name] = value;
            return (T)this;
        }

        protected TagBuilder Tag { get; private set; }

        public virtual string ToHtmlString()
        {
            ApplyInnerHtml();
            return Tag.ToString();
        }

        private List<IHtmlString> childs = new List<IHtmlString>();

        protected void AddChild(IHtmlString child)
        {
            childs.Add(child);
        }

        protected virtual void ApplyInnerHtml()
        {
            if (childs.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var child in childs)
                {
                    sb.Append(child.ToHtmlString());
                }
                Tag.InnerHtml = sb.ToString();
            }
        }
    }
}
