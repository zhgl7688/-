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
    public partial class TreetestController : Controller
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
            treetest form = null;
            using(var db = new LUOLAI1401Context())
            {
                form = db.treetest.Find(id);
            }
            ViewBag.RowState = 2;
            if (form == null)
            {
                ViewBag.RowState = 1;
                form = new treetest
                {
                    OrganizeCode = id
                };
            }
            return View(new { form = form, dataSource = new {  }});
        }

    }
    public partial class TreetestApiController : ApiController
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]class treetestListModel {
            public string OrganizeCode { get; set; }
            public string ParentCode { get; set; }
            public string ParentCode_N { get; set; }
            public string OrganizeSeq { get; set; }
            public string OrganizeName { get; set; }
            public string Description { get; set; }
            public string CompCode { get; set; }
            public string N_Code { get; set; }
        }
        public object Get()
        {
            var sbCondition = new System.Text.StringBuilder();
            var TreeFilter = HttpContext.Current.Request.Get("TreeFilter");
            if(!string.IsNullOrEmpty(TreeFilter))
            {
                sbCondition.AppendFormat("({0}) AND ", string.Format("{0}{1}{2}", "a.ParentCode='", TreeFilter, "'"));
            }

            if(sbCondition.Length>4) sbCondition.Length-=4;
            var pageReq = new PageRequest();
            using (var db = new LUOLAI1401Context())
            {
                return pageReq.ToPageList<treetestListModel>(db.Database, "a.OrganizeCode ,a.ParentCode ,a.ParentCode_N ,a.OrganizeSeq ,a.OrganizeName ,a.Description ,a.CompCode ,a.N_Code ", "treetest a ", sbCondition.ToString(), "OrganizeCode", "desc");
            }
        }
        [System.Web.Http.HttpGet]
        public System.Net.Http.HttpResponseMessage Types()
        {
            using(var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "SELECT OrganizeCode id,CASE WHEN ParentCode='0' THEN NULL ELSE ParentCode END parent,OrganizeName text FROM treetest";
                using(var reader = cmd.ExecuteReader())
                {
                    return new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new System.Net.Http.StringContent(reader.ToJsonArrayString()) };
                }
            }
        }
        public object Post(JObject post)
        {
            var form = post["form"].ToObject<treetest>();
            using (var db = new LUOLAI1401Context())
            {
                var dbForm = db.treetest.Find(form.OrganizeCode);
                if (dbForm == null)
                {
                    form.CreateDate = DateTime.Now;
                    form.CreatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                    db.treetest.Add(form);
                }
                else
                {
                    dbForm.ParentCode = form.ParentCode;
                    dbForm.ParentCode_N = form.ParentCode_N;
                    dbForm.OrganizeSeq = form.OrganizeSeq;
                    dbForm.OrganizeName = form.OrganizeName;
                    dbForm.Description = form.Description;
                    dbForm.CompCode = form.CompCode;
                    dbForm.N_Code = form.N_Code;
                    dbForm.UpdateDate = form.UpdateDate = DateTime.Now;
                    dbForm.UpdatePerson = form.UpdatePerson = (HttpContext.Current.Session["sys_user"] as sys_user).UserName;
                }
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
                    db.treetest.Remove(r => r.OrganizeCode == did);
                }
                db.SaveChanges();
            }
            return true;
        }
        public object NewOrganizeCode()
        {
            return new SysSerialServices().GetNewSerial("treetest");
        }

    }
}
