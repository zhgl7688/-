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
    public partial class CertloanController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> TRemark, DRemark;
                using(var db = new SysContext())
                {
                    TRemark = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='Tui'")).ToList();
                    DRemark = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='yes/no'")).ToList();
                }
                return View(new {dataSource = new {TRemark,DRemark}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(long id)
        {
            HR_CertLoans form = null;
            List<ComboItem> TRemark = null, DRemark = null;
            using(var db = new SysContext())
            {
                TRemark = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='Tui'")).ToList();
                DRemark = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='yes/no'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                form = db.HR_CertLoans.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new HR_CertLoans
                {
                    FID = id
                };
            }
            return View(new { form = form, dataSource = new { TRemark,DRemark }});
        }

    }
    public partial class CertloanApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class HR_CertLoansListModel {
            public long FID { get; set; }
            public string CID { get; set; }
            public string Department { get; set; }
            public string GAddress { get; set; }
            public string ProjName { get; set; }
            public decimal? Amt { get; set; }
            public DateTime loanDate { get; set; }
            public DateTime? EndTime { get; set; }
            public decimal? KAmt { get; set; }
            public string TRemark { get; set; }
            public string TRemark_RefText { get; set; }
            public string DRemark { get; set; }
            public string DRemark_RefText { get; set; }
            public string Scan { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.TextBox(sbCondition, "CID", "a.CID", "");
            SerachCondition.TextBox(sbCondition, "Department", "a.Department", "");
            SerachCondition.Dropdown(sbCondition, "TRemark", "a.TRemark", "");
            SerachCondition.Dropdown(sbCondition, "DRemark", "a.DRemark", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<HR_CertLoansListModel>(db.Database, "a.FID ,a.CID ,a.Department ,a.GAddress ,a.ProjName ,a.Amt ,a.loanDate ,a.EndTime ,a.KAmt ,b.Text TRemark_RefText ,a.TRemark ,c.Text DRemark_RefText ,a.DRemark ,a.Scan ", "HR_CertLoans a LEFT JOIN [Bcp.Sysy].dbo.sys_code b ON a.TRemark = b.Value AND (b.CodeType='Tui') LEFT JOIN [Bcp.Sysy].dbo.sys_code c ON a.DRemark = c.Value AND (c.CodeType='yes/no') ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<HR_CertLoans>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.HR_CertLoans.Find(form.FID);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.HR_CertLoans.Add(form);
                }
                else
                {
                    dbForm.CID = form.CID;
                    dbForm.Department = form.Department;
                    dbForm.GAddress = form.GAddress;
                    dbForm.ProjName = form.ProjName;
                    dbForm.Amt = form.Amt;
                    dbForm.loanDate = form.loanDate;
                    dbForm.EndTime = form.EndTime;
                    dbForm.KAmt = form.KAmt;
                    dbForm.TRemark = form.TRemark;
                    dbForm.DRemark = form.DRemark;
                    dbForm.Scan = form.Scan;
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
                    db.HR_CertLoans.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("HR_CertLoans", null));
        }

    }
}
