using Fruit.Web.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Web
{

    public class ExporterSetting
    {
        public string title;
        public string url;
        public List<List<Column>> columns;

        [Newtonsoft.Json.JsonConverter(typeof(JDictionaryConverter))]
        public Dictionary<string, object> queryParams;
    }
}
