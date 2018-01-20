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

namespace Fruit.Web.Areas.Baseinfo.Controllers
{
    public partial class FptypeController : Controller
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
            BD_InvTypes form = null;
            List<ComboItem> BillType = null;
            using(var db = new SysContext())
            {
                BillType = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='invoiceType'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                form = db.BD_InvTypes.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new BD_InvTypes
                {
                    FID = id
                };
            }
            return View(new { form = form, dataSource = new { BillType }});
        }

    }
    public partial class FptypeApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class BD_InvTypesListModel {
            public long FID { get; set; }
            public string Name { get; set; }
            public string BillType { get; set; }
            public string BillType_RefText { get; set; }
            public decimal TaxRate { get; set; }
            public decimal Ratio { get; set; }
            public string Remark { get; set; }
            public string CreatePerson { get; set; }
            public DateTime CreateDate { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<BD_InvTypesListModel>(db.Database, "a.FID ,a.Name ,b.Text BillType_RefText ,a.BillType ,a.TaxRate ,a.Ratio ,a.Remark ,a.CreatePerson ,a.CreateDate ", "BD_InvTypes a LEFT JOIN [Bcp.Sysy].dbo.sys_code b ON a.BillType = b.Value AND (b.CodeType='invoiceType') ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<BD_InvTypes>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.BD_InvTypes.Find(form.FID);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.BD_InvTypes.Add(form);
                }
                else
                {
                    dbForm.Name = form.Name;
                    dbForm.BillType = form.BillType;
                    dbForm.TaxRate = form.TaxRate;
                    dbForm.Ratio = form.Ratio;
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
                    db.BD_InvTypes.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return new SysSerialServices().GetNewSerial("BD_InvTypes");
        }

    }
}
