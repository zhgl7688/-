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

namespace Fruit.Web.Areas.Business.Controllers
{
    public partial class Fw_ProductinfoController : Controller
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
            fw_productinfo form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_productinfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_productinfo
                {
                    proid = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Fw_ProductinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_productinfoListModel {
            public int proid { get; set; }
            public int? catid { get; set; }
            public string proname { get; set; }
            public string spec { get; set; }
            public string memid { get; set; }
            public decimal? price { get; set; }
            public string sellmode { get; set; }
            public string grade { get; set; }
            public int? minprodcount { get; set; }
            public int? stock { get; set; }
            public int? isactive { get; set; }
            public int? depositseller { get; set; }
            public int? depositbuyer { get; set; }
            public int? issale { get; set; }
            public DateTime? createdate { get; set; }
            public DateTime? modifydate { get; set; }
            public int? viewcount { get; set; }
            public string remark { get; set; }
            public string procode { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_productinfoListModel>(db.Database, "a.proid ,a.catid ,a.proname ,a.spec ,a.memid ,a.price ,a.sellmode ,a.grade ,a.minprodcount ,a.stock ,a.isactive ,a.depositseller ,a.depositbuyer ,a.issale ,a.createdate ,a.modifydate ,a.viewcount ,a.remark ,a.procode ", "fw_productinfo a ", sbCondition.ToString(), "a.proid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_productinfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_productinfo.Find(form.proid);
                if (dbForm == null)
                {
                    db.fw_productinfo.Add(form);
                }
                else
                {
                    dbForm.catid = form.catid;
                    dbForm.proname = form.proname;
                    dbForm.spec = form.spec;
                    dbForm.memid = form.memid;
                    dbForm.price = form.price;
                    dbForm.sellmode = form.sellmode;
                    dbForm.grade = form.grade;
                    dbForm.minprodcount = form.minprodcount;
                    dbForm.stock = form.stock;
                    dbForm.isactive = form.isactive;
                    dbForm.depositseller = form.depositseller;
                    dbForm.depositbuyer = form.depositbuyer;
                    dbForm.issale = form.issale;
                    dbForm.createdate = form.createdate;
                    dbForm.modifydate = form.modifydate;
                    dbForm.viewcount = form.viewcount;
                    dbForm.remark = form.remark;
                    dbForm.procode = form.procode;
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
                    db.fw_productinfo.Remove(r => r.proid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newproid()
        {
            return new SysSerialServices().GetNewSerial("fw_productinfo");
        }

    }
}
