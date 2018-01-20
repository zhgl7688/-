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
    public partial class CxhdwhController : Controller
    {
        public ActionResult Index(string id = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                return View("Edit", id);
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_patrolPrmPicsSetViewable : wq_patrolPrmPicsSet {
            public string DealerCode_RefText { get; set; }
        }
        public ActionResult Edit(string id)
        {
            wq_patrolPrmPicsSet form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.Database.SqlQuery<wq_patrolPrmPicsSetViewable>("select a.compcode ,b.PopName DealerCode_RefText ,a.DealerCode ,a.code ,a.name ,a.PicsType ,a.bgtime ,a.edtime ,a.CONTENT  from wq_patrolPrmPicsSet a LEFT JOIN wq_termPop b ON a.DealerCode = b.PopCode  where a.code = @key", new System.Data.SqlClient.SqlParameter("@key", id)).FirstOrDefault();
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new wq_patrolPrmPicsSet
                {
                    code = id,
                    compcode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode)
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class CxhdwhApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_patrolPrmPicsSetListModel {
            public string code { get; set; }
            public string name { get; set; }
            public DateTime? bgtime { get; set; }
            public DateTime? edtime { get; set; }
            public string CONTENT { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            sbCondition.AppendFormat("({0}) AND ", string.Format("{0}{1}{2}{3}{4}", "((a.CompCode='", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "') or ('", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode, "'='super'))"));
            var Filter1 = HttpContext.Current.Request.Get("Filter1");
            switch (Filter1)
            {
                case "Filter1":
                    sbCondition.Append("(a.CompCode='0000') AND ");
                    break;
                case "Filter2":
                    sbCondition.Append("(a.CompCode<>'0000') AND ");
                    break;
            }

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<wq_patrolPrmPicsSetListModel>(db.Database, "a.code ,a.name ,a.bgtime ,a.edtime ,a.CONTENT ", "wq_patrolPrmPicsSet a ", sbCondition.ToString(), "code", "desc");
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class wq_patrolPrmPicsSet_bdInwq_patrolPrmPicsSetModel {
            public string code { get; set; }
            public int Series { get; set; }
            public string CustCode { get; set; }
            public string CustName { get; set; }
            public string Contact1 { get; set; }
            public string Tel1 { get; set; }
            public string Mobile1 { get; set; }
            public string Contact2 { get; set; }
            public string Tel2 { get; set; }
            public string Mobile2 { get; set; }
        }
        public object GetDetial(string id)
        {
            using (var db = new LUOLAI1401Context())
            {
                var pageReq = new PageRequest();
                pageReq.Page = null; // 子表读取时不分页
                var wq_patrolPrmPicsSet_bd = pageReq.ToPageList<wq_patrolPrmPicsSet_bdInwq_patrolPrmPicsSetModel>(db.Database, "a.code ,a.Series ,a.CustCode ,a.CustName ,a.Contact1 ,a.Tel1 ,a.Mobile1 ,a.Contact2 ,a.Tel2 ,a.Mobile2 ", "wq_patrolPrmPicsSet_bd a ", "a.code = '" + id + "'", "a.Series", "desc").Rows;
                return new {wq_patrolPrmPicsSet_bd};
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<wq_patrolPrmPicsSet>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.wq_patrolPrmPicsSet.Find(form.code);
                if (dbForm == null)
                {
                    form.compcode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode);
                    db.wq_patrolPrmPicsSet.Add(form);
                }
                else
                {
                    dbForm.name = form.name;
                    dbForm.bgtime = form.bgtime;
                    dbForm.edtime = form.edtime;
                    dbForm.CONTENT = form.CONTENT;
                }
                if( post["wq_patrolPrmPicsSet_bd"] != null )
                {
                    var sub= post["wq_patrolPrmPicsSet_bd"] as JArray;
                    if( sub.Count > 0)
                    {
                        for(int i = 0; i < sub.Count; i++ )
                        {
                            JObject obj = sub[i] as JObject;
                            RowState rowState = (RowState)(int)obj["_row_state"];
                            var model = obj.ToObject<wq_patrolPrmPicsSet_bd>(JsonExtension.FixJsonSerializer);
                            switch (rowState){
                                case RowState.Changed:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                                    break;
                                case RowState.New:
                                    model.code = form.code;
                                    model.Series = new SysSerialServices().GetNewSerial("wq_patrolPrmPicsSet_bd." + form.code, null);
                                    db.wq_patrolPrmPicsSet_bd.Add(model);
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
                    db.wq_patrolPrmPicsSet_bd.Remove(r => r.code == did);
                    db.wq_patrolPrmPicsSet.Remove(r => r.code == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newcode()
        {
            return string.Format("{0:D12}", new SysSerialServices().GetNewSerial("wq_patrolPrmPicsSet", null));
        }

    }
}
