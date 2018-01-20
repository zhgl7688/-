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

namespace Fruit.Web.Areas.Baseinfo.Controllers
{
    public partial class OverdueController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> CustID;
                using(var db = new LUOLAI1401Context())
                {
                    CustID = db.BD_Customers.Select(i=>new ComboItem{ Text = i.Contact, Value = "" +  i.FID }).ToList();
                }
                return View(new {dataSource = new {CustID}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(int id)
        {
            PM_Overdue form = null;
            List<ComboItem> CustID = null;
            using(var db = new LUOLAI1401Context())
            {
                CustID = db.BD_Customers.Select(i=>new ComboItem{ Text = i.Contact, Value = "" + i.FID }).ToList();
                form = db.PM_Overdue.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new PM_Overdue
                {
                    FID = id
                };
            }
            return View(new { form = form, dataSource = new { CustID }});
        }

    }
    public partial class OverdueApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class PM_OverdueListModel {
            public int? FID { get; set; }
            public string CustID { get; set; }
            public string CustID_RefText { get; set; }
            public decimal? Amt { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<PM_OverdueListModel>(db.Database, "a.FID ,b.Contact CustID_RefText ,a.CustID ,a.Amt ", "PM_Overdue a LEFT JOIN BD_Customers b ON a.CustID = b.FID ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JArray post)
        {
            using (var db = new LUOLAI1401Context())
            {
                for(var i = 0; i < post.Count; i++)
                {
                    var form = post[i].ToObject<PM_Overdue>(JsonExtension.FixJsonSerializer);
                    var dbForm = db.PM_Overdue.Find(form.FID);
                    RowState rowState = (RowState)(int)post[i]["_row_state"];
                    switch(rowState)
                    {
                        case RowState.Changed:
                            dbForm.CustID = form.CustID;
                            dbForm.Amt = form.Amt;
                            break;
                        case RowState.Deleted:
                            db.Entry(dbForm).State = System.Data.Entity.EntityState.Deleted;
                            break;
                        case RowState.New:
                            form.FID = int.Parse((string)NewFID());
                            db.PM_Overdue.Add(form);
                            break;
                    }
                }
                db.SaveChanges();
                return true;
            }
        }
        public object Delete(string id)
        {
            var _ids = new List<int>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = int.Parse(_id);
                    db.PM_Overdue.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("PM_Overdue", null));
        }

    }
}
