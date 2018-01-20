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

namespace Fruit.Web.Areas.Wage.Controllers
{
    public partial class FundsetController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> FType;
                using(var db = new SysContext())
                {
                    FType = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='SBType'")).ToList();
                }
                return View(new {dataSource = new {FType}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(int id)
        {
            HR_HousingFundSet form = null;
            List<ComboItem> FType = null;
            using(var db = new SysContext())
            {
                FType = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='SBType'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                form = db.HR_HousingFundSet.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new HR_HousingFundSet
                {
                    ID = id
                };
            }
            return View(new { form = form, dataSource = new { FType }});
        }

    }
    public partial class FundsetApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class HR_HousingFundSetListModel {
            public int ID { get; set; }
            public string FType { get; set; }
            public string FType_RefText { get; set; }
            public decimal? pIndv { get; set; }
            public decimal? pCorp { get; set; }
            public string Remark { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<HR_HousingFundSetListModel>(db.Database, "a.ID ,b.Text FType_RefText ,a.FType ,a.pIndv ,a.pCorp ,a.Remark ", "HR_HousingFundSet a LEFT JOIN [Bcp.Sysy].dbo.sys_code b ON a.FType = b.Value AND (b.CodeType='SBType') ", sbCondition.ToString(), "a.ID", "desc");
            }
        }
        public object Post(JArray post)
        {
            using (var db = new LUOLAI1401Context())
            {
                for(var i = 0; i < post.Count; i++)
                {
                    var form = post[i].ToObject<HR_HousingFundSet>(JsonExtension.FixJsonSerializer);
                    var dbForm = db.HR_HousingFundSet.Find(form.ID);
                    RowState rowState = (RowState)(int)post[i]["_row_state"];
                    switch(rowState)
                    {
                        case RowState.Changed:
                            dbForm.FType = form.FType;
                            dbForm.pIndv = form.pIndv;
                            dbForm.pCorp = form.pCorp;
                            dbForm.Remark = form.Remark;
                            dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                            dbForm.UpdatePerson = form.UpdatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                            break;
                        case RowState.Deleted:
                            db.Entry(dbForm).State = System.Data.Entity.EntityState.Deleted;
                            break;
                        case RowState.New:
                            form.ID = int.Parse((string)NewID());
                            form.CreateDate = DateTime.Now;
                            form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                            db.HR_HousingFundSet.Add(form);
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
                    db.HR_HousingFundSet.Remove(r => r.ID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewID()
        {
            return string.Format("{0:D12}", new SysSerialServices().GetNewSerial("HR_HousingFundSet", null));
        }

    }
}
