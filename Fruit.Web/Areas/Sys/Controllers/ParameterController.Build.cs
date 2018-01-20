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

namespace Fruit.Web.Areas.Sys.Controllers
{
    public partial class ParameterController : Controller
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
            BD_SysParas form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.BD_SysParas.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new BD_SysParas
                {
                    FID = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class ParameterApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class BD_SysParasListModel {
            public int FID { get; set; }
            public int bidEx { get; set; }
            public int certEx { get; set; }
            public int certEx2 { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<BD_SysParasListModel>(db.Database, "a.FID ,a.bidEx ,a.certEx ,a.certEx2 ", "BD_SysParas a ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<BD_SysParas>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.BD_SysParas.Find(form.FID);
                if (dbForm == null)
                {
                    db.BD_SysParas.Add(form);
                }
                else
                {
                    dbForm.bidEx = form.bidEx;
                    dbForm.certEx = form.certEx;
                    dbForm.certEx2 = form.certEx2;
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
                    db.BD_SysParas.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return new SysSerialServices().GetNewSerial("BD_SysParas");
        }

    }
}
