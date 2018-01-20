using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Web
{
    /// <summary>
    /// 用于 Fruit Web Api 的消息处理扩展
    /// </summary>
    public static class HttpMessageExtensions
    {
        public static IRequestWrapper GetRequestWrapper(this HttpRequestMessage reqMsg, HttpMessageContentFormat format)
        {
            return new BaseRequestWrapper();
        }

        public static IResponseWrapper GetResponseWrapper(this HttpResponseMessage repMsg, HttpMessageContentFormat format)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// 表示消息内容的格式，用于 Fruit Web Api
    /// </summary>
    [Flags]
    public enum HttpMessageContentFormat
    {
        Json = 1,
        Xml = 2
    }

    public interface IRequestWrapper
    {
        bool TryGetValue<TModel>(string path, out TModel outValue);

        TModel GetValue<TModel>(string path);
    }

    public interface IResponseWrapper
    {
        void BeginObject(string name);
        void EndObject();
        void BeginArray(string name);
        void EndArray();
        void Value(string name, object value);
    }

    class BaseRequestWrapper : IRequestWrapper
    {

        public TModel GetValue<TModel>(string path)
        {
            return default(TModel);
        }

        public bool TryGetValue<TModel>(string path, out TModel outValue)
        {
            outValue = default(TModel);
            return false;
        }
    }
}
