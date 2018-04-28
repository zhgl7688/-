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

namespace Fruit.Web.Areas.Baseinfo.Controllers
{
    public partial class Fw_DepositinfoController : Controller
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

        // 用于 cat2 字段级联数据源类型
        public class cat2ComboItem : ComboItem
        {
            public string parentid { get; set; }
        }

        public ActionResult Edit(int id)
        {
            fw_depositinfo form = null;
            List<ComboItem> cat1 = null, memid = null;
            List<cat2ComboItem> cat2 = null;
            using(var db = new LUOLAI1401Context())
            {
                cat1 = db.Database.SqlQuery<ComboItem>("select catname Text, cast(catid as varchar(10)) Value from fw_categoryinfo where " + string.Format("{0}", "/*TABLEALIAS*/parentid='0'")).ToList();
                cat2 = db.Database.SqlQuery<cat2ComboItem>("select catname Text, cast(catid as varchar(10)) Value,cast(parentid as varchar(10)) parentid from fw_categoryinfo where " + string.Format("{0}", "/*TABLEALIAS*/parentid !='0'")).ToList();
                memid = db.fw_memberinfo.Select(i=>new ComboItem{ Text = i.realname, Value = "" + i.memid }).ToList();
                form = db.fw_depositinfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_depositinfo
                {
                    depid = id
                };
            }
            return View(new { form = form, dataSource = new { cat1,cat2,memid }});
        }

    }
    public partial class Fw_DepositinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_depositinfoListModel {
            public int depid { get; set; }
            public string cat1 { get; set; }
            public string cat1_RefText { get; set; }
            public string cat2 { get; set; }
            public string cat2_RefText { get; set; }
            public string memid { get; set; }
            public string memid_RefText { get; set; }
            public decimal? deposit { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_depositinfoListModel>(db.Database, "a.depid ,b.catname cat1_RefText ,a.cat1 ,c.catname cat2_RefText ,a.cat2 ,d.realname memid_RefText ,a.memid ,a.deposit ", "fw_depositinfo a LEFT JOIN fw_categoryinfo b ON a.cat1 = b.catid AND (b.parentid='0') LEFT JOIN fw_categoryinfo c ON a.cat2 = c.catid AND (c.parentid !='0') LEFT JOIN fw_memberinfo d ON a.memid = d.memid ", sbCondition.ToString(), "a.depid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_depositinfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_depositinfo.Find(form.depid);
                if (dbForm == null)
                {
                    db.fw_depositinfo.Add(form);
                }
                else
                {
                    dbForm.cat1 = form.cat1;
                    dbForm.cat2 = form.cat2;
                    dbForm.memid = form.memid;
                    dbForm.deposit = form.deposit;
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
                    db.fw_depositinfo.Remove(r => r.depid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newdepid()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("fw_depositinfo", null));
        }

    }
}
