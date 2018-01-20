using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Fruit.Web
{

    /// <summary>
    /// 辅助导出数据处理
    /// </summary>
    public class Exporter : ActionResult
    {
        public const string DEFAULT_EXPORTER = "api";


        public override void ExecuteResult(ControllerContext context)
        {
            string data = HttpContext.Current.Request["data"];
            var setting = Newtonsoft.Json.JsonConvert.DeserializeObject<ExporterSetting>(data);

            var urls = setting.url.Split('/');
            var area = FormatName(urls[2]);
            var controller = FormatName(urls[3]);
            var action = urls.Length > 4 ? urls[4] : "Get";

            var type = Type.GetType(string.Format("Fruit.Web.Areas.{0}.Controllers.{1}ApiController, Fruit.Web", area, controller));
            if (type == null)
            {
                area = urls[2];
                controller = urls[3];
                type = Type.GetType(string.Format("Fruit.Web.Areas.{0}.Controllers.{1}ApiController, Fruit.Web", area, controller));
            }
            if (type == null)
            {
                var webAss = Type.GetType("Fruit.Web.Controllers.HomeController, Fruit.Web").Assembly;
                type = webAss.GetTypes().First(t => t.FullName.Equals(string.Format("Fruit.Web.Areas.{0}.Controllers.{1}ApiController", area, controller), StringComparison.OrdinalIgnoreCase));
            }
            var instaince = Activator.CreateInstance(type);
            var actionMethod = type.GetMethod(action, Type.EmptyTypes);

            // 处理 queryParams 重写
            if (setting != null && setting.queryParams.Count > 0)
            {
                foreach (var queryParam in setting.queryParams)
                {
                    HttpContext.Current.Items["QueryParams_" + queryParam.Key] = queryParam.Value.ToString();
                }
            }

            object _data = null;
            if (actionMethod == null)
            {
                actionMethod = type.GetMethod(action, new Type[] { typeof(JObject) });
                _data = actionMethod.Invoke(instaince, new object[]{new JObject()});
            }
            else
            {
                _data = actionMethod.Invoke(instaince, null);
            }

            var exporter = new ExcelExporter();
            exporter.Init(setting);

            
            context.HttpContext.Response.ContentType = "application/octet-stream";
            context.HttpContext.Response.AddHeader("Content-Disposition", "attachment;fileName=" + context.HttpContext.Server.UrlEncode(setting.title) + ".xls");
            if (_data is PageRequest.IPageList)
            {
                _data = ((PageRequest.IPageList)_data).GetEnumerable();
            }
            exporter.Export((IEnumerable)_data, context.HttpContext.Response.OutputStream);            
        }

        private string FormatName(string name)
        {
            return name.Substring(0, 1).ToUpperInvariant() + name.Substring(1).ToLowerInvariant();
        }
    }
}
