using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Web
{
    public class GridColumnBuilder : BaseBuilder<GridColumnBuilder>
    {
        public GridColumnBuilder()
            : base("th")
        { }

        /// <summary>
        /// 设置列关联的字段
        /// </summary>
        /// <param name="field">字符名</param>
        /// <returns></returns>
        public GridColumnBuilder Field(string field)
        {
            Tag.Attributes["field"] = field;
            return this;
        }

        /// <summary>
        /// 设置列的显示标题
        /// </summary>
        /// <param name="title">标题</param>
        /// <returns></returns>
        public override GridColumnBuilder Title(string title)
        {
            Tag.SetInnerText(title);
            return base.Title(title);
        }

        /// <summary>
        /// 设置列的对应方式
        /// </summary>
        /// <param name="align">对应方式</param>
        /// <returns></returns>
        public GridColumnBuilder Align(string align)
        {
            Tag.Attributes["align"] = align;
            return this;
        }

        /// <summary>
        /// 设置列宽
        /// </summary>
        /// <param name="width">宽度</param>
        /// <returns></returns>
        public GridColumnBuilder Width(int width)
        {
            Tag.Attributes["width"] = width.ToString();
            return this;
        }

        /// <summary>
        /// 设置编辑器信息
        /// </summary>
        /// <param name="editor">编辑器信息</param>
        /// <returns></returns>
        public GridColumnBuilder Editor(string editor)
        {
            Tag.Attributes["editor"] = editor;
            return this;
        }

        /// <summary>
        /// 设置格式化函数
        /// </summary>
        /// <param name="formatter">格式化函数</param>
        /// <returns></returns>
        public GridColumnBuilder Formatter(string formatter)
        {
            Tag.Attributes["formatter"] = formatter;
            return this;
        }

        /// <summary>
        /// 设置是否隐藏列
        /// </summary>
        /// <param name="hidden">是否隐藏列</param>
        /// <returns></returns>
        public GridColumnBuilder Hidden(bool hidden = true)
        {
            Tag.Attributes["hidden"] = hidden ? "true" : "false";
            return this;
        }

    }
}
