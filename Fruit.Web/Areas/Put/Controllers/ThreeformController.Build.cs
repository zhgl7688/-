/// 注意：本文件由 FruitBuilder 生成和管理，请误手工更改
using Fruit.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Fruit.Data;
using Fruit.Models;
using Newtonsoft.Json.Linq;

namespace Fruit.Web.Areas.Put.Controllers
{
    public partial class ThreeformController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                return View();
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(int id)
        {
            three1 form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.three1.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new three1
                {
                    id = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class ThreeformApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class three1ListModel {
            public int id { get; set; }
            public string name { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<three1ListModel>(db.Database, "a.id ,a.name ", "three1 a ", sbCondition.ToString(), "a.id", "desc");
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class three2Inthree1Model {
            public int pid { get; set; }
            public int id { get; set; }
            public string name { get; set; }
        }
        public object GetDetial(string id)
        {
            using (var db = new LUOLAI1401Context())
            {
                var pageReq = new PageRequest();
                pageReq.Page = null; // 子表读取时不分页
                var three2 = pageReq.ToPageList<three2Inthree1Model>(db.Database, "a.pid ,a.id ,a.name ", "three2 a ", "a.pid = " + id, "a.id", "desc").Rows;
                return new {three2};
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class three3Inthree1Model {
            public int gid { get; set; }
            public int pid { get; set; }
            public int? id { get; set; }
            public string name { get; set; }
        }
        [System.Web.Http.HttpGet, System.Web.Http.ActionName("Loadthree3")]
        public object Loadthree3([FromUri] int gid, [FromUri] int pid)
        {
            using(var db = new LUOLAI1401Context())
            {
                var pageReq = new PageRequest();
                pageReq.Page = null; // 子表读取时不分页
                return pageReq.ToPageList<three3Inthree1Model>(db.Database, "a.gid ,a.pid ,a.id ,a.name ", "three3 a", "a.gid = '" + gid + "' AND a.pid = '" + pid + "'", "a.gid" ,"desc").Rows;
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<three1>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.three1.Find(form.id);
                if (dbForm == null)
                {
                    db.three1.Add(form);
                }
                else
                {
                    dbForm.name = form.name;
                }
                // 记录多级零时主键对应(key int 为 js 生成的页内全局唯一编号)
                var _id_maps = new Dictionary<int, object[]>();
                if( post["three2"] != null )
                {
                    var sub= post["three2"] as JArray;
                    if( sub.Count > 0)
                    {
                        for(int i = 0; i < sub.Count; i++ )
                        {
                            JObject obj = sub[i] as JObject;
                            RowState rowState = (RowState)(int)obj["_row_state"];
                            var model = obj.ToObject<three2>(JsonExtension.FixJsonSerializer);
                            switch (rowState){
                                case RowState.Changed:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                                    break;
                                case RowState.New:
                                    model.pid = form.id;
                                    model.id = new SysSerialServices().GetNewSerial("three2." + form.id, null);
                                    _id_maps.Add((int)obj["_id_"], new object[]{model.pid, model.id});
                                    db.three2.Add(model);
                                    break;
                                case RowState.Deleted:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                                    break;
                            }
                        }
                    }
                }
                if( post["three3"] != null )
                {
                    var sub= post["three3"] as JArray;
                    if( sub.Count > 0)
                    {
                        for(int i = 0; i < sub.Count; i++ )
                        {
                            JObject obj = sub[i] as JObject;
                            RowState rowState = (RowState)(int)obj["_row_state"];
                            var model = obj.ToObject<three3>(JsonExtension.FixJsonSerializer);
                            switch (rowState){
                                case RowState.Changed:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                                    break;
                                case RowState.New:
                                    if(obj["_pid_"] != null && _id_maps.ContainsKey((int)obj["_pid_"]))
                                    {
                                        model.gid = (int)_id_maps[(int)obj["_pid_"]][0];
                                        model.pid = (int)_id_maps[(int)obj["_pid_"]][1];
                                    }
                                    model.id = new SysSerialServices().GetNewSerial("three3." + form.id, null);
                                    db.three3.Add(model);
                                    break;
                                case RowState.Deleted:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                                    break;
                            }
                        }
                    }
                }
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        public object Delete(string id)
        {
            var _ids = new List<int>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = int.Parse(_id);
                    db.three2.Remove(r => r.pid == did);
                    db.three1.Remove(r => r.id == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newid()
        {
            return new SysSerialServices().GetNewSerial("three1");
        }

    }
}
