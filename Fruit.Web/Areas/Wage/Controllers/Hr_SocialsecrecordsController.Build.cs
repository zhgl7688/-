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
    public partial class Hr_SocialsecrecordsController : Controller
    {
        public ActionResult Index()
        {
                // 提供搜索下拉框数据源
                List<ComboItem> SSType;
                using(var db = new SysContext())
                {
                    SSType = db.Database.SqlQuery<ComboItem>("select Text Text, Value Value from sys_code where " + string.Format("{0}", "/*TABLEALIAS*/CodeType='FType'")).ToList();
                }
                return View(new {dataSource = new {SSType}});
        }

    }
    public partial class Hr_SocialsecrecordsApiController : ApiController
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
                    cmd.Parameters.Add(new SqlParameter("@tableName", "HR_SocialSecRecords"));
                    cmd.Parameters.Add(new SqlParameter("@fieldNames", "HR_SocialSecRecords.FYear AS FYear,HR_SocialSecRecords.FMonth AS FMonth,HR_SocialSecRecords.empCode AS empCode,HR_SocialSecRecords.SSType AS SSType,HR_SocialSecRecords.SSAmt AS SSAmt,HR_SocialSecRecords.indvPension AS indvPension,HR_SocialSecRecords.indvMedical AS indvMedical,HR_SocialSecRecords.indvUnemploy AS indvUnemploy,HR_SocialSecRecords.corpPension AS corpPension,HR_SocialSecRecords.corpBaseMedical AS corpBaseMedical,HR_SocialSecRecords.corpLocalMedical AS corpLocalMedical,HR_SocialSecRecords.corpUnemploy AS corpUnemploy,HR_SocialSecRecords.corpInjury AS corpInjury,HR_SocialSecRecords.corpBirth AS corpBirth,HR_SocialSecRecords.indvTotal AS indvTotal,HR_SocialSecRecords.corpTotal AS corpTotal,HR_SocialSecRecords.Total AS Total,HR_SocialSecRecords.Remark AS Remark"));
                    cmd.Parameters.Add(new SqlParameter("@fieldSort", "FMonth,FYear desc"));
                    var sbCondition = new StringBuilder();
                    SerachCondition.TextBox(sbCondition, "FYear", "FYear", "");
                    SerachCondition.TextBox(sbCondition, "FMonth", "FMonth", "");
                    SerachCondition.PopupSelect(sbCondition, "empCode", "empCode", "");
                    SerachCondition.Dropdown(sbCondition, "SSType", "SSType", "");
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
