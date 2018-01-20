using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    /// <summary>
    /// 表示一个存储过程参数
    /// </summary>
    public class SpParameter : TrackObject
    {
        public string ParameterName {
            get { return (string)Get("ParameterName"); }
            set { Set("ParameterName", value); }
        }
        public string ParameterType
        {
            get { return (string)Get("ParameterType"); }
            set { Set("ParameterType", value); }
        }
        public short? MaxLength
        {
            get { return (short?)Get("MaxLength"); }
            set { Set("MaxLength", value); }
        }

        public bool IsOutput
        {
            get { 
                var value = Get("IsOutput");
                if (value == null)
                    return false;
                return (bool)value;
            }
            set { Set("IsOutput", value); }
        }
    }
}
