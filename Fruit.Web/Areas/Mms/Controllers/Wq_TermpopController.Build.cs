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

namespace Fruit.Web.Areas.Mms.Controllers
{
    public partial class Wq_TermpopController : Controller
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
            wq_termPop form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.wq_termPop.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new wq_termPop
                {
                    PopCode = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class Wq_TermpopApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_termPopListModel {
            public string PopCode { get; set; }
            public string PopName { get; set; }
            public string Address { get; set; }
            public string Contact1 { get; set; }
            public string Tel1 { get; set; }
            public string Mobile1 { get; set; }
            public string Contact2 { get; set; }
            public string Tel2 { get; set; }
            public string Mobile2 { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            sbCondition.AppendFormat("({0}) AND ", string.Format("{0}{1}{2}{3}{4}", "((a.CompCode='", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "') or ('", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode, "'='super'))"));
            SerachCondition.TextBox(sbCondition, "PopName", "a.PopName", "varchar");
            SerachCondition.TextBox(sbCondition, "Contact1", "a.Contact1", "varchar");
            SerachCondition.TextBox(sbCondition, "Tel1", "a.Tel1", "varchar");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<wq_termPopListModel>(db.Database, "a.PopCode ,a.PopName ,a.Address ,a.Contact1 ,a.Tel1 ,a.Mobile1 ,a.Contact2 ,a.Tel2 ,a.Mobile2 ", "wq_termPop a ", sbCondition.ToString(), "a.PopCode", "desc");
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_Pop_DealerInwq_termPopModel {
            public string PopCode { get; set; }
            public int id { get; set; }
            public string CompCode { get; set; }
            public string DealerCode { get; set; }
        }
        public object GetDetial(string id)
        {
            using (var db = new LUOLAI1401Context())
            {
                var pageReq = new PageRequest();
                pageReq.Page = null; // 子表读取时不分页
                var wq_Pop_Dealer = pageReq.ToPageList<wq_Pop_DealerInwq_termPopModel>(db.Database, "a.PopCode ,a.id ,a.CompCode ,a.DealerCode ", "wq_Pop_Dealer a ", "a.PopCode = '" + id + "'", "a.id", "desc").Rows;
                return new {wq_Pop_Dealer};
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<wq_termPop>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.wq_termPop.Find(form.PopCode);
                if (dbForm == null)
                {
                    db.wq_termPop.Add(form);
                }
                else
                {
                    dbForm.PopName = form.PopName;
                    dbForm.Address = form.Address;
                    dbForm.Contact1 = form.Contact1;
                    dbForm.Tel1 = form.Tel1;
                    dbForm.Mobile1 = form.Mobile1;
                    dbForm.Contact2 = form.Contact2;
                    dbForm.Tel2 = form.Tel2;
                    dbForm.Mobile2 = form.Mobile2;
                }
                // 记录多级零时主键对应(key int 为 js 生成的页内全局唯一编号)
                var _id_maps = new Dictionary<int, object[]>();
                if( post["wq_Pop_Dealer"] != null )
                {
                    var sub= post["wq_Pop_Dealer"] as JArray;
                    if( sub.Count > 0)
                    {
                        for(int i = 0; i < sub.Count; i++ )
                        {
                            JObject obj = sub[i] as JObject;
                            RowState rowState = (RowState)(int)obj["_row_state"];
                            var model = obj.ToObject<wq_Pop_Dealer>(JsonExtension.FixJsonSerializer);
                            switch (rowState){
                                case RowState.Changed:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                                    break;
                                case RowState.New:
                                    model.PopCode = form.PopCode;
                                    model.id = new SysSerialServices().GetNewSerial("wq_Pop_Dealer." + form.PopCode, null);
                                    db.wq_Pop_Dealer.Add(model);
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
                    db.wq_Pop_Dealer.Remove(r => r.PopCode == did);
                    db.wq_termPop.Remove(r => r.PopCode == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewPopCode()
        {
            return string.Format("{0:D12}", new SysSerialServices().GetNewSerial("wq_termPop", null));
        }

    }
}
