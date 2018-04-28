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

namespace Fruit.Web.Areas.Business.Controllers
{
    public partial class ObjectionController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> isShow;
                using(var db = new SysContext())
                {
                    isShow = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
                }
                return View(new {dataSource = new {isShow}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(int id)
        {
            fw_Objection form = null;
            List<ComboItem> Objectionstatus = null, otherstatus = null, IsContinue = null, objState = null, isShow = null, IsAgreed = null, IsOnJudge = null;
            using(var db = new SysContext())
            {
                Objectionstatus = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='HandleState'")).ToList();
                otherstatus = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='OtherState'")).ToList();
                IsContinue = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
                objState = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
                isShow = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
                IsAgreed = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='JJResult'")).ToList();
                IsOnJudge = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_Objection.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_Objection
                {
                    objectionid = id
                };
            }
            return View(new { form = form, dataSource = new { Objectionstatus,otherstatus,IsContinue,objState,isShow,IsAgreed,IsOnJudge }});
        }

    }
    public partial class ObjectionApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_ObjectionListModel {
            public int objectionid { get; set; }
            public string orderid { get; set; }
            public decimal? orderprice { get; set; }
            public string Objectionstatus { get; set; }
            public string Objectionstatus_RefText { get; set; }
            public string otherstatus { get; set; }
            public string otherstatus_RefText { get; set; }
            public decimal? otherprice { get; set; }
            public int? IsContinue { get; set; }
            public string IsContinue_RefText { get; set; }
            public string MemId { get; set; }
            public string MemIdSeller { get; set; }
            public int? objState { get; set; }
            public string objState_RefText { get; set; }
            public string isShow { get; set; }
            public string isShow_RefText { get; set; }
            public int? Cimg1 { get; set; }
            public int? Cimg2 { get; set; }
            public int? Cimg3 { get; set; }
            public string IsAgreed { get; set; }
            public string IsAgreed_RefText { get; set; }
            public string IsOnJudge { get; set; }
            public string IsOnJudge_RefText { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.TextBox(sbCondition, "orderid", "a.orderid", "");
            SerachCondition.TextBox(sbCondition, "MemId", "a.MemId", "");
            SerachCondition.Dropdown(sbCondition, "isShow", "a.isShow", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_ObjectionListModel>(db.Database, "a.objectionid ,a.orderid ,a.orderprice ,b.Text Objectionstatus_RefText ,a.Objectionstatus ,c.Text otherstatus_RefText ,a.otherstatus ,a.otherprice ,d.Text IsContinue_RefText ,a.IsContinue ,a.MemId ,a.MemIdSeller ,e.Text objState_RefText ,a.objState ,f.Text isShow_RefText ,a.isShow ,a.Cimg1 ,a.Cimg2 ,a.Cimg3 ,g.Text IsAgreed_RefText ,a.IsAgreed ,h.Text IsOnJudge_RefText ,a.IsOnJudge ", "fw_Objection a LEFT JOIN [SYS_YLW].dbo.sys_code b ON a.Objectionstatus = b.Value AND (b.CodeType='HandleState') LEFT JOIN [SYS_YLW].dbo.sys_code c ON a.otherstatus = c.Value AND (c.CodeType='OtherState') LEFT JOIN [SYS_YLW].dbo.sys_code d ON a.IsContinue = d.Value AND (d.CodeType='YN') LEFT JOIN [SYS_YLW].dbo.sys_code e ON a.objState = e.Value AND (e.CodeType='YN') LEFT JOIN [SYS_YLW].dbo.sys_code f ON a.isShow = f.Value AND (f.CodeType='YN') LEFT JOIN [SYS_YLW].dbo.sys_code g ON a.IsAgreed = g.Value AND (g.CodeType='JJResult') LEFT JOIN [SYS_YLW].dbo.sys_code h ON a.IsOnJudge = h.Value AND (h.CodeType='YN') ", sbCondition.ToString(), "a.objectionid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_Objection>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_Objection.Find(form.objectionid);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.fw_Objection.Add(form);
                }
                else
                {
                    dbForm.orderid = form.orderid;
                    dbForm.orderprice = form.orderprice;
                    dbForm.Objectionstatus = form.Objectionstatus;
                    dbForm.otherstatus = form.otherstatus;
                    dbForm.otherprice = form.otherprice;
                    dbForm.IsContinue = form.IsContinue;
                    dbForm.MemId = form.MemId;
                    dbForm.MemIdSeller = form.MemIdSeller;
                    dbForm.objState = form.objState;
                    dbForm.isShow = form.isShow;
                    dbForm.Cimg1 = form.Cimg1;
                    dbForm.Cimg2 = form.Cimg2;
                    dbForm.Cimg3 = form.Cimg3;
                    dbForm.IsAgreed = form.IsAgreed;
                    dbForm.explain = form.explain;
                    dbForm.sysExplain = form.sysExplain;
                    dbForm.IsOnJudge = form.IsOnJudge;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                    dbForm.UpdatePerson = form.UpdatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                }
                if (form.isShow == "0")
                {
                    //插入异议不通过短信功能
                    db.fw_messageinfo.Add(
                        new fw_messageinfo
                        {
                            title = "异议申请反馈",
                            messcontent = form.sysExplain,
                            toid = form.MemId,
                            fromid = "sys",
                            isdelete=0,
                            isread=0,
                             CreateDate=DateTime.Now
                        }   );
                }
                // 记录多级零时主键对应(key int 为 js 生成的页内全局唯一编号)
                var _id_maps = new Dictionary<int, object[]>();
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        [System.Web.Http.HttpPost]
        public object DoBShencheng(JObject row)
        {
            object result = null;
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "exec ups_ObjectionResult @id";
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@id";
                p1.Value = (string)row.GetValue("objectionid");
                cmd.Parameters.Add(p1);
                result = cmd.ExecuteScalar();
            }
            return result;
        }
        public object Delete(string id)
        {
            var _ids = new List<int>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = int.Parse(_id);
                    db.fw_Objection.Remove(r => r.objectionid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newobjectionid()
        {
            return new SysSerialServices().GetNewSerial("fw_Objection");
        }

    }
}
