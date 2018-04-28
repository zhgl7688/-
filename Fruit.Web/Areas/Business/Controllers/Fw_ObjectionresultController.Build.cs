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
    public partial class Fw_ObjectionresultController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> MemId;
                using(var db = new LUOLAI1401Context())
                {
                    MemId = db.v_MemberAll.Select(i=>new ComboItem{ Text = i.corpname, Value = "" +  i.memid }).ToList();
                }
                return View(new {dataSource = new {MemId}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(int id)
        {
            fw_ObjectionResult form = null;
            List<ComboItem> MemId = null, IsContinue = null, paymemId = null, paymentmode = null, resultstatus = null,v_Orderinfo_ordstate = null,v_Orderinfo_SelfMentioning = null;
            using(var db = new SysContext())
            {
                IsContinue = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
                paymentmode = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='PayKind'")).ToList();
                resultstatus = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='ObjState'")).ToList();
                v_Orderinfo_ordstate = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where /*TABLEALIAS*/CodeType='OrderState'").ToList();
                v_Orderinfo_SelfMentioning = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where /*TABLEALIAS*/CodeType='JHFS'").ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                MemId = db.v_MemberAll.Select(i=>new ComboItem{ Text = i.corpname, Value = "" + i.memid }).ToList();
                paymemId = db.v_MemberAll.Select(i=>new ComboItem{ Text = i.corpname, Value = "" + i.memid }).ToList();
                form = db.fw_ObjectionResult.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_ObjectionResult
                {
                    objectionid = id
                };
            }
            return View(new { form = form, dataSource = new { MemId,IsContinue,paymemId,paymentmode,resultstatus,v_Orderinfo_ordstate,v_Orderinfo_SelfMentioning }});
        }

    }
    public partial class Fw_ObjectionresultApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_ObjectionResultListModel {
            public int objectionid { get; set; }
            public string MemId { get; set; }
            public string MemId_RefText { get; set; }
            public string orderid { get; set; }
            public int? IsContinue { get; set; }
            public string IsContinue_RefText { get; set; }
            public string paymemId { get; set; }
            public string paymemId_RefText { get; set; }
            public decimal? payment { get; set; }
            public string paymentmode { get; set; }
            public string paymentmode_RefText { get; set; }
            public DateTime? paymentendtime { get; set; }
            public string resultstatus { get; set; }
            public string resultstatus_RefText { get; set; }
            public string explain { get; set; }
            public string resultperson { get; set; }
            public string resultphone { get; set; }
            public string finalresult { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.Dropdown(sbCondition, "MemId", "a.MemId", "");
            SerachCondition.TextBox(sbCondition, "orderid", "a.orderid", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_ObjectionResultListModel>(db.Database, "a.objectionid ,b.corpname MemId_RefText ,a.MemId ,a.orderid ,c.Text IsContinue_RefText ,a.IsContinue ,d.corpname paymemId_RefText ,a.paymemId ,a.payment ,e.Text paymentmode_RefText ,a.paymentmode ,a.paymentendtime ,f.Text resultstatus_RefText ,a.resultstatus ,a.explain ,a.resultperson ,a.resultphone ,a.finalresult ", "fw_ObjectionResult a LEFT JOIN v_MemberAll b ON a.MemId = b.memid LEFT JOIN [SYS_YLW].dbo.sys_code c ON a.IsContinue = c.Value AND (c.CodeType='YN') LEFT JOIN v_MemberAll d ON a.paymemId = d.memid LEFT JOIN [SYS_YLW].dbo.sys_code e ON a.paymentmode = e.Value AND (e.CodeType='PayKind') LEFT JOIN [SYS_YLW].dbo.sys_code f ON a.resultstatus = f.Value AND (f.CodeType='ObjState') ", sbCondition.ToString(), "a.objectionid", "desc");
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class v_objectionInfw_ObjectionResultModel {
            public int objectionid { get; set; }
            public string orderid { get; set; }
            public decimal? orderprice { get; set; }
            public string Objectionstatus { get; set; }
            public string otherstatus { get; set; }
            public decimal? otherprice { get; set; }
            public string explain { get; set; }
            public string IsAgreed { get; set; }
            public string IsContinue { get; set; }
            public string IsOnJudge { get; set; }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class v_OrderinfoInfw_ObjectionResultModel {
            public int objectionid { get; set; }
            public string selleruserid { get; set; }
            public string buyeruserid { get; set; }
            public string tranmode { get; set; }
            public int? ordstate { get; set; }
            public string ordstate_RefText { get; set; }
            public decimal? totalamount { get; set; }
            public decimal? buyerDepAmt { get; set; }
            public decimal? sellerDepAmt { get; set; }
            public string SelfMentioning { get; set; }
            public string SelfMentioning_RefText { get; set; }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class v_ObjectionResult1Infw_ObjectionResultModel {
            public int objectionid { get; set; }
            public string memid { get; set; }
            public string phone { get; set; }
            public string realname { get; set; }
            public int? gender { get; set; }
            public string email { get; set; }
            public string qq { get; set; }
            public string wechat { get; set; }
            public DateTime? regtime { get; set; }
            public string corpname { get; set; }
            public string abbname { get; set; }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class v_ObjectionResultInfw_ObjectionResultModel {
            public int objectionid { get; set; }
            public string memid { get; set; }
            public string phone { get; set; }
            public string realname { get; set; }
            public int? gender { get; set; }
            public string email { get; set; }
            public string qq { get; set; }
            public string wechat { get; set; }
            public DateTime? regtime { get; set; }
            public string corpname { get; set; }
            public string abbname { get; set; }
        }
        public object GetDetial(string id)
        {
            using (var db = new LUOLAI1401Context())
            {
                var pageReq = new PageRequest();
                pageReq.Page = null; // 子表读取时不分页
                var v_objection = pageReq.ToPageList<v_objectionInfw_ObjectionResultModel>(db.Database, "a.objectionid ,a.orderid ,a.orderprice ,a.Objectionstatus ,a.otherstatus ,a.otherprice ,a.explain ,a.IsAgreed ,a.IsContinue ,a.IsOnJudge ", "v_objection a ", "a.objectionid = " + id, "a.orderid", "desc").Rows;
                var v_Orderinfo = pageReq.ToPageList<v_OrderinfoInfw_ObjectionResultModel>(db.Database, "a.objectionid ,a.selleruserid ,a.buyeruserid ,a.tranmode ,b.Text ordstate_RefText ,a.ordstate ,a.totalamount ,a.buyerDepAmt ,a.sellerDepAmt ,c.Text SelfMentioning_RefText ,a.SelfMentioning ", "v_Orderinfo a LEFT JOIN [SYS_YLW].dbo.sys_code b ON a.ordstate = b.Value AND (b.CodeType='OrderState') LEFT JOIN [SYS_YLW].dbo.sys_code c ON a.SelfMentioning = c.Value AND (c.CodeType='JHFS') ", "a.objectionid = " + id, "a.selleruserid", "desc").Rows;
                var v_ObjectionResult1 = pageReq.ToPageList<v_ObjectionResult1Infw_ObjectionResultModel>(db.Database, "a.objectionid ,a.memid ,a.phone ,a.realname ,a.gender ,a.email ,a.qq ,a.wechat ,a.regtime ,a.corpname ,a.abbname ", "v_ObjectionResult1 a ", "a.objectionid = " + id, "a.phone", "desc").Rows;
                var v_ObjectionResult = pageReq.ToPageList<v_ObjectionResultInfw_ObjectionResultModel>(db.Database, "a.objectionid ,a.memid ,a.phone ,a.realname ,a.gender ,a.email ,a.qq ,a.wechat ,a.regtime ,a.corpname ,a.abbname ", "v_ObjectionResult a ", "a.objectionid = " + id, "a.phone", "desc").Rows;
                return new {v_objection, v_Orderinfo, v_ObjectionResult1, v_ObjectionResult};
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_ObjectionResult>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_ObjectionResult.Find(form.objectionid);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.fw_ObjectionResult.Add(form);
                }
                else
                {
                    dbForm.MemId = form.MemId;
                    dbForm.orderid = form.orderid;
                    dbForm.IsContinue = form.IsContinue;
                    dbForm.paymemId = form.paymemId;
                    dbForm.payment = form.payment;
                    dbForm.paymentmode = form.paymentmode;
                    dbForm.paymentendtime = form.paymentendtime;
                    dbForm.resultstatus = form.resultstatus;
                    dbForm.explain = form.explain;
                    dbForm.resultperson = form.resultperson;
                    dbForm.resultphone = form.resultphone;
                    dbForm.finalresult = form.finalresult;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                    dbForm.UpdatePerson = form.UpdatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                }
                // 记录多级零时主键对应(key int 为 js 生成的页内全局唯一编号)
                var _id_maps = new Dictionary<int, object[]>();
                if( post["v_objection"] != null )
                {
                    var sub= post["v_objection"] as JArray;
                    if( sub.Count > 0)
                    {
                        for(int i = 0; i < sub.Count; i++ )
                        {
                            JObject obj = sub[i] as JObject;
                            RowState rowState = (RowState)(int)obj["_row_state"];
                            var model = obj.ToObject<v_objection>(JsonExtension.FixJsonSerializer);
                            switch (rowState){
                                case RowState.Changed:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                                    break;
                                case RowState.New:
                                    model.objectionid = form.objectionid;
                            model.orderid = new SysSerialServices().GetNewSerial("v_objection." + form.objectionid, null).ToString();
                                    db.v_objection.Add(model);
                                    break;
                                case RowState.Deleted:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                                    break;
                            }
                        }
                    }
                }
                if( post["v_Orderinfo"] != null )
                {
                    var sub= post["v_Orderinfo"] as JArray;
                    if( sub.Count > 0)
                    {
                        for(int i = 0; i < sub.Count; i++ )
                        {
                            JObject obj = sub[i] as JObject;
                            RowState rowState = (RowState)(int)obj["_row_state"];
                            var model = obj.ToObject<v_Orderinfo>(JsonExtension.FixJsonSerializer);
                            switch (rowState){
                                case RowState.Changed:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                                    break;
                                case RowState.New:
                                    model.objectionid = form.objectionid;
                            model.selleruserid = new SysSerialServices().GetNewSerial("v_Orderinfo." + form.objectionid, null).ToString();
                                    db.v_Orderinfo.Add(model);
                                    break;
                                case RowState.Deleted:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                                    break;
                            }
                        }
                    }
                }
                if( post["v_ObjectionResult1"] != null )
                {
                    var sub= post["v_ObjectionResult1"] as JArray;
                    if( sub.Count > 0)
                    {
                        for(int i = 0; i < sub.Count; i++ )
                        {
                            JObject obj = sub[i] as JObject;
                            RowState rowState = (RowState)(int)obj["_row_state"];
                            var model = obj.ToObject<v_ObjectionResult1>(JsonExtension.FixJsonSerializer);
                            switch (rowState){
                                case RowState.Changed:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                                    break;
                                case RowState.New:
                                    model.objectionid = form.objectionid;
                            model.phone = new SysSerialServices().GetNewSerial("v_ObjectionResult1." + form.objectionid, null).ToString();
                                    db.v_ObjectionResult1.Add(model);
                                    break;
                                case RowState.Deleted:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                                    break;
                            }
                        }
                    }
                }
                if( post["v_ObjectionResult"] != null )
                {
                    var sub= post["v_ObjectionResult"] as JArray;
                    if( sub.Count > 0)
                    {
                        for(int i = 0; i < sub.Count; i++ )
                        {
                            JObject obj = sub[i] as JObject;
                            RowState rowState = (RowState)(int)obj["_row_state"];
                            var model = obj.ToObject<v_ObjectionResult>(JsonExtension.FixJsonSerializer);
                            switch (rowState){
                                case RowState.Changed:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                                    break;
                                case RowState.New:
                                    model.objectionid = form.objectionid;
                            model.phone = new SysSerialServices().GetNewSerial("v_ObjectionResult." + form.objectionid, null).ToString();
                                    db.v_ObjectionResult.Add(model);
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
            var _ids = new List<int>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = int.Parse(_id);
                  
                    db.fw_ObjectionResult.Remove(r => r.objectionid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newobjectionid()
        {
            return new SysSerialServices().GetNewSerial("fw_ObjectionResult");
        }

    }
}
