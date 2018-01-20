using Fruit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Fruit.Web
{
    public static class FruitHtmlHelper
    {
        /// <summary>
        /// 输出一个树型网格
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="builder">树型网格构建方法</param>
        /// <returns></returns>
        public static IHtmlString TreeGrid(this HtmlHelper htmlHelper, Action<TreeGridBuilder> builder)
        {
            TreeGridBuilder tb = new TreeGridBuilder(htmlHelper);
            builder(tb);
            return tb;
        }

        /// <summary>
        /// 输出一个树型网格
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="builder">树型网格构建方法</param>
        /// <returns></returns>
        public static IHtmlString TreeGrid<TModel>(this HtmlHelper<TModel> htmlHelper, Action<TreeGridBuilder> builder)
        {
            TreeGridBuilder tb = new TreeGridBuilder(htmlHelper);
            tb.ModelType = typeof(TModel);
            builder(tb);
            return tb;
        }

        /// <summary>
        /// 输出一个数据网格
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="builder">数据网格构建方法</param>
        /// <returns></returns>
        public static IHtmlString DataGrid(this HtmlHelper htmlHelper, Action<DataGridBuilder> builder)
        {
            DataGridBuilder tb = new DataGridBuilder(htmlHelper);
            builder(tb);
            return tb;
        }


        /// <summary>
        /// 输出一个工具栏
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="builder">工具栏构建方法</param>
        /// <returns></returns>
        public static IHtmlString Toolbar(this HtmlHelper htmlHelper, Action<ToolbarBuilder> builder)
        {
            ToolbarBuilder tb = new ToolbarBuilder(htmlHelper);
            builder(tb);
            return tb;
        }

        /// <summary>
        /// 输出一个链接按钮
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="builder">链接按钮构建方法</param>
        /// <returns></returns>
        public static IHtmlString LinkButton(this HtmlHelper htmlHelper, Action<LinkButtonBuilder> builder)
        {
            LinkButtonBuilder tb = new LinkButtonBuilder(htmlHelper);
            builder(tb);
            return tb;
        }
    }
}
