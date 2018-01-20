using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Models
{
    public class sys_searchScheme
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(200)]
        public string PageCode { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(20)]
        public string CompCode { get; set; }

        [StringLength(100)]
        public string UserCode { get; set; }

        public string Data { get; set; }
    }
}
