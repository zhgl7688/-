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
    public partial class Base_ProductinfoController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> PdtCate, BaseUnits;
                using(var db = new LUOLAI1401Context())
                {
                    PdtCate = db.base_productCate.Select(i=>new ComboItem{ Text = i.CateName, Value = "" +  i.CateName }).ToList();
                }
                using(var db = new SysContext())
                {
                    BaseUnits = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "CodeType='MeasureUnit'")).ToList();
                }
                return View(new {dataSource = new {PdtCate,BaseUnits}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(string id)
        {
            base_productInfo form = null;
            List<ComboItem> PdtCate = null, BaseUnits = null;
            using(var db = new SysContext())
            {
                BaseUnits = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "CodeType='MeasureUnit'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                PdtCate = db.base_productCate.Select(i=>new ComboItem{ Text = i.CateName, Value = "" + i.CateName }).ToList();
                form = db.base_productInfo.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new base_productInfo
                {
                    id = id,
                    CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode)
                };
            }
            return View(new { form = form, dataSource = new { PdtCate,BaseUnits }});
        }

    }
    public partial class Base_ProductinfoApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class base_productInfoListModel {
            public string id { get; set; }
            public string PdtCode { get; set; }
            public string PdtName { get; set; }
            public string PdtCate { get; set; }
            public string PdtCate_RefText { get; set; }
            public string Spec { get; set; }
            public string BaseUnits { get; set; }
            public string BaseUnits_RefText { get; set; }
            public decimal RetailPrc { get; set; }
            public decimal TradPrc { get; set; }
            public string Description { get; set; }
            public string DelFlag { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            sbCondition.AppendFormat("({0}) AND ", string.Format("{0}{1}{2}", "a.CompCode= '", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "'"));
            SerachCondition.TextBox(sbCondition, "PdtCode", "a.PdtCode", "varchar");
            SerachCondition.TextBox(sbCondition, "PdtName", "a.PdtName", "varchar");
            SerachCondition.SelectUser(sbCondition, "PdtCate", "a.PdtCate", "varchar");
            SerachCondition.TextBox(sbCondition, "Spec", "a.Spec", "varchar");
            SerachCondition.Dropdown(sbCondition, "BaseUnits", "a.BaseUnits", "varchar");
            SerachCondition.TextBox(sbCondition, "RetailPrc", "a.RetailPrc", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<base_productInfoListModel>(db.Database, "a.id ,a.PdtCode ,a.PdtName ,b.CateName PdtCate_RefText ,a.PdtCate ,a.Spec ,c.Text BaseUnits_RefText ,a.BaseUnits ,a.RetailPrc ,a.TradPrc ,a.Description ,a.DelFlag ", "base_productInfo a LEFT JOIN base_productCate b ON a.PdtCate = b.CateName LEFT JOIN [Bcp.Sys].dbo.sys_code c ON a.BaseUnits = c.Value ", sbCondition.ToString(), "a.id", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<base_productInfo>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.base_productInfo.Find(form.id);
                if (dbForm == null)
                {
                    form.CompCode = string.Format("{0}", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode);
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.base_productInfo.Add(form);
                }
                else
                {
                    dbForm.PdtCode = form.PdtCode;
                    dbForm.PdtName = form.PdtName;
                    dbForm.PdtCate = form.PdtCate;
                    dbForm.Spec = form.Spec;
                    dbForm.BaseUnits = form.BaseUnits;
                    dbForm.RetailPrc = form.RetailPrc;
                    dbForm.TradPrc = form.TradPrc;
                    dbForm.Pic1 = form.Pic1;
                    dbForm.Pic2 = form.Pic2;
                    dbForm.Pic3 = form.Pic3;
                    dbForm.Pic4 = form.Pic4;
                    dbForm.Pic5 = form.Pic5;
                    dbForm.Pic6 = form.Pic6;
                    dbForm.Pic7 = form.Pic7;
                    dbForm.Pic8 = form.Pic8;
                    dbForm.Description = form.Description;
                    dbForm.DelFlag = form.DelFlag;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                    dbForm.UpdatePerson = form.UpdatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
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
                    db.base_productInfo.Remove(r => r.id == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newid()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("base_productInfo", null));
        }

    }
}
