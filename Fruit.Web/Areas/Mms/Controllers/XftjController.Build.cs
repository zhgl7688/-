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
    public partial class XftjController : Controller
    {
        public ActionResult Index()
        {
                // 提供搜索下拉框数据源
                List<ComboItem> 部门;
                using(var db = new SysContext())
                {
                    部门 = db.Database.SqlQuery<ComboItem>("select OrganizeName Text, OrganizeCode Value from sys_organize where " + string.Format("{0}{1}{2}", "CompCode='", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "'")).ToList();
                }
                return View(new {dataSource = new {部门}});
        }

    }
    public partial class XftjApiController : ApiController
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
                    cmd.Parameters.Add(new SqlParameter("@tableName", "KZ_xftj"));
                    cmd.Parameters.Add(new SqlParameter("@fieldNames", "KZ_xftj.部门 AS 部门,KZ_xftj.巡店日期 AS 巡店日期,KZ_xftj.所属角色 AS 所属角色,KZ_xftj.员工编号 AS 员工编号,KZ_xftj.员工姓名 AS 员工姓名,KZ_xftj.计划数 AS 计划数,KZ_xftj.计划外数 AS 计划外数,KZ_xftj.进店完成数 AS 进店完成数,KZ_xftj.生动化拍照 AS 生动化拍照,KZ_xftj.活动拍照 AS 活动拍照,KZ_xftj.库存销量 AS 库存销量,KZ_xftj.订单采集 AS 订单采集,KZ_xftj.竞品上报 AS 竞品上报,KZ_xftj.离店完成数 AS 离店完成数"));
                    cmd.Parameters.Add(new SqlParameter("@fieldSort", "巡店日期"));
                    var sbCondition = new StringBuilder();
                    sbCondition.Append(string.Format("{0}{1}{2}{3}{4}{5}{6}", "((员工编号 IN (SELECT UserCode FROM dbo.sys_user WHERE OrganizeName LIKE '", System.Web.HttpContext.Current.Session["OrganizeName"], "%')AND 所属公司='", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "') or ('", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode, "'='super'))"));
                    sbCondition.Append(" AND ");
                    SerachCondition.Dropdown(sbCondition, "部门", "部门", "varchar");
                    SerachCondition.Date(sbCondition, "巡店日期", "巡店日期", "varchar");
                    SerachCondition.TextBox(sbCondition, "所属角色", "所属角色", "varchar");
                    SerachCondition.TextBox(sbCondition, "员工编号", "员工编号", "varchar");
                    SerachCondition.TextBox(sbCondition, "员工姓名", "员工姓名", "varchar");
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
