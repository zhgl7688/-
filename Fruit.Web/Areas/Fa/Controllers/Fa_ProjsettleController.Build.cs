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
    public partial class Fa_ProjsettleController : Controller
    {
        public ActionResult Index()
        {
                return View();
        }

    }
    public partial class Fa_ProjsettleApiController : ApiController
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
                    cmd.Parameters.Add(new SqlParameter("@tableName", "Settlement_Report"));
                    cmd.Parameters.Add(new SqlParameter("@fieldNames", "Settlement_Report.no AS no,Settlement_Report.开票日期 AS 开票日期,Settlement_Report.开票摘要 AS 开票摘要,Settlement_Report.号码 AS 号码,Settlement_Report.价税合计 AS 价税合计,Settlement_Report.金额3 AS 金额3,Settlement_Report.金额11 AS 金额11,Settlement_Report.应收税管费 AS 应收税管费,Settlement_Report.日期 AS 日期,Settlement_Report.收付款摘要 AS 收付款摘要,Settlement_Report.收款 AS 收款,Settlement_Report.支付 AS 支付,Settlement_Report.上期结余 AS 上期结余,Settlement_Report.负责人编号 AS CustID,Settlement_Report.年 AS Dates"));
                    cmd.Parameters.Add(new SqlParameter("@fieldSort", "no"));
                    var sbCondition = new StringBuilder();
                    SerachCondition.PopupSelect(sbCondition, "CustID", "CustID", "");
                    SerachCondition.TextBox(sbCondition, "Dates", "Dates", "");
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
