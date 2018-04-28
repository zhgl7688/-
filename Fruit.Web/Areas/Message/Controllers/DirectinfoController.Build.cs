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

namespace Fruit.Web.Areas.Message.Controllers
{
    public partial class DirectinfoController : Controller
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
        public ActionResult Edit(string id)
        {
            fw_Direct form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_Direct.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_Direct
                {
                    dirid = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class DirectinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_DirectListModel {
            public string dirid { get; set; }
            public string item { get; set; }
            public string corpname { get; set; }
            public string dirname { get; set; }
            public decimal? dirvalue { get; set; }
            public int? UpValue { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_DirectListModel>(db.Database, "a.dirid ,a.item ,a.corpname ,a.dirname ,a.dirvalue ,a.UpValue ", "fw_Direct a ", sbCondition.ToString(), "a.dirid", "desc");
            }
        }
        public object Post(JArray post)
        {
            using (var db = new LUOLAI1401Context())
            {
                for(var i = 0; i < post.Count; i++)
                {
                    var form = post[i].ToObject<fw_Direct>(JsonExtension.FixJsonSerializer);
                    var dbForm = db.fw_Direct.Find(form.dirid);
                    RowState rowState = (RowState)(int)post[i]["_row_state"];
                    switch(rowState)
                    {
                        case RowState.Changed:
                            dbForm.item = form.item;
                            dbForm.corpname = form.corpname;
                            dbForm.dirname = form.dirname;
                            dbForm.dirvalue = form.dirvalue;
                            dbForm.UpValue = form.UpValue;
                            break;
                        case RowState.Deleted:
                            db.Entry(dbForm).State = System.Data.Entity.EntityState.Deleted;
                            break;
                        case RowState.New:
                            form.dirid = (string)Newdirid();
                            db.fw_Direct.Add(form);
                            break;
                    }
                }
                db.SaveChanges();
                return true;
            }
        }
        [System.Web.Http.HttpPost]
        public object DoBFB(JObject row)
        {
            object result = null;
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "exec ups_examine '','" + (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode + "','5','L'";
                result = cmd.ExecuteScalar();
            }
            return result;
        }
        public object Delete(string id)
        {
            var _ids = new List<string>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = _id;
                    db.fw_Direct.Remove(r => r.dirid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newdirid()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("fw_Direct", null));
        }

    }
}
