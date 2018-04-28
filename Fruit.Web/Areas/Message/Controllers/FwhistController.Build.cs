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

namespace Fruit.Web.Areas.Message.Controllers
{
    public partial class FwhistController : Controller
    {
        public ActionResult Index()
        {
                return View();
        }

    }
    public partial class FwhistApiController : ApiController
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
                    cmd.Parameters.Add(new SqlParameter("@tableName", "Rinout_hist"));
                    cmd.Parameters.Add(new SqlParameter("@fieldNames", "Rinout_hist.会员手机 AS 会员手机,Rinout_hist.公司名 AS 公司名,Rinout_hist.订单号 AS 订单号,Rinout_hist.操作名称 AS 操作名称,Rinout_hist.冻结变化 AS 冻结变化,Rinout_hist.余额变化 AS 余额变化,Rinout_hist.最后冻结 AS 最后冻结,Rinout_hist.最后余额 AS 最后余额,Rinout_hist.日期 AS 日期"));
                    cmd.Parameters.Add(new SqlParameter("@fieldSort", "日期"));
                    var sbCondition = new StringBuilder();
                    SerachCondition.TextBox(sbCondition, "会员手机", "会员手机", "");
                    SerachCondition.TextBox(sbCondition, "公司名", "公司名", "");
                    SerachCondition.TextBox(sbCondition, "订单号", "订单号", "");
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
