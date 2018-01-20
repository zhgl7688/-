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

namespace Fruit.Web.Areas.Project.Controllers
{
    public partial class ProjectinfoController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> projl;
                using(var db = new LUOLAI1401Context())
                {
                    projl = db.HR_PractiseCerts.Select(i=>new ComboItem{ Text = i.empID, Value = "" +  i.FID }).ToList();
                }
                return View(new {dataSource = new {projl}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class PM_ProjectInfosViewable : PM_ProjectInfos {
            public string CustID_RefText { get; set; }
        }
        public ActionResult Edit(int id)
        {
            PM_ProjectInfos form = null;
            List<ComboItem> bClose = null, safetyPerson = null, qualityPerson = null, buildPerson = null, dataPerson = null, materialPerson = null, projl = null, samplePerson = null;
            using(var db = new SysContext())
            {
                bClose = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='yes/no'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                safetyPerson = db.HR_PractiseCerts.Select(i=>new ComboItem{ Text = i.empID, Value = "" + i.FID }).ToList();
                qualityPerson = db.HR_PractiseCerts.Select(i=>new ComboItem{ Text = i.empID, Value = "" + i.FID }).ToList();
                buildPerson = db.HR_PractiseCerts.Select(i=>new ComboItem{ Text = i.empID, Value = "" + i.FID }).ToList();
                dataPerson = db.HR_PractiseCerts.Select(i=>new ComboItem{ Text = i.empID, Value = "" + i.FID }).ToList();
                materialPerson = db.HR_PractiseCerts.Select(i=>new ComboItem{ Text = i.empID, Value = "" + i.FID }).ToList();
                projl = db.HR_PractiseCerts.Select(i=>new ComboItem{ Text = i.empID, Value = "" + i.FID }).ToList();
                samplePerson = db.HR_PractiseCerts.Select(i=>new ComboItem{ Text = i.empID, Value = "" + i.FID }).ToList();
                form = db.Database.SqlQuery<PM_ProjectInfosViewable>("select a.FID ,a.appNumber ,a.Code ,a.ProjName ,b.Contact CustID_RefText ,a.CustID ,a.Owner ,a.BeginDate ,a.EndDate ,a.BaDate ,a.bClose ,a.projAddress ,a.exDays ,a.Remark ,a.CreatePerson ,a.CreateDate ,a.UpdateDate ,a.proje ,a.safetyPerson ,a.qualityPerson ,a.buildPerson ,a.dataPerson ,a.materialPerson ,a.projl ,a.samplePerson  from PM_ProjectInfos a LEFT JOIN BD_Customers b ON a.CustID = b.FID  where a.FID = @key", new System.Data.SqlClient.SqlParameter("@key", id)).FirstOrDefault();
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new PM_ProjectInfos
                {
                    FID = id,
                    bClose = string.Format("{0}", "n")
                };
            }
            return View(new { form = form, dataSource = new { bClose,safetyPerson,qualityPerson,buildPerson,dataPerson,materialPerson,projl,samplePerson }});
        }

    }
    public partial class ProjectinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class PM_ProjectInfosListModel {
            public int FID { get; set; }
            public string appNumber { get; set; }
            public string Code { get; set; }
            public string ProjName { get; set; }
            public string CustID { get; set; }
            public string CustID_RefText { get; set; }
            public string Owner { get; set; }
            public string BeginDate { get; set; }
            public string EndDate { get; set; }
            public string BaDate { get; set; }
            public string bClose { get; set; }
            public string bClose_RefText { get; set; }
            public int exDays { get; set; }
            public string proje { get; set; }
            public int? projl { get; set; }
            public string projl_RefText { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.PopupSelect(sbCondition, "CustID", "a.CustID", "");
            SerachCondition.Date(sbCondition, "BeginDate", "a.BeginDate", "");
            SerachCondition.Date(sbCondition, "EndDate", "a.EndDate", "");
            SerachCondition.Dropdown(sbCondition, "projl", "a.projl", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<PM_ProjectInfosListModel>(db.Database, "a.FID ,a.appNumber ,a.Code ,a.ProjName ,b.Contact CustID_RefText ,a.CustID ,a.Owner ,a.BeginDate ,a.EndDate ,a.BaDate ,c.Text bClose_RefText ,a.bClose ,a.exDays ,a.proje ,d.empID projl_RefText ,a.projl ", "PM_ProjectInfos a LEFT JOIN BD_Customers b ON a.CustID = b.FID LEFT JOIN [Bcp.Sysy].dbo.sys_code c ON a.bClose = c.Value AND (c.CodeType='yes/no') LEFT JOIN HR_PractiseCerts d ON a.projl = d.FID ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<PM_ProjectInfos>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.PM_ProjectInfos.Find(form.FID);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.PM_ProjectInfos.Add(form);
                }
                else
                {
                    dbForm.appNumber = form.appNumber;
                    dbForm.Code = form.Code;
                    dbForm.ProjName = form.ProjName;
                    dbForm.CustID = form.CustID;
                    dbForm.Owner = form.Owner;
                    dbForm.BeginDate = form.BeginDate;
                    dbForm.EndDate = form.EndDate;
                    dbForm.BaDate = form.BaDate;
                    dbForm.bClose = form.bClose;
                    dbForm.projAddress = form.projAddress;
                    dbForm.exDays = form.exDays;
                    dbForm.Remark = form.Remark;
                    dbForm.proje = form.proje;
                    dbForm.safetyPerson = form.safetyPerson;
                    dbForm.qualityPerson = form.qualityPerson;
                    dbForm.buildPerson = form.buildPerson;
                    dbForm.dataPerson = form.dataPerson;
                    dbForm.materialPerson = form.materialPerson;
                    dbForm.projl = form.projl;
                    dbForm.samplePerson = form.samplePerson;
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
            var _ids = new List<int>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = int.Parse(_id);
                    db.PM_ProjectInfos.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("PM_ProjectInfos", null));
        }

    }
}
