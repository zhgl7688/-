using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Metadata
{

    /// <summary>
    /// 提供简单的模型元数据的解析与缓存
    /// </summary>
    public static class FModelMetaDataProvider
    {
        private static Dictionary<Type, FModelMetaData> modelMetaDatas = new Dictionary<Type,FModelMetaData>();

        /// <summary>
        /// 获取特定类型的模型元数据
        /// </summary>
        /// <typeparam name="Type">要获取模型元数据的类型</typeparam>
        /// <returns>模型元数据</returns>
        public static FModelMetaData GetMetaData<Type>()
        {
            return GetMetaData(typeof(Type));
        }

        /// <summary>
        /// 获取特定类型的模型元数据
        /// </summary>
        /// <param name="type">要获取模型元数据的类型</param>
        /// <returns>模型元数据</returns>
        public static FModelMetaData GetMetaData(this Type type)
        {
            if (modelMetaDatas.ContainsKey(type))
            {
                return modelMetaDatas[type];
            }
            return modelMetaDatas[type] = CreateMetaDataForType(type);
        }

        private static FModelMetaData CreateMetaDataForType(Type type)
        {
            FModelMetaData mmd = new FModelMetaData();
            mmd.Type = type;
            foreach (var pi in type.GetProperties())
            {
                CreateMetaDataForProperty(mmd, pi);
            }
            FinishModelMetaData(mmd);
            return mmd;
        }

        private static void FinishModelMetaData(FModelMetaData mmd)
        {
            
        }

        private static void CreateMetaDataForProperty(FModelMetaData mmd, PropertyInfo pi)
        {
            FPropertyMetaData p = new FPropertyMetaData(pi);
            p.Name = pi.Name;
            p.Type = pi.PropertyType;
            HandlePropertyAttributes(p);
            mmd.Properties.Add(p);
        }

        private static void HandlePropertyAttributes(FPropertyMetaData p)
        {
            // 可能的字段名处理
            var columnAttr = p.PropertyInfo.GetCustomAttribute<ColumnAttribute>();
            if (columnAttr != null)
            {
                p.ColumnName = columnAttr.Name;
                p.ColumnOrder = columnAttr.Order;
            }
            else
            {
                p.ColumnName = p.Name;
            }
            // 主键
            var keyAttr = p.PropertyInfo.GetCustomAttribute<KeyAttribute>();
            if (keyAttr != null)
            {
                p.IsKey = true;
            }
        }

        private static void HandleTypeAttributes(FModelMetaData mmd)
        {
            // 可能的表名处理
            var tableAttr = (TableAttribute)mmd.Type.GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();
            if (tableAttr != null)
            {
                mmd.TableName = tableAttr.Name;
                mmd.TableSchema = tableAttr.Schema;
            }
            else
            {
                mmd.TableName = mmd.Type.Name;
            }
        }
    }
}
