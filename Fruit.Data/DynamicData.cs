using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    /// <summary>
    /// 提供对动态对象和模型对象的互操作支持
    /// </summary>
    public static class DynamicData
    {
        public const string RowStateFieldName = "_row_state";

        /// <summary>
        /// 自动保存整个表格的改变
        /// </summary>
        /// <typeparam name="TContext">数据上下文类型</typeparam>
        /// <typeparam name="TModel">数据模型类型</typeparam>
        /// <param name="array">客户端回调数据</param>
        /// <param name="modelHandler">
        /// 可选的模型处理程序，如果此回调返回 True，不处理当前模型。
        /// </param>
        /// <returns></returns>
        public static int Save<TContext, TModel>(JArray array, Func<TContext,TModel,RowState,bool> modelHandler = null)
            where TContext : DbContext, new()
            where TModel: class, new()
        {
            if (array != null && array.Count > 0)
            {
                var prop = FindDbSetProperty(typeof(TContext), typeof(TModel));
                if (prop == null)
                {
                    throw new Exception("未能在数据上下文类中找到数据集为 DbSet<" + typeof(TModel).Name + "> 类型的属性！");
                }

                using (var db = new TContext())
                {
                    DbSet<TModel> dbSet = (DbSet<TModel>)prop.GetValue(db);
                    for (int i = 0; i < array.Count; i++)
                    {
                        var item = array[i] as JObject;
                        var newObject = item.ToObject<TModel>();

                        RowState rowState = (RowState)item.Value<int>(RowStateFieldName);
                        if (modelHandler != null && modelHandler(db, newObject, rowState))
                        {
                            continue;
                        }
                        switch (rowState)
                        {
                            case RowState.Changed:
                                ApplyChange(db, dbSet, newObject, item);
                                break;
                            case RowState.New:
                                dbSet.Add(newObject);
                                break;
                            case RowState.Deleted:
                                db.Entry(newObject).State = EntityState.Deleted;
                                break;
                        }
                    }

                    return db.SaveChanges();
                }
            }
            return 0;
        }

        private static void ApplyChange<TContext, TModel>(TContext db, DbSet<TModel> dbSet, object newObject, JObject item)
            where TContext : DbContext
            where TModel:class
        {
            if (item == null)
            {
                db.Entry(newObject).State = EntityState.Modified;
            }
            else
            {
                var oldObject = dbSet.Find(item.Value<String>("_id"));
                if (oldObject == null)
                {
                    throw new Exception("无法自动保存对象更新，Json 数据中缺少关键 _id 主键。");
                }
                foreach (KeyValuePair<string, JToken> kvp in item)
                {
                    var oldProp = oldObject.GetType().GetProperty(kvp.Key);
                    var newProp = newObject.GetType().GetProperty(kvp.Key);
                    if (oldProp != null && newProp != null && oldProp.PropertyType == newProp.PropertyType)
                    {
                        oldProp.SetValue(oldObject, newProp.GetValue(newObject));
                    }
                }
            }
        }

        private static Dictionary<Type, Dictionary<Type, PropertyInfo>> dbsetPropertyCache = new Dictionary<Type, Dictionary<Type, PropertyInfo>>();

        private static PropertyInfo FindDbSetProperty(Type dbContextType, Type modelType)
        {
            if(!dbsetPropertyCache.ContainsKey(dbContextType)) {
                dbsetPropertyCache[dbContextType] = new Dictionary<Type,PropertyInfo>();
            }
            if(!dbsetPropertyCache[dbContextType].ContainsKey(modelType)) {
                bool found = false;
                foreach (var prop in dbContextType.GetProperties())
                {
                    if (prop.PropertyType.Name.Equals("DbSet`1") && prop.PropertyType.GenericTypeArguments[0] == modelType)
                    {
                        dbsetPropertyCache[dbContextType][modelType] = prop;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    dbsetPropertyCache[dbContextType][modelType] = null;
                }
            }
            return dbsetPropertyCache[dbContextType][modelType];
        }

        public static void SaveCheckList<TContext, TMapModel, TModel>(JArray array, System.Linq.Expressions.Expression<Func<TMapModel, bool>> contion = null)
            where TContext : DbContext, new()
            where TMapModel : class, new()
            where TModel : class, new()
        {
            // TODO 未完成功能
            if (array != null && array.Count > 0)
            {
                var propMap = FindDbSetProperty(typeof(TContext), typeof(TMapModel));
                if (propMap == null)
                {
                    throw new Exception("未能在数据上下文类中找到数据集为 DbSet<" + typeof(TMapModel).Name + "> 类型的属性！");
                }
                using (var db = new TContext())
                {
                    DbSet<TMapModel> dbMapSet = (DbSet<TMapModel>)propMap.GetValue(db);

                    if (contion != null)
                    {
                        var dbSelectedList = dbMapSet.Where(contion).ToList();
                        foreach (JObject item in array)
                        {
                            if (item["Selected"].ToString() == "1" || item["Selected"].ToString() == "true")
                            {

                            }
                        }
                    }
                    else
                    {
                        
                        foreach (JObject item in array)
                        {
                            if (item[RowStateFieldName] == null)
                            {
                                // 没有跟踪信息！
                            }
                            else
                            {
                                if (item["Selected"].ToString() == "1" || item["Selected"].ToString() == "true")
                                {
                                    // 已选中项目
                                }
                                else
                                {
                                    // 未选中项目，从 MAP 模型中删除
                                    
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 按 JObject 中存在的键复制模型中的数据
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="source">源数据对象</param>
        /// <param name="target">目标数据对象</param>
        /// <param name="jfilter">JObject 复制键过滤</param>
        public static void Copy<TModel>(TModel source, TModel target, JObject jfilter)
        {
            Type modelType = typeof(TModel);
            foreach (var kp in jfilter)
            {
                var p = modelType.GetProperty(kp.Key);
                if (p != null)
                {
                    p.SetValue(target, p.GetValue(source));
                }
            }
        }
    }
}
