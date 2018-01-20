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

namespace Fruit.Web.Areas.Fa.Controllers
{
    public partial class RinvoiceController : Controller
    {
        public ActionResult Index()
        {
                // 提供搜索下拉框数据源
                List<ComboItem> 分部负责人, 公司;
                using(var db = new LUOLAI1401Context())
                {
                    分部负责人 = db.BD_Customers.Select(i=>new ComboItem{ Text = i.Contact, Value = i.Contact }).ToList();
                }
                using(var db = new SysContext())
                {
                    公司 = db.Database.SqlQuery<ComboItem>("select Text Text, Text Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='HtCorp'")).ToList();
                }
                return View(new {dataSource = new {分部负责人,公司}});
        }

    }
    public partial class RinvoiceApiController : ApiController
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
                    cmd.Parameters.Add(new SqlParameter("@tableName", "R_Invoices"));
                    cmd.Parameters.Add(new SqlParameter("@fieldNames", "R_Invoices.公司 AS 公司,R_Invoices.发票号 AS 发票号,R_Invoices.分部负责人 AS 分部负责人,R_Invoices.开票日期 AS 开票日期"));
                    cmd.Parameters.Add(new SqlParameter("@fieldSum", "SUM(R_Invoices.金额) AS 金额"));
                    cmd.Parameters.Add(new SqlParameter("@fieldSort", "开票日期"));
                    var sbCondition = new StringBuilder();
                    SerachCondition.Dropdown(sbCondition, "公司", "公司", "");
                    SerachCondition.Dropdown(sbCondition, "分部负责人", "分部负责人", "");
                    SerachCondition.Date(sbCondition, "开票日期", "开票日期", "");
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
                    if (!rq.Page.HasValue || rq.Page.Value > 1)
                        return JObject.Parse("{rows:" + jsonArrayString + ", total:" + rowTotal + "}");
                    // 合计值请求
                    cmd = db.Database.Connection.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "proc_SearchList";
                    cmd.Parameters.Add(new SqlParameter("@sumField", 1));
                    cmd.Parameters.Add(new SqlParameter("@tableName", "R_Invoices"));
                    cmd.Parameters.Add(new SqlParameter("@fieldSum", "SUM(R_Invoices.金额) AS 金额"));
                    if(sbCondition.Length > 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@condition", sbCondition.ToString()));
                    }
                    using(var reader = cmd.ExecuteReader())
                    {
                        return JObject.Parse("{rows:" + jsonArrayString + ", total:" + rowTotal + ", footer:" + reader.ToJsonArrayString() + "}");
                    }
                } finally {
                    db.Database.Connection.Close();
                }
            }
        }

    }
}
