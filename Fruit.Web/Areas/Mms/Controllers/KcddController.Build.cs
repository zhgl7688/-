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
    public partial class KcddController : Controller
    {
        public ActionResult Index()
        {
                // 提供搜索下拉框数据源
                List<ComboItem> 部门, 客户类型;
                using(var db = new SysContext())
                {
                    部门 = db.Database.SqlQuery<ComboItem>("select OrganizeName Text, OrganizeCode Value from sys_organize where " + string.Format("{0}{1}{2}", "CompCode='", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "'")).ToList();
                    客户类型 = db.Database.SqlQuery<ComboItem>("select Text Text, Text Value from sys_code where " + string.Format("{0}", "CodeType='Kz_zd'")).ToList();
                }
                return View(new {dataSource = new {部门,客户类型}});
        }

    }
    public partial class KcddApiController : ApiController
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
                    cmd.Parameters.Add(new SqlParameter("@tableName", "KZ_kcdd"));
                    cmd.Parameters.Add(new SqlParameter("@fieldNames", "KZ_kcdd.巡访类型 AS 巡访类型,KZ_kcdd.所属角色 AS 所属角色,KZ_kcdd.部门 AS 部门,KZ_kcdd.员工编号 AS 员工编号,KZ_kcdd.员工姓名 AS 员工姓名,KZ_kcdd.客户编号 AS 客户编号,KZ_kcdd.客户名称 AS 客户名称,KZ_kcdd.客户类型 AS 客户类型,KZ_kcdd.经度 AS 经度,KZ_kcdd.纬度 AS 纬度,KZ_kcdd.巡访日期 AS 巡访日期,KZ_kcdd.备注 AS 备注"));
                    cmd.Parameters.Add(new SqlParameter("@fieldSort", "巡访日期"));
                    var sbCondition = new StringBuilder();
                    sbCondition.Append(string.Format("{0}{1}{2}{3}{4}{5}{6}", "((员工编号 IN (SELECT UserCode FROM dbo.sys_user WHERE OrganizeName LIKE '", System.Web.HttpContext.Current.Session["OrganizeName"], "%')AND 所属公司='", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "') or ('", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode, "'='super'))"));
                    sbCondition.Append(" AND ");
                    SerachCondition.Dropdown(sbCondition, "部门", "部门", "varchar");
                    SerachCondition.TextBox(sbCondition, "员工编号", "员工编号", "varchar");
                    SerachCondition.TextBox(sbCondition, "员工姓名", "员工姓名", "varchar");
                    SerachCondition.TextBox(sbCondition, "客户编号", "客户编号", "varchar");
                    SerachCondition.TextBox(sbCondition, "客户名称", "客户名称", "varchar");
                    SerachCondition.Dropdown(sbCondition, "客户类型", "客户类型", "varchar");
                    SerachCondition.Date(sbCondition, "巡访日期", "巡访日期", "varchar");
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
