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
    public partial class Fw_RecordinfoController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> memid, frommodule;
                using(var db = new LUOLAI1401Context())
                {
                    memid = db.fw_memberinfo.Select(i=>new ComboItem{ Text = i.realname, Value = "" +  i.memid }).ToList();
                }
                using(var db = new SysContext())
                {
                    frommodule = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='SourceModule'")).ToList();
                }
                return View(new {dataSource = new {memid,frommodule}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(int id)
        {
            fw_recordinfo form = null;
            List<ComboItem> frommodule = null, memid = null;
            using(var db = new SysContext())
            {
                frommodule = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='SourceModule'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                memid = db.fw_memberinfo.Select(i=>new ComboItem{ Text = i.realname, Value = "" + i.memid }).ToList();
                form = db.fw_recordinfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_recordinfo
                {
                    Id = id
                };
            }
            return View(new { form = form, dataSource = new { frommodule,memid }});
        }

    }
    public partial class Fw_RecordinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_recordinfoListModel {
            public int Id { get; set; }
            public int? fromid { get; set; }
            public string frommodule { get; set; }
            public string frommodule_RefText { get; set; }
            public decimal? price { get; set; }
            public decimal? counts { get; set; }
            public string memid { get; set; }
            public string memid_RefText { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.Dropdown(sbCondition, "frommodule", "a.frommodule", "");
            SerachCondition.Dropdown(sbCondition, "memid", "a.memid", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_recordinfoListModel>(db.Database, "a.Id ,a.fromid ,b.Text frommodule_RefText ,a.frommodule ,a.price ,a.counts ,c.realname memid_RefText ,a.memid ", "fw_recordinfo a LEFT JOIN [SYS_YLW].dbo.sys_code b ON a.frommodule = b.Value AND (b.CodeType='SourceModule') LEFT JOIN fw_memberinfo c ON a.memid = c.memid ", sbCondition.ToString(), "a.Id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_recordinfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_recordinfo.Find(form.Id);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.fw_recordinfo.Add(form);
                }
                else
                {
                    dbForm.fromid = form.fromid;
                    dbForm.frommodule = form.frommodule;
                    dbForm.price = form.price;
                    dbForm.counts = form.counts;
                    dbForm.memid = form.memid;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                    dbForm.UpdatePerson = form.UpdatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                }
                // 记录多级零时主键对应(key int 为 js 生成的页内全局唯一编号)
                var _id_maps = new Dictionary<int, object[]>();
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
                    db.fw_recordinfo.Remove(r => r.Id == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewId()
        {
            return new SysSerialServices().GetNewSerial("fw_recordinfo");
        }

    }
}
