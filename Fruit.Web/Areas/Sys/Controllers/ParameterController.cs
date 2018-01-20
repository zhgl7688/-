using Fruit.Data;
using Fruit.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Fruit.Web.Areas.Sys.Controllers
{
    public class ParameterController : Controller
    {
        // GET: Sys/Parameter
        public ActionResult Index()
        {
            return View();
        }
    }

    public class ParameterApiController : ApiController
    {
        public object Get()
        {
            using (var db = new SysContext())
            {
                return db.sys_parameter.AsNoTracking().OrderBy(p => p.ParamCode).ToList();
            }
        }

        public object Post(JArray data)
        {

            using (var db = new SysContext())
            {
                for (int i = 0; i < data.Count; i++)
                {
                    JObject obj = data[i] as JObject;
                    RowState rowState = (RowState)(int)obj["_row_state"];
                    sys_parameter parameter = obj.ToObject<sys_parameter>();
                    switch (rowState)
                    {
                        case RowState.Changed:
                            db.Entry(parameter).State = System.Data.Entity.EntityState.Modified;
                            break;
                        case RowState.New:
                            db.sys_parameter.Add(parameter);
                            break;
                        case RowState.Deleted:
                            db.Entry(parameter).State = System.Data.Entity.EntityState.Deleted;
                            break;
                    }
                }
                db.SaveChanges();
            }
            return true;
        }
    }
}