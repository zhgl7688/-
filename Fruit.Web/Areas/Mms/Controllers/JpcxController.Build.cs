﻿/// 注意：本文件由 FruitBuilder 生成和管理，请误手工更改
using Fruit.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Fruit.Data;
using Fruit.Models;
using Newtonsoft.Json.Linq;

namespace Fruit.Web.Areas.Mms.Controllers
{
    public partial class JpcxController : Controller
    {
        public ActionResult Index()
        {
                return View();
        }

    }
    public partial class JpcxApiController : ApiController
    {
        public object Get(JObject req)
        {
            using (var db = new LUOLAI1401Context())
            {
                db.Database.Connection.Open();
                try {
                    var cmd = db.Database.Connection.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "proc_SearchList";
                    cmd.Parameters.Add(new SqlParameter("@tableName", "wq_goodsreport GD LEFT JOIN  wq_termPop PO ON  GD.DealerCode=PO.PopCode"));
                    cmd.Parameters.Add(new SqlParameter("@fieldNames", "PO.PopName AS PopName,PO.Address AS Address,GD.RPdtCode AS RPdtCode,GD.PrmDate AS PrmDate,GD.Remark AS Remark,GD.No AS No"));
                    cmd.Parameters.Add(new SqlParameter("@fieldSort", "GD.PrmDate"));
                    var sbCondition = new StringBuilder();
                    sbCondition.Append(string.Format("{0}{1}{2}{3}{4}{5}{6}", "((GD.UserCode IN (SELECT UserCode FROM dbo.sys_user WHERE OrganizeName LIKE '", System.Web.HttpContext.Current.Session["OrganizeName"], "%') AND GD.CompCode='", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "') or ('", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode, "'='super ' ))"));
                    sbCondition.Append(" AND ");
                    SerachCondition.TextBox(sbCondition, "PopName", "PopName", "varchar");
                    SerachCondition.TextBox(sbCondition, "Address", "Address", "varchar");
                    SerachCondition.TextBox(sbCondition, "RPdtCode", "RPdtCode", "varchar");
                    SerachCondition.Date(sbCondition, "PrmDate", "PrmDate", "varchar");
                    if(sbCondition.Length > 5)
                    {
                        sbCondition.Length-=5;
                        cmd.Parameters.Add(new SqlParameter("@condition", sbCondition.ToString()));
                    }
                    if (HttpContext.Current.Request.Get("_report_") == "1")
                    {
                        // 报表请求条件合成
                        return sbCondition.ToString();
                    }
                    SqlParameter rowTotalParameter = null;
                    int rowTotal = 0;
                    int.TryParse(HttpContext.Current.Request.Get("total"), out rowTotal);
                    var rq = new PageRequest();
                    if(rq.Page.HasValue)
                    {
                        var pageSize = rq.Rows.HasValue ? rq.Rows.Value : 20;
                        var rowStart = (rq.Page.Value - 1) * pageSize + 1;
                        var rowEnd = rq.Page.Value * pageSize;
                        cmd.Parameters.Add(new SqlParameter("@rowStart", rowStart));
                        cmd.Parameters.Add(new SqlParameter("@rowEnd", rowEnd));
                        if(rowStart == 1)
                        {
                            cmd.Parameters.Add(rowTotalParameter = new SqlParameter("@rowTotal", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, true, 0, 0, null, System.Data.DataRowVersion.Default, 0));
                        }
                    }
                    string jsonArrayString = null;
                    using(var reader = cmd.ExecuteReader())
                    {
                        jsonArrayString = reader.ToJsonArrayString();
                    }
                    if (rowTotalParameter != null)
                    {
                        rowTotal = (int)rowTotalParameter.Value;
                    }
                    return JObject.Parse("{rows:" + jsonArrayString + ", total:" + rowTotal + "}");
                } finally {
                    db.Database.Connection.Close();
                }
            }
        }

    }
}
