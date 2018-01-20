using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    /// <summary>
    /// 表示数据源中的一个表类型
    /// </summary>
    public class TableType : TrackObject
    {
        private ITableMetadataProvider provider;

        internal TableType(ITableMetadataProvider provider, string typeName)
        {
            this.provider = provider;
            this.TypeName = typeName;

            this.SetUnchange();
        }

        public string TypeName
        {
            get { return (string)Get("TypeName"); }
            set { Set("TypeName", value); }
        }

        private TableTypeFieldCollection mFields = null;
        public TableTypeFieldCollection Fields
        {
            get
            {
                if (mFields == null)
                {
                    mFields = new TableTypeFieldCollection(provider, this);
                }
                return mFields;
            }
        }
    }
}
