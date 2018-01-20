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

namespace Fruit.Web.Areas.Rols.Controllers
{
    public partial class VcertificateController : Controller
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
        public ActionResult Edit(int id)
        {
            v_Certificate form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.v_Certificate.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new v_Certificate
                {
                    FID = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class VcertificateApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class v_CertificateListModel {
            public int? FID { get; set; }
            public string Name { get; set; }
            public string Text { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<v_CertificateListModel>(db.Database, "a.FID ,a.Name ,a.Text ", "v_Certificate a ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<v_Certificate>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.v_Certificate.Find(form.FID);
                if (dbForm == null)
                {
                    db.v_Certificate.Add(form);
                }
                else
                {
                    dbForm.Name = form.Name;
                    dbForm.Text = form.Text;
                }
                // 记录多级零时主键对应(key int 为 js 生成的页内全局唯一编号)
                var _id_maps = new Dictionary<int, object[]>();
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
                    db.v_Certificate.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return new SysSerialServices().GetNewSerial("v_Certificate");
        }

    }
}
