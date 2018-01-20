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
    public partial class Wq_TuangouController : Controller
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
            wq_grouppurchase form = null;
            List<ComboItem> DealerCode = null, DealerType = null;
            using(var db = new SysContext())
            {
                DealerType = db.Database.SqlQuery<ComboItem>("select Text Text, Text Value from sys_code where " + string.Format("{0}", "CodeType='Kz_zd'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                DealerCode = db.Database.SqlQuery<ComboItem>("select PopName Text, PopCode Value from wq_termPop where " + string.Format("{0}{1}{2}", "CompCode='", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "'")).ToList();
                form = db.wq_grouppurchase.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new wq_grouppurchase
                {
                    id = id,
                    CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode),
                    DealerType = string.Format("{0}", "团购客户")
                };
            }
            return View(new { form = form, dataSource = new { DealerCode,DealerType }});
        }

    }
    public partial class Wq_TuangouApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_grouppurchaseListModel {
            public int? id { get; set; }
            public string DealerCode { get; set; }
            public string DealerCode_RefText { get; set; }
            public string DealerType { get; set; }
            public string DealerType_RefText { get; set; }
            public decimal? Commodityprice { get; set; }
            public string address { get; set; }
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
                return pageReq.ToPageList<wq_grouppurchaseListModel>(db.Database, "a.id ,b.PopName DealerCode_RefText ,a.DealerCode ,c.Text DealerType_RefText ,a.DealerType ,a.Commodityprice ,a.address ,a.Remark ", "wq_grouppurchase a LEFT JOIN wq_termPop b ON a.DealerCode = b.PopCode LEFT JOIN [Bcp.Sys].dbo.sys_code c ON a.DealerType = c.Text ", sbCondition.ToString(), "id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<wq_grouppurchase>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.wq_grouppurchase.Find(form.id);
                if (dbForm == null)
                {
                    form.CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode);
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.wq_grouppurchase.Add(form);
                }
                else
                {
                    dbForm.DealerCode = form.DealerCode;
                    dbForm.DealerType = form.DealerType;
                    dbForm.Commodityprice = form.Commodityprice;
                    dbForm.address = form.address;
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
                    db.wq_grouppurchase.Remove(r => r.id == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newid()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("wq_grouppurchase", null));
        }

    }
}
