using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Models
{
    public class sys_session
    {
        [Key]
        public string SessionName { get; set; }

        public string Condition { get; set; }

        public string DefaultValue { get; set; }

        public string Connection { get; set; }

        public string T_SQL { get; set; }

        public int Sort { get; set; }
    }
}
