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

namespace Fruit.Web.Areas.Project.Controllers
{
    public partial class Fa_DepositController : Controller
    {
        public ActionResult Index(string PID)
        {
            Session["PIDS"] = PID;

                // 提供搜索下拉框数据源
                List<ComboItem> depAmtStatus;
                using(var db = new SysContext())
                {
                    depAmtStatus = db.Database.SqlQuery<ComboItem>("select Text Text, Text Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='depAmt'")).ToList();
                }
                return View(new {dataSource = new {depAmtStatus}});
        }

    }
    public partial class Fa_DepositApiController : ApiController
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
                    cmd.Parameters.Add(new SqlParameter("@tableName", "FA_Deposit "));
                    cmd.Parameters.Add(new SqlParameter("@fieldNames", "FA_Deposit .FID AS FID,FA_Deposit .PID AS PID,FA_Deposit .depAmtStatus AS depAmtStatus,FA_Deposit .depAmt AS depAmt,FA_Deposit .CreatorID AS CreatorID,FA_Deposit .CreatedTime AS CreatedTime"));
                    cmd.Parameters.Add(new SqlParameter("@fieldSort", "CreatedTime desc"));
                    var sbCondition = new StringBuilder();



                    var PIDS = HttpContext.Current.Session["PIDS"].ToString();
                    if (!string.IsNullOrEmpty(PIDS))
                    {
                        sbCondition.AppendFormat("{0} like '%{1}%'", "PID", PIDS);
                        sbCondition.Append(" AND ");

                    }

                    SerachCondition.TextBox(sbCondition, "PID", "PID", "");
                    SerachCondition.Dropdown(sbCondition, "depAmtStatus", "depAmtStatus", "");
                    SerachCondition.TextBox(sbCondition, "depAmt", "depAmt", "");
                    SerachCondition.Date(sbCondition, "CreatedTime", "CreatedTime", "");
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
