using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Metadata
{
    public class FPropertyMetaData
    {

        public FPropertyMetaData(System.Reflection.PropertyInfo pi)
        {
            this.PropertyInfo = pi;
        }

        public System.Reflection.PropertyInfo PropertyInfo { get; private set; }

        public string Name { get; set; }

        public Type Type { get; set; }

        public string ColumnName { get; set; }

        public int ColumnOrder { get; set; }

        public bool IsKey { get; set; }

        public bool CanWrite
        {
            get { return PropertyInfo.CanWrite; }
        }


        public bool HasAttribute<TAttr>()
            where TAttr : Attribute
        {
            return PropertyInfo.GetCustomAttributes(typeof(TAttr), true).Length > 0;
        }

        public TAttr GetAttribute<TAttr>()
            where TAttr : Attribute
        {
            return PropertyInfo.GetCustomAttributes(typeof(TAttr), true).FirstOrDefault() as TAttr;
        }
    }
}
