/// 注意：本文件由 FruitBuilder 生成和管理，请误手工更改
using Fruit.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Fruit.Data;
using Fruit.Models;
using Newtonsoft.Json.Linq;

namespace Fruit.Web.Areas.Mms.Controllers
{
    public partial class BstypeController : Controller
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
        public ActionResult Edit(string id)
        {
            SYS_BSDATATYPE form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.SYS_BSDATATYPE.Find(id);
            }
            if (form == null)
            {
                form = new SYS_BSDATATYPE
                {
                    DataTypeID = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class BstypeApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class SYS_BSDATATYPEListModel {
            public string DataTypeID { get; set; }
            public string DataTypeName { get; set; }
            public string Remark { get; set; }
        }
        public object Get()
        {
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<SYS_BSDATATYPEListModel>(db.Database, "a.DataTypeID ,a.DataTypeName ,a.Remark ", "SYS_BSDATATYPE a ", "", "DataTypeID", "desc");
            }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class SYS_BSDATAInSYS_BSDATATYPEModel {
            public string DataTypeID { get; set; }
            public int? Series { get; set; }
            public string DataID { get; set; }
            public string DataName { get; set; }
            public string Remark { get; set; }
        }
        public object GetDetial(string id)
        {
            using (var db = new LUOLAI1401Context())
            {
                var pageReq = new PageRequest();
                pageReq.Page = null; // 子表读取时不分页
                var SYS_BSDATA = pageReq.ToPageList<SYS_BSDATAInSYS_BSDATATYPEModel>(db.Database, "a.DataTypeID ,a.Series ,a.DataID ,a.DataName ,a.Remark ", "SYS_BSDATA a ", "a.DataTypeID = '" + id + "'", "a.Series", "desc").Rows;
                return new {SYS_BSDATA};
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<SYS_BSDATATYPE>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.SYS_BSDATATYPE.Find(form.DataTypeID);
                if (dbForm == null)
                {
                    db.SYS_BSDATATYPE.Add(form);
                }
                else
                {
                    dbForm.DataTypeName = form.DataTypeName;
                    dbForm.Remark = form.Remark;
                }
                if( post["SYS_BSDATA"] != null )
                {
                    var sub= post["SYS_BSDATA"] as JArray;
                    if( sub.Count > 0)
                    {
                        for(int i = 0; i < sub.Count; i++ )
                        {
                            JObject obj = sub[i] as JObject;
                            RowState rowState = (RowState)(int)obj["_row_state"];
                            var model = obj.ToObject<SYS_BSDATA>();
                            switch (rowState){
                                case RowState.Changed:
                                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                                    break;
                                case RowState.New:
                                    model.DataTypeID = form.DataTypeID;
                                    model.Series = new SysSerialServices().GetNewSerial("SYS_BSDATA." + form.DataTypeID, null);
                                    db.SYS_BSDATA.Add(model);
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

            using (var db = new LUOLAI1401Context())
            {
                db.SYS_BSDATA.Remove(r => r.DataTypeID == id);
                db.SYS_BSDATATYPE.Remove(r => r.DataTypeID == id);
                db.SaveChanges();
            }
            return true;
        }
        public object NewDataTypeID()
        {
            return string.Format("{0:D12}", new SysSerialServices().GetNewSerial("SYS_BSDATATYPE", null));
        }

    }
}
