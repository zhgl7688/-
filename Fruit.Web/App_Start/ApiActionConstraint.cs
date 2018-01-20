using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Routing;

namespace Fruit.Web
{
    /// <summary>
    /// 对 api 访问提供方法定位
    /// </summary>
    public class ApiActionConstraint : IHttpRouteConstraint
    {
        private const string _id = "id";
        private static readonly string[] array = new string[] { "GET", "PUT", "DELETE", "POST", "EDIT", "UPDATE", "AUDIT", "DOWNLOAD" };

        public bool Match(System.Net.Http.HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            if (values == null)
                return true;

            if (!values.ContainsKey(parameterName) || !values.ContainsKey(_id))
                return true;

            var action = values[parameterName].ToString().ToLower();

            if (System.Text.RegularExpressions.Regex.IsMatch(action, @"\d|\-", System.Text.RegularExpressions.RegexOptions.Compiled) && !action.StartsWith("LOAD", StringComparison.OrdinalIgnoreCase))
            {
                values[parameterName] = request.Method.ToString();
                values["id"] = action;
            }
            else if (string.IsNullOrEmpty(action))
            {
                values[parameterName] = request.Method.ToString();
            }
            //else if (string.IsNullOrEmpty(values[_id].ToString()))
            //{
            //    var isidstr = true;
            //    array.ToList().ForEach(x =>
            //    {
            //        if (action.StartsWith(x.ToLower()))
            //            isidstr = false;
            //    });

            //    if (isidstr)
            //    {
            //        values[_id] = values[parameterName];
            //        values[parameterName] = request.Method.ToString();
            //    }
            //}
            return true;
        }
    }
}