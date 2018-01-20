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
    public partial class WagesetController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> HFType;
                using(var db = new SysContext())
                {
                    HFType = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='FType'")).ToList();
                }
                return View(new {dataSource = new {HFType}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class HR_WageSetsViewable : HR_WageSets {
            public string empID_RefText { get; set; }
        }
        public ActionResult Edit(string id)
        {
            HR_WageSets form = null;
            List<ComboItem> HFType = null, CustID = null;
            using(var db = new SysContext())
            {
                HFType = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='FType'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                CustID = db.BD_Customers.Select(i=>new ComboItem{ Text = i.Contact, Value = "" + i.FID }).ToList();
                form = db.Database.SqlQuery<HR_WageSetsViewable>("select a.FID ,b.empID empID_RefText ,a.empID ,a.HFType ,a.SSBaseAmt ,a.HFBaseAmt ,a.CustID ,a.START_TIME ,a.END_TIME ,a.Remark ,a.CreatePerson ,a.CreateDate ,a.UpdateDate  from HR_WageSets a LEFT JOIN HR_PractiseCerts b ON a.empID = b.FID  where a.FID = @key", new System.Data.SqlClient.SqlParameter("@key", id)).FirstOrDefault();
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new HR_WageSets
                {
                    FID = id
                };
            }
            return View(new { form = form, dataSource = new { HFType,CustID }});
        }

    }
    public partial class WagesetApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class HR_WageSetsListModel {
            public string FID { get; set; }
            public int? empID { get; set; }
            public string empID_RefText { get; set; }
            public string HFType { get; set; }
            public string HFType_RefText { get; set; }
            public decimal SSBaseAmt { get; set; }
            public decimal HFBaseAmt { get; set; }
            public string CustID { get; set; }
            public string CustID_RefText { get; set; }
            public string START_TIME { get; set; }
            public string END_TIME { get; set; }
            public string Remark { get; set; }
            public string CreatePerson { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime UpdateDate { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.PopupSelect(sbCondition, "empID", "a.empID", "");
            SerachCondition.Dropdown(sbCondition, "HFType", "a.HFType", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<HR_WageSetsListModel>(db.Database, "a.FID ,b.empID empID_RefText ,a.empID ,c.Text HFType_RefText ,a.HFType ,a.SSBaseAmt ,a.HFBaseAmt ,d.Contact CustID_RefText ,a.CustID ,a.START_TIME ,a.END_TIME ,a.Remark ,a.CreatePerson ,a.CreateDate ,a.UpdateDate ", "HR_WageSets a LEFT JOIN HR_PractiseCerts b ON a.empID = b.FID LEFT JOIN [Bcp.Sysy].dbo.sys_code c ON a.HFType = c.Value AND (c.CodeType='FType') LEFT JOIN BD_Customers d ON a.CustID = d.FID ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<HR_WageSets>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.HR_WageSets.Find(form.FID);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.HR_WageSets.Add(form);
                }
                else
                {
                    dbForm.empID = form.empID;
                    dbForm.HFType = form.HFType;
                    dbForm.SSBaseAmt = form.SSBaseAmt;
                    dbForm.HFBaseAmt = form.HFBaseAmt;
                    dbForm.CustID = form.CustID;
                    dbForm.START_TIME = form.START_TIME;
                    dbForm.END_TIME = form.END_TIME;
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
                    db.HR_WageSets.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return string.Format("{0:D12}", new SysSerialServices().GetNewSerial("HR_WageSets", null));
        }

    }
}
