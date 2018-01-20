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
    public class AttendTrackStController : Controller
    {
        // GET: Mms/AttendTrackSt
        public ActionResult Index()
        {
            return View();
        }
    }


    public class AttendTrackStApiController : ApiController
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

                    var getValue = HttpContext.Current.Request.Get("OrganizeName", HttpRequestGetMode.GetOnly);
                    if (!string.IsNullOrEmpty(getValue))
                    {
                        sbCondition.Append("OrganizeName = @OrganizeName AND ");
                        parameters.Add(new System.Data.SqlClient.SqlParameter("@OrganizeName", getValue));
                    }
                    getValue = HttpContext.Current.Request.Get("UserCode", HttpRequestGetMode.GetOnly);
                    if (!string.IsNullOrEmpty(getValue))
                    {
                        sbCondition.Append("UserCode = @userCode AND ");
                        parameters.Add(new System.Data.SqlClient.SqlParameter("@userCode", getValue));
                    }

                    getValue = HttpContext.Current.Request.Get("UserName", HttpRequestGetMode.GetOnly);
                    if (!string.IsNullOrEmpty(getValue))
                    {
                        sbCondition.Append("UserName = @userName AND ");
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
                            sbCondition.Append("CreateDate >= @start AND ");
                            parameters.Add(new System.Data.SqlClient.SqlParameter("@start", start.Value));
                        }

                        if (end != null)
                        {
                            sbCondition.Append("CreateDate < @end AND ");
                            parameters.Add(new System.Data.SqlClient.SqlParameter("@end", end.Value));
                        }
                    }

                    sbCondition.Append("(CompCode ='" + System.Web.HttpContext.Current.Session["CompCode"] + "' or 'super'='" + ((sys_user)System.Web.HttpContext.Current.Session["sys_user"]).UserCode + "') and");
                    if (sbCondition.Length > 4) sbCondition.Length -= 4;

                    // 分页组件初始化
                    PageRequest pr = new PageRequest();
                    return pr.ToPageList<AttendTrack>(db.Database, "*", "attendTrack_Statistics", sbCondition.ToString(), "CreateDate", "asc", parameters.ToArray());

                }
                finally
                {
                    db.Database.Connection.Close();
                }
            }
        }
    }
}