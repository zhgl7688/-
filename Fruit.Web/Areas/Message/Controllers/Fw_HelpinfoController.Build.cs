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

namespace Fruit.Web.Areas.Message.Controllers
{
    public partial class Fw_HelpinfoController : Controller
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
        public ActionResult Edit(string id)
        {
            fw_helpinfo form = null;
            List<ComboItem> isenabled = null;
            using(var db = new SysContext())
            {
                isenabled = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_helpinfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_helpinfo
                {
                    helpid = id
                };
            }
            return View(new { form = form, dataSource = new { isenabled }});
        }

    }
    public partial class Fw_HelpinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_helpinfoListModel {
            public string helpid { get; set; }
            public string helptitle { get; set; }
            public int? isenabled { get; set; }
            public string isenabled_RefText { get; set; }
            public DateTime? CreateDate { get; set; }
            public string CreatePerson { get; set; }
            public DateTime? UpdateDate { get; set; }
            public string UpdatePerson { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.TextBox(sbCondition, "helptitle", "a.helptitle", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_helpinfoListModel>(db.Database, "a.helpid ,a.helptitle ,b.Text isenabled_RefText ,a.isenabled ,a.CreateDate ,a.CreatePerson ,a.UpdateDate ,a.UpdatePerson ", "fw_helpinfo a LEFT JOIN [SYS_YLW].dbo.sys_code b ON a.isenabled = b.Value AND (b.CodeType='YN') ", sbCondition.ToString(), "a.helpid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_helpinfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_helpinfo.Find(form.helpid);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.fw_helpinfo.Add(form);
                }
                else
                {
                    dbForm.helptitle = form.helptitle;
                    dbForm.isenabled = form.isenabled;
                    dbForm.helpcontent_h = form.helpcontent_h;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                    dbForm.UpdatePerson = form.UpdatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                }
                // 记录多级零时主键对应(key int 为 js 生成的页内全局唯一编号)
                var _id_maps = new Dictionary<int, object[]>();
                db.SaveChanges();
            }
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "UPDATE fw_helpinfo SET helpcontent=REPLACE(helpcontent_h,'/upload/','/admin/upload/') WHERE helpid=@helpid";
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@helpid";
                p1.Value = form.helpid;
                cmd.Parameters.Add(p1);
                cmd.ExecuteNonQuery();
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
                    db.fw_helpinfo.Remove(r => r.helpid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newhelpid()
        {
            return new SysSerialServices().GetNewSerial("fw_helpinfo");
        }

    }
}
