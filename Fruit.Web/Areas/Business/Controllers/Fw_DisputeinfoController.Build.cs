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
    public partial class Fw_DisputeinfoController : Controller
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
            fw_disputeinfo form = null;
            List<ComboItem> ordid = null, disputememid = null;
            using(var db = new SysContext())
            {
                disputememid = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='Breach'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                ordid = db.fw_orderinfo.Select(i=>new ComboItem{ Text = i.ordid, Value = "" + i.ordid }).ToList();
                form = db.fw_disputeinfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_disputeinfo
                {
                    disid = id
                };
            }
            return View(new { form = form, dataSource = new { ordid,disputememid }});
        }

    }
    public partial class Fw_DisputeinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_disputeinfoListModel {
            public int disid { get; set; }
            public string ordid { get; set; }
            public string ordid_RefText { get; set; }
            public string disputememid { get; set; }
            public string disputememid_RefText { get; set; }
            public decimal? dispositacount { get; set; }
            public string remark { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_disputeinfoListModel>(db.Database, "a.disid ,b.ordid ordid_RefText ,a.ordid ,c.Text disputememid_RefText ,a.disputememid ,a.dispositacount ,a.remark ", "fw_disputeinfo a LEFT JOIN fw_orderinfo b ON a.ordid = b.ordid LEFT JOIN [SYS_YLW].dbo.sys_code c ON a.disputememid = c.Value AND (c.CodeType='Breach') ", sbCondition.ToString(), "a.disid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_disputeinfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_disputeinfo.Find(form.disid);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.fw_disputeinfo.Add(form);
                }
                else
                {
                    dbForm.ordid = form.ordid;
                    dbForm.disputememid = form.disputememid;
                    dbForm.dispositacount = form.dispositacount;
                    dbForm.remark = form.remark;
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
                    db.fw_disputeinfo.Remove(r => r.disid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newdisid()
        {
            return new SysSerialServices().GetNewSerial("fw_disputeinfo");
        }

    }
}
