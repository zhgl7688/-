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
    public partial class CustomerController : Controller
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
            BD_Customers form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.BD_Customers.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new BD_Customers
                {
                    FID = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class CustomerApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class BD_CustomersListModel {
            public long FID { get; set; }
            public string Contact { get; set; }
            public string CName { get; set; }
            public string BankAcc { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public string Remark { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.TextBox(sbCondition, "Contact", "a.Contact", "");
            SerachCondition.TextBox(sbCondition, "CName", "a.CName", "");
            SerachCondition.TextBox(sbCondition, "BankAcc", "a.BankAcc", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<BD_CustomersListModel>(db.Database, "a.FID ,a.Contact ,a.CName ,a.BankAcc ,a.Phone ,a.Address ,a.Remark ", "BD_Customers a ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<BD_Customers>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.BD_Customers.Find(form.FID);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.BD_Customers.Add(form);
                }
                else
                {
                    dbForm.Contact = form.Contact;
                    dbForm.CName = form.CName;
                    dbForm.BankAcc = form.BankAcc;
                    dbForm.Phone = form.Phone;
                    dbForm.Address = form.Address;
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
                    db.BD_Customers.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return new SysSerialServices().GetNewSerial("BD_Customers");
        }

    }
}
