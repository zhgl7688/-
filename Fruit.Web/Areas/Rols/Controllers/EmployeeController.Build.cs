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
    public partial class EmployeeController : Controller
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
        public ActionResult Edit(int id)
        {
            HR_Employees form = null;
            List<ComboItem> Sex = null, Department = null, FStatus = null, Type = null, CustID = null;
            using(var db = new SysContext())
            {
                Sex = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='Sex'")).ToList();
                FStatus = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='forstatus'")).ToList();
                Type = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='PersonType'")).ToList();
            }
            using(var db = new LUOLAI1401Context())
            {
                Department = db.BD_Department.Select(i=>new ComboItem{ Text = i.FName, Value = "" + i.FCode }).ToList();
                CustID = db.BD_Customers.Select(i=>new ComboItem{ Text = i.Contact, Value = "" + i.FID }).ToList();
                form = db.HR_Employees.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new HR_Employees
                {
                    FID = id
                };
            }
            return View(new { form = form, dataSource = new { Sex,Department,FStatus,Type,CustID }});
        }

    }
    public partial class EmployeeApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class HR_EmployeesListModel {
            public int FID { get; set; }
            public string Code { get; set; }
            public string FName { get; set; }
            public string Sex { get; set; }
            public string Sex_RefText { get; set; }
            public string Nation { get; set; }
            public string Mobile { get; set; }
            public string IDCardNo { get; set; }
            public string Department { get; set; }
            public string Department_RefText { get; set; }
            public string FStatus { get; set; }
            public string FStatus_RefText { get; set; }
            public string Type { get; set; }
            public string Type_RefText { get; set; }
            public string CustID { get; set; }
            public string CustID_RefText { get; set; }
            public string Remark { get; set; }
            public string CreatePerson { get; set; }
            public DateTime CreateDate { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            SerachCondition.TextBox(sbCondition, "FName", "a.FName", "");
            SerachCondition.TextBox(sbCondition, "Mobile", "a.Mobile", "");
            SerachCondition.TextBox(sbCondition, "IDCardNo", "a.IDCardNo", "");

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<HR_EmployeesListModel>(db.Database, "a.FID ,a.Code ,a.FName ,b.Text Sex_RefText ,a.Sex ,a.Nation ,a.Mobile ,a.IDCardNo ,c.FName Department_RefText ,a.Department ,d.Text FStatus_RefText ,a.FStatus ,e.Text Type_RefText ,a.Type ,f.Contact CustID_RefText ,a.CustID ,a.Remark ,a.CreatePerson ,a.CreateDate ", "HR_Employees a LEFT JOIN [Bcp.Sysy].dbo.sys_code b ON a.Sex = b.Value AND (b.CodeType='Sex') LEFT JOIN BD_Department c ON a.Department = c.FCode LEFT JOIN [Bcp.Sysy].dbo.sys_code d ON a.FStatus = d.Value AND (d.CodeType='forstatus') LEFT JOIN [Bcp.Sysy].dbo.sys_code e ON a.Type = e.Value AND (e.CodeType='PersonType') LEFT JOIN BD_Customers f ON a.CustID = f.FID ", sbCondition.ToString(), "a.FID", "desc");
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<HR_Employees>(JsonExtension.FixJsonSerializer);
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.HR_Employees.Find(form.FID);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.HR_Employees.Add(form);
                }
                else
                {
                    dbForm.Code = form.Code;
                    dbForm.FName = form.FName;
                    dbForm.Sex = form.Sex;
                    dbForm.Nation = form.Nation;
                    dbForm.Mobile = form.Mobile;
                    dbForm.IDCardNo = form.IDCardNo;
                    dbForm.Department = form.Department;
                    dbForm.FStatus = form.FStatus;
                    dbForm.Type = form.Type;
                    dbForm.CustID = form.CustID;
                    dbForm.Remark = form.Remark;
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
            var _ids = new List<int>();

            using (var db = new LUOLAI1401Context())
            {
                foreach(string _id in id.Split(','))
                {
                    var did = int.Parse(_id);
                    db.HR_Employees.Remove(r => r.FID == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewFID()
        {
            return new SysSerialServices().GetNewSerial("HR_Employees");
        }

    }
}
