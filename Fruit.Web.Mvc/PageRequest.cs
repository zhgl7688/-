using Fruit.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Fruit.Web
{
    public class PageRequest
    {
        public PageRequest()
        {
            //Page = 1;
            //Rows = 20;
            //Order = "asc";

            var qs = HttpContext.Current.Request.QueryString;
            if (qs["page"] != null)
            {
                Page = int.Parse(qs["page"]);
            }
            if (qs["rows"] != null)
            {
                Rows = int.Parse(qs["rows"]);
            }
            if (qs["sort"] != null)
            {
                Sort = qs["sort"];
            }
            if (qs["order"] != null)
            {
                Order = qs["order"];
            }
        }

        public int? Page { get; set; }
        public int? Rows { get; set; }
        public string Sort { get; set; }
        public string Order { get; set; }

        public int RowStart
        {
            get { return (Page.Value - 1) * Rows.Value + 1; }
        }

        public int RowEnd
        {
            get { return Page.Value * Rows.Value; }
        }

        public IQueryable<TModel> ApplyQuery<TModel>(IQueryable<TModel> query, string defaultSort, string defaultOrder = "asc")
        {
            IQueryable<TModel> rQuery = query;
            var funcType = typeof(Func<,>).MakeGenericType(typeof(TModel), typeof(TModel).GetProperty(defaultSort).PropertyType);
            var expType = typeof(System.Linq.Expressions.Expression<>).MakeGenericType(funcType);
            var expSort = ExpressionExtension.CreateKeySelector(typeof(TModel), Sort ?? defaultSort);
            if (Order == "desc" || string.IsNullOrEmpty(Order) && defaultOrder == "desc")
            {
                var orderMethod = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);
                orderMethod = orderMethod.MakeGenericMethod(typeof(TModel), typeof(TModel).GetProperty(defaultSort).PropertyType);
                rQuery = (IQueryable<TModel>)orderMethod.Invoke(null, new object[] { query, expSort });
            }
            else
            {
                var orderMethod = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
                orderMethod = orderMethod.MakeGenericMethod(typeof(TModel), typeof(TModel).GetProperty(defaultSort).PropertyType);
                rQuery = (IQueryable<TModel>)orderMethod.Invoke(null, new object[]{query, expSort});
            }
            if (Page.HasValue && Page.Value > 1 && Rows.HasValue && Rows.Value > 1)
            {
                rQuery = rQuery.Skip((Page.Value - 1) * Rows.Value).Take(Rows.Value);
            }
            else if(Rows.HasValue && Rows.Value > 1)
            {
                rQuery = rQuery.Take(Rows.Value);
            }
            return rQuery;
        }

        public IQueryable<TModel> ApplyQuery<TModel>(IQueryable<TModel> query, string defaultSort, string defaultOrder, out int total)
        {
            total = query.Count();
            return ApplyQuery(query, defaultSort, defaultOrder);
        }

        public PageList<TModel> ToPageList<TModel>(IQueryable<TModel> query, string defaultSort, string defaultOrder = "asc")
        {
            PageList<TModel> list = new PageList<TModel>();
            int total;
            list.Rows = ApplyQuery(query, defaultSort, defaultOrder, out total).ToList();
            list.Total = total;
            return list;
        }

        public PageList<TModel> ToPageList<TModel>(Database database, string select, string from, string where, string defaultSort, string defaultOrder = "asc", params object[] parameters)
        {
            if (!string.IsNullOrEmpty(where))
            {
                where = " and " + where;
            }
            PageList<TModel> list = new PageList<TModel>();
            var parameters2 = new List<object>();
            foreach (ICloneable c in parameters)
            {
                parameters2.Add(c.Clone());
            }
            list.Total = database.SqlQuery<int>(string.Format("select count(*) from {0} where 1=1 {1}", from, where), parameters).FirstOrDefault();
            if (list.Total == 0)
            {
                // 无数据，不需要再次进行库查询
                list.Rows = new List<TModel>();
            }
            else
            {
                string sql = null;
                if (Page.HasValue)
                {
                    // 分页查询处理
                    if (Page == 1)
                    {
                        // 第一页查询处理
                        sql = string.Format("select TOP {5} {0} from {1} where 1=1 {2} order by {3} {4}", select, from, where, Sort ?? defaultSort, Order ?? defaultOrder, RowEnd);
                    }
                    else
                    {
                        sql = string.Format("SELECT * FROM (select {0},ROW_NUMBER() OVER (order by {3} {4}) AS ROWID from {1} where 1=1 {2}) t WHERE ROWID>={5} AND ROWID<={6} ORDER BY ROWID", select, from, where, Sort ?? defaultSort, Order ?? defaultOrder, RowStart, RowEnd);
                    }
                }
                else
                {
                    // 不分页查询处理
                    sql = string.Format("select {0} from {1} where 1=1 {2} order by {3} {4}", select, from, where, Sort ?? defaultSort, Order ?? defaultOrder);
                }
                list.Rows = database.SqlQuery<TModel>(sql, parameters2.ToArray()).ToList();
            }
            return list;
        }

        public class PageList<TModel> : IPageList
        {
            [Newtonsoft.Json.JsonProperty("total")]
            public int Total { get; set; }

            [Newtonsoft.Json.JsonProperty("rows")]
            public IList<TModel> Rows { get; set; }

            public IEnumerator GetEnumerator()
            {
                return Rows.GetEnumerator();
            }

            public IEnumerable GetEnumerable()
            {
                return Rows;
            }
        }

        public interface IPageList
        {
            IEnumerable GetEnumerable();
        }
    }
}
