/// 注意：本文件由 FruitBuilder 生成和管理，请误手工更改
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
    public partial class KqjlController : Controller
    {
        public ActionResult Index()
        {
                return View();
        }

    }
    public partial class KqjlApiController : ApiController
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
                    cmd.Parameters.Add(new SqlParameter("@tableName", "kz_kqjl"));
                    cmd.Parameters.Add(new SqlParameter("@fieldNames", "kz_kqjl.月份 AS 月份,kz_kqjl.OrganizeName AS OrganizeName,kz_kqjl.用户编号 AS 用户编号,kz_kqjl.用户名 AS 用户名,kz_kqjl.Ktype AS Ktype,kz_kqjl.一号 AS 一号,kz_kqjl.二号 AS 二号,kz_kqjl.三号 AS 三号,kz_kqjl.四号 AS 四号,kz_kqjl.五号 AS 五号,kz_kqjl.六号 AS 六号,kz_kqjl.七号 AS 七号,kz_kqjl.八号 AS 八号,kz_kqjl.九号 AS 九号,kz_kqjl.十号 AS 十号,kz_kqjl.十一号 AS 十一号,kz_kqjl.十二号 AS 十二号,kz_kqjl.十三号 AS 十三号,kz_kqjl.十四号 AS 十四号,kz_kqjl.十五号 AS 十五号,kz_kqjl.十六号 AS 十六号,kz_kqjl.十七号 AS 十七号,kz_kqjl.十八号 AS 十八号,kz_kqjl.十九号 AS 十九号,kz_kqjl.二十号 AS 二十号,kz_kqjl.二十一号 AS 二十一号,kz_kqjl.二十二号 AS 二十二号,kz_kqjl.二十三号 AS 二十三号,kz_kqjl.二十四号 AS 二十四号,kz_kqjl.二十五号 AS 二十五号,kz_kqjl.二十六号 AS 二十六号,kz_kqjl.二十七号 AS 二十七号,kz_kqjl.二十八号 AS 二十八号,kz_kqjl.二十九号 AS 二十九号,kz_kqjl.三十号 AS 三十号,kz_kqjl.三十一号 AS 三十一号"));
                    cmd.Parameters.Add(new SqlParameter("@fieldSort", "用户名,Ktype"));
                    var sbCondition = new StringBuilder();
                    sbCondition.Append(string.Format("{0}{1}{2}{3}{4}{5}{6}", "((用户编号 IN (SELECT UserCode FROM dbo.sys_user WHERE OrganizeName LIKE '", System.Web.HttpContext.Current.Session["OrganizeName"], "%')AND 所属公司='", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "') or ('", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode, "'='super'))"));
                    sbCondition.Append(" AND ");
                    SerachCondition.TextBox(sbCondition, "月份", "月份", "");
                    SerachCondition.TextBox(sbCondition, "OrganizeName", "OrganizeName", "");
                    SerachCondition.TextBox(sbCondition, "用户编号", "用户编号", "");
                    SerachCondition.TextBox(sbCondition, "用户名", "用户名", "");
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
