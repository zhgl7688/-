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

namespace Fruit.Web.Areas.Contr.Controllers
{
    public partial class ContractfileController : Controller
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
        public ActionResult Edit(long id)
        {
            PM_ContractFiles form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.PM_ContractFiles.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new PM_ContractFiles
                {
                    FID = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class ContractfileApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class PM_ContractFilesListModel {
            public long FID { get; set; }
            public string PManager { get; set; }
            public string type { get; set; }
            public string PID { get; set; }
            public decimal? Amt { get; set; }
            public string Owner { get; set; }
            public string ProjName { get; set; }
            public string BeginDate { get; set; }
            public string EndDate { get; set; }
            public string projAddress { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.Date(sbCondition, "BeginDate", "a.BeginDate", "");
            SerachCondition.Date(sbCondition, "EndDate", "a.EndDate", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<PM_ContractFilesListModel>(db.Database, "a.FID ,a.PManager ,a.type ,a.PID ,a.Amt ,a.Owner ,a.ProjName ,a.BeginDate ,a.EndDate ,a.projAddress ", "PM_ContractFiles a ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<PM_ContractFiles>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.PM_ContractFiles.Find(form.FID);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.PM_ContractFiles.Add(form);
                }
                else
                {
                    dbForm.PManager = form.PManager;
                    dbForm.type = form.type;
                    dbForm.PID = form.PID;
                    dbForm.Amt = form.Amt;
                    dbForm.Owner = form.Owner;
                    dbForm.ProjName = form.ProjName;
                    dbForm.BeginDate = form.BeginDate;
                    dbForm.EndDate = form.EndDate;
                    dbForm.projAddress = form.projAddress;
                    dbForm.Remark = form.Remark;
                    dbForm.Attach = form.Attach;
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
            var _ids = new List<long>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = long.Parse(_id);
                    db.PM_ContractFiles.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("PM_ContractFiles", null));
        }

    }
}
