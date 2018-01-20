using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    /// <summary>
    /// 表示数据源中的一个存储过程
    /// </summary>
    public class StoredProcedure : TrackObject
    {
        private ITableMetadataProvider provider;

        internal StoredProcedure(ITableMetadataProvider provider, string name)
        {
            this.provider = provider;
            this.Name = name;

            this.SetUnchange();
        }

        public string Name
        {
            get { return (string)Get("Name"); }
            set { Set("Name", value); }
        }

        private SpParameterCollection parameters;
        public SpParameterCollection Parameters
        {
            get
            {
                if (parameters == null)
                {
                    parameters = new SpParameterCollection(provider, this);
                }
                return parameters;
            }
        }
    }
}
