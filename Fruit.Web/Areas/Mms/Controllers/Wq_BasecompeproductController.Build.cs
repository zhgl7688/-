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
    public partial class Wq_BasecompeproductController : Controller
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
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_goodsreportViewable : wq_goodsreport {
            public string UserCode_RefText { get; set; }
            public string DealerCode_RefText { get; set; }
        }
        public ActionResult Edit(string id)
        {
            wq_goodsreport form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.Database.SqlQuery<wq_goodsreportViewable>("select b.name UserCode_RefText ,a.UserCode ,a.PrmDate ,c.PopName DealerCode_RefText ,a.DealerCode ,a.RPdtCode ,a.Remark ,a.Pic1 ,a.Pic2 ,a.Pic3 ,a.Pic4 ,a.Pic5 ,a.Pic6 ,a.Pic7 ,a.Pic8 ,a.CreatePerson ,a.CreateDate ,a.UpdatePerson ,a.UpdateDate ,a.CompCode  from wq_goodsreport a LEFT JOIN PersonInfo b ON a.UserCode = b.psncode LEFT JOIN wq_termPop c ON a.DealerCode = c.PopCode  where a.RPdtCode = @key", new System.Data.SqlClient.SqlParameter("@key", id)).FirstOrDefault();
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new wq_goodsreport
                {
                    RPdtCode = id,
                    CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode)
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Wq_BasecompeproductApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_goodsreportListModel {
            public string UserCode { get; set; }
            public string UserCode_RefText { get; set; }
            public DateTime? PrmDate { get; set; }
            public string DealerCode { get; set; }
            public string DealerCode_RefText { get; set; }
            public string RPdtCode { get; set; }
            public string Remark { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            sbCondition.AppendFormat("({0}) AND ", string.Format("{0}{1}{2}{3}{4}", "(a.CompCode +'`' +a.UserCode IN (SELECT UserCode+'`'+CompCode  FROM dbo.sys_user WHERE OrganizeName LIKE '", System.Web.HttpContext.Current.Session["OrganizeName"], "%')) or ('", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode, "'='super')"));
            SerachCondition.PopupSelect(sbCondition, "UserCode", "a.UserCode", "");
            SerachCondition.Date(sbCondition, "PrmDate", "a.PrmDate", "");
            SerachCondition.PopupSelect(sbCondition, "DealerCode", "a.DealerCode", "");
            SerachCondition.TextBox(sbCondition, "RPdtCode", "a.RPdtCode", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<wq_goodsreportListModel>(db.Database, "b.name UserCode_RefText ,a.UserCode ,a.PrmDate ,c.PopName DealerCode_RefText ,a.DealerCode ,a.RPdtCode ,a.Remark ", "wq_goodsreport a LEFT JOIN PersonInfo b ON a.UserCode = b.psncode LEFT JOIN wq_termPop c ON a.DealerCode = c.PopCode ", sbCondition.ToString(), "RPdtCode", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<wq_goodsreport>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.wq_goodsreport.Find(form.RPdtCode);
                if (dbForm == null)
                {
                    form.CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode);
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.wq_goodsreport.Add(form);
                }
                else
                {
                    dbForm.UserCode = form.UserCode;
                    dbForm.PrmDate = form.PrmDate;
                    dbForm.DealerCode = form.DealerCode;
                    dbForm.Remark = form.Remark;
                    dbForm.Pic1 = form.Pic1;
                    dbForm.Pic2 = form.Pic2;
                    dbForm.Pic3 = form.Pic3;
                    dbForm.Pic4 = form.Pic4;
                    dbForm.Pic5 = form.Pic5;
                    dbForm.Pic6 = form.Pic6;
                    dbForm.Pic7 = form.Pic7;
                    dbForm.Pic8 = form.Pic8;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                    dbForm.UpdatePerson = form.UpdatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                }
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        public object Delete(string id)
        {
            var _ids = new List<string>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = _id;
                    db.wq_goodsreport.Remove(r => r.RPdtCode == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewRPdtCode()
        {
            return string.Format("{0:D12}", new SysSerialServices().GetNewSerial("wq_goodsreport", null));
        }

    }
}
