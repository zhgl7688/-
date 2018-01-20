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

namespace Fruit.Web.Areas.Wage.Controllers
{
    public partial class SocialsercuritysetController : Controller
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
            HR_SocialSecuritySets form = null;
            List<ComboItem> FType = null;
            using(var db = new SysContext())
            {
                FType = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='FType'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                form = db.HR_SocialSecuritySets.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new HR_SocialSecuritySets
                {
                    FID = id
                };
            }
            return View(new { form = form, dataSource = new { FType }});
        }

    }
    public partial class SocialsercuritysetApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class HR_SocialSecuritySetsListModel {
            public string FID { get; set; }
            public decimal pIndvPension { get; set; }
            public decimal pIndvMedical { get; set; }
            public decimal pIndvUnemploy { get; set; }
            public decimal pCorpPension { get; set; }
            public decimal pCorpBaseMedical { get; set; }
            public decimal pCorpLocalMedical { get; set; }
            public decimal pCorpInjury { get; set; }
            public decimal pCorpBirth { get; set; }
            public decimal pCorpUnemploy { get; set; }
            public string FType { get; set; }
            public string FType_RefText { get; set; }
            public string Remark { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<HR_SocialSecuritySetsListModel>(db.Database, "a.FID ,a.pIndvPension ,a.pIndvMedical ,a.pIndvUnemploy ,a.pCorpPension ,a.pCorpBaseMedical ,a.pCorpLocalMedical ,a.pCorpInjury ,a.pCorpBirth ,a.pCorpUnemploy ,b.Text FType_RefText ,a.FType ,a.Remark ", "HR_SocialSecuritySets a LEFT JOIN [Bcp.Sysy].dbo.sys_code b ON a.FType = b.Value AND (b.CodeType='FType') ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<HR_SocialSecuritySets>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.HR_SocialSecuritySets.Find(form.FID);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.HR_SocialSecuritySets.Add(form);
                }
                else
                {
                    dbForm.pIndvPension = form.pIndvPension;
                    dbForm.pIndvMedical = form.pIndvMedical;
                    dbForm.pIndvUnemploy = form.pIndvUnemploy;
                    dbForm.pCorpPension = form.pCorpPension;
                    dbForm.pCorpBaseMedical = form.pCorpBaseMedical;
                    dbForm.pCorpLocalMedical = form.pCorpLocalMedical;
                    dbForm.pCorpInjury = form.pCorpInjury;
                    dbForm.pCorpBirth = form.pCorpBirth;
                    dbForm.pCorpUnemploy = form.pCorpUnemploy;
                    dbForm.FType = form.FType;
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
            var _ids = new List<string>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = _id;
                    db.HR_SocialSecuritySets.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return new SysSerialServices().GetNewSerial("HR_SocialSecuritySets");
        }

    }
}
