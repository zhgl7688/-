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

namespace Fruit.Web.Areas.Project.Controllers
{
    public partial class ProjectinfobidController : Controller
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
        public ActionResult Edit(long id)
        {
            PM_ProjectInfoBids form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.PM_ProjectInfoBids.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new PM_ProjectInfoBids
                {
                    FID = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class ProjectinfobidApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class PM_ProjectInfoBidsListModel {
            public long FID { get; set; }
            public int Code { get; set; }
            public string ProjName { get; set; }
            public string CustID { get; set; }
            public string appNumber { get; set; }
            public string Owner { get; set; }
            public decimal totalInvAmt { get; set; }
            public decimal depAmt { get; set; }
            public long depAmtStatus { get; set; }
            public decimal advAmt { get; set; }
            public decimal bookAmt { get; set; }
            public decimal bidAmt { get; set; }
            public decimal agencyAmt { get; set; }
            public string PManager { get; set; }
            public DateTime bidDate { get; set; }
            public string bidAddress { get; set; }
            public string bidPerson { get; set; }
            public long bidAgent { get; set; }
            public string Attention { get; set; }
            public string Winner { get; set; }
            public string Remark { get; set; }
            public string CreatePerson { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime UpdateDate { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<PM_ProjectInfoBidsListModel>(db.Database, "a.FID ,a.Code ,a.ProjName ,a.CustID ,a.appNumber ,a.Owner ,a.totalInvAmt ,a.depAmt ,a.depAmtStatus ,a.advAmt ,a.bookAmt ,a.bidAmt ,a.agencyAmt ,a.PManager ,a.bidDate ,a.bidAddress ,a.bidPerson ,a.bidAgent ,a.Attention ,a.Winner ,a.Remark ,a.CreatePerson ,a.CreateDate ,a.UpdateDate ", "PM_ProjectInfoBids a ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<PM_ProjectInfoBids>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.PM_ProjectInfoBids.Find(form.FID);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.PM_ProjectInfoBids.Add(form);
                }
                else
                {
                    dbForm.Code = form.Code;
                    dbForm.ProjName = form.ProjName;
                    dbForm.CustID = form.CustID;
                    dbForm.appNumber = form.appNumber;
                    dbForm.Owner = form.Owner;
                    dbForm.totalInvAmt = form.totalInvAmt;
                    dbForm.depAmt = form.depAmt;
                    dbForm.depAmtStatus = form.depAmtStatus;
                    dbForm.advAmt = form.advAmt;
                    dbForm.bookAmt = form.bookAmt;
                    dbForm.bidAmt = form.bidAmt;
                    dbForm.agencyAmt = form.agencyAmt;
                    dbForm.PManager = form.PManager;
                    dbForm.bidDate = form.bidDate;
                    dbForm.bidAddress = form.bidAddress;
                    dbForm.bidPerson = form.bidPerson;
                    dbForm.bidAgent = form.bidAgent;
                    dbForm.Attention = form.Attention;
                    dbForm.Winner = form.Winner;
                    dbForm.Remark = form.Remark;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                }
                // 记录多级零时主键对应(key int 为 js 生成的页内全局唯一编号)
                var _id_maps = new Dictionary<int, object[]>();
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        public object Delete(string id)
        {
            var _ids = new List<long>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = long.Parse(_id);
                    db.PM_ProjectInfoBids.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return new SysSerialServices().GetNewSerial("PM_ProjectInfoBids");
        }

    }
}
