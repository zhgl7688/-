using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Models
{
    public partial class sys_serial
    {
        [Key]
        public string Table { get; set; }

        public int Serial { get; set; }

        public int? Cycle { get; set; }
    }
}
