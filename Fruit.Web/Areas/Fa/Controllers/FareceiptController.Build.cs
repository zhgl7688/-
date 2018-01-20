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

namespace Fruit.Web.Areas.Fa.Controllers
{
    public partial class FareceiptController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> Corp, SZType, payType;
                using(var db = new SysContext())
                {
                    Corp = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='HtCorp'")).ToList();
                    SZType = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='SHZHType'")).ToList();
                    payType = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='PayKind'")).ToList();
                }
                return View(new {dataSource = new {Corp,SZType,payType}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class FA_ReceiptsViewable : FA_Receipts {
            public string CustID_RefText { get; set; }
        }
        public ActionResult Edit(string id)
        {
            FA_Receipts form = null;
            List<ComboItem> Corp = null, SZType = null, payType = null;
            using(var db = new SysContext())
            {
                Corp = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='HtCorp'")).ToList();
                SZType = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='SHZHType'")).ToList();
                payType = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='PayKind'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                form = db.Database.SqlQuery<FA_ReceiptsViewable>("select a.FID ,a.Corp ,a.SZType ,b.Contact CustID_RefText ,a.CustID ,a.Fdate ,a.Amt ,a.payType ,a.Remark ,a.CreatePerson ,a.CreateDate ,a.UpdateDate  from FA_Receipts a LEFT JOIN BD_Customers b ON a.CustID = b.FID  where a.FID = @key", new System.Data.SqlClient.SqlParameter("@key", id)).FirstOrDefault();
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new FA_Receipts
                {
                    FID = id
                };
            }
            return View(new { form = form, dataSource = new { Corp,SZType,payType }});
        }

    }
    public partial class FareceiptApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class FA_ReceiptsListModel {
            public string FID { get; set; }
            public string Corp { get; set; }
            public string Corp_RefText { get; set; }
            public string SZType { get; set; }
            public string SZType_RefText { get; set; }
            public string CustID { get; set; }
            public string CustID_RefText { get; set; }
            public DateTime Fdate { get; set; }
            public decimal Amt { get; set; }
            public string payType { get; set; }
            public string payType_RefText { get; set; }
            public string Remark { get; set; }
            public string CreatePerson { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime UpdateDate { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.Dropdown(sbCondition, "Corp", "a.Corp", "");
            SerachCondition.Dropdown(sbCondition, "SZType", "a.SZType", "");
            SerachCondition.PopupSelect(sbCondition, "CustID", "a.CustID", "");
            SerachCondition.Date(sbCondition, "Fdate", "a.Fdate", "");
            SerachCondition.Dropdown(sbCondition, "payType", "a.payType", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<FA_ReceiptsListModel>(db.Database, "a.FID ,b.Text Corp_RefText ,a.Corp ,c.Text SZType_RefText ,a.SZType ,d.Contact CustID_RefText ,a.CustID ,a.Fdate ,a.Amt ,e.Text payType_RefText ,a.payType ,a.Remark ,a.CreatePerson ,a.CreateDate ,a.UpdateDate ", "FA_Receipts a LEFT JOIN [Bcp.Sysy].dbo.sys_code b ON a.Corp = b.Value AND (b.CodeType='HtCorp') LEFT JOIN [Bcp.Sysy].dbo.sys_code c ON a.SZType = c.Value AND (c.CodeType='SHZHType') LEFT JOIN BD_Customers d ON a.CustID = d.FID LEFT JOIN [Bcp.Sysy].dbo.sys_code e ON a.payType = e.Value AND (e.CodeType='PayKind') ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<FA_Receipts>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.FA_Receipts.Find(form.FID);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.FA_Receipts.Add(form);
                }
                else
                {
                    dbForm.Corp = form.Corp;
                    dbForm.SZType = form.SZType;
                    dbForm.CustID = form.CustID;
                    dbForm.Fdate = form.Fdate;
                    dbForm.Amt = form.Amt;
                    dbForm.payType = form.payType;
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
                    db.FA_Receipts.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return string.Format("{0:D12}", new SysSerialServices().GetNewSerial("FA_Receipts", null));
        }

    }
}
