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
    public partial class Fw_OrderinfoController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> selleruserid, buyeruserid, tranmode, ordstate;
                using(var db = new LUOLAI1401Context())
                {
                    selleruserid = db.fw_memberinfo.Select(i=>new ComboItem{ Text = i.realname, Value = "" +  i.memid }).ToList();
                    buyeruserid = db.fw_memberinfo.Select(i=>new ComboItem{ Text = i.realname, Value = "" +  i.memid }).ToList();
                }
                using(var db = new SysContext())
                {
                    tranmode = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='TradingMode'")).ToList();
                    ordstate = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='OrderState'")).ToList();
                }
                return View(new {dataSource = new {selleruserid,buyeruserid,tranmode,ordstate}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(string id)
        {
            fw_orderinfo form = null;
            List<ComboItem> selleruserid = null, buyeruserid = null, tranmode = null, ordstate = null, isactive = null, SelfMentioning = null;
            using(var db = new SysContext())
            {
                tranmode = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='TradingMode'")).ToList();
                ordstate = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='OrderState'")).ToList();
                isactive = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
                SelfMentioning = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='JHFS'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                selleruserid = db.fw_memberinfo.Select(i=>new ComboItem{ Text = i.realname, Value = "" + i.memid }).ToList();
                buyeruserid = db.fw_memberinfo.Select(i=>new ComboItem{ Text = i.realname, Value = "" + i.memid }).ToList();
                form = db.fw_orderinfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_orderinfo
                {
                    ordid = id
                };
            }
            return View(new { form = form, dataSource = new { selleruserid,buyeruserid,tranmode,ordstate,isactive,SelfMentioning }});
        }

    }
    public partial class Fw_OrderinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_orderinfoListModel {
            public string ordid { get; set; }
            public string selleruserid { get; set; }
            public string selleruserid_RefText { get; set; }
            public string buyeruserid { get; set; }
            public string buyeruserid_RefText { get; set; }
            public string tranmode { get; set; }
            public string tranmode_RefText { get; set; }
            public int? fromid { get; set; }
            public int? ordstate { get; set; }
            public string ordstate_RefText { get; set; }
            public int? isactive { get; set; }
            public string isactive_RefText { get; set; }
            public string SelfMentioning { get; set; }
            public string SelfMentioning_RefText { get; set; }
            public string pathvoucher { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.Dropdown(sbCondition, "selleruserid", "a.selleruserid", "");
            SerachCondition.Dropdown(sbCondition, "buyeruserid", "a.buyeruserid", "");
            SerachCondition.Dropdown(sbCondition, "tranmode", "a.tranmode", "");
            SerachCondition.Dropdown(sbCondition, "ordstate", "a.ordstate", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_orderinfoListModel>(db.Database, "a.ordid ,b.realname selleruserid_RefText ,a.selleruserid ,c.realname buyeruserid_RefText ,a.buyeruserid ,d.Text tranmode_RefText ,a.tranmode ,a.fromid ,e.Text ordstate_RefText ,a.ordstate ,f.Text isactive_RefText ,a.isactive ,g.Text SelfMentioning_RefText ,a.SelfMentioning ,a.pathvoucher ", "fw_orderinfo a LEFT JOIN fw_memberinfo b ON a.selleruserid = b.memid LEFT JOIN fw_memberinfo c ON a.buyeruserid = c.memid LEFT JOIN [SYS_YLW].dbo.sys_code d ON a.tranmode = d.Value AND (d.CodeType='TradingMode') LEFT JOIN [SYS_YLW].dbo.sys_code e ON a.ordstate = e.Value AND (e.CodeType='OrderState') LEFT JOIN [SYS_YLW].dbo.sys_code f ON a.isactive = f.Value AND (f.CodeType='YN') LEFT JOIN [SYS_YLW].dbo.sys_code g ON a.SelfMentioning = g.Value AND (g.CodeType='JHFS') ", sbCondition.ToString(), "a.ordid", "desc");
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_orderlistInfw_orderinfoModel {
            public string ordid { get; set; }
            public int ordlistid { get; set; }
            public string proname { get; set; }
            public string spec { get; set; }
            public decimal? price { get; set; }
            public decimal? counts { get; set; }
        }
        public object GetDetial(string id)
        {
            using (var db = new LUOLAI1401Context())
            {
                var pageReq = new PageRequest();
                pageReq.Page = null; // 子表读取时不分页
                var fw_orderlist = pageReq.ToPageList<fw_orderlistInfw_orderinfoModel>(db.Database, "a.ordid ,a.ordlistid ,a.proname ,a.spec ,a.price ,a.counts ", "fw_orderlist a ", "a.ordid = '" + id + "'", "a.ordlistid", "desc").Rows;
                return new {fw_orderlist};
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_orderinfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_orderinfo.Find(form.ordid);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.fw_orderinfo.Add(form);
                }
                else
                {
                    dbForm.selleruserid = form.selleruserid;
                    dbForm.buyeruserid = form.buyeruserid;
                    dbForm.tranmode = form.tranmode;
                    dbForm.fromid = form.fromid;
                    dbForm.ordstate = form.ordstate;
                    dbForm.isactive = form.isactive;
                    dbForm.SelfMentioning = form.SelfMentioning;
                    dbForm.pathvoucher = form.pathvoucher;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                    dbForm.UpdatePerson = form.UpdatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                }
                // 记录多级零时主键对应(key int 为 js 生成的页内全局唯一编号)
                var _id_maps = new Dictionary<int, object[]>();
                if( post["fw_orderlist"] != null )
                {
                    var sub= post["fw_orderlist"] as JArray;
                    if( sub.Count > 0)
                    {
                        for(int i = 0; i < sub.Count; i++ )
                        {
                            JObject obj = sub[i] as JObject;
                            RowState rowState = (RowState)(int)obj["_row_state"];
                            var model = obj.ToObject<fw_orderlist>(JsonExtension.FixJsonSerializer);
                            switch (rowState){
                                case RowState.Changed:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                                    break;
                                case RowState.New:
                                    model.ordid = form.ordid;
                            model.ordlistid = new SysSerialServices().GetNewSerial("fw_orderlist." + form.ordid, null);
                                    db.fw_orderlist.Add(model);
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
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "UPDATE fw_orderinfo SET pathfilename=Path FROM SYS_YLW.dbo.sys_file WHERE pathvoucher=FileId AND ordid=@ordid";
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@ordid";
                p1.Value = form.ordid;
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
                    db.fw_orderlist.Remove(r => r.ordid == did);
                    db.fw_orderinfo.Remove(r => r.ordid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newordid()
        {
            return new SysSerialServices().GetNewSerial("fw_orderinfo");
        }

    }
}
