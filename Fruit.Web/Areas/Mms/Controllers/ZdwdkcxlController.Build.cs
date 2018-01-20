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
    public partial class ZdwdkcxlController : Controller
    {
        public ActionResult Index()
        {
                // 提供搜索下拉框数据源
                List<ComboItem> 终端类型;
                using(var db = new SysContext())
                {
                    终端类型 = db.Database.SqlQuery<ComboItem>("select Text Text, Text Value from sys_code where " + string.Format("{0}", "CodeType='Kz_zd'")).ToList();
                }
                return View(new {dataSource = new {终端类型}});
        }

    }
    public partial class ZdwdkcxlApiController : ApiController
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
                    cmd.Parameters.Add(new SqlParameter("@tableName", "zdwdxl"));
                    cmd.Parameters.Add(new SqlParameter("@fieldNames", "zdwdxl.终端编号 AS 终端编号,zdwdxl.终端名称 AS 终端名称,zdwdxl.联系人 AS 联系人,zdwdxl.联系电话 AS 联系电话,zdwdxl.联系手机 AS 联系手机,zdwdxl.地址 AS 地址,zdwdxl.老板名 AS 老板名,zdwdxl.老板电话 AS 老板电话,zdwdxl.老板手机 AS 老板手机,zdwdxl.终端类型 AS 终端类型,zdwdxl.结算方式 AS 结算方式,zdwdxl.商超营业面积 AS 商超营业面积,zdwdxl.采购方式 AS 采购方式,zdwdxl.上座率 AS 上座率,zdwdxl.平均消费 AS 平均消费,zdwdxl.进店类型 AS 进店类型,zdwdxl.包间数 AS 包间数,zdwdxl.流通营业面积 AS 流通营业面积,zdwdxl.门面数 AS 门面数,zdwdxl.酒水容量 AS 酒水容量,zdwdxl.团购金额 AS 团购金额,zdwdxl.业务员编号 AS 业务员编号,zdwdxl.业务员名称 AS 业务员名称"));
                    cmd.Parameters.Add(new SqlParameter("@fieldSort", "zdwdxl.终端编号"));
                    var sbCondition = new StringBuilder();
                    sbCondition.Append(string.Format("{0}{1}{2}{3}{4}{5}{6}", "((业务员编号 IN (SELECT UserCode FROM dbo.sys_user WHERE OrganizeName LIKE '", System.Web.HttpContext.Current.Session["OrganizeName"], "%')AND 所属公司='", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode, "') or ('", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).UserCode, "'='super'))"));
                    sbCondition.Append(" AND ");
                    SerachCondition.TextBox(sbCondition, "终端名称", "终端名称", "");
                    SerachCondition.TextBox(sbCondition, "联系人", "联系人", "");
                    SerachCondition.TextBox(sbCondition, "联系手机", "联系手机", "");
                    SerachCondition.Dropdown(sbCondition, "终端类型", "终端类型", "");
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
