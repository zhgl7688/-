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
    public partial class Wq_CanyinController : Controller
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
            wq_diningcustomers form = null;
            List<ComboItem> DealerCode = null, shoptype = null;
            using(var db = new SysContext())
            {
                shoptype = db.Database.SqlQuery<ComboItem>("select Text Text, Text Value from sys_code where " + string.Format("{0}", "CodeType='jdlx'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                DealerCode = db.Database.SqlQuery<ComboItem>("select PopName Text, PopCode Value from wq_termPop where " + string.Format("{0}{1}{2}", "CompCode='", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "'")).ToList();
                form = db.wq_diningcustomers.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new wq_diningcustomers
                {
                    id = id,
                    CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode),
                    DealerType = string.Format("{0}", "餐饮客户")
                };
            }
            return View(new { form = form, dataSource = new { DealerCode,shoptype }});
        }

    }
    public partial class Wq_CanyinApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_diningcustomersListModel {
            public int? id { get; set; }
            public string DealerCode { get; set; }
            public string DealerCode_RefText { get; set; }
            public string DealerType { get; set; }
            public string attendance { get; set; }
            public string averageconsumption { get; set; }
            public string shoptype { get; set; }
            public string shoptype_RefText { get; set; }
            public string between_number { get; set; }
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
                return pageReq.ToPageList<wq_diningcustomersListModel>(db.Database, "a.id ,b.PopName DealerCode_RefText ,a.DealerCode ,a.DealerType ,a.attendance ,a.averageconsumption ,c.Text shoptype_RefText ,a.shoptype ,a.between_number ,a.Remark ", "wq_diningcustomers a LEFT JOIN wq_termPop b ON a.DealerCode = b.PopCode LEFT JOIN [Bcp.Sys].dbo.sys_code c ON a.shoptype = c.Text ", sbCondition.ToString(), "id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<wq_diningcustomers>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.wq_diningcustomers.Find(form.id);
                if (dbForm == null)
                {
                    form.CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode);
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.wq_diningcustomers.Add(form);
                }
                else
                {
                    dbForm.DealerCode = form.DealerCode;
                    dbForm.DealerType = form.DealerType;
                    dbForm.attendance = form.attendance;
                    dbForm.averageconsumption = form.averageconsumption;
                    dbForm.shoptype = form.shoptype;
                    dbForm.between_number = form.between_number;
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
                    db.wq_diningcustomers.Remove(r => r.id == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newid()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("wq_diningcustomers", null));
        }

    }
}
