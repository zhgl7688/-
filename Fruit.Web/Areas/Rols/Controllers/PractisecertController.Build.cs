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
    public partial class PractisecertController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> certType, bUsed, CustID;
                using(var db = new SysContext())
                {
                    certType = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='zhengshutype'")).ToList();
                    bUsed = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='yw'")).ToList();
                    CustID = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='yw'")).ToList();
                }
                return View(new {dataSource = new {certType,bUsed,CustID}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(int id)
        {
            HR_PractiseCerts form = null;
            List<ComboItem> Corp = null, certType = null, bUsed = null, CustID = null;
            using(var db = new SysContext())
            {
                Corp = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='HtCorp'")).ToList();
                certType = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='zhengshutype'")).ToList();
                bUsed = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='yw'")).ToList();
                CustID = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='yw'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                form = db.HR_PractiseCerts.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new HR_PractiseCerts
                {
                    FID = id
                };
            }
            return View(new { form = form, dataSource = new { Corp,certType,bUsed,CustID }});
        }

    }
    public partial class PractisecertApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class HR_PractiseCertsListModel {
            public int FID { get; set; }
            public string certName { get; set; }
            public string Corp { get; set; }
            public string Corp_RefText { get; set; }
            public string empID { get; set; }
            public string certType { get; set; }
            public string certType_RefText { get; set; }
            public string CodeID { get; set; }
            public string certOrgan { get; set; }
            public string certNo { get; set; }
            public string regNo { get; set; }
            public DateTime expireDate { get; set; }
            public string onProject { get; set; }
            public string START_TIME { get; set; }
            public string END_TIME { get; set; }
            public decimal? GAmt { get; set; }
            public string bUsed { get; set; }
            public string bUsed_RefText { get; set; }
            public string trainStatus { get; set; }
            public string CustID { get; set; }
            public string CustID_RefText { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.TextBox(sbCondition, "empID", "a.empID", "");
            SerachCondition.Dropdown(sbCondition, "certType", "a.certType", "");
            SerachCondition.TextBox(sbCondition, "certOrgan", "a.certOrgan", "");
            SerachCondition.TextBox(sbCondition, "certNo", "a.certNo", "");
            SerachCondition.TextBox(sbCondition, "regNo", "a.regNo", "");
            SerachCondition.Date(sbCondition, "expireDate", "a.expireDate", "");
            SerachCondition.Dropdown(sbCondition, "bUsed", "a.bUsed", "");
            SerachCondition.Dropdown(sbCondition, "CustID", "a.CustID", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<HR_PractiseCertsListModel>(db.Database, "a.FID ,a.certName ,b.Text Corp_RefText ,a.Corp ,a.empID ,c.Text certType_RefText ,a.certType ,a.CodeID ,a.certOrgan ,a.certNo ,a.regNo ,a.expireDate ,a.onProject ,a.START_TIME ,a.END_TIME ,a.GAmt ,d.Text bUsed_RefText ,a.bUsed ,a.trainStatus ,e.Text CustID_RefText ,a.CustID ", "HR_PractiseCerts a LEFT JOIN [Bcp.Sysy].dbo.sys_code b ON a.Corp = b.Value AND (b.CodeType='HtCorp') LEFT JOIN [Bcp.Sysy].dbo.sys_code c ON a.certType = c.Value AND (c.CodeType='zhengshutype') LEFT JOIN [Bcp.Sysy].dbo.sys_code d ON a.bUsed = d.Value AND (d.CodeType='yw') LEFT JOIN [Bcp.Sysy].dbo.sys_code e ON a.CustID = e.Value AND (e.CodeType='yw') ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<HR_PractiseCerts>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.HR_PractiseCerts.Find(form.FID);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.HR_PractiseCerts.Add(form);
                }
                else
                {
                    dbForm.certName = form.certName;
                    dbForm.Corp = form.Corp;
                    dbForm.empID = form.empID;
                    dbForm.certType = form.certType;
                    dbForm.CodeID = form.CodeID;
                    dbForm.certOrgan = form.certOrgan;
                    dbForm.certNo = form.certNo;
                    dbForm.regNo = form.regNo;
                    dbForm.expireDate = form.expireDate;
                    dbForm.onProject = form.onProject;
                    dbForm.START_TIME = form.START_TIME;
                    dbForm.END_TIME = form.END_TIME;
                    dbForm.GAmt = form.GAmt;
                    dbForm.bUsed = form.bUsed;
                    dbForm.trainStatus = form.trainStatus;
                    dbForm.CustID = form.CustID;
                    dbForm.Scan1 = form.Scan1;
                    dbForm.Scan2 = form.Scan2;
                    dbForm.Scan3 = form.Scan3;
                    dbForm.Scan4 = form.Scan4;
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
                    db.HR_PractiseCerts.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("HR_PractiseCerts", null));
        }

    }
}
