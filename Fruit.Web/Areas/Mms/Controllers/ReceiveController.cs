using Fruit.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Fruit.Data;
using Fruit.Models;

namespace Fruit.Web.Areas.Mms.Controllers
{
    public class ReceiveController : Controller
    {
        // GET: Mms/Receive
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

        public ActionResult Edit(string id)
        {
            mms_receive form = null;
            List<ComboItem> warehouseItems = null, payKinds = null, supplyType = null;
            using (var db = new MmsContext())
            {
                form = db.mms_receive.Find(id);
                warehouseItems = db.mms_warehouse.OrderBy(w => w.WarehouseName).Select(w => new ComboItem
                {
                    Text = w.WarehouseName,
                    Value = w.WarehouseCode
                }).ToList();
            }

            using (var db = new SysContext())
            {
                payKinds = db.sys_code.Where(c => c.CodeType == "PayKind").OrderBy(c => c.Seq).Select(c=>new ComboItem
                {
                    Text = c.Text,
                    Value = c.Code
                }).ToList();

                supplyType = db.sys_code.Where(c => c.CodeType == "SupplyType").OrderBy(c => c.Seq).Select(c => new ComboItem
                {
                    Text = c.Text,
                    Value = c.Code
                }).ToList();
            }

            if (form == null)
            {
                form = new mms_receive { BillNo = id };
            }

            return View(new { form = form, @readonly = form.ApproveState != null, dataSource = new { warehouseItems, payKinds, supplyType } });
        }
    }

    public class ReceiveApiController: ApiController
    {
        public object Get()
        {
            using(var db = new MmsContext())
            {
                var qr = new PageRequest();
                return qr.ToPageList(db.mms_receive.LeftJoin(db.mms_merchants, r => r.SupplierCode, m => m.MerchantsCode, (r, m) => new
                {
                    r.BillNo,
                    SupplierName = m.MerchantsName,
                    r.SupplyType,
                    r.ContractCode,
                    r.ReceiveDate,
                    r.TotalMoney,
                    r.WarehouseCode,
                    r.OriginalNum,
                    r.DoPerson,
                    r.Remark
                }).LeftJoin(db.mms_warehouse, r => r.WarehouseCode, w => w.WarehouseCode, (r, w) => new
                {
                    r.BillNo,
                    r.SupplierName,
                    r.SupplyType,
                    r.ContractCode,
                    r.ReceiveDate,
                    r.TotalMoney,
                    w.WarehouseName,
                    r.OriginalNum,
                    r.DoPerson,
                    r.Remark
                }), "BillNo", "desc");
            }
        }

        public object Get(string id)
        {
            using (var db = new MmsContext())
            {
                var form = db.mms_receive.Find(id);
                if (form == null)
                {
                    form = new mms_receive { BillNo = id };
                }
                var grid = form.mms_receiveDetail.ToList();
                return new { form, grid };
            }
        }

        public object GetDetial(string id)
        {
            using (var db = new MmsContext())
            {
                return db.mms_receiveDetail.AsNoTracking().Where(r => r.BillNo == id).LeftJoin(db.mms_material, r => r.MaterialCode, m => m.MaterialCode, (r,m) => new
                {
                    r.BillNo,
                    r.RowId,
                    r.MaterialCode,
                    r.Unit,
                    r.CheckNum,
                    r.Num,
                    r.RemainNum,
                    r.UnitPrice,
                    r.Money,
                    r.PrePay,
                    r.SrcBillType,
                    r.SrcBillNo,
                    r.SrcRowId,
                    r.CreatePerson,
                    r.UpdatePerson,
                    r.CreateDate,
                    r.UpdateDate,
                    m.MaterialName,
                    m.Model
                }).ToList();
            }
        }

        public object Delete(string id)
        {
            using (var db = new MmsContext())
            {
                db.mms_receiveDetail.Remove(r => r.BillNo == id);
                db.mms_receive.Remove(r => r.BillNo == id);
                db.SaveChanges();
            }
            return true;
        }

        [System.Web.Http.HttpGet]
        public IList<string> GetBillNo()
        {
            using (var db = new MmsContext())
            {
                var q = HttpContext.Current.Request.QueryString["q"];
                IQueryable<mms_receive> queryable = db.mms_receive;
                if (!string.IsNullOrEmpty(q))
                {
                    queryable = queryable.Where(r => r.BillNo.StartsWith(q));
                }
                return queryable.OrderBy(r => r.BillNo).Take(10).Select(r => r.BillNo).ToList();
            }
        }

        [System.Web.Http.HttpPost]
        public string NewBillNo()
        {
            using (var db = new MmsContext())
            {
                string head = DateTime.Now.ToString("yyyyMMdd");
                var no = db.mms_receive.OrderByDescending(r => r.BillNo).Select(r => r.BillNo).FirstOrDefault();
                if (no == null || !no.StartsWith(head))
                {
                    return head + "0001";
                }
                else
                {
                    int i = int.Parse(no.Substring(8)) + 1;
                    if (i > 9999)
                    {
                        throw new Exception("意外的超出了可用订单位数！");
                    }
                    return head + i.ToString("D4");
                }
            }
        }
    }
}