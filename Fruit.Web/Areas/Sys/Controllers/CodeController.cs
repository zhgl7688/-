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
    public class CodeController : Controller
    {
        // GET: Sys/Code
        public ActionResult Index()
        {
            return View();
        }
    }

    public class CodeApiController : ApiController
    {
        [System.Web.Http.HttpGet]
        public object Types()
        {
            using (var db = new SysContext())
            {
                return db.sys_codeType.AsNoTracking().OrderBy(c => c.Seq).ToList();
            }
        }

        [System.Web.Http.HttpPost]
        public object Types(JArray array)
        {
            using (var db = new SysContext())
            {
                for (int i = 0; i < array.Count; i++)
                {
                    var row_state = (RowState)(int)array[i]["_row_state"];
                    sys_codeType codeType = array[i].ToObject<sys_codeType>();
                    switch (row_state)
                    {
                        case RowState.Changed:
                            var oldCodeType = db.sys_codeType.Find(codeType.CodeType);
                            oldCodeType.CodeTypeName = codeType.CodeTypeName;
                            oldCodeType.Seq = codeType.Seq;
                            oldCodeType.Description = codeType.Description;
                            oldCodeType.UpdateDate = DateTime.Now;
                            oldCodeType.UpdatePerson = User.Identity.GetUserInfo().UserName;
                            break;
                        case RowState.New:
                            codeType.CreateDate = DateTime.Now;
                            codeType.CreatePerson = User.Identity.GetUserInfo().UserName;
                            db.sys_codeType.Add(codeType);
                            break;
                        case RowState.Deleted:
                            db.sys_code.Remove(r => r.CodeType == codeType.CodeType);
                            db.sys_codeType.Remove(r => r.CodeType == codeType.CodeType);
                            break;
                    }
                }

                // 记录操作日志
                db.sys_log.Add(new sys_log
                {
                    UserCode = User.Identity.Name,
                    UserName = User.Identity.GetUserInfo().UserName,
                    Position = Request.RequestUri.PathAndQuery,
                    Target = "字典数据",
                    Type = "修改",
                    Message = array.ToString(),
                    Date = DateTime.Now,
                });

                db.SaveChanges();
            }
            return Types();
        }

        public object Get(string type = null)
        {
            var pageReq = new PageRequest();
            using (var db = new SysContext())
            {
                IQueryable<sys_code> query = db.sys_code.AsNoTracking();
                if (!string.IsNullOrEmpty(type))
                {
                    query = query.Where(c => c.CodeType == type);
                }
                //query = pageReq.ApplyQuery<sys_code>(query, "Seq");
                //return query.ToList();

                int total;
                query = pageReq.ApplyQuery<sys_code>(query, "Seq", null, out total);
                dynamic result = new System.Dynamic.ExpandoObject();
                result.rows = query.ToList();
                result.total = total;
                return result;
            }
        }

        public object Post(JArray data)
        {
            using (var db = new SysContext())
            {
                for(int i=0;i<data.Count;i++){
                    JObject obj = data[i] as JObject;
                    RowState rowState = (RowState)(int)obj["_row_state"];
                    sys_code code = obj.ToObject<sys_code>();
                    switch (rowState)
                    {
                        case RowState.Changed:
                            db.Entry(code).State = System.Data.Entity.EntityState.Modified;
                            break;
                        case RowState.New:
                            db.sys_code.Add(code);
                            break;
                        case RowState.Deleted:
                            db.Entry(code).State = System.Data.Entity.EntityState.Deleted;
                            break;
                    }
                }

                // 记录操作日志
                db.sys_log.Add(new sys_log
                {
                    UserCode = User.Identity.Name,
                    UserName = User.Identity.GetUserInfo().UserName,
                    Position = Request.RequestUri.PathAndQuery,
                    Target = "字典分类数据",
                    Type = "修改",
                    Message = data.ToString(),
                    Date = DateTime.Now,
                });
                db.SaveChanges();
            }
            return true;
        }
    }
}