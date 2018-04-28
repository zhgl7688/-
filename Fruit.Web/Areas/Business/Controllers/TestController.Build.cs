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
    public partial class TestController : Controller
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
            fw_Objection form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_Objection.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_Objection
                {
                    objectionid = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class TestApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_ObjectionListModel {
            public int objectionid { get; set; }
            public string orderid { get; set; }
            public decimal? orderprice { get; set; }
            public string Objectionstatus { get; set; }
            public decimal? otherprice { get; set; }
            public string otherstatus { get; set; }
            public string explain { get; set; }
            public string MemId { get; set; }
            public string MemIdSeller { get; set; }
            public DateTime? CancelDate { get; set; }
            public int? objState { get; set; }
            public string isShow { get; set; }
            public string IsAgreed { get; set; }
            public string IsOnJudge { get; set; }
            public int? IsContinue { get; set; }
            public string img0 { get; set; }
            public string img1 { get; set; }
            public string img2 { get; set; }
            public int? Cimg1 { get; set; }
            public int? Cimg2 { get; set; }
            public int? Cimg3 { get; set; }
            public string sysExplain { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_ObjectionListModel>(db.Database, "a.objectionid ,a.orderid ,a.orderprice ,a.Objectionstatus ,a.otherprice ,a.otherstatus ,a.explain ,a.MemId ,a.MemIdSeller ,a.CancelDate ,a.objState ,a.isShow ,a.IsAgreed ,a.IsOnJudge ,a.IsContinue ,a.img0 ,a.img1 ,a.img2 ,a.Cimg1 ,a.Cimg2 ,a.Cimg3 ,a.sysExplain ", "fw_Objection a ", sbCondition.ToString(), "a.objectionid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_Objection>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_Objection.Find(form.objectionid);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.fw_Objection.Add(form);
                }
                else
                {
                    dbForm.orderid = form.orderid;
                    dbForm.orderprice = form.orderprice;
                    dbForm.Objectionstatus = form.Objectionstatus;
                    dbForm.otherprice = form.otherprice;
                    dbForm.otherstatus = form.otherstatus;
                    dbForm.explain = form.explain;
                    dbForm.MemId = form.MemId;
                    dbForm.MemIdSeller = form.MemIdSeller;
                    dbForm.CancelDate = form.CancelDate;
                    dbForm.objState = form.objState;
                    dbForm.isShow = form.isShow;
                    dbForm.IsAgreed = form.IsAgreed;
                    dbForm.IsOnJudge = form.IsOnJudge;
                    dbForm.IsContinue = form.IsContinue;
                    dbForm.img0 = form.img0;
                    dbForm.img1 = form.img1;
                    dbForm.img2 = form.img2;
                    dbForm.Cimg1 = form.Cimg1;
                    dbForm.Cimg2 = form.Cimg2;
                    dbForm.Cimg3 = form.Cimg3;
                    dbForm.sysExplain = form.sysExplain;
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
                    db.fw_Objection.Remove(r => r.objectionid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newobjectionid()
        {
            return new SysSerialServices().GetNewSerial("fw_Objection");
        }

    }
}
