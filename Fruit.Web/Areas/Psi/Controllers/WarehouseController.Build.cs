/// 注意：本文件由 FruitBuilder 生成和管理，请误手工更改
using Fruit.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Fruit.Data;
using Fruit.Models;
using Newtonsoft.Json.Linq;

namespace Fruit.Web.Areas.Psi.Controllers
{
    public partial class WarehouseController : Controller
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
            tbBdWarehouse form = null;
            using(var db = new SMTERPContext())
            {
                form = db.tbBdWarehouse.Find(Guid.Parse(id));
            }
            if (form == null)
            {
                form = new tbBdWarehouse
                {
                    Code = Guid.Parse(id)
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class WarehouseApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class tbBdWarehouseListModel {
            public Guid Code { get; set; }
            public string Name { get; set; }
            public Guid? DepartCode { get; set; }
            public Guid? ParentCode { get; set; }
            public string Telephone { get; set; }
            public string Address { get; set; }
            public string Mobilephone { get; set; }
            public string Remark { get; set; }
            public int? Sort { get; set; }
            public bool? IfStop { get; set; }
            public bool? IfStores { get; set; }
        }
        public object Get()
        {
            var pageReq = new PageRequest();
            using (var db = new SMTERPContext())
            {
                return pageReq.ToPageList<tbBdWarehouseListModel>(db.Database, "a.Code ,a.Name ,a.DepartCode ,a.ParentCode ,a.Telephone ,a.Address ,a.Mobilephone ,a.Remark ,a.Sort ,a.IfStop ,a.IfStores ", "tbBdWarehouse a ", "", "Code", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<tbBdWarehouse>();
            using (var db = new SMTERPContext())
            {
                var dbForm = db.tbBdWarehouse.Find(form.Code);
                if (dbForm == null)
                {
                    db.tbBdWarehouse.Add(form);
                }
                else
                {
                    dbForm.Name = form.Name;
                    dbForm.LevelCode = form.LevelCode;
                    dbForm.Level = form.Level;
                    dbForm.DepartCode = form.DepartCode;
                    dbForm.ParentCode = form.ParentCode;
                    dbForm.Telephone = form.Telephone;
                    dbForm.Address = form.Address;
                    dbForm.Mobilephone = form.Mobilephone;
                    dbForm.Remark = form.Remark;
                    dbForm.Sort = form.Sort;
                    dbForm.IfStop = form.IfStop;
                    dbForm.IfStores = form.IfStores;
                }
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        public object Delete(string id)
        {

            using (var db = new SMTERPContext())
            {
                db.tbBdWarehouse.Remove(r => r.Code == Guid.Parse(id));
                db.SaveChanges();
            }
            return true;
        }
        public object NewCode()
        {
            return Guid.NewGuid();
        }

    }
}
