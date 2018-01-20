using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Fruit.Web
{
    public class SerachCondition
    {
        public static void MultiTextBox(StringBuilder sbCondition, string name, string refname, string fieldType, bool appendAnd = true, bool appendOr = false)
        {
            TextBox(sbCondition, name, refname, fieldType, appendAnd, appendOr);
        }
        public static void TextBox(StringBuilder sbCondition, string name, string refname, string fieldType, bool appendAnd = true, bool appendOr = false)
        {
            var value = HttpContext.Current.Request.Get(name);
            if (!string.IsNullOrEmpty(value))
            {
                if (fieldType == "int" || fieldType == "float" || fieldType == "money")
                {
                    sbCondition.AppendFormat("{0} = {1}", refname, value);
                }
                else if(fieldType == "uniqueidentifier")
                {
                    sbCondition.AppendFormat("{0} = '{1}'", refname, ToSafeSqlString(value));
                }
                else
                {
                    sbCondition.AppendFormat("{0} like '%{1}%'", refname, ToSafeSqlString(value));
                }

                if (appendAnd)
                {
                    sbCondition.Append(" AND ");
                }
                else if (appendOr)
                {
                    sbCondition.Append("  OR ");
                }
            }            
        }
        public static void NumberBox(StringBuilder sbCondition, string name, string refname, string fieldType, bool appendAnd = true, bool appendOr = false)
        {
            var value = HttpContext.Current.Request.Get(name);
            if (!string.IsNullOrEmpty(value))
            {
                if (fieldType == "int" || fieldType == "float" || fieldType == "money")
                {
                    sbCondition.AppendFormat("{0} = {1}", refname, value);
                }
                else
                {
                    sbCondition.AppendFormat("{0} like '{1}'", refname, ToSafeSqlString(value));
                }

                if (appendAnd)
                {
                    sbCondition.Append(" AND ");
                }
                else if (appendOr)
                {
                    sbCondition.Append("  OR ");
                }
            }
        }
        public static void Guid(StringBuilder sbCondition, string name, string refname, string fieldType, bool appendAnd = true, bool appendOr = false)
        {
            var value = HttpContext.Current.Request.Get(name);
            if (!string.IsNullOrEmpty(value))
            {
                System.Guid guid;
                if (System.Guid.TryParse(value, out guid))
                {
                    sbCondition.AppendFormat("{0} = '{1}'", refname, ToSafeSqlString(value));

                    if (appendAnd)
                    {
                        sbCondition.Append(" AND ");
                    }
                    else if (appendOr)
                    {
                        sbCondition.Append("  OR ");
                    }
                }
            }
        }
        public static void Dropdown(StringBuilder sbCondition, string name, string refname, string fieldType, bool appendAnd = true, bool appendOr = false)
        {
            var value = HttpContext.Current.Request.Get(name);
            if (!string.IsNullOrEmpty(value))
            {
                if (fieldType == "int" || fieldType == "float" || fieldType == "money")
                {
                    sbCondition.AppendFormat("{0} = {1}", refname, value);
                }
                else
                {
                    sbCondition.AppendFormat("{0} = '{1}'", refname, ToSafeSqlString(value));
                }

                if (appendAnd)
                {
                    sbCondition.Append(" AND ");
                }
                else if (appendOr)
                {
                    sbCondition.Append("  OR ");
                }
            }
        }
        public static void RadioBox(StringBuilder sbCondition, string name, string refname, string fieldType, bool appendAnd = true, bool appendOr = false)
        {
            var value = HttpContext.Current.Request.Get(name);
            if (!string.IsNullOrEmpty(value))
            {
                if (fieldType == "int" || fieldType == "float" || fieldType == "money")
                {
                    sbCondition.AppendFormat("{0} = {1}", refname, value);
                }
                else
                {
                    sbCondition.AppendFormat("{0} like '%{1}%'", refname, ToSafeSqlString(value));
                }

                if (appendAnd)
                {
                    sbCondition.Append(" AND ");
                }
                else if (appendOr)
                {
                    sbCondition.Append("  OR ");
                }
            }
        }
        public static void CheckBox(StringBuilder sbCondition, string name, string refname, string fieldType, bool appendAnd = true, bool appendOr = false)
        {
            var value = HttpContext.Current.Request.Get(name);
            if (!string.IsNullOrEmpty(value))
            {
                if (fieldType == "int" || fieldType == "float" || fieldType == "money")
                {
                    sbCondition.AppendFormat("{0} = {1}", refname, value);
                }
                else
                {
                    sbCondition.AppendFormat("{0} like '%{1}%'", refname, ToSafeSqlString(value));
                }

                if (appendAnd)
                {
                    sbCondition.Append(" AND ");
                }
                else if (appendOr)
                {
                    sbCondition.Append("  OR ");
                }
            }
        }
        public static void AutoComplete(StringBuilder sbCondition, string name, string refname, string fieldType, bool appendAnd = true, bool appendOr = false)
        {
            var value = HttpContext.Current.Request.Get(name);
            if (!string.IsNullOrEmpty(value))
            {
                if (fieldType == "int" || fieldType == "float" || fieldType == "money")
                {
                    sbCondition.AppendFormat("{0} = {1}", refname, value);
                }
                else
                {
                    sbCondition.AppendFormat("{0} like '%{1}%'", refname, ToSafeSqlString(value));
                }

                if (appendAnd)
                {
                    sbCondition.Append(" AND ");
                }
                else if (appendOr)
                {
                    sbCondition.Append("  OR ");
                }
            }
        }
        public static void PopupSelect(StringBuilder sbCondition, string name, string refname, string fieldType, bool appendAnd = true, bool appendOr = false)
        {
            var value = HttpContext.Current.Request.Get(name);
            if (!string.IsNullOrEmpty(value))
            {
                if (fieldType == "int" || fieldType == "float" || fieldType == "money")
                {
                    sbCondition.AppendFormat("{0} = {1}", refname, value);
                }
                else
                {
                    sbCondition.AppendFormat("{0} like '%{1}%'", refname, ToSafeSqlString(value));
                }

                if (appendAnd)
                {
                    sbCondition.Append(" AND ");
                }
                else if (appendOr)
                {
                    sbCondition.Append("  OR ");
                }
            }
        }
        public static void PopupSelectWithMultiple(StringBuilder sbCondition, string name, string refname, string fieldType, bool appendAnd = true, bool appendOr = false)
        {
            var value = HttpContext.Current.Request.Get(name);
            if (!string.IsNullOrEmpty(value))
            {
                sbCondition.AppendFormat("({0} = '{1}' or {0} like '{1},%' or {0} like '%,{1},%' or {0} like '%,{1}')", refname, ToSafeSqlString(value));

                if (appendAnd)
                {
                    sbCondition.Append(" AND ");
                }
                else if (appendOr)
                {
                    sbCondition.Append("  OR ");
                }
            }
        }


        public static void DateTime(StringBuilder sbCondition, string name, string refname, string fieldType, bool appendAnd = true, bool appendOr = false)
        {
            Date(sbCondition, name, refname, fieldType, appendAnd, appendOr);
        }

        public static void Date(StringBuilder sbCondition, string name, string refname, string fieldType, bool appendAnd = true, bool appendOr = false)
        {
            var value = HttpContext.Current.Request.Get(name);
            if (!string.IsNullOrEmpty(value))
            {
                DateTime? start = null, end = null;
                var datePair = value.Split('到');
                if (datePair.Length == 2)
                {
                    DateTime date;
                    if (System.DateTime.TryParse(datePair[0].Trim(), out date))
                    {
                        start = date;
                    }
                    if (System.DateTime.TryParse(datePair[1].Trim(), out date))
                    {
                        end = date.AddDays(1);
                    }
                }
                else
                {
                    DateTime date;
                    if (System.DateTime.TryParse(datePair[0].Trim(), out date))
                    {
                        start = new DateTime(date.Year, date.Month, date.Day);
                        end = start.Value.AddDays(1);
                    }
                }

                if (start != null)
                {
                    sbCondition.AppendFormat("{0} >= '{1:yyyy-MM-dd}' AND ", refname, start.Value);
                }

                if (end != null)
                {
                    sbCondition.AppendFormat("{0} < '{1:yyyy-MM-dd}' AND ", refname, end.Value);
                }
            }
        }

        
        public static void Time(StringBuilder sbCondition, string name, string refname, string fieldType, bool appendAnd = true, bool appendOr = false)
        {
            var value = HttpContext.Current.Request.Get(name);
            if (!string.IsNullOrEmpty(value))
            {
                TimeSpan ts;
                if (TimeSpan.TryParse(value, out ts))
                {
                    sbCondition.AppendFormat("{0} < '{1:HH:mm:ss}' AND ", refname, ts);
                }
                else if (TimeSpan.TryParseExact(value, "HH:mm:ss", null, out ts))
                {
                    sbCondition.AppendFormat("{0} < '{1:HH-mm-ss}' AND ", refname, ts);
                }
            }
        }

        public static void SelectUser(StringBuilder sbCondition, string name, string refname, string fieldType, bool appendAnd = true, bool appendOr = false)
        {
            var value = HttpContext.Current.Request.Get(name);
            if (!string.IsNullOrEmpty(value))
            {
                sbCondition.AppendFormat("{0} in ('{1}')", refname, string.Join("','", ToSafeSqlString(value).Split(',')));

                if (appendAnd)
                {
                    sbCondition.Append(" AND ");
                }
                else if(appendOr)
                {
                    sbCondition.Append("  OR ");
                }
            }
        }

        private static string ToSafeSqlString(string value)
        {
            return value.Replace("'", "''");
        }
    }
}
