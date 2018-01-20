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
    public partial class BidprojectinfoController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> depAmtStatus;
                using(var db = new SysContext())
                {
                    depAmtStatus = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='depAmt'")).ToList();
                }
                return View(new {dataSource = new {depAmtStatus}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class PM_ProjectInfoBidsViewable : PM_ProjectInfoBids {
            public string CustID_RefText { get; set; }
        }
        public ActionResult Edit(string id)
        {
            PM_ProjectInfoBids form = null;
            List<ComboItem> depAmtStatus = null;
            using(var db = new SysContext())
            {
                depAmtStatus = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='depAmt'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                form = db.Database.SqlQuery<PM_ProjectInfoBidsViewable>("select a.FID ,a.Code ,a.ProjName ,b.Contact CustID_RefText ,a.CustID ,a.appNumber ,a.Owner ,a.totalInvAmt ,a.depAmt ,a.depAmtStatus ,a.advAmt ,a.bookAmt ,a.bidAmt ,a.agencyAmt ,a.PManager ,a.bidDate ,a.bidAddress ,a.bidPerson ,a.bidAgent ,a.Attention ,a.Winner ,a.Remark ,a.CreatePerson ,a.CreateDate ,a.UpdateDate ,a.UpdatePerson  from PM_ProjectInfoBids a LEFT JOIN BD_Customers b ON a.CustID = b.FID  where a.Code = @key", new System.Data.SqlClient.SqlParameter("@key", id)).FirstOrDefault();
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new PM_ProjectInfoBids
                {
                    Code = id,
                    depAmtStatus = string.Format("{0}", "5")
                };
            }
            return View(new { form = form, dataSource = new { depAmtStatus }});
        }

    }
    public partial class BidprojectinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class PM_ProjectInfoBidsListModel {
            public string Code { get; set; }
            public string ProjName { get; set; }
            public string CustID { get; set; }
            public string CustID_RefText { get; set; }
            public string appNumber { get; set; }
            public string Owner { get; set; }
            public decimal totalInvAmt { get; set; }
            public decimal depAmt { get; set; }
            public string depAmtStatus { get; set; }
            public string depAmtStatus_RefText { get; set; }
            public decimal advAmt { get; set; }
            public decimal bookAmt { get; set; }
            public decimal bidAmt { get; set; }
            public decimal agencyAmt { get; set; }
            public string PManager { get; set; }
            public string bidDate { get; set; }
            public string bidAddress { get; set; }
            public string bidPerson { get; set; }
            public string bidAgent { get; set; }
            public string Attention { get; set; }
            public string Winner { get; set; }
            public string Remark { get; set; }
            public string CreatePerson { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime UpdateDate { get; set; }
            public string UpdatePerson { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.TextBox(sbCondition, "appNumber", "a.appNumber", "");
            SerachCondition.Dropdown(sbCondition, "depAmtStatus", "a.depAmtStatus", "");
            SerachCondition.TextBox(sbCondition, "PManager", "a.PManager", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<PM_ProjectInfoBidsListModel>(db.Database, "a.Code ,a.ProjName ,b.Contact CustID_RefText ,a.CustID ,a.appNumber ,a.Owner ,a.totalInvAmt ,a.depAmt ,c.Text depAmtStatus_RefText ,a.depAmtStatus ,a.advAmt ,a.bookAmt ,a.bidAmt ,a.agencyAmt ,a.PManager ,a.bidDate ,a.bidAddress ,a.bidPerson ,a.bidAgent ,a.Attention ,a.Winner ,a.Remark ,a.CreatePerson ,a.CreateDate ,a.UpdateDate ,a.UpdatePerson ", "PM_ProjectInfoBids a LEFT JOIN BD_Customers b ON a.CustID = b.FID LEFT JOIN [Bcp.Sysy].dbo.sys_code c ON a.depAmtStatus = c.Value AND (c.CodeType='depAmt') ", sbCondition.ToString(), "a.Code", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<PM_ProjectInfoBids>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.PM_ProjectInfoBids.Find(form.Code);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.PM_ProjectInfoBids.Add(form);
                }
                else
                {
                    dbForm.ProjName = form.ProjName;
                    dbForm.CustID = form.CustID;
                    dbForm.appNumber = form.appNumber;
                    dbForm.Owner = form.Owner;
                    dbForm.totalInvAmt = form.totalInvAmt;
                    dbForm.depAmt = form.depAmt;
                    dbForm.depAmtStatus = form.depAmtStatus;
                    dbForm.advAmt = form.advAmt;
                    dbForm.bookAmt = form.bookAmt;
                    dbForm.bidAmt = form.bidAmt;
                    dbForm.agencyAmt = form.agencyAmt;
                    dbForm.PManager = form.PManager;
                    dbForm.bidDate = form.bidDate;
                    dbForm.bidAddress = form.bidAddress;
                    dbForm.bidPerson = form.bidPerson;
                    dbForm.bidAgent = form.bidAgent;
                    dbForm.Attention = form.Attention;
                    dbForm.Winner = form.Winner;
                    dbForm.Remark = form.Remark;
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
                cmd.CommandText = "exec DepositLog @ID";
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@ID";
                p1.Value = form.Code;
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
                    db.PM_ProjectInfoBids.Remove(r => r.Code == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewCode()
        {
            return string.Format("{0:D12}", new SysSerialServices().GetNewSerial("PM_ProjectInfoBids", null));
        }

    }
}
