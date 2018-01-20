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
    public partial class Wq_ShangchaoController : Controller
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
            wq_businesscustomers form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.wq_businesscustomers.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new wq_businesscustomers
                {
                    id = id,
                    CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode),
                    DealerType = string.Format("{0}", "商超客户")
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Wq_ShangchaoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_businesscustomersListModel {
            public int? id { get; set; }
            public string DealerCode { get; set; }
            public string DealerType { get; set; }
            public string Businessarea { get; set; }
            public string Procurementmode { get; set; }
            public string Remark { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            sbCondition.AppendFormat("({0}) AND ", string.Format("{0}{1}{2}{3}{4}", "((a.CompCode='", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "') or ('", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode, "'='super'))"));
            SerachCondition.TextBox(sbCondition, "DealerCode", "a.DealerCode", "");
            SerachCondition.TextBox(sbCondition, "Procurementmode", "a.Procurementmode", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<wq_businesscustomersListModel>(db.Database, "a.id ,a.DealerCode ,a.DealerType ,a.Businessarea ,a.Procurementmode ,a.Remark ", "wq_businesscustomers a ", sbCondition.ToString(), "id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<wq_businesscustomers>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.wq_businesscustomers.Find(form.id);
                if (dbForm == null)
                {
                    form.CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode);
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.wq_businesscustomers.Add(form);
                }
                else
                {
                    dbForm.DealerCode = form.DealerCode;
                    dbForm.DealerType = form.DealerType;
                    dbForm.Businessarea = form.Businessarea;
                    dbForm.Procurementmode = form.Procurementmode;
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
                    db.wq_businesscustomers.Remove(r => r.id == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newid()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("wq_businesscustomers", null));
        }

    }
}
