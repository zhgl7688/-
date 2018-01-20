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

namespace Fruit.Web.Areas.Wage.Controllers
{
    public partial class Hr_HousfundrecordsController : Controller
    {
        public ActionResult Index()
        {
                // 提供搜索下拉框数据源
                List<ComboItem> HFType;
                using(var db = new SysContext())
                {
                    HFType = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='FType'")).ToList();
                }
                return View(new {dataSource = new {HFType}});
        }

    }
    public partial class Hr_HousfundrecordsApiController : ApiController
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
                    cmd.Parameters.Add(new SqlParameter("@tableName", "HR_HousFundRecords"));
                    cmd.Parameters.Add(new SqlParameter("@fieldNames", "HR_HousFundRecords.FID AS FID,HR_HousFundRecords.FYear AS FYear,HR_HousFundRecords.FMonth AS FMonth,HR_HousFundRecords.HFCode AS HFCode,HR_HousFundRecords.HFType AS HFType,HR_HousFundRecords.empCode AS empCode,HR_HousFundRecords.corpFund AS corpFund,HR_HousFundRecords.indvFund AS indvFund,HR_HousFundRecords.HFAmt AS HFAmt,HR_HousFundRecords.Total AS Total,HR_HousFundRecords.CreatePerson AS CreatePerson,HR_HousFundRecords.Remark AS Remark,HR_HousFundRecords.CreateDate AS CreateDate"));
                    cmd.Parameters.Add(new SqlParameter("@fieldSort", "FMonth,FYear desc"));
                    var sbCondition = new StringBuilder();
                    SerachCondition.TextBox(sbCondition, "FID", "FID", "");
                    SerachCondition.TextBox(sbCondition, "FYear", "FYear", "");
                    SerachCondition.TextBox(sbCondition, "FMonth", "FMonth", "");
                    SerachCondition.TextBox(sbCondition, "HFCode", "HFCode", "");
                    SerachCondition.Dropdown(sbCondition, "HFType", "HFType", "");
                    SerachCondition.PopupSelect(sbCondition, "empCode", "empCode", "");
                    SerachCondition.Date(sbCondition, "CreateDate", "CreateDate", "");
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
