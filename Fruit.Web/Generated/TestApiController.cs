using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Fruit.Web.Generated
{
    public class TestApiController : ApiController
    {
      //  [HttpGet, Route("/wapi/test/{guid}")]
        public HttpResponseMessage MyApi(HttpRequestMessage request, int guid)
        {
            var req = request.GetRequestWrapper(HttpMessageContentFormat.Json | HttpMessageContentFormat.Xml);
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent("我的内部api结果 " + guid) };
        }
    }
}