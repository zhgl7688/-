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
    public partial class BomController : Controller
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
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class tblBomViewable : tblBom {
            public string ProductCode_RefText { get; set; }
            public string speccode_RefText { get; set; }
        }
        public ActionResult Edit(Guid id)
        {
            tblBom form = null;
            List<TreeItem> parentGUID = null;
            using(var db = new SMTERPContext())
            {
                parentGUID = db.tblBom.Select(t=>new TreeItem{Id = "" + t.guid, Text = "" + t.ProductCode, ParentId = "" + t.parentGUID}).ToList();
                form = db.Database.SqlQuery<tblBomViewable>("select b.Name ProductCode_RefText ,a.ProductCode ,a.guidmain ,a.guid ,a.parentGUID ,a.qty ,a.waste ,a.unit ,a.isBOM ,a.Status ,a.Audit ,a.AuditDate ,a.CreatePerson ,a.CreateDate ,a.remark ,a.productkindguid ,a.price ,a.totalmoney ,a.version ,a.bommodel ,a.CT ,a.position ,c.Text speccode_RefText ,a.speccode ,a.color ,a.height ,a.lenght ,a.width ,a.clientguid ,a.begindate ,a.pirtuce ,a.supplierguid  from tblBom a LEFT JOIN tbBdProduct b ON a.ProductCode = b.Guid LEFT JOIN [Bcp.Sys].dbo.sys_code c ON a.speccode = c.Text AND " + string.Format("{0}", "CodeType='Kz_zd'") + " where a.guid = @key", new System.Data.SqlClient.SqlParameter("@key", id)).FirstOrDefault();
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new tblBom
                {
                    guid = id
                };
            }
            return View(new { form = form, dataSource = new { parentGUID }});
        }

    }
    public partial class BomApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class tblBomListModel {
            public Guid guid { get; set; }
            public Guid ProductCode { get; set; }
            public string ProductCode_RefText { get; set; }
            public Guid? guidmain { get; set; }
            public Guid? parentGUID { get; set; }
            public decimal qty { get; set; }
            public decimal? waste { get; set; }
            public string unit { get; set; }
            public bool isBOM { get; set; }
            public string Status { get; set; }
            public string Audit { get; set; }
            public DateTime? AuditDate { get; set; }
            public string remark { get; set; }
            public Guid? productkindguid { get; set; }
            public decimal? price { get; set; }
            public decimal? totalmoney { get; set; }
            public string version { get; set; }
            public string bommodel { get; set; }
            public string CT { get; set; }
            public string position { get; set; }
            public string speccode { get; set; }
            public string speccode_RefText { get; set; }
            public string color { get; set; }
            public string height { get; set; }
            public string lenght { get; set; }
            public string width { get; set; }
            public Guid? clientguid { get; set; }
            public DateTime? begindate { get; set; }
            public byte[] pirtuce { get; set; }
            public Guid? supplierguid { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new SMTERPContext())
            {
                return pageReq.ToPageList<tblBomListModel>(db.Database, "a.guid ,b.Name ProductCode_RefText ,a.ProductCode ,a.guidmain ,a.parentGUID ,a.qty ,a.waste ,a.unit ,a.isBOM ,a.Status ,a.Audit ,a.AuditDate ,a.remark ,a.productkindguid ,a.price ,a.totalmoney ,a.version ,a.bommodel ,a.CT ,a.position ,c.Text speccode_RefText ,a.speccode ,a.color ,a.height ,a.lenght ,a.width ,a.clientguid ,a.begindate ,a.pirtuce ,a.supplierguid ", "tblBom a LEFT JOIN tbBdProduct b ON a.ProductCode = b.Guid LEFT JOIN [Bcp.Sys].dbo.sys_code c ON a.speccode = c.Text ", sbCondition.ToString(), "guid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<tblBom>(JsonExtension.FixJsonSerializer);
            using (var db = new SMTERPContext())
            {
                var dbForm = db.tblBom.Find(form.guid);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.tblBom.Add(form);
                }
                else
                {
                    dbForm.ProductCode = form.ProductCode;
                    dbForm.guidmain = form.guidmain;
                    dbForm.parentGUID = form.parentGUID;
                    dbForm.qty = form.qty;
                    dbForm.waste = form.waste;
                    dbForm.unit = form.unit;
                    dbForm.isBOM = form.isBOM;
                    dbForm.Status = form.Status;
                    dbForm.Audit = form.Audit;
                    dbForm.AuditDate = form.AuditDate;
                    dbForm.remark = form.remark;
                    dbForm.productkindguid = form.productkindguid;
                    dbForm.price = form.price;
                    dbForm.totalmoney = form.totalmoney;
                    dbForm.version = form.version;
                    dbForm.bommodel = form.bommodel;
                    dbForm.CT = form.CT;
                    dbForm.position = form.position;
                    dbForm.speccode = form.speccode;
                    dbForm.color = form.color;
                    dbForm.height = form.height;
                    dbForm.lenght = form.lenght;
                    dbForm.width = form.width;
                    dbForm.clientguid = form.clientguid;
                    dbForm.begindate = form.begindate;
                    dbForm.pirtuce = form.pirtuce;
                    dbForm.supplierguid = form.supplierguid;
                }
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        public object Delete(string id)
        {
            var _ids = new List<Guid>();

            using (var db = new SMTERPContext())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = Guid.Parse(_id);
                    db.tblBom.Remove(r => r.guid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newguid()
        {
            return Guid.NewGuid();
        }

    }
}
