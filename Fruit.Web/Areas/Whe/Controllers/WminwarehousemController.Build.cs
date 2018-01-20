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

namespace Fruit.Web.Areas.Whe.Controllers
{
    public partial class WminwarehousemController : Controller
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
        public ActionResult Edit(Guid id)
        {
            tbWmInWarehouses form = null;
            List<ComboItem> Color = null;
            List<TreeItem> ParentCode = null;
            using(var db = new SMTERPContext())
            {
                ParentCode = db.tbWmInWarehouses.Select(t=>new TreeItem{Id = "" + t.Guid, Text = "" + t.Guid, ParentId = "" + t.ParentCode}).ToList();
                //Color = db.tbBdColor.Select(i=>new ComboItem{ Text = i.Name, Value = "" + i.Guid }).ToList();
                form = db.tbWmInWarehouses.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new tbWmInWarehouses
                {
                    Guid = id
                };
            }
            return View(new { form = form, dataSource = new { ParentCode,Color }});
        }

    }
    public partial class WminwarehousemApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class tbWmInWarehousesListModel {
            public Guid Guid { get; set; }
            public Guid? ParentCode { get; set; }
            public Guid? Customer { get; set; }
            public Guid? Department { get; set; }
            public string Source { get; set; }
            public string SourceCode { get; set; }
            public string SourceSheetID { get; set; }
            public Guid? Supplier { get; set; }
            public Guid? ProductCode { get; set; }
            public string EnglishName { get; set; }
            public Guid? ProductCategory { get; set; }
            public float? Width { get; set; }
            public float? Height { get; set; }
            public Guid? WareHouse { get; set; }
            public Guid? SaleCode { get; set; }
            public string SaleSheetID { get; set; }
            public Guid? Color { get; set; }
            public string Color_RefText { get; set; }
            public string LotsCode { get; set; }
            public string SN { get; set; }
            public Guid? Unit { get; set; }
            public float? OutAmount { get; set; }
            public float? Amount { get; set; }
            public Guid? WarehouseLocation { get; set; }
            public DateTime? ManufactureDate { get; set; }
            public decimal? Price { get; set; }
            public decimal? SalePrice { get; set; }
            public float? Tax { get; set; }
            public decimal? TotalCharge { get; set; }
            public string Remark { get; set; }
            public DateTime? ReviceDate { get; set; }
            public Guid? RepertoryUid { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new SMTERPContext())
            {
                return pageReq.ToPageList<tbWmInWarehousesListModel>(db.Database, "a.Guid ,a.ParentCode ,a.Customer ,a.Department ,a.Source ,a.SourceCode ,a.SourceSheetID ,a.Supplier ,a.ProductCode ,a.EnglishName ,a.ProductCategory ,a.Width ,a.Height ,a.WareHouse ,a.SaleCode ,a.SaleSheetID ,b.Name Color_RefText ,a.Color ,a.LotsCode ,a.SN ,a.Unit ,a.OutAmount ,a.Amount ,a.WarehouseLocation ,a.ManufactureDate ,a.Price ,a.SalePrice ,a.Tax ,a.TotalCharge ,a.Remark ,a.ReviceDate ,a.RepertoryUid ", "tbWmInWarehouses a LEFT JOIN tbBdColor b ON a.Color = b.Guid ", sbCondition.ToString(), "a.Guid", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<tbWmInWarehouses>(JsonExtension.FixJsonSerializer);
            using (var db = new SMTERPContext())
            {
                var dbForm = db.tbWmInWarehouses.Find(form.Guid);
                if (dbForm == null)
                {
                    db.tbWmInWarehouses.Add(form);
                }
                else
                {
                    dbForm.ParentCode = form.ParentCode;
                    dbForm.Customer = form.Customer;
                    dbForm.Department = form.Department;
                    dbForm.Source = form.Source;
                    dbForm.SourceCode = form.SourceCode;
                    dbForm.SourceSheetID = form.SourceSheetID;
                    dbForm.Supplier = form.Supplier;
                    dbForm.ProductCode = form.ProductCode;
                    dbForm.EnglishName = form.EnglishName;
                    dbForm.ProductCategory = form.ProductCategory;
                    dbForm.Width = form.Width;
                    dbForm.Height = form.Height;
                    dbForm.WareHouse = form.WareHouse;
                    dbForm.SaleCode = form.SaleCode;
                    dbForm.SaleSheetID = form.SaleSheetID;
                    dbForm.Color = form.Color;
                    dbForm.LotsCode = form.LotsCode;
                    dbForm.SN = form.SN;
                    dbForm.Unit = form.Unit;
                    dbForm.OutAmount = form.OutAmount;
                    dbForm.Amount = form.Amount;
                    dbForm.WarehouseLocation = form.WarehouseLocation;
                    dbForm.ManufactureDate = form.ManufactureDate;
                    dbForm.Price = form.Price;
                    dbForm.SalePrice = form.SalePrice;
                    dbForm.Tax = form.Tax;
                    dbForm.TotalCharge = form.TotalCharge;
                    dbForm.Remark = form.Remark;
                    dbForm.ReviceDate = form.ReviceDate;
                    dbForm.RepertoryUid = form.RepertoryUid;
                }
                // 记录多级零时主键对应(key int 为 js 生成的页内全局唯一编号)
                var _id_maps = new Dictionary<int, object[]>();
                db.SaveChanges();
            }
            return new { success = true, form = form };
        }
        [System.Web.Http.HttpPost]
        public object DoShowMyDateDialog(JObject row)
        {
            object result = null;
            using(var db = new SMTERPContext())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "select @date";
                var p1 = cmd.CreateParameter();
                p1.ParameterName = "@date";
                p1.Value = (string)row.GetValue("date");
                cmd.Parameters.Add(p1);
                result = cmd.ExecuteScalar();
            }
            return result;
        }
        public object Delete(string id)
        {
            var _ids = new List<Guid>();

            using (var db = new SMTERPContext())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = Guid.Parse(_id);
                    db.tbWmInWarehouses.Remove(r => r.Guid == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewGuid()
        {
            return Guid.NewGuid();
        }

    }
}
