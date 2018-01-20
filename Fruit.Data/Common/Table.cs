using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    /// <summary>
    /// 表示数据源中的一个表
    /// </summary>
    public class Table : TrackObject
    {
        private ITableMetadataProvider provider;

        internal Table(ITableMetadataProvider provider, string tableName)
        {
            this.provider = provider;
            this.TableName = tableName;

            this.SetUnchange();
        }

        public string TableName
        {
            get { return (string)Get("TableName"); }
            set { Set("TableName", value); }
        }

        private FieldCollection mFields = null;
        public FieldCollection Fields
        {
            get
            {
                if (mFields == null)
                {
                    mFields = new FieldCollection(provider, this);
                }
                return mFields;
            }
        }
    }
}
