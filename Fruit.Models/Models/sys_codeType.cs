using System;
using System.Collections.Generic;

namespace Fruit.Models
{
    public partial class sys_codeType
    {
        public sys_codeType()
        {
            this.sys_code = new List<sys_code>();
        }

        public string CodeType { get; set; }
        public string CodeTypeName { get; set; }
        public string Description { get; set; }
        public string Seq { get; set; }
        public string CreatePerson { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<sys_code> sys_code { get; set; }
    }
}
