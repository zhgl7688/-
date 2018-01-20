using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Fruit;

namespace Fruit.Web
{
    /// <summary>
    /// HttpRequest 扩展方法
    /// </summary>
    public static class HttpRequestExtension
    {
        public static TModel Get<TModel>(this HttpRequest request, HttpRequestGetMode getMode = HttpRequestGetMode.Default)
        {
            TModel inst = Activator.CreateInstance<TModel>();
            foreach(var prop in typeof(TModel).GetProperties())
            {
                if (prop.SetMethod == null)
                    continue;
                var value = Get(request, prop.Name, getMode);
                if (value == null)
                    continue;
                prop.SetValue(inst, value.To(prop.PropertyType));
            }
            return inst;
        }

        public static string Get(this HttpRequest request, string name, HttpRequestGetMode getMode = HttpRequestGetMode.Default)
        {
            switch(getMode)
            {
                case HttpRequestGetMode.Default:
                    return request[name] ?? (string)HttpContext.Current.Items["QueryParams_" + name];
                case HttpRequestGetMode.GetOnly:
                    return request.QueryString[name];
                case HttpRequestGetMode.PostOnly:
                    return request.Form[name];
                default:
                    {
                        var val = request.QueryString[name];
                        if (val != null)
                            return val;
                        return request.Form[name];
                    }
            }
        }

        public static string GetReferrer(this HttpRequest request, string name)
        {
            if (request.UrlReferrer != null && !string.IsNullOrEmpty(request.UrlReferrer.Query))
            {
                string[] splits = request.UrlReferrer.Query.Split('?', '=', '&');
                // [0] - empty string before ?
                // [2n+1] - field name
                // [2n+2] - field value
                for (int i = 1; i < splits.Length; i += 2)
                {
                    if (string.Equals(splits[i], name, StringComparison.OrdinalIgnoreCase))
                    {
                        return HttpUtility.UrlDecode(splits[i + 1]);
                    }
                }
            }
            return string.Empty;
        }
    }

    public enum HttpRequestGetMode
    {
        /// <summary>
        /// 默认模式，以表单值优先,查询参数其次
        /// </summary>
        Default,
        /// <summary>
        /// 以查询参数优先，表单值其次
        /// </summary>
        GetFrist,
        /// <summary>
        /// 只使用查询参数值
        /// </summary>
        GetOnly,
        /// <summary>
        /// 只使用表单值
        /// </summary>
        PostOnly,
    }
}
