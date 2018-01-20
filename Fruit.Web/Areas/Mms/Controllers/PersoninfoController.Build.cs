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
    public partial class PersoninfoController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> position;
                using(var db = new SysContext())
                {
                    position = db.Database.SqlQuery<ComboItem>("select OrganizeName Text, OrganizeCode Value from sys_organize where " + string.Format("{0}{1}", "CompCode=", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode)).ToList();
                }
                return View(new {dataSource = new {position}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class PersonInfoViewable : PersonInfo {
            public string RoleCode_RefText { get; set; }
        }
        public ActionResult Edit(string id)
        {
            PersonInfo form = null;
            List<ComboItem> sex = null, idtype = null, EmployeeStatus = null, position = null, marital = null, lasteducation = null, HighestDegree = null, rank = null,wq_unittype_remark = null;
            using(var db = new SysContext())
            {
                sex = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "CodeType='Sex'")).ToList();
                idtype = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "CodeType='idtype'")).ToList();
                EmployeeStatus = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "CodeType='forstatus'")).ToList();
                position = db.Database.SqlQuery<ComboItem>("select OrganizeName Text, OrganizeCode Value from sys_organize where " + string.Format("{0}{1}", "CompCode=", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode)).ToList();
                marital = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "CodeType='marital'")).ToList();
                lasteducation = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "CodeType='lasteducation'")).ToList();
                HighestDegree = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "CodeType='HighestDegree'")).ToList();
                rank = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "CodeType='rank'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                form = db.Database.SqlQuery<PersonInfoViewable>("select a.ID ,a.psncode ,a.name ,a.Mobile ,a.sex ,a.idtype ,a.CertNumber ,a.nationality ,a.nativeplace ,a.age ,a.birthdate ,a.BirthPlace ,a.characterrpr ,a.permanreside ,a.FileNumber ,a.SocialSecurity ,a.WageCard ,a.EmployeeStatus ,a.position ,b.RoleName RoleCode_RefText ,a.RoleCode ,a.effectdate ,a.stuffdate ,a.workaddr ,a.marital ,a.country ,a.polity ,a.health ,a.Height ,a.Weight ,a.Vision ,a.GraduationSchool ,a.lasteducation ,a.HighestDegree ,a.major ,a.rank ,a.joinworkdate ,a.FreeWorkYears ,a.is_status ,a.kq_status ,a.glbdef1 ,a.Photo ,a.CreatePerson ,a.CreateDate ,a.UpdatePerson ,a.UpdateDate  from PersonInfo a LEFT JOIN [Bcp.Sys].dbo.sys_role b ON a.RoleCode = b.RoleCode AND " + string.Format("{0}{1}{2}", "CompCode='", System.Web.HttpContext.Current.Session["CompCode"], "'") + " where a.ID = @key", new System.Data.SqlClient.SqlParameter("@key", id)).FirstOrDefault();
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new PersonInfo
                {
                    ID = id,
                    kq_status = string.Format("{0}", "1"),
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };
            }
            return View(new { form = form, dataSource = new { sex,idtype,EmployeeStatus,position,marital,lasteducation,HighestDegree,rank,wq_unittype_remark }});
        }

    }
    public partial class PersoninfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class PersonInfoListModel {
            public string ID { get; set; }
            public string psncode { get; set; }
            public string name { get; set; }
            public string Mobile { get; set; }
            public string sex { get; set; }
            public string sex_RefText { get; set; }
            public string EmployeeStatus { get; set; }
            public string EmployeeStatus_RefText { get; set; }
            public string position { get; set; }
            public string position_RefText { get; set; }
            public string RoleCode { get; set; }
            public string RoleCode_RefText { get; set; }
            public DateTime stuffdate { get; set; }
            public string is_status { get; set; }
            public string kq_status { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.TextBox(sbCondition, "psncode", "a.psncode", "");
            SerachCondition.TextBox(sbCondition, "name", "a.name", "");
            SerachCondition.TextBox(sbCondition, "Mobile", "a.Mobile", "");
            SerachCondition.Dropdown(sbCondition, "position", "a.position", "");
            SerachCondition.PopupSelect(sbCondition, "RoleCode", "a.RoleCode", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<PersonInfoListModel>(db.Database, "a.ID ,a.psncode ,a.name ,a.Mobile ,b.Text sex_RefText ,a.sex ,c.Text EmployeeStatus_RefText ,a.EmployeeStatus ,d.OrganizeName position_RefText ,a.position ,e.RoleName RoleCode_RefText ,a.RoleCode ,a.stuffdate ,a.is_status ,a.kq_status ", "PersonInfo a LEFT JOIN [Bcp.Sys].dbo.sys_code b ON a.sex = b.Value LEFT JOIN [Bcp.Sys].dbo.sys_code c ON a.EmployeeStatus = c.Value LEFT JOIN [Bcp.Sys].dbo.sys_organize d ON a.position = d.OrganizeCode LEFT JOIN [Bcp.Sys].dbo.sys_role e ON a.RoleCode = e.RoleCode ", sbCondition.ToString(), "a.ID", "desc");
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_unittypeInPersonInfoModel {
            public string usercode { get; set; }
            public string compcode { get; set; }
            public int id { get; set; }
            public string sbbh { get; set; }
            public string sbxh { get; set; }
            public string bbmc { get; set; }
            public string remark { get; set; }
        }
        public object GetDetial(string id)
        {
            using (var db = new LUOLAI1401Context())
            {
                var pageReq = new PageRequest();
                pageReq.Page = null; // 子表读取时不分页
                var wq_unittype = pageReq.ToPageList<wq_unittypeInPersonInfoModel>(db.Database, "a.usercode ,a.compcode ,a.id ,a.sbbh ,a.sbxh ,a.bbmc ,a.remark ", "wq_unittype a INNER JOIN PersonInfo z ON z.psncode = a.usercode AND z.CompCode = a.compcode ", "z.ID = '" + id + "'", "a.id", "desc").Rows;
                return new {wq_unittype};
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<PersonInfo>(JsonExtension.FixJsonSerializer);
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
                    dbForm.psncode = form.psncode;
                    dbForm.name = form.name;
                    dbForm.Mobile = form.Mobile;
                    dbForm.sex = form.sex;
                    dbForm.idtype = form.idtype;
                    dbForm.CertNumber = form.CertNumber;
                    dbForm.nationality = form.nationality;
                    dbForm.nativeplace = form.nativeplace;
                    dbForm.age = form.age;
                    dbForm.birthdate = form.birthdate;
                    dbForm.BirthPlace = form.BirthPlace;
                    dbForm.FileNumber = form.FileNumber;
                    dbForm.SocialSecurity = form.SocialSecurity;
                    dbForm.WageCard = form.WageCard;
                    dbForm.EmployeeStatus = form.EmployeeStatus;
                    dbForm.position = form.position;
                    dbForm.RoleCode = form.RoleCode;
                    dbForm.effectdate = form.effectdate;
                    dbForm.stuffdate = form.stuffdate;
                    dbForm.marital = form.marital;
                    dbForm.health = form.health;
                    dbForm.Height = form.Height;
                    dbForm.Weight = form.Weight;
                    dbForm.GraduationSchool = form.GraduationSchool;
                    dbForm.lasteducation = form.lasteducation;
                    dbForm.HighestDegree = form.HighestDegree;
                    dbForm.major = form.major;
                    dbForm.rank = form.rank;
                    dbForm.is_status = form.is_status;
                    dbForm.kq_status = form.kq_status;
                    dbForm.glbdef1 = form.glbdef1;
                    dbForm.Photo = form.Photo;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                    dbForm.UpdatePerson = form.UpdatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                }
                if( post["wq_unittype"] != null )
                {
                    var sub= post["wq_unittype"] as JArray;
                    if( sub.Count > 0)
                    {
                        for(int i = 0; i < sub.Count; i++ )
                        {
                            JObject obj = sub[i] as JObject;
                            RowState rowState = (RowState)(int)obj["_row_state"];
                            var model = obj.ToObject<wq_unittype>(JsonExtension.FixJsonSerializer);
                            switch (rowState){
                                case RowState.Changed:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                                    break;
                                case RowState.New:
                                    model.usercode = form.psncode;
                                    model.compcode = form.CompCode;
                                    model.id = new SysSerialServices().GetNewSerial("wq_unittype." + form.ID, null);
                                    db.wq_unittype.Add(model);
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
                    try
                    {
                        db.Database.ExecuteSqlCommand(string.Format("{0}{1}", "raiserror '不能删除人员！' + ", did));
                    }
                    catch (System.Data.Common.DbException err)
                    {
                        throw err;
                    }
                    db.PersonInfo.Remove(r => r.ID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewID()
        {
            return string.Format("{0:D12}", new SysSerialServices().GetNewSerial("PersonInfo", null));
        }

    }
}
