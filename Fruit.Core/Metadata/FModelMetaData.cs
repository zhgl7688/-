using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Metadata
{
    /// <summary>
    /// 提供简单的模型元数据
    /// </summary>
    public class FModelMetaData
    {
        /// <summary>
        /// 对应的模型类
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 模型可能的表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 模型可能的表架构名称
        /// </summary>
        public string TableSchema { get; set; }

        private FPropertyMetaDataCollection mProperties = null;

        /// <summary>
        /// 模型的属性集合
        /// </summary>
        public FPropertyMetaDataCollection Properties
        {
            get
            {
                if (mProperties == null)
                {
                    mProperties = new FPropertyMetaDataCollection(this);
                }
                return mProperties;
            }
        }

        public bool HasAttribute<TAttr>()
            where TAttr : Attribute
        {
            return Type.GetCustomAttributes(typeof(TAttr), true).Length > 0;
        }

        public TAttr GetAttribute<TAttr>()
            where TAttr : Attribute
        {
            return Type.GetCustomAttributes(typeof(TAttr), true).FirstOrDefault() as TAttr;
        }
    }
}
