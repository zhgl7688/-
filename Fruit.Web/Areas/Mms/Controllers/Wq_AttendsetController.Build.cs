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
    public partial class Wq_AttendsetController : Controller
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
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_attendSetViewable : wq_attendSet {
            public string RoleCode_RefText { get; set; }
        }
        public ActionResult Edit(string id)
        {
            wq_attendSet form = null;
            List<ComboItem> AttPhotoFlag = null, iswatermark = null, isresolution = null;
            using(var db = new SysContext())
            {
                AttPhotoFlag = db.Database.SqlQuery<ComboItem>("select Text Text, Text Value from sys_code where " + string.Format("{0}", "CodeType='yes/no'")).ToList();
                iswatermark = db.Database.SqlQuery<ComboItem>("select Text Text, Text Value from sys_code where " + string.Format("{0}", "CodeType='yes/no'")).ToList();
                isresolution = db.Database.SqlQuery<ComboItem>("select Text Text, Text Value from sys_code where " + string.Format("{0}", "CodeType='zpfbl'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                form = db.Database.SqlQuery<wq_attendSetViewable>("select a.CompCode ,b.RoleName RoleCode_RefText ,a.RoleCode ,a.AttRange ,a.BgnTimeAM ,a.EndTimeAM ,a.BgnTimePM ,a.EndTimePM ,a.AttRole ,a.AttPhotoFlag ,a.iswatermark ,a.isresolution ,a.CreatePerson ,a.CreateDate ,a.UpdatePerson ,a.UpdateDate ,a.id  from wq_attendSet a LEFT JOIN [Bcp.Sys].dbo.sys_role b ON a.RoleCode = b.RoleCode  where a.id = @key", new System.Data.SqlClient.SqlParameter("@key", id)).FirstOrDefault();
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new wq_attendSet
                {
                    id = id,
                    CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode)
                };
            }
            return View(new { form = form, dataSource = new { AttPhotoFlag,iswatermark,isresolution }});
        }

    }
    public partial class Wq_AttendsetApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_attendSetListModel {
            public string id { get; set; }
            public string RoleCode { get; set; }
            public string RoleCode_RefText { get; set; }
            public string BgnTimeAM { get; set; }
            public string EndTimeAM { get; set; }
            public string BgnTimePM { get; set; }
            public string EndTimePM { get; set; }
            public string AttPhotoFlag { get; set; }
            public string AttPhotoFlag_RefText { get; set; }
            public string iswatermark { get; set; }
            public string iswatermark_RefText { get; set; }
            public string isresolution { get; set; }
            public string isresolution_RefText { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            sbCondition.AppendFormat("({0}) AND ", string.Format("{0}{1}{2}", "a.CompCode= '", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "'"));

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<wq_attendSetListModel>(db.Database, "a.id ,b.RoleName RoleCode_RefText ,a.RoleCode ,a.BgnTimeAM ,a.EndTimeAM ,a.BgnTimePM ,a.EndTimePM ,c.Text AttPhotoFlag_RefText ,a.AttPhotoFlag ,d.Text iswatermark_RefText ,a.iswatermark ,e.Text isresolution_RefText ,a.isresolution ", "wq_attendSet a LEFT JOIN [Bcp.Sys].dbo.sys_role b ON a.RoleCode = b.RoleCode LEFT JOIN [Bcp.Sys].dbo.sys_code c ON a.AttPhotoFlag = c.Text LEFT JOIN [Bcp.Sys].dbo.sys_code d ON a.iswatermark = d.Text LEFT JOIN [Bcp.Sys].dbo.sys_code e ON a.isresolution = e.Text ", sbCondition.ToString(), "id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<wq_attendSet>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.wq_attendSet.Find(form.id);
                if (dbForm == null)
                {
                    form.CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode);
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.wq_attendSet.Add(form);
                }
                else
                {
                    dbForm.RoleCode = form.RoleCode;
                    dbForm.BgnTimeAM = form.BgnTimeAM;
                    dbForm.EndTimeAM = form.EndTimeAM;
                    dbForm.BgnTimePM = form.BgnTimePM;
                    dbForm.EndTimePM = form.EndTimePM;
                    dbForm.AttPhotoFlag = form.AttPhotoFlag;
                    dbForm.iswatermark = form.iswatermark;
                    dbForm.isresolution = form.isresolution;
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
                    db.wq_attendSet.Remove(r => r.id == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newid()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("wq_attendSet", null));
        }

    }
}
