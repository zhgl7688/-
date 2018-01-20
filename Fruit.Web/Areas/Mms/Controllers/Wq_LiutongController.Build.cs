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

namespace Fruit.Web.Areas.Mms.Controllers
{
    public partial class Wq_LiutongController : Controller
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
        public ActionResult Edit(int id)
        {
            wq_distributionclient form = null;
            List<ComboItem> DealerCode = null;
            using(var db = new LUOLAI1401Context())
            {
                DealerCode = db.Database.SqlQuery<ComboItem>("select PopName Text, PopCode Value from wq_termPop where " + string.Format("{0}{1}{2}", "CompCode='", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "'")).ToList();
                form = db.wq_distributionclient.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new wq_distributionclient
                {
                    id = id,
                    CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode),
                    DealerType = string.Format("{0}", "流通客户")
                };
            }
            return View(new { form = form, dataSource = new { DealerCode }});
        }

    }
    public partial class Wq_LiutongApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_distributionclientListModel {
            public int? id { get; set; }
            public string DealerCode { get; set; }
            public string DealerCode_RefText { get; set; }
            public string DealerType { get; set; }
            public string Businessarea { get; set; }
            public string Appearancenumber { get; set; }
            public string Winecapacity { get; set; }
            public string jkfs { get; set; }
            public string Remark { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            sbCondition.AppendFormat("({0}) AND ", string.Format("{0}{1}{2}{3}{4}", "((a.CompCode='", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "') or ('", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode, "'='super'))"));

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<wq_distributionclientListModel>(db.Database, "a.id ,b.PopName DealerCode_RefText ,a.DealerCode ,a.DealerType ,a.Businessarea ,a.Appearancenumber ,a.Winecapacity ,a.jkfs ,a.Remark ", "wq_distributionclient a LEFT JOIN wq_termPop b ON a.DealerCode = b.PopCode ", sbCondition.ToString(), "id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<wq_distributionclient>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.wq_distributionclient.Find(form.id);
                if (dbForm == null)
                {
                    form.CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode);
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.wq_distributionclient.Add(form);
                }
                else
                {
                    dbForm.DealerCode = form.DealerCode;
                    dbForm.DealerType = form.DealerType;
                    dbForm.Businessarea = form.Businessarea;
                    dbForm.Appearancenumber = form.Appearancenumber;
                    dbForm.Winecapacity = form.Winecapacity;
                    dbForm.jkfs = form.jkfs;
                    dbForm.Remark = form.Remark;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                    dbForm.UpdatePerson = form.UpdatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
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
                    db.wq_distributionclient.Remove(r => r.id == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newid()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("wq_distributionclient", null));
        }

    }
}
