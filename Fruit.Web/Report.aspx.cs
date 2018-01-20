using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fruit.Web
{
    public partial class Report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebReport1.ReportFile = Server.MapPath("~/App_Data/Reports/" + Request["ReportFile"]);
        }

        protected void WebReport1_StartReport(object sender, EventArgs e)
        {
            var report = WebReport1.Report;
            if (report != null)
            {
                foreach (FastReport.Data.Parameter param in report.Parameters)
                {
                    param.Value = GetValueForType(param.Name, param.DataType);
                    if (param.Value == null && param.Name == "LoginedUserCode")
                    {
                        param.Value = User.Identity.Name;
                    }
                }
            }
        }

        private object GetValueForType(string name, Type type)
        {
            var stringVal = Request[name];
            if (type == typeof(string))
            {
                return stringVal;
            }
            if (type == typeof(Guid) && !string.IsNullOrEmpty(stringVal))
            {
                return Guid.Parse(stringVal);
            }
            if (type == typeof(Int32) && !string.IsNullOrEmpty(stringVal))
            {
                return Int32.Parse(stringVal);
            }

            return null;
        }
    }
}