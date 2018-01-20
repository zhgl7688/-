using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fruit.Web
{

    public class DataGridBuilder : BaseBuilder<DataGridBuilder>
    {
        private HtmlHelper htmlHelper;

        public DataGridBuilder()
            : this(null)
        { }

        public DataGridBuilder(HtmlHelper htmlHelper)
            : base("table")
        {
            this.htmlHelper = htmlHelper;
            Tag.Attributes.Add("class", "easyui-datagrid");
        }

        public DataGridBuilder DataBind()
        {
            Data("bind", "datagrid");
            Tag.Attributes.Remove("class");
            return this;
        }

        public Type ModelType { get; internal set; }

        /// <summary>
        /// 设置是否显示一个自动行号
        /// </summary>
        /// <param name="rownumbers">是否显示一个自动行号</param>
        /// <returns></returns>
        public DataGridBuilder RowNumbers(bool rownumbers = true)
        {
            Tag.Attributes.Add("rownumbers", rownumbers ? "true" : "false");
            return this;
        }

        private List<GridColumnBuilder> columns = new List<GridColumnBuilder>();

        /// <summary>
        /// 向表格加入一个列
        /// </summary>
        /// <param name="columnBuilder"></param>
        /// <returns></returns>
        public DataGridBuilder AddColumn(GridColumnBuilder columnBuilder)
        {
            columns.Add(columnBuilder);
            return this;
        }

        /// <summary>
        /// 向表格加入一个隐藏列
        /// </summary>
        /// <param name="field">列名</param>
        /// <returns></returns>
        public DataGridBuilder AddHiddenColumn(string field)
        {
            return AddColumn(new GridColumnBuilder().Hidden().Field(field));
        }

        /// <summary>
        /// 向表格加入一个列
        /// </summary>
        /// <param name="field">列名称</param>
        /// <param name="title">标题</param>
        /// <param name="align">对齐</param>
        /// <param name="width">宽</param>
        /// <param name="editor">编辑器表达式</param>
        /// <param name="formatter">自定义显示格式化函数</param>
        /// <returns></returns>
        public DataGridBuilder AddColumn(string field, string title = null, string align = null, int? width = null, string editor = null, string formatter = null)
        {
            GridColumnBuilder columnBuilder = new GridColumnBuilder();
            columnBuilder.Field(field);
            columnBuilder.Title(title ?? field);
            columnBuilder.Align(align);
            if (width != null)
            {
                columnBuilder.Width(width.Value);
            }
            if (editor != null)
            {
                columnBuilder.Editor(editor);
            }
            if (formatter != null)
            {
                columnBuilder.Formatter(formatter);
            }
            return AddColumn(columnBuilder);
        }

        public override string ToHtmlString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<thead><tr>");
            foreach (var cb in columns)
            {
                sb.Append(cb.ToHtmlString());
            }
            sb.Append("</tr></thead>");
            Tag.InnerHtml = sb.ToString();
            return base.ToHtmlString();
        }
    }
}
