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
    public partial class Fw_DirectpriceController : Controller
    {
        public ActionResult Index(string id = null)        {
            if (id == null)
            {
                // 提供搜索下拉框数据源
                List<ComboItem> isshow;
                using(var db = new SysContext())
                {
                    isshow = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
                }
                return View(new {dataSource = new {isshow}});
            }
            else
            {
                return View("Edit", id);
            }
        }
        public ActionResult Edit(int id)
        {
            fw_directprice form = null;
            List<ComboItem> isshow = null;
            using(var db = new SysContext())
            {
                isshow = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='YN'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                form = db.fw_directprice.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new fw_directprice
                {
                    directid = id
                };
            }
            return View(new { form = form, dataSource = new { isshow }});
        }

    }
    public partial class Fw_DirectpriceApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class fw_directpriceListModel {
            public int directid { get; set; }
            public string directtitle { get; set; }
            public string catid0 { get; set; }
            public DateTime? directtime0 { get; set; }
            public decimal? directprice0 { get; set; }
            public int? isshow { get; set; }
            public string isshow_RefText { get; set; }
            public decimal? floatprice0 { get; set; }
            public double? floatrate0 { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.TextBox(sbCondition, "directtitle", "a.directtitle", "");
            SerachCondition.TextBox(sbCondition, "catid0", "a.catid0", "");
            SerachCondition.Date(sbCondition, "directtime0", "a.directtime0", "");
            SerachCondition.Dropdown(sbCondition, "isshow", "a.isshow", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<fw_directpriceListModel>(db.Database, "a.directid ,a.directtitle ,a.catid0 ,a.directtime0 ,a.directprice0 ,b.Text isshow_RefText ,a.isshow ,a.floatprice0 ,a.floatrate0 ", "fw_directprice a LEFT JOIN [SYS_YLW].dbo.sys_code b ON a.isshow = b.Value AND (b.CodeType='YN') ", sbCondition.ToString(), "a.directid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<fw_directprice>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.fw_directprice.Find(form.directid);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.fw_directprice.Add(form);
                }
                else
                {
                    dbForm.directtitle = form.directtitle;
                    dbForm.catid0 = form.catid0;
                    dbForm.directtime0 = form.directtime0;
                    dbForm.directprice0 = form.directprice0;
                    dbForm.isshow = form.isshow;
                    dbForm.floatprice0 = form.floatprice0;
                    dbForm.floatrate0 = form.floatrate0;
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
            var _ids = new List<int>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = int.Parse(_id);
                    db.fw_directprice.Remove(r => r.directid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object Newdirectid()
        {
            return string.Format("{0:D0}", new SysSerialServices().GetNewSerial("fw_directprice", null));
        }

    }
}
