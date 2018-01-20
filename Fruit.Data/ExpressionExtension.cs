using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Fruit;

namespace Fruit.Data
{
    public static class ExpressionExtension
    {
        public static int Remove<TModel>(this DbSet<TModel> dbSet, Expression<Func<TModel, bool>> predicate)
            where TModel : class
        {
            return Remove<TModel>(dbSet.Where(predicate));
        }

        public static int Remove<T>(this IQueryable<T> query) where T : class
        {
            string sql = query.ToString();
            var objQuery = query.GetValue<System.Data.Entity.Core.Objects.ObjectQuery>("InternalQuery.ObjectQuery");
            if (objQuery == null)
            {
                throw new Exception("Remove 方法只能用于数据上下文中！");
            }
            sql = "delete " + sql.Substring(sql.IndexOf("from", StringComparison.OrdinalIgnoreCase));
            sql = sql.Replace("[Extent1].", "").Replace("AS [Extent1]", "").Replace("__linq__", "");
            List<object> objs = new List<object>();
            foreach (var para in objQuery.Parameters)
            {
                objs.Add(para.Value);
            }
            int index = objQuery.Context.ExecuteStoreCommand(sql, objs.ToArray());
            return 0;
        }

        public static int Update<T>(this DbSet<T> dbSet, Expression<Func<T, bool>> predicate, Expression<Func<T>> updater) where T : class
        {
            return Update<T>(dbSet.Where(predicate), updater);
        }

        public static int Update<T>(this IQueryable<T> query, Expression<Func<T>> updater) where T : class
        {
            string sql = query.ToString();
            var objQuery = query.GetValue<System.Data.Entity.Core.Objects.ObjectQuery>("InternalQuery.ObjectQuery");
            if (objQuery == null)
            {
                throw new Exception("Update 方法只能用于数据上下文中！");
            }
            List<object> objParams = new List<object>();
            sql = sql.Substring(sql.IndexOf("from", StringComparison.OrdinalIgnoreCase)).Replace("__linq__", "");
            int paramindex = objQuery.Parameters.Count;
            foreach (var para in objQuery.Parameters)
            {
                objParams.Add(para.Value);
            }
            //获取Update的赋值语句
            var valueObj = updater.Compile().Invoke();
            MemberInitExpression updateMemberExpr = (MemberInitExpression)updater.Body;
            StringBuilder updateBuilder = new StringBuilder();
            Type valueType = typeof(T);
            foreach (var bind in updateMemberExpr.Bindings.Cast<MemberAssignment>())
            {
                string name = bind.Member.Name;
                updateBuilder.AppendFormat("{0}=@p{1},", name, paramindex++);
                var value = valueType.GetProperty(name).GetValue(valueObj);
                objParams.Add(value);
            }
            if (updateBuilder.Length == 0)
            {
                throw new Exception("请填写要更新的值");
            }
            else
            {
                sql = " update [Extent1] set " + updateBuilder.Remove(updateBuilder.Length - 1, 1).ToString() + " " + sql;
            }
            int index = objQuery.Context.ExecuteStoreCommand(sql, objParams.ToArray());
            return index;
        }

        public static Expression CreateKeySelector(Type modelType, string name)
        {
            var funcType = typeof(Func<,>).MakeGenericType(modelType, modelType.GetProperty(name).PropertyType);
            var paramExp = Expression.Parameter(modelType);
            var propExp = Expression.PropertyOrField(paramExp, name);
            return Expression.Lambda(funcType, propExp, paramExp);
        }

        public static Expression<Func<TModel, TKey>> CreateKeySelector<TModel, TKey>(string name)
        {
            var paramExp = Expression.Parameter(typeof(TModel));
            var propExp = Expression.PropertyOrField(paramExp, name);
            var convertedExp = Expression.Convert(propExp, typeof(TKey));
            var keySelectorExp = Expression.Lambda<Func<TModel, TKey>>(convertedExp, paramExp);
            return keySelectorExp;
        }
    }
}
