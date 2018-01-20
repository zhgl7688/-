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

namespace Fruit.Web.Areas.Whe.Controllers
{
    public partial class WmqualitycheckmController : Controller
    {
        public ActionResult Index(string id = null)
        {
            if (string.IsNullOrEmpty(id))
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
            Cg_cgddm form = null;
            using(var db = new Jiajusale9Context())
            {
                form = db.Cg_cgddm.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new Cg_cgddm
                {
                    Dh = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class WmqualitycheckmApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class Cg_cgddmListModel {
            public string Dh { get; set; }
            public int? Zt { get; set; }
            public int? Practice { get; set; }
            public string Ydh { get; set; }
            public int? Ck { get; set; }
            public int? Gys { get; set; }
            public int? Cgy { get; set; }
            public string Lxr { get; set; }
            public string Lxdh { get; set; }
            public int? Hb { get; set; }
            public DateTime? Ccrq { get; set; }
            public DateTime? Dhrq { get; set; }
            public int? Jsfs { get; set; }
            public int? Fktj { get; set; }
            public float? Hl { get; set; }
            public float? Taxrate { get; set; }
            public string Dhdz { get; set; }
            public float? Cgje { get; set; }
            public float? Jjje { get; set; }
            public float? Sjje { get; set; }
            public float? Yunfei { get; set; }
            public float? Yufuje { get; set; }
            public float? Yfkye { get; set; }
            public float? Yifuje { get; set; }
            public string Bz { get; set; }
            public DateTime? Zdrq { get; set; }
            public int? Zd { get; set; }
            public DateTime? Shrq { get; set; }
            public int? Sh { get; set; }
            public DateTime? Finishrq { get; set; }
            public bool Ifjz { get; set; }
            public string workFlowId { get; set; }
            public string workFlowInsId { get; set; }
            public string WorktaskId { get; set; }
            public string WorktaskInsId { get; set; }
            public bool IfSubmit { get; set; }
            public int? Ystk { get; set; }
            public string Gbbz { get; set; }
            public DateTime? GbDate { get; set; }
            public int? Account { get; set; }
            public float? Cjfh { get; set; }
            public float? Ml { get; set; }
            public string CurrentSPList { get; set; }
            public int? NodeID { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new Jiajusale9Context())
            {
                return pageReq.ToPageList<Cg_cgddmListModel>(db.Database, "a.Dh ,a.Zt ,a.Practice ,a.Ydh ,a.Ck ,a.Gys ,a.Cgy ,a.Lxr ,a.Lxdh ,a.Hb ,a.Ccrq ,a.Dhrq ,a.Jsfs ,a.Fktj ,a.Hl ,a.Taxrate ,a.Dhdz ,a.Cgje ,a.Jjje ,a.Sjje ,a.Yunfei ,a.Yufuje ,a.Yfkye ,a.Yifuje ,a.Bz ,a.Zdrq ,a.Zd ,a.Shrq ,a.Sh ,a.Finishrq ,a.Ifjz ,a.workFlowId ,a.workFlowInsId ,a.WorktaskId ,a.WorktaskInsId ,a.IfSubmit ,a.Ystk ,a.Gbbz ,a.GbDate ,a.Account ,a.Cjfh ,a.Ml ,a.CurrentSPList ,a.NodeID ", "Cg_cgddm a ", sbCondition.ToString(), "Dh", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<Cg_cgddm>();
            using (var db = new Jiajusale9Context())
            {
                var dbForm = db.Cg_cgddm.Find(form.Dh);
                if (dbForm == null)
                {
                    db.Cg_cgddm.Add(form);
                }
                else
                {
                    dbForm.Zt = form.Zt;
                    dbForm.Practice = form.Practice;
                    dbForm.Ydh = form.Ydh;
                    dbForm.Ck = form.Ck;
                    dbForm.Gys = form.Gys;
                    dbForm.Cgy = form.Cgy;
                    dbForm.Lxr = form.Lxr;
                    dbForm.Lxdh = form.Lxdh;
                    dbForm.Hb = form.Hb;
                    dbForm.Ccrq = form.Ccrq;
                    dbForm.Dhrq = form.Dhrq;
                    dbForm.Jsfs = form.Jsfs;
                    dbForm.Fktj = form.Fktj;
                    dbForm.Hl = form.Hl;
                    dbForm.Taxrate = form.Taxrate;
                    dbForm.Dhdz = form.Dhdz;
                    dbForm.Cgje = form.Cgje;
                    dbForm.Jjje = form.Jjje;
                    dbForm.Sjje = form.Sjje;
                    dbForm.Yunfei = form.Yunfei;
                    dbForm.Yufuje = form.Yufuje;
                    dbForm.Yfkye = form.Yfkye;
                    dbForm.Yifuje = form.Yifuje;
                    dbForm.Bz = form.Bz;
                    dbForm.Zdrq = form.Zdrq;
                    dbForm.Zd = form.Zd;
                    dbForm.Shrq = form.Shrq;
                    dbForm.Sh = form.Sh;
                    dbForm.Finishrq = form.Finishrq;
                    dbForm.Ifjz = form.Ifjz;
                    dbForm.workFlowId = form.workFlowId;
                    dbForm.workFlowInsId = form.workFlowInsId;
                    dbForm.WorktaskId = form.WorktaskId;
                    dbForm.WorktaskInsId = form.WorktaskInsId;
                    dbForm.IfSubmit = form.IfSubmit;
                    dbForm.Ystk = form.Ystk;
                    dbForm.Gbbz = form.Gbbz;
                    dbForm.GbDate = form.GbDate;
                    dbForm.Account = form.Account;
                    dbForm.Cjfh = form.Cjfh;
                    dbForm.Ml = form.Ml;
                    dbForm.CurrentSPList = form.CurrentSPList;
                    dbForm.NodeID = form.NodeID;
                }
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        public object Delete(string id)
        {
            var _ids = new List<string>();

            using (var db = new Jiajusale9Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = _id;
                    db.Cg_cgddm.Remove(r => r.Dh == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewDh()
        {
            return new SysSerialServices().GetNewSerial("Cg_cgddm");
        }

    }
}
