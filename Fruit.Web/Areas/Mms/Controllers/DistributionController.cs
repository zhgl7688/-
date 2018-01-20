using Fruit.Web.Models;
using Newtonsoft.Json.Linq;
using System;
using Fruit.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Fruit.Web.Areas.Mms.Controllers
{
    public class DistributionController : Controller
    {
        // GET: Mms/Distribution
        public ActionResult Index()
        {
            return View();
        }
    }
    public class DistributionApiController : ApiController
    {
        public object Get(JObject req)
        {
            using (var db = new LUOLAI1401Context())
            {
                try
                {
                    // 准备输入条件
                    var sbCondition = new StringBuilder();
                    ArrayList parameters = new ArrayList();

                    var getValue = HttpContext.Current.Request.Get("UserCode", HttpRequestGetMode.GetOnly);
                    if (!string.IsNullOrEmpty(getValue))
                    {
                        sbCondition.Append("Distribution.UserCode = @userCode AND ");
                        parameters.Add(new System.Data.SqlClient.SqlParameter("@userCode", getValue));
                    }

                    getValue = HttpContext.Current.Request.Get("UserName", HttpRequestGetMode.GetOnly);
                    if (!string.IsNullOrEmpty(getValue))
                    {
                        sbCondition.Append("Distribution.UserName = @userName AND ");
                        parameters.Add(new System.Data.SqlClient.SqlParameter("@userName", getValue));
                    }

                    getValue = HttpContext.Current.Request.Get("Time", HttpRequestGetMode.GetOnly);
                    if (!string.IsNullOrEmpty(getValue))
                    {
                        DateTime? start = null, end = null;
                        var datePair = getValue.Split('到');
                        if (datePair.Length == 2)
                        {
                            DateTime date;
                            if (DateTime.TryParseExact(datePair[0].Trim(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out date))
                            {
                                start = date;
                            }
                            if (DateTime.TryParseExact(datePair[1].Trim(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out date))
                            {
                                end = date.AddDays(1);
                            }
                        }
                        else
                        {
                            DateTime date;
                            if (DateTime.TryParseExact(datePair[0].Trim(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out date))
                            {
                                start = new DateTime(date.Year, date.Month, date.Day);
                                end = start.Value.AddDays(1);
                            }
                        }

                        if (start != null)
                        {
                            sbCondition.Append("Distribution.CreateDate >= @start AND ");
                            parameters.Add(new System.Data.SqlClient.SqlParameter("@start", start.Value));
                        }

                        if (end != null)
                        {
                            sbCondition.Append("Distribution.CreateDate < @end AND ");
                            parameters.Add(new System.Data.SqlClient.SqlParameter("@end", end.Value));
                        }
                    }

                    sbCondition.Append("(Distribution.CompCode ='" + System.Web.HttpContext.Current.Session["CompCode"] + "' or 'super'='" + ((sys_user)System.Web.HttpContext.Current.Session["sys_user"]).UserCode + "') and");
                    if (sbCondition.Length > 4) sbCondition.Length -= 4;

                    // 分页组件初始化
                    PageRequest pr = new PageRequest();
                    return pr.ToPageList<AttendTrack>(db.Database, "Distribution.UserCode,Distribution.UserName,Distribution.CreateDate,TckLong,TckLat,Address,Distribution.Mobile,Distribution.OrganizeName", "Distribution INNER JOIN attendTrack_v ON dbo.attendTrack_v.UserCode = dbo.Distribution.UserCode AND dbo.attendTrack_v.CreateDate = dbo.Distribution.CreateDate AND dbo.attendTrack_v.CompCode = dbo.Distribution.CompCode", sbCondition.ToString(), "Distribution.CreateDate", "asc", parameters.ToArray());

                }
                finally
                {
                    db.Database.Connection.Close();
                }
            }
        }
    }
}