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
    public partial class BorrowsController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> BorrowName, CustID;
                using(var db = new LUOLAI1401Context())
                {
                    BorrowName = db.v_Certificate.Select(i=>new ComboItem{ Text = i.Name, Value = "" +  i.FID }).ToList();
                }
                using(var db = new SysContext())
                {
                    CustID = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='FType'")).ToList();
                }
                return View(new {dataSource = new {BorrowName,CustID}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(string id)
        {
            HR_Borrows form = null;
            List<ComboItem> CustID = null, BorrowName = null;
            using(var db = new SysContext())
            {
                CustID = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='FType'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                BorrowName = db.v_Certificate.Select(i=>new ComboItem{ Text = i.Name, Value = "" + i.FID }).ToList();
                form = db.HR_Borrows.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new HR_Borrows
                {
                    FID = id
                };
            }
            return View(new { form = form, dataSource = new { CustID,BorrowName }});
        }

    }
    public partial class BorrowsApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class HR_BorrowsListModel {
            public string FID { get; set; }
            public string BorrowDate { get; set; }
            public string CustID { get; set; }
            public string CustID_RefText { get; set; }
            public string BorrowName { get; set; }
            public string BorrowName_RefText { get; set; }
            public string Used { get; set; }
            public string BorrowPerson { get; set; }
            public string BorrowPhone { get; set; }
            public string ReturePerson { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.Date(sbCondition, "BorrowDate", "a.BorrowDate", "");
            SerachCondition.Dropdown(sbCondition, "CustID", "a.CustID", "");
            SerachCondition.Dropdown(sbCondition, "BorrowName", "a.BorrowName", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<HR_BorrowsListModel>(db.Database, "a.FID ,a.BorrowDate ,b.Text CustID_RefText ,a.CustID ,c.Name BorrowName_RefText ,a.BorrowName ,a.Used ,a.BorrowPerson ,a.BorrowPhone ,a.ReturePerson ", "HR_Borrows a LEFT JOIN [Bcp.Sysy].dbo.sys_code b ON a.CustID = b.Value AND (b.CodeType='FType') LEFT JOIN v_Certificate c ON a.BorrowName = c.FID ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<HR_Borrows>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.HR_Borrows.Find(form.FID);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.HR_Borrows.Add(form);
                }
                else
                {
                    dbForm.BorrowDate = form.BorrowDate;
                    dbForm.CustID = form.CustID;
                    dbForm.BorrowName = form.BorrowName;
                    dbForm.Used = form.Used;
                    dbForm.BorrowPerson = form.BorrowPerson;
                    dbForm.BorrowPhone = form.BorrowPhone;
                    dbForm.ReturePerson = form.ReturePerson;
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
                    db.HR_Borrows.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("HR_Borrows", null));
        }

    }
}
