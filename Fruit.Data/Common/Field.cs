using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    public class Field : TrackObject
    {
        public string Name
        {
            get { return (string)Get("Name"); }
            set { Set("Name", value); }
        }

        public string FieldType
        {
            get { return (string)Get("FieldType"); }
            set { Set("FieldType", value); }
        }

        public bool Nullable
        {
            get { return (bool)Get("Nullable"); }
            set { Set("Nullable", value); }
        }

        public short Max_Length
        {
            get { return (short)Get("Max_Length"); }
            set { Set("Max_Length", value); }
        }

        public byte Scale
        {
            get { return (byte)Get("Scale"); }
            set { Set("Scale", value); }
        }
    }
}
