using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Models
{
    public class sys_file
    {
        [Key, Column(Order = 0)]
        public int FileId { get; set; }

        [Key, Column(Order = 1)]
        public int Serial { get; set; }

        public string FileName { get; set; }

        public string Path { get; set; }

        public string CreatePerson { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
