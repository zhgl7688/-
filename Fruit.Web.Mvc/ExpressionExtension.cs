using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Fruit.Web
{
    public static class MvcExpressionExtension
    {
        public static IQueryable<TModel> DateRange<TModel>(this IQueryable<TModel> queryable, string name, string dateRangeExpression)
        {
            if (dateRangeExpression == null) return queryable;
            dateRangeExpression = dateRangeExpression.Trim();

            var dateParts = dateRangeExpression.Split(new string[] { "到" }, StringSplitOptions.None);

            DateTime? date1 = new DateTime();
            DateTime? date2 = new DateTime();

            if (dateParts.Length >= 1)
            {
                if (dateParts.Length == 1)
                {
                    date1 = DateTime.Parse(dateParts[0]);
                    date2 = date1.Value.AddDays(1);
                }
                else if (dateParts.Length > 1)
                {
                    date1 = dateParts[0].Trim().Length > 1 ? DateTime.Parse(dateParts[0]) : (DateTime?)null;
                    date2 = dateParts[1].Trim().Length > 1 ? DateTime.Parse(dateParts[1]).AddDays(1) : (DateTime?)null;
                }

                bool isNullable = false;
                if (typeof(Nullable<DateTime>) == typeof(TModel).GetProperty(name).PropertyType)
                {
                    isNullable = true;
                }


                var paramExp = Expression.Parameter(typeof(TModel));
                Expression propertyInvoke = Expression.PropertyOrField(paramExp, name);
                if (isNullable)
                {
                    propertyInvoke = Expression.PropertyOrField(propertyInvoke, "Value");
                }
                var start = Expression.GreaterThanOrEqual(propertyInvoke, Expression.Constant(date1.HasValue ? date1.Value : DateTime.Now));
                var end = Expression.LessThan(propertyInvoke, Expression.Constant(date2.HasValue ? date2.Value : DateTime.Now));
                if (date1.HasValue && date2.HasValue)
                {
                    var func = Expression.And(start, end);
                    var exp = Expression.Lambda<Func<TModel, bool>>(func, paramExp);
                    queryable = queryable.Where(exp);
                }
                else if (date1.HasValue)
                {
                    var exp = Expression.Lambda<Func<TModel, bool>>(start, paramExp);
                    queryable = queryable.Where(exp);
                }
                else if(date2.HasValue)
                {
                    var exp = Expression.Lambda<Func<TModel, bool>>(end, paramExp);
                    queryable = queryable.Where(exp);
                }
            }
            return queryable;
        }


        //public static IQueryable<TModel> Condition<TModel>(this IQueryable<TModel> queryable, Dictionary<string, Expression<Func<TModel, string, bool>>> expressions)
        //{
        //    foreach (var kv in expressions)
        //    {
        //        string value = HttpContext.Current.Request[kv.Key];
        //        if (string.IsNullOrEmpty(value))
        //        {
        //            queryable = queryable.Where(kv.Value);
        //        }
        //    }
        //    return queryable;
        //}
    }
}
