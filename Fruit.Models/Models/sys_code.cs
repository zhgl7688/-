using System;
using System.Collections.Generic;

namespace Fruit.Models
{
    public partial class sys_code
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public string ParentCode { get; set; }
        public string Seq { get; set; }
        public Nullable<bool> IsEnable { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public string Description { get; set; }
        public string CodeTypeName { get; set; }
        public string CodeType { get; set; }
        public string CreatePerson { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdatePerson { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual sys_codeType sys_codeType { get; set; }
    }
}
