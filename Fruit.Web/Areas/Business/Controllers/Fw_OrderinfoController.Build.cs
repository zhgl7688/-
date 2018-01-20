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
                return View();
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(string id)
        {
            fw_orderinfo form = null;
            using(var db = new LUOLAI1401Context())
            {
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
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Fw_OrderinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_orderinfoListModel {
            public string ordid { get; set; }
            public string selleruserid { get; set; }
            public string buyeruserid { get; set; }
            public string tranmode { get; set; }
            public string contracturl { get; set; }
            public int? fromid { get; set; }
            public int? ordstate { get; set; }
            public int? isactive { get; set; }
            public DateTime? createdate { get; set; }
            public string ordcode { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_orderinfoListModel>(db.Database, "a.ordid ,a.selleruserid ,a.buyeruserid ,a.tranmode ,a.contracturl ,a.fromid ,a.ordstate ,a.isactive ,a.createdate ,a.ordcode ", "fw_orderinfo a ", sbCondition.ToString(), "a.ordid", "desc");
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_orderlistInfw_orderinfoModel {
            public string ordid { get; set; }
            public int ordlistid { get; set; }
            public string proname { get; set; }
            public string spec { get; set; }
            public decimal? price { get; set; }
            public decimal? counts { get; set; }
            public string listcode { get; set; }
        }
        public object GetDetial(string id)
        {
            using (var db = new LUOLAI1401Context())
            {
                var pageReq = new PageRequest();
                pageReq.Page = null; // 子表读取时不分页
                var fw_orderlist = pageReq.ToPageList<fw_orderlistInfw_orderinfoModel>(db.Database, "a.ordid ,a.ordlistid ,a.proname ,a.spec ,a.price ,a.counts ,a.listcode ", "fw_orderlist a ", "a.ordid = '" + id + "'", "a.ordlistid", "desc").Rows;
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
                    db.fw_orderinfo.Add(form);
                }
                else
                {
                    dbForm.selleruserid = form.selleruserid;
                    dbForm.buyeruserid = form.buyeruserid;
                    dbForm.tranmode = form.tranmode;
                    dbForm.contracturl = form.contracturl;
                    dbForm.fromid = form.fromid;
                    dbForm.ordstate = form.ordstate;
                    dbForm.isactive = form.isactive;
                    dbForm.createdate = form.createdate;
                    dbForm.ordcode = form.ordcode;
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
