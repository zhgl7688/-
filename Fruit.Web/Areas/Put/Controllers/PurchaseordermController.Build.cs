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

namespace Fruit.Web.Areas.Put.Controllers
{
    public partial class PurchaseordermController : Controller
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
            PersonInfo form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.PersonInfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new PersonInfo
                {
                    ID = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class PurchaseordermApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class PersonInfoListModel {
            public string ID { get; set; }
            public string name { get; set; }
            public string Password { get; set; }
            public string CertNumber { get; set; }
            public string nativeplace { get; set; }
            public string FileNumber { get; set; }
            public string UserAccount { get; set; }
            public string psncode { get; set; }
            public string RoleCode { get; set; }
            public string WageCard { get; set; }
            public string SocialSecurity { get; set; }
            public string Mobile { get; set; }
            public string usedname { get; set; }
            public string sex { get; set; }
            public string birthdate { get; set; }
            public int? age { get; set; }
            public DateTime? stuffdate { get; set; }
            public DateTime? effectdate { get; set; }
            public string workaddr { get; set; }
            public string BirthPlace { get; set; }
            public string nationality { get; set; }
            public string position { get; set; }
            public string permanreside { get; set; }
            public string characterrpr { get; set; }
            public string marital { get; set; }
            public string EmployeeStatus { get; set; }
            public string country { get; set; }
            public string idtype { get; set; }
            public string polity { get; set; }
            public string health { get; set; }
            public string Height { get; set; }
            public string Weight { get; set; }
            public string Vision { get; set; }
            public string lasteducation { get; set; }
            public string HighestDegree { get; set; }
            public string major { get; set; }
            public string rank { get; set; }
            public string GraduationSchool { get; set; }
            public string joinworkdate { get; set; }
            public string FreeWorkYears { get; set; }
            public string is_status { get; set; }
            public string kq_status { get; set; }
            public string glbdef1 { get; set; }
            public string Photo { get; set; }
            public string CompCode { get; set; }
            public string soft { get; set; }
            public string phonetype { get; set; }
            public string os { get; set; }
            public string isline { get; set; }
            public string KQ { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<PersonInfoListModel>(db.Database, "a.ID ,a.name ,a.Password ,a.CertNumber ,a.nativeplace ,a.FileNumber ,a.UserAccount ,a.psncode ,a.RoleCode ,a.WageCard ,a.SocialSecurity ,a.Mobile ,a.usedname ,a.sex ,a.birthdate ,a.age ,a.stuffdate ,a.effectdate ,a.workaddr ,a.BirthPlace ,a.nationality ,a.position ,a.permanreside ,a.characterrpr ,a.marital ,a.EmployeeStatus ,a.country ,a.idtype ,a.polity ,a.health ,a.Height ,a.Weight ,a.Vision ,a.lasteducation ,a.HighestDegree ,a.major ,a.rank ,a.GraduationSchool ,a.joinworkdate ,a.FreeWorkYears ,a.is_status ,a.kq_status ,a.glbdef1 ,a.Photo ,a.CompCode ,a.soft ,a.phonetype ,a.os ,a.isline ,a.KQ ", "PersonInfo a ", sbCondition.ToString(), "ID", "desc");
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_attendTrackInPersonInfoModel {
            public string UserCode { get; set; }
            public string CompCode { get; set; }
            public string id { get; set; }
            public string TckDateTime { get; set; }
            public decimal? TckLong { get; set; }
            public decimal? TckLat { get; set; }
            public string Address { get; set; }
        }
        public object GetDetial(string id)
        {
            using (var db = new LUOLAI1401Context())
            {
                var pageReq = new PageRequest();
                pageReq.Page = null; // 子表读取时不分页
                var wq_attendTrack = pageReq.ToPageList<wq_attendTrackInPersonInfoModel>(db.Database, "a.UserCode ,a.CompCode ,a.id ,a.TckDateTime ,a.TckLong ,a.TckLat ,a.Address ", "wq_attendTrack a ", "a.UserCode = '" + id + "'", "a.id", "desc").Rows;
                return new {wq_attendTrack};
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<PersonInfo>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.PersonInfo.Find(form.ID);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.PersonInfo.Add(form);
                }
                else
                {
                    dbForm.name = form.name;
                    dbForm.Password = form.Password;
                    dbForm.CertNumber = form.CertNumber;
                    dbForm.nativeplace = form.nativeplace;
                    dbForm.FileNumber = form.FileNumber;
                    dbForm.UserAccount = form.UserAccount;
                    dbForm.psncode = form.psncode;
                    dbForm.RoleCode = form.RoleCode;
                    dbForm.WageCard = form.WageCard;
                    dbForm.SocialSecurity = form.SocialSecurity;
                    dbForm.Mobile = form.Mobile;
                    dbForm.usedname = form.usedname;
                    dbForm.sex = form.sex;
                    dbForm.birthdate = form.birthdate;
                    dbForm.age = form.age;
                    dbForm.stuffdate = form.stuffdate;
                    dbForm.effectdate = form.effectdate;
                    dbForm.workaddr = form.workaddr;
                    dbForm.BirthPlace = form.BirthPlace;
                    dbForm.nationality = form.nationality;
                    dbForm.position = form.position;
                    dbForm.permanreside = form.permanreside;
                    dbForm.characterrpr = form.characterrpr;
                    dbForm.marital = form.marital;
                    dbForm.EmployeeStatus = form.EmployeeStatus;
                    dbForm.country = form.country;
                    dbForm.idtype = form.idtype;
                    dbForm.polity = form.polity;
                    dbForm.health = form.health;
                    dbForm.Height = form.Height;
                    dbForm.Weight = form.Weight;
                    dbForm.Vision = form.Vision;
                    dbForm.lasteducation = form.lasteducation;
                    dbForm.HighestDegree = form.HighestDegree;
                    dbForm.major = form.major;
                    dbForm.rank = form.rank;
                    dbForm.GraduationSchool = form.GraduationSchool;
                    dbForm.joinworkdate = form.joinworkdate;
                    dbForm.FreeWorkYears = form.FreeWorkYears;
                    dbForm.is_status = form.is_status;
                    dbForm.kq_status = form.kq_status;
                    dbForm.glbdef1 = form.glbdef1;
                    dbForm.Photo = form.Photo;
                    dbForm.CompCode = form.CompCode;
                    dbForm.soft = form.soft;
                    dbForm.phonetype = form.phonetype;
                    dbForm.os = form.os;
                    dbForm.isline = form.isline;
                    dbForm.KQ = form.KQ;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                    dbForm.UpdatePerson = form.UpdatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                }
                if( post["wq_attendTrack"] != null )
                {
                    var sub= post["wq_attendTrack"] as JArray;
                    if( sub.Count > 0)
                    {
                        for(int i = 0; i < sub.Count; i++ )
                        {
                            JObject obj = sub[i] as JObject;
                            RowState rowState = (RowState)(int)obj["_row_state"];
                            var model = obj.ToObject<wq_attendTrack>();
                            switch (rowState){
                                case RowState.Changed:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                                    break;
                                case RowState.New:
                                    model.UserCode = form.psncode;
                                    model.CompCode = form.CompCode;
                                    model.id = ""+new SysSerialServices().GetNewSerial("wq_attendTrack." + form.ID, null);
                                    db.wq_attendTrack.Add(model);
                                    break;
                                case RowState.Deleted:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                                    break;
                            }
                        }
                    }
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
                    db.PersonInfo.Remove(r => r.ID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewID()
        {
            return new SysSerialServices().GetNewSerial("PersonInfo");
        }

    }
}
